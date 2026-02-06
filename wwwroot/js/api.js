// Configurare API
const API_BASE_URL = 'http://localhost:5000/api';

// Funcții helper pentru comunicare cu API
const API = {
    // Clienți
    async getClienti() {
        return await this.request('GET', '/Clienti');
    },
    
    async getClient(id) {
        return await this.request('GET', `/Clienti/${id}`);
    },
    
    async createClient(data) {
        return await this.request('POST', '/Clienti', data);
    },
    
    async updateClient(id, data) {
        return await this.request('PUT', `/Clienti/${id}`, data);
    },
    
    async deleteClient(id) {
        return await this.request('DELETE', `/Clienti/${id}`);
    },
    
    // Proiecte
    async getProiecte() {
        return await this.request('GET', '/Proiecte');
    },
    
    async getProiect(id) {
        return await this.request('GET', `/Proiecte/${id}`);
    },
    
    async getProiecteByStatus(status) {
        return await this.request('GET', `/Proiecte/Status/${status}`);
    },
    
    async getProiecteStatistici() {
        return await this.request('GET', '/Proiecte/Statistici');
    },
    
    async createProiect(data) {
        return await this.request('POST', '/Proiecte', data);
    },
    
    async updateProiect(id, data) {
        return await this.request('PUT', `/Proiecte/${id}`, data);
    },
    
    async deleteProiect(id) {
        return await this.request('DELETE', `/Proiecte/${id}`);
    },
    
    // Cheltuieli
    async getCheltuieli() {
        return await this.request('GET', '/Cheltuieli');
    },
    
    async getCheltuiala(id) {
        return await this.request('GET', `/Cheltuieli/${id}`);
    },
    
    async getCheltuieliByProiect(proiectId) {
        return await this.request('GET', `/Cheltuieli/Proiect/${proiectId}`);
    },
    
    async getCheltuieliByTip(tip) {
        return await this.request('GET', `/Cheltuieli/Tip/${tip}`);
    },
    
    async createCheltuiala(data) {
        return await this.request('POST', '/Cheltuieli', data);
    },
    
    async updateCheltuiala(id, data) {
        return await this.request('PUT', `/Cheltuieli/${id}`, data);
    },
    
    async deleteCheltuiala(id) {
        return await this.request('DELETE', `/Cheltuieli/${id}`);
    },
    
    // Stoc
    async getStoc() {
        return await this.request('GET', '/Stoc');
    },
    
    async getStocItem(id) {
        return await this.request('GET', `/Stoc/${id}`);
    },
    
    async getStocByCategorie(categorie) {
        return await this.request('GET', `/Stoc/Categorie/${categorie}`);
    },
    
    async getStocValoare() {
        return await this.request('GET', '/Stoc/Valoare');
    },
    
    async getStocScazut() {
        return await this.request('GET', '/Stoc/StocScazut');
    },
    
    async createStocItem(data) {
        return await this.request('POST', '/Stoc', data);
    },
    
    async updateStocItem(id, data) {
        return await this.request('PUT', `/Stoc/${id}`, data);
    },
    
    async deleteStocItem(id) {
        return await this.request('DELETE', `/Stoc/${id}`);
    },
    
    // Mișcări Stoc
    async getMiscariStoc() {
        return await this.request('GET', '/MiscariStoc');
    },
    
    async getMiscareStoc(id) {
        return await this.request('GET', `/MiscariStoc/${id}`);
    },
    
    async getMiscariByProdus(stocId) {
        return await this.request('GET', `/MiscariStoc/Produs/${stocId}`);
    },
    
    async createMiscareStoc(data) {
        return await this.request('POST', '/MiscariStoc', data);
    },
    
    async deleteMiscareStoc(id) {
        return await this.request('DELETE', `/MiscariStoc/${id}`);
    },
    
    // Calcul Cant - NOU
    async createCalculCant(data) {
        return await this.request('POST', '/CalculCant', data);
    },
    
    async getCalculCant(id) {
        return await this.request('GET', `/CalculCant/${id}`);
    },
    
    async getCalculeByProiect(proiectId) {
        return await this.request('GET', `/CalculCant/Proiect/${proiectId}`);
    },
    
    async deleteCalculCant(id) {
        return await this.request('DELETE', `/CalculCant/${id}`);
    },
    
    // Funcție generică pentru cereri HTTP
    async request(method, endpoint, data = null) {
        const options = {
            method: method,
            headers: {
                'Content-Type': 'application/json',
            }
        };
        
        if (data && (method === 'POST' || method === 'PUT')) {
            options.body = JSON.stringify(data);
        }
        
        try {
            showLoading();
            const response = await fetch(`${API_BASE_URL}${endpoint}`, options);
            
            // Pentru 204 No Content (de obicei nu mai este folosit, dar păstrăm pentru siguranță)
            if (response.status === 204) {
                hideLoading();
                return { success: true };
            }
            
            // Verificăm dacă răspunsul are conținut
            const contentType = response.headers.get("content-type");
            let result;
            
            if (contentType && contentType.indexOf("application/json") !== -1) {
                result = await response.json();
            } else {
                // Dacă nu e JSON, returnăm success pentru status-uri OK
                if (response.ok) {
                    hideLoading();
                    return { success: true };
                }
                throw new Error('Eroare la comunicarea cu serverul');
            }
            
            if (!response.ok) {
                throw new Error(result.message || 'Eroare la comunicarea cu serverul');
            }
            
            hideLoading();
            return result;
        } catch (error) {
            hideLoading();
            console.error('API Error:', error);
            showError(error.message);
            throw error;
        }
    }
};

// Funcții helper pentru UI
function showLoading() {
    let loader = document.getElementById('loading-overlay');
    if (!loader) {
        loader = document.createElement('div');
        loader.id = 'loading-overlay';
        loader.className = 'loading-overlay';
        loader.innerHTML = '<div class="loading"></div>';
        document.body.appendChild(loader);
    }
    loader.style.display = 'flex';
}

function hideLoading() {
    const loader = document.getElementById('loading-overlay');
    if (loader) {
        loader.style.display = 'none';
    }
}

function showSuccess(message) {
    showAlert(message, 'success');
}

function showError(message) {
    showAlert(message, 'error');
}

function showInfo(message) {
    showAlert(message, 'info');
}

function showAlert(message, type = 'info') {
    const alertDiv = document.createElement('div');
    alertDiv.className = `alert alert-${type}`;
    alertDiv.textContent = message;
    
    const container = document.querySelector('.container');
    if (container) {
        container.insertBefore(alertDiv, container.firstChild);
        
        setTimeout(() => {
            alertDiv.style.animation = 'fadeOut 0.3s';
            setTimeout(() => alertDiv.remove(), 300);
        }, 3000);
    }
}

// Funcții helper pentru formatare
function formatDate(dateString) {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('ro-RO');
}

function formatCurrency(amount) {
    if (amount === null || amount === undefined) return '-';
    return new Intl.NumberFormat('ro-RO', {
        style: 'currency',
        currency: 'RON'
    }).format(amount);
}

function formatNumber(number, decimals = 2) {
    if (number === null || number === undefined) return '-';
    return Number(number).toFixed(decimals);
}

// Funcție pentru a obține badge-ul de status
function getStatusBadge(status) {
    const badges = {
        'planificat': '<span class="badge badge-info">Planificat</span>',
        'in_lucru': '<span class="badge badge-warning">În lucru</span>',
        'finalizat': '<span class="badge badge-success">Finalizat</span>',
        'anulat': '<span class="badge badge-danger">Anulat</span>'
    };
    return badges[status] || `<span class="badge">${status}</span>`;
}

// Funcție pentru confirmare ștergere
function confirmDelete(itemName) {
    return confirm(`Sigur vrei să ștergi "${itemName}"?\n\nAceastă acțiune nu poate fi anulată.`);
}

// Export pentru utilizare în alte module
if (typeof module !== 'undefined' && module.exports) {
    module.exports = API;
}