USE [MyDB]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 3/26/2018 11:34:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 3/26/2018 11:34:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[State] [varchar](2) NULL,
	[City] [varchar](50) NOT NULL,
	[City_Inactive_Date] [varchar](10) NULL,
	[City_AddTime] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[City] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CJISOffenseCodes]    Script Date: 3/26/2018 11:34:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CJISOffenseCodes](
	[Offense Validation CD] [varchar](1) NULL,
	[Offense Code] [int] NOT NULL,
	[Offense Txn Type CD] [varchar](1) NULL,
	[Offense Statute] [varchar](15) NULL,
	[Offense Type of Statute CD] [varchar](2) NULL,
	[Statute Literal 25] [varchar](50) NULL,
	[Offense Default Type of Charge] [varchar](1) NULL,
	[Offense Type of Charge] [varchar](1) NULL,
	[Offense Literal Identifier CD] [varchar](5) NULL,
	[Offense Degree] [varchar](1) NULL,
	[BCS Hierarchy CD] [varchar](10) NULL,
	[Offense Enacted] [varchar](10) NULL,
	[Offense Repealed] [varchar](10) NULL,
	[ALPS Cognizant CD] [varchar](1) NULL,
	[Add_Date Time] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Offense Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[schools]    Script Date: 3/26/2018 11:34:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[schools](
	[CDS_Code] [varchar](50) NOT NULL,
	[Status] [varchar](50) NULL,
	[County] [varchar](50) NULL,
	[District] [varchar](50) NULL,
	[schoolname] [varchar](50) NULL,
	[sc_MailCity] [varchar](50) NULL,
	[sc_DOCType] [varchar](50) NULL,
	[Schooltype] [varchar](50) NULL,
	[sc_Latitude] [varchar](50) NULL,
	[sc_Longitude] [varchar](50) NULL,
PRIMARY KEY NONCLUSTERED 
(
	[CDS_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Stops]    Script Date: 3/26/2018 11:34:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stops](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Time] [datetime] NOT NULL,
	[JsonStop] [nvarchar](max) NULL,
	[JsonDojStop] [nvarchar](max) NULL,
	[PersonCount] [int] NOT NULL,
	[Latitude] [nvarchar](55) NULL,
	[Longitude] [nvarchar](55) NULL,
	[Beat] [nvarchar](10) NULL,
	[JsonInstrumentation] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[StatusMessage] [nvarchar](max) NULL,
	[UserProfileID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Stops] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile_Conf]    Script Date: 3/26/2018 11:34:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile_Conf](
	[ID] [int] IDENTITY(111101000,1) NOT NULL,
	[NTUserName] [nvarchar](max) NULL,
	[UserProfileID] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserProfile_Conf] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfiles]    Script Date: 3/26/2018 11:34:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfiles](
	[ID] [int] IDENTITY(111101000,1) NOT NULL,
	[Agency] [nvarchar](2) NULL,
	[ORI] [nvarchar](20) NULL,
	[Years] [int] NOT NULL,
	[Assignment] [nvarchar](255) NULL,
	[AssignmentOther] [nvarchar](max) NULL,
	[AssignmentKey] [int] NOT NULL,
 CONSTRAINT [PK_dbo.UserProfiles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Stops]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Stops_dbo.UserProfiles_UserProfileID] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[UserProfiles] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Stops] CHECK CONSTRAINT [FK_dbo.Stops_dbo.UserProfiles_UserProfileID]
GO
ALTER TABLE [dbo].[UserProfile_Conf]  WITH CHECK ADD  CONSTRAINT [FK_dbo.UserProfile_Conf_dbo.UserProfiles_UserProfileID] FOREIGN KEY([UserProfileID])
REFERENCES [dbo].[UserProfiles] ([ID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserProfile_Conf] CHECK CONSTRAINT [FK_dbo.UserProfile_Conf_dbo.UserProfiles_UserProfileID]
GO
