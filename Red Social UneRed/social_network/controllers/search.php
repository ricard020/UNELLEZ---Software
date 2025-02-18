<?php
// Conectar a la base de datos
$host = 'localhost'; // Cambiar si es necesario
$db = 'unered';
$user = 'root'; // Cambiar con tus credenciales
$pass = ''; // Cambiar con tus credenciales
$charset = 'utf8mb4';

$dsn = "mysql:host=$host;dbname=$db;charset=$charset";
$options = [
    PDO::ATTR_ERRMODE            => PDO::ERRMODE_EXCEPTION,
    PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC,
    PDO::ATTR_EMULATE_PREPARES   => false,
];

try {
    $pdo = new PDO($dsn, $user, $pass, $options);
} catch (\PDOException $e) {
    throw new \PDOException($e->getMessage(), (int)$e->getCode());
}

// Recibir los parámetros de búsqueda desde JavaScript
$query = $_GET['query'];
$category = $_GET['category'];

// Construir la consulta SQL según la categoría seleccionada
if ($category === 'usuarios') {
    $sql = "SELECT * FROM usuarios WHERE nombre LIKE :query OR apellido LIKE :query";
} elseif ($category === 'categorias') {
    $sql = "SELECT * FROM categorias WHERE nombre LIKE :query";
} elseif ($category === 'proyectos') {
    $sql = "SELECT * FROM proyectos WHERE titulo LIKE :query OR descripcion LIKE :query";
} elseif ($category === 'valoraciones') {
    $sql = "SELECT * FROM valoraciones WHERE valoracion LIKE :query";
} else {
    $sql = ""; // En caso de que no sea una categoría válida
}

$stmt = $pdo->prepare($sql);
$stmt->execute(['query' => "%$query%"]);

$results = $stmt->fetchAll();

echo json_encode($results);
?>
