<?php
require_once '../Funciones/Solicitudes_backend.php';
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../../CSS/Lista_solicitud.css">
</head>
<header class="d-flex justify-content-between align-items-center" style="position: fixed; top: 0; left: 0; width: 100%; z-index: 1000; background-color: rgb(49, 49, 49); padding: 10px 20px;">
    <a href="../perfil_jefesubprograma.php">
    <img title="Inicio" src="../../imagen/logo unellez.png" alt="Logo Unellez" width="60">
    </a>
    <h1 style="color: #FF6B00;">Solicitudes Pendientes</h1>
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

<div class="container py-5">
<body class="bg-light">
    <div class="container py-5">
        <div class="btn-group mb-3" role="group" aria-label="Filtros de estado">
            <a href="?subprograma_id=<?= $subprogramaId ?>&estado=Pendiente" class="btn btn-outline-danger <?php echo ($estado === 'Pendiente') ? 'active' : ''; ?>">Pendientes</a>
            <a href="?subprograma_id=<?= $subprogramaId ?>&estado=Diferida" class="btn btn-outline-warning <?php echo ($estado === 'Diferida') ? 'active' : ''; ?>">Diferidas</a>
            <a href="?subprograma_id=<?= $subprogramaId ?>&estado=Elevada" class="btn btn-outline-info <?php echo ($estado === 'Elevada') ? 'active' : ''; ?>">Elevadas</a>
        </div>
        <?php if (!empty($solicitudes)): ?>
            <div class="table-container active">
                <table class="table table-striped table-hover table-bordered">
                    <thead class="bg-custom-orange text-white">
                        <tr>
                            <th>ID Solicitud</th>
                            <th>Nombre Estudiante</th>
                            <th>Tipo Solicitud</th>
                            <th>Fecha Solicitud</th>
                            <th>Archivo PDF</th>
                            <?php if ($estado === 'Diferida' || $estado === 'Elevada'): ?>
                                <th>Notas</th>
                                <th>Caso</th>
                                <th>Resolución</th>
                            <?php endif; ?>
                            <th>Acción</th>
                        </tr>
                    </thead>
                    <tbody>
                        <?php foreach ($solicitudes as $solicitud): ?>
                            <tr>
                                <td><?php echo htmlspecialchars($solicitud['solicitud_id']); ?></td>
                                <td><?php echo htmlspecialchars($solicitud['nombre_estudiante']); ?></td>
                                <td><?php echo htmlspecialchars($solicitud['tipo_solicitud']); ?></td>
                                <td><?php echo date("d/m/Y h:i A", strtotime($solicitud['fecha_solicitud'])); ?></td>
                                <td>
                                    <a href="../Funciones/ver_pdf.php?id=<?php echo htmlspecialchars($solicitud['solicitud_id']); ?>" target="_blank" class="btn btn-info btn-sm">Ver PDF</a>
                                </td>
                                <?php if ($estado === 'Diferida' || $estado === 'Elevada'): ?>
                                    <td>
                                        <button class="btn btn-secondary btn-sm" onclick="openNotesModal('<?php echo htmlspecialchars($solicitud['nota']); ?>')">Ver notas</button>
                                    </td>
                                    <td><?php echo htmlspecialchars($solicitud['numero_caso']); ?></td>
                                    <td><?php echo htmlspecialchars($solicitud['numero_resolucion']); ?></td>
                                <?php endif; ?>
                                <td>
                                    <?php if ($estado === 'Pendiente'): ?>
                                        <button class="btn btn-success btn-sm" onclick="openModal('aceptar', <?php echo $solicitud['solicitud_id']; ?>)">Aceptar</button>
                                        <button class="btn btn-danger btn-sm" onclick="openModal('rechazar', <?php echo $solicitud['solicitud_id']; ?>)">Rechazar</button>
                                        <button class="btn btn-warning btn-sm" onclick="openModal('diferir', <?php echo $solicitud['solicitud_id']; ?>)">Diferir</button>
                                        <button class="btn btn-primary btn-sm" onclick="openModal('elevar', <?php echo $solicitud['solicitud_id']; ?>)">Elevar</button>
                                    <?php else: ?>
                                        <button class="btn btn-success btn-sm" onclick="openModal('aceptar', <?php echo $solicitud['solicitud_id']; ?>)">Aceptar</button>
                                        <button class="btn btn-danger btn-sm" onclick="openModal('rechazar', <?php echo $solicitud['solicitud_id']; ?>)">Rechazar</button>
                                    <?php endif; ?>
                                </td>
                            </tr>
                        <?php endforeach; ?>
                    </tbody>
                </table>
            </div>
        <?php else: ?>
            <p class="text-center">No se encontraron solicitudes pendientes.</p>
        <?php endif; ?>
    </div>

    <!-- Modal para acciones -->
    <div class="modal fade" id="actionModal" tabindex="-1" aria-labelledby="actionModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="actionModalLabel">Acción</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="actionForm" method="POST" action="../Funciones/accion_solicitud.php">
                        <input type="hidden" id="solicitudId" name="solicitud_id">
                        <input type="hidden" id="accion" name="accion">
                        <div class="mb-3">
                            <label for="nota" class="form-label">Nota</label>
                            <textarea class="form-control" id="nota" name="nota" rows="3" required></textarea>
                            <div class="invalid-feedback">La nota no puede estar vacía.</div>
                        </div>
                        <div class="mb-3">
                            <label for="numero_caso" class="form-label">Número de caso</label>
                            <input type="number" class="form-control" id="numero_caso" name="numero_caso" required>
                            <div class="invalid-feedback">El número de caso no puede estar vacío.</div>
                        </div>
                        <div class="mb-3">
                            <label for="numero_resolucion" class="form-label">Número de resolución</label>
                            <input type="number" class="form-control" id="numero_resolucion" name="numero_resolucion" required>
                            <div class="invalid-feedback">El número de resolución no puede estar vacío.</div>
                        </div>
                        <button type="submit" class="btn btn-primary">Confirmar</button>
                        <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal para ver notas -->
    <div class="modal fade" id="notesModal" tabindex="-1" aria-labelledby="notesModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="notesModalLabel">Notas de la Solicitud</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p id="notesContent"></p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>
            </div>
        </div>
    </div>

    

    <footer class="py-3 text-center position-fixed bottom-0 w-100" style="background-color: #FF6B00;  height: 50px;">
    <div class="container d-flex justify-content-center w-100">
        <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
    </footer>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function openModal(action, solicitudId) {
            document.getElementById('solicitudId').value = solicitudId;
            document.getElementById('accion').value = action;
            document.getElementById('actionModalLabel').innerText = action.charAt(0).toUpperCase() + action.slice(1) + ' Solicitud';
            var modal = new bootstrap.Modal(document.getElementById('actionModal'));
            modal.show();
        }

        function openNotesModal(nota) {
            document.getElementById('notesContent').innerText = nota;
            var modal = new bootstrap.Modal(document.getElementById('notesModal'));
            modal.show();
        }

        document.getElementById('actionForm').addEventListener('submit', function(event) {
            event.preventDefault();
            var nota = document.getElementById('nota');
            if (nota.value.trim() === '') {
                nota.classList.add('is-invalid');
                return;
            } else {
                nota.classList.remove('is-invalid');
            }

            var formData = new FormData(this);
            fetch('../Funciones/accion_solicitud.php', {
                method: 'POST',
                body: formData
            })
            .then(response => response.text())
            .then(data => {
                if (data.includes("Ya existe una solicitud")) {
                    alert(data);
                } else {
                    alert(data);
                    location.reload();
                }
            })
            .catch(error => console.error('Error:', error));
        });
    </script>
</body>
</html>