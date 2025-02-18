<?php
// Archivo: /php/db_connection.php

// Configuración de conexión
$host = '127.0.0.1'; // Dirección del servidor
$port = '3306';      // Puerto de conexión
$dbname = 'bduni';   // Nombre de la base de datos
$username = 'root';  // Usuario de la base de datos
$password = '';      // Contraseña del usuario

try {
    // Crear la conexión usando PDO
    $pdo = new PDO("mysql:host=$host;port=$port;dbname=$dbname;charset=utf8mb4", $username, $password);

    // Configurar opciones de PDO
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION); // Manejo de errores
    $pdo->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC); // Modo de fetch predeterminado
} catch (PDOException $e) {
    // Manejo de errores de conexión
    die("Error al conectar a la base de datos: " . $e->getMessage());
}
?>
