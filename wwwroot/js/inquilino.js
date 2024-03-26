/** Verificacion formulario **/

const inputs = document.getElementsByTagName("input");
Array.from(inputs).map(element => {
    // veriifica ne cada input
    element.addEventListener("input", function (event) {
        console.log("valor cambiado:", event.target.getAttribute("verfy"));
        const inputValue = event.target.value;
        // Aquí puedes realizar la verificación para cada letra ingresada
        if (inputValue.length > 0) {
            const lastChar = inputValue[inputValue.length - 1];
            console.log("lera ingresada:", lastChar);
            // Realizar acciones según la última letra ingresada
        }
    });

    // verifica una ves cambiado a otro input
    element.addEventListener("change", function (event) {
        console.log(" valor final :", event.target.value);
        
        console.log(event.target.id);
    });
});



/**
 * 0

input#nombre-inquilino.form-control
1
input#apellido-inquilino.form-control
2
input#dni-inquilino.form-control
3
input#email-inquilino.form-control
apellido
input#apellido-inquilino.form-control
apellido-inquilino
input#apellido-inquilino.form-control
dni
input#dni-inquilino.form-control
dni-inquilino
input#dni-inquilino.form-control
email
input#email-inquilino.form-control
email-inquilino
input#email-inquilino.form-control
nombre
input#nombre-inquilino.form-control
nombre-inquilino
input#nombre-inquilino.form-control
 */