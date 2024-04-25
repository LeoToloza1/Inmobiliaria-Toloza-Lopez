Date.prototype.addYears = function (years) {
    let date = new Date(this.valueOf());
    date.setFullYear(date.getFullYear() + years);
    return date;
};
document.addEventListener('DOMContentLoaded', function () {
    // elementos seccion modal inquilino
    const modalInquilino = document.getElementById('modalInquilino');
    const formInquilino = document.getElementById('formInquilino');
    const btnSend = document.getElementById('btnSend');
    const btnCancel = document.getElementById('btnCancel');
    const urlPostInquilino = "/inquilino/PostCreate";

    // elemntos seccion contrato
    const fechaInicio = document.getElementById('fechaInicio');
    const fechaFin = document.getElementById('fechaFin');
    const inputValor = document.getElementById('valueInquilino');
    const selectInquilino = document.getElementById('idInquilino');
    const contratoEtapa2 = document.getElementById('contratoEtapa2');
    let timeoutId = null;
    const urlGetInquilinos = "/inquilino/findinquilinos?value=";

    // SET FECHAS CONTRATO 
    fechaInicio.value = new Date().toISOString().split('T')[0];
    fechaFin.value = new Date().addYears(1).toISOString().split('T')[0];

    // listener botones modal inquilino
    btnSend.addEventListener('click', (event) => {
        event.preventDefault();
        event.stopPropagation();
        const formInputs = Array.from(formInquilino.querySelectorAll('input'));
        const formData = {};
        formInputs.forEach((input) => {
            formData[input.name] = input.value;
        });
        console.log("Datos a enviar:", formData);
        registroInquilino(formData);
    });

    btnCancel.addEventListener('click', (event) => {
        event.preventDefault();
        event.stopPropagation();
        modalInquilino.classList.add('d-none');
    });

    // Listener valores inquilino a buscar,recursivo para cada char
    inputValor.focus();
    inputValor.addEventListener('input', () => {
        selectInquilino.innerHTML = '';
        const valor = inputValor.value.trim();
        if (timeoutId) {
            clearTimeout(timeoutId);
        }
        if (valor.length >= 3) {
            findInquilinos(valor);
        } else {
            selectInquilino.classList.add("d-none");
            contratoEtapa2.classList.add("d-none");
        }
    });

 const    findInquilinos=(valor) =>{
        fetch(urlGetInquilinos + encodeURIComponent(valor))
            .then(response => {
                if (!response.ok) {
                    selectInquilino.innerHTML = '';
                    throw new Error('Error al obtener los datos.');
                }                
                try {
                   return  response.json();
                }
                catch {
                    console.log('error');
                }
                return false;
            })
            .then(data => {
                if (data.length === 0) {
                    modalInquilino.classList.remove('d-none');                     
                }else{
                    data.forEach(item => {
                        const option = document.createElement('option');
                        option.value = item.id;
                        option.text = `${item.apellido}, ${item.nombre}. DNI: ${item.dni} Teléfono: ${item.telefono}`;
                        selectInquilino.appendChild(option);
                    });
                    toggleDisplay(data);
                }
            })
            .catch(error => {
                selectInquilino.innerHTML = '';
                const option = document.createElement('option');
                option.value = '';
                option.text = "EL servidor respondio conun error contacte al servicio";
                alerta('Inquilino', "EL servidor respondio conun error contacte al servicio")
                selectInquilino.appendChild(option);
                console.error('Error:', error);
                selectInquilino.classList.add("d-none");
            });
    }
const registroInquilino = (formData) => {
    fetch(urlPostInquilino, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(formData)        
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Error al crear el inquilino.');
            }
            return response.json();
        })
        .then(data => {
            if(data > 0){             
                    const option = document.createElement('option');
                    option.value = data;
                    option.text = `${formData.apellido}, ${formData.nombre}. DNI: ${formData.dni} Teléfono: ${formData.telefono}`;
                    console.log('Inquilino creado:', formData, data);
                    selectInquilino.appendChild(option);  
                    selectInquilino.classList.remove("d-none");
                    contratoEtapa2.classList.remove("d-none");   
                    modalInquilino.classList.add('d-none');           

            console.log('Inquilino creado:', formData, data);
            alerta('Inquilino', 'Inquilino creado con exito')
            modalInquilino.classList.add('d-none');
            }else{
                console.log('Inquilino NO creado:', formData, data);
            }
        })
}
    function toggleDisplay(data) {
        //const selectInquilino = document.getElementById('idInquilino');
        if (data.length > 0) {
            selectInquilino.classList.remove("d-none");
            contratoEtapa2.classList.remove("d-none");
            //   selectInquilino.style.display = 'block';
        } else {
            selectInquilino.classList.add("d-none");
            contratoEtapa2.classList.add("d-none");
        }
    }
});