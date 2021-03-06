USE [Lottery]
GO
/****** Object:  Table [dbo].[Bets]    Script Date: 5/1/2021 10:19:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bets](
	[BetID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NULL,
	[BetNumber] [int] NULL,
	[BetAmount] [decimal](18, 2) NULL,
	[CreatedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
 CONSTRAINT [PK_Bets] PRIMARY KEY CLUSTERED 
(
	[BetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/1/2021 10:19:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bets] ADD  CONSTRAINT [DF_Bets_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  StoredProcedure [dbo].[Lottery_01_GetWinner]    Script Date: 5/1/2021 10:19:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Lottery_01_GetWinner]
	-- Add the parameters for the stored procedure here
	@UserID bigint = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	declare @WinningNumber int = 0,@WinningAmount decimal(18,2)=0

	set @WinningNumber =(select top 1 BetNumber from Bets Where IsActive = 1
	group by BetNumber
	order by SUM(BetAmount))

	set @WinningAmount = (select top 1 SUM(BetAmount) from Bets Where IsActive = 1
	group by BetNumber
	order by SUM(BetAmount))

	select UserID,@WinningNumber as BetNumber,@WinningAmount as TotalBetAmount from Bets
	where BetNumber = @WinningNumber and UserID = @UserID and IsActive = 1

	update Bets
	set IsActive = 0
	where UserID = @UserID

    -- Insert statements for procedure here
	--SELECT bt.*,tbl1.TotalBetAmount
	--FROM Bets bt
	--  INNER JOIN
	--  (
	--	SELECT BetNumber, MIN(BetAmount) MinPoint,SUM(BetAmount) as TotalBetAmount
	--	FROM Bets
	--	GROUP BY BetNumber
	--  ) tbl1
	--  ON tbl1.BetNumber = bt.BetNumber
	--WHERE tbl1.MinPoint = bt.BetAmount
	--and tbl1.TotalBetAmount > 0
END
GO
/****** Object:  StoredProcedure [dbo].[Lottery_01_RegisterUser]    Script Date: 5/1/2021 10:19:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Lottery_01_RegisterUser]
	-- Add the parameters for the stored procedure here
	@Email nvarchar(50) = NULL,
	@Password nvarchar(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF(@Email is not null and @Password is not null)
	begin

		insert into Users
		(UserName,
		Email,
		[Password],
		IsActive,
		CreatedDate)
		Values
		(@Email,
		@Email,
		@Password,
		1,
		GETDATE())
	end
END
GO
/****** Object:  StoredProcedure [dbo].[Lottery_01_SaveBet]    Script Date: 5/1/2021 10:19:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Lottery_01_SaveBet]
	-- Add the parameters for the stored procedure here
	@BetNumber int = NULL,
	@BetAmount int = NULL,
	@UserID bigint = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF(@BetAmount is not null and @UserID is not null)
	begin
		insert into Bets
		(UserID,
		BetNumber,
		BetAmount,
		CreatedDate)
		values
		(@UserID,
		@BetNumber,
		@BetAmount,
		GETDATE())

	end
END
GO
/****** Object:  StoredProcedure [dbo].[Lottery_01_VerifyUser]    Script Date: 5/1/2021 10:19:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Lottery_01_VerifyUser] 
	-- Add the parameters for the stored procedure here
	@Email nvarchar(50) = NULL,
	@Password nvarchar(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	if(@Email is not null and @Password is not null)
	begin
		select top 1 UserID,Email,IsActive,CreatedDate from Users
		where Email = @Email and [Password] = @Password
		order by 1 desc
	end
END
GO
