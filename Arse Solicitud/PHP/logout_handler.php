<?php
// Iniciar sesión
session_start();

// Verificar si hay una sesión activa
if (isset($_SESSION['usuario_id'])) {
    // Destruir todas las variables de sesión
    $_SESSION = [];
    
    // Destruir la sesión
    session_destroy();
}

// Redirigir al inicio de sesión (index.html)
header('Location: ../index.html');
exit;
?>
