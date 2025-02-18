<?php
session_start();
require_once '../../PHP/bd_conexion.php';

if (!isset($_SESSION['user_id'])) {
    echo json_encode(['error' => 'No ha iniciado sesión.']);
    exit();
}

$userId = $_SESSION['user_id'];

try {
    // Consulta para obtener los datos del estudiante y sus subprogramas
    $stmt = $pdo->prepare("
        SELECT 
            u.primer_nombre,
            u.segundo_nombre,
            u.primer_apellido,
            u.segundo_apellido,
            u.ci,
            u.correo,
            u.foto_perfil,
            s.nombre_subprograma,
            s.programa_id,
            se.sede_id,
            p.nombre_programa,
            sd.nombre_sede
        FROM usuarios u
        LEFT JOIN subprogramas_estudiantes se ON u.id = se.usuario_id
        LEFT JOIN subprogramas s ON se.subprograma_id = s.id
        LEFT JOIN programas p ON s.programa_id = p.id
        LEFT JOIN sedes sd ON se.sede_id = sd.id
        WHERE u.id = :userId AND u.rol = 'Estudiante'
        ORDER BY p.nombre_programa, sd.nombre_sede, s.nombre_subprograma
    ");
    $stmt->execute(['userId' => $userId]);
    $estudiante = $stmt->fetchAll(PDO::FETCH_ASSOC);

    if ($estudiante) {
        $result = [
            'primer_nombre' => $estudiante[0]['primer_nombre'],
            'segundo_nombre' => $estudiante[0]['segundo_nombre'],
            'primer_apellido' => $estudiante[0]['primer_apellido'],
            'segundo_apellido' => $estudiante[0]['segundo_apellido'],
            'ci' => $estudiante[0]['ci'],
            'correo' => $estudiante[0]['correo'],
            'foto_perfil' => $estudiante[0]['foto_perfil'],
            'subprogramas' => []
        ];

        foreach ($estudiante as $row) {
            $programaId = $row['programa_id'];
            $nombrePrograma = $row['nombre_programa'];
            $sedeId = $row['sede_id'];
            $nombreSede = $row['nombre_sede'];
            $nombreSubprograma = $row['nombre_subprograma'];

            if (!isset($result['subprogramas'][$programaId])) {
                $result['subprogramas'][$programaId] = [
                    'nombre_programa' => $nombrePrograma,
                    'sedes' => []
                ];
            }

            if (!isset($result['subprogramas'][$programaId]['sedes'][$sedeId])) {
                $result['subprogramas'][$programaId]['sedes'][$sedeId] = [
                    'nombre_sede' => $nombreSede,
                    'subprogramas' => []
                ];
            }

            $result['subprogramas'][$programaId]['sedes'][$sedeId]['subprogramas'][] = $nombreSubprograma;
        }

        echo json_encode($result);
    } else {
        echo json_encode(['error' => 'No se encontraron datos del estudiante.']);
    }
} catch (PDOException $e) {
    echo json_encode(['error' => 'Error al obtener los datos del estudiante: ' . $e->getMessage()]);
}
?>
