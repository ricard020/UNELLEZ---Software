<?php
session_start();
require_once '../../PHP/bd_conexion.php';

// Verificar si el usuario está logueado y es un jefe de subprograma
if (!isset($_SESSION['user_id']) || $_SESSION['user_role'] !== 'JP') {
    header('Location: ../../PHP/logout.php');
    exit;
}

// Manejar la solicitud POST para actualizar los datos
if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $usuario_id = $_SESSION['user_id'];
    $primer_nombre = $_POST['primer_nombre'];
    $primer_apellido = $_POST['primer_apellido'];
    $correo = $_POST['correo'];

    // Verificar si el correo ya está en uso
    $stmt = $pdo->prepare("SELECT COUNT(*) FROM usuarios WHERE correo = :correo AND id != :id");
    $stmt->execute([
        'correo' => $correo,
        'id' => $usuario_id
    ]);
    $correo_en_uso = $stmt->fetchColumn();

    if ($correo_en_uso > 0) {
        $error = "El correo ya está en uso por otro usuario.";
    } else {
        $stmt = $pdo->prepare("UPDATE usuarios SET primer_nombre = :primer_nombre, primer_apellido = :primer_apellido, correo = :correo WHERE id = :id");
        $stmt->execute([
            'primer_nombre' => $primer_nombre,
            'primer_apellido' => $primer_apellido,
            'correo' => $correo,
            'id' => $usuario_id
        ]);

        header('Location: ../perfil_jefesubprograma.php');
        exit;
    }
}
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Completar Datos</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body class="bg-light">
    <div class="container py-5">
        <div class="row">
            <div class="col-lg-6 offset-lg-3">
                <div class="card">
                    <div class="card-header bg-primary text-white">
                        <h5 class="card-title">Completar Datos</h5>
                    </div>
                    <div class="card-body">
                        <?php if (isset($error)): ?>
                            <div class="alert alert-danger">
                                <?php echo $error; ?>
                            </div>
                        <?php endif; ?>
                        <form action="completar_datos.php" method="POST">
                            <!-- Primer Nombre -->
                            <div class="mb-3">
                                <label for="primer_nombre" class="form-label">Nombre</label>
                                <input type="text" id="primer_nombre" name="primer_nombre" class="form-control" required>
                            </div>
                            <!-- Primer Apellido -->
                            <div class="mb-3">
                                <label for="primer_apellido" class="form-label">Apellido</label>
                                <input type="text" id="primer_apellido" name="primer_apellido" class="form-control" required>
                            </div>

                            <!-- Correo -->
                            <div class="mb-3">
                                <label for="correo" class="form-label">Correo</label>
                                <input type="email" id="correo" name="correo" class="form-control" required>
                            </div>
                            <!-- Botones -->
                            <div class="d-flex justify-content-between">
                            <a href="../../php/logout.php" class="btn btn-danger">Cancelar</a>
                                <button type="submit" class="btn btn-primary">Guardar</button>

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