<?php
require_once '../../PHP/bd_conexion.php';
require_once '../../php/admin_seguridad.php';
require_once '../../fpdf/fpdf.php';

verificarRol('Estudiante');

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $solicitudData = [
        'tipo_solicitud' => $_POST['tipo_solicitud'],
        'descripcion_general' => $_POST['descripcion_general'],
        'detalles' => isset($_POST['detalles']) ? json_encode($_POST['detalles']) : json_encode([]),
        'subprograma_id' => $_POST['subprograma_id'],
        'semestre' => $_POST['semestre']
    ];

    $_SESSION['solicitud_data'] = $solicitudData;
} else {
    $solicitudData = $_SESSION['solicitud_data'];
}

// Obtener el nombre del subprograma desde la base de datos
$stmt = $pdo->prepare("SELECT nombre_subprograma FROM subprogramas WHERE id = :subprograma_id");
$stmt->execute(['subprograma_id' => $solicitudData['subprograma_id']]);
$subprograma = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$subprograma) {
    die("Error: Subprograma no válido.");
}

// Obtener el jefe_id desde la tabla jefe_subprogramas
$stmt = $pdo->prepare("SELECT jefe_id FROM jefe_subprogramas WHERE subprograma_id = :subprograma_id");
$stmt->execute(['subprograma_id' => $solicitudData['subprograma_id']]);
$jefeSubprograma = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$jefeSubprograma) {
    echo "<script>
        alert('No hay jefe de subprograma asignado actualmente para este subprograma.');
        window.location.href = '../perfil_estudiante.php';
    </script>";
    exit;
}

// Obtener el nombre del jefe de subprograma desde la tabla usuarios
$stmt = $pdo->prepare("SELECT CONCAT(primer_nombre, ' ', primer_apellido) AS nombre_jefe FROM usuarios WHERE id = :jefe_id AND rol = 'JP'");
$stmt->execute(['jefe_id' => $jefeSubprograma['jefe_id']]);
$jefe = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$jefe) {
    echo "<script>
        alert('Jefe de subprograma no válido.');
        window.location.href = '../perfil_estudiante.php';
    </script>";
    exit;
}

// Obtener el sede_id desde la tabla subprogramas_estudiantes
$stmt = $pdo->prepare("SELECT sede_id FROM subprogramas_estudiantes WHERE subprograma_id = :subprograma_id AND usuario_id = :usuario_id");
$stmt->execute([
    'subprograma_id' => $solicitudData['subprograma_id'],
    'usuario_id' => $_SESSION['user_id']
]);
$subprogramaEstudiante = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$subprogramaEstudiante) {
    die("Error: No se encontró la relación entre el subprograma y el usuario.");
}

// Obtener el nombre de la sede desde la base de datos
$stmt = $pdo->prepare("SELECT nombre_sede FROM sedes WHERE id = :sede_id");
$stmt->execute(['sede_id' => $subprogramaEstudiante['sede_id']]);
$sede = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$sede) {
    die("Error: Sede no válida.");
}

// Obtener el nombre del usuario desde la base de datos
$stmt = $pdo->prepare("SELECT CONCAT(primer_nombre, ' ', primer_apellido) AS nombre_usuario, ci FROM usuarios WHERE id = :user_id");
$stmt->execute(['user_id' => $_SESSION['user_id']]);
$usuario = $stmt->fetch(PDO::FETCH_ASSOC);

if (!$usuario) {
    die("Error: Usuario no válido.");
}

// Obtener el tipo de solicitud y los detalles
$tipoSolicitud = $solicitudData['tipo_solicitud'];
$detalles = json_decode($solicitudData['detalles'], true);

// Verificar si el tipo de solicitud es "Levante de Prelación"
if (in_array('Levante Prelación', explode(',', $tipoSolicitud))) {
    $subproyectoCursando = $detalles['subproyecto-cursando'] ?? 'No especificado';
    $subproyectoPrelar = $detalles['subproyecto-a-prelar'] ?? 'No especificado';
    // Aquí puedes agregar el código para manejar los datos específicos de "Levante de Prelación"
    // Por ejemplo, agregar estos datos al PDF
}

// Crear instancia de FPDF
$pdf = new FPDF();
$pdf->AddPage();

// Agregar fuentes personalizadas
$pdf->AddFont('DejaVuSansCondensed', '', 'DejaVuSansCondensed.php');
$pdf->AddFont('DejaVuSansCondensed', 'B', 'DejaVuSansCondensed-Bold.php');

// Configurar la fuente
$pdf->SetFont('DejaVuSansCondensed', 'B', 16);
$pdf->Image('../../imagen/logo unellez.png', 10, 10, 20); // Ajusta la ruta si es necesario

// Agregar la fecha actual
$pdf->SetFont('DejaVuSansCondensed', 'B', 12);
$pdf->Cell(0, 10, utf8_decode('Fecha: ' . date('d/m/Y')), 0, 1, 'R');

$pdf->SetFont('DejaVuSansCondensed', 'B', 20);
$pdf->Cell(0, 40, utf8_decode('Exposición de motivo'), 0, 1, 'C');

$pdf->SetFont('DejaVuSansCondensed', '', 12);
$pdf->Ln(10);

function convertirARomano($numero) {
    $map = [
        'M' => 1000, 'CM' => 900, 'D' => 500, 'CD' => 400,
        'C' => 100, 'XC' => 90, 'L' => 50, 'XL' => 40,
        'X' => 10, 'IX' => 9, 'V' => 5, 'IV' => 4, 'I' => 1
    ];
    $resultado = '';
    while ($numero > 0) {
        foreach ($map as $romano => $entero) {
            if ($numero >= $entero) {
                $numero -= $entero;
                $resultado .= $romano;
                break;
            }
        }
    }
    return $resultado;
}

// Mapeo de nombres amigables
$nombresAmigables = [
    'carrera-curso-cambio' => 'Carrera Actual',
    'carrera-equivalencia-cambio' => 'Carrera a Elegir',
    'carrera-curso-equivalencia' => 'Carrera Actual',
    'carrera-equivalencia-elegida' => 'Equivalencia a Aplicar',
    'subproyectos-inscribir-1' => 'Subproyecto Temporal',
    'subproyectos-inscribir-2' => 'Subproyecto Temporal',
    'subproyectos-inscribir-3' => 'Subproyecto Temporal',
    'carreras-reingreso' => 'Carrera',
    'carrera-retiro' => 'Carrera',
    'extension-actual' => 'Carrera',
    'Sede-nueva' => 'Zona a Elegir',
    'Unidades-de-crédito-extras-solicitado' => 'Cantidad de crédito a Solicitar',
    'subproyecto-cursando' => 'Subproyecto Cursando',
    'subproyecto-a-prelar' => 'Subproyecto a Prelar'
];

// Llamada a la función convertirARomano y uso del resultado en el PDF
$pdf->MultiCell(0, 10, utf8_decode('Yo, ' . $usuario['nombre_usuario'] . ', titular de la cédula de identidad ' . $usuario['ci'] . ', cursante del semestre ' . convertirARomano($solicitudData['semestre']) . ' de la carrera ' . $subprograma['nombre_subprograma'] . '. Me dirijo a usted ' . $jefe['nombre_jefe'] . ', jefe del subprograma ' . $subprograma['nombre_subprograma'] . ' para realizar las siguientes solicitud:'), 0, 'J');

$pdf->Ln(10);

// Agregar el tipo de solicitud
$pdf->SetFont('DejaVuSansCondensed', 'B', 12);
$pdf->MultiCell(0, 10, utf8_decode(' ' . ucfirst($solicitudData['tipo_solicitud'])), 0, 'J');
$pdf->Ln(5);

// Agregar los detalles de la solicitud
$detalles = json_decode($solicitudData['detalles'], true);

if (is_array($detalles)) {
    foreach ($detalles as $key => $value) {
        if ($value !== "Seleccione una opción") {
            $nombreAmigable = $nombresAmigables[$key] ?? ucfirst(str_replace('-', ' ', $key));
            $pdf->MultiCell(0, 10, utf8_decode($nombreAmigable . ': ' . $value), 0, 1);
        }
    }
} else {
    $pdf->Cell(0, 10, utf8_decode('No hay detalles disponibles.'), 0, 1);
}

$pdf->Ln(10);
$pdf->SetFont('DejaVuSansCondensed', '', 12);
$pdf->MultiCell(0, 10, utf8_decode($solicitudData['descripcion_general']), 0, 'J');

// Agregar "att. [nombre del estudiante]"
$pdf->Ln(20);
$pdf->SetFont('DejaVuSansCondensed', 'B', 12);
$pdf->Cell(0, 10, utf8_decode('Att. ' . $usuario['nombre_usuario']), 0, 1, 'R');

// Obtener el contenido del PDF como una cadena
$pdfContent = $pdf->Output('S');

// Codificar el contenido del PDF en base64
$pdfBase64 = base64_encode($pdfContent);
?>

<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Vista Previa de la Solicitud</title>
    <link rel="stylesheet" href="../../CSS/estilo_solicitudes.css">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <style>
        body, html {
            margin: 0;
            padding: 0;
            height: 100%;
            display: flex;
            flex-direction: column;
        }
        .pdf-container {
            flex: 1;
            display: flex;
            justify-content: center;
            align-items: center;
        }
        .pdf-viewer {
            width: 100%;
            height: 100%;
        }
        .buttons-container {
            display: flex;
            justify-content: center;
            padding: 10px;
            background-color: #f1f1f1;
        }
        .buttons-container button {
            margin: 0 10px;
            padding: 10px 20px;
            font-size: 16px;
            border: none;
            cursor: pointer;
        }
        .btn-green {
            background-color: #28a745;
            color: white;
        }
        .btn-green:hover {
            background-color: #218838;
        }
        .btn-orange {
            background-color: #f44336;
            color: white;
        }
        .btn-orange:hover {
            background-color: #e53935;
        }
    </style>
</head>
<body>
    <div class="pdf-container">
        <?php if (isset($pdfBase64)): ?>
            <iframe class="pdf-viewer" src="data:application/pdf;base64,<?= htmlspecialchars($pdfBase64); ?>" frameborder="0"></iframe>
        <?php else: ?>
            <p>Error al generar la vista previa del PDF.</p>
        <?php endif; ?>
    </div>
    <div class="buttons-container">
        <form method="POST" action="confirmar_envio.php">
            <input type="hidden" name="pdfBase64" value="<?= htmlspecialchars($pdfBase64); ?>">
            <input type="hidden" name="detalles" value="<?= htmlspecialchars(json_encode($detalles)); ?>">
            <input type="hidden" name="subprograma_id_anterior" value="<?= htmlspecialchars($solicitudData['subprograma_id']); ?>">
            <input type="hidden" name="sede_id_anterior" value="<?= htmlspecialchars($subprogramaEstudiante['sede_id']); ?>">
            <input type="hidden" name="tipo_solicitud" value="<?= htmlspecialchars($solicitudData['tipo_solicitud']); ?>">
            <input type="hidden" name="nueva_sede_nombre" value="<?= htmlspecialchars($detalles['extension-nueva'] ?? ''); ?>">
            <button type="submit" class="btn btn-green"><i class="fa fa-check"></i> Confirmar Envío</button>
            <button type="button" class="btn btn-orange" onclick="window.location.href='../perfil_estudiante.php'"><i class="fa fa-times"></i> Cancelar</button>
        </form>
    </div>
</body>
</html>
