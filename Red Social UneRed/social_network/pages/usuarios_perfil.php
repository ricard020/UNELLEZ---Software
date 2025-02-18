<?php
// Incluir el archivo que maneja la conexión a la base de datos
include("../models/data_perfil.php");

$buttonSelected = 'posts';
if(isset($_POST["tab"]) && $_POST["tab"] == 'comments') $buttonSelected = 'comments';
if(isset($_POST["tab"]) && $_POST["tab"] == 'shared') $buttonSelected = 'shared';

// Verificar si el usuario está accediendo a un perfil específico
if (isset($_GET['usuario_id'])) {
    $usuario_id = $_GET['usuario_id'];

    // Consultar la base de datos para obtener los datos del usuario
    $query = "SELECT * FROM usuarios WHERE id = ?";
    $stmt = $conn->prepare($query);
    $stmt->bind_param("i", $usuario_id);
    $stmt->execute();
    $result = $stmt->get_result();

    if ($result->num_rows > 0) {
        $user = $result->fetch_assoc();
        // Almacenar los datos del usuario en variables
        $nombre_usuario = $user['nombre'];
        $apellido_usuario = $user['apellido'];
        $correo_usuario = $user['email'];
        $foto_perfil = $user['foto_perfil'] ? $user['foto_perfil'] : 'default_profile.jpg';
        $carrera_usuario = $user['carrera'];
        $semestre_usuario = $user['semestre'];
    } else {
        // Si no se encuentra el usuario
        echo "Usuario no encontrado.";
        exit;
    }
} else {
    // Si no se pasa un usuario_id
    echo "No se ha seleccionado un usuario.";
    exit;
}
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <meta name="description" content="Perfil de Usuario">
    <title>Perfil de Usuario</title>
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link rel="stylesheet" href="../public/css/perfil.css">
    <style>


    /* Estilo general para el body */
    body {
        color: white;
    }

    .profile-container {

         padding-bottom: 5rem !important;

    }

    /* Estilo para los botones */
    .btn-custom, .btn-custom-sesion {
        background-color: #ee5d1c; /* Naranja */
        border: none;
        color: white;
        border-radius: 5px;
        padding: 10px 20px;
        font-size: 1rem;
        cursor: pointer;
        transition: background-color 0.3s;
    }

    .btn-custom:hover, .btn-custom-sesion:hover {
        background-color: #d45614; /* Naranja oscuro */
    }

    .btn-custom-sesion {
        background-color: #ee5d1c;
        margin-left: 10px;
    }

    /* Estilo para la imagen de perfil */
    .profile-pic {
        width: 150px;
        height: 150px;
        border-radius: 50%;
        object-fit: cover;
        border: 4px solid #ffffff;
        transition: all 0.3s ease-in-out;
    }

    .profile-pic:hover {
        transform: scale(1.1);
    }

    /* Estilo de los formularios y entradas */
    .form-control {
        background-color: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.3);
        color: white;
        border-radius: 10px;
        padding: 10px;
    }

    .form-control:focus {
        border-color: #ee5d1c;
        box-shadow: 0 0 5px rgba(238, 93, 28, 0.5);
    }

    /* Estilo para las pestañas */
    .tabs .btn {
        background-color: rgba(0, 0, 0, 0.3);
        color: white;
        border-radius: 10px;
        padding: 10px 20px;
        margin-right: 5px;
        font-size: 1rem;
        text-transform: uppercase;
        font-weight: bold;
    }

    .tabs .btn:hover {
        background-color: rgba(0, 0, 0, 0.5);
    }

    .tabs .btn-warning {
        background-color: #ee5d1c !important;
        color: white !important;
    }

    /* Modal de edición */
    .modal-content {
        background-color: #293737;
        border-radius: 15px;
        box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3);
        color: white;
        padding: 20px;
        border: none;
    }

    .modal-header {
        border-bottom: 1px solid rgba(255, 255, 255, 0.3);
    }

    .modal-title {
        font-size: 1.5rem;
        font-weight: bold;
        color: #ee5d1c;
        text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.5);
    }

    .btn-close {
        color: white;
        opacity: 0.8;
    }

    .badge {
        position: absolute;
        top: -5px;
        right: -5px;
        padding: 0.3em 0.7em;
        font-size: 12px;
    }

    /* Estilo para la campana de notificaciones */
    .notification-icon {
        position: relative;
        font-size: 24px;
    }
    
    .notification-icon .badge {
        position: absolute;
        top: -5px;
        right: -5px;
        padding: 0.3em 0.7em;
        font-size: 12px;
    }

    /* Asegurarse de que el contenedor del perfil se vea bien */
    .profile-container {
        background-color: #293737;
        border-radius: 10px;
        padding: 20px;
        margin-right: auto;
        margin-left: auto;
    }

    /* Estilo para el contenedor de select */
    .select-container select {
        background-color: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.3);
        color: white;
        border-radius: 10px;
        padding: 10px;
    }
.notification-icon {
    position: relative;
    font-size: 30px;
}
a {
    color: #fafafa;
    text-decoration: underline;
}
.form-control {
    background-color: rgba(255, 255, 255, 0.1);
    border: 1px solid rgba(255, 255, 255, 0.3);
    color: white;
    border-radius: 10px;
    padding: 10px;
    -webkit-appearance: none; /* Para eliminar la apariencia predeterminada en Safari */
    -moz-appearance: none;    /* Para eliminar la apariencia predeterminada en Firefox */
    appearance: none;         /* Para eliminar la apariencia predeterminada en la mayoría de los navegadores */
}
.select-container {
    position: relative;
    display: inline-block;
}

.select-container select {
    background-color: rgba(255, 255, 255, 0.1);
    border: 1px solid rgba(255, 255, 255, 0.3);
    color: white;
    border-radius: 10px;
    padding: 10px;
    -webkit-appearance: none; /* Eliminar la apariencia predeterminada en Safari */
    -moz-appearance: none;    /* Eliminar la apariencia predeterminada en Firefox */
    appearance: none;         /* Eliminar la apariencia predeterminada en otros navegadores */
}

.d-flex, .btn, .btn2 {
    flex: 1; /* Asegura que los botones ocupen un espacio equitativo */
}

.mb-3 {
    margin-bottom: 1rem !important;
    max-width: 600px;
    margin: auto;
}

.post, .comentarios {
    max-width: 600px !important;
    margin: 0px auto !important;
}

.notification-icon .badge {
    position: absolute;
    top: -5px;
    right: -5px;
    padding: 0.3em 0.7em;
    font-size: 12px;
    
}
     /* Responsivo */
     @media (max-width: 768px) {
        .tabs {
            flex-direction: column;
            align-items: center;
        }

        .tabs .btn {
            width: 80%;
            text-align: center;
        }

        .content-section {
            margin-top: 10px;
        }
    }
    </style>
</head>
<body>
    <!-- Barra lateral izquierda de navegación -->
    <?php include("../includes/partials/navbar.php"); ?>

    <div class="container profile-container" >
        <!-- Imagen de perfil -->
        <img src="<?php echo htmlspecialchars($foto_perfil); ?>" alt="Foto de perfil" class="profile-pic">

        <div class="profile-info">
            <h1><?php echo htmlspecialchars($nombre_usuario . ' ' . $apellido_usuario); ?></h1>
            <p><?php echo htmlspecialchars($correo_usuario); ?></p>
            <p class="user-bio">¡Hola! soy <?php echo htmlspecialchars($nombre_usuario); ?>, estudiante de <?php echo htmlspecialchars($carrera_usuario); ?>, semestre <?php echo htmlspecialchars($semestre_usuario); ?>. Bienvenid@ a mi perfil</p>
        </div>
        <form method="POST" class="tabs pb-3">
            <button type="submit" name="tab" value="posts" class="btn <?php if($buttonSelected == 'posts') echo 'btn-warning text-white' ?> tab">Publicaciones</button>
            <button type="submit" name="tab" value="comments" class="btn <?php if($buttonSelected == 'comments') echo 'btn-warning text-white' ?> tab">Comentarios</button>
            <button type="submit" name="tab" value="shared" class="btn <?php if($buttonSelected == 'shared') echo 'btn-warning text-white' ?> tab">Compartidos</button>
        </form>

        <!-- Incluir los posts -->
        <?php
            if($buttonSelected == 'comments') {
                include_once("../models/comment_user_perfil.php");
            } elseif($buttonSelected == 'shared') {
                include_once("../models/porst_retweet_perfil.php");
            } else {
                include_once("../models/post_user_perfil.php");
            }
        ?>
    </div>

    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
