CREATE TABLE Departments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(40) NOT NULL
);

CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(40) NOT NULL
);

CREATE TABLE Phones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PhoneNumber NVARCHAR(20) NOT NULL
);

CREATE TABLE Residents (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Initial NVARCHAR(15) NOT NULL,
    DepartmentId INT NOT NULL
    CONSTRAINT FK_Residents_Departments FOREIGN KEY (DepartmentId) REFERENCES Departments(Id)
);

CREATE TABLE Staffs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentId INT NOT NULL,
    RoleId INT NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Initials NVARCHAR(15) NOT NULL,
    FirstName NVARCHAR(30) NOT NULL,
    LastName NVARCHAR(30) NOT NULL,
    CONSTRAINT FK_Staffs_Departments FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),
    CONSTRAINT FK_Staffs_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

CREATE TABLE Responsibility_areas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    StaffId INT NULL,
    PhoneId INT NULL,
    [Description] NVARCHAR(200),
    [Date] DATE NOT NULL,
    CONSTRAINT FK_Responsibility_areas_Staffs FOREIGN KEY (StaffId) REFERENCES Staffs(Id) ON DELETE SET NULL,
    CONSTRAINT FK_Responsibility_areas_Phones FOREIGN KEY (PhoneId) REFERENCES Phones(Id) ON DELETE SET NULL
);

CREATE TABLE Post_its (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ResidentId INT NOT NULL,
    StaffId INT NULL,          
    [Date] DATETIME2 NOT NULL,
    Payment NVARCHAR(100),
    ShoppingDay NVARCHAR(100),
    Mood NVARCHAR(100),
    [Status] NVARCHAR(500),
    [Events] NVARCHAR(300),
    RelativesContact NVARCHAR(100),
    
    CONSTRAINT FK_Post_its_Residents FOREIGN KEY (ResidentId) REFERENCES Residents(Id) ON DELETE CASCADE,
    CONSTRAINT FK_Post_its_Staffs FOREIGN KEY (StaffId) REFERENCES Staffs(Id) ON DELETE SET NULL
);

CREATE TABLE Pn_times (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostItId INT NOT NULL,
    [Description] NVARCHAR(200) NOT NULL,
    [Time] DATETIME2 NOT NULL,
    CONSTRAINT FK_Pn_times_Post_its FOREIGN KEY (PostItId) REFERENCES Post_its(Id) ON DELETE CASCADE
);

CREATE TABLE Medicines (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostItId INT NOT NULL,
    [Description] NVARCHAR(200) NOT NULL,
    [TimeStamp] DATETIME2 NOT NULL,
    CreatedAt DATETIME2 NULL,

    CONSTRAINT FK_Medicines_Post_its FOREIGN KEY (PostItId) REFERENCES Post_its(Id) ON DELETE CASCADE
);

CREATE TABLE Risks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PostItId INT NOT NULL UNIQUE, 
    RiskAssessment NVARCHAR(40),
    CONSTRAINT FK_Risks_Post_its FOREIGN KEY (PostItId) REFERENCES Post_its(Id) ON DELETE CASCADE
);