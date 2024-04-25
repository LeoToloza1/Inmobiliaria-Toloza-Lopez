document.addEventListener('DOMContentLoaded', function () {
  //modal inquilino
    const modalInquilino= document.getElementById('modalInquilino');
    const formInquilino= document.getElementById('formInquilino');
    const btnSend= document.getElementById('btnSend');
    const btnCancel= document.getElementById('btnCancel');

    const fechaInicio = document.getElementById('fechaInicio');
    const fechaActual = new Date().toISOString().split('T')[0];
    const inputValor = document.getElementById('valueInquilino');
    const selectInquilino = document.getElementById('idInquilino');
    const contratoEtapa2 = document.getElementById('contratoEtapa2');

   // listener botones modal inquilino
   const contolarBtnSendClick = (event) => {
    event.preventDefault();
    event.stopPropagation();
    const formInput = Array.from(formInquilino.querySelectorAll('input')); 
    const dataInquilino = {};
    formInput.forEach((input) => {
        dataInquilino[input.name] = input.value; 
    });
    sendDataInquilino(dataInquilino);

};
const sendDataInquilino = (data) => {
  fetch('url_del_servidor', {
      method: 'POST',
      headers: {
          'Content-Type': 'application/json'
      },
      body: JSON.stringify(data)
  })
  .then(response => {
      if (!response.ok) {
          throw new Error('Error al enviar los datos.');
      }
      return response.json();
  })
  .then(data => {
      console.log('Datos enviados con éxito:', data);
      // Aquí puedes manejar la respuesta del servidor
  })
  .catch(error => {
      console.error('Error:', error);
      // Aquí puedes manejar errores de envío
  });
};


btnSend.addEventListener('click', contolarBtnSendClick);


  //  btnSend.addEventListener('click', (event)=> {
  //     event.preventDefault();
  //     event.stopPropagation();
  //     const formInput = Array.from(formInquilino.querySelectorAll('input')); 
  //     const dataInquilino = {};
  //     formInput.forEach((input) => {
  //         dataInquilino[input.name] = input.value; 
  //     });
  //     console.log("Datos a enviar:", dataInquilino);
  //     sendDataInquilino(dataInquilino);

  //   })
    btnCancel.addEventListener('click', () => {
      modalInquilino.classList.add('d-none');
    });

    let timeoutId = null;
    const urlInquilinos = "/inquilino/findinquilinos?value=";
    fechaInicio.value = fechaActual;
    inputValor.focus();
    inputValor.addEventListener('input', () => {
        selectInquilino.innerHTML = '';
        const valor = inputValor.value.trim();
        if (timeoutId) {
            clearTimeout(timeoutId);
        }
        if (valor.length >= 3) {
          console.log(valor);
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
                  modalInquilino.classList.remove("d-none");

              registroInquilino2(data.length);
              // openD();
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

function registroInquilino2(valor){
  
  console.log("registro " + valor)
}
});