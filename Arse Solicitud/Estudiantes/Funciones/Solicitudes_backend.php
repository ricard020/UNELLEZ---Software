<?php
require_once '../../PHP/bd_conexion.php';
require_once '../../php/admin_seguridad.php';
verificarRol('Estudiante');

if (!isset($_GET['subprograma_id'])) {
    die("Error: Subprograma no seleccionado.");
}

$subprograma_id = $_GET['subprograma_id'];
$idUsuario = $_SESSION['user_id'];

// Verificar que el subprograma pertenece al usuario autenticado
$stmt = $pdo->prepare("
    SELECT sp.id, sp.nombre_subprograma, s.nombre_sede 
    FROM subprogramas_estudiantes se
    JOIN subprogramas sp ON se.subprograma_id = sp.id
    JOIN sedes s ON se.sede_id = s.id
    WHERE se.usuario_id = :idUsuario AND sp.id = :subprograma_id
");
$stmt->execute(['idUsuario' => $idUsuario, 'subprograma_id' => $subprograma_id]);

$subprogramaData = $stmt->fetch(PDO::FETCH_ASSOC);
if (!$subprogramaData) {
    die("Error: Subprograma no válido o no pertenece al usuario.");
}

// Obtener los subprogramas inscritos por el usuario
$stmt = $pdo->prepare("SELECT sp.id, sp.nombre_subprograma FROM subprogramas_estudiantes se JOIN subprogramas sp ON se.subprograma_id = sp.id WHERE se.usuario_id = :idUsuario");
$stmt->execute(['idUsuario' => $idUsuario]);
$subprogramasInscritos = $stmt->fetchAll(PDO::FETCH_ASSOC);

// Obtener las opciones de solicitud desde la base de datos
$stmt = $pdo->prepare("SELECT id, nombre_tipo FROM tipos_solicitudes");
$stmt->execute();
$tiposSolicitud = $stmt->fetchAll(PDO::FETCH_ASSOC);

// Obtener los subprogramas para los campos dinámicos
$stmt = $pdo->prepare("SELECT nombre_subprograma FROM subprogramas");
$stmt->execute();
$subprogramas = $stmt->fetchAll(PDO::FETCH_COLUMN);

// Obtener los subproyectos para los campos dinámicos
$stmt = $pdo->prepare("SELECT nombre_sub_proyecto FROM sub_proyectos WHERE subprograma_id = :subprograma_id");
$stmt->execute(['subprograma_id' => $subprograma_id]);
$subproyectos = $stmt->fetchAll(PDO::FETCH_COLUMN);
array_unshift($subproyectos, "Seleccione una opción"); // Agregar opción al inicio

// Obtener las sedes disponibles para el subprograma
$stmt = $pdo->prepare("SELECT s.id, s.nombre_sede FROM subprogramas_sedes ss JOIN sedes s ON ss.sede_id = s.id WHERE ss.subprograma_id = :subprograma_id");
$stmt->execute(['subprograma_id' => $subprograma_id]);
$sedesDisponibles = $stmt->fetchAll(PDO::FETCH_ASSOC);

// Obtener el número de semestres del subprograma
$stmt = $pdo->prepare("SELECT Semestre FROM subprogramas WHERE id = :subprograma_id");
$stmt->execute(['subprograma_id' => $subprograma_id]);
$numeroSemestres = $stmt->fetchColumn();

// Obtener el nombre del jefe del subprograma
$stmt = $pdo->prepare("
    SELECT u.primer_nombre, u.primer_apellido 
    FROM jefe_subprogramas js
    JOIN usuarios u ON js.jefe_id = u.id
    WHERE js.subprograma_id = :subprograma_id
");
$stmt->execute(['subprograma_id' => $subprograma_id]);
$jefeActual = $stmt->fetch(PDO::FETCH_ASSOC);
$jefeNombreCompleto = $jefeActual ? $jefeActual['primer_nombre'] . ' ' . $jefeActual['primer_apellido'] : 'No asignado';

// Campos dinámicos para cada tipo de solicitud
$camposPorSolicitud = [
    'Cambio de carrera' => [
        ['label' => 'Carrera Actual', 'id' => 'carrera-curso-cambio', 'options' => $subprogramas],
        ['label' => 'Carrera a Elegir', 'id' => 'carrera-equivalencia-cambio', 'options' => $subprogramas] // Cambiar el ID aquí
    ],
    'Equivalencia' => [
        ['label' => 'Carrera Actual', 'id' => 'carrera-curso-equivalencia', 'options' => $subprogramas],
        ['label' => 'Carrera a Elegir', 'id' => 'carrera-equivalencia-elegida', 'options' => $subprogramas]
    ],
    'Inscripciones temporales' => [
        ['label' => 'Subproyecto a Inscribir 1', 'id' => 'subproyectos-inscribir-1', 'options' => $subproyectos],
        ['label' => 'Subproyecto a Inscribir 2', 'id' => 'subproyectos-inscribir-2', 'options' => $subproyectos],
        ['label' => 'Subproyecto a Inscribir 3', 'id' => 'subproyectos-inscribir-3', 'options' => $subproyectos]
    ],
    'Reingreso' => [
        ['label' => 'Carrera', 'id' => 'carreras-reingreso', 'options' => $subprogramas]
    ],
    'Retiro temporal' => [
        ['label' => 'Carrera', 'id' => 'carrera-retiro', 'options' => $subprogramas]
    ],
    'Traslado' => [
        ['label' => 'Carrera', 'id' => 'extension-actual', 'options' => [$subprogramaData['nombre_sede']], 'editable' => false],
        ['label' => 'Zona a elegir', 'id' => 'Sede-nueva', 'options' => array_column($sedesDisponibles, 'nombre_sede')]
    ],
    'Unidades de crédito extras' => [
        ['label' => 'Cantidad de Unidades a Solicitar', 'id' => 'Unidades-de-crédito-extras-solicitado', 'options' => ['1', '2', '3', '4', '5']]
    ],
    'Levante Prelación' => [
        ['label' => 'Subproyecto', 'id' => 'subproyecto-cursando', 'options' => $subproyectos],
        ['label' => 'Subproyecto a Prelar', 'id' => 'subproyecto-a-prelar', 'options' => $subproyectos]
    ]
];

// Reglas de compatibilidad
$compatibilidad = [
    "Reingreso" => ["Traslado", "Inscripciones temporales"],
    "Traslado" => ["Reingreso", "Cambio de carrera", "Equivalencia", "Inscripciones temporales", "Unidades de crédito extras"],
    "Cambio de carrera" => ["Equivalencia", "Inscripciones temporales"],
    "Equivalencia" => ["Traslado", "Cambio de carrera"],
    "Inscripciones temporales" => ["Reingreso", "Traslado", "Cambio de carrera", "Levante Prelación", "Unidades de crédito extras"],
    "Retiro temporal" => [], // No compatible con ninguna otra
    "Unidades de crédito extras" => ["Traslado", "Cambio de carrera", "Inscripciones temporales", "Levante Prelación"],
    "Levante Prelación" => ["Inscripciones temporales", "Unidades de crédito extras"]
];

if ($_SERVER['REQUEST_METHOD'] === 'POST') {
    $tipo_solicitud = $_POST['tipo_solicitud'];
    $detalles = isset($_POST['detalles']) ? json_encode($_POST['detalles']) : json_encode([]);
    $descripcion_general = $_POST['descripcion_general'];
    $semestre = $_POST['semestre'];

    // Validar longitud de la descripción general
    if (strlen($descripcion_general) > 485) {
        echo json_encode(['success' => false, 'message' => "La descripción general no puede exceder los 485 caracteres."]);
        exit;
    }

    // Validar compatibilidad
    $seleccionadas = explode(',', $tipo_solicitud);
    if (count($seleccionadas) > 3) {
        echo json_encode(['success' => false, 'message' => "No puede seleccionar más de 3 tipos de solicitud."]);
        exit;
    }

    foreach ($seleccionadas as $actual) {
        foreach ($seleccionadas as $otra) {
            if ($actual !== $otra && !in_array($otra, $compatibilidad[$actual])) {
                echo json_encode(['success' => false, 'message' => "La solicitud '$actual' no es compatible con '$otra'."]);
                exit;
            }
        }
    }

    // Verificar si el subprograma para "Cambio de carrera" ya está inscrito
    $cambioCarreraSubprograma = $_POST['detalles']['carrera-equivalencia-cambio'] ?? null;
    if ($cambioCarreraSubprograma) {
        foreach ($subprogramasInscritos as $subprograma) {
            if ($subprograma['nombre_subprograma'] === $cambioCarreraSubprograma) {
                die("Error: Ya estás inscrito en el subprograma seleccionado para Cambio de carrera.");
            }
        }
    }

    // Guardar los datos en la sesión para usarlos en la vista previa
    $_SESSION['solicitud_data'] = [
        'usuario_id' => $idUsuario,
        'subprograma_id' => $subprogramaData['id'],
        'tipo_solicitud' => $tipo_solicitud,
        'detalles' => $detalles,
        'descripcion_general' => $descripcion_general,
        'semestre' => $semestre
    ];
    
    echo json_encode(['success' => true, 'redirect' => '../funciones/generar_pdf.php']);
    exit;
}

function generarOpcionesSolicitud($tiposSolicitud) {
    $html = '';
    foreach ($tiposSolicitud as $tipo) {
        $html .= '<label>';
        $html .= '<input type="checkbox" class="solicitud-option" data-target="' . htmlspecialchars($tipo['nombre_tipo']) . '"> ' . htmlspecialchars($tipo['nombre_tipo']);
        $html .= '</label>';
    }
    return $html;
}

function generarCamposDinamicos($camposPorSolicitud, $compatibilidad, $subprogramaActual) {
    $html = '<script>';
    $html .= 'const camposPorSolicitud = ' . json_encode($camposPorSolicitud) . ';';
    $html .= 'const compatibilidad = ' . json_encode($compatibilidad) . ';';
    $html .= 'const subprogramaActual = ' . json_encode($subprogramaActual) . ';';
    $html .= '</script>';
    return $html;
}

function convertirARomano($numero) {
    $map = [
        'M' => 1000, 'CM' => 900, 'D' => 500, 'CD' => 400,
        'C' => 100, 'XC' => 90, 'L' => 50, 'XL' => 40,
        'X' => 10, 'IX' => 9, 'V' => 5, 'IV' => 4, 'I' => 1
    ];
    $resultado = '';
    while ($numero > 0) {
        foreach ($map as $romano => $arabigo) {
            if ($numero >= $arabigo) {
                $numero -= $arabigo;
                $resultado .= $romano;
                break;
            }
        }
    }
    return $resultado;
}
?>