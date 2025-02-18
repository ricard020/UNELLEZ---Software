<?php

include("../models/data_perfil.php");

$buttonSelected = 'posts';
if(isset($_POST["tab"]) && $_POST["tab"] == 'comments') $buttonSelected = 'comments';
if(isset($_POST["tab"]) && $_POST["tab"] == 'shared') $buttonSelected = 'shared';

?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
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

    .btn-link {

        --bs-link-color: #ee5d1c;
        --bs-btn-hover-color: #ee5d1c;
        --bs-btn-active-color: #ee5d1c;
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
        padding-bottom: 5rem !important;
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
    font-size: 40px;
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
/* Cambios responsive */
@media (max-width: 767.5px)  {
    .navbar {
        top: auto;
        bottom: 0; /* Mueve el navbar a la parte inferior */
        left: 0;
        height: 70px; /* Reduce la altura del navbar */
        width: 100%; /* Ocupa todo el ancho de la pantalla */
        flex-direction: row !important; /* Cambia a una fila horizontal */
        justify-content: space-around; /* Espaciado uniforme entre ítems */
        align-items: center;
        padding: 0;
    }

    .navbar-nav {
        width: 100%;
        padding: 0;
        margin: 0;
        list-style: none;
        display: flex;
        flex-direction: row !important;
    }
    .main-content {
        margin-left: 0 !important; /* Elimina el margen en móviles */
        margin-bottom: 80px !important; /* Espacio para la barra en la parte inferior */
    }

    .nav-item {
        margin: 0; /* Elimina márgenes extra */
    }

    .nav-link {
        font-size: 1.8rem; /* Íconos ligeramente más pequeños en móviles */
    }
}

    </style>

</head>
<body>
    <script>
        if ( window.history.replaceState ) {
            window.history.replaceState( null, null, window.location.href );
        }
    </script>
    <!-- Barra lateral izquierda de navegación -->
    <?php include("../includes/partials/navbar.php"); 
            include("../models/number_notification.php");
    ?>
    
    <div class="container profile-container">
        <!-- Imagen de perfil (clicable para actualizar) -->
        <form action="" method="POST" enctype="multipart/form-data">
            <label for="new_profile_picture">
                <img src="<?php echo htmlspecialchars($foto_perfil); ?>" alt="Foto de perfil" class="profile-pic" style="cursor: pointer;">
            </label>
            <input type="file" id="new_profile_picture" name="new_profile_picture" style="display: none;" onchange="this.form.submit()">
        </form>

        
        <div class="profile-info">
            <h1><?php echo htmlspecialchars($nombre_usuario . ' ' . $apellido_usuario); ?></h1>
            <p><?php echo htmlspecialchars($correo_usuario); ?></p>
            <p class="user-bio">¡Hola! soy <?php echo htmlspecialchars($nombre_usuario); ?>, estudiante de <?php echo htmlspecialchars($carrera_usuario); ?>, semestre <?php echo htmlspecialchars($semestre_usuario); ?>. Bienvenid@ a mi perfil.</p>

            <div class="profile-buttons">
                <!-- Botón para abrir la ventana modal de edición -->
                <button class="btn-custom" data-bs-toggle="modal" data-bs-target="#editProfileModal">
                    <i class="bi bi-pencil-square btn-icon"></i>Editar perfil
                </button> 

                <!-- Botón de cierre de sesión -->
                <button class="btn-custom-sesion" onclick="location.href='../includes/config/logout.php'">
                    <i class="bi bi-box-arrow-right btn-icon"></i>Cerrar sesión
                </button>
                
                <!-- Icono de campana con notificaciones no leídas -->
                <a href="./notificaciones.php" class="notification-icon">
                    <i class="bi bi-bell"></i>
                    <?php if ($total_notificaciones > 0): ?>
                        <span class="badge bg-danger"><?php echo $total_notificaciones; ?></span>
                    <?php endif; ?>
                </a>
            </div>
        </div>
        <form method="POST" class="tabs pb-3">
            <button type="submit" name="tab" value="posts" class="btn <?php if($buttonSelected == 'posts') echo 'btn-warning text-white' ?> tab">Publicaciones</button>
            <button type="submit" name="tab" value="comments" class="btn <?php if($buttonSelected == 'comments') echo 'btn-warning text-white' ?> tab">Comentarios</button>
            <button type="submit" name="tab" value="shared" class="btn <?php if($buttonSelected == 'shared') echo 'btn-warning text-white' ?> tab">Compartidos</button>
        </form>

        <!-- Incluir los posts -->
        <?php
            if($buttonSelected == 'comments') {
                include_once("../models/comment_my_user.php");
            } elseif($buttonSelected == 'shared') {
                include_once("../models/post_shared.php");
            } else {
                include_once("../models/post_my_user.php");
            }
        ?>
    </div>

<!-- Modal de Edición de Perfil -->
<div style="background: rgba(0, 0, 0, 0.85); color: white;" class="modal fade" id="editProfileModal" tabindex="-1" aria-labelledby="editProfileModalLabel" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content" style="background-color: #293737; border-radius: 15px; box-shadow: 0 8px 20px rgba(0, 0, 0, 0.3); color: white; padding: 20px; border: none;">
            <div class="modal-header" style="border-bottom: 1px solid rgba(255, 255, 255, 0.3);">
                <h5 class="modal-title" id="editProfileModalLabel" style="font-size: 1.5rem; font-weight: bold; color: #ee5d1c; text-shadow: 1px 1px 3px rgba(0, 0, 0, 0.5);">Editar Perfil</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" style="color: white; opacity: 0.8;"></button>
            </div>
            <div class="modal-body">
                <!-- Formulario para editar los datos -->
                <form action="" method="POST">
                    <div class="mb-3">
                        <label for="nombre" class="form-label" style="font-size: 1rem; color: white;">Nombre</label>
                        <input type="text" class="form-control" id="nombre" name="nombre" value="<?php echo htmlspecialchars($nombre_usuario); ?>" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
                    </div>
                    <div class="mb-3">
                        <label for="apellido" class="form-label" style="font-size: 1rem; color: white;">Apellido</label>
                        <input type="text" class="form-control" id="apellido" name="apellido" value="<?php echo htmlspecialchars($apellido_usuario); ?>" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
                    </div>
                    <div class="mb-3">
                        <label for="email" class="form-label" style="font-size: 1rem; color: white;">Correo Electrónico</label>
                        <input type="email" class="form-control" id="email" name="email" value="<?php echo htmlspecialchars($correo_usuario); ?>" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
                    </div>
                    <div class="mb-3">
                        <label for="contrasena" class="form-label" style="font-size: 1rem; color: white;">Nueva Contraseña</label>
                        <input type="password" class="form-control" id="contrasena" name="contrasena" value="<?php echo htmlspecialchars($contrasena_usuario); ?>" style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
                        <small class="form-text" style="color: #dcdcdc;">Dejar en blanco si no desea cambiar la contraseña.</small>
                    </div>
                    <div class="mb-3">
                        <label for="carrera" class="form-label" style="font-size: 1rem; color: white;">Carrera</label>
                        <input type="text" class="form-control" id="carrera" name="carrera" value="<?php echo htmlspecialchars($carrera_usuario); ?>" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
                    </div>
                    <div class="mb-3">
                        <label for="semestre" class="form-label" style="font-size: 1rem; color: white;">Semestre</label>
                        <select class="form-control" id="semestre" name="semestre" required style="background-color: rgba(255, 255, 255, 0.1); border: 1px solid rgba(255, 255, 255, 0.3); color: white; border-radius: 10px; padding: 10px;">
                            <option value="I" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'I') ? 'selected' : ''; ?>>I</option> 
                            <option value="II" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'II') ? 'selected' : ''; ?>>II</option>
                            <option value="III" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'III') ? 'selected' : ''; ?>>III</option>
                            <option value="IV" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'IV') ? 'selected' : ''; ?>>IV</option>
                            <option value="V" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'V') ? 'selected' : ''; ?>>V</option>
                            <option value="VI" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'VI') ? 'selected' : ''; ?>>VI</option>
                            <option value="VII" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'VII') ? 'selected' : ''; ?>>VII</option>
                            <option value="VIII" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'VIII') ? 'selected' : ''; ?>>VIII</option>
                            <option value="IX" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'IX') ? 'selected' : ''; ?>>IX</option>
                            <option value="X" style="background-color: #3f4b4b; color: white;" <?php echo ($semestre_usuario == 'X') ? 'selected' : ''; ?>>X</option>
                        </select>
                    </div>
                    <button type="submit" name="update_profile" class="btn" style="background-color: #ee5d1c; border: none; color: white; padding: 10px 20px; border-radius: 20px; font-size: 1rem; font-weight: bold; cursor: pointer; display: block; margin: 0 auto; text-align: center;">Guardar cambios</button>
                </form>
            </div>
        </div>
    </div>
</div>



    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>