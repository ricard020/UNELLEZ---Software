<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Error</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <div class="container mt-5">
        <div class="alert alert-danger">
            <?php if (isset($_GET['error'])): ?>
                <?= htmlspecialchars($_GET['error']); ?>
            <?php else: ?>
                Ha ocurrido un error desconocido.
            <?php endif; ?>
        </div>
        <a href="../perfil_estudiante.php" class="btn btn-primary">Volver al perfil</a>
    </div>
</body>
</html>