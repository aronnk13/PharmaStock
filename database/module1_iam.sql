-- MODULE 1: IDENTITY & ACCESS 

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'UserRole')
BEGIN
    CREATE TABLE UserRole (
        RoleID INT IDENTITY(1,1) PRIMARY KEY,
        RoleType VARCHAR(50) NOT NULL
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'User')
BEGIN
    CREATE TABLE [User] (
        UserID INT IDENTITY(1,1) PRIMARY KEY,
        Username VARCHAR(100) NOT NULL UNIQUE,
        RoleID INT NOT NULL,
        Email VARCHAR(100) NOT NULL UNIQUE,
        Phone VARCHAR(15) NOT NULL,
        CreatedOn DATETIME DEFAULT GETDATE(),
        CreatedBy VARCHAR(250) NOT NULL,
        UpdatedOn DATETIME DEFAULT GETDATE(),
        UpdatedBy VARCHAR(250) NOT NULL,
        StatusID BIT NOT NULL DEFAULT 1,
        PasswordHash VARCHAR(255) NOT NULL,
        CONSTRAINT FK_User_Role FOREIGN KEY (RoleID) REFERENCES UserRole(RoleID)
    );
END

IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Audit')
BEGIN
    CREATE TABLE Audit (
        AuditID INT IDENTITY(1,1) PRIMARY KEY,
        UserID INT NOT NULL,
        Action VARCHAR(100) NOT NULL,
        Resource VARCHAR(100) NOT NULL,
        [Timestamp] DATETIME DEFAULT GETDATE(),
        Metadata NVARCHAR(MAX),
        CONSTRAINT FK_Audit_User FOREIGN KEY (UserID) REFERENCES [User](UserID)
    );
END
