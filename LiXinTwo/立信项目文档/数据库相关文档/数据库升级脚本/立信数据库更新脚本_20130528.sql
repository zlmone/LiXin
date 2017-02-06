
---���������ű�--2013-5-28--
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sys_NoteResource](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NoteId] [int] NULL,
	[RealName] [nvarchar](100) NULL,
	[FileName] [nvarchar](100) NULL,
	[CreateDate] [datetime] NULL,
	[IsDelete] [int] NULL,
 CONSTRAINT [PK_SYS_NOTERESOURCE] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


