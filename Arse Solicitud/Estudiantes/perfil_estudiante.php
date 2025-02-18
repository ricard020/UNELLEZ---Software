<?php
// Importar la autenticación
require_once '../php/Admin_seguridad.php';
require_once './Funciones/perfil_estudiante_funcion.php';
verificarRol('Estudiante');
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Portal Académico</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <link href="../CSS/perfil_estudiante.css" rel="stylesheet">

</head>
<body>

    <!-- Header -->
    <header class="d-flex justify-content-between align-items-center" style="position: fixed; top: 0; left: 0; width: 100%; z-index: 1000; background-color: rgb(49, 49, 49); padding: 10px 20px;">
    <div class="container-fluid d-flex align-items-center justify-content-start" style="padding-left: 0;">
        <a href="perfil_estudiante.php">    
        <img title="Inicio" src="../imagen/logo unellez.png" alt="Icono de ejemplo" width="60" >
        </a>
        <div class="d-flex align-items-center w-100 justify-content-center">

            <h1 class="mb-0" style="color: #FF6B00;">Bienvenido</h1>
            
        </div>
        <div class="d-flex align-items-center ms-auto">
            <div class="dropdown position-relative me-4" id="notification-bell">
                <a href="#" id="notification-icon" data-bs-toggle="dropdown" aria-expanded="false">
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#FF6B00" stroke-width="2">
                        <path d="M18 8A6 6 0 0 0 6 8c0 7-3 9-3 9h18s-3-2-3-9"></path>
                        <path d="M13.73 21a2 2 0 0 1-3.46 0"></path>
                    </svg>
                    <span class="position-absolute top-0 start-100 translate-middle badge rounded-pill bg-warning"><?= count($notificaciones) ?></span>
                </a>
                <ul class="dropdown-menu dropdown-menu-end" aria-labelledby="notification-icon" style="width: 300px;">
                    <?php if (empty($notificaciones)): ?>
                        <li><span class="dropdown-item">No hay notificaciones</span></li>
                    <?php else: ?>
                        <?php foreach ($notificaciones as $notificacion): ?>
                            <li class="dropdown-item notification-item" data-bs-toggle="modal" data-bs-target="#notificationModal" data-id="<?= $notificacion['id'] ?>" data-message="<?= htmlspecialchars($notificacion['mensaje']) ?>" data-date="<?= date('d/m/Y h:i A', strtotime($notificacion['fecha_envio'])) ?>">
                                <div class="d-flex justify-content-between">
                                    <div class="notification-summary">Nueva solicitud</div>
                                    <div class="notification-date"><?= date('h:i A', strtotime($notificacion['fecha_envio'])) ?></div>
                                </div>
                            </li>
                        <?php endforeach; ?>
                    <?php endif; ?>
                </ul>
            </div>
            <div class="dropdown d-inline-block">
                <a href="#" id="profile-icon" data-bs-toggle="dropdown" aria-expanded="false">
                    <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke="#FF6B00" stroke-width="2">
                        <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"></path>
                        <circle cx="12" cy="7" r="4"></circle>
                    </svg>
                </a>
                <ul class="dropdown-menu dropdown-menu-end">
                    <li><a class="dropdown-item" href="./opciones/visualizar_perfil.php">Datos del usuario</a></li>
                    <li><a class="dropdown-item" href="./opciones/Lista_solicitud.php">Solicitudes</a></li>
                    <li><a class="dropdown-item" href="../PHP/logout.php">Cerrar Sesión</a></li>
                </ul>
            </div>
        </div>
    </div>
    </header>

    <!-- Modal para mostrar detalles de la notificación -->
    <div class="modal fade" id="notificationModal" tabindex="-1" aria-labelledby="notificationModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="notificationModalLabel">Detalles de la Notificación</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p id="notificationMessage"></p>
                    <p class="text-muted" id="notificationDate"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Main Content -->
    <div class="d-flex justify-content-center align-items-center" style="height: 100vh;">
        <div class="container" style="background-color: rgba(255, 249, 249, 0.99); border-radius: 20px; padding: 20px;">
            <div class="container-fluid content-container">
                <div class="container py-5">
                    <div class="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
                        <?php if (!empty($subprogramas)): ?>
                            <?php foreach ($subprogramas as $subprograma): ?>
                                <div class="col">
                                    <div class="d-flex justify-content-center align-items-center" style="background-color:rgb(249, 114, 42); border-radius: 15px; height: 150px;">
                                        <a href="opciones/solicitudes.php?subprograma_id=<?= urlencode($subprograma['id']); ?>" class="btn program btn-lg h-100 w-100 text-center d-flex align-items-center justify-content-center" style="font-size: 1.5rem;">
                                           <strong> <?= htmlspecialchars($subprograma['nombre_subprograma']); ?></strong>
                                        </a>
                                    </div>
                                </div>
                            <?php endforeach; ?>
                        <?php else: ?>
                            <p class="text-center">No estás inscrito en ningún subprograma.</p>
                        <?php endif; ?>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Footer -->
    <footer class="py-3 text-center position-fixed bottom-0 w-100" style="background-color: #FF6B00; height: 50px;">
    <div class="container d-flex justify-content-center w-100">
        <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
    </footer>

    <!-- Scripts -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        var notificationModal = document.getElementById('notificationModal');
        notificationModal.addEventListener('show.bs.modal', function (event) {
            var button = event.relatedTarget;
            var message = button.getAttribute('data-message');
            var date = button.getAttribute('data-date');
            var id = button.getAttribute('data-id');

            var modalMessage = notificationModal.querySelector('#notificationMessage');
            var modalDate = notificationModal.querySelector('#notificationDate');

            modalMessage.textContent = message;
            modalDate.textContent = date;

        });
    </script>
</body>
</html>