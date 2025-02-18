<?php
// Importar la autenticación
require_once '../../php/Admin_seguridad.php';
verificarRol('Estudiante');
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Portal Académico</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
        <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <link href="../../CSS/visualizar_perfil_estudiante.css" rel="stylesheet">
    
</head>
<body>

    <!-- Header -->
    <header class="d-flex justify-content-between align-items-center" style="position: fixed; top: 0; left: 0; width: 100%; z-index: 1000; background-color: rgb(49, 49, 49); padding: 10px 20px;">
    <a href="../perfil_estudiante.php">
    <img title="Inicio" src="../../imagen/logo unellez.png" alt="Logo Unellez" width="60">
    </a>
    <h1 style="color: #FF6B00;">Datos de usuario</h1>
    <div class="d-flex align-items-center">
        <div class="dropdown">
            <a href="#" class="text-white" data-bs-toggle="dropdown">
                <svg width="24" height="24" fill="none" stroke="#FF6B00" stroke-width="2" viewBox="0 0 24 24">
                    <circle cx="12" cy="7" r="4"></circle>
                    <path d="M4 21v-2a4 4 0 0 1 4-4h8a4 4 0 0 1 4 4v2"></path>
                </svg>
            </a>
                    <ul class="dropdown-menu dropdown-menu-end">
                        <li><a class="dropdown-item" href="./visualizar_perfil.php">Datos del usuario</a></li>
                        <li><a class="dropdown-item" href="./Lista_solicitud.php">Solicitudes</a></li>
                        <li><a class="dropdown-item" href="../../PHP/logout.php">Cerrar Sesión</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </header>

    <br>
    <!-- Main Content -->
    <main class="container mt-5 pt-5">
        <div class="row">
            <div class="col-md-4">
                <div class="card transform-card">
                    <div class="card-body text-center">
                        <div class="user-icon-container" id="user-photo-container">
                            <img id="user-photo" class="user-icon" src="../../imagen/user-icon.jpg" alt="Icono de usuario">
                            <div class="overlay">Click para actualizar foto de cédula</div>
                        </div>
                        <br>
                        <h5 class="card-title" id="nombre-apellido">Nombre y Apellido</h5>
                        <p class="card-text"><span class="card-text-label">Correo:</span> <span id="correo">Cargando...</span></p>
                        <p class="card-text"><span class="card-text-label">Cédula de Identidad:</span> <span id="ci">Cargando...</span></p>
                        <!-- Botón para editar contraseña -->
                        <button type="button" class="btn btn-warning mt-3" data-bs-toggle="modal" data-bs-target="#editarContrasenaModal">
                            Editar Contraseña
                        </button>
                        <button type="button" class="btn btn-primary mt-3" data-bs-toggle="modal" data-bs-target="#editarCorreoModal">
                            Editar Correo
                        </button>
                    </div>
                </div>
            </div>
            <div class="col-md-8">
                <div class="card transform-card">
                    <div class="card-body">
                        <h5 class="card-title">Subprogramas</h5>
                        <div id="subprogramas">Cargando...</div>
                    </div>
                </div>
            </div>
        </div>
    </main>
    <!-- Modal para editar contraseña -->
<div style="margin-top: 5%;" class="modal fade" id="editarContrasenaModal" tabindex="-1" aria-labelledby="editarContrasenaModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <form id="formEditarContrasena">
                <div class="modal-header">
                    <h5 class="modal-title" id="editarContrasenaModalLabel">Editar Contraseña</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="contrasenaActual" class="form-label">Contraseña Actual</label>
                        <input type="password" class="form-control" id="contrasenaActual" name="contrasenaActual" required>
                    </div>
                    <div class="mb-3">
                        <label for="nuevaContrasena" class="form-label">Nueva Contraseña</label>
                        <input type="password" class="form-control" id="nuevaContrasena" name="nuevaContrasena" required>
                        
                        
                    </div>
                    <div class="mb-3">
                        <label for="repetirContrasena" class="form-label">Repetir Nueva Contraseña</label>
                        <input type="password" class="form-control" id="repetirContrasena" name="repetirContrasena" required>
                    </div>
                    <div class="progress mt-2">
                            <div class="progress-bar" role="progressbar" style="width: 0%;" id="password-progress"></div>
                        </div>
                    <div class="mt-2">
                            <div class="requirement" id="length">● Mínimo 8 caracteres</div>
                            <div class="requirement" id="uppercase">● Al menos una mayúscula</div>
                            <div class="requirement" id="lowercase">● Al menos una minúscula</div>
                            <div class="requirement" id="number">● Al menos un número</div>
                        </div>
                </div>
                <div class="modal-footer d-flex justify-content-center">
                    <button type="submit" class="btn btn-primary" id="submit-button" disabled>Guardar Cambios</button>
                </div>
            </form>
        </div>
    </div>
</div>

<!-- Modal para editar Correo -->

<div style="margin-top: 10%;" class="modal fade" id="editarCorreoModal" tabindex="-1" aria-labelledby="editarCorreoModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <form id="formEditarCorreo">
                <div class="modal-header">
                    <h5 class="modal-title" id="editarCorreoModalLabel">Editar Correo Electrónico</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <label for="correoActual" class="form-label">Correo Actual</label>
                        <input type="email" class="form-control" id="correoActual" name="correoActual" readonly>
                    </div>
                    <div class="mb-3">
                        <label for="nuevoCorreo" class="form-label">Nuevo Correo Electrónico</label>
                        <input type="email" class="form-control" id="nuevoCorreo" name="nuevoCorreo" required>
                    </div>
                    <div class="mb-3">
                        <label for="contrasenaActualCorreo" class="form-label">Contraseña Actual</label>
                        <input type="password" class="form-control" id="contrasenaActualCorreo" name="contrasenaActualCorreo" required>
                    </div>
                </div>
                <div class="modal-footer d-flex justify-content-center">
                   
                    <button type="submit" class="btn btn-primary">Guardar Cambios</button>
                </div>
            </form>
        </div>
    </div>
</div>

<!-- Modal para editar Cedula -->

<div style="margin-top: 8%;" class="modal fade" id="editarFotoModal" tabindex="-1" aria-labelledby="editarFotoModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <form id="formEditarFoto" enctype="multipart/form-data">
                <div class="modal-header">
                    <h5 class="modal-title" id="editarFotoModalLabel">Actualizar Datos de cédula</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3 text-center">
                    <label for="nuevaFoto" class="form-label">Cédula Nueva</label>
                    <br>
                        <img id="previewFoto" class="user-icon mb-3" src="../../imagen/user-icon.jpg" alt="Vista previa de la foto">
                        
                        <input type="file" class="form-control" id="nuevaFoto" name="nuevaFoto" accept="image/*" required>
                    </div>
                </div>
                <div class="modal-footer d-flex justify-content-center">
    
                    <button type="submit" class="btn btn-primary">Guardar Cambios</button>
                </div>
            </form>
        </div>
    </div>
</div>


 <footer class="py-3 text-center position-fixed bottom-0 w-100" style="background-color: #FF6B00; height: 50px">
    <div class="container d-flex justify-content-center w-100">
        <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
    </footer>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
<script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
<script>
    // Función para obtener y renderizar los datos
    async function cargarDatosPerfil() {
        try {
            const response = await fetch('../Funciones/obtener_datos_estudiante.php');
            const data = await response.json();

            if (data.error) {
                alert(data.error);
                return;
            }

            // Renderizar los datos en el HTML
            document.getElementById('nombre-apellido').textContent = `${data.primer_nombre} ${data.segundo_nombre || ''} ${data.primer_apellido} ${data.segundo_apellido || ''}`.trim();
            document.getElementById('ci').textContent = data.ci || 'No disponible';
            document.getElementById('correo').textContent = data.correo || 'No disponible';

            // Cambiar la imagen si existe una URL válida
            if (data.foto_perfil) {
                const timestamp = new Date().getTime();
                const fotoPerfilUrl = `../../${data.foto_perfil}?t=${timestamp}`;
                document.getElementById('user-photo').src = fotoPerfilUrl;
            }

            // Renderizar los subprogramas agrupados por programa
            const subprogramasDiv = document.getElementById('subprogramas');
            subprogramasDiv.innerHTML = '';

            for (const programaId in data.subprogramas) {
                const programa = data.subprogramas[programaId];
                const programaDiv = document.createElement('div');
                programaDiv.classList.add('programa');

                const programaTitulo = document.createElement('h6');
                programaTitulo.classList.add('programa-titulo');
                programaTitulo.textContent = programa.nombre_programa;
                programaDiv.appendChild(programaTitulo);

                for (const sedeId in programa.sedes) {
                    const sede = programa.sedes[sedeId];
                    const sedeDiv = document.createElement('div');
                    sedeDiv.classList.add('sede');

                    const sedeTitulo = document.createElement('h6');
                    sedeTitulo.classList.add('sede-titulo');
                    sedeTitulo.textContent = sede.nombre_sede;
                    sedeDiv.appendChild(sedeTitulo);

                    const subprogramasList = document.createElement('ul');
                    sede.subprogramas.forEach(subprograma => {
                        const subprogramaItem = document.createElement('li');
                        subprogramaItem.textContent = subprograma;
                        subprogramasList.appendChild(subprogramaItem);
                    });

                    sedeDiv.appendChild(subprogramasList);
                    programaDiv.appendChild(sedeDiv);
                }

                subprogramasDiv.appendChild(programaDiv);
            }
        } catch (error) {
            console.error('Error al cargar los datos:', error);
            alert('Hubo un error al cargar los datos del perfil.');
        }
    }

    // Prellenar el correo actual al abrir el modal
$('#editarCorreoModal').on('show.bs.modal', function () {
    const correoActual = $('#correo').text();
    $('#correoActual').val(correoActual);
});

// Enviar formulario de edición de correo
$('#formEditarCorreo').on('submit', function (e) {
    e.preventDefault();

    const contrasenaActual = $('#contrasenaActualCorreo').val();
    const nuevoCorreo = $('#nuevoCorreo').val();

    $.ajax({
        url: '../Funciones/editar_correo.php',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ contrasenaActual, nuevoCorreo }),
        success: function (response) {
            const data = JSON.parse(response);
            if (data.success) {
                alert('Correo actualizado con éxito.');
                $('#correo').text(nuevoCorreo); // Actualizar visualmente
                $('#editarCorreoModal').modal('hide');
                location.reload(); // Refrescar la página
            } else {
                alert(data.error || 'Hubo un problema al actualizar el correo.');
            }
        },
        error: function () {
            alert('Error al comunicarse con el servidor.');
        }
    });
});
    // Validar contraseña
    $('#nuevaContrasena, #repetirContrasena').on('input', function () {
        const password = $('#nuevaContrasena').val();
        const confirmPassword = $('#repetirContrasena').val();

        const length = password.length >= 8;
        const uppercase = /[A-Z]/.test(password);
        const lowercase = /[a-z]/.test(password);
        const number = /\d/.test(password);
        const passwordsMatch = password === confirmPassword;

        $('#length').toggleClass('text-success', length).toggleClass('text-danger', !length);
        $('#uppercase').toggleClass('text-success', uppercase).toggleClass('text-danger', !uppercase);
        $('#lowercase').toggleClass('text-success', lowercase).toggleClass('text-danger', !lowercase);
        $('#number').toggleClass('text-success', number).toggleClass('text-danger', !number);

        const progress = (length + uppercase + lowercase + number) * 25;
        $('#password-progress').css('width', progress + '%');

        $('#submit-button').prop('disabled', !(length && uppercase && lowercase && number && passwordsMatch));
    });

    // Enviar formulario de edición de contraseña
    $('#formEditarContrasena').on('submit', function (e) {
        e.preventDefault();

        const currentPassword = $('#contrasenaActual').val();
        const newPassword = $('#nuevaContrasena').val();
        const confirmPassword = $('#repetirContrasena').val();

        if (newPassword !== confirmPassword) {
            alert('Las contraseñas no coinciden.');
            return;
        }

        $.ajax({
            url: '../../Estudiantes/Funciones/editar_contrasena.php',
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                contrasenaActual: currentPassword,
                nuevaContrasena: newPassword
            }),
            success: function (response) {
                const data = JSON.parse(response);
                if (data.success) {
                    alert('Contraseña actualizada con éxito. Redirigiendo al inicio...');
                    window.location.href = '../../index.html'; // Redirigir al inicio
                } else {
                    alert(data.error);
                }
            },
            error: function () {
                alert('Error al procesar la solicitud.');
            }
        });
    });

    // Cargar datos al cargar la página
    document.addEventListener('DOMContentLoaded', cargarDatosPerfil);

    // Manejar el clic en la foto de perfil y el contenedor
    document.getElementById('user-photo-container').addEventListener('click', function() {
        $('#editarFotoModal').modal('show');
    });

    // Mostrar vista previa de la nueva foto
    document.getElementById('nuevaFoto').addEventListener('change', function(event) {
        const [file] = event.target.files;
        if (file) {
            document.getElementById('previewFoto').src = URL.createObjectURL(file);
        }
    });

    // Enviar formulario de edición de foto
    $('#formEditarFoto').on('submit', function (e) {
        e.preventDefault();

        const formData = new FormData(this);

        $.ajax({
            url: '../Funciones/editar_foto.php',
            method: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                const data = JSON.parse(response);
                if (data.success) {
                    alert('Foto de perfil actualizada con éxito.');
                    location.reload(); // Refrescar la página
                } else {
                    alert(data.error || 'Hubo un problema al actualizar la foto de perfil.');
                }
            },
            error: function () {
                alert('Error al comunicarse con el servidor.');
            }
        });
    });
</script>

</body>
</html>