-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost:3307
-- Tiempo de generación: 06-02-2025 a las 04:01:36
-- Versión del servidor: 10.4.28-MariaDB
-- Versión de PHP: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `bduni`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `detalles_solicitudes`
--

CREATE TABLE `detalles_solicitudes` (
  `id` bigint(20) NOT NULL,
  `solicitud_id` bigint(20) NOT NULL,
  `tipo_solicitud_id` bigint(20) NOT NULL,
  `archivo_pdf` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Disparadores `detalles_solicitudes`
--
DELIMITER $$
CREATE TRIGGER `trigger_verificar_detalles` AFTER INSERT ON `detalles_solicitudes` FOR EACH ROW BEGIN
    DECLARE total INT;
    SELECT COUNT(*) INTO total
    FROM detalles_solicitudes
    WHERE solicitud_id = NEW.solicitud_id;
    IF total > 3 THEN
        SIGNAL SQLSTATE '45000'
        SET MESSAGE_TEXT = 'No se pueden agregar más de 3 detalles por solicitud';
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `historial_solicitudes`
--

CREATE TABLE `historial_solicitudes` (
  `id` bigint(20) NOT NULL,
  `usuario_id` bigint(20) DEFAULT NULL,
  `sede_id_anterior` bigint(20) DEFAULT NULL,
  `municipio_id_anterior` bigint(20) DEFAULT NULL,
  `subprograma_id_anterior` bigint(20) DEFAULT NULL,
  `nueva_sede_id` bigint(20) DEFAULT NULL,
  `nuevo_municipio_id` bigint(20) DEFAULT NULL,
  `nuevo_subprograma_id` bigint(20) DEFAULT NULL,
  `estado` enum('Pendiente','Aceptada','Rechazada','Diferida','Elevada') NOT NULL DEFAULT 'Pendiente',
  `fecha_solicitud` timestamp NOT NULL DEFAULT current_timestamp(),
  `fecha_resolucion` timestamp NULL DEFAULT NULL,
  `jp_id` bigint(20) DEFAULT NULL,
  `nota` text DEFAULT NULL,
  `archivo_pdf` varchar(255) DEFAULT NULL,
  `solicitud_id` bigint(20) NOT NULL,
  `subprogramas_estudiantes_id` bigint(20) DEFAULT NULL,
  `numero_caso` int(11) DEFAULT NULL,
  `numero_resolucion` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Disparadores `historial_solicitudes`
--
DELIMITER $$
CREATE TRIGGER `trigger_notificar_cambio_carrera` AFTER INSERT ON `historial_solicitudes` FOR EACH ROW BEGIN
    IF NEW.estado = 'Aprobada' THEN
        -- Insertar notificación para el estudiante
        INSERT INTO notificaciones (usuario_id, mensaje, fecha_envio, leido)
        VALUES (
            NEW.usuario_id,
            CONCAT('Su solicitud ha sido ', NEW.estado, '. ', NEW.nota),
            NOW(),
            0
        );

        -- Actualizar el subprograma del estudiante
        UPDATE subprogramas_estudiantes
        SET subprograma_id = NEW.nuevo_subprograma_id
        WHERE usuario_id = NEW.usuario_id;
    ELSEIF NEW.estado = 'Rechazada' THEN
        -- Insertar notificación para el estudiante
        INSERT INTO notificaciones (usuario_id, mensaje, fecha_envio, leido)
        VALUES (
            NEW.usuario_id,
            CONCAT('Su solicitud ha sido ', NEW.estado, '. ', NEW.nota),
            NOW(),
            0
        );
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `jefe_subprogramas`
--

CREATE TABLE `jefe_subprogramas` (
  `jefe_id` bigint(20) NOT NULL,
  `subprograma_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `jefe_subprogramas`
--

INSERT INTO `jefe_subprogramas` (`jefe_id`, `subprograma_id`) VALUES
(2, 4),
(3, 5),
(4, 6),
(5, 8),
(6, 9),
(7, 10),
(8, 11),
(9, 12),
(10, 13),
(11, 14),
(12, 15),
(13, 16),
(14, 17),
(15, 18),
(16, 19),
(17, 20),
(18, 21),
(19, 22),
(20, 23),
(21, 24),
(22, 25),
(23, 26),
(24, 27),
(25, 28),
(26, 29),
(27, 30),
(28, 31),
(29, 32),
(30, 33),
(31, 34),
(32, 35),
(33, 36),
(34, 37);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `municipios`
--

CREATE TABLE `municipios` (
  `id` bigint(20) NOT NULL,
  `nombre_municipio` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `municipios`
--

INSERT INTO `municipios` (`id`, `nombre_municipio`) VALUES
(1, 'Alberto Arbelo Torrealba'),
(2, 'Antonio José de Sucre'),
(3, 'Barinas'),
(4, 'Bolívar'),
(5, 'Cardenal Quintero'),
(6, 'Obispos'),
(8, 'Cárdenas'),
(9, 'Cruz Paredes'),
(10, 'Pedraza'),
(11, 'Pueblo Llano'),
(12, 'Rojas'),
(14, 'Sabaneta'),
(15, 'Santos Marquina'),
(16, 'Sosa');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `notificaciones`
--

CREATE TABLE `notificaciones` (
  `id` bigint(20) NOT NULL,
  `usuario_id` bigint(20) DEFAULT NULL,
  `mensaje` text NOT NULL,
  `fecha_envio` timestamp NOT NULL DEFAULT current_timestamp(),
  `leido` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `programas`
--

CREATE TABLE `programas` (
  `id` bigint(20) NOT NULL,
  `nombre_programa` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `programas`
--

INSERT INTO `programas` (`id`, `nombre_programa`) VALUES
(1, 'Ciencias Básicas y Aplicadas'),
(2, 'Ciencias De La Salud'),
(3, 'Ciencias Del Agro Y Mar'),
(4, 'Ciencias Jurídicas Y Políticas '),
(5, 'Ciencias Sociales Y Económicas'),
(6, 'Ciencias De La Educación Y Humanidades');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `sedes`
--

CREATE TABLE `sedes` (
  `id` bigint(20) NOT NULL,
  `nombre_sede` varchar(255) NOT NULL,
  `municipio_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `sedes`
--

INSERT INTO `sedes` (`id`, `nombre_sede`, `municipio_id`) VALUES
(1, 'General (Alberto Arbelo Torrealba)', 1),
(2, 'General Socopó (Antonio José de Sucre)', 2),
(3, 'Principal (Barinas)', 3),
(4, 'Barinitas (Bolívar)', 4),
(5, 'Principal (Cardenal Quintero - Mérida)', 5),
(6, 'General (Canagua)', 10),
(7, 'General (Caramuca)', 3),
(8, 'General (Cárdenas - Táchira)', 8),
(9, 'General (Barrancas - Cruz Paredes)', 9),
(10, 'Pedraza (Municipio Pedraza)', 10),
(11, 'General (Pueblo Llano - Mérida)', 11),
(12, 'General (Libertad - Rojas)', 12),
(13, 'General (San Silvestre)', 3),
(14, 'General (Sabaneta)', 14),
(15, 'Santos Marquina (Tabay - Mérida)', 15),
(16, 'General (Ciudad de Nutrias - Sosa)', 16),
(17, 'General (Santo Domingo - Mérida)', 5),
(18, 'General (Táriba - Táchira)', 8),
(19, 'Calderas (Bolívar)', 4),
(20, 'Fuerte Tavacare (Barinas)', 3),
(21, 'General (Obispos)', 6);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `solicitudes`
--

CREATE TABLE `solicitudes` (
  `id` bigint(20) NOT NULL,
  `usuario_id` bigint(20) DEFAULT NULL,
  `sede_id_anterior` bigint(20) DEFAULT NULL,
  `municipio_id_anterior` bigint(20) DEFAULT NULL,
  `subprograma_id_anterior` bigint(20) DEFAULT NULL,
  `nueva_sede_id` bigint(20) DEFAULT NULL,
  `nuevo_municipio_id` bigint(20) DEFAULT NULL,
  `nuevo_subprograma_id` bigint(20) DEFAULT NULL,
  `estado` enum('Pendiente','Aceptada','Rechazada','Diferida','Elevada') NOT NULL DEFAULT 'Pendiente',
  `fecha_solicitud` timestamp NOT NULL DEFAULT current_timestamp(),
  `jp_id` bigint(20) DEFAULT NULL,
  `nota` text DEFAULT NULL,
  `archivo_pdf` varchar(255) DEFAULT NULL,
  `subprogramas_estudiantes_id` bigint(20) DEFAULT NULL,
  `numero_caso` int(11) DEFAULT NULL,
  `numero_resolucion` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Disparadores `solicitudes`
--
DELIMITER $$
CREATE TRIGGER `trigger_actualizar_historial` AFTER UPDATE ON `solicitudes` FOR EACH ROW BEGIN
    -- Verifica si ha cambiado algún dato importante
    IF NEW.estado <> OLD.estado 
       OR NEW.nota <> OLD.nota 
       OR NEW.numero_caso <> OLD.numero_caso 
       OR NEW.numero_resolucion <> OLD.numero_resolucion THEN

        -- Actualizar el registro en historial_solicitudes si ya existe
        UPDATE historial_solicitudes 
        SET estado = NEW.estado,
            numero_caso = NEW.numero_caso,
            numero_resolucion = NEW.numero_resolucion,
            nota = NEW.nota,
            fecha_resolucion = NOW()
        WHERE solicitud_id = NEW.id;
        
        -- Si no existe, lo insertamos (para cuando sea la primera vez)
        IF ROW_COUNT() = 0 THEN
            INSERT INTO historial_solicitudes (
                solicitud_id, usuario_id, sede_id_anterior, municipio_id_anterior, 
                subprograma_id_anterior, nueva_sede_id, nuevo_municipio_id, 
                nuevo_subprograma_id, estado, fecha_solicitud, fecha_resolucion, 
                jp_id, nota, archivo_pdf, subprogramas_estudiantes_id, 
                numero_caso, numero_resolucion
            ) VALUES (
                NEW.id, NEW.usuario_id, NEW.sede_id_anterior, NEW.municipio_id_anterior, 
                NEW.subprograma_id_anterior, NEW.nueva_sede_id, NEW.nuevo_municipio_id, 
                NEW.nuevo_subprograma_id, NEW.estado, NEW.fecha_solicitud, NOW(), 
                NEW.jp_id, NEW.nota, NEW.archivo_pdf, NEW.subprogramas_estudiantes_id, 
                NEW.numero_caso, NEW.numero_resolucion
            );
        END IF;
    END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trigger_actualizar_subprograma` AFTER UPDATE ON `solicitudes` FOR EACH ROW BEGIN
    IF NEW.estado = 'Aprobada' THEN
        IF NEW.nuevo_subprograma_id IS NOT NULL OR NEW.nueva_sede_id IS NOT NULL THEN
            UPDATE subprogramas_estudiantes
            SET 
                subprograma_id = COALESCE(NEW.nuevo_subprograma_id, subprograma_id),
                sede_id = COALESCE(NEW.nueva_sede_id, sede_id)
            WHERE id = NEW.subprogramas_estudiantes_id
              AND usuario_id = NEW.usuario_id;
        END IF;
    END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trigger_insertar_en_historial` AFTER INSERT ON `solicitudes` FOR EACH ROW BEGIN
    INSERT INTO historial_solicitudes (
        solicitud_id, usuario_id, sede_id_anterior, municipio_id_anterior, 
        subprograma_id_anterior, nueva_sede_id, nuevo_municipio_id, 
        nuevo_subprograma_id, estado, fecha_solicitud, fecha_resolucion, 
        jp_id, nota, archivo_pdf, subprogramas_estudiantes_id, 
        numero_caso, numero_resolucion
    ) VALUES (
        NEW.id, NEW.usuario_id, NEW.sede_id_anterior, NEW.municipio_id_anterior, 
        NEW.subprograma_id_anterior, NEW.nueva_sede_id, NEW.nuevo_municipio_id, 
        NEW.nuevo_subprograma_id, NEW.estado, NEW.fecha_solicitud, NOW(), 
        NEW.jp_id, NEW.nota, NEW.archivo_pdf, NEW.subprogramas_estudiantes_id, 
        NEW.numero_caso, NEW.numero_resolucion
    );
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `trigger_insertar_notificacion` AFTER UPDATE ON `solicitudes` FOR EACH ROW BEGIN
    IF NEW.estado <> OLD.estado THEN
        INSERT INTO notificaciones (
            usuario_id,
            mensaje,
            fecha_envio,
            leido
        )
        VALUES (
            NEW.usuario_id,
            CONCAT('Su solicitud con ID ', NEW.id, ' ha cambiado a ', NEW.estado, '. ', NEW.nota),
            NOW(),
            0
        );
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `subprogramas`
--

CREATE TABLE `subprogramas` (
  `id` bigint(20) NOT NULL,
  `nombre_subprograma` varchar(255) NOT NULL,
  `programa_id` bigint(20) NOT NULL,
  `Semestre` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `subprogramas`
--

INSERT INTO `subprogramas` (`id`, `nombre_subprograma`, `programa_id`, `Semestre`) VALUES
(4, 'Arquitectura', 1, 8),
(5, 'ING. Civil', 1, 10),
(6, 'ING. De Petróleo', 1, 10),
(7, 'ING. En Informática', 1, 10),
(8, 'Meteorología', 1, 10),
(9, 'T.S.U En Construcción Civil', 1, 6),
(10, 'T.S.U En Informática ', 1, 6),
(11, 'Botánica Tropical', 2, 8),
(12, 'Enfermería', 2, 8),
(13, 'Estadística', 2, 8),
(14, 'Economía Agrícola', 3, 9),
(15, 'ING. Agroindustrial', 3, 9),
(16, 'ING. Agronómica', 3, 10),
(17, 'ING. En Producción Animal', 3, 10),
(18, 'Medicina Veterinaria', 3, 10),
(19, 'Derecho', 4, 4),
(20, 'Administración', 5, 8),
(21, 'Administración Agropecuaria', 5, 8),
(22, 'Contaduría Pública', 5, 8),
(23, 'Sociología Del Desarrollo', 5, 8),
(24, 'Turismo Agroecológico', 5, 8),
(25, 'Biología', 6, 8),
(26, 'Castellano Y Literatura', 6, 8),
(27, 'Educ. Integral', 6, 8),
(28, 'Educ. Men. Física', 6, 8),
(29, 'Educ. Men. Física, Deportes Y Recreación', 6, 8),
(30, 'Educ. Men. Matemática', 6, 8),
(31, 'Educ. Men. Química', 6, 8),
(32, 'Geografía E Historia', 6, 8),
(33, 'PFG. Agropecuaria', 6, NULL),
(34, 'Men. Arte', 6, 8),
(35, 'Educ. Especial', 6, 8),
(36, 'LIC. Orientación', 6, 8),
(37, 'PNF. Inglés', 6, 8);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `subprogramas_estudiantes`
--

CREATE TABLE `subprogramas_estudiantes` (
  `id` bigint(20) NOT NULL,
  `usuario_id` bigint(20) DEFAULT NULL,
  `subprograma_id` bigint(20) DEFAULT NULL,
  `sede_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `subprogramas_sedes`
--

CREATE TABLE `subprogramas_sedes` (
  `id` bigint(20) NOT NULL,
  `subprograma_id` bigint(20) NOT NULL,
  `sede_id` bigint(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `subprogramas_sedes`
--

INSERT INTO `subprogramas_sedes` (`id`, `subprograma_id`, `sede_id`) VALUES
(1, 4, 3),
(2, 5, 3),
(3, 6, 3),
(4, 7, 3),
(5, 8, 3),
(6, 9, 3),
(7, 10, 3),
(8, 11, 3),
(9, 12, 3),
(10, 13, 3),
(11, 14, 3),
(12, 15, 3),
(13, 16, 3),
(14, 17, 3),
(15, 18, 3),
(16, 19, 3),
(17, 20, 3),
(18, 21, 3),
(19, 22, 3),
(20, 23, 3),
(21, 24, 3),
(22, 5, 15),
(23, 7, 1),
(24, 7, 4),
(25, 7, 9),
(26, 7, 10),
(27, 7, 15),
(28, 7, 18),
(29, 7, 19),
(30, 9, 5),
(31, 10, 2),
(32, 10, 14),
(33, 13, 4),
(34, 13, 15),
(35, 15, 5),
(36, 15, 14),
(37, 16, 20),
(38, 17, 5),
(39, 17, 6),
(40, 17, 7),
(41, 17, 10),
(42, 17, 12),
(43, 17, 13),
(44, 17, 14),
(45, 17, 15),
(46, 17, 16),
(47, 19, 1),
(48, 19, 2),
(49, 19, 4),
(50, 19, 5),
(51, 19, 7),
(52, 19, 9),
(53, 19, 10),
(54, 19, 11),
(55, 19, 12),
(56, 19, 13),
(57, 19, 15),
(58, 19, 16),
(59, 19, 18),
(60, 19, 21),
(61, 20, 4),
(62, 20, 9),
(63, 20, 10),
(64, 20, 12),
(65, 20, 15),
(66, 20, 17),
(67, 20, 18),
(68, 20, 2),
(69, 21, 12),
(70, 21, 15),
(71, 21, 16),
(72, 22, 4),
(73, 22, 5),
(74, 22, 6),
(75, 22, 7),
(76, 22, 10),
(77, 22, 12),
(78, 22, 14),
(79, 22, 15),
(80, 22, 16),
(81, 22, 18),
(82, 22, 19),
(83, 22, 21),
(84, 22, 2),
(85, 23, 2),
(86, 23, 4),
(87, 23, 10),
(88, 24, 5),
(89, 24, 8),
(90, 25, 3),
(91, 26, 3),
(92, 26, 13),
(93, 26, 16),
(94, 27, 2),
(95, 27, 3),
(96, 27, 6),
(97, 27, 7),
(98, 27, 10),
(99, 27, 11),
(100, 27, 12),
(101, 27, 13),
(102, 27, 14),
(103, 27, 15),
(104, 27, 16),
(105, 28, 3),
(106, 29, 2),
(107, 29, 3),
(108, 29, 5),
(109, 29, 7),
(110, 29, 9),
(111, 29, 10),
(112, 29, 11),
(113, 29, 12),
(114, 29, 15),
(115, 29, 18),
(116, 29, 19),
(117, 30, 3),
(118, 31, 3),
(119, 32, 3),
(120, 33, 3),
(121, 34, 2),
(122, 34, 3),
(123, 34, 11),
(124, 34, 15),
(125, 34, 19),
(126, 35, 3),
(127, 36, 3),
(128, 36, 10),
(129, 36, 15),
(130, 36, 21),
(131, 37, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `sub_proyectos`
--

CREATE TABLE `sub_proyectos` (
  `id` bigint(20) NOT NULL,
  `nombre_sub_proyecto` varchar(255) NOT NULL,
  `unidades_credito` int(11) NOT NULL,
  `subprograma_id` bigint(20) DEFAULT NULL,
  `semestre` int(11) NOT NULL,
  `anio_academico` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `sub_proyectos`
--

INSERT INTO `sub_proyectos` (`id`, `nombre_sub_proyecto`, `unidades_credito`, `subprograma_id`, `semestre`, `anio_academico`) VALUES
(48, 'Lenguaje y Comunicación', 4, 7, 1, 0),
(49, 'Cálculo I', 4, 7, 1, 0),
(50, 'Desarrollo Personal y Profesional', 2, 7, 1, 0),
(51, 'Entrenamiento Físico General', 4, 7, 1, 0),
(52, 'Lógica', 4, 7, 1, 0),
(53, 'Cálculo II', 4, 7, 2, 0),
(54, 'Introducción a la Informática', 4, 7, 2, 0),
(55, 'Espacio Geográfico e Identidad Nacional', 2, 7, 2, 0),
(56, 'Estadística Descriptiva', 3, 7, 2, 0),
(57, 'Inglés Instrumental', 2, 7, 2, 0),
(58, 'Ecología y Educación Ambiental', 3, 7, 2, 0),
(59, 'Cálculo III', 4, 7, 3, 0),
(60, 'Física', 4, 7, 3, 0),
(61, 'Algoritmos y Programación I', 4, 7, 3, 0),
(62, 'Álgebra', 4, 7, 3, 0),
(63, 'Inferencia y Probabilidades', 3, 7, 3, 0),
(64, 'Cálculo IV', 4, 7, 4, 0),
(65, 'Estructuras Discretas', 4, 7, 4, 0),
(66, 'Algoritmos y Programación II', 4, 7, 4, 0),
(67, 'Electiva I (Métodos y Técnicas de Estudio)', 2, 7, 4, 0),
(68, 'Electrónica', 4, 7, 4, 0),
(69, 'Bases de Datos I', 3, 7, 5, 0),
(70, 'Organización y Sistemas', 4, 7, 5, 0),
(71, 'Algoritmos y Programación II', 4, 7, 5, 0),
(72, 'Arquitectura del Computador', 3, 7, 5, 0),
(73, 'Metodología de la Investigación', 3, 7, 5, 0),
(74, 'Bases de Datos II', 3, 7, 6, 0),
(75, 'Investigación de Operaciones', 4, 7, 6, 0),
(76, 'Sistemas Operativos', 4, 7, 6, 0),
(77, 'Métodos Numéricos', 3, 7, 6, 0),
(78, 'Principios de Ingeniería del Software', 5, 7, 6, 0),
(79, 'Arquitecturas Software', 3, 7, 7, 0),
(80, 'Metodología de Desarrollo de Software', 4, 7, 7, 0),
(81, 'Electiva II (Administración)', 2, 7, 7, 0),
(82, 'Redes y Comunicaciones I', 4, 7, 7, 0),
(83, 'Desarrollo de Aplicaciones I', 3, 7, 7, 0),
(84, 'Investigación Social', 4, 7, 7, 0),
(85, 'Gerencia y Mercadeo', 3, 7, 8, 0),
(86, 'Redes y Comunicaciones II', 4, 7, 8, 0),
(87, 'Electiva III (Edumática)', 2, 7, 8, 0),
(88, 'Desarrollo de Aplicaciones II', 3, 7, 8, 0),
(89, 'Proyecto de Grado', 4, 7, 8, 0),
(90, 'Seminario Ética Profesional', 3, 7, 8, 0),
(91, 'Planificación de Proyectos', 3, 7, 9, 0),
(92, 'Auditoría de Sistemas Informáticos', 4, 7, 9, 0),
(93, 'Trabajo de Grado', 8, 7, 9, 0),
(94, 'Pasantía Profesional', 10, 7, 10, 0),
(95, 'Lenguaje y Comunicación', 4, 5, 1, 0),
(96, 'Cálculo I', 4, 5, 1, 0),
(97, 'Geometría Analítica', 3, 5, 1, 0),
(98, 'Informática', 2, 5, 1, 0),
(99, 'Educación Física y Deporte', 2, 5, 1, 0),
(100, 'Cálculo II', 4, 5, 2, 0),
(101, 'Álgebra Lineal', 3, 5, 2, 0),
(102, 'Física I', 4, 5, 2, 0),
(103, 'Dibujo', 3, 5, 2, 0),
(104, 'Desarrollo Endógeno', 2, 5, 2, 0),
(105, 'Electiva I', 2, 5, 2, 0),
(106, 'Cálculo III', 4, 5, 3, 0),
(107, 'Química de los Materiales', 4, 5, 3, 0),
(108, 'Física II', 4, 5, 3, 0),
(109, 'Geometría Descriptiva', 2, 5, 3, 0),
(110, 'Introducción a las Bellas Artes', 1, 5, 3, 0),
(111, 'Mecánica Racional', 3, 5, 3, 0),
(112, 'Inglés Instrumental', 2, 5, 4, 0),
(113, 'Cálculo IV', 4, 5, 4, 0),
(114, 'Estadística Aplicada', 3, 5, 4, 0),
(115, 'Estática Aplicada', 3, 5, 4, 0),
(116, 'Dibujo de Proyectos', 2, 5, 4, 0),
(117, 'Topografía', 4, 5, 4, 0),
(118, 'Resistencia de Materiales', 3, 5, 5, 0),
(119, 'Estudio y Ensayo de los Materiales', 4, 5, 5, 0),
(120, 'Geología Aplicada', 2, 5, 5, 0),
(121, 'Hidrología', 3, 5, 5, 0),
(122, 'Mecánica de los Fluidos', 3, 5, 5, 0),
(123, 'Electiva II', 2, 5, 5, 0),
(124, 'Higiene y Saneamiento Ambiental', 3, 5, 6, 0),
(125, 'Estructuras I', 3, 5, 6, 0),
(126, 'Mecánica de Suelos', 4, 5, 6, 0),
(127, 'Ingeniería Vial I', 3, 5, 6, 0),
(128, 'Hidráulica', 3, 5, 6, 0),
(129, 'Metodología de la Investigación', 3, 5, 6, 0),
(130, 'Estructuras II', 3, 5, 7, 0),
(131, 'Concreto Armado', 4, 5, 7, 0),
(132, 'Instalaciones para Edificaciones', 3, 5, 7, 0),
(133, 'Acueductos Cloacas y Drenaje', 4, 5, 7, 0),
(134, 'Ingeniería Vial II', 3, 5, 7, 0),
(135, 'Electiva III', 2, 5, 7, 0),
(136, 'Fundaciones y Muros', 2, 5, 8, 0),
(137, 'Proyecto Estructural en Concreto Armado', 3, 5, 8, 0),
(138, 'Construcción de Edificios y Urbanismo', 2, 5, 8, 0),
(139, 'Administración y Control de Obras', 2, 5, 8, 0),
(140, 'Obras Hidráulicas', 3, 5, 8, 0),
(141, 'Pavimentos', 3, 5, 8, 0),
(142, 'Ética y Legislación', 2, 5, 9, 0),
(143, 'Proyecto Estructural en Acero', 3, 5, 9, 0),
(144, 'Ingeniería de la Construcción', 2, 5, 9, 0),
(145, 'Hidráulica Fluvial', 3, 5, 9, 0),
(146, 'Seminario', 2, 5, 9, 0),
(147, 'Electiva IV', 2, 5, 9, 0),
(148, 'Pasantías', 4, 5, 10, 0),
(149, 'Trabajo de Grado', 4, 5, 10, 0),
(150, 'Cálculo I', 4, 6, 1, 0),
(151, 'Química I', 5, 6, 1, 0),
(152, 'Geometría', 4, 6, 1, 0),
(153, 'Lenguaje y Comunicación', 3, 6, 1, 0),
(154, 'Orientación Personal Social', 2, 6, 1, 0),
(155, 'Cálculo II', 4, 6, 2, 0),
(156, 'Química Orgánica', 5, 6, 2, 0),
(157, 'Física I', 6, 6, 2, 0),
(158, 'Dibujo', 3, 6, 2, 0),
(159, 'Metodología de la Investigación', 3, 6, 2, 0),
(160, 'Cálculo III', 4, 6, 3, 0),
(161, 'Álgebra Lineal', 3, 6, 3, 0),
(162, 'Física II', 6, 6, 3, 0),
(163, 'Informática', 2, 6, 3, 0),
(164, 'Sociedad Venezolana', 2, 6, 3, 0),
(165, 'Educación Física y Deporte', 2, 6, 3, 0),
(166, 'Cálculo IV', 4, 6, 4, 0),
(167, 'Termodinámica', 4, 6, 4, 0),
(168, 'Estática', 3, 6, 4, 0),
(169, 'Programación', 3, 6, 4, 0),
(170, 'Introducción a la Ingeniería de Petróleo', 2, 6, 4, 0),
(171, 'Electiva I', 2, 6, 4, 0),
(172, 'Mecánica de los fluidos', 4, 6, 5, 0),
(173, 'Gasotecnia', 4, 6, 5, 0),
(174, 'Mecánica de los Materiales', 3, 6, 5, 0),
(175, 'Métodos Numéricos', 3, 6, 5, 0),
(176, 'Estadística', 4, 6, 5, 0),
(177, 'Geología Física', 4, 6, 5, 0),
(178, 'Perforación I', 4, 6, 6, 0),
(179, 'Facilidades de Superficie', 3, 6, 6, 0),
(180, 'Seguridad, Higiene y Ambiente', 3, 6, 6, 0),
(181, 'Geología Petrolera', 4, 6, 6, 0),
(182, 'Ingles Instrumental I', 3, 6, 6, 0),
(183, 'Orientación Personal', 2, 6, 6, 0),
(184, 'Perforación II', 4, 6, 7, 0),
(185, 'Ingeniería de Yacimientos I', 4, 6, 7, 0),
(186, 'Desarrollo de la Industria de los Hidrocarburos de Venezuela', 3, 6, 7, 0),
(187, 'ingeniería Económica', 3, 6, 7, 0),
(188, 'Ingles Instrumental II', 3, 6, 7, 0),
(189, 'Electiva II', 3, 6, 7, 0),
(190, 'Producción de los Hidrocarburos', 4, 6, 8, 0),
(191, 'Ingeniería de Yacimientos II', 4, 6, 8, 0),
(192, 'Interpretación de Registro de Pozos', 4, 6, 8, 0),
(193, 'Desarrollo Endógeno', 2, 6, 8, 0),
(194, 'Gerencia y Planificación de Proyectos', 3, 6, 8, 0),
(195, 'Electiva III', 3, 6, 8, 0),
(196, 'Procesos de Campo', 4, 6, 9, 0),
(197, 'Ingeniería de Yacimientos III', 4, 6, 9, 0),
(198, 'Completación y Reacondicionamiento de Pozos', 3, 6, 9, 0),
(199, 'Ingeniería del Gas', 3, 6, 9, 0),
(200, 'Seminario', 2, 6, 9, 0),
(201, 'Electiva IV', 3, 6, 9, 0),
(202, 'Prácticas Profesionales', 18, 6, 10, 0),
(203, 'Fundamento de la Física', 0, 8, 0, 0),
(204, 'Matemática Básica', 0, 8, 0, 0),
(205, 'Lenguaje y Comunicación', 0, 8, 0, 0),
(206, 'Cátedra Libre: Antonio W.Golbroüner: Fundamentos de las Ciencias Atmosféricas ', 0, 8, 0, 0),
(207, 'Educación Física, Deporte y Cultura', 0, 8, 0, 0),
(208, 'Introducción a la Universidad y al Programa PFGLM', 0, 8, 0, 0),
(209, 'Matemática I', 4, 8, 1, 0),
(210, 'Ingles I', 2, 8, 1, 0),
(211, 'Química Inorgánica', 4, 8, 1, 0),
(212, 'Vivencial I: Geografía Física', 3, 8, 1, 0),
(213, 'Física I', 4, 8, 1, 0),
(214, 'Seminario I: Técnicas de Computación para Meteorología', 2, 8, 1, 0),
(215, 'PFGLMSI-SIIPSI; PROYECTO SOCIOINTEGRADOR I: Delimitación y Caracterización del Espacio Geográfico para el Estudio Meteorológico y sus Aplicaciones ', 9, 8, 1, 0),
(216, 'Física II', 4, 8, 2, 0),
(217, 'Matemática II', 4, 8, 2, 0),
(218, 'Química Orgánica', 4, 8, 2, 0),
(219, 'Vivencial II: Cartografía e interpretación de Mapas', 3, 8, 2, 0),
(220, 'Ingles II', 2, 8, 2, 0),
(221, 'Taller I: Introducción a la Ciencia del Suelo', 2, 8, 2, 0),
(222, 'Meteorología Física', 3, 8, 3, 0),
(223, 'Matemática III', 4, 8, 3, 0),
(224, 'Física III', 4, 8, 3, 0),
(225, 'Estadística Básica', 4, 8, 3, 0),
(226, 'Seminario II: Inglés III', 2, 8, 3, 0),
(227, 'Vivencial III: Geomática', 3, 8, 3, 0),
(228, 'PFGLMSI-SIVPSI; PROYECTO SOCIOINTEGRADOR II: Técnica, Observación e Instrumentación Meteorológica', 9, 8, 3, 0),
(229, 'Termodinámica de la Atmósfera', 3, 8, 4, 0),
(230, 'Matemática IV', 4, 8, 4, 0),
(231, 'Métodos Estadísticos Aplicado a la Meteorología', 3, 8, 4, 0),
(232, 'Vivencial IV: Climatología', 3, 8, 4, 0),
(233, 'Taller II: Edafología', 2, 8, 4, 0),
(234, 'Álgebra Lineal', 4, 8, 4, 0),
(235, 'Códigos Meteorológicos', 3, 8, 5, 0),
(236, 'Vivencial V: Ecología', 3, 8, 5, 0),
(237, 'Seminario III: Variabilidad del Clima y Cambio Climático', 2, 8, 5, 0),
(238, 'Hidrología', 3, 8, 5, 0),
(239, 'Meteorología Agrícola', 2, 8, 5, 0),
(240, 'Electiva I', 2, 8, 5, 0),
(241, 'PFGLMSV-SVIPSI; PROYECTO SOCIOINTEGRADOR III: Observaciones Aplicadas A Sistemas Atmosféricos, Hidrológicos Y Socioproductivos ', 9, 8, 5, 0),
(242, 'Meteorología Dinámica', 3, 8, 6, 0),
(243, 'Vivencial VI: Gestión Integral de Cuencas', 3, 8, 6, 0),
(244, 'Taller III: Hidrología Subterránea', 2, 8, 6, 0),
(245, 'Métodos Numéricos', 4, 8, 6, 0),
(246, 'Física de las Nubes', 3, 8, 6, 0),
(247, 'Electiva II', 2, 8, 6, 0),
(248, 'Introducción a la Modelización Climática', 3, 8, 7, 0),
(249, 'Meteorología Sinóptica I', 3, 8, 7, 0),
(250, 'Programación I', 3, 8, 7, 0),
(251, 'Práctica Profesional I: Diagnóstico y Planificación ', 4, 8, 7, 0),
(252, 'Seminario IV: Hidrología Aplicada', 3, 8, 7, 0),
(253, 'Electiva II', 2, 8, 7, 0),
(254, 'PFGLSVII-SVIIIIPSI; Proyecto Sociointegrador IV: Diseño E Innovación de Prototipos Y Sensores A Escala Artesanales Y Científicos De Instrumentos Meteorológicos ', 9, 8, 7, 0),
(255, 'Programación II', 3, 8, 8, 0),
(256, 'Meteorología Sinóptica II', 3, 8, 8, 0),
(257, 'Taller IV: Riesgos Socionaturales y Tecnológicos', 2, 8, 8, 0),
(258, 'Meteorología Urbana y Contaminación Atmosférica', 3, 8, 8, 0),
(259, 'Micrometeorología', 3, 8, 8, 0),
(260, 'Práctica Profesional II: Caracterización y Delimitación', 4, 8, 8, 0),
(261, 'Meteorología Satelital', 4, 8, 9, 0),
(262, 'Meteorología Aeronáutica', 4, 8, 9, 0),
(263, 'Modelos y Predicciones Numéricas', 4, 8, 9, 0),
(264, 'Meteorología Marina y Oceanografía General', 3, 8, 9, 0),
(265, 'Seminario V: Riego y Drenaje', 2, 8, 9, 0),
(266, 'Práctica Profesional III: Ejecución del Plan ', 6, 8, 9, 0),
(267, 'Taller V: Gestión de Calidad', 3, 8, 10, 0),
(268, 'Meteorología Tropical', 4, 8, 10, 0),
(269, 'Trabajo de Aplicación de Conocimientos', 6, 8, 10, 0),
(270, 'Lenguaje y Comunicación', 4, 9, 1, 0),
(271, 'Instrumental', 2, 9, 1, 0),
(272, 'Álgebra y Geometría', 4, 9, 1, 0),
(273, 'Cálculo I', 4, 9, 1, 0),
(274, 'Educación Física y Deporte', 2, 9, 1, 0),
(275, 'Cálculo II', 4, 9, 2, 0),
(276, 'Física I', 4, 9, 2, 0),
(277, 'Dibujo', 2, 9, 2, 0),
(278, 'Informática', 2, 9, 2, 0),
(279, 'Fundamentos de Hidráulica', 2, 9, 2, 0),
(280, 'Electiva I', 2, 9, 2, 0),
(281, 'Tecnología del Concreto', 4, 9, 3, 0),
(282, 'Mecánica Racional', 3, 9, 3, 0),
(283, 'Topografía I', 4, 9, 4, 0),
(284, 'Desarrollo Endógeno', 2, 9, 4, 0),
(285, 'Obras Sanitarias', 3, 9, 4, 0),
(286, 'Electiva II', 2, 9, 3, 0),
(287, 'Costos y Presupuestos', 2, 9, 4, 0),
(288, 'Maquinaria y Equipos', 3, 9, 4, 0),
(289, 'Resistencia de Materiales', 3, 9, 4, 0),
(290, 'Topografía II ', 3, 9, 4, 0),
(291, 'Mecánica de Suelos', 4, 9, 4, 0),
(292, 'Instalaciones para Edificaciones', 3, 9, 4, 0),
(293, 'Construcción de Edificios', 3, 9, 5, 0),
(294, 'Ética y Legislación', 2, 9, 5, 0),
(295, 'Dibujo de Proyectos', 2, 9, 5, 0),
(296, 'Construcción de Carreteras', 3, 9, 5, 0),
(297, 'Administración y Control de Obras', 2, 9, 5, 0),
(298, 'Electiva III', 2, 9, 5, 0),
(299, 'Pasantías', 4, 9, 6, 0),
(300, 'Lenguaje y Comunicación', 2, 10, 1, 0),
(301, 'Educación Física', 4, 10, 1, 0),
(302, 'Cálculo I', 4, 10, 1, 0),
(303, 'Lógica', 2, 10, 1, 0),
(304, 'Informática I', 3, 10, 1, 0),
(305, 'Organización y Sistemas I', 3, 10, 1, 0),
(306, 'Inglés Técnico', 2, 10, 1, 0),
(307, 'Organización Social de La Producción', 4, 10, 2, 0),
(308, 'Cálculo II', 4, 10, 2, 0),
(309, 'Estadística y Probabilidades', 3, 10, 2, 0),
(310, 'Introducción a la Electricidad', 3, 10, 2, 0),
(311, 'Informática II', 2, 10, 2, 0),
(312, 'Lenguaje y Programación I', 2, 10, 2, 0),
(313, 'Organización y Sistemas II', 3, 10, 2, 0),
(314, 'Álgebra', 3, 10, 3, 0),
(315, 'Contabilidad General', 3, 10, 3, 0),
(316, 'Sistema de Información I', 3, 10, 3, 0),
(317, 'Arquitectura del Computador I', 3, 10, 3, 0),
(318, 'Lenguaje y Programación II', 2, 10, 3, 0),
(319, 'Informática III', 2, 10, 3, 0),
(320, 'Inglés Técnico II', 2, 10, 3, 0),
(321, 'Sistema de Información II', 3, 10, 4, 0),
(322, 'Teleprocesos y Comunicación de Datos I', 3, 10, 4, 0),
(323, 'Bases de Datos I', 3, 10, 4, 0),
(324, 'Lenguaje y Programación III', 4, 10, 4, 0),
(325, 'Manejo de Microcomputadores', 3, 10, 4, 0),
(326, 'Paquetes de Aplicación', 4, 10, 4, 0),
(327, 'Técnicas de Investigación Documental', 3, 10, 5, 0),
(328, 'Arquitectura del Computador II', 3, 10, 5, 0),
(329, 'Sistema de Información III', 3, 10, 5, 0),
(330, 'Teleprocesos y Comunicación de Datos II', 3, 10, 5, 0),
(331, 'Bases de Datos II', 2, 10, 5, 0),
(332, 'Lenguaje de Programación IV (Cobol)', 2, 10, 5, 0),
(333, 'Informática y Ética', 2, 10, 5, 0),
(334, 'Pasantías', 10, 10, 6, 0),
(335, 'Servicio Comunitario', 0, 10, 6, 0),
(336, 'Trabajo Especial de Grado', 8, 10, 6, 0),
(337, 'Taller: Introducción A La Universidad Y Al Programa', 2, 11, 0, 0),
(338, 'Taller: Introducción A La Matemática', 2, 11, 0, 0),
(339, 'Introducción A La Química', 2, 11, 0, 0),
(340, 'Seminario: Proyectos Sociointegradores', 2, 11, 0, 0),
(341, 'Comunicación Y Medios Tecnológicos', 2, 11, 0, 0),
(342, 'Seminario I: Estadística e Investigación', 4, 11, 1, 0),
(343, 'Comunicación Y Medios Tecnológicos', 4, 11, 1, 0),
(344, 'Química General', 4, 11, 1, 0),
(345, 'Biología', 4, 11, 1, 0),
(346, 'Matemática General', 4, 11, 1, 0),
(347, 'Vivencial I: Ecología Y Ambiente', 4, 11, 1, 0),
(348, 'Proyecto Socio Integrador I-A: Políticas De Salud Y Plantas Medicinales', 5, 11, 1, 0),
(349, 'Activación Del Desarrollo Psicobiológico I', 4, 11, 2, 0),
(350, 'Inglés', 4, 11, 2, 0),
(351, 'Morfología y Anatomía Vegetal', 4, 11, 2, 0),
(352, 'Química Orgánica', 4, 11, 2, 0),
(353, 'Vivencial II: Investigación Y Agroecología', 4, 11, 2, 0),
(354, 'Electiva I: Desarrollo Sustentable, Políticas De Salud Pública', 2, 11, 2, 0),
(355, 'Proyecto Socio Integrador I-B: Políticas De Salud Y Plantas Medicinales', 4, 11, 2, 0),
(356, 'Bioquímica General', 4, 11, 3, 0),
(357, 'Fisiología Vegetal', 4, 11, 3, 0),
(358, 'Seminario II: Estadística Aplicada', 4, 11, 3, 0),
(359, 'Vivencial III: Sistemas De Producción I', 4, 11, 3, 0),
(360, 'Orientación Integral: Diversidad, Discapacidad Y Accesibilidad', 4, 11, 3, 0),
(361, 'Fitogeografía', 4, 11, 3, 0),
(362, 'Vivencial IV: Sistemas De Producción II', 4, 11, 4, 0),
(363, 'Taller I: Etnometodología', 4, 11, 4, 0),
(364, 'Activación Del Desarrollo Psicobiológico II', 4, 11, 4, 0),
(365, 'Bioquímica Vegetal', 4, 11, 4, 0),
(366, 'Microbiología', 4, 11, 4, 0),
(367, 'Electiva II: Botánica Sistemática', 4, 11, 4, 0),
(368, 'Práctica I: Botánica Aplicada', 4, 11, 5, 0),
(369, 'Biotecnología Vegetal', 4, 11, 5, 0),
(370, 'Seminario III: Etnobotánica I', 4, 11, 5, 0),
(371, 'Botánica Sistemática Avanzada', 4, 11, 5, 0),
(372, 'Fitopatología', 4, 11, 5, 0),
(373, 'Electiva III: Biosistemática, Taxonomía De Plantas Superiores', 4, 11, 5, 0),
(374, 'Proyecto Socio Integrador III-A: Investigación Aplicada', 4, 11, 5, 0),
(375, 'Práctica II: Farmacognosia Y Farmacobotánica', 4, 11, 6, 0),
(376, 'Botánica Económica', 4, 11, 6, 0),
(377, 'Fitoterapia I', 4, 11, 6, 0),
(378, 'Ética Y Legislación Ambiental', 4, 11, 6, 0),
(379, 'Taller II: Etnometodología II', 4, 11, 6, 0),
(380, 'Electiva IV: Toxicología', 2, 11, 6, 0),
(381, 'Proyecto Socio Integrador III-B: Investigación Aplicada', 4, 11, 6, 0),
(382, 'Práctica III: Producción Farmacológica', 4, 11, 7, 0),
(383, 'Seminario IV: Gestión De Salud Botánica', 4, 11, 7, 0),
(384, 'Etnobotánica II', 4, 11, 7, 0),
(385, 'Genética Vegetal', 4, 11, 7, 0),
(386, 'Laboratorio De Micología', 4, 11, 7, 0),
(387, 'Proyecto Socio Integrador IV-A: Diseño, Aplicación Y Evaluación De Propuestas De Proyectos En Botánica', 5, 11, 7, 0),
(388, 'Práctica IV: Emprendimiento Socio Productivo', 4, 11, 8, 0),
(389, 'Proyecto Final De Grado: Experiencia De Investigación En Botánica', 4, 11, 8, 0),
(390, 'Fitoterapia II', 4, 11, 8, 0),
(391, 'Proyecto Nacional, Nueva Ciudadanía Y Programa De Formación De Grado', 2, 12, 0, 0),
(392, 'Uso De Medios Tecnológicos En El Programa De Formación De Grado', 2, 12, 0, 0),
(393, 'Biología', 2, 12, 0, 0),
(394, 'Química', 2, 12, 0, 0),
(395, 'Matemática General', 2, 12, 0, 0),
(396, 'Lenguaje y Comunicación', 2, 12, 0, 0),
(397, 'Teoría Y Práctica De La Comunicación Y El Lenguaje', 4, 12, 1, 0),
(398, 'Psicología En Salud', 4, 12, 1, 0),
(399, 'Estadística Básica Y Medios Tecnológicos', 4, 12, 1, 0),
(400, 'Seminario I: Investigación y Acción Participativa', 4, 12, 1, 0),
(401, 'Educación Física, Cultura Y Deporte', 4, 12, 1, 0),
(402, 'Vivencial I: Contexto Socio Profesional Del Enfermero', 4, 12, 1, 0),
(403, 'Proyecto Socio Integrador I-A: Políticas Públicas En El Sistema De Salud. Nutrición Y Dietética', 5, 12, 1, 0),
(404, 'Morfo Fisiología I', 4, 12, 2, 0),
(405, 'Deontología Y Legislación En Enfermeria', 4, 12, 2, 0),
(406, 'Taller I: Investigación Social', 4, 12, 2, 0),
(407, 'Vivencial II: Introducción A Los Procedimientos Básicos En Enfermería', 4, 12, 2, 0),
(408, 'Ecología y Ambiente', 4, 12, 2, 0),
(409, 'Grupo De Electivas I: Ingles I', 4, 12, 2, 0),
(410, 'Proyecto Socio Integrador I-B: Políticas Públicas En El Sistema De Salud. Nutrición Y Dietética.', 5, 12, 2, 0),
(411, 'Bioquímica', 4, 12, 3, 0),
(412, 'Seminario II: Investigación en Enfermería y Medios Tecnológicos', 4, 12, 3, 0),
(413, 'Vivencial III: Servicios de Enfermería', 4, 12, 3, 0),
(414, 'Morfo Fisiología II', 4, 12, 3, 0),
(415, 'Anatomía Humana I', 4, 12, 3, 0),
(416, 'Ingles II', 4, 12, 3, 0),
(417, 'Proyecto Socio Integrador II-A. Terminología Médica y Registros en Enfermería.', 5, 12, 3, 0),
(418, 'Vivencial IV: Promoción, Prevención e Intervención en Enfermería', 4, 12, 4, 0),
(419, 'Taller II: Investigación en Enfermería', 4, 12, 4, 0),
(420, 'Anatomía Humana II', 4, 12, 4, 0),
(421, 'Farmacología', 4, 12, 4, 0),
(422, 'Enfermería Psiquiátrica y de Salud Mental.', 4, 12, 4, 0),
(423, 'Grupo de Electivas II: Epidemiología', 2, 12, 4, 0),
(424, 'Proyecto Socio Integrador II-B: Terminología Médica y Registros en Enfermería.', 4, 12, 4, 0),
(425, 'Seminario III: Metodología para la Sistematización de experiencias investigativas en Enfermería.', 4, 12, 5, 0),
(426, 'Gerencia Institucional en Salud', 4, 12, 5, 0),
(427, 'Enfermería Quirúrgica I', 4, 12, 5, 0),
(428, 'Práctica I: Atención Primaria en Salud', 4, 12, 5, 0),
(429, 'Grupo de Electivas III: Enfermería Materno Infantil', 2, 12, 5, 0),
(430, 'Taller III: Aplicación de Procedimientos para el Análisis y Sistematización de las Experiencias de Intervención en Enfermería.', 4, 12, 6, 0),
(431, 'Práctica II: Enfermería Pediátrica', 4, 12, 6, 0),
(432, 'Enfermería Quirúrgica II', 4, 12, 6, 0),
(433, 'Fitopatología', 4, 12, 6, 0),
(434, 'Grupo de Electivas IV: Microbiología y Parasitología', 2, 12, 6, 0),
(435, 'Proyecto Socio Integrador III-B: Evaluación y Diseño de Programas de Enfermería.', 5, 12, 6, 0),
(436, 'Concentración Clínica de Enfermería en Áreas Críticas', 4, 12, 7, 0),
(437, 'Enfermería Quirúrgica III', 4, 12, 7, 0),
(438, 'Práctica III: Enfermería Ginecobstétrica', 4, 12, 7, 0),
(439, 'Seminario IV: Configuración de Experiencias', 4, 12, 7, 0),
(440, 'Grupo de Electivas V: Gerontología', 2, 12, 7, 0),
(441, 'Proyecto Socio Integrador IV-A: Implementación de Planes, Programas o Proyectos en Enfermería.', 5, 12, 7, 0),
(442, 'Taller IV: Ejecución y Evaluación de Planes, Programas o Proyectos de Enfermería.', 4, 12, 8, 0),
(443, 'Práctica IV: Clínico Quirúrgica', 4, 12, 8, 0),
(444, 'Proyecto Final: Sistematización de experiencias de investigación.', 4, 12, 8, 0),
(445, 'Proyecto Socio Integrador IV-B: Implementación de Planes, Programas o Proyectos en Enfermería.', 5, 12, 8, 0),
(446, 'Proyecto Nacional y Nueva Ciudadanía', 2, 13, 0, 0),
(447, 'Taller: Introducción a la Universidad y al PFGLES', 2, 13, 0, 0),
(448, 'Seminario: Comunicación e Investigación', 2, 13, 0, 0),
(449, 'Fundamentos Básicos de la Estadística y Salud Pública', 2, 13, 0, 0),
(450, 'Introducción a la Matemática', 2, 13, 0, 0),
(451, 'Medios Tecnológicos y Estadísticas en Salud', 2, 13, 0, 0),
(452, 'Comunicación y Lenguaje', 4, 13, 1, 0),
(453, 'Matemática General', 4, 13, 1, 0),
(454, 'Estadística Básica', 4, 13, 1, 0),
(455, 'Seminario I: Investigación e Informática en Salud', 4, 13, 1, 0),
(456, 'Inglés Instrumental', 4, 13, 1, 0),
(457, 'Vivencial I: Contexto Socioprofesional del Estadístico en Salud', 4, 13, 1, 0),
(458, 'Proyecto Sociointegrador I-A: Aproximación a las Políticas de Salud del Estado Venezolano', 5, 13, 1, 0),
(459, 'Actividad Física, Cultura y Deporte', 4, 13, 2, 0),
(460, 'Legislación y Ética Profesional del Estadístico en Salud', 4, 13, 2, 0),
(461, 'Taller I: Metodología de la Investigación', 4, 13, 2, 0),
(462, 'Vivencial II: Estadística y Registro de Salud I', 3, 13, 2, 0),
(463, 'Anatomía y Fisiología Humana', 4, 13, 2, 0),
(464, 'Electiva I: Pensamiento Político Latinoamericano y Caribeño', 2, 13, 2, 0),
(465, 'Proyecto Sociointegrador I-B: Aproximación a las Políticas de Salud del Estado Venezolano', 4, 13, 2, 0),
(466, 'Terminología Médica', 4, 13, 3, 0),
(467, 'Seminario II: Investigación e Informática en Salud II', 4, 13, 3, 0),
(468, 'Vivencial III: Estadística y Registro de Salud II', 4, 13, 3, 0),
(469, 'Psicología de las Relaciones Humanas', 4, 13, 3, 0),
(470, 'Epidemiología', 4, 13, 3, 0),
(471, 'Principios de la Administración', 4, 13, 3, 0),
(472, 'Proyecto Sociointegrador II-A: Registros Médicos y Estadísticas de la Salud en el Sistema Nacional Público de Salud', 5, 13, 3, 0),
(473, 'Vivencial IV: Estadística y Registro de Salud III', 4, 13, 4, 0),
(474, 'Estadística en Investigación', 4, 13, 4, 0),
(475, 'Taller II: Modelos y Procesos de la Investigación Social', 4, 13, 4, 0),
(476, 'Demografía I', 4, 13, 4, 0),
(477, 'Electiva II: Cosmovisión de la Salud en las Comunidades y Pueblos Indígenas', 2, 13, 4, 0),
(478, 'Proyecto Sociointegrador II-B: Registros Médicos y Estadísticas de Salud en el Sistema Nacional Público de Salud', 5, 13, 4, 0),
(479, 'Proyecto Sociointegrador III-A: Aplicabilidad de los Sistemas de Información al Servicio de la Estadística de Salud', 5, 13, 5, 0),
(480, 'Seminario III: Investigación e Informática en Salud III', 4, 13, 5, 0),
(481, 'Gerencia en las Instituciones de la Salud', 4, 13, 5, 0),
(482, 'Demografía II', 4, 13, 5, 0),
(483, 'Políticas Públicas en Salud', 4, 13, 5, 0),
(484, 'Práctica I: Planificación y Formulación de Proyectos en Salud', 4, 13, 5, 0),
(485, 'Electiva III: Ecología, Ambiente y Bienestar', 2, 13, 5, 0),
(486, 'Taller III: Redacción de Informes y Registro de Salud', 4, 13, 6, 0),
(487, 'Práctica II: Análisis de Registros y Estadísticas de Salud en los Centros Hospitalarios', 4, 13, 6, 0),
(488, 'Administración del Talento Humano', 4, 13, 6, 0),
(489, 'Auditoría de Salud', 4, 13, 6, 0),
(490, 'Electiva IV: Clasificación Internacional de Enfermedades', 4, 13, 6, 0),
(491, 'Proyecto Sociointegrador IV-A: Diseño, Aplicación y Evaluación de Registros de Estadísticas Públicas', 5, 13, 7, 0),
(492, 'Salud Ocupacional y Medicina Legal', 4, 13, 7, 0),
(493, 'Práctica III: Diseño de Programas de Innovación y Sistematización de Registros de Salud', 4, 13, 7, 0),
(494, 'Gerencia del Talento Humano en Salud', 4, 13, 7, 0),
(495, 'Seminario IV: Configuración de Experiencias de Investigación en Registros y Estadísticas de Salud', 4, 13, 7, 0),
(496, 'Electiva V: Técnicas e Instrumentos de Registro en el Área de Salud', 2, 13, 7, 0),
(497, 'Proyecto Sociointegrador IV-B: Aplicabilidad de los Sistemas de Información al Servicio de la Estadística de Salud', 4, 13, 8, 0),
(498, 'Taller IV: Metodología para la Operacionalización de Proyectos de Salud', 4, 13, 8, 0),
(499, 'Práctica IV: Desempeño del Licenciado en Estadísticas de Salud', 4, 13, 8, 0),
(500, 'Proyecto Final: Sistematización de la Práctica de Investigación', 4, 13, 8, 0),
(501, 'Ecología y Ambiente', 3, 14, 1, 0),
(502, 'Matemática I', 4, 14, 1, 0),
(503, 'Inglés Instrumental', 3, 14, 1, 0),
(504, 'Entrenamiento Físico General', 3, 14, 1, 0),
(505, 'Lenguaje y Comunicación', 4, 14, 1, 0),
(506, 'Historia Económica y Social de Venezuela', 2, 14, 1, 0),
(507, 'Orientación Institucional', 3, 14, 1, 0),
(508, 'Computación', 2, 14, 2, 0),
(509, 'Matemática II', 4, 14, 2, 0),
(510, 'Principios de Teoría Económica', 3, 14, 2, 0),
(511, 'Contabilidad Financiera I', 3, 14, 2, 0),
(512, 'Estadística Descriptiva', 3, 14, 2, 0),
(513, 'Organización Deportiva', 3, 14, 2, 0),
(514, 'Electiva I', 2, 14, 2, 0),
(515, 'Producción Vegetal', 2, 14, 3, 0),
(516, 'Microeconomía I', 3, 14, 3, 0),
(517, 'Mercadotecnia', 2, 14, 3, 0),
(518, 'Contabilidad Financiera II', 3, 14, 3, 0),
(519, 'Estadística Inferencial', 3, 14, 3, 0),
(520, 'Metodología de la Investigación', 3, 14, 3, 0),
(521, 'Producción Animal', 2, 14, 4, 0),
(522, 'Microeconomía II', 3, 14, 4, 0),
(523, 'Contabilidad Social', 3, 14, 4, 0),
(524, 'Práctica I: Comercialización de Productos Agrícolas', 4, 14, 4, 0),
(525, 'Computación Aplicada', 2, 14, 4, 0),
(526, 'Electiva II', 2, 14, 4, 0),
(527, 'Contabilidad Agropecuaria', 3, 14, 5, 0),
(528, 'Matemática Financiera', 4, 14, 5, 0),
(529, 'Microeconomía III', 3, 14, 5, 0),
(530, 'Investigación de Mercado', 3, 14, 5, 0),
(531, 'Administración de Fincas', 3, 14, 5, 0),
(532, 'Investigación Social', 3, 14, 5, 0),
(533, 'Planificación de Fincas', 3, 14, 6, 0),
(534, 'Desarrollo Económico', 3, 14, 6, 0),
(535, 'Teoría y Política Monetaria', 3, 14, 6, 0),
(536, 'Econometría', 3, 14, 6, 0),
(537, 'Práctica II: Valoración de Bienes Agrícolas', 4, 14, 6, 0),
(538, 'Electiva III', 2, 14, 6, 0),
(539, 'Planificación Agrícola', 3, 14, 7, 0),
(540, 'Teoría y Política Fiscal', 3, 14, 7, 0),
(541, 'Comercio y Finanzas Internacionales', 3, 14, 7, 0),
(542, 'Investigación de Operaciones', 3, 14, 7, 0),
(543, 'Formulación y Evaluación de Proyectos I', 3, 14, 7, 0),
(544, 'Seminario de Investigación', 3, 14, 7, 0),
(545, 'Política y Desarrollo Agrícola', 3, 14, 8, 0),
(546, 'Dinámica Económica', 3, 14, 8, 0),
(547, 'Mercado de Capitales', 3, 14, 8, 0),
(548, 'Presupuesto Público', 3, 14, 8, 0),
(549, 'Formulación y Evaluación De Proyectos II', 3, 14, 8, 0),
(550, 'Ética Profesional', 2, 14, 8, 0),
(551, 'Pasantía Profesional', 6, 14, 9, 0),
(552, 'Trabajo de Grado', 4, 14, 9, 0),
(553, 'Cálculo I', 3, 15, 1, 0),
(554, 'Organización Social de la Producción I', 4, 15, 1, 0),
(555, 'Educación Física y Deportes', 2, 15, 1, 0),
(556, 'Metodología de la Investigación', 3, 15, 1, 0),
(557, 'Lenguaje y Comunicación', 3, 15, 1, 0),
(558, 'Introducción a los Medios de Comunicación', 3, 15, 1, 0),
(559, 'Inglés', 2, 15, 1, 0),
(560, 'Electiva I', 2, 15, 2, 0),
(561, 'Organización Social de la Producción II', 4, 15, 2, 0),
(562, 'Cálculo II', 3, 15, 2, 0),
(563, 'Física I', 3, 15, 2, 0),
(564, 'Dibujo', 2, 15, 2, 0),
(565, 'Química', 4, 15, 2, 0),
(566, 'Biología', 3, 15, 2, 0),
(567, 'Informática', 2, 15, 3, 0),
(568, 'Organización Social de la Producción III', 2, 15, 3, 0),
(569, 'Física II', 3, 15, 3, 0),
(570, 'Cálculo III', 3, 15, 3, 0),
(571, 'Química Orgánica', 3, 15, 3, 0),
(572, 'Química II', 3, 15, 3, 0),
(573, 'Microbiología General', 3, 15, 3, 0),
(574, 'Electiva II', 2, 15, 4, 0),
(575, 'Cálculo IV', 3, 15, 4, 0),
(576, 'Estadística', 3, 15, 4, 0),
(577, 'Principios de Ingeniería I', 3, 15, 4, 0),
(578, 'Principios de Diseño', 3, 15, 4, 0),
(579, 'Bioquímica General', 3, 15, 4, 0),
(580, 'Microbiología Aplicada', 3, 15, 4, 0),
(581, 'Administración Industrial I', 3, 15, 5, 0),
(582, 'Principios de Ingeniería II', 3, 15, 5, 0),
(583, 'Instalaciones Agroindustriales', 3, 15, 5, 0),
(584, 'Instrumentación y Control', 3, 15, 5, 0),
(585, 'Bioquímica Aplicada', 3, 15, 5, 0),
(586, 'Fisiología y Tecnología Postcosecha', 3, 15, 5, 0),
(587, 'Fisiología y Beneficio Animal', 3, 15, 5, 0),
(588, 'Extensión y Desarrollo Rural', 2, 15, 6, 0),
(589, 'Electiva III', 2, 15, 6, 0),
(590, 'Administración Industrial II', 3, 15, 6, 0),
(591, 'Control y Aseguramiento de la Calidad', 2, 15, 6, 0),
(592, 'Agroindustria Vegetal I', 4, 15, 6, 0),
(593, 'Agroindustria Animal I', 4, 15, 6, 0),
(594, 'Administración Industrial III', 3, 15, 7, 0),
(595, 'Agroindustria Vegetal II', 4, 15, 7, 0),
(596, 'Laboratorio I Agroindustria Vegetal', 2, 15, 7, 0),
(597, 'Agroindustria Animal II', 4, 15, 7, 0),
(598, 'Laboratorio I Agroindustria Animal', 2, 15, 7, 0),
(599, 'Electiva IV', 2, 15, 7, 0),
(600, 'Ética y Legislación Laboral', 3, 15, 8, 0),
(601, 'Gerencia y Planificación de Proyectos', 3, 15, 8, 0),
(602, 'Optimización de Procesos', 3, 15, 8, 0),
(603, 'Seguridad e Higiene Industrial', 2, 15, 8, 0),
(604, 'Desechos Agroindustriales', 3, 15, 8, 0),
(605, 'Laboratorio II Agroindustria Vegetal', 2, 15, 8, 0),
(606, 'Laboratorio II Agroindustria Animal', 2, 15, 8, 0),
(607, 'Servicio Social Comunitario', 0, 15, 9, 0),
(608, 'Trabajo de Grado', 7, 15, 9, 0),
(609, 'Pasantías', 7, 15, 9, 0),
(610, 'Lenguaje y Comunicación', 3, 16, 1, 0),
(611, 'Inglés I', 2, 16, 1, 0),
(612, 'Cálculo I', 3, 16, 1, 0),
(613, 'Cultura', 2, 16, 1, 0),
(614, 'Educación Física y Deporte', 2, 16, 1, 0),
(615, 'Organización Social de la Producción', 3, 16, 2, 0),
(616, 'Inglés II', 2, 16, 2, 0),
(617, 'Cálculo II', 3, 16, 2, 0),
(618, 'Dibujo', 2, 16, 2, 0),
(619, 'Química', 3, 16, 2, 0),
(620, 'Electiva I', 2, 16, 2, 0),
(621, 'Morfología y Anatomía Vegetal', 3, 16, 3, 0),
(622, 'Física', 3, 16, 3, 0),
(623, 'Topografía Cartográfica', 3, 16, 3, 0),
(624, 'Cálculo III', 3, 16, 3, 0),
(625, 'Informática', 3, 16, 3, 0),
(626, 'Química Orgánica', 3, 16, 3, 0),
(627, 'Botánica Sistemática', 3, 16, 4, 0),
(628, 'Estadística', 3, 16, 4, 0),
(629, 'Cereales y Leguminosas', 3, 16, 4, 0),
(630, 'Hortalizas, Raíces y Tubérculos', 3, 16, 4, 0),
(631, 'Bioquímica', 3, 16, 4, 0),
(632, 'Edafología', 3, 16, 4, 0),
(633, 'Bioquímica General', 3, 16, 4, 0),
(634, 'Microbiología Aplicada', 3, 16, 4, 0),
(635, 'Potencialidades del Sector Agroindustrial', 2, 16, 4, 0),
(636, 'Cultivos Semi-perennes', 3, 16, 5, 0),
(637, 'Mecanización Agrícola', 3, 16, 5, 0),
(638, 'Climatología', 3, 16, 5, 0),
(639, 'Relaciones Humanas', 2, 16, 5, 0),
(640, 'Edafología Aplicada', 3, 16, 5, 0),
(641, 'Electiva II', 2, 16, 5, 0),
(642, 'Diseño Experimental', 3, 16, 6, 0),
(643, 'Hidrología', 3, 16, 6, 0),
(644, 'Cultivos Perennes I', 3, 16, 6, 0),
(645, 'Vivencial', 2, 16, 6, 0),
(646, 'Fisiología Vegetal', 4, 16, 6, 0),
(647, 'Trabajo Social Comunitario', 1, 16, 6, 0),
(648, 'Economía y Mercadeo Agrícola', 2, 16, 7, 0),
(649, 'Entomología', 4, 16, 7, 0),
(650, 'Drenaje y Riego', 4, 16, 7, 0),
(651, 'Ética', 2, 16, 7, 0),
(652, 'Combate de Malezas', 3, 16, 7, 0),
(653, 'Electiva III', 3, 16, 7, 0),
(654, 'Genética', 4, 16, 8, 0),
(655, 'Administración de Empresas Agropecuarias', 3, 16, 8, 0),
(656, 'Derecho Agrario y Ambiental', 2, 16, 8, 0),
(657, 'Conservación de Suelos y Aguas', 3, 16, 8, 0),
(658, 'Agroecología', 2, 16, 8, 0),
(659, 'Electiva IV', 2, 16, 8, 0),
(660, 'Gerencia de Empresas', 3, 16, 9, 0),
(661, 'Formulación y Evaluación de Proyectos', 3, 16, 9, 0),
(662, 'Aplicación de conocimientos I', 2, 16, 9, 0),
(663, 'Extensión Agrícola y Desarrollo Rural', 3, 16, 9, 0),
(664, 'Fitopatología', 4, 16, 9, 0),
(665, 'Electiva V', 2, 16, 9, 0),
(666, 'Pasantía Profesional', 6, 16, 10, 0),
(667, 'Aplicación de conocimientos II', 3, 16, 10, 0),
(668, 'Cálculo I', 3, 17, 1, 0),
(669, 'Cultura', 2, 17, 1, 0),
(670, 'Educación Física y Deporte', 2, 17, 1, 0),
(671, 'Lenguaje y Comunicación', 3, 17, 1, 0),
(672, 'Inglés I', 2, 17, 1, 0),
(673, 'Cálculo II', 3, 17, 2, 0),
(674, 'Dibujo', 2, 17, 2, 0),
(675, 'Química', 3, 17, 2, 0),
(676, 'Biología', 3, 17, 2, 0),
(677, 'Organización Social de la Producción', 3, 17, 2, 0),
(678, 'Especialidad deportiva', 2, 17, 2, 0),
(679, 'Interpretación de Textos en Inglés', 2, 17, 2, 0),
(680, 'Redacción y presentación de textos expositivos', 2, 17, 2, 0),
(681, 'Electiva I', 2, 17, 2, 0),
(682, 'Cálculo III', 3, 17, 3, 0),
(683, 'Botánica', 3, 17, 3, 0),
(684, 'Química Orgánica', 3, 17, 3, 0),
(685, 'Ecología Animal', 2, 17, 3, 0),
(686, 'Zoología', 2, 17, 3, 0),
(687, 'Topografía Cartográfica', 3, 17, 3, 0),
(688, 'Informática', 3, 17, 3, 0),
(689, 'Bioquímica', 3, 17, 4, 0),
(690, 'Física', 3, 17, 4, 0),
(691, 'Anatomía Animal', 2, 17, 4, 0),
(692, 'Estadística', 3, 17, 4, 0),
(693, 'Edafología', 3, 17, 4, 0),
(694, 'Electiva II', 2, 17, 4, 0),
(695, 'Especialidad Cultural', 2, 17, 4, 0),
(696, 'Ética y Valores Sociales', 2, 17, 4, 0),
(697, 'Tecnología de la Información y la Comunicación', 2, 17, 4, 0),
(698, 'Prácticas Profesionales con Productores', 2, 17, 4, 0),
(699, 'Fisiología Animal', 3, 17, 5, 0),
(700, 'Microbiología', 3, 17, 5, 0),
(701, 'Mecanización Agrícola', 3, 17, 5, 0),
(702, 'Acuicultura', 2, 17, 5, 0),
(703, 'Equinos-Cunicultura', 2, 17, 5, 0),
(704, 'Sistema de Producción de Búfalos', 2, 17, 5, 0),
(705, 'Vivencial', 2, 17, 5, 0),
(706, 'Diseño Experimental', 3, 17, 6, 0),
(707, 'Forrajicultura Tropical', 3, 17, 6, 0),
(708, 'Genética y Mejoramiento', 3, 17, 6, 0),
(709, 'Nutrición y Alimentación', 3, 17, 6, 0),
(710, 'Reproducción Animal', 2, 17, 6, 0),
(711, 'Turismo Agroecológico', 2, 17, 6, 0),
(712, 'S.P Fauna Silvestre y Especies Promisorias', 2, 17, 6, 0),
(713, 'Electiva III', 2, 17, 6, 0),
(714, 'Sensores Remotos y S.I.G.', 3, 17, 6, 0),
(715, 'Sistemas Agropecuarios Sostenibles', 2, 17, 6, 0),
(716, 'Sanidad Animal', 3, 17, 7, 0),
(717, 'Economía y Mercado', 2, 17, 7, 0),
(718, 'Legislación Agraria', 2, 17, 7, 0),
(719, 'Sistemas de Producción con Aves', 2, 17, 7, 0),
(720, 'S.P. Bovinos de Carne', 3, 17, 7, 0),
(721, 'S.P Ovinos-Caprinos', 2, 17, 7, 0),
(722, 'Tecnología de la Leche y Carne', 2, 17, 7, 0),
(723, 'Diseño de Infraestructuras Pecuarias', 2, 17, 8, 0),
(724, 'Simulación de Pastizales', 2, 17, 8, 0),
(725, 'Drenaje y Riego', 2, 17, 8, 0),
(726, 'Energías Alternativas', 2, 17, 8, 0),
(727, 'Administración de Empresas Agropecuarias', 2, 17, 8, 0),
(728, 'Apicultura', 2, 17, 8, 0),
(729, 'S.P. Bovinos Doble Propósito', 3, 17, 8, 0),
(730, 'S.P. Porcinos', 2, 17, 8, 0),
(731, 'Tecnología Alternativa Conservación Prod. Agropecuarios', 2, 17, 8, 0),
(732, 'Formulación y Evaluación de Proyectos', 3, 17, 9, 0),
(733, 'Extensión y Desarrollo Rural', 3, 17, 9, 0),
(734, 'Gerencia de Empresas', 3, 17, 9, 0),
(735, 'Impacto y Soluciones Ambientales Prod. Animal', 2, 17, 9, 0),
(736, 'Sistemas Agro Silvopastoriles', 2, 17, 9, 0),
(737, 'Aplicación de Conocimientos I', 2, 17, 9, 0),
(738, 'Servicio Comunitario*', 0, 17, 10, 0),
(739, 'Introducción a la Universidad y al PNF en Medicina Veterinaria', 0, 18, 0, 0),
(740, 'Proyecto Nacional Nueva Ciudadanía, Lectura y Comprensión', 0, 18, 0, 0),
(741, 'Matemática', 0, 18, 0, 0),
(742, 'Química', 0, 18, 0, 0),
(743, 'Biología', 0, 18, 0, 0),
(744, 'Ciencias Tecnológicas (TIC)', 0, 18, 0, 0),
(745, 'Proyecto I - A. Contexto y elementos que integran la realidad en la salud animal.', 4, 18, 1, 0),
(746, 'Anatomía de los animales domésticos', 5, 18, 1, 0),
(747, 'Histología Veterinaria', 3, 18, 1, 0),
(748, 'Sistemas de Producción Animal I', 3, 18, 1, 0),
(749, 'Bioquímica I', 2, 18, 1, 0),
(750, 'Estadística Descriptiva', 2, 18, 1, 0),
(751, 'Pastos y Forraje', 2, 18, 1, 0),
(752, 'Proyecto I – B. Contexto y elementos que integran la realidad en la salud animal.', 4, 18, 2, 0),
(753, 'Anatomía Comparada de los animales domésticos', 5, 18, 2, 0),
(754, 'Histología y Embriología', 3, 18, 2, 0),
(755, 'Sistemas de Producción Animal II', 3, 18, 2, 0),
(756, 'Bioquímica II', 2, 18, 2, 0),
(757, 'Alimentación y Nutrición Animal monogástricos', 2, 18, 2, 0),
(758, 'Inglés Técnico I', 1, 18, 2, 0),
(759, 'Electiva I: Uso y Aprovechamiento de los desechos Orgánicos', 1, 18, 2, 0),
(760, 'Proyecto II – A. Producción y manejo animal.', 4, 18, 3, 0),
(761, 'Fisiología Animal Básica', 3, 18, 3, 0),
(762, 'Manejo de Sistemas de Producción Animal I', 3, 18, 3, 0),
(763, 'Alimentación y Nutrición de poligástricos', 2, 18, 3, 0),
(764, 'Microbiología Veterinaria', 3, 18, 3, 0),
(765, 'Parasitología y Enfermedades Parasitaria', 3, 18, 3, 0),
(766, 'Actividad Recreativa y Cultural', 1, 18, 3, 0),
(767, 'Electiva II: *Orientación e Integración Psicosocial.', 1, 18, 3, 0),
(768, 'Proyecto II – B. Producción y Manejo Animal.', 4, 18, 4, 0),
(769, 'Fisiología Animal Aplicada', 3, 18, 4, 0),
(770, 'Inmunología', 2, 18, 4, 0),
(771, 'Bioestadística', 2, 18, 4, 0),
(772, 'Farmacología y Toxicología', 3, 18, 4, 0),
(773, 'Manejo de Sistemas de Producción Animal II', 3, 18, 4, 0),
(774, 'Genética', 2, 18, 4, 0),
(775, 'Inglés Técnico II', 1, 18, 4, 0),
(776, 'Proyecto III – A. Salud animal.', 4, 18, 5, 0),
(777, 'Mejoramiento Genético', 2, 18, 5, 0),
(778, 'Enfermedades Infectocontagiosas', 2, 18, 5, 0),
(779, 'Semiología', 3, 18, 5, 0),
(780, 'Anatomía Patológica', 3, 18, 5, 0),
(781, 'Laboratorio Clínico', 3, 18, 5, 0),
(782, 'Ética y Deontología Veterinaria', 1, 18, 5, 0),
(783, 'Imagenología Animal', 2, 18, 5, 0),
(784, 'Proyecto III – B. Salud Animal.', 4, 18, 6, 0),
(785, 'Práctica Profesional I', 5, 18, 6, 0),
(786, 'Salud Pública', 2, 18, 6, 0),
(787, 'Cirugía I (Instrumentación)', 3, 18, 6, 0),
(788, 'Introducción a la Clínica Veterinaria', 3, 18, 6, 0),
(789, 'Reproducción Animal I', 2, 18, 6, 0),
(790, 'Enfermedades no Infecciosas', 2, 18, 6, 0),
(791, 'Proyecto IV – A. Agentes Epidemiológicos', 4, 18, 7, 0),
(792, 'Epidemiología', 2, 18, 7, 0),
(793, 'Higiene de los Alimentos', 2, 18, 7, 0),
(794, 'Clínica I', 3, 18, 7, 0),
(795, 'Cirugía II', 3, 18, 7, 0),
(796, 'Reproducción Animal II', 2, 18, 7, 0),
(797, 'Inglés Técnico III', 1, 18, 7, 0),
(798, 'Electiva III: Fauna Silvestre', 1, 18, 7, 0),
(799, 'Proyecto IV – B. Agentes Epidemiológicos', 4, 18, 8, 0),
(800, 'Prevención y Riesgo en la Profesión', 2, 18, 8, 0),
(801, 'Clínica II', 3, 18, 8, 0),
(802, 'Medicina de Rumiantes', 3, 18, 8, 0),
(803, 'Industria de Productos Cárnicos', 3, 18, 8, 0),
(804, 'Gestión Agropecuaria', 2, 18, 8, 0),
(805, 'Electiva IV: Organización Comunitaria', 1, 18, 8, 0),
(806, 'Proyecto V – A. Aplicación de Diseño Experimental', 4, 18, 9, 0),
(807, 'Medicina en Equinos, Caninos y Otras Especies', 3, 18, 9, 0),
(808, 'Legislación Veterinaria', 2, 18, 9, 0),
(809, 'Biotecnología en Ciencias Veterinarias', 3, 18, 9, 0),
(810, 'Industria de Productos Lácteos', 3, 18, 9, 0),
(811, 'Proyecto V – B. Resultados de Diseño Experimental.', 4, 18, 10, 0),
(812, 'Práctica Profesional II', 9, 18, 10, 0),
(813, 'Medicina de Cerdos y Aves', 3, 18, 10, 0),
(814, 'Terapias Alternativas para la Medicina Veterinaria', 2, 18, 10, 0),
(846, 'Matemática General', 3, 20, 1, 1),
(847, 'Lenguaje y Comunicación', 4, 20, 1, 1),
(848, 'Administración I', 4, 20, 1, 1),
(849, 'Ecología y Educación Ambiental', 2, 20, 1, 1),
(850, 'Inglés Instrumental', 2, 20, 1, 1),
(851, 'Espacio Geográfico e Identidad Nacional', 2, 20, 1, 1),
(852, 'Educación Física, Deporte y Recreación', 2, 20, 1, 1),
(853, 'Matemática para Administradores', 3, 20, 2, 1),
(854, 'Administración II', 4, 20, 2, 1),
(855, 'Contabilidad Financiera I', 4, 20, 2, 1),
(856, 'Informática', 3, 20, 2, 1),
(857, 'Metodología de la Investigación', 3, 20, 2, 1),
(858, 'Electiva I', 2, 20, 2, 1),
(859, 'Legislación', 3, 20, 3, 2),
(860, 'Contabilidad Financiera II', 4, 20, 3, 2),
(861, 'Estadística para Administradores I', 3, 20, 3, 2),
(862, 'Matemática Financiera', 3, 20, 3, 2),
(863, 'Diseño Organizacional', 3, 20, 3, 2),
(864, 'Fundamentos Económicos', 4, 20, 3, 2),
(865, 'Derecho Laboral y Seguridad Social', 3, 20, 4, 2),
(866, 'Estadística para Administradores II', 3, 20, 4, 2),
(867, 'Contabilidad Administrativa I', 4, 20, 4, 2),
(868, 'Sistemas y Procedimientos Administrativos', 3, 20, 4, 2),
(869, 'Macroeconomía', 3, 20, 4, 2),
(870, 'Participación Ciudadana y Desarrollo Local', 2, 20, 4, 2),
(871, 'Desarrollo Personal y Ética Profesional', 2, 20, 4, 2),
(872, 'Investigación Social', 2, 20, 5, 3),
(873, 'Marketing', 3, 20, 5, 3),
(874, 'Comportamiento Organizacional', 3, 20, 5, 3),
(875, 'Contabilidad Administrativa II', 3, 20, 5, 3),
(876, 'Administración de Recursos Humanos', 3, 20, 5, 3),
(877, 'Administración de Operaciones', 3, 20, 5, 3),
(878, 'Administración Pública', 3, 20, 5, 3),
(879, 'Investigación de Mercado', 3, 20, 6, 3),
(880, 'Desarrollo Organizacional', 3, 20, 6, 3),
(881, 'Administración Financiera I', 4, 20, 6, 3),
(882, 'Software de Aplicación Administrativa', 2, 20, 6, 3),
(883, 'Tributación', 3, 20, 6, 3),
(884, 'Presupuesto Empresarial', 3, 20, 6, 3),
(885, 'Electiva II', 2, 20, 6, 3),
(886, 'Administración Financiera II', 3, 20, 7, 4),
(887, 'Formulación, Evaluación y Administración de Proyectos', 3, 20, 7, 4),
(888, 'Auditoría Administrativa', 3, 20, 7, 4),
(889, 'Sistemas de Información Gerencial', 3, 20, 7, 4),
(890, 'Práctica Profesional I (PPI): Trabajo de Aplicación', 3, 20, 7, 4),
(891, 'Finanzas y Presupuesto del Sector Público', 3, 20, 7, 4),
(892, 'Práctica Profesional II (PPII): Pasantías', 4, 20, 8, 4),
(893, 'Trabajo de Aplicación', 8, 20, 8, 4),
(894, 'Electiva III', 2, 20, 8, 4),
(895, 'Proyecto Nacional y Nueva Ciudadanía', 2, 21, 0, 0),
(896, 'Taller Introducción a la Universidad y al Programa', 2, 21, 0, 0),
(897, 'Introducción a la Matemática', 2, 21, 0, 0),
(898, 'Taller Capacidades Críticas, Analíticas y Dialógicas', 2, 21, 0, 0),
(899, 'Comunicación e Investigación', 2, 21, 0, 0),
(900, 'Proyecto Sociointegrador I-A: Ética Profesional y Desarrollo Organizacional', 5, 21, 1, 1),
(901, 'Lengua, Comunicación, y Medios Tecnológicos', 4, 21, 1, 1),
(902, 'Matemática General', 4, 21, 1, 1),
(903, 'Vivencial I: Sistemas de Producción I Pecuaria', 4, 21, 1, 1),
(904, 'Seminario I: Investigación y Acción Participativa', 4, 21, 1, 1),
(905, 'Deporte, Cultura y Recreación', 4, 21, 1, 1),
(906, 'Proyecto Socio Integrador I-B: Ética Profesional y Desarrollo Organizacional', 4, 21, 2, 1),
(907, 'Inglés Instrumental', 4, 21, 2, 1),
(908, 'Taller I: Investigación Social', 4, 21, 2, 1),
(909, 'Vivencial II: Administración Agropecuaria', 4, 21, 2, 1),
(910, 'Matemática Financiera', 4, 21, 2, 1),
(911, 'Contabilidad General', 4, 21, 2, 1),
(912, 'Electiva I: Legislación Fiscal', 2, 21, 2, 1),
(913, 'Proyecto Sociointegrador II-A: Formulación de Proyectos Agropecuarios', 5, 21, 3, 2),
(914, 'Contabilidad Agropecuaria', 4, 21, 3, 2),
(915, 'Administración', 4, 21, 3, 2),
(916, 'Vivencial III: Sistemas de Producción II Vegetal', 4, 21, 3, 2),
(917, 'Taller II: Orientación e Investigación', 4, 21, 3, 2),
(918, 'Electiva II: Agroecología', 2, 21, 3, 2),
(919, 'Proyecto Sociointegrador II-B: Formulación de Proyectos Agropecuarios', 4, 21, 4, 2),
(920, 'Administración Financiera I', 4, 21, 4, 2),
(921, 'Taller III: Desarrollo Rural Agropecuario', 4, 21, 4, 2),
(922, 'Vivencial IV: Sistemas de Producción II Acuícola', 4, 21, 4, 2),
(923, 'Administración de la Producción', 4, 21, 4, 2),
(924, 'Seminario II: Metodología de la Investigación en Administración Agropecuaria', 4, 21, 4, 2),
(925, 'Electiva III: Banca y Exportación', 2, 21, 4, 2),
(926, 'Administración Financiera II', 4, 21, 5, 3),
(927, 'Práctica I: Planificación de Finca', 4, 21, 5, 3),
(928, 'Estadística', 4, 21, 5, 3),
(929, 'Administración de Costo I', 4, 21, 5, 3),
(930, 'Comercialización Agropecuaria', 4, 21, 5, 3),
(931, 'Electiva IV: Relaciones Interpersonales e Interinstitucionales', 2, 21, 5, 3),
(932, 'Proyecto Socio Integrador III-A: Evaluación de Proyectos Agropecuarios', 5, 21, 5, 3),
(933, 'Electiva V: Cooperativismo Agrario', 2, 21, 6, 3),
(934, 'Práctica II: Avalúo de Finca', 4, 21, 6, 3),
(935, 'Proyecto Socio Integrador III-B: Evaluación de Proyectos Agropecuarios', 4, 21, 6, 3),
(936, 'Taller IV: Auditoria Administrativa', 4, 21, 6, 3),
(937, 'Administración de Costo II', 4, 21, 6, 3),
(938, 'Presupuesto y Planificación', 4, 21, 6, 3),
(939, 'Seminario III: Sistematización de Experiencias de Investigación Agropecuaria', 4, 21, 6, 3),
(940, 'Software de Aplicación', 4, 21, 7, 4),
(941, 'Práctica III: Maquinaria e Infraestructura', 4, 21, 7, 4),
(942, 'Derecho Laboral', 4, 21, 7, 4),
(943, 'Seminario IV: Investigación de Operaciones', 4, 21, 7, 4),
(944, 'Mercadotecnia', 4, 21, 7, 4),
(945, 'Proyecto Socio Integrador IV-A: Ejecución y Evaluación de Proyecto', 5, 21, 7, 4),
(946, 'Taller V: Derecho Agrario', 4, 21, 8, 4),
(947, 'Práctica IV: Presentación y Socialización del Trabajo de Aplicación', 4, 21, 8, 4),
(948, 'Proyecto Final: Sistematización de Experiencia de Investigación', 4, 21, 8, 4),
(949, 'Proyecto Socio Integrador IV-B: Ejecución y Evaluación de Proyecto', 4, 21, 8, 4),
(950, 'Lenguaje y Comunicación', 4, 23, 1, 1),
(951, 'Historia Económica y Social de Venezuela', 2, 23, 1, 1),
(952, 'Sociología', 2, 23, 1, 1),
(953, 'Ecología y Educación Ambiental', 2, 23, 1, 1),
(954, 'Inglés Instrumental', 2, 23, 1, 1),
(955, 'Matemática General', 4, 23, 1, 1),
(956, 'Antropología', 4, 23, 2, 1),
(957, 'Entorno Económico', 4, 23, 2, 1),
(958, 'Filosofía Social', 2, 23, 2, 1),
(959, 'Espacio Geográfico e Identidad Nacional', 2, 23, 2, 1),
(960, 'Informática', 2, 23, 2, 1),
(961, 'Legislación Social', 4, 23, 2, 1),
(962, 'Metodología de la Investigación', 4, 23, 3, 2),
(963, 'Teoría Social Clásica I', 4, 23, 3, 2),
(964, 'Estadística Básica', 3, 23, 3, 2),
(965, 'Teorías del Desarrollo y Políticas Públicas', 2, 23, 3, 2),
(966, 'Sociología y Procesos Educativos', 2, 23, 3, 2),
(967, 'Electiva I', 2, 23, 3, 2),
(968, 'Análisis Estadístico con SPSS', 2, 23, 4, 2),
(969, 'Investigación Social Cuantitativa', 4, 23, 4, 2),
(970, 'Entrenamiento Físico General', 2, 23, 4, 2),
(971, 'Planificación de Políticas y Servicios Públicos', 2, 23, 4, 2),
(972, 'Teoría Social Clásica II', 3, 23, 4, 2),
(973, 'Población y Desarrollo Humano ', 4, 23, 4, 2),
(974, 'Participación Ciudadana y Desarrollo Local', 4, 23, 5, 3),
(975, 'Investigación Social Cualitativa', 4, 23, 5, 3),
(976, 'Teoría Organizacional y Relaciones Humanas', 2, 23, 5, 3),
(977, 'Desarollo Regional', 3, 23, 5, 3),
(978, 'Teoría Social Contemporánea', 4, 23, 5, 3),
(979, 'Electiva II', 2, 23, 5, 3),
(980, 'Práctica Profesional I: Gestión y Administración de los Servicios Públicos', 3, 23, 6, 3),
(981, 'Nuevas Corrientes de la Teoría Social', 4, 23, 6, 3),
(982, 'Sociología de la Familia y la Salud', 3, 23, 6, 3),
(983, 'Ética Social', 2, 23, 6, 3),
(984, 'Planificación Institucional', 3, 23, 6, 3),
(985, 'Diseño y Evaluación de Proyectos', 4, 23, 6, 3),
(986, 'Práctica Profesional II: Seminario de Tesis I', 4, 23, 7, 4),
(987, 'Técnicas de Investigación Social', 4, 23, 7, 4),
(988, 'Epistemología', 3, 23, 7, 4),
(989, 'Psicología Social', 4, 23, 7, 4),
(990, 'Electiva III', 2, 23, 7, 4),
(991, 'Práctica Profesional III: Sociología de la Empresa', 4, 23, 8, 4),
(992, 'Seminario de Tesis II', 4, 23, 8, 4),
(993, 'Proyecto Nacional y Nueva Ciudadanía', 2, 24, 0, 1),
(994, 'Taller: Introducción a la Universidad y al Programa', 2, 24, 0, 1),
(995, 'Introducción a la Matemática', 2, 24, 0, 1),
(996, 'Taller: Capacidades Críticas, Analíticas y Dialógicas', 2, 24, 0, 1),
(997, 'Seminario: Comunicación e Investigación', 2, 24, 0, 1),
(998, 'Proyecto Socio Integrador I-A: Aproximación a las Políticas Turísticas y Agro Ecológicas en Venezuela con Énfasis en Eje Norte Llanero', 5, 24, 1, 1),
(999, 'Lenguaje y Comunicación', 4, 24, 1, 1),
(1000, 'Matemática General', 4, 24, 1, 1),
(1001, 'Vivencial I: Sociología del Turismo', 4, 24, 1, 1),
(1002, 'Seminario I: Investigación y Acción Participativa', 4, 24, 1, 1),
(1003, 'Formación Sociocrítica I: Fundamentos del Turismo Agroecológico', 4, 24, 1, 1),
(1004, 'Cultura Física General', 4, 24, 1, 1),
(1005, 'Proyecto Socio Integrador I-B: Aproximación a las Políticas Turísticas y Agro Ecológicas en Venezuela con Énfasis en el Eje Norte Llanero', 4, 24, 2, 1),
(1006, 'Estadística Aplicada al Turismo', 4, 24, 2, 1),
(1007, 'Taller I: Investigación Social', 4, 24, 2, 1),
(1008, 'Vivencial II: Geografía Turística de Venezuela I', 4, 24, 2, 1),
(1009, 'Idiomas Modernos I: Inglés y Francés', 4, 24, 2, 1),
(1010, 'Formación Sociocrítica II: Agroecoturismo y el Uso de Medios Tecnológicos', 4, 24, 2, 1),
(1011, 'Orientación y Ética Profesional', 4, 24, 2, 1),
(1012, 'Grupo de Electivas I: Manifestaciones Culturales y Turísticas', 2, 24, 2, 1),
(1013, 'Proyecto Socio Integrador II-A: Sistemas Turísticos Agroecológicos Integrales', 5, 24, 3, 2),
(1014, 'Psicología del Turismo', 4, 24, 3, 2),
(1015, 'Seminario II: Investigación Agroecológica I', 4, 24, 3, 2),
(1016, 'Vivencial III: Geografía Turística de Venezuela II', 4, 24, 3, 2),
(1017, 'Taller II: Animación Sociocultural', 4, 24, 3, 2),
(1018, 'Formación Sociocrítica III: Fundamentos Legales y Problemática Socio Ambiental de Venezuela', 4, 24, 3, 2),
(1019, 'Fundamentos de Fauna y Flora', 4, 24, 3, 2),
(1020, 'Proyecto Socio Integrador II-B: Sistemas Turísticos Agroecológicos Integrales', 4, 24, 4, 2),
(1021, 'Planificación Turística', 4, 24, 4, 2),
(1022, 'Administración y Contabilidad Turística', 4, 24, 4, 2),
(1023, 'Seminario III: Investigación Agroecológica II', 4, 24, 4, 2),
(1024, 'Vivencial IV: Senderos Turísticos', 4, 24, 4, 2),
(1025, 'Idiomas Modernos II: Inglés y Francés', 4, 24, 4, 2),
(1026, 'Formación Sociocrítica IV: Fundamentos Antropológicos y Socioculturales de la Agroecología', 4, 24, 4, 2),
(1027, 'Grupo de Electivas II: Turismo Comunitario y Participación Sociocultural', 2, 24, 4, 2),
(1028, 'Turismo Rural', 4, 24, 5, 3),
(1029, 'Seminario IV: Investigación Agroecológica III', 4, 24, 5, 3),
(1030, 'Idiomas Modernos III: Inglés y Francés', 4, 24, 5, 3),
(1031, 'Formación Sociocrítica V:  Áreas Naturales, Conservación e Interpretación Ambiental', 4, 24, 5, 3),
(1032, 'Práctica I: Exploración Agroecológica', 4, 24, 5, 3),
(1033, 'Grupo de Electivas III: Relaciones Interpersonales e Interinstitucionales ', 2, 24, 5, 3),
(1034, 'Proyecto Socio Integrador III-A: Diseño y Construcción de Escenarios Contextos y Ambientes turísticos Agroecológicos', 5, 24, 5, 3),
(1035, 'Taller III: Investigación Diseño de Planes, Estratégicos, Programas y Proyectos de Turismo Agroecológico', 4, 24, 6, 3),
(1036, 'Mercadotecnia de Turismo', 4, 24, 6, 3),
(1037, 'Orientación para el Emprendimiento, la Sustentabilidad y el Desarollo SocioProductivo', 4, 24, 6, 3),
(1038, 'Grupo de Electivas IV: Hotelería y Servicios', 2, 24, 6, 3),
(1039, 'Formación Socio-Crítica VI: Economía y Turismo Agroecológico', 4, 24, 6, 3),
(1040, 'Práctica II: Planificación del Turismo en Ámbitos Educativos, Comunitarios, Institucionales Públicos y Privados en Espacios Urbanos y Extraurbanas', 4, 24, 6, 3),
(1041, 'Proyecto Socio Integrador III-B: Diseño y Construcción de Escenarios Contextos y Ambientes Turísticos Agroecológicos', 4, 24, 6, 3),
(1042, 'Servicio Comunitario', 0, 24, 7, 4),
(1043, 'Investigación de Mercado', 4, 24, 7, 4),
(1044, 'Gestión de Empresas Turísticas Agroecológicas', 4, 24, 7, 4),
(1045, 'Práctica III: Implementación de la Propuesta de Intervención en Turismo Agroecológico', 4, 24, 7, 4),
(1046, 'Seminario V: Sistematización en Investigación Turística Agroecológica ', 4, 24, 7, 4);
INSERT INTO `sub_proyectos` (`id`, `nombre_sub_proyecto`, `unidades_credito`, `subprograma_id`, `semestre`, `anio_academico`) VALUES
(1047, 'Proyecto Socio Integrador IV-A: Diseño, Aplicación y Evaluación de Propuestas de Proyectos Turísticos', 5, 24, 7, 4),
(1048, 'Taller IV: Investigación, Ejecución y Evaluación de Planes, Orogramas o Proyectos Turísticos', 4, 24, 8, 4),
(1049, 'Práctica IV: Intervención en Ámbitos de Inserción socioproductiva', 4, 24, 8, 4),
(1050, 'Proyecto Final: Sistematización de la Práctica de Investigación - Intervención en Turismo Agroecológico ', 4, 24, 8, 4),
(1051, 'Proyecto Socio Integrador IV-B: Diseño, Aplicación y Evaluación de Propuestas de Proyectos turísticos', 4, 24, 8, 4),
(1052, 'Contabilidad I', 3, 22, 1, 0),
(1053, 'Espacio Geográfico e Identidad Nacional', 2, 22, 1, 0),
(1054, 'Lenguaje y Comunicación', 4, 22, 1, 0),
(1055, 'Educación Física, Deporte y Recreación', 2, 22, 1, 0),
(1056, 'Matemática', 4, 22, 1, 0),
(1057, 'Introducción al Derecho', 3, 22, 1, 0),
(1058, 'Contabilidad II', 4, 22, 2, 0),
(1059, 'Fundamentos Económicos', 4, 22, 2, 0),
(1060, 'Inglés Instrumental', 2, 22, 2, 0),
(1061, 'Estadística', 3, 22, 2, 0),
(1062, 'Introducción a la Administración', 3, 22, 2, 0),
(1063, 'Legislación Mercantil', 3, 22, 2, 0),
(1064, 'Contabilidad III', 4, 22, 3, 0),
(1065, 'Matemática Financiera', 3, 22, 3, 0),
(1066, 'Informática', 3, 22, 3, 0),
(1067, 'Metodología de la Investigación', 3, 22, 3, 0),
(1068, 'Macroeconomía', 3, 22, 3, 0),
(1069, 'Electiva I', 2, 22, 3, 0),
(1070, 'Contabilidad IV', 4, 22, 4, 0),
(1071, 'Contabilidad de Costos I', 4, 22, 4, 0),
(1072, 'Ecología y Educación Ambiental', 2, 22, 4, 0),
(1073, 'Investigación Social', 2, 22, 4, 0),
(1074, 'Participación Ciudadana y Desarrollo Local', 2, 22, 4, 0),
(1075, 'Electiva II', 2, 22, 4, 0),
(1076, 'Contabilidad de Costos II', 4, 22, 5, 0),
(1077, 'Tributación I', 2, 22, 5, 0),
(1078, 'Análisis de Estados Financieros', 3, 22, 5, 0),
(1079, 'Administración Pública', 3, 22, 5, 0),
(1080, 'Derecho Laboral y Seguridad Social', 3, 22, 5, 0),
(1081, 'Desarrollo Gerencial', 2, 22, 5, 0),
(1082, 'Electiva III', 2, 22, 5, 0),
(1083, 'Contabilidad y Presupuesto del Sector Público', 4, 22, 6, 0),
(1084, 'Contabilidades Especiales', 3, 22, 6, 0),
(1085, 'Auditoría Básica', 3, 22, 6, 0),
(1086, 'Tributación II', 3, 22, 6, 0),
(1087, 'Administración Financiera', 3, 22, 6, 0),
(1088, 'Sistemas Administrativos y Contables', 3, 22, 6, 0),
(1089, 'Contabilidad Agropecuaria', 3, 22, 7, 0),
(1090, 'Auditoría Superior', 3, 22, 7, 0),
(1091, 'Presupuesto Empresarial', 3, 22, 7, 0),
(1092, 'Software de Aplicación', 2, 22, 7, 0),
(1093, 'Investigación de Operaciones', 3, 22, 7, 0),
(1094, 'Ética Profesional', 1, 22, 7, 0),
(1095, 'Práctica Profesional I (Seminario Trabajo de Aplicación)', 2, 22, 7, 0),
(1096, 'Práctica Profesional II (Pasantía)', 4, 22, 8, 0),
(1097, 'Trabajo de Aplicación', 8, 22, 8, 0),
(1098, 'Introducción al Derecho', 3, 19, 0, 1),
(1099, 'Derecho Constitucional', 3, 19, 0, 1),
(1100, 'Lógica Jurídica', 3, 19, 0, 1),
(1101, 'Sociología Jurídica', 3, 19, 0, 1),
(1102, 'Derecho Romano I', 3, 19, 0, 1),
(1103, 'Derecho Civil I (Personas y Bienes)', 3, 19, 0, 1),
(1104, 'Derecho Registral y Notarial', 3, 19, 0, 1),
(1105, 'Derecho Administrativo I', 3, 19, 0, 1),
(1106, 'Eje Transversal Formación Ciudadana I', 0, 19, 0, 1),
(1107, 'Eje Transversal Desarrollo Personal I', 0, 19, 0, 1),
(1108, 'Eje Transversal Proyección Institucional I', 0, 19, 0, 1),
(1109, 'Derechos Humanos', 3, 19, 0, 2),
(1110, 'Derecho Romano II', 3, 19, 0, 2),
(1111, 'Derecho Civil II (Obligaciones)', 3, 19, 0, 2),
(1112, 'Derecho Administrativo II', 3, 19, 0, 2),
(1113, 'Derecho Internacional Público', 3, 19, 0, 2),
(1114, 'Derecho Penal I', 3, 19, 0, 2),
(1115, 'Criminología y Criminalística', 3, 19, 0, 2),
(1116, 'Derecho Procesal Civil I', 4, 19, 0, 2),
(1117, 'Eje Transversal Formación Ciudadana II', 0, 19, 0, 2),
(1118, 'Eje Transversal Desarrollo Personal II', 0, 19, 0, 2),
(1119, 'Eje Transversal Proyección Institucional II', 0, 19, 0, 2),
(1120, 'Derecho Tributario', 4, 19, 0, 3),
(1121, 'Derecho Civil III (Contratos)', 3, 19, 0, 3),
(1122, 'Derecho Laboral', 3, 19, 0, 3),
(1123, 'Derecho Contencioso Administrativo', 4, 19, 0, 3),
(1124, 'Derecho Penal II', 3, 19, 0, 3),
(1125, 'Medicina Legal y Práctica Forense', 2, 19, 0, 3),
(1126, 'Derecho Procesal Civil II', 4, 19, 0, 3),
(1127, 'Derecho Probatorio', 3, 19, 0, 3),
(1128, 'Práctica I', 2, 19, 0, 3),
(1129, 'Eje Transversal Formación Ciudadana III', 0, 19, 0, 3),
(1130, 'Eje Transversal Desarrollo Personal III', 0, 19, 0, 3),
(1131, 'Eje Transversal Proyección Institucional III', 0, 19, 0, 3),
(1132, 'Derecho Internacional Privado', 3, 19, 0, 4),
(1133, 'Derechos Especiales(Agrario y Ambiental, Seguridad Social y Protección del Niño y del Adolescente)', 5, 19, 0, 4),
(1134, 'Derecho Civil IV (Familia y Sucesiones)', 3, 19, 0, 4),
(1135, 'Derecho Procesal Laboral', 4, 19, 0, 4),
(1136, 'Derecho Mercantil', 3, 19, 0, 4),
(1137, 'Derecho Procesal Penal', 4, 19, 0, 4),
(1138, 'Derecho Procesal Civil III', 4, 19, 0, 4),
(1139, 'Práctica II', 2, 19, 0, 4),
(1140, 'Eje Transversal Formación Ciudadana IV', 0, 19, 0, 4),
(1141, 'Eje Transversal Desarrollo Personal IV', 0, 19, 0, 4),
(1142, 'Eje Transversal Proyección Institucional IV', 0, 19, 0, 4),
(1143, 'Informática', 2, 25, 1, 1),
(1144, 'Entrenamiento Físico Genera', 2, 25, 1, 1),
(1145, 'Lenguaje y Comunicación', 4, 25, 1, 1),
(1146, 'Inglés Instrumental', 2, 25, 1, 1),
(1147, 'Matemática General', 4, 25, 1, 1),
(1148, 'Espacio Geográfico e Identidad Nacional', 2, 25, 1, 1),
(1149, 'Filosofía de la Educación', 3, 25, 2, 1),
(1150, 'Psicología Evolutiva', 3, 25, 2, 1),
(1151, 'Biología General', 4, 25, 2, 1),
(1152, 'Química General', 3, 25, 2, 1),
(1153, 'Física', 4, 25, 2, 1),
(1154, 'Estadística Descriptiva', 3, 25, 2, 1),
(1155, 'Psicología del Aprendizaje', 3, 25, 3, 2),
(1156, 'Educación y Desarrollo', 3, 25, 3, 2),
(1157, 'Química Orgánica', 3, 25, 3, 2),
(1158, 'Fisicoquímica', 4, 25, 3, 2),
(1159, 'Ecología', 2, 25, 3, 2),
(1160, 'Educación Ambiental', 2, 25, 3, 2),
(1161, 'Química Analítica', 3, 25, 3, 2),
(1162, 'Orientación Educativa', 3, 25, 4, 2),
(1163, 'Bioquímica', 4, 25, 4, 2),
(1164, 'Genética', 4, 25, 4, 2),
(1165, 'Gestión Ambiental', 4, 25, 4, 2),
(1166, 'Planificación de la Enseñanza', 4, 25, 4, 2),
(1167, 'Electiva I : Derechos Humanos', 2, 25, 5, 3),
(1168, 'Electiva I : Métodos y Técnicas de Estudio', 2, 25, 5, 3),
(1169, 'Medios de Comunicación', 2, 25, 5, 3),
(1170, 'Electiva I : Desarrollo Personal y Profesional', 2, 25, 5, 3),
(1171, 'Biología Celular', 4, 25, 5, 3),
(1172, 'Microbiología', 4, 25, 5, 3),
(1173, 'Electiva I : Comunicación Oral y Escrita', 2, 25, 5, 3),
(1174, 'Electiva I : Comunicación, Valores y Relaciones Interpersonales', 2, 25, 5, 3),
(1175, 'Sociología de la Educación', 3, 25, 5, 3),
(1176, 'Electiva I : Pensamiento Bolivariano', 2, 25, 5, 3),
(1177, 'Metodología de la Investigación', 3, 25, 5, 3),
(1178, 'Gerencia Educativa', 3, 25, 5, 3),
(1179, 'Evaluación del Aprendizaje', 3, 25, 5, 3),
(1180, 'Ética y Docencia', 4, 25, 6, 3),
(1181, 'Biología Vegetal', 2, 25, 6, 3),
(1182, 'Electiva II: Tecnologías de Información y Comunicación Educativa', 2, 25, 6, 3),
(1183, 'Multimedia como Herramienta Instruccional', 2, 25, 6, 3),
(1184, 'Elaboración y Utilización de Recursos Audiovisuales', 2, 25, 6, 3),
(1185, 'ELECTIVA II: NEUROLINGUISTICA', 2, 25, 6, 3),
(1186, 'ELECTIVA II: EDUCACION, POLITICA Y GERENCIA PUBLICA', 2, 25, 6, 3),
(1187, 'Extensión Ambiental Comunitario', 2, 25, 6, 3),
(1188, 'Principios de Gerencia Estratégica', 2, 25, 6, 3),
(1189, 'Electiva II: Comportamiento Organización', 2, 25, 6, 3),
(1190, 'Electiva II: Teoria y Planificación Curricular', 2, 25, 6, 3),
(1191, 'Investigación Social', 0, NULL, 6, 3),
(1192, 'PPI: Administración Supervisor de la Educación', 3, 25, 6, 3),
(1193, 'Metodología y Recursos para el Aprendizaje', 4, 25, 6, 3),
(1194, 'Electiva II: Diseño y Evaluación Curricular', 2, 25, 6, 3),
(1195, 'Biología Animal', 4, 25, 7, 4),
(1196, 'Tópicos de Biotecnología', 4, 25, 7, 4),
(1197, 'Electiva III: Biodiversidad', 2, 25, 7, 4),
(1198, 'Electiva III: Manejo Sustentable de Recursos Naturales Endogenos', 2, 25, 7, 4),
(1199, 'Electiva III: Gestión de Residuos Solidos Urbanos', 2, 25, 7, 4),
(1200, 'Seminario de Grado', 3, 25, 7, 4),
(1201, 'PPII: El Docente como agente de cambio social', 3, 25, 7, 4),
(1202, 'Servicio Social Comunitario', 0, 25, 7, 4),
(1203, 'Trabajo de Grado', 4, 25, 8, 4),
(1204, 'PPIII: DOCENCIA INTEGRADA', 7, 25, 8, 4),
(1205, 'Lenguaje y Comunicación', 4, 26, 1, 1),
(1206, 'Inglés Instrumental', 2, 26, 1, 1),
(1207, 'Entrenamiento Físico General', 2, 26, 1, 1),
(1208, 'Espacio Geográfico e Identidad Nacional', 2, 26, 1, 1),
(1209, 'Matemática General', 4, 26, 1, 1),
(1210, 'Informática', 2, 26, 1, 1),
(1211, 'Psicología Evolutiva', 3, 26, 2, 1),
(1212, 'Sociología de la Educación', 3, 26, 2, 1),
(1213, 'Filosofía de la Educación', 3, 26, 2, 1),
(1214, 'Introducción al Estudio de la Lengua y Literatura', 3, 26, 2, 1),
(1215, 'Lectura y Escritura', 4, 26, 2, 1),
(1216, 'Historia del Arte', 3, 26, 2, 1),
(1217, 'Psicología del Aprendizaje', 3, 26, 3, 2),
(1218, 'Ecología y Educación Ambiental', 3, 26, 3, 2),
(1219, 'Gramática I', 3, 26, 3, 2),
(1220, 'Literatura Infantil y Juvenil', 3, 26, 3, 2),
(1221, 'Lingüística General', 3, 26, 3, 2),
(1222, 'Electiva I', 2, 26, 3, 2),
(1223, 'Química Analítica', 3, 25, 3, 2),
(1224, 'Orientación Educativa', 3, 26, 4, 2),
(1225, 'Estadística Descriptiva', 3, 26, 4, 2),
(1226, 'Planificación de la Enseñanza', 4, 26, 4, 2),
(1227, 'Gramática II', 3, 26, 4, 2),
(1228, 'Teoría y Análisis Literario', 3, 26, 4, 2),
(1229, 'Literatura Universal', 3, 26, 4, 2),
(1230, 'Gerencia Educativa', 3, 26, 5, 3),
(1231, 'Evaluación del Aprendizaje', 3, 26, 5, 3),
(1232, 'Metodología de la Investigación', 3, 26, 5, 3),
(1233, 'Literatura Hispanoamericana I', 3, 26, 5, 3),
(1234, 'Fonética y Fonología', 3, 26, 5, 3),
(1235, 'Electiva II', 2, 26, 5, 3),
(1236, 'Práctica Profesional I: El Docente como Agente de Cambio Social', 3, 26, 6, 3),
(1237, 'Metod. y Rec. para Aprendizaje', 4, 26, 6, 3),
(1238, 'Investigación Social', 4, 26, 6, 3),
(1239, 'Ética y Docencia', 2, 26, 6, 3),
(1240, 'Morfosintaxis', 3, 26, 6, 3),
(1241, 'Literatura Venezolana I', 3, 26, 6, 3),
(1242, 'Práctica Profesional II: Administración y Supervisión de la Educación', 3, 26, 7, 4),
(1243, 'Literatura Venezolana II', 3, 26, 7, 4),
(1244, 'Estrategias Metodológicas para la Enseñanza Lengua y Literatura', 3, 26, 7, 4),
(1245, 'Literatura Hispanoamericana II', 3, 26, 7, 4),
(1246, 'Electiva III', 2, 26, 7, 4),
(1247, 'Práctica Profesional III. Docencia Integrada', 7, 26, 8, 4),
(1248, 'Trabajo de Grado', 4, 26, 8, 4),
(1249, 'Lenguaje y Comunicación', 4, 27, 1, 1),
(1250, 'Inglés Instrumental', 2, 27, 1, 1),
(1251, 'Entrenamiento Físico General', 2, 27, 1, 1),
(1252, 'Espacio Geográfico e Identidad Nacional', 2, 27, 1, 1),
(1253, 'Informática', 2, 27, 1, 1),
(1254, 'Matemática General', 4, 27, 1, 1),
(1255, 'Psicología Evolutiva', 3, 27, 2, 1),
(1256, 'Filosofía de la Educación', 3, 27, 2, 1),
(1257, 'Sociología de la Educación', 3, 27, 2, 1),
(1258, 'Lectura y Escritura', 4, 27, 2, 1),
(1259, 'Geometría', 4, 27, 2, 1),
(1260, 'Educación Física y Recreación', 3, 27, 2, 1),
(1261, 'Psicología del Aprendizaje', 3, 27, 3, 2),
(1262, 'Ecología y Educación Ambiental', 3, 27, 3, 2),
(1263, 'Gramática Castellana', 4, 27, 3, 2),
(1264, 'Formación Comunal y Ciudadana', 3, 27, 3, 2),
(1265, 'Educación para el Trabajo', 4, 27, 3, 2),
(1266, 'Electiva I', 2, 27, 3, 2),
(1267, 'Orientación Educativa', 3, 27, 4, 2),
(1268, 'Estadística Descriptiva', 3, 27, 4, 2),
(1269, 'Planificación de la Enseñanza', 4, 27, 4, 2),
(1270, 'Educación Musical y Artes Escénicas', 3, 27, 4, 2),
(1271, 'Ciencias Naturales I', 4, 27, 4, 2),
(1272, 'Literatura Infantil y Juvenil', 3, 27, 4, 2),
(1273, 'Gerencia Educativa', 3, 27, 5, 3),
(1274, 'Evaluación del Aprendizaje', 3, 27, 5, 3),
(1275, 'Metodología de la Investigación', 3, 27, 5, 3),
(1276, 'Geografía de Venezuela', 4, 27, 5, 3),
(1277, 'Ciencias Naturales II', 4, 27, 5, 3),
(1278, 'Electiva II', 2, 27, 5, 3),
(1279, 'Práctica Profesional I: Administración y Supervisión de la Educación', 3, 27, 6, 3),
(1280, 'Metodología y Recursos para el Aprendizaje', 4, 27, 6, 3),
(1281, 'Investigación Social', 4, 27, 6, 3),
(1282, 'Educación para la Salud', 2, 27, 6, 3),
(1283, 'Artes Plásticas', 3, 27, 6, 3),
(1284, 'Ética y Docencia', 3, 27, 6, 3),
(1285, 'Práctica Profesional II: El Docente como Agente de Cambio Socia', 3, 27, 6, 4),
(1286, 'Didáctica de la Matemática', 3, 27, 6, 4),
(1287, 'Didáctica y Administración de la Escuela Multigrado', 2, 27, 6, 4),
(1288, 'Historia de Venezuela', 4, 27, 6, 4),
(1289, 'Electiva III', 2, 27, 6, 4),
(1290, 'Práctica Profesional III. Docencia Integrada', 7, 27, 8, 4),
(1291, 'Trabajo de Grado', 4, 27, 8, 4),
(1292, 'Lenguaje y Comunicación ', 4, 28, 1, 1),
(1293, 'Inglés Instrumental ', 2, 28, 1, 1),
(1294, 'Entrenamiento Físico General', 2, 28, 1, 1),
(1295, 'Espacio Geográfico e Identidad Nacional', 2, 28, 1, 1),
(1296, 'Informática', 2, 28, 1, 1),
(1297, 'Matemática General', 4, 28, 1, 1),
(1298, 'Psicología Evolutiva', 3, 28, 2, 1),
(1299, 'Filosofía de la Educación', 3, 28, 2, 1),
(1300, 'Sociología de la Educación', 3, 28, 2, 1),
(1301, ' Química General ', 4, 28, 2, 1),
(1302, ' Introducción a la Física ', 3, 28, 2, 1),
(1303, ' Introducción al Cálculo ', 4, 28, 2, 1),
(1304, 'Psicología del Aprendizaje', 3, 28, 3, 2),
(1305, 'Ecología y Educación Ambiental', 3, 28, 3, 2),
(1306, ' Cálculo Diferencial ', 4, 28, 3, 2),
(1307, ' Física I ', 5, 28, 3, 2),
(1308, ' Estadística Descriptiva ', 3, 28, 3, 2),
(1309, ' Electiva I ', 2, 27, 3, 2),
(1310, 'Orientación Educativa', 3, 28, 4, 2),
(1311, ' Gerencia Educativa ', 3, 28, 4, 2),
(1312, ' Planificación de la Enseñanza ', 4, 28, 4, 2),
(1313, ' Cálculo Integral ', 4, 28, 4, 2),
(1314, ' Física II ', 5, 28, 4, 2),
(1315, ' Laboratorio I ', 2, 28, 4, 2),
(1316, ' Ecuaciones Diferenciales ', 3, 28, 5, 3),
(1317, ' Evaluación del Aprendizaje ', 3, 28, 5, 3),
(1318, 'Metodología de la Investigación', 3, 28, 5, 3),
(1319, ' Física III ', 5, 28, 5, 3),
(1320, ' Laboratorio II ', 2, 28, 5, 3),
(1321, ' Electiva II ', 2, 28, 5, 3),
(1322, ' Práctica Profesional I: Administración y Supervisión de la Educación ', 3, 28, 6, 3),
(1323, 'Metodología y Recursos para el Aprendizaje', 4, 28, 6, 3),
(1324, 'Investigación Social', 4, 28, 6, 3),
(1325, ' Ética y Docencia ', 3, 28, 6, 3),
(1326, ' Física IV ', 4, 28, 6, 3),
(1327, ' Laboratorio III ', 3, 28, 6, 3),
(1328, 'Práctica Profesional II: El Docente como Agente de Cambio Social', 3, 28, 6, 4),
(1329, ' Didáctica de la Física ', 3, 28, 6, 4),
(1330, ' Física V ', 4, 28, 6, 4),
(1331, ' Laboratorio III ', 2, 28, 6, 4),
(1332, 'Electiva III', 2, 28, 6, 4),
(1333, ' Práctica Profesional III. Docencia Integrada ', 7, 28, 8, 4),
(1334, 'Trabajo de Grado', 4, 28, 8, 4),
(1335, 'Lenguaje y Comunicación', 4, 29, 1, 1),
(1336, 'Inglés Instrumental', 2, 29, 1, 1),
(1337, 'Entrenamiento Físico General', 2, 29, 1, 1),
(1338, 'Espacio Geográfico e Identidad Nacional', 2, 29, 1, 1),
(1339, 'Informática', 2, 29, 1, 1),
(1340, 'Matemática General', 4, 29, 1, 1),
(1341, 'Psicología Evolutiva', 3, 29, 2, 1),
(1342, 'Filosofía de la Educación', 3, 29, 2, 1),
(1343, 'Sociología de la Educación', 3, 29, 2, 1),
(1344, 'Anatomía y Fisiología Humana', 3, 29, 2, 1),
(1345, 'Atletismo', 3, 29, 2, 1),
(1346, 'Natación', 3, 29, 2, 1),
(1347, 'Psicología del Aprendizaje', 3, 29, 3, 2),
(1348, 'Ecología y Educación Ambiental', 3, 29, 3, 2),
(1349, 'Fisiología del Ejercicio', 3, 29, 3, 2),
(1350, 'Gimnasia (Femenina)', 3, 29, 3, 2),
(1351, 'Gimnasia (Masculina)', 3, 29, 3, 2),
(1352, 'Recreación y Primeros Auxilios', 3, 29, 3, 2),
(1353, 'Electiva I', 2, 27, 3, 2),
(1354, 'Orientación Educativa', 3, 29, 4, 2),
(1355, 'Estadística Descriptiva', 3, 29, 4, 2),
(1356, 'Planificación de la Enseñanza', 4, 29, 4, 2),
(1357, 'Voleibol', 3, 29, 4, 2),
(1358, 'Análisis Integral del Movimiento', 3, 29, 4, 2),
(1359, 'Administración Deportiva', 3, 29, 4, 2),
(1360, 'Gerencia Educativa', 3, 29, 5, 3),
(1361, 'Evaluación del Aprendizaje', 3, 29, 5, 3),
(1362, 'Metodología de la Investigación', 3, 29, 5, 3),
(1363, 'Principios Científicos del Entrenamiento', 3, 29, 5, 3),
(1364, 'Baloncesto', 3, 29, 5, 3),
(1365, 'Electiva II', 2, 29, 5, 3),
(1366, 'Práctica Profesional I: Administración y Supervisión de la Educación Especial', 3, 29, 6, 3),
(1367, 'Metodología y Recursos para el Aprendizaje', 4, 29, 6, 3),
(1368, 'Investigación Social', 4, 29, 6, 3),
(1369, 'Ética y Docencia', 3, 29, 6, 3),
(1370, 'Medición y Evaluación del Rendimiento Deportivo', 3, 29, 6, 3),
(1371, 'Béisbol(M) y Softbol(F)', 3, 29, 6, 3),
(1372, 'Práctica Profesional II: El Docente como Agente de Cambio Socia', 3, 29, 6, 4),
(1373, 'Didáctica de la Educación Física', 3, 29, 6, 4),
(1374, 'Fútbol(M) y Fútbol Sala(F)', 3, 29, 6, 4),
(1375, 'Educación Física Especial', 3, 29, 6, 4),
(1376, 'Electiva III', 2, 27, 6, 4),
(1377, 'Práctica Profesional III. Docencia Integrada', 7, 29, 8, 4),
(1378, 'Trabajo de Grado', 4, 29, 8, 4),
(1379, 'Lenguaje y Comunicación', 4, 30, 1, 1),
(1380, 'Inglés Instrumental', 2, 30, 1, 1),
(1381, 'Entrenamiento Físico General', 2, 30, 1, 1),
(1382, 'Espacio Geográfico e Identidad Nacional', 2, 30, 1, 1),
(1383, 'Informática', 2, 30, 1, 1),
(1384, 'Matemática General', 4, 30, 1, 1),
(1385, 'Psicología Evolutiva', 3, 30, 2, 1),
(1386, 'Filosofía de la Educación', 3, 30, 2, 1),
(1387, 'Sociología de la Educación', 3, 30, 2, 1),
(1388, 'Lógica', 2, 30, 2, 1),
(1389, 'Introducción al Cálculo', 4, 30, 2, 1),
(1390, 'Fundamentos de Trigonometría', 3, 30, 2, 1),
(1391, 'Psicología del Aprendizaje', 3, 30, 3, 2),
(1392, 'Ecología y Educación Ambiental', 3, 30, 3, 2),
(1393, 'Geometría', 4, 30, 3, 2),
(1394, 'Cálculo Diferencial', 4, 30, 3, 2),
(1395, 'Álgebra I', 4, 30, 3, 2),
(1396, 'Electiva I', 2, 30, 3, 2),
(1397, 'Orientación Educativa', 3, 30, 4, 2),
(1398, 'Estadística Descriptiva', 3, 30, 4, 2),
(1399, 'Planificación de la Enseñanza', 4, 30, 4, 2),
(1400, 'Cálculo Integral', 4, 30, 4, 2),
(1401, 'Álgebra II', 3, 30, 4, 2),
(1402, 'Física I', 4, 30, 4, 2),
(1403, 'Gerencia Educativa', 3, 30, 5, 3),
(1404, 'Evaluación del Aprendizaje', 3, 30, 5, 3),
(1405, 'Metodología de la Investigación', 3, 30, 5, 3),
(1406, 'Cálculo de Variables Múltiples', 4, 30, 5, 3),
(1407, 'Física II', 4, 30, 5, 3),
(1408, 'Electiva II', 2, 30, 5, 3),
(1409, 'Práctica Profesional I: Administración y Supervisión de la Educación', 3, 30, 6, 3),
(1410, 'Metodología y Recursos para el Aprendizaje', 4, 30, 6, 3),
(1411, 'Investigación Social', 4, 30, 6, 3),
(1412, 'Ética y Docencia', 3, 30, 6, 3),
(1413, 'Medición y Evaluación del Rendimiento Deportivo', 3, 30, 6, 3),
(1414, 'Béisbol(M) y Softbol(F)', 3, 30, 6, 3),
(1415, 'Práctica Profesional II: El Docente como Agente de Cambio Social', 3, 30, 7, 4),
(1416, 'Didáctica de la Educación Física', 3, 30, 7, 4),
(1417, 'Fútbol(M) y Fútbol Sala(F)', 3, 30, 7, 4),
(1418, 'Educación Física Especial', 3, 30, 7, 4),
(1419, 'Electiva III', 2, 30, 7, 4),
(1420, 'Práctica Profesional III. Docencia Integrada', 7, 30, 8, 4),
(1421, 'Trabajo de Grado', 4, 30, 8, 4),
(1422, 'Lenguaje y Comunicación', 4, 31, 1, 1),
(1423, 'Inglés Instrumental', 2, 31, 1, 1),
(1424, 'Entrenamiento Físico General', 2, 31, 1, 1),
(1425, 'Espacio Geográfico e Identidad Nacional', 2, 31, 1, 1),
(1426, 'Informática', 2, 31, 1, 1),
(1427, 'Matemática General', 4, 31, 1, 1),
(1428, 'Psicología Evolutiva', 3, 31, 2, 1),
(1429, 'Principios de Química I', 4, 31, 2, 1),
(1430, 'Filosofía de la Educación', 3, 31, 2, 1),
(1431, 'Sociología de la Educación', 3, 31, 2, 1),
(1432, 'Oratoria', 1, 31, 2, 1),
(1433, 'Matemática Aplicada a la Química', 3, 31, 2, 1),
(1434, 'Psicología del Aprendizaje', 3, 31, 3, 2),
(1435, 'Ecología y Educación Ambiental', 3, 31, 3, 2),
(1436, 'Fundamentos de Física', 3, 31, 3, 2),
(1437, 'Fundamentos de Biología', 3, 31, 3, 2),
(1438, 'Principios de Química II', 4, 31, 3, 2),
(1439, 'Electiva I', 2, 31, 3, 2),
(1440, 'Orientación Educativa', 3, 31, 4, 2),
(1441, 'Planificación de la Enseñanza', 4, 31, 4, 2),
(1442, 'Metodología de la Investigación', 3, 31, 4, 2),
(1443, 'Química Analítica', 4, 31, 4, 2),
(1444, 'Fisicoquímica I', 3, 31, 4, 2),
(1445, 'Química Orgánica', 3, 31, 4, 2),
(1446, 'Fisicoquímica II', 3, 31, 5, 3),
(1447, 'Química Orgánica II', 3, 31, 5, 3),
(1448, 'Gerencia Educativa', 3, 31, 5, 3),
(1449, 'Estadística Descriptiva', 3, 31, 5, 3),
(1450, 'Evaluación del Aprendizaje', 3, 31, 5, 3),
(1451, 'Electiva II', 2, 31, 5, 3),
(1452, 'Formulación y Resolución de Problemas en Química', 3, 31, 6, 3),
(1453, 'Bioquímica', 4, 31, 6, 3),
(1454, 'Práctica Profesional I: Administración y Supervisión de la Educación Especial', 3, 31, 6, 3),
(1455, 'Análisis Instrumental', 3, 31, 6, 3),
(1456, 'Metodología y Recursos para el Aprendizaje', 4, 31, 6, 3),
(1457, 'Investigación Social', 4, 31, 6, 3),
(1458, 'Práctica Profesional II: El Docente como Agente de Cambio Social', 3, 31, 7, 4),
(1459, 'Seminario de Trabajo de Grado', 3, 31, 7, 4),
(1460, 'Ética y Docencia', 3, 31, 7, 4),
(1461, 'Química Inorgánica', 3, 31, 7, 4),
(1462, 'Electiva III', 2, 31, 7, 4),
(1463, 'Práctica Profesional III. Docencia Integrada', 7, 31, 8, 4),
(1464, 'Trabajo de Grado', 4, 31, 8, 4),
(1465, 'Lenguaje y Comunicación', 4, 32, 1, 1),
(1466, 'Inglés Instrumental', 2, 32, 1, 1),
(1467, 'Entrenamiento Físico General', 2, 32, 1, 1),
(1468, 'Espacio Geográfico e Identidad Nacional', 2, 32, 1, 1),
(1469, 'Informática', 2, 32, 1, 1),
(1470, 'Matemática General', 4, 32, 1, 1),
(1471, 'Psicología Evolutiva', 3, 32, 2, 1),
(1472, 'Filosofía de la Educación', 3, 32, 2, 1),
(1473, 'Sociología de la Educación', 3, 32, 2, 1),
(1474, 'Ecología y Educación Ambiental', 3, 32, 2, 1),
(1475, 'Geografía de Venezuela', 4, 32, 2, 1),
(1476, 'Historia de Venezuela I', 5, 32, 2, 1),
(1477, 'Psicología del Aprendizaje', 3, 32, 3, 2),
(1478, 'Pensamiento Sociopolítico de Simón Bolívar', 3, 32, 3, 2),
(1479, 'Historia de Venezuela II', 5, 32, 3, 2),
(1480, 'Cartografía e Interpretación de Mapas', 3, 32, 3, 2),
(1481, 'Geografía de Venezuela', 4, 32, 3, 2),
(1482, 'Electiva I', 2, 32, 3, 2),
(1483, 'Orientación Educativa', 3, 32, 4, 2),
(1484, 'Estadística Descriptiva', 3, 32, 4, 2),
(1485, 'Planificación de la Enseñanza', 4, 32, 4, 2),
(1486, 'Historia de las Civilizaciones I', 4, 32, 4, 2),
(1487, 'Geomorfología Aplicada a Venezuela', 4, 32, 4, 2),
(1488, 'Geohistoria Regional y Local', 4, 32, 4, 2),
(1489, 'Gerencia Educativa', 3, 32, 5, 3),
(1490, 'Evaluación del Aprendizaje', 3, 32, 5, 3),
(1491, 'Metodología de la Investigación', 3, 32, 5, 3),
(1492, 'Historia de las Civilizaciones II', 4, 32, 5, 3),
(1493, 'Geografía de la Población', 4, 32, 5, 3),
(1494, 'Electiva II', 2, 32, 5, 3),
(1495, 'Práctica Profesional I: Administración y Supervisión de la Educación', 3, 32, 6, 3),
(1496, 'Metodología y Recursos para el Aprendizaje', 4, 32, 6, 3),
(1497, 'Investigación Social', 4, 32, 6, 3),
(1498, 'Ética y Docencia', 3, 32, 6, 3),
(1499, 'Seminario de Investigación en Geografía e Historia', 4, 32, 6, 3),
(1500, 'Climatología y Meteorología', 4, 32, 6, 3),
(1501, 'Práctica Profesional II: El Docente como Agente de Cambio', 3, 32, 7, 4),
(1502, 'Historia de América', 4, 32, 7, 4),
(1503, 'Metodología para la Enseñanza de la Geografía e Historia', 3, 32, 7, 4),
(1504, 'Problemas Geohistóricos del Mundo Contemporáneo', 3, 32, 7, 4),
(1505, 'Electiva III', 2, 32, 7, 4),
(1506, 'Práctica Profesional III. Docencia Integrada', 7, 32, 8, 4),
(1507, 'Trabajo de Grado', 4, 32, 8, 4),
(1508, 'Introducción a las Matemáticas', 0, 4, 0, 1),
(1509, 'Orientación e Identidad Universitaria', 0, 4, 0, 1),
(1510, 'Tecnologías del Aprendizaje y Conocimiento', 0, 4, 0, 1),
(1511, 'Introducción al Dibujo Arquitectónico', 0, 4, 0, 1),
(1512, 'Eco-ética', 0, 4, 0, 1),
(1513, 'Nociones de Composición Arquitectónica', 0, 4, 0, 1),
(1514, 'Taller de Expresión Plástica', 0, 4, 0, 1),
(1515, 'Introducción al Análisis del Territorio', 0, 4, 0, 1),
(1516, 'Matemáticas I', 3, 4, 1, 1),
(1517, 'Lenguaje y Comunicación', 3, 4, 1, 1),
(1518, 'Dibujo Arquitectónico', 4, 4, 1, 1),
(1519, 'Actividad Física y Recreación', 3, 4, 1, 1),
(1520, 'Historia de la Arquitectura I', 3, 4, 1, 1),
(1521, 'Proyecto de Diseño 1.0 ( Espacios y Recorridos)', 6, 4, 1, 1),
(1522, 'Proyecto de Diseño 1.1 (Viviendas Unifamiliares)', 6, 4, 2, 1),
(1523, 'Matemáticas II', 3, 4, 2, 1),
(1524, 'Geometría Descriptiva I', 3, 4, 2, 1),
(1525, 'Física', 3, 4, 2, 1),
(1526, 'Historia de la Arquitectura II', 3, 4, 2, 1),
(1527, 'Metodologías del Diseño Arquitectónico', 4, 4, 2, 1),
(1528, 'Antropología', 4, 4, 3, 2),
(1529, 'Geometría Descriptiva II', 3, 4, 3, 2),
(1530, 'Mecánica Estática', 3, 4, 3, 2),
(1531, 'Historia de la Arquitectura III', 3, 4, 3, 2),
(1532, 'ELECTIVA I', 3, 4, 3, 2),
(1533, 'Proyecto de Diseño 2.0 (Espacios Habitables Colectivos', 6, 4, 3, 2),
(1534, 'Proyecto de Diseño 2.1 (Espacios Mixtos)', 6, 4, 4, 2),
(1535, 'Sociología', 3, 4, 4, 2),
(1536, 'Dibujo Asistido por Computador', 4, 4, 4, 2),
(1537, 'Resistencia de Materiales', 3, 4, 4, 2),
(1538, 'Topografía', 3, 4, 4, 2),
(1539, 'Historia de la Arquitectura IV', 3, 4, 4, 2),
(1540, 'Técnicas de Expresión Digital', 3, 4, 5, 3),
(1541, 'Prácticas Profesionales I', 3, 4, 5, 3),
(1542, 'Estructuras I', 3, 4, 5, 3),
(1543, 'Construcción I', 4, 4, 5, 3),
(1544, 'Seminario de Estudios Urbanos', 3, 4, 5, 3),
(1545, 'Proyecto de Diseño 3.0 (Espacios Educativos)', 6, 4, 5, 3),
(1546, 'Fotografia', 3, 4, 6, 3),
(1547, 'Estructuras II', 3, 4, 6, 3),
(1548, 'Construcción II', 3, 4, 6, 3),
(1549, 'Proyecto de Diseño 3.1 (Espacios Periurbanos)', 6, 4, 6, 3),
(1550, 'Taller de Estudios Urbanos', 4, 4, 6, 3),
(1551, 'ELECTIVA II', 3, 4, 6, 3),
(1552, 'Redacción de Articulos Cientificos', 3, 4, 7, 4),
(1553, 'Gerencia de la Construcción', 3, 4, 7, 4),
(1554, 'Deontología de la Profesión', 3, 4, 7, 4),
(1555, 'Construcción III', 4, 4, 7, 4),
(1556, 'Geotecnia', 3, 4, 7, 4),
(1557, 'Proyecto de Diseño 4.0 (Intervención de Asentamientos Formales/Informales)', 6, 4, 7, 4),
(1558, 'Proyecto de Diseño 4.1 (Intervención en Cascos Historicos)', 6, 4, 8, 4),
(1559, 'Prácticas Profesionales II', 3, 4, 8, 4),
(1560, 'Teoría y Crítica de la Arquitectura I', 3, 4, 8, 4),
(1561, 'Taller Experimental Tierra', 4, 4, 8, 4),
(1562, 'Ambiente, Paisaje y Paisajismo', 3, 4, 8, 4),
(1563, 'Lenguaje y Comunicación', 4, 34, 1, 1),
(1564, 'Inglés Instrumental', 2, 34, 1, 1),
(1565, 'Entrenamiento Físico General', 2, 34, 1, 1),
(1566, 'Espacio Geográfico e Identidad Nacional', 2, 34, 1, 1),
(1567, 'Informática', 2, 34, 1, 1),
(1568, 'Matemática General', 4, 34, 1, 1),
(1569, 'Psicología Evolutiva', 3, 34, 2, 1),
(1570, 'Filosofía de la Educación', 3, 34, 2, 1),
(1571, 'Sociología de la Educación', 3, 34, 2, 1),
(1572, 'Dibujo Analítico', 2, 34, 2, 1),
(1573, 'Historia del Arte I', 3, 34, 2, 1),
(1574, 'Introducción a las Bellas Artes ', 2, 34, 2, 1),
(1575, 'Psicología del Aprendizaje', 3, 34, 3, 2),
(1576, 'Ecología y Educación Ambiental', 3, 34, 3, 2),
(1577, 'Dibujo Descriptivo', 2, 34, 3, 2),
(1578, 'Historia del Arte II', 3, 34, 3, 2),
(1579, 'Arte y Proceso Educativo', 3, 34, 3, 2),
(1580, 'Electiva I', 2, 34, 3, 2),
(1581, 'Orientación Educativa', 3, 34, 4, 2),
(1582, 'Estadística Descriptiva', 3, 34, 4, 2),
(1583, 'Planificación de la Enseñanza', 4, 34, 4, 2),
(1584, 'Dibujo Expresivo', 2, 34, 4, 2),
(1585, 'Historia del Arte III', 3, 34, 4, 2),
(1586, 'Danza I ', 2, 34, 4, 2),
(1587, 'Gerencia Educativa', 3, 34, 5, 3),
(1588, 'Evaluación del Aprendizaje', 3, 34, 5, 3),
(1589, 'Metodología de la Investigación', 3, 34, 5, 3),
(1590, 'Educación Musical I ', 3, 34, 5, 3),
(1591, 'Técnicas de la Pintura', 4, 34, 5, 3),
(1592, 'Electiva II', 2, 34, 5, 3),
(1593, 'Práctica Profesional I: Administración y Supervisión de la Educación', 3, 34, 6, 3),
(1594, 'Metodología  y Recursos para el Aprendizaje', 4, 34, 6, 3),
(1595, 'Investigación Social', 4, 34, 6, 3),
(1596, 'Ética y Docencia', 3, 34, 6, 3),
(1597, 'Educación Musical II', 3, 34, 6, 3),
(1598, 'Expresión Teatral I', 2, 34, 6, 3),
(1599, 'Práctica Profesional II:El Docente como Agente de Cambio Social', 3, 34, 7, 4),
(1600, 'Proyecto Social Comunitario', 0, 34, 7, 4),
(1601, 'Pintura al Óleo', 2, 34, 7, 4),
(1602, 'Expresión Teatral II', 2, 34, 7, 4),
(1603, 'Danza II', 2, 34, 7, 4),
(1604, 'Electiva III', 2, 34, 7, 4),
(1605, 'Práctica Profesional III: Docencia Integrada', 7, 34, 8, 4),
(1606, 'Trabajo de Grado', 4, 34, 8, 4),
(1607, 'Lenguaje y Comunicación', 4, 35, 1, 1),
(1608, 'Inglés Instrumental', 2, 35, 1, 1),
(1609, 'Entrenamiento Físico General', 2, 35, 1, 1),
(1610, 'Espacio Geográfico e Identidad Nacional', 2, 35, 1, 1),
(1611, 'Informática', 2, 35, 1, 1),
(1612, 'Matemática General', 4, 35, 1, 1),
(1613, 'Psicología Evolutiva', 3, 35, 2, 1),
(1614, 'Fundamentos de la Educación Especial ', 3, 35, 2, 1),
(1615, 'Atención integral a la persona con Compromisos Cognitivos I', 4, 35, 2, 1),
(1616, 'Prevención, salud y atención temprana ', 3, 35, 2, 1),
(1617, 'Atención integral a la persona con Talento', 4, 35, 2, 1),
(1618, 'Dificultades y Trastornos del lenguaje', 3, 35, 2, 1),
(1619, 'Psicología del Aprendizaje', 3, 35, 3, 2),
(1620, 'Ecología y Educación Ambiental', 3, 35, 3, 2),
(1621, 'Atención integral a la persona con Compromisos Cognitivos II', 4, 35, 3, 2),
(1622, 'Educación sexual a la persona con N.E.E', 3, 35, 3, 2),
(1623, 'Atención integral al Deficiente Visual I', 3, 35, 3, 2),
(1624, 'Electiva I', 2, 35, 3, 2),
(1625, 'Atención integral al Autista', 3, 35, 4, 2),
(1626, 'Estadística Descriptiva', 3, 35, 4, 2),
(1627, 'Planificación de la Enseñanza en la Educación Especial', 4, 35, 4, 2),
(1628, 'Atención integral al Deficiente Visual II', 4, 35, 4, 2),
(1629, 'Educación para el trabajo en la E. Esp', 3, 35, 4, 2),
(1630, 'Atención integral al Deficiente auditivo I', 4, 35, 4, 2),
(1631, 'Gerencia Educativa', 3, 35, 5, 3),
(1632, 'Creatividad en la Educación Especial', 3, 35, 5, 3),
(1633, 'Metodología de la Investigación ', 3, 35, 5, 3),
(1634, 'Atención integral al Deficiente Auditivo II', 4, 35, 5, 3),
(1635, 'Integración Psicosocial', 3, 35, 5, 3),
(1636, 'Electiva II', 2, 35, 5, 3),
(1637, 'Práctica Profesional I: El Docente como Agente de Cambio Social y su rol en equipos interdisciplinarios', 3, 35, 6, 3),
(1638, 'Metodología y Recursos para el Aprendizaje en la Educación Especial ', 4, 35, 6, 3),
(1639, 'Investigación Social', 4, 35, 6, 3),
(1640, 'Ética y Docencia', 3, 35, 6, 3),
(1641, 'Atención integral a las personas con dificultades de aprendizaje ', 4, 35, 6, 3),
(1642, 'Atención integral a la persona con Impedimentos Físicos', 4, 35, 6, 3),
(1643, 'Práctica Profesional II: Administración y Supervisión de la Educación Especial', 3, 35, 7, 4),
(1644, 'Seminario de Trabajo de Grado', 3, 35, 7, 4),
(1645, 'Atención Integral a la Familia', 3, 35, 7, 4),
(1646, 'Entrenamiento Físico en la Educación Especial', 4, 35, 7, 4),
(1647, 'Entrenamiento Social Independiente', 3, 35, 7, 4),
(1648, 'Electiva III', 2, 35, 7, 4),
(1649, 'Práctica Profesional III: Docencia Integrada', 7, 35, 8, 4),
(1650, 'Proyecto de Investigación en Educación Especial', 4, 35, 8, 4),
(1651, 'Lenguaje y Comunicación', 3, 37, 1, 1),
(1652, 'Inglés 1', 8, 37, 1, 1),
(1653, 'Entrenamiento Físico General', 2, 37, 1, 1),
(1654, 'Informática', 3, 37, 1, 1),
(1655, 'Matemática General', 4, 37, 1, 1),
(1656, 'Psicología Evolutiva', 3, 37, 2, 1),
(1657, 'Lingüística General', 3, 37, 2, 1),
(1658, 'Sociología de la Educación', 3, 37, 2, 1),
(1659, 'Filosofía de la Educación', 3, 37, 2, 1),
(1660, 'Inglés 2', 8, 37, 2, 1),
(1661, 'Ecología y Educación Ambiental', 3, 37, 2, 1),
(1662, 'Psicología del Aprendizaje', 3, 37, 3, 2),
(1663, 'Metodología de la Investigación', 3, 37, 3, 2),
(1664, 'Inglés 3', 6, 37, 3, 2),
(1665, 'Morfosintaxis del Español', 3, 37, 3, 2),
(1666, 'Adquisición de Lenguaje', 4, 37, 3, 2),
(1667, 'Espacio Geográfico e Identidad Nacional', 2, 37, 3, 2),
(1668, 'Orientación Educativa', 3, 37, 4, 2),
(1669, 'Lingüística Aplicada a ELE', 3, 37, 4, 2),
(1670, 'Planificación de la Enseñanza', 4, 37, 4, 2),
(1671, 'Inglés 4', 6, 37, 4, 2),
(1672, 'Fonética y Fonología del Inglés', 3, 37, 4, 2),
(1673, 'Morfosintaxis del Inglés', 3, 37, 4, 2),
(1674, 'Gerencia Educativa', 3, 37, 5, 3),
(1675, 'Evaluación del Aprendizaje', 3, 37, 5, 3),
(1676, 'Lectoescritura 1', 6, 37, 5, 3),
(1677, 'Investigación Social', 4, 37, 5, 3),
(1678, 'Gramática Inglesa', 4, 37, 5, 3),
(1679, 'Electiva I', 2, 37, 5, 3),
(1680, 'Práctica Profesional I: Administración y Supervisión Educativa', 3, 37, 6, 3),
(1681, 'Metodología y Recursos para el Aprendizaje', 4, 37, 6, 3),
(1682, 'Lectoescritura 2', 6, 37, 6, 3),
(1683, 'Ética y Docencia', 3, 37, 6, 3),
(1684, 'Literatura Inglesa I', 3, 37, 6, 3),
(1685, 'Electiva II', 2, 37, 6, 3),
(1686, 'Práctica Profesional II: Agente de Cambio', 3, 37, 7, 4),
(1687, 'Cultura y Civilización Anglosajona', 3, 37, 7, 4),
(1688, 'Estadística Descriptiva', 3, 37, 7, 4),
(1689, 'Estrategias y Metodología para la Enseñanza de Lenguas Extranjeras', 3, 37, 7, 4),
(1690, 'Metodología de la Investigación en Lenguas Extranjeras', 3, 37, 7, 4),
(1691, 'Electiva III', 2, 37, 7, 4),
(1692, 'Literatura Inglesa II', 3, 37, 7, 4),
(1693, 'Práctica Profesional III: Docencia Integrada', 7, 37, 8, 4),
(1694, 'Trabajo de Grado', 4, 37, 8, 4),
(1695, 'Proyecto Nacional y Nueva Ciudadanía', 2, 36, 0, 1),
(1696, 'Taller: Introducción a la Universidad y al Programa', 2, 36, 0, 1),
(1697, 'Introducción a la Orientación Científica y Profesional', 2, 36, 0, 1),
(1698, 'Taller: Capacidades Críticas, Analíticas y Dialógicas', 2, 36, 0, 1),
(1699, 'Seminario: Comunicación e Investigación en Orientación', 2, 36, 0, 1),
(1700, 'Teoría y Práctica de la Comunicación y el Lenguaje', 4, 36, 1, 1),
(1701, 'Matemática General', 4, 36, 1, 1),
(1702, 'Psicología del Desarrollo y el Aprendizaje', 4, 36, 1, 1),
(1703, 'Seminario I: Investigación y Acción Participativa', 4, 36, 1, 1),
(1704, 'Geografía e Historia de Venezuela', 4, 36, 1, 1),
(1705, 'Vivencial I: Contexto Socio Profesional del Orientador', 4, 36, 1, 1),
(1706, 'Proyecto Socio Integrador I-A: Políticas Educativas y Sistema de Orientación', 5, 36, 1, 1),
(1707, 'Actividad Física, Cultura y Deporte', 4, 36, 2, 1),
(1708, 'Deontología y Legislación en Orientación', 4, 36, 2, 1),
(1709, 'Taller I: Investigación Social', 4, 36, 2, 1),
(1710, 'Vivencial II: La Tutoría, Acompañamiento y Orientación', 4, 36, 2, 1),
(1711, 'Ecología y Ambiente', 4, 36, 2, 1),
(1712, 'Grupo de Electivas 1: Orientación Familiar, Orientación en Salud', 2, 36, 2, 1),
(1713, 'Proyecto Socio Integrador I-B: Políticas Educativas y Sistema de Orientación', 4, 36, 2, 1),
(1714, 'Diseño y Desarrollo de Modelos de Orientación', 4, 36, 3, 2),
(1715, 'Seminario II: Investigación en Orientación y Medios Tecnológicos', 4, 36, 3, 2),
(1716, 'Vivencial III: Servicios de Orientación', 4, 36, 3, 2),
(1717, 'Planificación e Intervención en Orientación', 4, 36, 3, 2),
(1718, 'Grupo de Electivas II: Técnicas y Dinámicas Grupales', 2, 36, 3, 2),
(1719, 'Proyecto Socio Integrador II-A: Medios Tecnológicos y Modelos de Orientación', 5, 36, 3, 2),
(1720, 'Vivencial IV: Diseño de Programas de Orientación', 4, 36, 4, 2),
(1721, 'Psicología del Niño, Niña, Adolescentes y Adulto', 4, 36, 4, 2),
(1722, 'Estadísticas de Investigación en Orientación', 4, 36, 4, 2),
(1723, 'Taller II: Modelos y Procesos de Investigación', 4, 36, 4, 2),
(1724, 'Grupo de Electivas III: Gestión de Servicios de Orientación', 2, 36, 4, 2),
(1725, 'Proyecto Socio Integrador II-B: Medios Tecnológicos y Modelos de Orientación', 4, 36, 4, 2),
(1726, 'Seminario III: Metodología para la Sistematización de Experiencias Investigativas', 4, 36, 5, 3),
(1727, 'Orientación Vocacional e Inserción Socioproductiva', 4, 36, 5, 3),
(1728, 'Aprendizaje, Métodos de Estudio y Conducción de Grupos', 4, 36, 5, 3),
(1729, 'Orientación y Sexualidad', 4, 36, 5, 3),
(1730, 'Práctica I: Planificación y Diseño en Orientación', 4, 36, 5, 3),
(1731, 'Grupo de Electivas IV: Estudios de Casos y Estrategias de Emprendimiento', 2, 36, 5, 3),
(1732, 'Proyecto Socio Integrador III-A: Evaluación y Diseño de Programas de Orientación', 5, 36, 5, 3),
(1733, 'Taller III: Aplicación de Procedimientos para el Análisis y Sistematización de las Experiencias de Intervención en Orientación', 4, 36, 6, 3),
(1734, 'Práctica II: Ejercicio de la Orientación en Ámbitos Educativos, Comunitarios y Socioproductivos', 4, 36, 6, 3),
(1735, 'Orientación para el Emprendimiento', 4, 36, 6, 3),
(1736, 'Modelos de Intervención en Orientación', 4, 36, 6, 3),
(1737, 'Grupo de Electivas V: Taller de Autodesarrollo', 2, 36, 6, 3),
(1738, 'Proyecto Socio Integrador III-B: Evaluación y Diseño de Programas de Orientación', 4, 36, 6, 3),
(1739, 'Orientación, Asesoramiento y Consulta a la Población en Situación Especial', 4, 36, 7, 4),
(1740, 'Práctica III: Intervención en el Ámbito Socio-Ocupacional del Orientador', 4, 36, 7, 4),
(1741, 'Gerencia de las Emociones y Calidad de Vida', 4, 36, 7, 4),
(1742, 'Seminario IV: Configuración de Experiencias', 4, 36, 7, 4),
(1743, 'Diseño de Técnicas e Instrumentos en Orientación', 4, 36, 7, 4),
(1744, 'Proyecto Socio Integrador IV-A: Implementación de Planes, Programas o Proyectos en Orientación', 5, 36, 7, 4),
(1745, 'Taller IV: Ejecución y Evaluación de Planes, Programas o Proyectos de Orientación', 4, 36, 8, 4),
(1746, 'Práctica IV: Desempeño del Orientador en su Ámbito Socioprofesional', 4, 36, 8, 4),
(1747, 'Proyecto Final: Sistematización de Experiencias de Investigación', 4, 36, 8, 4),
(1748, 'Proyecto Socio Integrador IV-B: Implementación de Planes, Programas o Proyectos en Orientación', 4, 36, 8, 4);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipos_solicitudes`
--

CREATE TABLE `tipos_solicitudes` (
  `id` bigint(20) NOT NULL,
  `nombre_tipo` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipos_solicitudes`
--

INSERT INTO `tipos_solicitudes` (`id`, `nombre_tipo`) VALUES
(3, 'Cambio de carrera'),
(4, 'Equivalencia'),
(8, 'Inscripciones temporales'),
(5, 'Levante Prelación'),
(1, 'Reingreso'),
(6, 'Retiro temporal'),
(2, 'Traslado'),
(7, 'Unidades de crédito extras');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` bigint(20) NOT NULL,
  `nombre_usuario` varchar(255) NOT NULL,
  `contrasena` varchar(255) NOT NULL,
  `primer_nombre` varchar(255) NOT NULL,
  `segundo_nombre` varchar(255) DEFAULT NULL,
  `primer_apellido` varchar(255) NOT NULL,
  `segundo_apellido` varchar(255) DEFAULT NULL,
  `ci` varchar(50) NOT NULL,
  `correo` varchar(255) NOT NULL,
  `rol` enum('SA','JP','Estudiante') NOT NULL,
  `fecha_creacion` timestamp NOT NULL DEFAULT current_timestamp(),
  `fecha_modificacion` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `ultimo_acceso` timestamp NULL DEFAULT NULL,
  `foto_perfil` text DEFAULT NULL,
  `sede_id` bigint(20) DEFAULT NULL,
  `municipio_id` bigint(20) DEFAULT NULL,
  `codigo_recuperacion` varchar(255) DEFAULT NULL,
  `fecha_expiracion_codigo` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id`, `nombre_usuario`, `contrasena`, `primer_nombre`, `segundo_nombre`, `primer_apellido`, `segundo_apellido`, `ci`, `correo`, `rol`, `fecha_creacion`, `fecha_modificacion`, `ultimo_acceso`, `foto_perfil`, `sede_id`, `municipio_id`, `codigo_recuperacion`, `fecha_expiracion_codigo`) VALUES
(1, 'superadmin', 'e34f92a20532a873cb3184398070b4b82a8fa29cf48572c203dc5f0fa6158231', 'Admin', NULL, 'Principal', NULL, '12423452345', 'superadmin@test@gmail.com', 'SA', '2024-12-03 02:18:47', '2024-12-24 07:57:53', NULL, NULL, NULL, NULL, NULL, NULL),
(2, 'Subparquitectura', '5010cc23745c895642d79b71dd1bc97cc12acd488eaed9a47f1013e69ad8386d', '', NULL, '', NULL, 'temporal-692de359c4', 'temporal_d17fbc1a28@temporal.com', 'JP', '2025-02-06 00:40:43', '2025-02-06 00:40:43', NULL, NULL, NULL, NULL, NULL, NULL),
(3, 'Subpingcivil', 'eedc363f856258608300d0c1b22e2c2d47da4ee5934fd0cd886f2d328fdc081c', '', NULL, '', NULL, 'temporal-39423ab4e8', 'temporal_2928975fad@temporal.com', 'JP', '2025-02-06 00:49:12', '2025-02-06 00:49:12', NULL, NULL, NULL, NULL, NULL, NULL),
(4, 'Subpingpetroleo', '8878e3b9bfcbc273f700f34d368791d3814970c8c5b522fedb0d79db1cf27062', '', NULL, '', NULL, 'temporal-70f4538e6a', 'temporal_06676f07d1@temporal.com', 'JP', '2025-02-06 00:52:35', '2025-02-06 00:52:35', NULL, NULL, NULL, NULL, NULL, NULL),
(5, 'Subpmeteorologia', 'eba22cdac0b3bf6221c7b33b4f19f4cb6fabc3fdc053e6d9c4d976e92dc438c8', '', NULL, '', NULL, 'temporal-4489ef7017', 'temporal_e3fe2dea9c@temporal.com', 'JP', '2025-02-06 00:56:51', '2025-02-06 00:56:51', NULL, NULL, NULL, NULL, NULL, NULL),
(6, 'Subpconstruccioncivil', '4be6507570453646bb8079838e203d8aed13869bb41d4f336752afea6b09128f', '', NULL, '', NULL, 'temporal-ace26348ba', 'temporal_8ebc4ac2b1@temporal.com', 'JP', '2025-02-06 01:02:24', '2025-02-06 01:02:24', NULL, NULL, NULL, NULL, NULL, NULL),
(7, 'Subptsuinformatica', '285e9f561a8e523bd175beed06960f63faf94f964a5232cd836011d214fc8e62', '', NULL, '', NULL, 'temporal-9b997bb76a', 'temporal_1151f2c9b2@temporal.com', 'JP', '2025-02-06 01:21:27', '2025-02-06 01:21:27', NULL, NULL, NULL, NULL, NULL, NULL),
(8, 'Subpbotanicatropical', '86ea0faf23dad827be6cd6730f986e6d5e2f79f10bdb526de011422c500a3080', '', NULL, '', NULL, 'temporal-c6f63d3fa3', 'temporal_77f5cb461b@temporal.com', 'JP', '2025-02-06 01:25:41', '2025-02-06 01:25:41', NULL, NULL, NULL, NULL, NULL, NULL),
(9, 'Subpenfermeria', '45afa743e0bcd2958437c2c24c3ccdb27dd06101c111c6afd406ab573440e06e', '', NULL, '', NULL, 'temporal-4ffbebcb73', 'temporal_b0808aa209@temporal.com', 'JP', '2025-02-06 01:27:40', '2025-02-06 01:27:40', NULL, NULL, NULL, NULL, NULL, NULL),
(10, 'Subpestadistica', 'ccbf0093f38f4a8ecf65633f4b96947f1ecc578d999d742b5accbf4e125cd132', '', NULL, '', NULL, 'temporal-9c46978ca3', 'temporal_06379a3d62@temporal.com', 'JP', '2025-02-06 01:29:49', '2025-02-06 01:29:49', NULL, NULL, NULL, NULL, NULL, NULL),
(11, 'Subpeconomiaagricola', 'd2105c10f81d369ff1bc26dfa57f0e6326e995b5d2f6c61a2f815ed7be2f616e', '', NULL, '', NULL, 'temporal-47eef23d76', 'temporal_cc5bd8a54e@temporal.com', 'JP', '2025-02-06 01:32:13', '2025-02-06 01:32:13', NULL, NULL, NULL, NULL, NULL, NULL),
(12, 'Subpingagroindustrial', '77a9053e85b7dcb9e672bedca851e6e32619c5129050efd93dc548483270ce8e', '', NULL, '', NULL, 'temporal-631ede68b1', 'temporal_77a4da2622@temporal.com', 'JP', '2025-02-06 01:36:22', '2025-02-06 01:36:22', NULL, NULL, NULL, NULL, NULL, NULL),
(13, 'Subpingagronomica', '80ef221610d01c4e5fc4509a3e3dacb25a1561b618e75c2ab27c6ef4a2369d3a', '', NULL, '', NULL, 'temporal-b2976e5809', 'temporal_199ff0b821@temporal.com', 'JP', '2025-02-06 01:39:05', '2025-02-06 01:39:05', NULL, NULL, NULL, NULL, NULL, NULL),
(14, 'Subpingproanimal', '2277fc5179550f28ce6c8f7924bbc4fb51796ab9eabccd5a8c8573dabf84ce6f', '', NULL, '', NULL, 'temporal-02f0e7ca5d', 'temporal_826d5a800e@temporal.com', 'JP', '2025-02-06 01:44:32', '2025-02-06 01:44:32', NULL, NULL, NULL, NULL, NULL, NULL),
(15, 'Subpveterinaria', '62d70e22e37eaa91badb62232abd78f09d7783a9e19f47c6ed784216085066d5', '', NULL, '', NULL, 'temporal-7096d15588', 'temporal_53156edb60@temporal.com', 'JP', '2025-02-06 01:45:59', '2025-02-06 01:45:59', NULL, NULL, NULL, NULL, NULL, NULL),
(16, 'Subpderecho', '053670e4a134432e074712a8c6e5c303b0a7c7e7b40570607dbb250ab6672db1', '', NULL, '', NULL, 'temporal-704863ee6e', 'temporal_36726ef617@temporal.com', 'JP', '2025-02-06 01:48:01', '2025-02-06 01:48:01', NULL, NULL, NULL, NULL, NULL, NULL),
(17, 'Subpadministracion', 'be3e1857aa1c391e1fc20d3b436a7492ff04ddca3d3dd21dfb46c8c1051689dc', '', NULL, '', NULL, 'temporal-d7d2fbc0e6', 'temporal_017dc94807@temporal.com', 'JP', '2025-02-06 01:49:08', '2025-02-06 01:49:08', NULL, NULL, NULL, NULL, NULL, NULL),
(18, 'Subpagropecuaria', '5cd2cf75fe4c15cb997ed2749e6ca50753dde4ade843d4ed7366fb2962ed715a', '', NULL, '', NULL, 'temporal-c49dfec304', 'temporal_5072ff7147@temporal.com', 'JP', '2025-02-06 01:50:53', '2025-02-06 01:50:53', NULL, NULL, NULL, NULL, NULL, NULL),
(19, 'Subpcontaduria', 'c73e0501989e70e4ca7e738569da53c728e4d896bd71b3a7bab01ade6c30de09', '', NULL, '', NULL, 'temporal-7af4a27d3d', 'temporal_2b755702dc@temporal.com', 'JP', '2025-02-06 01:52:52', '2025-02-06 01:52:52', NULL, NULL, NULL, NULL, NULL, NULL),
(20, 'Subpsociologia', '08db69f008dab1472b26e2cfd3ad321479aa2325e4c3ca848d38ab2b1972c2c4', '', NULL, '', NULL, 'temporal-073ba52ea6', 'temporal_07be032acd@temporal.com', 'JP', '2025-02-06 01:54:57', '2025-02-06 01:54:57', NULL, NULL, NULL, NULL, NULL, NULL),
(21, 'Subpturismo', 'df3bcb87f96570dc717c291c6162f4cd3cb8704cd977cb1d12bf4df734b1dafd', '', NULL, '', NULL, 'temporal-115b1d9f27', 'temporal_88c36d58b7@temporal.com', 'JP', '2025-02-06 01:56:44', '2025-02-06 01:56:44', NULL, NULL, NULL, NULL, NULL, NULL),
(22, 'Subpbiologia', '08c89bdab02c9ff91b6a8c76424d7fb9c13d9f84fad4cd4a81f6f5047a848dc4', '', NULL, '', NULL, 'temporal-79af35f283', 'temporal_56ae42bb52@temporal.com', 'JP', '2025-02-06 01:58:32', '2025-02-06 01:58:32', NULL, NULL, NULL, NULL, NULL, NULL),
(23, 'Subpcastellano', 'a9bb6d75ce44ee6b1bebaafec8c4d5dc6f6bbc3bc808c6de771ffda7c62cdf62', '', NULL, '', NULL, 'temporal-b2e0786c69', 'temporal_46dcadde29@temporal.com', 'JP', '2025-02-06 02:00:34', '2025-02-06 02:00:34', NULL, NULL, NULL, NULL, NULL, NULL),
(24, 'Subpeduintegral', 'a27de590fa18ff47c2a141a4ccc5d3122dbcbb7055d6b69618c378231db2c82a', '', NULL, '', NULL, 'temporal-9afd6bb3a4', 'temporal_6bfe996e41@temporal.com', 'JP', '2025-02-06 02:02:21', '2025-02-06 02:02:21', NULL, NULL, NULL, NULL, NULL, NULL),
(25, 'Subpedumenfisica', 'ba3f5fd75ebf3ad856cbe0e1c78285fda608a0f77762e19bfe79137253e9a20f', '', NULL, '', NULL, 'temporal-eb9186ac4b', 'temporal_827e491c3e@temporal.com', 'JP', '2025-02-06 02:05:17', '2025-02-06 02:05:17', NULL, NULL, NULL, NULL, NULL, NULL),
(26, 'Subpedumendeporte', '6260603a91e2df6680d42dacad8c7e5a90dd9a788db292c7ac09442ec95fbe1e', '', NULL, '', NULL, 'temporal-699a8f6eaa', 'temporal_a7cea05a44@temporal.com', 'JP', '2025-02-06 02:07:20', '2025-02-06 02:07:20', NULL, NULL, NULL, NULL, NULL, NULL),
(27, 'Subpedumenmatematica', '67a66423370312df0dc19d6c770093aaa35b212260ed32bacb14b043f81f1ff2', '', NULL, '', NULL, 'temporal-a50ad13216', 'temporal_2d799ebd6f@temporal.com', 'JP', '2025-02-06 02:09:33', '2025-02-06 02:09:33', NULL, NULL, NULL, NULL, NULL, NULL),
(28, 'Subpedumenquimica', '3d304313bde63b42422662a515d09ad96f02051f47977112d0a258f7add80df6', '', NULL, '', NULL, 'temporal-4fce319a0d', 'temporal_ad59d290a1@temporal.com', 'JP', '2025-02-06 02:12:12', '2025-02-06 02:12:12', NULL, NULL, NULL, NULL, NULL, NULL),
(29, 'Subpedumengeografia', '66507def03814bbcf1ace0011bb6b8d77a573baab208c67f8952c93ea3e29e91', '', NULL, '', NULL, 'temporal-367b0fbae1', 'temporal_cbe0ca2d59@temporal.com', 'JP', '2025-02-06 02:14:07', '2025-02-06 02:14:07', NULL, NULL, NULL, NULL, NULL, NULL),
(30, 'Subppfgagropecuaria', '66a901a0206b8f192959f326fc878f67f630e5ebbafde60d1e4ebd9085837f12', '', NULL, '', NULL, 'temporal-a486a84664', 'temporal_3680c459cb@temporal.com', 'JP', '2025-02-06 02:17:16', '2025-02-06 02:17:16', NULL, NULL, NULL, NULL, NULL, NULL),
(31, 'Subpmenarte', '76e1a0c720172276b678b2769df33ee147e44c91e9d4dc9a84afbdc344e14543', '', NULL, '', NULL, 'temporal-408d0d70ad', 'temporal_f76e582abd@temporal.com', 'JP', '2025-02-06 02:19:14', '2025-02-06 02:19:14', NULL, NULL, NULL, NULL, NULL, NULL),
(32, 'Subpeduespecial', '416d4fd14ac328c6050e44aa48b3203b118a7b719261061a7083e8ed2805a3c4', '', NULL, '', NULL, 'temporal-179a30eca4', 'temporal_dcc1fb5cad@temporal.com', 'JP', '2025-02-06 02:20:57', '2025-02-06 02:20:57', NULL, NULL, NULL, NULL, NULL, NULL),
(33, 'Subplicorientacion', 'e5a30ecbced9269dbcf7dd0e18186cbe2d4a33de8a1f9f329428ae6c46434372', '', NULL, '', NULL, 'temporal-485187cc4e', 'temporal_c38790d7e7@temporal.com', 'JP', '2025-02-06 02:22:47', '2025-02-06 02:22:47', NULL, NULL, NULL, NULL, NULL, NULL),
(34, 'Subppnfingles', '2921fa90ab05177240d8b0d6678e186fec70873b6b87386df66f1d433e73efdd', '', NULL, '', NULL, 'temporal-1980dfce2b', 'temporal_c7caf581a5@temporal.com', 'JP', '2025-02-06 02:24:58', '2025-02-06 02:24:58', NULL, NULL, NULL, NULL, NULL, NULL);

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `detalles_solicitudes`
--
ALTER TABLE `detalles_solicitudes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `solicitud_id` (`solicitud_id`),
  ADD KEY `tipo_solicitud_id` (`tipo_solicitud_id`);

--
-- Indices de la tabla `historial_solicitudes`
--
ALTER TABLE `historial_solicitudes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`),
  ADD KEY `sede_id_anterior` (`sede_id_anterior`),
  ADD KEY `municipio_id_anterior` (`municipio_id_anterior`),
  ADD KEY `subprograma_id_anterior` (`subprograma_id_anterior`),
  ADD KEY `nueva_sede_id` (`nueva_sede_id`),
  ADD KEY `nuevo_municipio_id` (`nuevo_municipio_id`),
  ADD KEY `nuevo_subprograma_id` (`nuevo_subprograma_id`),
  ADD KEY `jp_id` (`jp_id`),
  ADD KEY `fk_historial_solicitudes_solicitud` (`solicitud_id`),
  ADD KEY `subprogramas_estudiantes_id` (`subprogramas_estudiantes_id`),
  ADD KEY `fk_numero_caso` (`numero_caso`),
  ADD KEY `fk_numero_resolucion` (`numero_resolucion`);

--
-- Indices de la tabla `jefe_subprogramas`
--
ALTER TABLE `jefe_subprogramas`
  ADD PRIMARY KEY (`jefe_id`,`subprograma_id`),
  ADD KEY `subprograma_id` (`subprograma_id`);

--
-- Indices de la tabla `municipios`
--
ALTER TABLE `municipios`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `notificaciones`
--
ALTER TABLE `notificaciones`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`);

--
-- Indices de la tabla `programas`
--
ALTER TABLE `programas`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `sedes`
--
ALTER TABLE `sedes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `fk_sedes_municipio` (`municipio_id`);

--
-- Indices de la tabla `solicitudes`
--
ALTER TABLE `solicitudes`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `unique_numero_caso` (`numero_caso`),
  ADD UNIQUE KEY `unique_numero_resolucion` (`numero_resolucion`),
  ADD KEY `usuario_id` (`usuario_id`),
  ADD KEY `nueva_sede_id` (`nueva_sede_id`),
  ADD KEY `nuevo_municipio_id` (`nuevo_municipio_id`),
  ADD KEY `nuevo_subprograma_id` (`nuevo_subprograma_id`),
  ADD KEY `jp_id` (`jp_id`),
  ADD KEY `fk_sede_anterior` (`sede_id_anterior`),
  ADD KEY `fk_municipio_anterior` (`municipio_id_anterior`),
  ADD KEY `fk_subprograma_anterior` (`subprograma_id_anterior`),
  ADD KEY `subprogramas_estudiantes_id` (`subprogramas_estudiantes_id`);

--
-- Indices de la tabla `subprogramas`
--
ALTER TABLE `subprogramas`
  ADD PRIMARY KEY (`id`),
  ADD KEY `programa_id` (`programa_id`);

--
-- Indices de la tabla `subprogramas_estudiantes`
--
ALTER TABLE `subprogramas_estudiantes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`),
  ADD KEY `subprograma_id` (`subprograma_id`),
  ADD KEY `sede_id` (`sede_id`);

--
-- Indices de la tabla `subprogramas_sedes`
--
ALTER TABLE `subprogramas_sedes`
  ADD PRIMARY KEY (`id`),
  ADD KEY `subprograma_id` (`subprograma_id`),
  ADD KEY `sede_id` (`sede_id`);

--
-- Indices de la tabla `sub_proyectos`
--
ALTER TABLE `sub_proyectos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `subprograma_id` (`subprograma_id`);

--
-- Indices de la tabla `tipos_solicitudes`
--
ALTER TABLE `tipos_solicitudes`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `nombre_tipo` (`nombre_tipo`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `ci` (`ci`),
  ADD UNIQUE KEY `correo` (`correo`),
  ADD KEY `sede_id` (`sede_id`),
  ADD KEY `municipio_id` (`municipio_id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `detalles_solicitudes`
--
ALTER TABLE `detalles_solicitudes`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `historial_solicitudes`
--
ALTER TABLE `historial_solicitudes`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `municipios`
--
ALTER TABLE `municipios`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- AUTO_INCREMENT de la tabla `notificaciones`
--
ALTER TABLE `notificaciones`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `programas`
--
ALTER TABLE `programas`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `sedes`
--
ALTER TABLE `sedes`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=23;

--
-- AUTO_INCREMENT de la tabla `solicitudes`
--
ALTER TABLE `solicitudes`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `subprogramas`
--
ALTER TABLE `subprogramas`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=38;

--
-- AUTO_INCREMENT de la tabla `subprogramas_estudiantes`
--
ALTER TABLE `subprogramas_estudiantes`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `subprogramas_sedes`
--
ALTER TABLE `subprogramas_sedes`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=219;

--
-- AUTO_INCREMENT de la tabla `sub_proyectos`
--
ALTER TABLE `sub_proyectos`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1749;

--
-- AUTO_INCREMENT de la tabla `tipos_solicitudes`
--
ALTER TABLE `tipos_solicitudes`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` bigint(20) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=35;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `detalles_solicitudes`
--
ALTER TABLE `detalles_solicitudes`
  ADD CONSTRAINT `detalles_solicitudes_ibfk_1` FOREIGN KEY (`solicitud_id`) REFERENCES `solicitudes` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `detalles_solicitudes_ibfk_2` FOREIGN KEY (`tipo_solicitud_id`) REFERENCES `tipos_solicitudes` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `historial_solicitudes`
--
ALTER TABLE `historial_solicitudes`
  ADD CONSTRAINT `fk_historial_solicitudes_solicitud` FOREIGN KEY (`solicitud_id`) REFERENCES `solicitudes` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `historial_solicitudes_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `historial_solicitudes_ibfk_2` FOREIGN KEY (`sede_id_anterior`) REFERENCES `sedes` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `historial_solicitudes_ibfk_3` FOREIGN KEY (`municipio_id_anterior`) REFERENCES `municipios` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `historial_solicitudes_ibfk_4` FOREIGN KEY (`subprograma_id_anterior`) REFERENCES `subprogramas` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `historial_solicitudes_ibfk_5` FOREIGN KEY (`nueva_sede_id`) REFERENCES `sedes` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `historial_solicitudes_ibfk_6` FOREIGN KEY (`nuevo_municipio_id`) REFERENCES `municipios` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `historial_solicitudes_ibfk_7` FOREIGN KEY (`nuevo_subprograma_id`) REFERENCES `subprogramas` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `historial_solicitudes_ibfk_8` FOREIGN KEY (`jp_id`) REFERENCES `usuarios` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `historial_solicitudes_ibfk_9` FOREIGN KEY (`subprogramas_estudiantes_id`) REFERENCES `subprogramas_estudiantes` (`id`);

--
-- Filtros para la tabla `jefe_subprogramas`
--
ALTER TABLE `jefe_subprogramas`
  ADD CONSTRAINT `jefe_subprogramas_ibfk_1` FOREIGN KEY (`jefe_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `jefe_subprogramas_ibfk_2` FOREIGN KEY (`subprograma_id`) REFERENCES `subprogramas` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `notificaciones`
--
ALTER TABLE `notificaciones`
  ADD CONSTRAINT `notificaciones_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `sedes`
--
ALTER TABLE `sedes`
  ADD CONSTRAINT `fk_sedes_municipio` FOREIGN KEY (`municipio_id`) REFERENCES `municipios` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `solicitudes`
--
ALTER TABLE `solicitudes`
  ADD CONSTRAINT `fk_municipio_anterior` FOREIGN KEY (`municipio_id_anterior`) REFERENCES `municipios` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_sede_anterior` FOREIGN KEY (`sede_id_anterior`) REFERENCES `sedes` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_subprograma_anterior` FOREIGN KEY (`subprograma_id_anterior`) REFERENCES `subprogramas` (`id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `solicitudes_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `solicitudes_ibfk_2` FOREIGN KEY (`nueva_sede_id`) REFERENCES `sedes` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `solicitudes_ibfk_3` FOREIGN KEY (`nuevo_municipio_id`) REFERENCES `municipios` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `solicitudes_ibfk_4` FOREIGN KEY (`nuevo_subprograma_id`) REFERENCES `subprogramas` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `solicitudes_ibfk_5` FOREIGN KEY (`jp_id`) REFERENCES `usuarios` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `solicitudes_ibfk_6` FOREIGN KEY (`subprogramas_estudiantes_id`) REFERENCES `subprogramas_estudiantes` (`id`);

--
-- Filtros para la tabla `subprogramas`
--
ALTER TABLE `subprogramas`
  ADD CONSTRAINT `subprogramas_ibfk_3` FOREIGN KEY (`programa_id`) REFERENCES `programas` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `subprogramas_estudiantes`
--
ALTER TABLE `subprogramas_estudiantes`
  ADD CONSTRAINT `subprogramas_estudiantes_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `subprogramas_estudiantes_ibfk_2` FOREIGN KEY (`subprograma_id`) REFERENCES `subprogramas` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `subprogramas_estudiantes_ibfk_3` FOREIGN KEY (`sede_id`) REFERENCES `sedes` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `subprogramas_sedes`
--
ALTER TABLE `subprogramas_sedes`
  ADD CONSTRAINT `subprogramas_sedes_ibfk_1` FOREIGN KEY (`subprograma_id`) REFERENCES `subprogramas` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `subprogramas_sedes_ibfk_2` FOREIGN KEY (`sede_id`) REFERENCES `sedes` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `sub_proyectos`
--
ALTER TABLE `sub_proyectos`
  ADD CONSTRAINT `sub_proyectos_ibfk_1` FOREIGN KEY (`subprograma_id`) REFERENCES `subprogramas` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD CONSTRAINT `usuarios_ibfk_1` FOREIGN KEY (`sede_id`) REFERENCES `sedes` (`id`) ON DELETE SET NULL,
  ADD CONSTRAINT `usuarios_ibfk_2` FOREIGN KEY (`municipio_id`) REFERENCES `municipios` (`id`) ON DELETE SET NULL;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
