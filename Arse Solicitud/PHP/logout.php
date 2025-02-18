<?php
// Iniciar sesión
session_start();

// Destruir todas las variables de sesión
$_SESSION = [];

// Destruir la sesión
session_destroy();

// Redirigir al inicio de sesión
header('Location: ../index.html');
exit;
?>
