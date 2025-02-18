<?php
// archivo de configuración de la base de datos
require_once '../includes/config/database.php';

// variables
$nombre = $apellido = $email = $contrasena = $carrera = $semestre = $foto_perfil = "";

// Verifica si el formulario ha sido enviado
if ($_SERVER["REQUEST_METHOD"] == "POST") {
    // Recibe y limpia los datos del formulario
    $nombre = htmlspecialchars(trim($_POST["nombre"]));
    $apellido = htmlspecialchars(trim($_POST["apellido"]));
    $email = htmlspecialchars(trim($_POST["email"]));
    $contrasena = htmlspecialchars(trim($_POST["password"]));
    $carrera = htmlspecialchars(trim($_POST["carrera"]));
    $semestre = htmlspecialchars(trim($_POST["semestre"]));

    // Maneja la subida de la foto de perfil
    if (isset($_FILES["foto_perfil"]) && $_FILES["foto_perfil"]["error"] == 0) {
        $directorio_subida = "../uploads/profiles/";
        if (!is_dir($directorio_subida)) {
            mkdir($directorio_subida, 0777, true);
        }
        $extension = pathinfo($_FILES["foto_perfil"]["name"], PATHINFO_EXTENSION);
        $foto_perfil = $directorio_subida . $nombre . '_' . $apellido . '.' . $extension;
        if (!move_uploaded_file($_FILES["foto_perfil"]["tmp_name"], $foto_perfil)) {
            die("Error al subir el archivo.");
        }
    } else {
        $foto_perfil = "../public/images/profile-default.png";
    }

    // Encripta la contraseña
    $contrasena_hashed = password_hash($contrasena, PASSWORD_BCRYPT);

    // Verifica si el correo ya existe en la base de datos
    $sql_check = "SELECT id FROM usuarios WHERE email = ?";
    if ($stmt_check = $conn->prepare($sql_check)) {
        $stmt_check->bind_param("s", $email);
        $stmt_check->execute();
        $stmt_check->store_result();
        if ($stmt_check->num_rows > 0) {
            echo "<script>alert('El correo electrónico ya está en uso.'); window.location.href = '../index.html';</script>";
        } else {
            // Prepara y ejecuta la consulta SQL para insertar el usuario
            $sql = "INSERT INTO usuarios (nombre, apellido, email, contrasena, carrera, semestre, foto_perfil) VALUES (?, ?, ?, ?, ?, ?, ?)";
            if ($stmt = $conn->prepare($sql)) {
                $stmt->bind_param("sssssss", $nombre, $apellido, $email, $contrasena_hashed, $carrera, $semestre, $foto_perfil);
                if ($stmt->execute()) {
                    // Guardar el estado de registro exitoso en una variable de sesión
                    session_start();
                    $_SESSION["registro_exitoso"] = true;

                    // Redirige al usuario a la página de inicio
                    header("Location: ../index.html");
                    exit();
                } else {
                    echo "Algo salió mal. Por favor, inténtalo de nuevo más tarde.";
                }
                $stmt->close();
            }
        }
        $stmt_check->close();
    }
    $conn->close();
}
?>
