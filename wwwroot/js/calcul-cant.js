// Procesare Excel și calcul cant
// Folosește biblioteca SheetJS (xlsx.full.min.js)

const PIERDERE_PER_LATURA_CM = 0.4; // 4mm = 0.4cm

// Procesează fișierul Excel
async function procesareExcelCant(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        
        reader.onload = function(e) {
            try {
                const data = new Uint8Array(e.target.result);
                const workbook = XLSX.read(data, { type: 'array' });
                
                // Presupunem că datele sunt în prima foaie
                const firstSheet = workbook.Sheets[workbook.SheetNames[0]];
                const jsonData = XLSX.utils.sheet_to_json(firstSheet, { header: 1 });
                
                // Validare și procesare
                const linii = parseLiniiExcel(jsonData);
                
                if (linii.length === 0) {
                    reject(new Error('Fișierul nu conține date valide'));
                    return;
                }
                
                resolve(linii);
            } catch (error) {
                reject(new Error('Eroare la procesarea fișierului Excel: ' + error.message));
            }
        };
        
        reader.onerror = function() {
            reject(new Error('Eroare la citirea fișierului'));
        };
        
        reader.readAsArrayBuffer(file);
    });
}

// Parsează liniile din Excel
function parseLiniiExcel(jsonData) {
    const linii = [];
    
    // Sare peste header (prima linie)
    for (let i = 1; i < jsonData.length; i++) {
        const row = jsonData[i];
        
        // Verifică dacă linia are date
        if (!row || row.length < 4) continue;
        
        const lungime = parseFloat(row[0]);
        const latime = parseFloat(row[1]);
        const cantLungime = parseInt(row[2]) || 0;
        const cantLatime = parseInt(row[3]) || 0;
        
        // Validare
        if (isNaN(lungime) || isNaN(latime)) continue;
        if (lungime <= 0 || latime <= 0) continue;
        if (cantLungime < 0 || cantLungime > 2) continue;
        if (cantLatime < 0 || cantLatime > 2) continue;
        
        linii.push({
            lungime: lungime,
            latime: latime,
            cantLungime: cantLungime,
            cantLatime: cantLatime
        });
    }
    
    return linii;
}

// Calculează cant local (pentru previzualizare)
function calculeazaCantLocal(linii) {
    let totalCm = 0;
    const detalii = [];
    
    linii.forEach(linie => {
        const lungimeMm = linie.lungime;
        const latimeMm = linie.latime;
        
        // Calcul cant pe lungime (mm → cm + pierdere)
        let lungimeCantata = 0;
        if (linie.cantLungime > 0) {
            lungimeCantata = (lungimeMm / 10) * linie.cantLungime + 
                            (PIERDERE_PER_LATURA_CM * linie.cantLungime);
        }
        
        // Calcul cant pe lățime
        let latimeCantata = 0;
        if (linie.cantLatime > 0) {
            latimeCantata = (latimeMm / 10) * linie.cantLatime + 
                           (PIERDERE_PER_LATURA_CM * linie.cantLatime);
        }
        
        const totalLinie = lungimeCantata + latimeCantata;
        totalCm += totalLinie;
        
        detalii.push({
            lungime: lungimeMm,
            latime: latimeMm,
            cantLungime: linie.cantLungime,
            cantLatime: linie.cantLatime,
            lungimeCantata: lungimeCantata.toFixed(2),
            latimeCantata: latimeCantata.toFixed(2),
            totalLinie: totalLinie.toFixed(2)
        });
    });
    
    return {
        totalCantCm: totalCm.toFixed(2),
        totalCantMetri: (totalCm / 100).toFixed(2),
        detalii: detalii
    };
}

// Afișează rezultatul calculului
function afiseazaRezultatCant(rezultat, proiectId) {
    const resultDiv = document.getElementById('calcul-result');
    const totalCantDiv = document.getElementById('total-cant');
    const detailsTbody = document.getElementById('calcul-details');
    
    // Afișează totalul
    totalCantDiv.innerHTML = `
        <div style="font-size: 2em; font-weight: bold; color: var(--success-color);">
            ${rezultat.totalCantMetri} ml
        </div>
        <div style="font-size: 1.2em; color: #666;">
            (${rezultat.totalCantCm} cm)
        </div>
    `;
    
    // Afișează detaliile
    detailsTbody.innerHTML = rezultat.detalii.map((d, index) => `
        <tr>
            <td>${d.lungime}</td>
            <td>${d.latime}</td>
            <td>${d.cantLungime}</td>
            <td>${d.cantLatime}</td>
            <td>
                ${d.lungimeCantata > 0 ? `L: ${d.lungimeCantata} cm` : ''}
                ${d.lungimeCantata > 0 && d.latimeCantata > 0 ? '<br>' : ''}
                ${d.latimeCantata > 0 ? `l: ${d.latimeCantata} cm` : ''}
                ${d.lungimeCantata == 0 && d.latimeCantata == 0 ? '-' : ''}
            </td>
            <td><strong>${d.totalLinie} cm</strong></td>
        </tr>
    `).join('');
    
    // Adaugă linie totală
    detailsTbody.innerHTML += `
        <tr style="background: #f0f0f0; font-weight: bold;">
            <td colspan="5" style="text-align: right;">TOTAL:</td>
            <td><strong>${rezultat.totalCantCm} cm (${rezultat.totalCantMetri} ml)</strong></td>
        </tr>
    `;
    
    // Afișează secțiunea
    resultDiv.style.display = 'block';
}

// Descarcă template Excel
function descarcaTemplateExcel() {
    // Creează un workbook nou
    const wb = XLSX.utils.book_new();
    
    // Creează date pentru template
    const data = [
        ['Lungime (mm)', 'Lățime (mm)', 'Cant Lungime', 'Cant Lățime'],
        ['Exemplu:', '', '', ''],
        [750, 200, 1, 2],
        [1200, 400, 2, 1],
        [800, 300, 0, 1],
        [600, 250, 1, 0],
        ['', '', '', ''],
        ['Instrucțiuni:', '', '', ''],
        ['- Lungime și Lățime în milimetri', '', '', ''],
        ['- Cant Lungime: 0 (fără), 1 (o latură), 2 (două laturi)', '', '', ''],
        ['- Cant Lățime: 0 (fără), 1 (o latură), 2 (două laturi)', '', '', ''],
        ['- Pierdere tehnologică: 4mm per latură cantată', '', '', '']
    ];
    
    // Creează sheet
    const ws = XLSX.utils.aoa_to_sheet(data);
    
    // Adaugă sheet la workbook
    XLSX.utils.book_append_sheet(wb, ws, 'Template Cant');
    
    // Descarcă fișierul
    XLSX.writeFile(wb, 'template_calcul_cant.xlsx');
}

// Export funcții
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        procesareExcelCant,
        calculeazaCantLocal,
        afiseazaRezultatCant,
        descarcaTemplateExcel
    };
}