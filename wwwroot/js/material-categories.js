// Configurare categorii și subcategorii materiale
// MODIFICĂ ACEST FIȘIER PENTRU A ADĂUGA/EDITA CATEGORII

const MATERIAL_CATEGORIES = {
    "Feronerie": {
        subcategorii: [
            "Balamale",
            "Sine glisare",
            "Sisteme de ridicare",
            "Mânere și butoane",
            "Șuruburi și piulițe",
            "Plăci de prindere",
            "Amortizoare",
            "Suporți",
            "Altele"
        ],
        unitatiImplicite: "buc"
    },
    "MDF": {
        subcategorii: [
            "MDF brut",
            "MDF melaminat alb",
            "MDF melaminat negru",
            "MDF melaminat nuc",
            "MDF melaminat stejar",
            "MDF vopsit",
            "MDF hidrofug",
            "Altele"
        ],
        unitatiImplicite: "mp"
    },
    "PAL": {
        subcategorii: [
            "PAL brut",
            "PAL melaminat alb",
            "PAL melaminat negru",
            "PAL melaminat nuc",
            "PAL melaminat stejar",
            "PAL melaminat gri",
            "Cant ABS",
            "Cant PVC",
            "Altele"
        ],
        unitatiImplicite: "mp"
    },
    "Lemn Masiv": {
        subcategorii: [
            "Stejar",
            "Fag",
            "Pin",
            "Brad",
            "Nuc",
            "Cireș",
            "Frasin",
            "Mahon",
            "Altele"
        ],
        unitatiImplicite: "mp"
    },
    "Plăci": {
        subcategorii: [
            "Placaj",
            "OSB",
            "HDF",
            "Plăci stratificate",
            "Plexiglas",
            "Sticlă",
            "Altele"
        ],
        unitatiImplicite: "mp"
    },
    "Finisaje": {
        subcategorii: [
            "Lac transparent",
            "Lac colorat",
            "Vopsea albă",
            "Vopsea colorată",
            "Grund",
            "Ceară",
            "Ulei",
            "Perie/Pensulă",
            "Șmirghel",
            "Altele"
        ],
        unitatiImplicite: "litru"
    },
    "Consumabile": {
        subcategorii: [
            "Adeziv",
            "Silicon",
            "Cleiul",
            "Șuruburi diverse",
            "Cuie",
            "Discuri/Pânze",
            "Echipament protecție",
            "Altele"
        ],
        unitatiImplicite: "buc"
    }
};

// Unități de măsură disponibile
const UNITATI_MASURA = [
    { value: "buc", label: "Bucăți (buc)" },
    { value: "mp", label: "Metri pătrați (mp)" },
    { value: "ml", label: "Metri liniari (ml)" },
    { value: "kg", label: "Kilograme (kg)" },
    { value: "litru", label: "Litri" },
    { value: "set", label: "Set" },
    { value: "cutie", label: "Cutie" },
    { value: "pachet", label: "Pachet" }
];

// Funcție helper pentru a obține categoriile
function getMaterialCategories() {
    return Object.keys(MATERIAL_CATEGORIES);
}

// Funcție helper pentru a obține subcategoriile unei categorii
function getSubcategories(categorie) {
    return MATERIAL_CATEGORIES[categorie]?.subcategorii || [];
}

// Funcție helper pentru a obține unitatea implicită
function getDefaultUnit(categorie) {
    return MATERIAL_CATEGORIES[categorie]?.unitatiImplicite || "buc";
}

// Export pentru utilizare în alte module
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        MATERIAL_CATEGORIES,
        UNITATI_MASURA,
        getMaterialCategories,
        getSubcategories,
        getDefaultUnit
    };
}