document.addEventListener('DOMContentLoaded', function () {
  const modalInquilino = document.getElementById('modalInquilino');
  const formInquilino = document.getElementById('formInquilino');
  const btnSend = document.getElementById('btnSend');
  const btnCancel = document.getElementById('btnCancel');

  // Listener botones modal inquilino
  btnSend.addEventListener('click', (event) => {
      event.preventDefault();
      event.stopPropagation();
      const formInputs = Array.from(formInquilino.querySelectorAll('input'));
      const formData = {};
      formInputs.forEach((input) => {
          formData[input.name] = input.value;
      });
      console.log("Datos axxxx enviar:", formData);
      // Aquí puedes llamar a la función para enviar los datos
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
