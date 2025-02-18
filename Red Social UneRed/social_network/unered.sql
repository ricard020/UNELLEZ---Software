-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 14-02-2025 a las 07:42:30
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `unered`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `archivos_proyectos`
--

CREATE TABLE `archivos_proyectos` (
  `id` int(11) NOT NULL,
  `proyecto_id` int(11) NOT NULL,
  `archivo_url` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `archivos_proyectos`
--

INSERT INTO `archivos_proyectos` (`id`, `proyecto_id`, `archivo_url`) VALUES
(1, 3, '../uploads/threads/docs/docsEstrategias para Mejorar la Sostenibilidad Económica de Familias en la Urbanización Rómulo Betancourt.pptx'),
(2, 5, '../uploads/threads/docs/docsInforme sobre el Uso de Redes Sociales en Internet y Niveles de Autoestima en sus Usuarios - Sistemas Operativos - Módulo IV - Sección M-01.pdf'),
(3, 7, '../uploads/threads/docs/docsEnsayo sobre el Método Biográfico (Historia de Vida) - Grupo 5 - Sección FS-01 - Investigación Social.docx');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `categorias`
--

CREATE TABLE `categorias` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `categorias`
--

INSERT INTO `categorias` (`id`, `nombre`) VALUES
(2, 'Agronomía y Ciencias del Agro'),
(12, 'Arte y Diseño'),
(25, 'Astronomía y Ciencias Espaciales'),
(5, 'Ciencias de la Salud'),
(21, 'Conservación de Recursos Naturales'),
(11, 'Cultura y Patrimonio Local'),
(22, 'Deportes y Recreación'),
(17, 'Derecho y Ciencias Jurídicas'),
(13, 'Desarrollo Comunitario'),
(1, 'Desarrollo de Software'),
(8, 'Economía y Finanzas'),
(6, 'Educación y Formación Docente'),
(20, 'Emprendimiento Estudiantil'),
(9, 'Energías Renovables'),
(28, 'Estudios Sociales y Políticos'),
(3, 'Gestión Ambiental'),
(4, 'Ingeniería Civil'),
(18, 'Ingeniería en Sistemas'),
(7, 'Investigación Científica'),
(27, 'Literatura y Escritura Creativa'),
(29, 'Marketing Digital para Proyectos Estudiantiles'),
(15, 'Producción Agroindustrial'),
(26, 'Producción Audiovisual y Fotografía'),
(24, 'Proyectos de Vinculación Social'),
(19, 'Proyectos Innovadores para el Campo'),
(10, 'Proyectos Sociales'),
(23, 'Psicología y Bienestar'),
(30, 'Sistemas Hidráulicos y Gestión del Agua'),
(16, 'Tecnología de los Alimentos'),
(14, 'Turismo Sostenible');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `colaboradores`
--

CREATE TABLE `colaboradores` (
  `id` int(11) NOT NULL,
  `proyecto_id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `comentarios`
--

CREATE TABLE `comentarios` (
  `id` int(11) NOT NULL,
  `proyecto_id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `comentario` text NOT NULL,
  `fecha_comentario` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Disparadores `comentarios`
--
DELIMITER $$
CREATE TRIGGER `after_comment` AFTER INSERT ON `comentarios` FOR EACH ROW BEGIN
    INSERT INTO notificaciones (usuario_id, accion, origen_usuario_id, proyecto_id, mensaje, leido)
    VALUES (
        (SELECT usuario_id FROM proyectos WHERE id = NEW.proyecto_id), -- Receptor (dueño del proyecto)
        'Comentario', -- Acción
        NEW.usuario_id, -- Emisor (usuario que comenta)
        NEW.proyecto_id, -- Proyecto relacionado
        CONCAT('El usuario ', (SELECT nombre FROM usuarios WHERE id = NEW.usuario_id), ' comentó en tu proyecto.'), -- Mensaje
        0 -- Notificación no leída
    );
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `imagenes_proyectos`
--

CREATE TABLE `imagenes_proyectos` (
  `id` int(11) NOT NULL,
  `proyecto_id` int(11) NOT NULL,
  `imagen_url` varchar(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `imagenes_proyectos`
--

INSERT INTO `imagenes_proyectos` (`id`, `proyecto_id`, `imagen_url`) VALUES
(1, 1, '../uploads/threads/images/imagesarmando-oct-princ.png'),
(2, 2, '../uploads/threads/images/imagespsicologia-positiva-consejos.jpg'),
(3, 3, '../uploads/threads/images/imagesMIL20240901133401.jpg'),
(4, 4, '../uploads/threads/images/imagesdiablos-danzantes-de-yare-1.jpg'),
(5, 5, '../uploads/threads/images/images654134820_00.jpg'),
(6, 6, '../uploads/threads/images/imagesprivacidad-digital-e1609766761723.jpg'),
(7, 7, '../uploads/threads/images/imageshistoria-de-vida.png');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `notificaciones`
--

CREATE TABLE `notificaciones` (
  `id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `accion` enum('Retweet','Me gusta','Comentario') NOT NULL,
  `origen_usuario_id` int(11) NOT NULL,
  `proyecto_id` int(11) DEFAULT NULL,
  `mensaje` text NOT NULL,
  `leido` tinyint(1) DEFAULT 0,
  `fecha_notificacion` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proyectos`
--

CREATE TABLE `proyectos` (
  `id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `titulo` varchar(255) NOT NULL,
  `descripcion` text NOT NULL,
  `fecha_publicacion` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `proyectos`
--

INSERT INTO `proyectos` (`id`, `usuario_id`, `titulo`, `descripcion`, `fecha_publicacion`) VALUES
(1, 1, 'La Economía Circular: Un Camino hacia la Sostenibilidad', 'La economía circular se presenta como una alternativa innovadora al modelo lineal tradicional de \"tomar, hacer y desechar\", promoviendo un enfoque que busca maximizar el valor de los recursos a lo largo de su ciclo de vida. Este modelo se basa en la reutilización, el reciclaje y la regeneración de materiales, lo que no solo reduce el desperdicio y la contaminación, sino que también fomenta la creación de empleos y el crecimiento económico sostenible. A medida que las empresas y los consumidores se vuelven más conscientes de su impacto ambiental, la transición hacia una economía circular se convierte en una necesidad imperante, ofreciendo una oportunidad única para redefinir nuestras prácticas económicas y construir un futuro más resiliente y responsable.', '2025-02-14 06:12:31'),
(2, 1, 'El Poder de la Mente: Redefiniendo el Bienestar a Través de la Psicología Positiva', 'La psicología positiva nos invita a explorar el bienestar no como un destino, sino como un viaje enriquecedor que todos podemos emprender. A través de prácticas como la gratitud, la meditación y la conexión social, esta disciplina nos enseña a identificar y potenciar nuestras fortalezas innatas, revelando que pequeñas acciones, como expresar agradecimiento o realizar actos de bondad, pueden transformar nuestra perspectiva y la de quienes nos rodean. En un mundo a menudo marcado por el estrés y la ansiedad, la psicología positiva ofrece herramientas prácticas para reprogramar nuestra mente y redescubrir la alegría en lo cotidiano, promoviendo no solo el bienestar individual, sino también la creación de comunidades más resilientes y empáticas, donde el apoyo mutuo y la celebración de logros compartidos se convierten en la norma.', '2025-02-14 06:14:31'),
(3, 2, 'Estrategias para Mejorar la Sostenibilidad Económica de Familias en la Urbanización Rómulo Betancourt', 'Para mejorar la sostenibilidad económica de las familias en la urbanización Rómulo Betancourt en Barinas, es esencial implementar un enfoque integral que combine la educación, la colaboración comunitaria y el acceso a recursos. Una estrategia clave es fomentar la agricultura urbana, promoviendo huertos familiares y comunitarios que no solo proporcionen alimentos frescos y saludables, sino que también generen ingresos adicionales a través de la venta de excedentes en mercados locales. Además, la creación de cooperativas de ahorro y crédito puede empoderar a los residentes, facilitando el acceso a microcréditos que les permitan iniciar o expandir pequeños negocios, así como mejorar su capacidad de ahorro. La capacitación en habilidades emprendedoras y técnicas de gestión financiera es igualmente crucial, ya que equipará a los habitantes con las herramientas necesarias para desarrollar iniciativas sostenibles y adaptarse a las demandas del mercado. Asimismo, es importante establecer alianzas con organizaciones no gubernamentales y entidades gubernamentales que puedan ofrecer apoyo técnico y recursos, creando un ecosistema que favorezca el crecimiento económico local.', '2025-02-14 06:20:18'),
(4, 2, 'La Fiesta de los Diablos Danzantes de Yare: Un Patrimonio Cultural Vivo de Venezuela', 'La Fiesta de los Diablos Danzantes de Yare, celebrada en el estado Miranda, es una de las manifestaciones culturales más emblemáticas de Venezuela y ha sido reconocida por la UNESCO como Patrimonio Cultural Inmaterial de la Humanidad. Esta festividad, que honra a la Virgen de la Candelaria, fusiona elementos de la tradición católica con creencias africanas e indígenas, reflejando la rica diversidad cultural del país. Durante la celebración, los participantes, ataviados con coloridos trajes de diablos y máscaras elaboradas, realizan danzas vibrantes que simbolizan la lucha entre el bien y el mal, acompañados por música de tambores y otros instrumentos tradicionales. \r\n\r\nLo fascinante de esta festividad es su capacidad para unir a la comunidad, ya que los habitantes de Yare se involucran activamente en la organización y ejecución de los eventos, transmitiendo sus tradiciones de generación en generación. A través de la danza, la música y la vestimenta, los Diablos Danzantes cuentan historias de resistencia, fe y comunidad, convirtiéndose en un símbolo de la herencia cultural venezolana que perdura en el tiempo y que es vital para mantener viva la memoria colectiva y fortalecer el sentido de pertenencia entre los habitantes de la región.', '2025-02-14 06:25:40'),
(5, 3, 'El Uso de Redes Sociales en Internet y Niveles de Autoestima en sus Usuarios', 'En un mundo donde la vida se despliega en pantallas brillantes y los momentos se capturan en un clic, las redes sociales se han convertido en el escenario principal de nuestras interacciones y autoexpresiones. Sin embargo, este vibrante universo digital, lleno de likes y comentarios, plantea una pregunta intrigante: ¿cómo afecta realmente a nuestra autoestima? Por un lado, estas plataformas ofrecen un refugio donde las personas pueden compartir sus logros, recibir apoyo y encontrar comunidades afines, lo que puede elevar la autoestima y fomentar un sentido de pertenencia. Pero, por otro lado, la constante exposición a imágenes cuidadosamente curadas y vidas aparentemente perfectas puede desencadenar una espiral de comparaciones que socavan la autoconfianza. Investigaciones han revelado que, mientras algunos usuarios encuentran en las redes un espacio para la validación y el empoderamiento, otros se ven atrapados en un ciclo de ansiedad y autocrítica. Así, el verdadero reto radica en navegar este paisaje digital con conciencia, cultivando una relación saludable con las redes que potencie nuestra autenticidad y bienestar emocional, permitiéndonos disfrutar de sus beneficios sin perder de vista nuestra valía personal.', '2025-02-14 06:30:46'),
(6, 4, 'El Derecho a la Privacidad en la Era Digital: Retos y Perspectivas', 'En un mundo cada vez más interconectado, donde la información personal se comparte y almacena en múltiples plataformas digitales, el derecho a la privacidad se ha convertido en un tema candente en el ámbito del derecho y las ciencias jurídicas. La recopilación masiva de datos por parte de empresas y gobiernos plantea serias preocupaciones sobre cómo se protege la información personal de los individuos y qué derechos tienen sobre sus propios datos. \r\n\r\nLa implementación de regulaciones como el Reglamento General de Protección de Datos (GDPR) en la Unión Europea ha marcado un hito en la protección de la privacidad, estableciendo estándares más estrictos sobre el consentimiento y el manejo de datos personales. Sin embargo, en muchos países, la legislación aún se queda corta frente a la rápida evolución de la tecnología y las prácticas de recolección de datos. Además, el uso de tecnologías emergentes, como la inteligencia artificial y el reconocimiento facial, plantea nuevos dilemas éticos y legales sobre la vigilancia y el control social. En este contexto, los juristas y legisladores se enfrentan al desafío de encontrar un equilibrio entre la innovación tecnológica y la protección de los derechos fundamentales, asegurando que la privacidad de los ciudadanos no se vea comprometida en un mundo donde la información se ha convertido en un recurso valioso y, a menudo, explotado. La discusión sobre el derecho a la privacidad no solo es relevante para los abogados y legisladores, sino que también invita a la sociedad a reflexionar sobre el valor de la intimidad en la era digital y la necesidad de salvaguardar este derecho en un entorno en constante cambio.', '2025-02-14 06:35:30'),
(7, 5, 'El Método Biográfico: Una Ventana a la Historia de Vida de los Individuos', 'El método biográfico, utilizado en las ciencias sociales y la investigación cualitativa, se centra en la recopilación y análisis de las historias de vida de las personas, ofreciendo una perspectiva profunda y rica sobre sus experiencias, valores y contextos sociales. Este enfoque permite a los investigadores explorar cómo los individuos construyen su identidad a lo largo del tiempo, cómo enfrentan desafíos y cómo sus trayectorias se ven influenciadas por factores culturales, económicos y políticos. A través de entrevistas en profundidad, narraciones y análisis de documentos personales, el método biográfico no solo revela la singularidad de cada historia, sino que también permite identificar patrones y tendencias que pueden ser relevantes para comprender fenómenos sociales más amplios.\r\n\r\nUna de las grandes ventajas de este enfoque es su capacidad para dar voz a aquellos que a menudo son marginados en la investigación tradicional, permitiendo que sus experiencias y perspectivas sean reconocidas y valoradas. Además, el método biográfico fomenta una relación más cercana entre el investigador y el sujeto, promoviendo un diálogo que enriquece la comprensión mutua. Sin embargo, también presenta desafíos, como la subjetividad inherente a la interpretación de las narrativas y la necesidad de un enfoque ético que respete la privacidad y la dignidad de los participantes. En un mundo donde las historias personales a menudo se pierden en el ruido de la información masiva, el método biográfico se erige como una herramienta poderosa para rescatar y celebrar la diversidad de experiencias humanas, ofreciendo una ventana única a la complejidad de la vida individual y colectiva.', '2025-02-14 06:41:21');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `proyectos_categorias`
--

CREATE TABLE `proyectos_categorias` (
  `id` int(11) NOT NULL,
  `proyecto_id` int(11) NOT NULL,
  `categoria_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `proyectos_categorias`
--

INSERT INTO `proyectos_categorias` (`id`, `proyecto_id`, `categoria_id`) VALUES
(1, 1, 8),
(2, 2, 23),
(3, 3, 13),
(4, 4, 11),
(5, 5, 28),
(6, 6, 17),
(7, 7, 28);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `retweets`
--

CREATE TABLE `retweets` (
  `id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `proyecto_id` int(11) NOT NULL,
  `fecha_retweet` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Disparadores `retweets`
--
DELIMITER $$
CREATE TRIGGER `after_retweet` AFTER INSERT ON `retweets` FOR EACH ROW BEGIN
    INSERT INTO notificaciones (usuario_id, accion, origen_usuario_id, proyecto_id, mensaje, leido)
    VALUES (
        (SELECT usuario_id FROM proyectos WHERE id = NEW.proyecto_id), -- Receptor (dueño del proyecto)
        'Retweet', -- Acción
        NEW.usuario_id, -- Emisor (usuario que realiza el retweet)
        NEW.proyecto_id, -- Proyecto relacionado
        CONCAT('El usuario ', (SELECT nombre FROM usuarios WHERE id = NEW.usuario_id), ' compartió tu proyecto.'), -- Mensaje
        0 -- Notificación no leída
    );
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `seguidores_categorias`
--

CREATE TABLE `seguidores_categorias` (
  `id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `categoria_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios`
--

CREATE TABLE `usuarios` (
  `id` int(11) NOT NULL,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `carrera` varchar(100) NOT NULL,
  `semestre` varchar(10) NOT NULL,
  `foto_perfil` varchar(255) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  `contrasena` varchar(255) NOT NULL,
  `codigo_recuperacion` varchar(255) DEFAULT NULL,
  `fecha_expiracion_codigo` datetime DEFAULT NULL,
  `fecha_registro` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `usuarios`
--

INSERT INTO `usuarios` (`id`, `nombre`, `apellido`, `carrera`, `semestre`, `foto_perfil`, `email`, `contrasena`, `codigo_recuperacion`, `fecha_expiracion_codigo`, `fecha_registro`) VALUES
(1, 'Abdiel', 'Asad', 'Ingeniería en Informática', 'VIII', '../uploads/profiles/abdiel12052003@gmail.com.jpg', 'abdiel12052003@gmail.com', '$2y$10$FlWiKe5Ge6rotKjBh2flJOF8moeIs4mX6RYxXZGNBpKq2Z.apdP/q', NULL, NULL, '2025-02-14 06:00:51'),
(2, 'Lirio', 'Pérez', 'Ingeniería en Informática', 'IX', '../uploads/profiles/liriodanely020903@gmail.com.jpg', 'liriodanely020903@gmail.com', '$2y$10$um17yC430iHaWsmE3azUFOd1ca2ZZuYhiKM2xs9DNxYQIZkmhMvSW', NULL, NULL, '2025-02-14 06:15:45'),
(3, 'Beiker', 'Daza', 'Ingeniería en Informática', 'VIII', '../uploads/profiles/beiker.eduardo@gmail.com.jpg', 'beiker.eduardo@gmail.com', '$2y$10$7zz48NuhSvcfj0C7uwxP7OspaR7vxI5GCtqt0EjBZZuTYOuERLIli', NULL, NULL, '2025-02-14 06:26:37'),
(4, 'Luis', 'Valero', 'Ingeniería en Informática', 'VIII', '../uploads/profiles/siul200321@gmail.com.jpg', 'siul200321@gmail.com', '$2y$10$47iFUMy0ps707J7zofVBdO1AiB2B0eqTDpLsbFtbkqZyi9Z.wUk8S', NULL, NULL, '2025-02-14 06:31:42'),
(5, 'Bryant', 'Roa', 'Ingeniería en Informática', 'VII', '../uploads/profiles/bryant15j@gmail.com.jpg', 'bryant15j@gmail.com', '$2y$10$LM.Gb.j4WuwhyhPZpARPwOarS88r1owBTuWsYd68AkOZdtSYJtqDm', NULL, NULL, '2025-02-14 06:36:20');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuarios_categorias`
--

CREATE TABLE `usuarios_categorias` (
  `id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `categoria_id` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `valoraciones`
--

CREATE TABLE `valoraciones` (
  `id` int(11) NOT NULL,
  `proyecto_id` int(11) NOT NULL,
  `usuario_id` int(11) NOT NULL,
  `valoracion` enum('Me gusta','No me gusta') NOT NULL,
  `fecha_valoracion` timestamp NOT NULL DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Disparadores `valoraciones`
--
DELIMITER $$
CREATE TRIGGER `after_like` AFTER INSERT ON `valoraciones` FOR EACH ROW BEGIN
    INSERT INTO notificaciones (usuario_id, accion, origen_usuario_id, proyecto_id, mensaje, leido)
    VALUES (
        (SELECT usuario_id FROM proyectos WHERE id = NEW.proyecto_id), -- Receptor (dueño del proyecto)
        'Me gusta', -- Acción
        NEW.usuario_id, -- Emisor (usuario que da "Me gusta")
        NEW.proyecto_id, -- Proyecto relacionado
        CONCAT('El usuario ', (SELECT nombre FROM usuarios WHERE id = NEW.usuario_id), ' dio "Me gusta" a tu proyecto.'), -- Mensaje
        0 -- Notificación no leída
    );
END
$$
DELIMITER ;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `archivos_proyectos`
--
ALTER TABLE `archivos_proyectos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `proyecto_id` (`proyecto_id`);

--
-- Indices de la tabla `categorias`
--
ALTER TABLE `categorias`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `nombre` (`nombre`);

--
-- Indices de la tabla `colaboradores`
--
ALTER TABLE `colaboradores`
  ADD PRIMARY KEY (`id`),
  ADD KEY `proyecto_id` (`proyecto_id`),
  ADD KEY `usuario_id` (`usuario_id`);

--
-- Indices de la tabla `comentarios`
--
ALTER TABLE `comentarios`
  ADD PRIMARY KEY (`id`),
  ADD KEY `proyecto_id` (`proyecto_id`),
  ADD KEY `usuario_id` (`usuario_id`);

--
-- Indices de la tabla `imagenes_proyectos`
--
ALTER TABLE `imagenes_proyectos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `proyecto_id` (`proyecto_id`);

--
-- Indices de la tabla `notificaciones`
--
ALTER TABLE `notificaciones`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`),
  ADD KEY `origen_usuario_id` (`origen_usuario_id`),
  ADD KEY `proyecto_id` (`proyecto_id`);

--
-- Indices de la tabla `proyectos`
--
ALTER TABLE `proyectos`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`);

--
-- Indices de la tabla `proyectos_categorias`
--
ALTER TABLE `proyectos_categorias`
  ADD PRIMARY KEY (`id`),
  ADD KEY `proyecto_id` (`proyecto_id`),
  ADD KEY `categoria_id` (`categoria_id`);

--
-- Indices de la tabla `retweets`
--
ALTER TABLE `retweets`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`),
  ADD KEY `proyecto_id` (`proyecto_id`);

--
-- Indices de la tabla `seguidores_categorias`
--
ALTER TABLE `seguidores_categorias`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`),
  ADD KEY `categoria_id` (`categoria_id`);

--
-- Indices de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email` (`email`);

--
-- Indices de la tabla `usuarios_categorias`
--
ALTER TABLE `usuarios_categorias`
  ADD PRIMARY KEY (`id`),
  ADD KEY `usuario_id` (`usuario_id`),
  ADD KEY `categoria_id` (`categoria_id`);

--
-- Indices de la tabla `valoraciones`
--
ALTER TABLE `valoraciones`
  ADD PRIMARY KEY (`id`),
  ADD KEY `proyecto_id` (`proyecto_id`),
  ADD KEY `usuario_id` (`usuario_id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `archivos_proyectos`
--
ALTER TABLE `archivos_proyectos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `categorias`
--
ALTER TABLE `categorias`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=31;

--
-- AUTO_INCREMENT de la tabla `colaboradores`
--
ALTER TABLE `colaboradores`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `comentarios`
--
ALTER TABLE `comentarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `imagenes_proyectos`
--
ALTER TABLE `imagenes_proyectos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `notificaciones`
--
ALTER TABLE `notificaciones`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `proyectos`
--
ALTER TABLE `proyectos`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `proyectos_categorias`
--
ALTER TABLE `proyectos_categorias`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT de la tabla `retweets`
--
ALTER TABLE `retweets`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `seguidores_categorias`
--
ALTER TABLE `seguidores_categorias`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `usuarios`
--
ALTER TABLE `usuarios`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT de la tabla `usuarios_categorias`
--
ALTER TABLE `usuarios_categorias`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `valoraciones`
--
ALTER TABLE `valoraciones`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `archivos_proyectos`
--
ALTER TABLE `archivos_proyectos`
  ADD CONSTRAINT `archivos_proyectos_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`);

--
-- Filtros para la tabla `colaboradores`
--
ALTER TABLE `colaboradores`
  ADD CONSTRAINT `colaboradores_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`),
  ADD CONSTRAINT `colaboradores_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`);

--
-- Filtros para la tabla `comentarios`
--
ALTER TABLE `comentarios`
  ADD CONSTRAINT `comentarios_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`),
  ADD CONSTRAINT `comentarios_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`);

--
-- Filtros para la tabla `imagenes_proyectos`
--
ALTER TABLE `imagenes_proyectos`
  ADD CONSTRAINT `imagenes_proyectos_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`);

--
-- Filtros para la tabla `notificaciones`
--
ALTER TABLE `notificaciones`
  ADD CONSTRAINT `notificaciones_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`),
  ADD CONSTRAINT `notificaciones_ibfk_2` FOREIGN KEY (`origen_usuario_id`) REFERENCES `usuarios` (`id`),
  ADD CONSTRAINT `notificaciones_ibfk_3` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`);

--
-- Filtros para la tabla `proyectos`
--
ALTER TABLE `proyectos`
  ADD CONSTRAINT `proyectos_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`);

--
-- Filtros para la tabla `proyectos_categorias`
--
ALTER TABLE `proyectos_categorias`
  ADD CONSTRAINT `proyectos_categorias_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`),
  ADD CONSTRAINT `proyectos_categorias_ibfk_2` FOREIGN KEY (`categoria_id`) REFERENCES `categorias` (`id`);

--
-- Filtros para la tabla `retweets`
--
ALTER TABLE `retweets`
  ADD CONSTRAINT `retweets_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`) ON DELETE CASCADE,
  ADD CONSTRAINT `retweets_ibfk_2` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `seguidores_categorias`
--
ALTER TABLE `seguidores_categorias`
  ADD CONSTRAINT `seguidores_categorias_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`),
  ADD CONSTRAINT `seguidores_categorias_ibfk_2` FOREIGN KEY (`categoria_id`) REFERENCES `categorias` (`id`);

--
-- Filtros para la tabla `usuarios_categorias`
--
ALTER TABLE `usuarios_categorias`
  ADD CONSTRAINT `usuarios_categorias_ibfk_1` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`),
  ADD CONSTRAINT `usuarios_categorias_ibfk_2` FOREIGN KEY (`categoria_id`) REFERENCES `categorias` (`id`);

--
-- Filtros para la tabla `valoraciones`
--
ALTER TABLE `valoraciones`
  ADD CONSTRAINT `valoraciones_ibfk_1` FOREIGN KEY (`proyecto_id`) REFERENCES `proyectos` (`id`),
  ADD CONSTRAINT `valoraciones_ibfk_2` FOREIGN KEY (`usuario_id`) REFERENCES `usuarios` (`id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
