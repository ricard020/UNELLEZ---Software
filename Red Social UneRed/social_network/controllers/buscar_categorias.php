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

// Consultar categorías en la base de datos
$sql = "SELECT id, nombre FROM categorias WHERE nombre LIKE '%$searchText%'";

$result = $conn->query($sql);

$categorias = [];
if ($result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        $categorias[] = $row;
    }
}

// Devolver los resultados como JSON
header('Content-Type: application/json');
echo json_encode($categorias);

$conn->close();
?>