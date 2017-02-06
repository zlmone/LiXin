IF OBJECT_ID ('dbo.Free_UserOtherApply') IS NOT NULL
	DROP TABLE dbo.Free_UserOtherApply
GO

CREATE TABLE dbo.Free_UserOtherApply
	(
	ID               INT IDENTITY NOT NULL,
	ApplyID          INT NULL,
	ApplyName        NVARCHAR (200) NULL,
	Year             INT NULL,
	ConvertScore     DECIMAL (18,2) NULL,
	Memo             NVARCHAR (200) NULL,
	ApplyTime        DATETIME NULL,
	ApplyUserID      INT NULL,
	ApplyType        INT NULL,
	Status           INT NULL,
	ApproveStatus    INT NULL,
	DepApproveStatus INT NULL,
	IsDelete         INT NULL,
	tScore           DECIMAL (18,2) NULL,
	CPAScore         DECIMAL (18,2) NULL,
	GettScore        DECIMAL (18,2) NULL,
	GetCPAScore      DECIMAL (18,2) NULL,
	DepTrainReason   NVARCHAR (500) NULL,
	DepReason        NVARCHAR (500) NULL,
	typeForm         INT NULL,
	CreateUserID     INT NULL,
	CreateTime       DATETIME NULL,
	fromID           INT NULL,
	CONSTRAINT PK__Free_Use__3214EC276F1576F7 PRIMARY KEY (ID)
	)
GO

IF OBJECT_ID ('dbo.Free_UserApplyTeacherDetails') IS NOT NULL
	DROP TABLE dbo.Free_UserApplyTeacherDetails
GO

CREATE TABLE dbo.Free_UserApplyTeacherDetails
	(
	ID           INT IDENTITY NOT NULL,
	userApplyID  INT NULL,
	ClassName    NVARCHAR (200) NULL,
	ConvertScore DECIMAL (18,2) NULL,
	IsDelete     INT NULL,
	tScore       DECIMAL (18,2) NULL,
	CPAScore     DECIMAL (18,2) NULL,
	CONSTRAINT PK__Free_Use__3214EC2772E607DB PRIMARY KEY (ID)
	)
GO

IF OBJECT_ID ('dbo.Free_UserApplyOtherForm') IS NOT NULL
	DROP TABLE dbo.Free_UserApplyOtherForm
GO

CREATE TABLE dbo.Free_UserApplyOtherForm
	(
	ID               INT IDENTITY NOT NULL,
	OtherFromID      INT NULL,
	Year             INT NULL,
	CourseName       NVARCHAR (200) NULL,
	Address          NVARCHAR (200) NULL,
	StartTime        DATETIME NULL,
	EndTime          DATETIME NULL,
	TogetherScore    DECIMAL (18,2) NULL,
	CPAScore         DECIMAL (18,2) NULL,
	ApplyDateTime    DATETIME NULL,
	ApplyUserID      INT NULL,
	Status           INT NULL,
	ApproveStatus    INT NULL,
	DepApproveStatus INT NULL,
	IsDelete         INT NULL,
	GettScore        DECIMAL (18,2) NULL,
	GetCPAScore      DECIMAL (18,2) NULL,
	DepTrainReason   NVARCHAR (500) NULL,
	DepReason        NVARCHAR (500) NULL,
	CONSTRAINT PK__Free_Use__3214EC277E57BA87 PRIMARY KEY (ID)
	)
GO

IF OBJECT_ID ('dbo.Free_UserApplyFiles') IS NOT NULL
	DROP TABLE dbo.Free_UserApplyFiles
GO

CREATE TABLE dbo.Free_UserApplyFiles
	(
	ID           INT IDENTITY NOT NULL,
	userApplyID  INT NULL,
	ResourceName NVARCHAR (500) NULL,
	RealName     NVARCHAR (500) NULL,
	IsDelete     INT NULL,
	ConvertName  NVARCHAR (500) NULL,
	type         INT NULL,
	CONSTRAINT PK__Free_Use__3214EC2776B698BF PRIMARY KEY (ID)
	)
GO

IF OBJECT_ID ('dbo.Free_OtherFroms') IS NOT NULL
	DROP TABLE dbo.Free_OtherFroms
GO

CREATE TABLE dbo.Free_OtherFroms
	(
	Id       INT IDENTITY NOT NULL,
	Year     INT NOT NULL,
	FromName NVARCHAR (500) NOT NULL,
	FromType INT NOT NULL,
	FromTime DATETIME NOT NULL,
	IsDelete INT NOT NULL,
	CONSTRAINT PK_Free_OtherFroms PRIMARY KEY (Id)
	)
GO

IF OBJECT_ID ('dbo.Free_OtherApplyConfig') IS NOT NULL
	DROP TABLE dbo.Free_OtherApplyConfig
GO

CREATE TABLE dbo.Free_OtherApplyConfig
	(
	ID              INT IDENTITY NOT NULL,
	ApplyContent    NVARCHAR (200) NULL,
	ConvertType     INT NULL,
	ConvertBase     DECIMAL (18,2) NULL,
	ConvertBaseInfo NVARCHAR (50) NULL,
	ConvertBaseTo   DECIMAL (18,2) NULL,
	UploadTip       NVARCHAR (200) NULL,
	ConvertMax      DECIMAL (18,2) NULL,
	TrainGradeScore NVARCHAR (500) NULL,
	Memo            NVARCHAR (200) NULL,
	year            INT NULL,
	CreateTime      DATETIME NULL,
	IsDelete        INT NULL,
	ApplyType       INT NULL,
	MemoTip NVARCHAR(200)
	CONSTRAINT PK__Free_Oth__3214EC27658C0CBD PRIMARY KEY (ID)
	)
GO

IF OBJECT_ID ('dbo.Free_DepApprove') IS NOT NULL
	DROP TABLE dbo.Free_DepApprove
GO

CREATE TABLE dbo.Free_DepApprove
	(
	ID            INT IDENTITY NOT NULL,
	UserID        INT NULL,
	CreateUserID  INT NULL,
	CreateTime    DATETIME NULL,
	ManageDeptIDs NVARCHAR (500) NULL,
	LeardID       NVARCHAR (50) NULL,
	CONSTRAINT PK__Free_Dep__3214EC27031C6FA4 PRIMARY KEY (ID)
	)
GO

IF OBJECT_ID ('dbo.Free_Apporve') IS NOT NULL
	DROP TABLE dbo.Free_Apporve
GO

CREATE TABLE dbo.Free_Apporve
	(
	ID            INT IDENTITY NOT NULL,
	userApplyID   INT NULL,
	DepApprove    INT NULL,
	DepReason     NVARCHAR (500) NULL,
	CheackBatch   INT NULL,
	type          INT NULL,
	ApproveUserID INT NULL,
	ApproveTime   DATETIME NULL,
	ApproveType   INT NULL,
	LookFile      INT NULL,
	CONSTRAINT PK__Free_App__3214EC277A8729A3 PRIMARY KEY (ID)
	)
GO

IF OBJECT_ID ('dbo.Free_ApplyConfig') IS NOT NULL
	DROP TABLE dbo.Free_ApplyConfig
GO

CREATE TABLE dbo.Free_ApplyConfig
	(
	ID                INT IDENTITY NOT NULL,
	FreeName          NVARCHAR (200) NULL,
	UploadTip         NVARCHAR (200) NULL,
	ApplyType         INT NULL,
	CPAFreeScore      DECIMAL (18,2) NULL,
	TogetherFreeScore DECIMAL (18,2) NULL,
	Memo              NVARCHAR (200) NULL,
	year              INT NULL,
	CreateTime        DATETIME NULL,
	IsDelete          INT NULL,
	MemoTip           NVARCHAR(200)
	CONSTRAINT PK__Free_App__3214EC275DEAEAF5 PRIMARY KEY (ID)
	)
GO

IF OBJECT_ID ('dbo.Free_AllOtherApply') IS NOT NULL
	DROP TABLE dbo.Free_AllOtherApply
GO

CREATE TABLE dbo.Free_AllOtherApply
	(
	ID          INT IDENTITY NOT NULL,
	userapplyID INT NULL,
	userIDs     NVARCHAR (max) NULL,
	CONSTRAINT PK__Free_All__3214EC27116A8EFB PRIMARY KEY (ID)
	)
GO

