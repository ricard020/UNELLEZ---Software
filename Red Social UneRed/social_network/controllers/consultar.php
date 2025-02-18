<?php
// Conexión a la base de datos
include ("../includes/config/database.php");

$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    die("Conexión fallida: " . $conn->connect_error);
}

// Obtener el filtro y el término de búsqueda desde la solicitud AJAX
$filter = $_GET['filter'];
$query = $_GET['query'];

// Inicializar la respuesta como un array vacío
$response = [];

if ($filter == 'projects') {
    // Consulta para obtener proyectos según el término de búsqueda
    $sql = "SELECT * FROM proyectos WHERE titulo LIKE '%$query%' OR descripcion LIKE '%$query%'";
    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        while($row = $result->fetch_assoc()) {
            $response[] = $row;
        }
    } else {
        $response[] = "No se encontraron proyectos.";
    }
} elseif ($filter == 'categories') {
    // Consulta para obtener categorías
    $sql = "SELECT id, nombre AS nombre_categoria FROM categorias WHERE nombre LIKE '%$query%'";
    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        while ($row = $result->fetch_assoc()) {
            $response[] = $row; // Agregar cada categoría al array de respuesta
        }
    } else {
        $response[] = ["error" => "No se encontraron categorías."];
    }

} elseif ($filter == 'users') {
    // Consulta para obtener usuarios
    $sql = "SELECT id, nombre, apellido, foto_perfil FROM usuarios WHERE nombre LIKE '%$query%' OR apellido LIKE '%$query%'";
    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        while($row = $result->fetch_assoc()) {
            $response[] = $row;
        }
    } else {
        $response[] = "No se encontraron usuarios.";
    }
} elseif ($filter == 'retweets') {
    // Consulta para obtener retweets
    $sql = "SELECT * FROM retweets WHERE contenido LIKE '%$query%'";
    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        while($row = $result->fetch_assoc()) {
            $response[] = $row;
        }
    } else {
        $response[] = "No se encontraron retweets.";
    }
} elseif ($filter == 'valuated') {
    // Consulta para obtener proyectos valorados
    $sql = "SELECT * FROM proyectos WHERE valoracion >= 4 AND (titulo LIKE '%$query%' OR descripcion LIKE '%$query%')";
    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        while($row = $result->fetch_assoc()) {
            $response[] = $row;
        }
    } else {
        $response[] = "No se encontraron proyectos valorados.";
    }
} else {
    // Si el filtro es "all", muestra todo
    $sql = "SELECT * FROM proyectos WHERE titulo LIKE '%$query%' OR descripcion LIKE '%$query%'";
    $result = $conn->query($sql);

    if ($result->num_rows > 0) {
        while($row = $result->fetch_assoc()) {
            $response[] = $row;
        }
    } else {
        $response[] = "No se encontraron resultados.";
    }
}

// Devolver los resultados en formato JSON
echo json_encode($response);

$conn->close();
?>
