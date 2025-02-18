<?php
// Conectar a la base de datos
$servername = "localhost"; // 
$username = "root"; // El usuario siempre es root
$password = ""; // Si tienes tu bd con contraseña, colócala aquí
$dbname = "unered"; // Nombre de la Base de Datos (cambiar si lo tienes con otro nombre)

// Crear conexión
$conn = new mysqli($servername, $username, $password, $dbname);

// Verificar conexión
if ($conn->connect_error) {
    die("Conexión fallida: " . $conn->connect_error);
}

// Iniciar la sesión para obtener el ID del usuario actual

$usuario_id = $_SESSION['id'];

// Consulta para contar las notificaciones no leídas
$query = "SELECT COUNT(*) AS total_notificaciones FROM notificaciones WHERE usuario_id = ? AND leido = 0";

// Preparar la consulta
$stmt = $conn->prepare($query);
$stmt->bind_param("i", $usuario_id); // "i" para integer (tipo de la variable $usuario_id)
$stmt->execute();

// Obtener el resultado
$result = $stmt->get_result();
$row = $result->fetch_assoc();

// Obtener el total de notificaciones no leídas
$total_notificaciones = $row['total_notificaciones'];

// Cerrar la conexión
$stmt->close();
$conn->close();
?>
