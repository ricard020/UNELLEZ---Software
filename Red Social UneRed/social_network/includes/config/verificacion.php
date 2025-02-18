<?php
    // Inicia la sesión
    session_start();

    // Verifica si el usuario ha iniciado sesión, de lo contrario, redirige a la página de inicio de sesión
    if (!isset($_SESSION["loggedin"]) || $_SESSION["loggedin"] !== true) {
        header("Location: ../../index.html");
        exit;
    }

    // archivo de configuración de la base de datos
    require_once '../includes/config/database.php';

    // Obtiene el nombre de usuario de la sesión
    $email_usuario = $_SESSION["nombre_usuario"];  // En la sesión estamos almacenando el email

    // Consulta para obtener los datos del usuario (nombre y apellido)
    $query = "SELECT nombre, apellido FROM usuarios WHERE email = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("s", $email_usuario);  // Usamos el email de la sesión
    $stmt->execute();
    $result = $stmt->get_result();
    $user = $result->fetch_assoc();

    // Verifica si el usuario existe
    if (!$user) {
        header("location: ../index.html");
        exit;
    }

    // Asignar los valores del usuario a las variables
    $nombre_usuario = $user['nombre']; 
    $apellido_usuario = $user['apellido'];

    // Crear el nombre completo del usuario
    $nombre_completo = $nombre_usuario . " " . $apellido_usuario;

?>