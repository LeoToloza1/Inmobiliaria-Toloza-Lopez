document.addEventListener('DOMContentLoaded', function () {
    const fechaInicio = document.getElementById('fechaInicio');
    const fechaActual = new Date().toISOString().split('T')[0];
    const inputValor = document.getElementById('valueInquilino');
    const selectInquilino = document.getElementById('idInquilino');
    const contratoEtapa2 = document.getElementById('contratoEtapa2');
    let timeoutId = null;
    const urlInquilinos = "/inquilino/findinquilinos?value=";

    fechaInicio.value = fechaActual;
    inputValor.focus();
    inputValor.addEventListener('input', () => {
        selectInquilino.innerHTML = '';
        const valor = inputValor.value.trim();
        console.log(valor);
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


    function findInquilinos(valor) {
        fetch(urlInquilinos + encodeURIComponent(valor))
            .then(response => {
                if (!response.ok) {
                    selectInquilino.innerHTML = '';
                    throw new Error('Error al obtener los datos.');
                }
                let resp = false;
                try {
                    //   selectInquilino.innerHTML = '';
                    resp = response.json();
                }
                catch {
                    console.log('error');
                }
                return resp;
            })
            .then(data => {
                if (data.length === 0) {
                    alerta('Inquilino', "No se econtro inquilino debe crearlo antes")
                    //   inputValor.value="";
                }
                data.forEach(item => {
                    const option = document.createElement('option');
                    option.value = item.id;
                    option.text = `${item.apellido}, ${item.nombre}. DNI: ${item.dni} Teléfono: ${item.telefono}`;
                    selectInquilino.appendChild(option);
                });
                toggleDisplay(data);
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
    function alerta(titulo, msg) {
        Swal.fire({
            title: `<h1>${titulo}</h1>`,
            icon: 'question',
            html: ` <b>${msg}</b>`,
            showConfirmButton: false,
            //     showCloseButton: true,
            showCancelButton: true,
            focusCancel: true,
            cancelButtonText: `<i class="fas fa-thumbs-down"></i> Cerrar`,
            cancelButtonAriaLabel: "Thumbs down"
        })
    }
});