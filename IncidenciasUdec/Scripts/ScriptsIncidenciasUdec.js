function activarFormularioDocente() {

    var div = document.getElementById("formularioDocente");
    div.style.display = 'block';
    var boton = document.getElementById("actFormEstudiante");
    boton.style.display = 'none';
}

function activarFormularioEstudiante() {
    var div = document.getElementById("formularioEstudiante");
    div.style.display = 'block';
    var boton = document.getElementById("btnActFormDocente");
    boton.style.display = 'none';
}

function verTodosTiposUsuario() {
    window.location.reload();
}
