
CREATE TABLE Volunteers (
    VolunteerId INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL,
    TaskType NVARCHAR(100) NOT NULL,
    Availability DATETIME NOT NULL,
    CONSTRAINT FK_Volunteers_AspNetUsers FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

CREATE TABLE IncidentReports (
    IncidentId INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL,
    Date DATETIME NOT NULL,
    Incident NVARCHAR(255) NOT NULL,
    Location NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
)

CREATE USER [ST10269117@rcconnect.edu.za] FROM EXTERNAL PROVIDER;
ALTER ROLE db_datareader ADD MEMBER [ST10269117@rcconnect.edu.za];
ALTER ROLE db_datawriter ADD MEMBER [ST10269117@rcconnect.edu.za];


CREATE TABLE Donations (
    DonationId INT PRIMARY KEY IDENTITY(1,1),
    UserId NVARCHAR(450) NOT NULL,
    Date DATETIME NOT NULL,
    Type NVARCHAR(100) NOT NULL,
    AidQuantity INT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES AspNetUsers(Id)
);

CREATE TABLE AspNetUsers (
    Id NVARCHAR(450) PRIMARY KEY,
    UserName NVARCHAR(256),
    NormalizedUserName NVARCHAR(256),
    Email NVARCHAR(256),
    NormalizedEmail NVARCHAR(256),
    EmailConfirmed BIT NOT NULL,
    PasswordHash NVARCHAR(MAX),
    SecurityStamp NVARCHAR(MAX),
    ConcurrencyStamp NVARCHAR(MAX),
    PhoneNumber NVARCHAR(MAX),
    PhoneNumberConfirmed BIT NOT NULL,
    TwoFactorEnabled BIT NOT NULL,
    LockoutEnd DATETIMEOFFSET,
    LockoutEnabled BIT NOT NULL,
    AccessFailedCount INT NOT NULL
);
