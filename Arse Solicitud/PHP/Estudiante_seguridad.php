<?php
// Archivo: /php/auth.php
session_start(); // Iniciar sesión

// Verificar si el usuario está autenticado
if (!isset($_SESSION['user_id'])) {
    // Redirigir al login si no está autenticado
    header("Location: ../index.html");
    exit();
}

// Función para verificar el rol del usuario
function verificarRol($rolRequerido) {
    if ($_SESSION['user_role'] !== $rolRequerido) {
        // Redirigir si el rol no coincide
        header("Location: ../html/acceso_denegado.html");
        exit();
    }
}
