<?php
// archivo de configuración de la base de datos
require_once '../includes/config/database.php';

// variables
$nombre_usuario = $contrasena = "";
$nombre_usuario_err = $contrasena_err = "";

// Verifica si el formulario ha sido enviado
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Recibe y limpia los datos del formulario
    if (empty(trim($_POST["username"]))) {
        $nombre_usuario_err = "Por favor, ingrese su nombre de usuario.";
    } else {
        $nombre_usuario = trim($_POST["username"]);
    }

    if (empty(trim($_POST["contrasena"]))) {
        $contrasena_err = "Por favor, ingrese su contraseña.";
    } else {
        $contrasena = trim($_POST["contrasena"]);
    }

    // Valida las credenciales
    if (empty($nombre_usuario_err) && empty($contrasena_err)) {
        // Prepara la consulta SQL
        $sql = "SELECT id, email, contrasena FROM usuarios WHERE email = ?";
        
        if ($stmt = $conn->prepare($sql)) {
            $stmt->bind_param("s", $param_nombre_usuario);
            $param_nombre_usuario = $nombre_usuario;
            
            if ($stmt->execute()) {
                $stmt->store_result();
                
                // Verifica si el nombre de usuario existe
                if ($stmt->num_rows == 1) {
                    $stmt->bind_result($id, $nombre_usuario, $hashed_password);
                    if ($stmt->fetch()) {
                        if (password_verify($contrasena, $hashed_password)) {
                            // Si la contraseña es correcta, inicia una nueva sesión
                            session_start();
                            $_SESSION["loggedin"] = true;
                            $_SESSION["id"] = $id;
                            $_SESSION["nombre_usuario"] = $nombre_usuario;

                            // Redirige al usuario a la página principal
                            header("Location: ../pages/perfil.php");
                        } else {
                            // La contraseña no es válida
                            $contrasena_err = "La contraseña que has ingresado no es válida.";
                            header("Location: ../index.html");
                        }
                    }
                } else {
                    // El nombre de usuario no existe
                    $nombre_usuario_err = "No se encontró ninguna cuenta con ese nombre de usuario.";
                    header("Location: ../index.html");
                }
            } else {
                echo "Algo salió mal. Por favor, inténtalo de nuevo más tarde.";
                header("Location: ../index.html");
            }
            $stmt->close();
        }
    }
    $conn->close();
}
?>
