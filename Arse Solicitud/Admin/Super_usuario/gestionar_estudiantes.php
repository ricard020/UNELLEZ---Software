<?php
require_once '../../PHP/bd_conexion.php';
require_once '../../PHP/Admin_seguridad.php';
require_once '../funcion/GE_funcion.php';
verificarRol('SA');

$estudiantes = obtenerEstudiantes($pdo);
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Super Usuario - Gestionar Estudiantes</title>
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
<body class="bg-light" style= "font-family: Arial, sans-serif; ">
    <div class="container py-5">
        <header class="text-center mb-4">
            <h1 class="display-5 fw-semibold">Estudiantes</h1>
        </header>

        <div class="table-responsive">
            <table class="table table-striped table-hover align-middle">
                <thead class="table-primary">
                    <tr>
                        <th>ID</th>
                        <th>Nombre de Usuario</th>
                        <th>Primer Nombre</th>
                        <th>Segundo Nombre</th>
                        <th>Primer Apellido</th>
                        <th>Segundo Apellido</th>
                        <th>Correo</th>
                        <th>Sub-programas</th>
                        <th>Fecha de Creación</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody id="estudiantesList">
                    <?php if (!empty($estudiantes)): ?>
                        <?php foreach ($estudiantes as $estudiante): ?>
                            <tr id="estudiante-<?= $estudiante['id']; ?>" data-id="<?= $estudiante['id']; ?>">
                                <td><?= htmlspecialchars($estudiante['id']); ?></td>
                                <td class="nombre_usuario"><?= htmlspecialchars($estudiante['nombre_usuario']); ?></td>
                                <td class="primer_nombre"><?= htmlspecialchars($estudiante['primer_nombre']); ?></td>
                                <td class="segundo_nombre"><?= htmlspecialchars($estudiante['segundo_nombre']); ?></td>
                                <td class="primer_apellido"><?= htmlspecialchars($estudiante['primer_apellido']); ?></td>
                                <td class="segundo_apellido"><?= htmlspecialchars($estudiante['segundo_apellido']); ?></td>
                                <td><?= htmlspecialchars($estudiante['correo']); ?></td>
                                <td class="subprogramas"><?= htmlspecialchars($estudiante['subprogramas']); ?></td>
                                <td><?= htmlspecialchars($estudiante['fecha_creacion']); ?></td>
                                <td>
                                    <button class="btn btn-warning btn-sm me-1" onclick="editEstudiante(<?= $estudiante['id']; ?>)">Editar</button>
                                </td>
                            </tr>
                        <?php endforeach; ?>
                    <?php else: ?>
                        <tr>
                            <td colspan="10" class="text-center">No se encontraron estudiantes registrados.</td>
                        </tr>
                    <?php endif; ?>
                </tbody>
            </table>
        </div>

        <footer class="text-center mt-4">
            <a href="../menu_super_usuario.php" class="btn btn-primary">&larr; Volver al Menú</a>
        </footer>
    </div>

    <!-- Modal para editar estudiante -->
    <div class="modal fade" id="editModal" tabindex="-1" aria-labelledby="editModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="editModalLabel">Editar Estudiante</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <form id="editForm">
                        <input type="hidden" id="editId" name="id">
                        <div class="mb-3">
                            <label for="editNombreUsuario" class="form-label">Nombre de Usuario</label>
                            <input type="text" class="form-control" id="editNombreUsuario" name="nombre_usuario" required>
                        </div>
                        <div class="mb-3">
                            <label for="editContrasena" class="form-label">Contraseña</label>
                            <input type="password" class="form-control" id="editContrasena" name="contrasena" required>
                        </div>
                        <button type="submit" class="btn btn-primary">Guardar Cambios</button>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function editEstudiante(id) {
            const modal = new bootstrap.Modal(document.getElementById('editModal'));
            const row = document.querySelector(`tr[data-id='${id}']`);
            if (!row) {
                console.error(`No se encontró la fila con data-id=${id}`);
                return;
            }
            document.getElementById('editId').value = id;
            document.getElementById('editNombreUsuario').value = row.querySelector('.nombre_usuario').textContent.trim();
            document.getElementById('editContrasena').value = ''; // La contraseña no se muestra por seguridad

            modal.show();
        }

        document.getElementById('editForm').addEventListener('submit', function(event) {
            event.preventDefault();
            const id = document.getElementById('editId').value;
            const nombre_usuario = document.getElementById('editNombreUsuario').value;
            const contrasena = document.getElementById('editContrasena').value;

            fetch('../funcion/GE_funcion.php', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ action: 'update', id, nombre_usuario, contrasena })
            })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Estudiante actualizado correctamente.');
                    location.reload();
                } else {
                    alert('Error al actualizar el estudiante: ' + data.message);
                }
            })
            .catch(error => alert('Error en la solicitud: ' + error.message));
        });
    </script>
</body>
</html>