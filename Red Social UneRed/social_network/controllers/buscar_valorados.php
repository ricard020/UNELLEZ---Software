<?php
// Archivo: buscar_valorados.php

// Mostrar errores para depuración (solo en desarrollo)
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);

// Configuración de la conexión a la base de datos
include('../includes/config/database.php');

// Crear la conexión a la base de datos
$conn = new mysqli($servername, $username, $password, $dbname);

// Verificar si la conexión fue exitosa
if ($conn->connect_error) {
    die("Conexión fallida: " . $conn->connect_error);
}

// Obtener el texto de búsqueda desde la solicitud AJAX
$searchText = isset($_GET['query']) ? $conn->real_escape_string($_GET['query']) : '';

// Consulta SQL para obtener proyectos con la cantidad de "Me gusta", ordenados de mayor a menor
$sql = "SELECT p.id, p.titulo, p.descripcion, 
                COUNT(v.id) AS like_count
        FROM proyectos p
        LEFT JOIN valoraciones v ON p.id = v.proyecto_id
        WHERE p.titulo LIKE '%$searchText%' AND v.valoracion = 'Me gusta'
        GROUP BY p.id
        ORDER BY like_count DESC";

// Ejecutar la consulta
$result = $conn->query($sql);

// Inicializar el arreglo de proyectos
$proyectos = [];

if ($result && $result->num_rows > 0) {
    while ($row = $result->fetch_assoc()) {
        $proyectos[] = [
            'id' => $row['id'], // ID del proyecto
            'titulo' => $row['titulo'],
            'descripcion' => $row['descripcion'],
            'like_count' => $row['like_count'],
            'post_id' => $row['id'] // Duplicamos el ID como post_id
        ];
    }
}

// Retornar los resultados como JSON
header('Content-Type: application/json');
echo json_encode($proyectos);

// Cerrar la conexión
$conn->close();
?>
