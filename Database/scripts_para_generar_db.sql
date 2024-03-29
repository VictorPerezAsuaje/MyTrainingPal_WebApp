USE [master]
GO
/****** Object:  Database [MyTrainingPalDb]    Script Date: 06/11/2022 9:09:30 ******/
CREATE DATABASE [MyTrainingPalDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MyTrainingPalDb', FILENAME = N'C:\Users\Victor\MyTrainingPalDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MyTrainingPalDb_log', FILENAME = N'C:\Users\Victor\MyTrainingPalDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [MyTrainingPalDb] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyTrainingPalDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyTrainingPalDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [MyTrainingPalDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MyTrainingPalDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MyTrainingPalDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [MyTrainingPalDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MyTrainingPalDb] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [MyTrainingPalDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [MyTrainingPalDb] SET  MULTI_USER 
GO
ALTER DATABASE [MyTrainingPalDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MyTrainingPalDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MyTrainingPalDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MyTrainingPalDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MyTrainingPalDb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MyTrainingPalDb] SET QUERY_STORE = OFF
GO
USE [MyTrainingPalDb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
USE [MyTrainingPalDb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 06/11/2022 9:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Exercises]    Script Date: 06/11/2022 9:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Exercises](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Level] [int] NOT NULL,
	[ForceType] [int] NOT NULL,
	[RequiresEquipment] [bit] NOT NULL,
	[VideoUrl] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Exercises] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExercisesMuscleGroups]    Script Date: 06/11/2022 9:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExercisesMuscleGroups](
	[ExerciseId] [int] NOT NULL,
	[MuscleGroupId] [int] NOT NULL,
 CONSTRAINT [PK_ExercisesMuscleGroups] PRIMARY KEY CLUSTERED 
(
	[ExerciseId] ASC,
	[MuscleGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MuscleGroups]    Script Date: 06/11/2022 9:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MuscleGroups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_MuscleGroups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sets]    Script Date: 06/11/2022 9:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sets](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[WorkoutId] [int] NOT NULL,
	[ExerciseId] [int] NOT NULL,
	[SetType] [int] NOT NULL,
	[Repetitions] [int] NULL,
	[Time] [datetime] NULL,
 CONSTRAINT [PK_Set] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 06/11/2022 9:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[LastName] [varchar](150) NOT NULL,
	[Email] [varchar](150) NOT NULL,
	[Password] [nchar](20) NOT NULL,
	[IsPremium] [bit] NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserWorkoutHistory]    Script Date: 06/11/2022 9:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserWorkoutHistory](
	[WorkoutId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[CompletionDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Workout]    Script Date: 06/11/2022 9:09:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Workout](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[NumberOfSets] [int] NOT NULL,
	[WorkoutType] [int] NOT NULL,
	[UserId] [int] NULL,
 CONSTRAINT [PK_Workout] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_ExercisesMuscleGroups_MuscleGroupId]    Script Date: 06/11/2022 9:09:30 ******/
CREATE NONCLUSTERED INDEX [IX_ExercisesMuscleGroups_MuscleGroupId] ON [dbo].[ExercisesMuscleGroups]
(
	[MuscleGroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Exercises] ADD  CONSTRAINT [DF__Exercises__Level__36B12243]  DEFAULT ((0)) FOR [Level]
GO
ALTER TABLE [dbo].[Exercises] ADD  CONSTRAINT [DF__Exercises__Force__35BCFE0A]  DEFAULT ((0)) FOR [ForceType]
GO
ALTER TABLE [dbo].[Exercises] ADD  CONSTRAINT [DF__Exercises__Requi__37A5467C]  DEFAULT (CONVERT([bit],(0))) FOR [RequiresEquipment]
GO
ALTER TABLE [dbo].[ExercisesMuscleGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExercisesMuscleGroups_Exercises_ExerciseId] FOREIGN KEY([ExerciseId])
REFERENCES [dbo].[Exercises] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExercisesMuscleGroups] CHECK CONSTRAINT [FK_ExercisesMuscleGroups_Exercises_ExerciseId]
GO
ALTER TABLE [dbo].[ExercisesMuscleGroups]  WITH CHECK ADD  CONSTRAINT [FK_ExercisesMuscleGroups_MuscleGroups_MuscleGroupId] FOREIGN KEY([MuscleGroupId])
REFERENCES [dbo].[MuscleGroups] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ExercisesMuscleGroups] CHECK CONSTRAINT [FK_ExercisesMuscleGroups_MuscleGroups_MuscleGroupId]
GO
ALTER TABLE [dbo].[Sets]  WITH CHECK ADD  CONSTRAINT [FK_Set_Exercises] FOREIGN KEY([ExerciseId])
REFERENCES [dbo].[Exercises] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sets] CHECK CONSTRAINT [FK_Set_Exercises]
GO
ALTER TABLE [dbo].[Sets]  WITH CHECK ADD  CONSTRAINT [FK_Sets_Workout] FOREIGN KEY([WorkoutId])
REFERENCES [dbo].[Workout] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Sets] CHECK CONSTRAINT [FK_Sets_Workout]
GO
ALTER TABLE [dbo].[UserWorkoutHistory]  WITH CHECK ADD  CONSTRAINT [FK_UserWorkoutHistory_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserWorkoutHistory] CHECK CONSTRAINT [FK_UserWorkoutHistory_User]
GO
ALTER TABLE [dbo].[UserWorkoutHistory]  WITH CHECK ADD  CONSTRAINT [FK_UserWorkoutHistory_Workout] FOREIGN KEY([WorkoutId])
REFERENCES [dbo].[Workout] ([Id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserWorkoutHistory] CHECK CONSTRAINT [FK_UserWorkoutHistory_Workout]
GO
USE [master]
GO
ALTER DATABASE [MyTrainingPalDb] SET  READ_WRITE 
GO
