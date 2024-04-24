-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 24-04-2024 a las 01:43:40
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
-- Base de datos: `raffarraffa_inmobiliaria`
--
CREATE DATABASE IF NOT EXISTS `raffarraffa_inmobiliaria` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `raffarraffa_inmobiliaria`;

DELIMITER $$
--
-- Procedimientos
--
DROP PROCEDURE IF EXISTS `listarCiudades`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listarCiudades` ()   BEGIN
    select * from ciudad;
END$$

DROP PROCEDURE IF EXISTS `listarUsuarios`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listarUsuarios` ()   BEGIN
    SELECT id,nombre,apellido,dni,email,rol,avatarUrl,borrado FROM usuario;
END$$

DROP PROCEDURE IF EXISTS `listarZonas`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listarZonas` ()   BEGIN
    select * from zona;
END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles` ()   BEGIN 
SELECT
    i.id,
    i.direccion,
    i.uso,
    i.id_tipo,
    i.ambientes,
    i.coordenadas,
    i.latitud,
    i.longitud,
    i.precio,
    i.id_propietario,
    i.estado,
    i.id_ciudad,
    i.id_zona,
    i.borrado,
    i.descripcion,
    t.id AS t_id_tipo ,
    t.tipo AS tipo_inmueble,
    p.id AS p_id,
    p.nombre AS nombre_propietario,
    p.apellido AS apellido_propietario, 
    c.ciudad ,
    z.zona 
FROM inmueble AS i 
   INNER JOIN tipo_inmueble AS t
   ON i.id_tipo = t.id
   INNER JOIN propietario AS p 
   ON i.id_propietario = p.id 
   JOIN ciudad AS c 
   ON c.id = i.id_ciudad 
   JOIN zona AS z ON z.id = i.id_zona 
WHERE i.borrado = 0  ;
END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_2`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_2` ()   BEGIN
SELECT
	i.id,
    i.direccion,
    i.uso,
    i.id_tipo,
    i.ambientes,
    i.coordenadas,
    i.latitud,
    i.longitud,
    i.precio,
    i.id_propietario,
    i.estado,
    i.id_ciudad,
    i.id_zona,
    i.borrado,
    i.descripcion,
    t.id AS t_id_tipo ,
    t.tipo AS tipo_inmueble,
    p.id AS p_id,
    p.nombre AS nombre_propietario,
    p.apellido AS apellido_propietario, 
    c.ciudad ,
    z.zona 
FROM inmueble AS i 
   INNER JOIN tipo_inmueble AS t
   ON i.id_tipo = t.id
   INNER JOIN propietario AS p 
   ON i.id_propietario = p.id 
   JOIN ciudad AS c 
   ON c.id = i.id_ciudad 
   JOIN zona AS z
   ON z.id = i.id_zona 
   JOIN contrato AS cont
   ON i.id = cont.id_inmueble
WHERE i.borrado = 0  ;
END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_3`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_3` ()   BEGIN
SET @f_inicio = '2024-04-01';
SET @f_fin = '2024-04-10';
SELECT
	i.id,
    i.direccion,
    i.uso,
    i.id_tipo,
    i.ambientes,
    i.coordenadas,
    i.latitud,
    i.longitud,
    i.precio,
    i.id_propietario,
    i.estado,
    i.id_ciudad,
    i.id_zona,
    i.borrado,
    i.descripcion,
    t.id AS t_id_tipo ,
    t.tipo AS tipo_inmueble,
    p.id AS p_id,
    p.nombre AS nombre_propietario,
    p.apellido AS apellido_propietario, 
    c.ciudad ,
    z.zona 
FROM inmueble AS i 
   INNER JOIN tipo_inmueble AS t
   ON i.id_tipo = t.id
   INNER JOIN propietario AS p 
   ON i.id_propietario = p.id 
   JOIN ciudad AS c 
   ON c.id = i.id_ciudad 
   JOIN zona AS z
   ON z.id = i.id_zona 
   JOIN contrato AS cont
   ON i.id = cont.id_inmueble
WHERE i.borrado = 0   
	AND (@f_inicio < cont.fecha_fin AND @f_fin > cont.fecha_inicio) ;

END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_4`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_4` (IN `f_inicio` DATE, IN `f_fin` DATE)  SQL SECURITY INVOKER BEGIN

SELECT
    i.id,
    i.direccion,
    i.uso,
    i.id_tipo,
    i.ambientes,
    i.coordenadas,
    i.latitud,
    i.longitud,
    i.precio,
    i.id_propietario,
    i.estado,
    i.id_ciudad,
    i.id_zona,
    i.borrado,
    i.descripcion,
    t.id AS t_id_tipo,
    t.tipo AS tipo_inmueble,
    p.id AS p_id,
    p.nombre AS nombre_propietario,
    p.apellido AS apellido_propietario,
    c.ciudad,
    z.zona
FROM
    inmueble AS i
    INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id
    INNER JOIN propietario AS p ON i.id_propietario = p.id
    JOIN ciudad AS c ON c.id = i.id_ciudad
    JOIN zona AS z ON z.id = i.id_zona
    LEFT JOIN contrato AS cont ON i.id = cont.id_inmueble
        AND (
            cont.fecha_inicio <= f_fin
            AND cont.fecha_fin >= f_inicio
        )
WHERE
    i.borrado = 0
     AND cont.id_inmueble IS NULL
    ;

END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_5`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_5` (IN `f_inicio` DATE, IN `f_fin` DATE)  SQL SECURITY INVOKER BEGIN

    SELECT
        i.id,
        i.direccion,
        i.uso,
        i.id_tipo,
        i.ambientes,
        i.coordenadas,
        i.latitud,
        i.longitud,
        i.precio,
        i.id_propietario,
        i.estado,
        i.id_ciudad,
        i.id_zona,
        i.borrado,
        i.descripcion,
        t.id AS t_id_tipo,
        t.tipo AS tipo_inmueble,
        p.id AS p_id,
        p.nombre AS nombre_propietario,
        p.apellido AS apellido_propietario,
        c.ciudad,
        z.zona
    FROM
        inmueble AS i
        INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id
        INNER JOIN propietario AS p ON i.id_propietario = p.id
        JOIN ciudad AS c ON c.id = i.id_ciudad
        JOIN zona AS z ON z.id = i.id_zona
        LEFT JOIN contrato AS cont 
            ON i.id = cont.id_inmueble
            AND (
	                f_fin <= cont.fecha_inicio 
                OR
					f_inicio >= cont.fecha_fin
            )
    WHERE
        i.borrado = 0
        AND cont.id_inmueble IS NULL;

END$$

DROP PROCEDURE IF EXISTS `listar_inmuebles_6`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `listar_inmuebles_6` (IN `f_inicio` DATE, IN `f_fin` DATE)  SQL SECURITY INVOKER BEGIN

    SELECT
        i.id,
        i.direccion,
        i.uso,
        i.id_tipo,
        i.ambientes,
        i.coordenadas,
        i.latitud,
        i.longitud,
        i.precio,
        i.id_propietario,
        i.estado,
        i.id_ciudad,
        i.id_zona,
        i.borrado,
        i.descripcion,
        t.id AS t_id_tipo,
        t.tipo AS tipo_inmueble,
        p.id AS p_id,
        p.nombre AS nombre_propietario,
        p.apellido AS apellido_propietario,
        c.ciudad,
        z.zona
    FROM
        inmueble AS i
        INNER JOIN tipo_inmueble AS t ON i.id_tipo = t.id
        INNER JOIN propietario AS p ON i.id_propietario = p.id
        JOIN ciudad AS c ON c.id = i.id_ciudad
        JOIN zona AS z ON z.id = i.id_zona
        LEFT JOIN contrato AS cont 
            ON i.id = cont.id_inmueble
            AND (
                (f_fin <= cont.fecha_inicio) 
                OR (f_inicio >= cont.fecha_fin)
            )
        LEFT JOIN contrato AS cont2 
            ON i.id = cont2.id_inmueble
            AND (cont2.fecha_inicio IS NULL OR cont2.fecha_inicio <= f_fin)
            AND (cont2.fecha_fin IS NULL OR cont2.fecha_fin >= f_inicio)
    WHERE
        i.borrado = 0
        AND (cont2.id_inmueble IS NULL)
    ;

END$$

DROP PROCEDURE IF EXISTS `verify_contrato`$$
CREATE DEFINER=`root`@`localhost` PROCEDURE `verify_contrato` (IN `f_inicio` DATE, IN `f_fin` DATE, IN `id_prop` INT)   BEGIN
-- SET @id :=1;
-- SET @in := '2024-04-01';
-- SET @out := '2024-05-01';
 IF (f_inicio < f_fin) THEN
SELECT id
FROM contrato
WHERE
id_inmueble= id_prop
AND NOT (
    (f_fin < fecha_inicio )
OR
    (fecha_fin < f_inicio)
    )
    LIMIT 1
;
ELSE
SET @MESSAGE_TEXT = 'Fecha Inicio Mayor que Fecha Fin';
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = @MESSAGE_TEXT;
    END IF;
END$$

--
-- Funciones
--
DROP FUNCTION IF EXISTS `validarFechas`$$
CREATE DEFINER=`root`@`localhost` FUNCTION `validarFechas` (`f_inicio` DATE, `f_fin` DATE) RETURNS INT(11)  BEGIN
    DECLARE resultado INT;

    IF (f_inicio < f_fin) THEN
        SELECT id INTO resultado
        FROM contrato
        WHERE
            id_inmueble = 1 
            AND (
                (f_fin < fecha_inicio )
                OR
                (fecha_fin < f_inicio)
            );  
    ELSE   
        SET @MESSAGE_TEXT = 'Fecha Inicio Mayor que Fecha Fin';
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = @MESSAGE_TEXT;
    END IF;

    RETURN resultado;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `ciudad`
--

DROP TABLE IF EXISTS `ciudad`;
CREATE TABLE `ciudad` (
  `id` int(11) NOT NULL,
  `ciudad` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contrato`
--

DROP TABLE IF EXISTS `contrato`;
CREATE TABLE `contrato` (
  `id` int(11) NOT NULL,
  `id_inquilino` int(11) NOT NULL,
  `id_inmueble` int(11) NOT NULL,
  `fecha_inicio` date DEFAULT NULL,
  `fecha_fin` date DEFAULT NULL,
  `fecha_efectiva` date DEFAULT NULL,
  `monto` decimal(9,2) DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT 0,
  `creado_fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `creado_usuario` int(11) NOT NULL,
  `borrado_fecha` datetime NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  `borrado_usuario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmueble`
--

DROP TABLE IF EXISTS `inmueble`;
CREATE TABLE `inmueble` (
  `id` int(11) NOT NULL,
  `direccion` varchar(100) NOT NULL COMMENT 'calle y altura',
  `uso` enum('Comercial','Residencial') NOT NULL DEFAULT 'Comercial',
  `id_tipo` int(11) NOT NULL,
  `ambientes` tinyint(2) NOT NULL DEFAULT 1,
  `coordenadas` varchar(100) DEFAULT NULL,
  `precio` decimal(11,2) DEFAULT NULL,
  `id_propietario` int(11) NOT NULL,
  `estado` enum('Disponible','Retirado') NOT NULL DEFAULT 'Disponible',
  `id_ciudad` int(11) NOT NULL,
  `id_zona` int(11) NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0,
  `descripcion` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilino`
--

DROP TABLE IF EXISTS `inquilino`;
CREATE TABLE `inquilino` (
  `id` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(11) NOT NULL,
  `email` varchar(45) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pago`
--

DROP TABLE IF EXISTS `pago`;
CREATE TABLE `pago` (
  `id` int(11) NOT NULL,
  `id_contrato` int(11) NOT NULL,
  `fecha_pago` date NOT NULL DEFAULT current_timestamp(),
  `importe` decimal(11,2) NOT NULL COMMENT 'si es negativo es una nota de credito',
  `estado` varchar(25) DEFAULT NULL,
  `numero_pago` int(10) UNSIGNED NOT NULL DEFAULT 1,
  `detalle` varchar(150) NOT NULL COMMENT 'aca van los detalles de cada abono -> paga el mes x - abono mes x',
  `creado_fecha` datetime NOT NULL DEFAULT current_timestamp(),
  `creado_usuario` int(11) NOT NULL,
  `borrado_fecha` datetime NOT NULL DEFAULT '0000-00-00 00:00:00' ON UPDATE current_timestamp(),
  `borrado_usuario` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietario`
--

DROP TABLE IF EXISTS `propietario`;
CREATE TABLE `propietario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `email` varchar(100) NOT NULL,
  `telefono` varchar(45) NOT NULL,
  `borrado` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `tipo_inmueble`
--

DROP TABLE IF EXISTS `tipo_inmueble`;
CREATE TABLE `tipo_inmueble` (
  `id` int(11) NOT NULL,
  `tipo` varchar(200) NOT NULL,
  `borrado` tinyint(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `tipo_inmueble`
--

INSERT INTO `tipo_inmueble` (`id`, `tipo`, `borrado`) VALUES
(1, 'Casa', 0),
(2, 'Departamento', 0),
(3, 'Deposito', 0),
(4, 'Local', 0),
(5, 'Cabaña', 0),
(6, 'Quinta', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `usuario`
--

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE `usuario` (
  `id` int(11) NOT NULL,
  `nombre` varchar(45) NOT NULL,
  `apellido` varchar(45) NOT NULL,
  `dni` varchar(45) NOT NULL,
  `email` varchar(45) NOT NULL,
  `password` varchar(250) NOT NULL,
  `rol` enum('usuario','administrador') NOT NULL DEFAULT 'usuario' COMMENT 'solo vamos a usar 2 tipos de usuarios.\\n- usuario normal de la plataforma\\n- un  administrador',
  `avatarUrl` varchar(100) DEFAULT NULL,
  `borrado` tinyint(1) DEFAULT 0 COMMENT '0 para activo, 1 para inactivo'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci COMMENT='tabla para usuarios internos del sistema';

--
-- Volcado de datos para la tabla `usuario`
--

INSERT INTO `usuario` (`id`, `nombre`, `apellido`, `dni`, `email`, `password`, `rol`, `avatarUrl`, `borrado`) VALUES
(1, 'Leonel', 'Toloza', '38860057', 'admin@admin.com', '$2a$11$zujeCmTH/ewXdFu738wpqur5i69oPqLpIam6vGtLmHRdr55zft.m6', 'administrador', '3ec637a6-8e23-4e65-990e-39432a5f90b3.webp', 0),
(2, 'Santiago', 'Toloza', '38860057', 'leotoloza6@gmail.com', '$2a$11$zujeCmTH/ewXdFu738wpqur5i69oPqLpIam6vGtLmHRdr55zft.m6', 'usuario', 'Designer.jpeg', 0),
(3, 'Rafael ', 'Lopez', '123456', 'lopezrafa@gmail.com', '$2a$11$fP4bmBL.WdxxJY9pIhSIRuvmDR92qnG4ymEsaCbQACatxpu5rNzDa', 'administrador', 'cristanDROID.png', 0);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `zona`
--

DROP TABLE IF EXISTS `zona`;
CREATE TABLE `zona` (
  `id` int(11) NOT NULL,
  `zona` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `zona`
--

INSERT INTO `zona` (`id`, `zona`) VALUES
(1, 'Norte'),
(2, 'Centro');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `ciudad`
--
ALTER TABLE `ciudad`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD PRIMARY KEY (`id`),
  ADD KEY `contrato_inmueble_idx` (`id_inmueble`),
  ADD KEY `contrato_inquilino_idx` (`id_inquilino`),
  ADD KEY `fecha_inicio` (`fecha_inicio`,`fecha_fin`);

--
-- Indices de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD PRIMARY KEY (`id`),
  ADD KEY `inmueble_tipo_idx` (`id_tipo`),
  ADD KEY `propietario_inmueble_idx` (`id_propietario`),
  ADD KEY `fk_inmueble_ciudad1_idx` (`id_ciudad`),
  ADD KEY `fk_inmueble_zona1_idx` (`id_zona`);

--
-- Indices de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email_UNIQUE` (`email`);

--
-- Indices de la tabla `pago`
--
ALTER TABLE `pago`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id_contrato` (`id_contrato`,`numero_pago`),
  ADD KEY `pago_contrato_idx` (`id_contrato`);

--
-- Indices de la tabla `propietario`
--
ALTER TABLE `propietario`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `email_UNIQUE` (`email`);

--
-- Indices de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `tipo` (`tipo`);

--
-- Indices de la tabla `usuario`
--
ALTER TABLE `usuario`
  ADD PRIMARY KEY (`id`);

--
-- Indices de la tabla `zona`
--
ALTER TABLE `zona`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `ciudad`
--
ALTER TABLE `ciudad`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `contrato`
--
ALTER TABLE `contrato`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inmueble`
--
ALTER TABLE `inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `inquilino`
--
ALTER TABLE `inquilino`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `pago`
--
ALTER TABLE `pago`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `propietario`
--
ALTER TABLE `propietario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `tipo_inmueble`
--
ALTER TABLE `tipo_inmueble`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `usuario`
--
ALTER TABLE `usuario`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `zona`
--
ALTER TABLE `zona`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `contrato`
--
ALTER TABLE `contrato`
  ADD CONSTRAINT `contrato_inmueble` FOREIGN KEY (`id_inmueble`) REFERENCES `inmueble` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `contrato_inquilino` FOREIGN KEY (`id_inquilino`) REFERENCES `inquilino` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `inmueble`
--
ALTER TABLE `inmueble`
  ADD CONSTRAINT `fk_inmueble_ciudad1` FOREIGN KEY (`id_ciudad`) REFERENCES `ciudad` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_inmueble_zona1` FOREIGN KEY (`id_zona`) REFERENCES `zona` (`id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `inmueble_tipo` FOREIGN KEY (`id_tipo`) REFERENCES `tipo_inmueble` (`id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `propietario_inmueble` FOREIGN KEY (`id_propietario`) REFERENCES `propietario` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Filtros para la tabla `pago`
--
ALTER TABLE `pago`
  ADD CONSTRAINT `pago_contrato` FOREIGN KEY (`id_contrato`) REFERENCES `contrato` (`id`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
