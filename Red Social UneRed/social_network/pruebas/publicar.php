<?php
// Inicia la sesión
    session_start();

    // Verifica si el usuario ha iniciado sesión, de lo contrario, redirige a la página de inicio de sesión
    if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
        header("Location: ../../index.html");
        exit;
    }

    // configuración de la base de datos
    require_once '../includes/config/database.php';
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Publicar Proyecto - UNERED</title>
    <!-- Enlace a Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <!-- Enlace a los iconos de Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons/font/bootstrap-icons.css" rel="stylesheet">
    <!-- Tu archivo CSS personalizado -->
    <link rel="stylesheet" href="styles.css">
</head>
<body>

<?php include("../includes/partials/navbar.php"); ?>
    <div class="container my-5">
        <h1 class="text-center mb-4">Publicar Proyecto</h1>
        <form action="./procesar_publicacion.php" method="POST" enctype="multipart/form-data" class="border p-4 rounded shadow-sm">
            <div class="mb-3">
                <label for="titulo" class="form-label">Título del Proyecto</label>
                <input type="text" name="titulo" id="titulo" class="form-control" placeholder="Título del Proyecto" required>
            </div>
            <div class="mb-3">
                <label for="descripcion" class="form-label">Descripción</label>
                <textarea name="descripcion" id="descripcion" class="form-control" placeholder="Describe tu proyecto..." rows="4" required></textarea>
            </div>
            <div class="mb-3">
                <label for="categorias" class="form-label">Selecciona Categorías</label>
                <select name="categorias[]" id="categorias" class="form-select" multiple required>
                    <?php
                    $query = "SELECT * FROM categorias";
                    $result = $conn->query($query);
                    while ($row = $result->fetch_assoc()) {
                        echo "<option value='" . $row['id'] . "'>" . $row['nombre'] . "</option>";
                    }
                    ?>
                </select>
            </div>
            <div class="mb-3">
                <label for="imagenes" class="form-label">Subir Imágenes</label>
                <input type="file" name="imagenes[]" id="imagenes" class="form-control" multiple accept="image/*">
            </div>
            <div class="mb-3">
                <label for="archivos" class="form-label">Subir Archivos Adjuntos</label>
                <input type="file" name="archivos[]" id="archivos" class="form-control" multiple>
            </div>
            <button type="submit" class="btn btn-primary w-100">Publicar <i class="bi bi-upload"></i></button>
        </form>
    </div>

    <!-- Enlace a Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js" integrity="sha384-pzjw8f+ua7Kw1TIq0J5Lg7p1gD6EO0bX8hFwkpDbZXlDhGspgMjtcYyT1fYzxI8v" crossorigin="anonymous"></script>
</body>
</html>
