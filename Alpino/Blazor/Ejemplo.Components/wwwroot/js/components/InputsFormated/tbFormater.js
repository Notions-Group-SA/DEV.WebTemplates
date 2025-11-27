//class CuitFormatter {
//    constructor() {
//        this.maxLength = 13; // Longitud máxima con guiones: "XX-XXXXXXXX-X"
//    }

//    attach(input) {
//        if (!input) return;

//        input.addEventListener('input', (e) => {
//            let raw = e.target.value.replace(/\D/g, ''); // Eliminar todo lo que no sea dígito

//            if (raw.length > 11) raw = raw.slice(0, 11); // Limitar a 11 números

//            let formatted = '';

//            if (raw.length <= 2) {
//                formatted = raw;
//            } else if (raw.length <= 10) {
//                formatted = `${raw.slice(0, 2)}-${raw.slice(2)}`;
//            } else {
//                formatted = `${raw.slice(0, 2)}-${raw.slice(2, 10)}-${raw.slice(10)}`;
//            }

//            e.target.value = formatted;
//        });

//        input.setAttribute('maxlength', this.maxLength);
//    }
//}

//window.cuitFormatter = new CuitFormatter();

class CuitFormatter {
    constructor() {
        this.maxLength = 13; // Longitud máxima con guiones: "XX-XXXXXXXX-X"
    }

    attach(input) {
        if (!input) return;

        input.addEventListener('input', (e) => {
            let raw = e.target.value.replace(/\D/g, ''); // Quitar todo lo que no sea dígito

            if (raw.length > 11) raw = raw.slice(0, 11); // Limitar a 11 dígitos

            let formatted = '';

            if (raw.length <= 2) {
                formatted = raw;
            } else if (raw.length <= 9) {
                // Hasta 7 en el medio → XX-XXXXXXX
                formatted = `${raw.slice(0, 2)}-${raw.slice(2)}`;
            } else {
                // 10 u 11 dígitos → XX-XXXXXXX/X-... o XX-XXXXXXXX-X
                formatted = `${raw.slice(0, 2)}-${raw.slice(2, raw.length - 1)}-${raw.slice(-1)}`;
            }

            e.target.value = formatted;
        });

        input.setAttribute('maxlength', this.maxLength);
    }
}

window.cuitFormatter = new CuitFormatter();

window.initCuitInputs = () => {
    document.querySelectorAll('.cuit').forEach(input => {
        window.cuitFormatter.attach(input);
    });
};



class DniFormatter {
    constructor() {
        this.maxLength = 10; // 8 números + 2 puntos
    }

    attach(input) {
        if (!input) return;

        input.addEventListener('input', (e) => {
            let raw = e.target.value.replace(/\D/g, ''); // Solo números

            if (raw.length > 8) raw = raw.slice(0, 8); // Máximo 8 dígitos

            let formatted = '';
            if (raw.length <= 3) {
                formatted = raw;
            } else if (raw.length <= 6) {
                formatted = `${raw.slice(0, raw.length - 3)}.${raw.slice(-3)}`;
            } else {
                formatted = `${raw.slice(0, raw.length - 6)}.${raw.slice(-6, -3)}.${raw.slice(-3)}`;
            }

            e.target.value = formatted;
        });

        input.setAttribute('maxlength', this.maxLength);
    }
}

window.dniFormatter = new DniFormatter();

window.initDniInputs = () => {
    document.querySelectorAll('.dni').forEach(input => {
        window.dniFormatter.attach(input);
    });
};


class MoneyFormatter {
    attach(input) {
        if (!input) return;

        input.addEventListener('input', (e) => {
            let raw = e.target.value.replace(/\D/g, ''); // Solo dígitos

            if (!raw) {
                e.target.value = '';
                return;
            }

            // Formatear con separador de miles (puntos)
            let formatted = raw.replace(/\B(?=(\d{3})+(?!\d))/g, ".");
            e.target.value = formatted;
        });

        // No se aplica maxlength aquí, porque puede ser gigante
    }
}

window.moneyFormatter = new MoneyFormatter();

window.initMoneyInputs = () => {
    document.querySelectorAll('.money').forEach(input => {
        window.moneyFormatter.attach(input);
    });
};