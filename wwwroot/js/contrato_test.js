document.addEventListener('DOMContentLoaded', function () {
  // elementos seccion modal inquilino
  const modalInquilino = document.getElementById('modalInquilino');
  const formInquilino = document.getElementById('formInquilino');
  const btnSend = document.getElementById('btnSend');
  const btnCancel = document.getElementById('btnCancel');
  const urlPostInquilino = "/inquilino/sendinquilino";

// elemntos seccion contrato
  const fechaInicio = document.getElementById('fechaInicio');
  const fechaFin = document.getElementById('fechaFin');
  const fechaActual = new Date().toISOString().split('T')[0];
  const inputValorInquilino = document.getElementById('valueInquilino');
  const selectInquilino = document.getElementById('idInquilino');
  const contratoEtapa2 = document.getElementById('contratoEtapa2');  
  const urlGetInquilino = "/inquilino/findinquilinos?value=";
 let timeoutId = null;

/** seccion formulario contrato **/
// set fecha actual en form
  fechaInicio.value = fechaActual;
  fechaFin.value = fechaActual;
// en foco input para busqueda
inputValorInquilino.focus();
// listener valores inquilino a buscar,recursivo para cada char 

inputValorInquilino.addEventListener('input', () => {
  selectInquilino.innerHTML = '';
  const valor = inputValor.value.trim();
  if (timeoutId) {
      clearTimeout(timeoutId);
  }
  if (valor.length >= 3) {
    // busqueda inquilino
      findInquilinos(valor);
  } else {

      selectInquilino.classList.add("d-none");
      contratoEtapa2.classList.add("d-none");
  }
});

// fucnion busqyeuda inquilino  
const findInquilinos = (valor) => {
// fetch con concatenacion de dato inquilino en url
    fetch(urlGetInquilinos + encodeURIComponent(valor))
        .then(response => {
            if (!response.ok) {
                selectInquilino.innerHTML = '';
                throw new Error('Error al obtener los datos.');
            }
            let resp = false;
            try {
                resp = response.json();
            } catch {
                console.log('error');
            }
            return resp;
        })
        .then(data => {
            if (data.length === 0) {
                alert2('Inquilinos', "No se encontró. Cree uno", "error");
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
            option.text = "El servidor respondió con un error. Contacte al servicio";
            alerta('Inquilino', "El servidor respondió con un error. Contacte al servicio");
            selectInquilino.appendChild(option);
            console.error('Error:', error);
            selectInquilino.classList.add("d-none");
        });
};


  // Listener botones modal inquilino
  btnSend.addEventListener('click', (event) => {
      event.preventDefault();
      event.stopPropagation();
      const formInputs = Array.from(formInquilino.querySelectorAll('input'));
      const formData = {};
      formInputs.forEach((input) => {
          formData[input.name] = input.value;
      });
      console.log("Datos a enviar:", formData);
        registroInquilino2(formData);
  });

  btnCancel.addEventListener('click', (event) => {
    event.preventDefault();
    event.stopPropagation();
      modalInquilino.classList.add('d-none');
  });

  function registroInquilino2(datos) {
      // Aquí puedes enviar los datos al servidor
      console.log("Enviando datos:", datos);
  }

});
