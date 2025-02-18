<?php
require_once '../../PHP/bd_conexion.php';
require_once '../../PHP/Admin_seguridad.php';
require_once '../funcion/GJ_funcion.php';
verificarRol('SA');

$jefes = obtenerJefesSubprograma($pdo);
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Super Usuario - Gestionar Jefes de Subprograma</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<style>
        
        body {
            margin: 0px;
            padding: 0px;
            font-family: Arial, sans-serif;
            background-image: url('../../Imagen/2.jpg');
            background-size: cover;
            background-position: center;
            background-attachment: fixed;
            min-height: 100vh;
        }
</style>
<body class="bg-light">
    <div class="container py-5">
        <header class="text-center mb-4">
            <h1 class="display-5 fw-semibold">Jefes de Subprograma</h1>
        </header>

        <div class="table-responsive">
            <table class="table table-striped table-hover align-middle">
                <thead class="table-primary">
                    <tr>
                        <th>ID</th>
                        <th>Nombre</th>
                        <th>Apellido</th>
                        <th>Correo</th>
                        <th>Subprograma</th>
                        <th>Fecha de Creación</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody id="jefesList">
                    <?php if (!empty($jefes)): ?>
                        <?php foreach ($jefes as $jefe): ?>
                            <tr id="jefe-<?= $jefe['id']; ?>" data-nombre-usuario="<?= htmlspecialchars($jefe['nombre_usuario']); ?>">
                                <td><?= htmlspecialchars($jefe['id']); ?></td>
                                <td><?= htmlspecialchars($jefe['primer_nombre']); ?></td>
                                <td><?= htmlspecialchars($jefe['primer_apellido']); ?></td>
                                <td><?= htmlspecialchars($jefe['correo']); ?></td>
                                <td><?= htmlspecialchars($jefe['subprogramas'] ?? 'Sin asignar'); ?></td>
                                <td><?= htmlspecialchars($jefe['fecha_creacion']); ?></td>
                                <td>
                                    <button class="btn btn-warning btn-sm me-1" onclick="editJefe(<?= $jefe['id']; ?>)">Editar</button>
                                    <button class="btn btn-danger btn-sm" onclick="deleteJefe(<?= $jefe['id']; ?>)">Eliminar</button>
                                </td>
                            </tr>
                        <?php endforeach; ?>
                    <?php else: ?>
                        <tr>
                            <td colspan="7" class="text-center">No se encontraron jefes registrados.</td>
                        </tr>
                    <?php endif; ?>
                </tbody>
            </table>
        </div>

        <footer class="text-center mt-4">
            <a href="../menu_super_usuario.php" class="btn btn-primary">&larr; Volver al Menú</a>
        </footer>
    </div>

    <!-- Modal para editar jefe -->
    <div class="modal fade" id="editModal" tabindex="-1" aria-labelledby="editModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="editModalLabel">Editar Jefe de Subprograma</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="editForm">
                        <input type="hidden" id="editId" name="id">
                        <div class="mb-3">
                            <label for="editNombre" class="form-label">Nombre</label>
                            <input type="text" class="form-control" id="editNombre" name="nombre" required>
                        </div>
                        <div class="mb-3">
                            <label for="editApellido" class="form-label">Apellido</label>
                            <input type="text" class="form-control" id="editApellido" name="apellido" required>
                        </div>
                        <div class="mb-3">
                            <label for="editCorreo" class="form-label">Correo</label>
                            <input type="email" class="form-control" id="editCorreo" name="correo" required>
                        </div>
                        <div class="mb-3">
                            <label for="editNombreUsuario" class="form-label">Nombre de Usuario</label>
                            <input type="text" class="form-control" id="editNombreUsuario" name="nombre_usuario" required>
                        </div>
                        <div class="mb-3">
                            <label for="editPassword" class="form-label">Contraseña</label>
                            <input type="password" class="form-control" id="editPassword" name="password">
                        </div>
                        <button type="submit" class="btn btn-primary">Guardar Cambios</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function deleteJefe(id) {
            if (confirm(`¿Está seguro de que desea eliminar al jefe con ID ${id}?`)) {
                fetch('../funcion/GJ_funcion.php', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({ action: 'delete', id: id })
                })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        alert('Jefe eliminado exitosamente');
                        location.reload();
                    } else {
                        alert('Error al eliminar jefe: ' + data.message);
                    }
                })
                .catch(error => {
                    console.error('Error en la solicitud:', error);
                    alert('Error en la solicitud: ' + error.message);
                });
            }
        }

        function editJefe(id) {
            const modal = new bootstrap.Modal(document.getElementById('editModal'));
            const row = document.getElementById(`jefe-${id}`);
            document.getElementById('editId').value = id;
            document.getElementById('editNombreUsuario').value = row.getAttribute('data-nombre-usuario');
            document.getElementById('editNombre').value = row.cells[1].textContent.trim();
            document.getElementById('editApellido').value = row.cells[2].textContent.trim();
            document.getElementById('editCorreo').value = row.cells[3].textContent.trim();

            modal.show();
        }

        document.getElementById('editForm').addEventListener('submit', function(event) {
            event.preventDefault();
            const id = document.getElementById('editId').value;
            const nombre_usuario = document.getElementById('editNombreUsuario').value;
            const nombre = document.getElementById('editNombre').value;
            const apellido = document.getElementById('editApellido').value;
            const correo = document.getElementById('editCorreo').value;
            const password = document.getElementById('editPassword').value;

            fetch('../funcion/GJ_funcion.php', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ action: 'update', id, nombre_usuario, nombre, apellido, correo, password })
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Jefe actualizado correctamente.');
                    location.reload();
                } else {
                    alert('Error al actualizar el jefe: ' + data.message);
                }
            })
            .catch(error => alert('Error en la solicitud: ' + error.message));
        });
    </script>
</body>
</html>
