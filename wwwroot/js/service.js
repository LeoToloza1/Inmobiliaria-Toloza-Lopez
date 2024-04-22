function alerta(titulo, msg, icon = "question") {
  Swal.fire({
    title: `<h1>${titulo}</h1>`,
    icon: `${icon}`,
    html: ` <b>${msg}</b>`,
    showConfirmButton: false,
    //     showCloseButton: true,
    showCancelButton: true,
    focusCancel: true,
    cancelButtonText: `<i class="fas fa-thumbs-down"></i> Cerrar`,
    cancelButtonAriaLabel: "Thumbs down",
  });
}
const alert2 = (titulo, msg, icon = "question") => {
  Swal.fire({
    title: `<h1>${titulo}</h1>`,
    icon: `${icon}`,
    html: ` <b>${msg}</b>`,
    showConfirmButton: false,
    showCancelButton: true,
    focusCancel: true,
    cancelButtonText: `<i class="fas fa-thumbs-down"></i> Cerrar`,
    cancelButtonAriaLabel: "Thumbs down",
  });
};
function confirmar() {
  const swalWithBootstrapButtons = Swal.mixin({
    customClass: {
      confirmButton: "btn btn-success",
      cancelButton: "btn btn-danger",
    },
    buttonsStyling: false,
  });
  swalWithBootstrapButtons
    .fire({
      title: "Are you sure?",
      text: "You won't be able to revert this!",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Yes, delete it!",
      cancelButtonText: "No, cancel!",
      reverseButtons: true,
    })
    .then((result) => {
      if (result.isConfirmed) {
        swalWithBootstrapButtons.fire({
          title: "Deleted!",
          text: "Your file has been deleted.",
          icon: "success",
        });
      } else if (
        /* Read more about handling dismissals below */
        result.dismiss === Swal.DismissReason.cancel
      ) {
        swalWithBootstrapButtons.fire({
          title: "Cancelled",
          text: "Your imaginary file is safe :)",
          icon: "error",
        });
      }
    });
}
const formModal2 = async () => {
  const { value: formValues } = await Swal.fire({
    title: "Multiple inputs",
    html: `
        <input id="swal-input1" class="swal2-input">
        <input id="swal-input2" class="swal2-input">
        `,
    focusConfirm: false,
    preConfirm: () => {
      return [
        document.getElementById("swal-input1").value,
        document.getElementById("swal-input2").value,
      ];
    },
  });
  if (formValues) {
    Swal.fire(JSON.stringify(formValues));
  }
};
async function formModal() {
  const { value: formValues } = await Swal.fire({
    title: "Multiple inputs",
    html: `
        <input id="swal-input1" class="swal2-input">
        <input id="swal-input2" class="swal2-input">
        `,
    focusConfirm: false,
    preConfirm: () => {
      return [
        document.getElementById("swal-input1").value,
        document.getElementById("swal-input2").value,
      ];
    },
  });
  if (formValues) {
    Swal.fire(JSON.stringify(formValues));
  }
}
export function confirmarBtn(btnId, formId) {
  document.getElementById(btnId).addEventListener("click", function (event) {
    event.preventDefault();
    Swal.fire({
      title: "¿Estás seguro?",
      text: "Estás por realizar una acción importante. ¿Deseas continuar?",
      icon: "warning",
      showCancelButton: true,
      confirmButtonColor: "#3085d6",
      cancelButtonColor: "#d33",
      confirmButtonText: "Sí, continuar",
      cancelButtonText: "Cancelar",
      allowOutsideClick: false,
    }).then((result) => {
      if (result.isConfirmed) {
        document.getElementById(formId).submit();
      }
    });
  });
}
