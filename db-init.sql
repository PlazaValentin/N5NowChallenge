USE [master]
GO

IF DB_ID('N5NowChallengeDb') IS NOT NULL
  SET NOEXEC ON

CREATE DATABASE N5NowChallengeDb
GO
USE N5NowChallengeDb
GO
CREATE TABLE [PermissionsType](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](300) NOT NULL,
)
GO
CREATE TABLE [Permissions](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	FirstNameEmployee [nvarchar](150) NOT NULL,
	LastNameEmployee [nvarchar](150) NOT NULL,
	[PermissionTypeId] [int] NOT NULL,
	DatePermission [datetime] NOT NULL,
	FOREIGN KEY ([PermissionTypeId]) REFERENCES [PermissionsType]([Id])
)
GO
INSERT INTO [PermissionsType] ([Description]) VALUES ('Admin')
INSERT INTO [PermissionsType] ([Description]) VALUES ('User')
INSERT INTO [PermissionsType] ([Description]) VALUES ('Sudo')
INSERT INTO [PermissionsType] ([Description]) VALUES ('Viewer')
INSERT INTO [PermissionsType] ([Description]) VALUES ('Manager')
INSERT INTO [PermissionsType] ([Description]) VALUES ('Guest')
INSERT INTO [PermissionsType] ([Description]) VALUES ('Editor')
INSERT INTO [PermissionsType] ([Description]) VALUES ('Analyst')
INSERT INTO [PermissionsType] ([Description]) VALUES ('Developer')
INSERT INTO [PermissionsType] ([Description]) VALUES ('Supervisor')