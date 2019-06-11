-- Create the DB if it doesn't exist
Use master

if not exists(select * from sys.databases where name='BiblifunLogDB')
         create database BiblifunLogDB;
Go

-- now create the table if it doesn't exist
Use BiblifunLogDB;
Go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if not exists (select * from sys.tables t where t.name = 'Log') 

CREATE TABLE [Log](
         [Id] [int] IDENTITY(1,1) NOT NULL,
         [Message] nvarchar(max) NULL,
         [MessageTemplate] nvarchar(max) NULL,
         [Level] nvarchar(128) NULL,
         [TimeStamp] datetime2(4) NOT NULL,
         [Exception] nvarchar(max) NULL,
         [Properties] xml NULL,
		 [LogEvent] nvarchar(max)
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED
(
         [Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]

GO