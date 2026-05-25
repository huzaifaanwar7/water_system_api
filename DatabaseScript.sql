-- ============================================================
-- ISMVCC Cricket Management Platform - Phase 1 Schema
-- SQL Server (T-SQL)
-- ============================================================

IF DB_ID('ISMVCC') IS NULL CREATE DATABASE ISMVCC;
GO
USE ISMVCC;
GO

-- =============== Users ===============
IF OBJECT_ID('dbo.Users','U') IS NULL
CREATE TABLE dbo.Users (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    Username            NVARCHAR(100)   NOT NULL UNIQUE,
    Email               NVARCHAR(150)   NULL,
    Phone               NVARCHAR(20)    NULL,
    Password            NVARCHAR(255)   NOT NULL,
    FullName            NVARCHAR(100)   NOT NULL,
    AvatarUrl           NVARCHAR(500)   NULL,
    Role                NVARCHAR(30)    NOT NULL DEFAULT 'Fan',
    ApprovalStatus      NVARCHAR(20)    NOT NULL DEFAULT 'Approved',
    ApprovedByUserId    INT             NULL,
    ApprovedAt          DATETIME2       NULL,
    LinkedPlayerId      INT             NULL,
    TeamId              INT             NULL,
    IsActive            BIT             NOT NULL DEFAULT 1,
    CreatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

IF OBJECT_ID('dbo.Teams','U') IS NULL
CREATE TABLE dbo.Teams (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    Name                NVARCHAR(120)   NOT NULL,
    ShortCode           NVARCHAR(4)     NOT NULL UNIQUE,
    Category            NVARCHAR(50)    NULL,
    City                NVARCHAR(100)   NULL,
    HomeVenue           NVARCHAR(100)   NULL,
    FoundedYear         SMALLINT        NULL,
    PrimaryColorHex     NVARCHAR(7)     NULL,
    SecondaryColorHex   NVARCHAR(7)     NULL,
    LogoUrl             NVARCHAR(500)   NULL,
    CaptainUserId       INT             NULL,
    CaptainPlayerId     INT             NULL,
    ApprovalStatus      NVARCHAR(20)    NOT NULL DEFAULT 'Pending',
    ApprovedByUserId    INT             NULL,
    ApprovedAt          DATETIME2       NULL,
    IsDeleted           BIT             NOT NULL DEFAULT 0,
    CreatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt           DATETIME2       NULL
);
GO

IF OBJECT_ID('dbo.Players','U') IS NULL
CREATE TABLE dbo.Players (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    FullName            NVARCHAR(150)   NOT NULL,
    DateOfBirth         DATETIME2       NULL,
    Role                NVARCHAR(30)    NULL,
    BattingHandedness   NVARCHAR(10)    NULL,
    BowlingStyle        NVARCHAR(50)    NULL,
    City                NVARCHAR(100)   NULL,
    PhotoUrl            NVARCHAR(500)   NULL,
    TeamId              INT             NULL,
    JerseyNumber        INT             NULL,
    ApprovalStatus      NVARCHAR(20)    NOT NULL DEFAULT 'Pending',
    CreatedByUserId     INT             NULL,
    ApprovedByUserId    INT             NULL,
    ApprovedAt          DATETIME2       NULL,
    IsDeleted           BIT             NOT NULL DEFAULT 0,
    CreatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt           DATETIME2       NULL
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Players_FullName' AND object_id=OBJECT_ID('dbo.Players'))
CREATE INDEX IX_Players_FullName ON dbo.Players(FullName) WHERE IsDeleted = 0;
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Players_TeamId' AND object_id=OBJECT_ID('dbo.Players'))
CREATE INDEX IX_Players_TeamId ON dbo.Players(TeamId);
GO

IF OBJECT_ID('dbo.TeamPlayers','U') IS NULL
CREATE TABLE dbo.TeamPlayers (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    TeamId          INT             NOT NULL,
    PlayerId        INT             NOT NULL,
    JerseyNumber    INT             NULL,
    Season          NVARCHAR(20)    NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    CreatedAt       DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_TeamPlayers_Team   FOREIGN KEY (TeamId)   REFERENCES dbo.Teams(Id),
    CONSTRAINT FK_TeamPlayers_Player FOREIGN KEY (PlayerId) REFERENCES dbo.Players(Id)
);
GO

IF OBJECT_ID('dbo.Tournaments','U') IS NULL
CREATE TABLE dbo.Tournaments (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    Name                NVARCHAR(200)   NOT NULL,
    Edition             NVARCHAR(50)    NULL,
    Category            NVARCHAR(50)    NULL,
    Format              NVARCHAR(20)    NULL,
    MatchFormat         NVARCHAR(20)    NULL,
    OversPerInnings     INT             NULL,
    StartDate           DATETIME2       NULL,
    EndDate             DATETIME2       NULL,
    Stage               NVARCHAR(30)    NOT NULL DEFAULT 'Registration',
    LogoUrl             NVARCHAR(500)   NULL,
    CreatedByUserId     INT             NULL,
    IsDeleted           BIT             NOT NULL DEFAULT 0,
    CreatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt           DATETIME2       NULL
);
GO

IF OBJECT_ID('dbo.TournamentTeams','U') IS NULL
CREATE TABLE dbo.TournamentTeams (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    TournamentId    INT             NOT NULL,
    TeamId          INT             NOT NULL,
    GroupName       NVARCHAR(20)    NULL,
    CreatedAt       DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_TT_Tournament FOREIGN KEY (TournamentId) REFERENCES dbo.Tournaments(Id),
    CONSTRAINT FK_TT_Team       FOREIGN KEY (TeamId)       REFERENCES dbo.Teams(Id)
);
GO

IF OBJECT_ID('dbo.Matches','U') IS NULL
CREATE TABLE dbo.Matches (
    Id                      INT IDENTITY(1,1) PRIMARY KEY,
    TournamentId            INT             NULL,
    MatchName               NVARCHAR(200)   NOT NULL,
    HomeTeamId              INT             NOT NULL,
    AwayTeamId              INT             NOT NULL,
    Venue                   NVARCHAR(150)   NULL,
    ScheduledStart          DATETIME2       NOT NULL,
    ActualStart             DATETIME2       NULL,
    ActualEnd               DATETIME2       NULL,
    MatchFormat             NVARCHAR(20)    NOT NULL DEFAULT 'T20',
    OversPerInnings         INT             NOT NULL DEFAULT 20,
    BallsPerOver            INT             NOT NULL DEFAULT 6,
    TossWinnerTeamId        INT             NULL,
    TossDecision            NVARCHAR(10)    NULL,
    MatchState              NVARCHAR(20)    NOT NULL DEFAULT 'Scheduled',
    ResultWinnerTeamId      INT             NULL,
    ResultMargin            NVARCHAR(100)   NULL,
    ManOfTheMatchPlayerId   INT             NULL,
    StageLabel              NVARCHAR(50)    NULL,
    CreatedByUserId         INT             NULL,
    IsDeleted               BIT             NOT NULL DEFAULT 0,
    CreatedAt               DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt               DATETIME2       NULL,
    CONSTRAINT FK_Match_Home FOREIGN KEY (HomeTeamId) REFERENCES dbo.Teams(Id),
    CONSTRAINT FK_Match_Away FOREIGN KEY (AwayTeamId) REFERENCES dbo.Teams(Id)
);
GO
-- Add new penalty columns to existing Matches table (idempotent)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name='HomePenaltyRuns' AND object_id=OBJECT_ID('dbo.Matches'))
    ALTER TABLE dbo.Matches ADD HomePenaltyRuns INT NOT NULL DEFAULT 0;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name='AwayPenaltyRuns' AND object_id=OBJECT_ID('dbo.Matches'))
    ALTER TABLE dbo.Matches ADD AwayPenaltyRuns INT NOT NULL DEFAULT 0;
GO
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE Name='PenaltyReason' AND object_id=OBJECT_ID('dbo.Matches'))
    ALTER TABLE dbo.Matches ADD PenaltyReason NVARCHAR(200) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Matches_State' AND object_id=OBJECT_ID('dbo.Matches'))
CREATE INDEX IX_Matches_State ON dbo.Matches(MatchState);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Matches_Sched' AND object_id=OBJECT_ID('dbo.Matches'))
CREATE INDEX IX_Matches_Sched ON dbo.Matches(ScheduledStart);
GO

IF OBJECT_ID('dbo.ApprovalRequests','U') IS NULL
CREATE TABLE dbo.ApprovalRequests (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    EntityType          NVARCHAR(30)    NOT NULL,
    EntityId            INT             NOT NULL,
    RequestedByUserId   INT             NOT NULL,
    Status              NVARCHAR(20)    NOT NULL DEFAULT 'Pending',
    Notes               NVARCHAR(500)   NULL,
    RejectionReason     NVARCHAR(500)   NULL,
    ReviewedByUserId    INT             NULL,
    ReviewedAt          DATETIME2       NULL,
    CreatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME()
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Approval_Status' AND object_id=OBJECT_ID('dbo.ApprovalRequests'))
CREATE INDEX IX_Approval_Status ON dbo.ApprovalRequests(Status, EntityType);
GO

-- =============== Innings ===============
IF OBJECT_ID('dbo.Innings','U') IS NULL
CREATE TABLE dbo.Innings (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    MatchId             INT             NOT NULL,
    InningsNumber       INT             NOT NULL,
    BattingTeamId       INT             NOT NULL,
    BowlingTeamId       INT             NOT NULL,
    TotalRuns           INT             NOT NULL DEFAULT 0,
    Wickets             INT             NOT NULL DEFAULT 0,
    LegalBallsBowled    INT             NOT NULL DEFAULT 0,
    ExtrasWides         INT             NOT NULL DEFAULT 0,
    ExtrasNoBalls       INT             NOT NULL DEFAULT 0,
    ExtrasByes          INT             NOT NULL DEFAULT 0,
    ExtrasLegByes       INT             NOT NULL DEFAULT 0,
    ExtrasPenalty       INT             NOT NULL DEFAULT 0,
    Target              INT             NULL,
    IsClosed            BIT             NOT NULL DEFAULT 0,
    ClosedAt            DATETIME2       NULL,
    ClosedReason        NVARCHAR(20)    NULL,
    CreatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_Innings_Match FOREIGN KEY (MatchId) REFERENCES dbo.Matches(Id)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Innings_Match' AND object_id=OBJECT_ID('dbo.Innings'))
CREATE INDEX IX_Innings_Match ON dbo.Innings(MatchId, InningsNumber);
GO

-- =============== Balls (fact table) ===============
IF OBJECT_ID('dbo.Balls','U') IS NULL
CREATE TABLE dbo.Balls (
    Id                      INT IDENTITY(1,1) PRIMARY KEY,
    InningsId               INT             NOT NULL,
    MatchId                 INT             NOT NULL,
    BallGuid                UNIQUEIDENTIFIER NOT NULL,
    OverNumber              INT             NOT NULL,
    BallInOver              INT             NOT NULL,
    BallSequence            INT             NOT NULL,
    StrikerPlayerId         INT             NULL,
    NonStrikerPlayerId      INT             NULL,
    BowlerPlayerId          INT             NULL,
    RunsBatter              INT             NOT NULL DEFAULT 0,
    RunsExtras              INT             NOT NULL DEFAULT 0,
    ExtrasType              NVARCHAR(20)    NULL,
    IsLegalDelivery         BIT             NOT NULL DEFAULT 1,
    IsFreeHit               BIT             NOT NULL DEFAULT 0,
    IsWicket                BIT             NOT NULL DEFAULT 0,
    WicketType              NVARCHAR(20)    NULL,
    DismissedPlayerId       INT             NULL,
    FielderPlayerId         INT             NULL,
    Commentary              NVARCHAR(500)   NULL,
    ScoredByUserId          INT             NULL,
    IsUndone                BIT             NOT NULL DEFAULT 0,
    BowledAt                DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT UQ_Balls_BallGuid UNIQUE (BallGuid),
    CONSTRAINT FK_Balls_Innings FOREIGN KEY (InningsId) REFERENCES dbo.Innings(Id),
    CONSTRAINT FK_Balls_Match   FOREIGN KEY (MatchId)   REFERENCES dbo.Matches(Id)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Balls_InningsSeq' AND object_id=OBJECT_ID('dbo.Balls'))
CREATE INDEX IX_Balls_InningsSeq ON dbo.Balls(InningsId, BallSequence);
GO

-- =============== BattingScores ===============
IF OBJECT_ID('dbo.BattingScores','U') IS NULL
CREATE TABLE dbo.BattingScores (
    Id                      INT IDENTITY(1,1) PRIMARY KEY,
    InningsId               INT             NOT NULL,
    PlayerId                INT             NOT NULL,
    BattingOrder            INT             NOT NULL,
    Runs                    INT             NOT NULL DEFAULT 0,
    BallsFaced              INT             NOT NULL DEFAULT 0,
    Fours                   INT             NOT NULL DEFAULT 0,
    Sixes                   INT             NOT NULL DEFAULT 0,
    IsOut                   BIT             NOT NULL DEFAULT 0,
    DismissalDescription    NVARCHAR(200)   NULL,
    DismissalBallId         INT             NULL,
    CONSTRAINT FK_BS_Innings FOREIGN KEY (InningsId) REFERENCES dbo.Innings(Id)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_BS_Innings_Player' AND object_id=OBJECT_ID('dbo.BattingScores'))
CREATE UNIQUE INDEX IX_BS_Innings_Player ON dbo.BattingScores(InningsId, PlayerId);
GO

-- =============== BowlingFigures ===============
IF OBJECT_ID('dbo.BowlingFigures','U') IS NULL
CREATE TABLE dbo.BowlingFigures (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    InningsId       INT             NOT NULL,
    PlayerId        INT             NOT NULL,
    LegalBalls      INT             NOT NULL DEFAULT 0,
    Maidens         INT             NOT NULL DEFAULT 0,
    RunsConceded    INT             NOT NULL DEFAULT 0,
    Wickets         INT             NOT NULL DEFAULT 0,
    Dots            INT             NOT NULL DEFAULT 0,
    Fours           INT             NOT NULL DEFAULT 0,
    Sixes           INT             NOT NULL DEFAULT 0,
    Wides           INT             NOT NULL DEFAULT 0,
    NoBalls         INT             NOT NULL DEFAULT 0,
    CONSTRAINT FK_BF_Innings FOREIGN KEY (InningsId) REFERENCES dbo.Innings(Id)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_BF_Innings_Player' AND object_id=OBJECT_ID('dbo.BowlingFigures'))
CREATE UNIQUE INDEX IX_BF_Innings_Player ON dbo.BowlingFigures(InningsId, PlayerId);
GO

-- =============== FallOfWickets ===============
IF OBJECT_ID('dbo.FallOfWickets','U') IS NULL
CREATE TABLE dbo.FallOfWickets (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    InningsId           INT             NOT NULL,
    BallId              INT             NOT NULL,
    WicketNumber        INT             NOT NULL,
    Runs                INT             NOT NULL,
    LegalBallsAt        INT             NOT NULL,
    DismissedPlayerId   INT             NOT NULL,
    CreatedAt           DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT FK_FoW_Innings FOREIGN KEY (InningsId) REFERENCES dbo.Innings(Id)
);
GO

-- =============== Commentaries ===============
IF OBJECT_ID('dbo.Commentaries','U') IS NULL
CREATE TABLE dbo.Commentaries (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    BallId          INT             NOT NULL,
    MatchId         INT             NOT NULL,
    Text            NVARCHAR(1000)  NOT NULL,
    IsMilestone     BIT             NOT NULL DEFAULT 0,
    MilestoneType   NVARCHAR(30)    NULL,
    IsOverridden    BIT             NOT NULL DEFAULT 0,
    CreatedAt       DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    CONSTRAINT UQ_Comm_Ball UNIQUE (BallId),
    CONSTRAINT FK_Comm_Ball  FOREIGN KEY (BallId)  REFERENCES dbo.Balls(Id),
    CONSTRAINT FK_Comm_Match FOREIGN KEY (MatchId) REFERENCES dbo.Matches(Id)
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Comm_Match' AND object_id=OBJECT_ID('dbo.Commentaries'))
CREATE INDEX IX_Comm_Match ON dbo.Commentaries(MatchId, CreatedAt DESC);
GO

-- =============== Sponsors ===============
IF OBJECT_ID('dbo.Sponsors','U') IS NULL
CREATE TABLE dbo.Sponsors (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    Name            NVARCHAR(150)   NOT NULL,
    Tagline         NVARCHAR(200)   NULL,
    LogoUrl         NVARCHAR(500)   NULL,
    WebsiteUrl      NVARCHAR(500)   NULL,
    ContactPhone   NVARCHAR(30)    NULL,
    Slots           NVARCHAR(300)   NULL,
    TournamentId    INT             NULL,
    IsActive        BIT             NOT NULL DEFAULT 1,
    IsDeleted       BIT             NOT NULL DEFAULT 0,
    CreatedAt       DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME(),
    UpdatedAt       DATETIME2       NULL
);
GO

-- =============== Notifications ===============
IF OBJECT_ID('dbo.Notifications','U') IS NULL
CREATE TABLE dbo.Notifications (
    Id              INT IDENTITY(1,1) PRIMARY KEY,
    UserId          INT             NULL,
    Type            NVARCHAR(100)   NOT NULL,
    Title           NVARCHAR(150)   NOT NULL,
    Body            NVARCHAR(500)   NULL,
    MatchId         INT             NULL,
    TournamentId    INT             NULL,
    IsRead          BIT             NOT NULL DEFAULT 0,
    CreatedAt       DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME()
);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name='IX_Notif_User' AND object_id=OBJECT_ID('dbo.Notifications'))
CREATE INDEX IX_Notif_User ON dbo.Notifications(UserId, CreatedAt DESC);
GO

-- =============== OutboxBalls (audit trail) ===============
IF OBJECT_ID('dbo.OutboxBalls','U') IS NULL
CREATE TABLE dbo.OutboxBalls (
    Id                  INT IDENTITY(1,1) PRIMARY KEY,
    BallId              INT             NULL,
    BallGuid            UNIQUEIDENTIFIER NOT NULL,
    MatchId             INT             NOT NULL,
    InningsId           INT             NOT NULL,
    ClientDeviceId      NVARCHAR(100)   NULL,
    ReceivedAt          DATETIME2       NOT NULL DEFAULT SYSUTCDATETIME()
);
GO

-- =============== Seed Users ===============
IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = 'superadmin')
INSERT INTO dbo.Users (Username, Email, Password, FullName, Role, ApprovalStatus, IsActive)
VALUES ('superadmin', 'admin@ismvcc.com', 'Admin@123', 'Super Admin', 'SuperAdmin', 'Approved', 1);
GO

IF NOT EXISTS (SELECT 1 FROM dbo.Users WHERE Username = 'scorer')
INSERT INTO dbo.Users (Username, Email, Password, FullName, Role, ApprovalStatus, IsActive)
VALUES ('scorer', 'scorer@ismvcc.com', 'Scorer@123', 'Default Scorer', 'Scorer', 'Approved', 1);
GO
