<?php
$servername = "localhost"; // 
$username = "root"; // El usuairo siempre es root
$password = ""; // Si tienes tu bd con contrasena colocala aqui, auqnue MySQL siempre viene sin contrasena
$dbname = "unered"; // Nombre de la Base de Datos (cambiar si lo tienes con otro nombre)

// Crear conexión
$conn = new mysqli($servername, $username, $password, $dbname);

// Verificar conexión
if ($conn->connect_error) {
    die("Conexión fallida: " . $conn->connect_error);
}
?>
