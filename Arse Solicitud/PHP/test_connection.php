<?php
// Archivo: /php/test_connection.php

// Importar el archivo de conexión
require_once './bd_register.php';

try {
    // Intentar una consulta simple para verificar la conexión
    $stmt = $pdo->query("SELECT 1");
    if ($stmt) {
        echo "Conexión a la base de datos exitosa.";
    } else {
        echo "Conexión establecida, pero la consulta falló.";
    }
} catch (PDOException $e) {
    // Mostrar errores si ocurre algún problema
    echo "Error al realizar la prueba de conexión: " . $e->getMessage();
}
