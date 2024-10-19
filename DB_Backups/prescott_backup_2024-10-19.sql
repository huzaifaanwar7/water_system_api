-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Oct 19, 2024 at 01:16 PM
-- Server version: 10.4.21-MariaDB
-- PHP Version: 8.1.0

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `Prescott`
--

-- --------------------------------------------------------

--
-- Table structure for table `amenity`
--

CREATE TABLE `amenity` (
  `Id` int(11) NOT NULL,
  `BuildingId` int(11) NOT NULL,
  `AmenityName` varchar(255) NOT NULL,
  `Description` text DEFAULT NULL,
  `CreatedBy` varchar(255) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedBy` varchar(255) DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `amenity`
--

INSERT INTO `amenity` (`Id`, `BuildingId`, `AmenityName`, `Description`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `IsDeleted`) VALUES
(1, 2, 'Pool', '13th Floor, Infinity Pool', '5c94965a-611c-45f6-9ef9-a4b8f0ddddc6', '2024-10-19 13:50:43', NULL, NULL, 0),
(3, 1, 'Cinema', '3rd Floor, Kids Cinema', '5c94965a-611c-45f6-9ef9-a4b8f0ddddc6', '2024-10-19 14:04:37', NULL, NULL, 0);

-- --------------------------------------------------------

--
-- Table structure for table `amenity_images`
--

CREATE TABLE `amenity_images` (
  `Id` int(11) NOT NULL,
  `FileName` varchar(255) NOT NULL,
  `FilePath` varchar(255) NOT NULL,
  `FileType` varchar(255) DEFAULT NULL,
  `AmenityId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `amenity_images`
--

INSERT INTO `amenity_images` (`Id`, `FileName`, `FilePath`, `FileType`, `AmenityId`) VALUES
(1, 'RARE JVC_Ext-Balcony_Birdview_02_Final.jpg', '/files/upload/RARE JVC_Ext-Balcony_Birdview_02_Final.jpg', 'image/jpeg', 1),
(2, 'RARE JVC_Ext-Balcony_Birdview_02_Final.jpg', '/files/upload/RARE JVC_Ext-Balcony_Birdview_02_Final.jpg', 'image/jpeg', 2),
(3, 'RARE JVC_Ext-Balcony_Birdview_02_Final.jpg', '/files/upload/RARE JVC_Ext-Balcony_Birdview_02_Final.jpg', 'image/jpeg', 3);

-- --------------------------------------------------------

--
-- Table structure for table `announcements`
--

CREATE TABLE `announcements` (
  `Id` int(11) NOT NULL,
  `BuildingId` int(11) NOT NULL,
  `Title` varchar(255) NOT NULL,
  `Content` text DEFAULT NULL,
  `CreatedBy` varchar(255) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedBy` varchar(255) DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `announcements`
--

INSERT INTO `announcements` (`Id`, `BuildingId`, `Title`, `Content`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `IsDeleted`) VALUES
(2, 1, 'Roof Top Pool', 'Roof Top Pool is not working today.', '5c94965a-611c-45f6-9ef9-a4b8f0ddddc6', '2024-10-15 11:36:01', NULL, '2024-10-18 17:53:09', 0),
(3, 2, 'Cinema', 'Cinema will remain closed today for maintenance.', '5c94965a-611c-45f6-9ef9-a4b8f0ddddc6', '2024-10-15 11:45:52', NULL, '2024-10-18 17:47:53', 0);

-- --------------------------------------------------------

--
-- Table structure for table `announcement_images`
--

CREATE TABLE `announcement_images` (
  `Id` int(11) NOT NULL,
  `FileName` varchar(255) NOT NULL,
  `FilePath` varchar(255) NOT NULL,
  `FileType` varchar(255) DEFAULT NULL,
  `AnnouncementId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `announcement_images`
--

INSERT INTO `announcement_images` (`Id`, `FileName`, `FilePath`, `FileType`, `AnnouncementId`) VALUES
(5, '3.jpeg', '/files/upload/3.jpeg', 'image/jpeg', 3),
(6, '3.jpeg', '/files/upload/3.jpeg', 'image/jpeg', 2);

-- --------------------------------------------------------

--
-- Table structure for table `building`
--

CREATE TABLE `building` (
  `Id` int(11) NOT NULL,
  `BuildingName` varchar(255) NOT NULL,
  `BuildingDescription` text DEFAULT NULL,
  `Address` text DEFAULT NULL,
  `CreatedBy` varchar(255) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedBy` varchar(255) DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `building`
--

INSERT INTO `building` (`Id`, `BuildingName`, `BuildingDescription`, `Address`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `IsDeleted`) VALUES
(1, 'Fairways', NULL, NULL, 'a3745ae7-9722-449e-82f8-8fe5e00bcbd9', '2024-10-10 09:40:30', NULL, NULL, 0),
(2, 'Legado', NULL, NULL, 'a3745ae7-9722-449e-82f8-8fe5e00bcbd9', '2024-10-10 09:41:10', NULL, NULL, 0);

-- --------------------------------------------------------

--
-- Table structure for table `dropdownlistchild`
--

CREATE TABLE `dropdownlistchild` (
  `Id` int(11) NOT NULL,
  `ParentId` int(11) NOT NULL,
  `Label` varchar(255) NOT NULL,
  `Value` varchar(255) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `dropdownlistchild`
--

INSERT INTO `dropdownlistchild` (`Id`, `ParentId`, `Label`, `Value`, `CreatedAt`, `IsDeleted`) VALUES
(1, 1, 'Finance', '', '2024-07-20 15:26:22', 0),
(2, 1, 'Real Estate', '', '2024-07-20 15:26:22', 0),
(3, 1, 'Fashion', '', '2024-07-20 15:26:22', 0),
(4, 1, 'Warehouse', '', '2024-07-20 15:26:22', 0);

-- --------------------------------------------------------

--
-- Table structure for table `dropdownlistparent`
--

CREATE TABLE `dropdownlistparent` (
  `Id` int(11) NOT NULL,
  `Type` varchar(255) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `dropdownlistparent`
--

INSERT INTO `dropdownlistparent` (`Id`, `Type`, `CreatedAt`, `IsDeleted`) VALUES
(1, 'BusinessType', '2024-07-20 15:23:48', 0);

-- --------------------------------------------------------

--
-- Table structure for table `reservation`
--

CREATE TABLE `reservation` (
  `Id` int(11) NOT NULL,
  `BuildingId` int(11) NOT NULL,
  `AmenityId` int(11) NOT NULL,
  `Reason` text DEFAULT NULL,
  `FromDate` datetime NOT NULL,
  `ToDate` datetime NOT NULL,
  `CreatedBy` varchar(255) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedBy` varchar(255) DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `roles`
--

CREATE TABLE `roles` (
  `Id` varchar(255) NOT NULL,
  `RoleName` varchar(255) NOT NULL,
  `RoleDescription` text DEFAULT NULL,
  `CreatedBy` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedBy` varchar(255) DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `roles`
--

INSERT INTO `roles` (`Id`, `RoleName`, `RoleDescription`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `IsDeleted`) VALUES
('361fa6a4-abe4-459e-8596-c8409458bd1e', 'Admin', 'Admin', '', '2024-06-14 18:21:53', NULL, NULL, 0),
('6a844134-717f-4ec7-b0c0-6c77fb84b594', 'Tenant', 'Tenant', '', '2024-06-14 18:22:15', NULL, NULL, 0),
('7bbb8f39-853d-419c-9821-4a9df220a805', 'SuperAdmin', 'Super Admin', '', '2024-06-14 18:21:30', NULL, NULL, 0);

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `Id` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `FirstName` varchar(255) NOT NULL,
  `LastName` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `EmailVerified` tinyint(1) NOT NULL DEFAULT 0,
  `Password` varchar(255) NOT NULL,
  `Mobile` varchar(20) DEFAULT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `PhotoURL` varchar(255) DEFAULT NULL,
  `Address` text DEFAULT NULL,
  `BuildingId` int(11) NOT NULL,
  `FirebaseId` varchar(255) NOT NULL,
  `CreatedBy` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedBy` varchar(255) DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0,
  `IsActive` tinyint(1) NOT NULL DEFAULT 1,
  `UserSignUpType` varchar(25) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `users`
--

INSERT INTO `users` (`Id`, `RoleId`, `FirstName`, `LastName`, `Email`, `EmailVerified`, `Password`, `Mobile`, `Phone`, `PhotoURL`, `Address`, `BuildingId`, `FirebaseId`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `IsDeleted`, `IsActive`, `UserSignUpType`) VALUES
('01f16b3d-0a33-4123-aec5-5a807fc2199d', '6a844134-717f-4ec7-b0c0-6c77fb84b594', 'Owais', '', 'abcd456@gmail.com', 1, 'Ov@1234', NULL, NULL, 'https://lh3.googleusercontent.com/a/ACg8ocKgaPEVSsDCqYBLlZTiLdJtUvLdkacXoSDp2BqlME7mcF_oRA=s96-c', NULL, 1, '', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-11 16:04:08', NULL, NULL, 0, 1, 'Email'),
('14ed06dc-dc49-49bf-81f0-51a66c95c2d4', ' ', 'Test', '', 'asdf@gmail.com', 1, 'Test', NULL, NULL, '', NULL, 2, '', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-15 13:11:45', NULL, NULL, 0, 1, 'Email'),
('1f452e17-e1d4-4f44-8bf4-55d1ac1251a4', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Mudassir', '', 'abc@gmail.com', 1, 'Ov@1234', NULL, NULL, NULL, NULL, 0, 'ANSifnpWpkSzyOocAIqSM6v9b103', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-10 11:50:45', NULL, NULL, 0, 1, 'Email'),
('2bdeb35f-e438-4af1-a625-d3fa495498a1', '', 'Owais', '', 'xyz@gmail.com', 1, 'qwerty', NULL, NULL, '', NULL, 0, '7jPBHYa9jIfjXPUqVFHHsGRFHXF2', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-11 11:49:32', NULL, NULL, 0, 1, 'Email'),
('46e5a2af-6a9a-40eb-818d-a7dbfb235bac', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Mudassir', '', 'abc@gmail.com', 1, 'Ov@1234', NULL, NULL, NULL, NULL, 0, 'ANSifnpWpkSzyOocAIqSM6v9b103', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-10 11:24:00', NULL, NULL, 0, 1, 'Email'),
('5c94965a-611c-45f6-9ef9-a4b8f0ddddc6', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Suffi Ullah', '', 'm.suffiullah16@gmail.com', 1, 'Suffi@123', NULL, NULL, NULL, NULL, 0, '3rXXKfWHCxT56TOjp8TGX8dXDJ33', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-07-08 17:11:39', NULL, NULL, 0, 1, 'Email'),
('68eafacb-1f21-4076-9690-f945db98c0b0', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'sohail khan', '', 'suhelk.ae@gmail.com', 1, '', NULL, NULL, NULL, NULL, 0, 'JbvO5guHltdsQFQAup0bV2XlQ5y2', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-07-14 19:01:49', NULL, '2024-07-14 19:03:16', 0, 1, 'Google'),
('698eded4-e565-4b87-a36f-5969973d4e7e', '6a844134-717f-4ec7-b0c0-6c77fb84b594', 'Mudassir Mukhtar', '', 'mudassirmukhtar4@gmail.com', 1, '', NULL, NULL, 'https://lh3.googleusercontent.com/a/ACg8ocJgtjTnAbGNtMuQxpkkrf0LmbHJyr3u03rNXEO0TJpXawkyXU09=s96-c', NULL, 0, '', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-14 13:28:56', NULL, NULL, 0, 1, 'Google'),
('a46eec46-5645-4fc5-babf-2f15b1b1488c', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Mudassir', '', 'abc@gmail.com', 1, 'Ov@1234', NULL, NULL, NULL, NULL, 0, 'ANSifnpWpkSzyOocAIqSM6v9b103', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-10 11:23:47', NULL, NULL, 0, 1, 'Email'),
('abe32b6f-8f6f-4a22-8ffb-68dd546ad6dc', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Mudassir', '', 'abc@gmail.com', 1, 'Ov@1234', NULL, NULL, NULL, NULL, 0, 'ANSifnpWpkSzyOocAIqSM6v9b103', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-10 11:50:50', NULL, NULL, 0, 1, 'Email'),
('ae2ac56c-89dc-46dc-930e-6045d66318cb', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Mudassir', '', 'abc@gmail.com', 1, 'Ov@1234', NULL, NULL, NULL, NULL, 0, 'ANSifnpWpkSzyOocAIqSM6v9b103', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-10 11:23:53', NULL, NULL, 0, 1, 'Email'),
('b79db6e3-db3a-4a17-b3a1-02c135fbd0d3', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Mudassir', '', 'abc@gmail.com', 1, 'Ov@1234', NULL, NULL, NULL, NULL, 0, 'ANSifnpWpkSzyOocAIqSM6v9b103', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-10 11:23:35', NULL, NULL, 0, 1, 'Email'),
('bff21f5a-d66e-4273-b9a7-194d237af174', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Mudassir', '', 'abc@gmail.com', 1, 'Ov@1234', NULL, NULL, NULL, NULL, 0, 'ANSifnpWpkSzyOocAIqSM6v9b103', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-10 11:49:32', NULL, NULL, 0, 1, 'Email'),
('e0f9d7f1-ba08-4dba-a79d-adcf25bca73b', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Mudassir', '', 'abc@gmail.com', 1, 'Ov@1234', NULL, NULL, NULL, NULL, 0, 'XkDfVE0qhebVfkpcOb3ZN7Keo1u2', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-10 11:20:44', NULL, NULL, 0, 1, 'Email'),
('f06a67c1-0af5-4b7a-a7e7-32d08eab6d77', '6a844134-717f-4ec7-b0c0-6c77fb84b594', 'Suffi', '', 'suffi8606@gmail.com', 1, '', NULL, NULL, 'https://lh3.googleusercontent.com/a/ACg8ocKgaPEVSsDCqYBLlZTiLdJtUvLdkacXoSDp2BqlME7mcF_oRA=s96-c', NULL, 0, '', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-11 12:11:24', NULL, NULL, 0, 1, 'Google'),
('fc99d3ae-8eff-4397-a203-667205a7ae61', '', 'Owais', '', 'ov@gmail.com', 1, 'Ov@1234', NULL, NULL, '', NULL, 0, 'SIxRB1vR3AMM2j5Ft2J5QuLakP52', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-10-11 11:46:25', NULL, NULL, 0, 1, 'Email');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `amenity`
--
ALTER TABLE `amenity`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `amenity_images`
--
ALTER TABLE `amenity_images`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `announcements`
--
ALTER TABLE `announcements`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `announcement_images`
--
ALTER TABLE `announcement_images`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `building`
--
ALTER TABLE `building`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `dropdownlistchild`
--
ALTER TABLE `dropdownlistchild`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ParentId` (`ParentId`);

--
-- Indexes for table `dropdownlistparent`
--
ALTER TABLE `dropdownlistparent`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `reservation`
--
ALTER TABLE `reservation`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `amenity`
--
ALTER TABLE `amenity`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `amenity_images`
--
ALTER TABLE `amenity_images`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `announcements`
--
ALTER TABLE `announcements`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `announcement_images`
--
ALTER TABLE `announcement_images`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `building`
--
ALTER TABLE `building`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `dropdownlistchild`
--
ALTER TABLE `dropdownlistchild`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `dropdownlistparent`
--
ALTER TABLE `dropdownlistparent`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT for table `reservation`
--
ALTER TABLE `reservation`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `dropdownlistchild`
--
ALTER TABLE `dropdownlistchild`
  ADD CONSTRAINT `dropdownlistchild_ibfk_1` FOREIGN KEY (`ParentId`) REFERENCES `dropdownlistparent` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
