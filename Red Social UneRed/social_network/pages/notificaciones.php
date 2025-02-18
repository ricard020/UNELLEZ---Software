

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Notificaciones</title>
    
    <!-- Bootstrap CSS -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0-alpha3/dist/css/bootstrap.min.css" rel="stylesheet">
    <style>
        .container {
            max-width: 800px;
            margin: 20px auto;
            background: #293737;
            color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }
        .commentProfileCard > a {
            color: #ee5d1c;
            text-decoration: none;
        }
        .comentario {
            background: #4a5757;
            color: #fff;
        }
        .notificacion {
            display: flex;
            flex-direction: column;
            border-bottom: 1px solid #ddd;
            padding: 15px 0;
        }
        .notificacion:last-child {
            border-bottom: none;
        }
        .cabecera {
            display: flex;
            align-items: center;
        }
        .perfil {
            width: 50px;
            height: 50px;
            border-radius: 50%;
            object-fit: cover;
            margin-right: 15px;
        }
        .nombre-usuario {
            font-weight: bold;
            font-size: 1rem;
            margin: 0;
        }
        .cuerpo {
            display: flex;
            justify-content: space-between;
            margin-top: 10px;
            align-items: center;
        }
        .mensaje {
            flex-grow: 1;
            font-size: 1rem;
            color: #555;
            margin: 0 15px;
        }
        .icono {
            font-size: 2rem;
            color: #007bff;
        }
        .fecha {
            margin-top: 10px;
            font-size: 0.85rem;
            color: #888;
            text-align: right;
        }
        .nuevo {
            color: #ee5d1c;
            font-weight: bold;
            font-size: 0.85rem;
            margin-left: 5px;
        }
        .notificationOrangeColor, .notificationOrangeColor > * {
            color: #ee5d1c;
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

        <?php include("../controllers/control_notificaciones.php");?>
        <?php include("../includes/partials/navbar.php"); ?>
        

        <div class="container">
    <h1 class="text-center mb-4">Notificaciones</h1>
    <?php if (empty($notificaciones)): ?>
        <div class="alert alert-info text-center" role="alert">
            <i class="bi bi-info-circle"></i> No tienes notificaciones nuevas.
        </div>
    <?php else: ?>
        <div class="list-group">
            <?php foreach ($notificaciones as $notificacion): ?>
                <div class="comentario card mb-3 p-3">
                    <!-- Cabecera con foto y nombre -->
                    <div class="d-flex align-items-center mb-2 commentProfileCard"> <!-- Flexbox para alinear foto y nombre -->
                        <!-- Foto de perfil -->
                        <?php if ($notificacion['origen_foto']): ?>
                            <img src="../uploads/<?php echo htmlspecialchars($notificacion['origen_foto']); ?>" alt="Foto de perfil" class="rounded-circle me-2" style="width: 40px; height: 40px;">
                        <?php else: ?>
                            <img src="../uploads/default.png" alt="Foto de perfil por defecto" class="rounded-circle me-2" style="width: 40px; height: 40px;">
                        <?php endif; ?>
                        <!-- Nombre del usuario -->
                        <a href='./usuarios_perfil.php?usuario_id=<?php echo $notificacion['usuario_id']; ?>'>
                            <strong><?php echo htmlspecialchars($notificacion['origen_nombre'] . ' ' . $notificacion['origen_apellido']); ?></strong>
                        </a>
                    </div>

                    <!-- Cuerpo con mensaje e icono -->
                    <div class="d-flex justify-content-between align-items-center mb-2">
                        <p  class="mb-1 flex-grow-1"><?php echo htmlspecialchars($notificacion['mensaje']); ?></p>
                        <!-- Icono según la acción -->
                        <div class="notificacion-icono notificationOrangeColor">
                            <?php if ($notificacion['accion'] === 'Retweet'): ?>
                                <i class="bi bi-arrow-repeat" style="font-size: 1.5rem;"></i>
                            <?php elseif ($notificacion['accion'] === 'Me gusta'): ?>
                                <i class="bi bi-heart-fill" style="font-size: 1.5rem;"></i>
                            <?php elseif ($notificacion['accion'] === 'Comentario'): ?>
                                <i class="bi bi-chat-dots" style="font-size: 1.5rem;"></i>
                            <?php endif; ?>
                        </div>
                    </div>

                    <!-- Fecha de la notificación -->
                    <div class="notificationOrangeColor small">
                        <i class="bi bi-clock me-1"></i><?php echo htmlspecialchars($notificacion['fecha_notificacion']); ?>
                        <?php if (!$notificacion['leido']): ?>
                            <span class="nuevo">[Nuevo]</span>
                        <?php endif; ?>
                    </div>
                </div>
            <?php endforeach; ?>
        </div>
    <?php endif; ?>
</div>

    <!-- Bootstrap Icons -->
   
</body>
</html>
