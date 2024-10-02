-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: Oct 02, 2024 at 10:55 AM
-- Server version: 10.4.21-MariaDB-log
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
-- Table structure for table `DropdownListChild`
--

CREATE TABLE `DropdownListChild` (
  `Id` int(11) NOT NULL,
  `ParentId` int(11) NOT NULL,
  `Label` varchar(255) NOT NULL,
  `Value` varchar(255) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `DropdownListChild`
--

INSERT INTO `DropdownListChild` (`Id`, `ParentId`, `Label`, `Value`, `CreatedAt`, `IsDeleted`) VALUES
(1, 1, 'Finance', '', '2024-07-20 15:26:22', 0),
(2, 1, 'Real Estate', '', '2024-07-20 15:26:22', 0),
(3, 1, 'Fashion', '', '2024-07-20 15:26:22', 0),
(4, 1, 'Warehouse', '', '2024-07-20 15:26:22', 0);

-- --------------------------------------------------------

--
-- Table structure for table `DropdownListParent`
--

CREATE TABLE `DropdownListParent` (
  `Id` int(11) NOT NULL,
  `Type` varchar(255) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `DropdownListParent`
--

INSERT INTO `DropdownListParent` (`Id`, `Type`, `CreatedAt`, `IsDeleted`) VALUES
(1, 'BusinessType', '2024-07-20 15:23:48', 0);

-- --------------------------------------------------------

--
-- Table structure for table `Roles`
--

CREATE TABLE `Roles` (
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
-- Dumping data for table `Roles`
--

INSERT INTO `Roles` (`Id`, `RoleName`, `RoleDescription`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `IsDeleted`) VALUES
('361fa6a4-abe4-459e-8596-c8409458bd1e', 'Admin', 'Admin', '', '2024-06-14 18:21:53', NULL, NULL, 0),
('6a844134-717f-4ec7-b0c0-6c77fb84b594', 'Accountant', 'Accountant', '', '2024-06-14 18:22:15', NULL, NULL, 0),
('7bbb8f39-853d-419c-9821-4a9df220a805', 'SuperAdmin', 'Super Admin', '', '2024-06-14 18:21:30', NULL, NULL, 0);

-- --------------------------------------------------------

--
-- Table structure for table `Users`
--

CREATE TABLE `Users` (
  `Id` varchar(255) NOT NULL,
  `RoleId` varchar(255) NOT NULL,
  `FirstName` varchar(255) NOT NULL,
  `LastName` varchar(255) NOT NULL,
  `Email` varchar(255) NOT NULL,
  `EmailVerified` tinyint(1) NOT NULL DEFAULT 0,
  `Password` varchar(255) NOT NULL,
  `Mobile` varchar(20) DEFAULT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `Address` text DEFAULT NULL,
  `FirebaseId` varchar(255) NOT NULL,
  `CreatedBy` varchar(255) DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedBy` varchar(255) DEFAULT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT 0,
  `IsActive` tinyint(1) NOT NULL DEFAULT 1,
  `UserSignUpType` varchar(25) NOT NULL,
  `BusinessName` varchar(255) DEFAULT NULL,
  `BusinessType` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `Users`
--

INSERT INTO `Users` (`Id`, `RoleId`, `FirstName`, `LastName`, `Email`, `EmailVerified`, `Password`, `Mobile`, `Phone`, `Address`, `FirebaseId`, `CreatedBy`, `CreatedAt`, `UpdatedBy`, `UpdatedAt`, `IsDeleted`, `IsActive`, `UserSignUpType`, `BusinessName`, `BusinessType`) VALUES
('5c94965a-611c-45f6-9ef9-a4b8f0ddddc6', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Suffi Ullah', '', 'm.suffiullah16@gmail.com', 1, 'Suffi@123', NULL, NULL, NULL, '3rXXKfWHCxT56TOjp8TGX8dXDJ33', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-07-08 17:11:39', NULL, NULL, 0, 1, 'Email', 'eBook.com', '1'),
('68eafacb-1f21-4076-9690-f945db98c0b0', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'sohail khan', '', 'suhelk.ae@gmail.com', 1, '', NULL, NULL, NULL, 'JbvO5guHltdsQFQAup0bV2XlQ5y2', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-07-14 19:01:49', NULL, '2024-07-14 19:03:16', 0, 1, 'Google', 'skhan', '1'),
('a3745ae7-9722-449e-82f8-8fe5e00bcbd9', '361fa6a4-abe4-459e-8596-c8409458bd1e', 'Suffi', '', 'suffi8606@gmail.com', 1, '', '971545618115', '971554962082', '309 3rd floor Deyaar Head Office Building Al Barsha', 'JOZ2mPHabINBVVmvMqU4rbjWKDS2', '7bbb8f39-853d-419c-9821-4a9df220a805', '2024-07-20 13:22:32', 'a3745ae7-9722-449e-82f8-8fe5e00bcbd9', '2024-07-20 13:23:06', 0, 1, 'Google', 'Leads Load', '1');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `DropdownListChild`
--
ALTER TABLE `DropdownListChild`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `ParentId` (`ParentId`);

--
-- Indexes for table `DropdownListParent`
--
ALTER TABLE `DropdownListParent`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `Roles`
--
ALTER TABLE `Roles`
  ADD PRIMARY KEY (`Id`);

--
-- Indexes for table `Users`
--
ALTER TABLE `Users`
  ADD PRIMARY KEY (`Id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `DropdownListChild`
--
ALTER TABLE `DropdownListChild`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `DropdownListParent`
--
ALTER TABLE `DropdownListParent`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `DropdownListChild`
--
ALTER TABLE `DropdownListChild`
  ADD CONSTRAINT `dropdownlistchild_ibfk_1` FOREIGN KEY (`ParentId`) REFERENCES `DropdownListParent` (`Id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
