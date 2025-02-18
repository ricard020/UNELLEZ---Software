<?php

// Verificacion de errores
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);

// Configuración de la conexión a la base de datos
include('../includes/config/database.php');

// Crear conexión
$conn = new mysqli($servername, $username, $password, $dbname);

// Verificar conexión
if ($conn->connect_error) {
    die("Conexión fallida: " . $conn->connect_error);
}

// Obtener el texto de búsqueda desde la solicitud AJAX
$searchText = isset($_GET['query']) ? $conn->real_escape_string($_GET['query']) : '';

// Consultar proyectos en la base de datos
$sql = "SELECT id AS post_id, titulo, descripcion FROM proyectos WHERE titulo LIKE '%$searchText%' OR descripcion LIKE '%$searchText%'";

$result = $conn->query($sql);

$proyectos = [];
if ($result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        $proyectos[] = $row;
    }
}

// Devolver los resultados como JSON
header('Content-Type: application/json');
echo json_encode($proyectos);

$conn->close();
?>
