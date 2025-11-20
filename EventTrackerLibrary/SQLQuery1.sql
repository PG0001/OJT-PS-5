-- Create Database
CREATE DATABASE EventTrackerDB;
GO

USE EventTrackerDB;
GO

----------------------------------------
-- 1. User Table
----------------------------------------
CREATE TABLE [User] (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL -- Example: Admin / User
);
GO


----------------------------------------
-- 2. TaskItem Table
----------------------------------------
CREATE TABLE TaskItem (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    Priority NVARCHAR(20) NOT NULL CHECK (Priority IN ('Low', 'Medium', 'High')),
    Status NVARCHAR(20) NOT NULL CHECK (Status IN ('Pending', 'InProgress', 'Completed')),
    AssignedTo INT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    DueDate DATETIME2 NULL,

    CONSTRAINT FK_TaskItem_User FOREIGN KEY (AssignedTo)
        REFERENCES [User](Id)
        ON DELETE SET NULL
);
GO


----------------------------------------
-- 3. TaskComment Table
----------------------------------------
CREATE TABLE TaskComment (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TaskId INT NOT NULL,
    UserId INT NOT NULL,
    CommentText NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_TaskComment_TaskItem FOREIGN KEY (TaskId)
        REFERENCES TaskItem(Id)
        ON DELETE CASCADE,

    CONSTRAINT FK_TaskComment_User FOREIGN KEY (UserId)
        REFERENCES [User](Id)
        ON DELETE CASCADE
);
GO

----------------------------------------
-- Optional Helpful Indexes
----------------------------------------
CREATE INDEX IDX_TaskItem_AssignedTo ON TaskItem(AssignedTo);
CREATE INDEX IDX_TaskItem_Status ON TaskItem(Status);
CREATE INDEX IDX_TaskItem_Priority ON TaskItem(Priority);

CREATE INDEX IDX_TaskComment_TaskId ON TaskComment(TaskId);
CREATE INDEX IDX_TaskComment_UserId ON TaskComment(UserId);
GO
INSERT INTO [User] (Name, Email, PasswordHash, Role)
VALUES
('Admin User', 'admin@example.com', 'hashedpassword123', 'Admin'),
('Prakash', 'prakash@example.com', 'hash_pw_001', 'User'),
('Tamil', 'tamil@example.com', 'hash_pw_002', 'User'),
('John Doe', 'john@example.com', 'hash_pw_003', 'User');
GO
INSERT INTO TaskItem 
(Title, Description, Priority, Status, AssignedTo, CreatedAt, DueDate)
VALUES
('Setup Project', 'Initialize backend and frontend structure', 'High', 'InProgress', 1, GETDATE(), '2025-12-01'),
('Create Login Page', 'Build UI + API for authentication', 'Medium', 'Pending', 2, GETDATE(), '2025-12-05'),
('Database Design', 'Create schema for tasks and comments', 'High', 'Completed', 1, GETDATE(), '2025-11-20'),
('Task UI Page', 'Design task list and task form page', 'Low', 'Pending', 3, GETDATE(), '2025-12-10');
GO
INSERT INTO TaskComment (TaskId, UserId, CommentText, CreatedAt)
VALUES
(1, 1, 'Started project setup. Creating base API.', GETDATE()),
(1, 2, 'Frontend skeleton created.', GETDATE()),
(2, 3, 'Working on login UI.', GETDATE()),
(3, 1, 'Database schema reviewed and finalized.', GETDATE());
GO


select * from TaskComment;
select * from TaskItem;
select * from [User];
USE EventTrackerDB;
GO


