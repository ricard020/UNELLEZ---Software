<?php
// Importar la autenticación
require_once '../PHP/Admin_seguridad.php';
verificarRol('JP');

// Importar la conexión a la base de datos
require_once '../PHP/bd_conexion.php';

// Obtener los datos del usuario
$usuario_id = $_SESSION['user_id'];
$stmt = $pdo->prepare("SELECT primer_nombre, primer_apellido, ci, correo FROM usuarios WHERE id = :id");
$stmt->execute(['id' => $usuario_id]);
$usuario = $stmt->fetch(PDO::FETCH_ASSOC);

// Verificar si los datos están completos
if (empty($usuario['primer_nombre']) || empty($usuario['primer_apellido']) || empty($usuario['correo'])) {
    header('Location: ./Funciones/completar_datos.php');
    exit;
}

// Obtener el subprograma del jefe
$stmt = $pdo->prepare("SELECT subprograma_id FROM jefe_subprogramas WHERE jefe_id = :jefeId");
$stmt->execute(['jefeId' => $usuario_id]);
$subprograma = $stmt->fetch(PDO::FETCH_ASSOC);

if ($subprograma) {
    $subprogramaId = $subprograma['subprograma_id'];
} else {
    die("Error: No se encontró el subprograma del jefe.");
}

// Obtener el nombre del subprograma
$stmt = $pdo->prepare("SELECT nombre_subprograma FROM subprogramas WHERE id = :subprogramaId");
$stmt->execute(['subprogramaId' => $subprogramaId]);
$nombreSubprograma = $stmt->fetch(PDO::FETCH_ASSOC)['nombre_subprograma'];

// Obtener la cantidad de solicitudes pendientes
$stmt = $pdo->prepare("SELECT COUNT(*) as pendientes FROM solicitudes WHERE subprograma_id_anterior = :subprogramaId AND estado = 'Pendiente'");
$stmt->execute(['subprogramaId' => $subprogramaId]);
$solicitudesPendientes = $stmt->fetch(PDO::FETCH_ASSOC)['pendientes'];

// Obtener la cantidad de solicitudes diferidas
$stmt = $pdo->prepare("SELECT COUNT(*) as diferidas FROM solicitudes WHERE subprograma_id_anterior = :subprogramaId AND estado = 'Diferida'");
$stmt->execute(['subprogramaId' => $subprogramaId]);
$solicitudesDiferidas = $stmt->fetch(PDO::FETCH_ASSOC)['diferidas'];

// Obtener la cantidad de solicitudes elevadas
$stmt = $pdo->prepare("SELECT COUNT(*) as elevadas FROM solicitudes WHERE subprograma_id_anterior = :subprogramaId AND estado = 'Elevada'");
$stmt->execute(['subprogramaId' => $subprogramaId]);
$solicitudesElevadas = $stmt->fetch(PDO::FETCH_ASSOC)['elevadas'];
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Pantalla Principal - Jefe de Subprograma</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="../css/Visualizar_perfil_jefesubprogramas.css" rel="stylesheet">
    <link href="../css/header.css" rel="stylesheet">
    <style>
        
body {
    font-family: Arial, sans-serif;
    background-image: url('../imagen/2.jpg');
    background-size: cover; /* Ajusta la imagen para que cubra todo el fondo */
            background-position: center; /* Centra la imagen de fondo */
            background-repeat: no-repeat; 
            min-height: 100vh;
    background-size: cover;
    background-position: center;
    color: #333;
    line-height: 1.6;
}

header h1 {
    color: rgb(255, 107, 0);
}

h2 {text-align: center;}

header img {
    border-radius: 50%;
}

@keyframes slideDown {
    from {
        transform: translateY(-100%);
    }
    to {
        transform: translateY(0);
    }
}

header {
    animation: slideDown 0.5s ease-in-out;
}

@keyframes fadeIn {
    from {
        opacity: 0;
    }
    to {
        opacity: 1;
    }
}

body {
    animation: fadeIn 0.5s ease-in-out;
}

.menu {
    background-color: white;
    padding: 20px;
    border-radius: 15px;
    text-align: center;
    transition: transform 0.3s;
}
.menu:hover {
    transform: scale(1.05);
}



.solicitudes-menu {
    border: 2px;
    position: relative;
}
.solicitudes-menu .badge {
    position: absolute;
    top: 10px;
    right: 10px;
    border-radius: 50%;
    padding: 5px 10px;
    width: auto;
    height: 24px;
    display: flex;
    align-items: center;
    justify-content: center;
}
.solicitudes-menu .badge-pendientes {
    background-color: red;
    border: 1px solid black;
    color: black;
    width: 50px; /* Define el ancho */
    height: 50px; /* Define la altura */
}
.solicitudes-menu .badge-diferidas {
    background-color: yellow;
    border: 1px solid black;
    color: black;
    top: 70px;
    width: 50px; /* Define el ancho */
    height: 50px; /* Define la altura */
}
.solicitudes-menu .badge-elevadas {
    background-color: turquoise;
    border: 1px solid black;
    color: black;
    top: 130px;
    width: 50px; /* Define el ancho */
    height: 50px; /* Define la altura */
}

</style>
</head>
<body>

<header class="d-flex justify-content-between align-items-center">
        <a href="perfil_jefesubprograma.php">
        <img title="Inicio"  src="../imagen/logo unellez.png" alt="Logo Unellez" width="60">
        </a>
        <h1><?php echo htmlspecialchars($nombreSubprograma); ?></h1>
        <div class="d-flex align-items-center">
            <div class="dropdown">
                <a href="#" class="text-white" data-bs-toggle="dropdown">
                    <svg width="24" height="24" fill="none" stroke="#FF6B00" stroke-width="2" viewBox="0 0 24 24">
                        <circle cx="12" cy="7" r="4"></circle>
                        <path d="M4 21v-2a4 4 0 0 1 4-4h8a4 4 0 0 1 4 4v2"></path>
                    </svg>
                </a>
                <ul class="dropdown-menu">
                    <li><a class="dropdown-item" href="./opciones/visualizar_perfil_jefesubprograma.php">Datos del usuario</a></li>
                    <li><a class="dropdown-item" href="../PHP/logout.php">Cerrar Sesión</a></li>
                </ul>
            </div>
        </div>
    </header>

    <!-- Main Content -->
    <div class="container my-5">
    <h2> <?= htmlspecialchars($usuario['primer_nombre'] . ' ' . $usuario['primer_apellido']); ?></h2>
        <div class="row mt-5 text-center">
            <div class="col-md-4">
                <a href="../jefeSubPrograma/opciones/solicitudes.php?subprograma_id=<?= $subprogramaId ?>" class="text-decoration-none">
                <div class="menu bg-orange text-white shadow solicitudes-menu">
    <img src="../imagen/icon_solicitud.png" alt="Estadísticas" class="menu-icon">
    <h3 style="color: black;">Solicitudes</h3>
    <span class="badge badge-pendientes"><?php echo $solicitudesPendientes; ?></span>
    <span class="badge badge-diferidas"><?php echo $solicitudesDiferidas; ?></span>
    <span class="badge badge-elevadas"><?php echo $solicitudesElevadas; ?></span>
</div>

                </a>
            </div>
            <div class="col-md-4">
                <a href="../jefeSubPrograma/opciones/historial.php" class="text-decoration-none">
                    <div class="menu bg-orange text-white shadow solicitudes-menu">
                        <img src="../imagen/icon_historial.png" alt="Estadísticas" class="menu-icon">
                        <h3 style="color: black;">Historial</h3>
                    </div>
                </a>
            </div>
            <div class="col-md-4">
                <a href="../jefeSubPrograma/opciones/estadisticas.php" class="text-decoration-none">
                    <div class="menu bg-orange text-white shadow solicitudes-menu">
                        <img src="../imagen/icon_estadistica.png" alt="Estadísticas" class="menu-icon">
                        <h3 style="color: black;">Estadísticas</h3>
                    </div>
                </a>
            </div>
        </div>
    </div>

    <!-- Footer -->
    <footer class="py-3 text-center position-fixed bottom-0 w-500" style="background-color: #FF6B00; height: 50px;">
    <div class="container d-flex justify-content-center w-100">
        <a href="https://arse.unellez.edu.ve/arse/portal/login.php" class="text-white text-decoration-none">
            <i class="fas fa-globe"></i> ARSE DUX
        </a>
    </div>
    </footer>

    <!-- Scripts -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js"></script>
    <script>    document.addEventListener('DOMContentLoaded', function() {
        const badges = document.querySelectorAll('.solicitudes-menu .badge');
        badges.forEach(badge => {
            if (parseInt(badge.textContent) > 0) {
                badge.closest('.solicitudes-menu').style.border = '3px solid red';
            }
        });
    });</script>
</body>
</html>
