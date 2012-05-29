USE [master]
GO
/****** Object:  Database [PayMeBack]    Script Date: 05/30/2012 07:17:14 ******/
CREATE DATABASE [PayMeBack] ON  PRIMARY 
( NAME = N'PayMeBack', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\PayMeBack.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'PayMeBack_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\PayMeBack_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [PayMeBack] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PayMeBack].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PayMeBack] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [PayMeBack] SET ANSI_NULLS OFF
GO
ALTER DATABASE [PayMeBack] SET ANSI_PADDING OFF
GO
ALTER DATABASE [PayMeBack] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [PayMeBack] SET ARITHABORT OFF
GO
ALTER DATABASE [PayMeBack] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [PayMeBack] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [PayMeBack] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [PayMeBack] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [PayMeBack] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [PayMeBack] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [PayMeBack] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [PayMeBack] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [PayMeBack] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [PayMeBack] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [PayMeBack] SET  DISABLE_BROKER
GO
ALTER DATABASE [PayMeBack] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [PayMeBack] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [PayMeBack] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [PayMeBack] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [PayMeBack] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [PayMeBack] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [PayMeBack] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [PayMeBack] SET  READ_WRITE
GO
ALTER DATABASE [PayMeBack] SET RECOVERY FULL
GO
ALTER DATABASE [PayMeBack] SET  MULTI_USER
GO
ALTER DATABASE [PayMeBack] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [PayMeBack] SET DB_CHAINING OFF
GO
EXEC sys.sp_db_vardecimal_storage_format N'PayMeBack', N'ON'
GO
USE [PayMeBack]
GO
/****** Object:  User [NETWORK SERVICE]    Script Date: 05/30/2012 07:17:14 ******/
CREATE USER [NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Schema [Security]    Script Date: 05/30/2012 07:17:14 ******/
CREATE SCHEMA [Security] AUTHORIZATION [dbo]
GO
/****** Object:  Schema [Payment]    Script Date: 05/30/2012 07:17:14 ******/
CREATE SCHEMA [Payment] AUTHORIZATION [dbo]
GO
