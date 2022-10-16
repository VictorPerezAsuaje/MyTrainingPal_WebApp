USE [MyTrainingPalDb]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 29/09/2022 15:10:29 ******/
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
/****** Object:  Table [dbo].[Exercises]    Script Date: 29/09/2022 15:10:29 ******/
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
 CONSTRAINT [PK_Exercises] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExercisesMuscleGroups]    Script Date: 29/09/2022 15:10:29 ******/
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
/****** Object:  Table [dbo].[MuscleGroups]    Script Date: 29/09/2022 15:10:29 ******/
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
/****** Object:  Table [dbo].[Sets]    Script Date: 29/09/2022 15:10:29 ******/
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
/****** Object:  Table [dbo].[Users]    Script Date: 29/09/2022 15:10:29 ******/
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
/****** Object:  Table [dbo].[UserWorkoutHistory]    Script Date: 29/09/2022 15:10:29 ******/
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
/****** Object:  Table [dbo].[Workout]    Script Date: 29/09/2022 15:10:29 ******/
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
