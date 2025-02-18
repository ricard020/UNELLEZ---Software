<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Crear Cuenta de Jefe de Subprograma</title>
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
</head>

<body class="bg-light">
    <div class="container py-5">
        <div class="row">
            <div class="col-lg-6 offset-lg-3">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h5 class="card-title">Crear Cuenta de Jefe de Subprograma</h5>
                    </div>
                    <div class="card-body">
                        <form action="../funcion/CJS_funcion.php" method="POST">
                            <!-- Nombre de Usuario -->
                            <div class="mb-3">
                                <label for="nombre_usuario" class="form-label">Nombre de Usuario</label>
                                <input type="text" id="nombre_usuario" name="nombre_usuario" class="form-control" required>
                            </div>
                            <!-- Contraseña -->
                            <div class="mb-3">
                                <label for="contrasena" class="form-label">Contraseña</label>
                                <input type="password" id="contrasena" name="contrasena" class="form-control" required>
                            </div>
                            <!-- Seleccionar Subprograma -->
                            <div class="mb-3">
                                <label for="subprograma" class="form-label">Subprograma</label>
                                <select id="subprograma" name="subprograma" class="form-select" required>
                                    <?php
                                    // Obtener los subprogramas desde la base de datos
                                    require_once '../../PHP/bd_conexion.php';
                                    $stmt = $pdo->query("SELECT id, nombre_subprograma FROM subprogramas");
                                    while ($row = $stmt->fetch()) {
                                        echo "<option value='{$row['id']}'>{$row['nombre_subprograma']}</option>";
                                    }
                                    ?>
                                </select>
                            </div>
                            <!-- Botones -->
                            <div class="d-flex justify-content-between">
                                <a href="../menu_super_usuario.php" class="btn btn-secondary">Volver</a>
                                <button type="submit" class="btn btn-primary">Crear Cuenta</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>