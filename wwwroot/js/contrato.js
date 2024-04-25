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
             //      alerta('Inquilinos', "No se econtro Cree uno","error")
             registroInquilino();
          //          openD();
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





function  registroInquilino(){
    Swal.fire({
        title: "Nuevo Inquilino",
        html:`
        <div class="w-100 ">
        <div class="w-100 ">
            <label for="nombre-inquilino" class="">Nombre Inquilino</label>
            <input type="text" class="form-control" id="nombre-inquilino" placeholder="Ingrese nombre" name="nombre"
                required verfy="text" value="" >
        </div>
        <div class="w-100 ">
            <label for="apellido-inquilino" class="">Apellido Inquilino</label>
            <input type="text" class="form-control" id="apellido-inquilino" placeholder="Ingrese apellido"
                name="apellido" required verfy="text" value="">
        </div>
        <div class="w-100 ">
            <label for="dni-inquilino" class="">Documento Inquilino</label>
            <input type="number" class="form-control" id="dni-inquilino" placeholder="Ingrese DNI" name="dni" required
                verfy="number" value=""  title="Ingrese un dni valido" >
        </div>
        <div class="w-100 ">
            <label for="email-inquilino" class="">Email Inquilino</label>
            <input type="email" class="form-control" id="email-inquilino" placeholder="Ingrese Email" name="email"
                title="Ingrese un email valido" required verfy="email" value="" >
        </div>
        <div class="w-100 ">
            <label for="telefono-inquilino" class="">Telefono Inquilino</label>
            <input type="text" class="form-control" id="telefono-inquilino" placeholder="Ingrese Telefono" name="telefono"
                title="Ingrese un Telefono valido" required verfy="telefono" value="">
        </div>
    </div>   
        `,
        showCancelButton: true,
        cancelButtonText:"Cerrar",
        confirmButtonText: "GUARDAR",
        showLoaderOnConfirm: true,
        preConfirm: async (login) => {
            try {
                const githubUrl = ` https://api.github.com/users/${login} `;
                const response = await fetch(githubUrl);
                if (!response.ok) {
                return Swal.showValidationMessage(`
                    ${JSON.stringify(await response.json())}
                `);
                }
                console.log("mierda");
                return response.json();
            } 
            catch (error) {
                Swal.showValidationMessage(`
                Request failed: ${error}
                `);
            }
        },
        allowOutsideClick: () => !Swal.isLoading()
      }).then((result) => {
        if (result.isConfirmed) {
          Swal.fire({
            title: `${result.value.login}'s avatar`,
            imageUrl: result.value.avatar_url
          });
        }
      });
}   

});

Swal.fire({
    title: "Submit your Github username",
    input: "text",
    inputAttributes: {
      autocapitalize: "off"
    },
    showCancelButton: true,
    confirmButtonText: "Look up",
    showLoaderOnConfirm: true,
    preConfirm: 
    async (login) => {
      try {
        const githubUrl = ` https://api.github.com/users/${login} `;
        const response = await fetch(githubUrl);
        if (!response.ok) {
          return Swal.showValidationMessage(`
            ${JSON.stringify(await response.json())}
          `);
        }
        console.log("mierda");

        return response.json();

      } catch (error) {
        Swal.showValidationMessage(`
          Request failed: ${error}
        `);
      }

    
    },
    allowOutsideClick: () => !Swal.isLoading()
  }).then((result) => {
    if (result.isConfirmed) {
      Swal.fire({
        title: `${result.value.login}'s avatar`,
        imageUrl: result.value.avatar_url
      });
    }
  });


  