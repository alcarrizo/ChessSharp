-- phpMyAdmin SQL Dump
-- version 4.9.4
-- https://www.phpmyadmin.net/
--
-- Host: localhost:3306
-- Generation Time: Mar 05, 2020 at 05:31 PM
-- Server version: 10.3.16-MariaDB
-- PHP Version: 7.3.12

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
  `username` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `gameId` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `joinedUser` varchar(255) COLLATE utf8_unicode_ci NOT NULL,
  `playerCount` int(50) NOT NULL
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
('admin1', '$2y$10$Ow7VpO4dH.MqJ/9HvV9l0eISxEuUVrEnVsg3xmsNwHjabhgokmapC');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
