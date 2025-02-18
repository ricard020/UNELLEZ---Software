<?php
// Inicia la sesión
session_start();

// Verifica si el usuario ha iniciado sesión
if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
    header("Location: ../../index.html");
    exit;
}

// Conexión a la base de datos
require_once '../includes/config/database.php';

if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Verifica si el campo de categoría no está vacío
    if (isset($_POST['nombre']) && !empty($_POST['nombre'])) {
        $nombre_categoria = $_POST['nombre'];

        // Prepara la consulta para insertar la categoría
        $query = "INSERT INTO categorias (nombre) VALUES (?)";
        if ($stmt = $conn->prepare($query)) {
            // Vincula el parámetro
            $stmt->bind_param("s", $nombre_categoria);

            // Ejecuta la consulta
            if ($stmt->execute()) {
                // Redirige a la página de éxito o muestra un mensaje
                header("Location: categorias_exito.php"); // Redirección a una página de éxito
                exit;
            } else {
                echo "Error al agregar la categoría: " . $stmt->error;
            }

            // Cierra la declaración
            $stmt->close();
        } else {
            echo "Error en la preparación de la consulta: " . $conn->error;
        }
    } else {
        echo "Por favor, ingrese un nombre de categoría.";
    }
}

?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Agregar Categoría - UNERED</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <div class="container my-5">
        <h1 class="text-center mb-4">Agregar Nueva Categoría</h1>
        <form action="agregar_categoria.php" method="POST" class="border p-4 rounded shadow-sm">
            <div class="mb-3">
                <label for="nombre" class="form-label">Nombre de la Categoría</label>
                <input type="text" name="nombre" id="nombre" class="form-control" placeholder="Nombre de la categoría" required>
            </div>
            <button type="submit" class="btn btn-primary w-100">Agregar Categoría</button>
        </form>
        <a href="index.php" class="btn btn-secondary mt-3">Volver al Inicio</a>
    </div>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha1/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
