-- Recreate TodoListNotesDB (Users + Todos only, no Notes)
IF DB_ID('TodoListNotesDB') IS NULL
BEGIN
    CREATE DATABASE TodoListNotesDB;
END
GO
USE TodoListNotesDB;
GO

-- Drop old tables if exist (order matters)
IF OBJECT_ID('dbo.Todos', 'U') IS NOT NULL DROP TABLE dbo.Todos;
IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL DROP TABLE dbo.Users;
GO

-- Users
CREATE TABLE dbo.Users (
    UserId     INT IDENTITY(1,1) PRIMARY KEY,
    UserName   NVARCHAR(100) NOT NULL,
    Email      NVARCHAR(150) NOT NULL UNIQUE,
    Password   NVARCHAR(100) NOT NULL,         -- theo yêu cầu: plain text
    CreatedAt  DATETIME NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT GETDATE()
);
GO

-- Todos (ON DELETE CASCADE)
CREATE TABLE dbo.Todos (
    TodoId        INT IDENTITY(1,1) PRIMARY KEY,
    UserId        INT NOT NULL,
    Title         NVARCHAR(200) NOT NULL,
    Description   NVARCHAR(MAX) NULL,
    DueDate       DATETIME NULL,
    ReminderTime  DATETIME NULL,
    IsCompleted   BIT NOT NULL CONSTRAINT DF_Todos_IsCompleted DEFAULT (0),
    CreatedAt     DATETIME NOT NULL CONSTRAINT DF_Todos_CreatedAt DEFAULT GETDATE(),
    CONSTRAINT FK_Todos_Users FOREIGN KEY (UserId)
        REFERENCES dbo.Users(UserId) ON DELETE CASCADE
);
GO

-- Indexes (typical filters/sorts)
CREATE INDEX IX_Todos_UserId          ON dbo.Todos(UserId);
CREATE INDEX IX_Todos_DueDate         ON dbo.Todos(DueDate);
CREATE INDEX IX_Todos_ReminderTime    ON dbo.Todos(ReminderTime);
CREATE INDEX IX_Todos_IsCompleted     ON dbo.Todos(IsCompleted);
GO

-- Seed data
INSERT INTO dbo.Users (UserName, Email, Password)
VALUES
(N'Đỗ Trọng Tín', 'tin@example.com', '123456'),
(N'Lê Quốc Hội', 'hoi@example.com', 'password1'),
(N'Nguyễn Minh Thư', 'thu@example.com', 'mypassword');
GO

INSERT INTO dbo.Todos (UserId, Title, Description, DueDate, ReminderTime, IsCompleted)
VALUES
(1, N'Hoàn tất báo cáo nhóm', N'Viết phần kết luận và định dạng lại toàn bộ file', '2025-11-12', '2025-11-11 20:00', 0),
(2, N'Tập gym', N'Ngày tập ngực và tay sau', '2025-11-11', '2025-11-11 17:30', 1),
(3, N'Đi siêu thị', N'Mua đồ ăn cho tuần mới', '2025-11-13', NULL, 0);
GO