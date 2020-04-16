-- phpMyAdmin SQL Dump
-- version 5.0.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 17, 2020 at 01:17 AM
-- Server version: 10.4.11-MariaDB
-- PHP Version: 7.4.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `id12764393_players`
--

-- --------------------------------------------------------

--
-- Table structure for table `movement`
--

CREATE TABLE `movement` (
  `gameId` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `lastMove` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `ifcheck` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `checkMate` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `forfeit` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `askForDraw` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `startX` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `startY` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `endX` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `endY` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `enPassant` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `pawnX` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `pawnY` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `castling` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `rookStartX` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `rookStartY` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `rookEndX` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `rookEndY` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `promotion` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `pawnEvolvesTo` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Draw` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `askForRematch` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `Rematch` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `onlineplayers`
--

CREATE TABLE `onlineplayers` (
  `username` varchar(255) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `playerlobby`
--

CREATE TABLE `playerlobby` (
  `username` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `gameId` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `joinedUser` varchar(255) COLLATE utf8_unicode_ci DEFAULT NULL,
  `playerCount` int(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

-- --------------------------------------------------------

--
-- Table structure for table `playerlogininfo`
--

CREATE TABLE `playerlogininfo` (
  `username` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `password` varchar(255) COLLATE utf8_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

--
-- Dumping data for table `playerlogininfo`
--

INSERT INTO `playerlogininfo` (`username`, `password`) VALUES
('den', '$2y$10$hN.jzJqCB/KYTl3gRSXaF.ToX3HYJA.L2YeNrd69iVjYuF34mZxkC'),
('mike', '$2y$10$NfIHcRq/ecxY3gOvpVVWjOA2WIl1O.AdE8NLiNka0R6t3Txxwdub6'),
('admin1', '$2y$10$Ow7VpO4dH.MqJ/9HvV9l0eISxEuUVrEnVsg3xmsNwHjabhgokmapC'),
('newmike', '$2y$10$wZ36J/p2GOzhKlaoF/coF.m.mECWPGtNtgPCc5RPvh/hofRKA7doy'),
('den2', '$2y$10$stQi69fPkQ1sRkkEBpHmkOBjlb7bfYuI2jvL0beOXsX878Elmr6U6'),
('a', '$2y$10$04hUu85tMa4GPFsVuig4f.q/MBqeulyi8BjfZmqpHUyn6.P8lXNHC'),
('s', '$2y$10$vFizZTb8bSisKNUyFU5i5OgTiJsn4yAdIATH3dNbcwYROrkpPsVNq');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
