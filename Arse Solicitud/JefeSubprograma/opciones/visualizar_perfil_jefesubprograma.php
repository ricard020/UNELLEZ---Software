<?php
// Importar la autenticación
require_once '../../PHP/Admin_seguridad.php';
verificarRol('JP');

// Obtener los datos del jefe de subprograma
require_once '../../PHP/bd_conexion.php';
$userId = $_SESSION['user_id'];

try {
    $stmt = $pdo->prepare("
        SELECT 
            u.primer_nombre,
            u.primer_apellido,
            u.correo,
            s.nombre_subprograma
        FROM usuarios u
        LEFT JOIN jefe_subprogramas js ON u.id = js.jefe_id
        LEFT JOIN subprogramas s ON js.subprograma_id = s.id
        WHERE u.id = :userId AND u.rol = 'JP'
    ");
    $stmt->execute(['userId' => $userId]);
    $jefe = $stmt->fetch(PDO::FETCH_ASSOC);

    if (!$jefe) {
        echo "No se encontraron datos del jefe de subprograma.";
        exit();
    }
} catch (PDOException $e) {
    echo "Error al obtener los datos del jefe de subprograma: " . $e->getMessage();
    exit();
}
?>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Perfil del Jefe de Subprograma</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="../../CSS/Visualizar_perfil_jefesubprogramas.css" rel="stylesheet">
</head>
<body>
    <!-- Header -->
    <header class="d-flex justify-content-between align-items-center" style="position: fixed; top: 0; left: 0; width: 100%; z-index: 1000; background-color: rgb(49, 49, 49); padding: 10px 20px;">
    <a href="../perfil_jefesubprograma.php">
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
            <ul class="dropdown-menu">
                <li><a class="dropdown-item" href="./visualizar_perfil_jefesubprograma.php">Datos del usuario</a></li>
                <li><a class="dropdown-item" href="../../PHP/logout.php">Cerrar Sesión</a></li>
            </ul>
        </div>
     </div>
  </header>

    <!-- Main Content -->
    <div class="container mt-5 pt-5">
        <div class="card transform-card">
            <div class="card-body text-center">
                <h5 class="card-title" id="nombre-apellido"><?= htmlspecialchars($jefe['primer_nombre'] . ' ' . $jefe['primer_apellido']) ?></h5>
                <p class="card-text"><span class="card-text-label">Correo:</span> <span id="correo"><?= htmlspecialchars($jefe['correo']) ?></span></p>
                <p class="card-text"><span class="card-text-label">Subprograma:</span> <span id="subprograma"><?= htmlspecialchars($jefe['nombre_subprograma']) ?></span></p>
                <button class="btn btn-primary mt-3" data-bs-toggle="modal" data-bs-target="#editarDatosModal">Editar Datos</button>
                <button class="btn btn-secondary mt-3" data-bs-toggle="modal" data-bs-target="#editarContrasenaModal">Cambiar Contraseña</button>
                <button type="button" class="btn btn-primary mt-3" data-bs-toggle="modal" data-bs-target="#editarCorreoModal">
                    Editar Correo
                </button>
            </div>
        </div>
    </div>

    <!-- Modal para editar datos -->
    <div style="margin-top: 10%;" class="modal fade" id="editarDatosModal" tabindex="-1" aria-labelledby="editarDatosModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <form id="formEditarDatos">
                    <div class="modal-header">
                        <h5 class="modal-title" id="editarDatosModalLabel">Editar Datos</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div class="mb-3">
                            <label for="primerNombre" class="form-label">Primer Nombre</label>
                            <input type="text" class="form-control" id="primerNombre" name="primerNombre" value="<?= htmlspecialchars($jefe['primer_nombre']) ?>" required>
                        </div>
                        <div class="mb-3">
                            <label for="primerApellido" class="form-label">Primer Apellido</label>
                            <input type="text" class="form-control" id="primerApellido" name="primerApellido" value="<?= htmlspecialchars($jefe['primer_apellido']) ?>" required>
                        </div>
                    </div>
                    <div class="modal-footer d-flex justify-content-center">
                        <button type="submit" class="btn btn-primary">Guardar Cambios</button>
                    </div>

                </form>
            </div>
        </div>
    </div>

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

    
    <!--modal para editar correo -->

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

    <footer class="py-3 text-center position-fixed bottom-0 w-100" style="background-color: #FF6B00; height: 50px;">
    <div class="container d-flex justify-content-center w-100">
        <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
    </footer>
    
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script>
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
                url: '../Funciones/editar_contrasena.php',
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

        $('#formEditarDatos').on('submit', function (e) {
    e.preventDefault();

    const primerNombre = $('#primerNombre').val();
    const primerApellido = $('#primerApellido').val();

    console.log('Primer Nombre:', primerNombre);
    console.log('Primer Apellido:', primerApellido);

    $.ajax({
        url: '../Funciones/editar_datos.php',
        method: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            primerNombre: primerNombre,
            primerApellido: primerApellido,
        }),
        success: function (response) {
            const data = JSON.parse(response);
            if (data.success) {
                alert('Datos actualizados con éxito.');
                location.reload(); // Refrescar la página
            } else {
                alert(data.error);
            }
        },
        error: function () {
            alert('Error al procesar la solicitud.');
        }
    });
});

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

    </script>
</body>
</html>