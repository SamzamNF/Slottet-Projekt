-- Does NOT save "Initial" for GDPR reasons, to not leave anything left in logs that can ID a resident.

CREATE TABLE ResidentsAudit (
    AuditId INT IDENTITY(1,1) PRIMARY KEY,
    ResidentId INT NOT NULL,
    ActionType NVARCHAR(15),
    ChangedAt DATETIME2 DEFAULT SYSDATETIME(),

    OldDepartmentId INT NULL,
    NewDepartmentId INT NULL
);

CREATE TRIGGER trg_Residents_Audit
ON Residents
AFTER UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- DELETE
    INSERT INTO ResidentsAudit (
        ResidentId,
        ActionType,
        OldDepartmentId
    )
    SELECT
        d.Id,
        'DELETE',
        d.DepartmentId
    FROM deleted d
    LEFT JOIN inserted i
        ON i.Id = d.Id
    WHERE i.Id IS NULL;


    -- UPDATE
    INSERT INTO ResidentsAudit (
        ResidentId,
        ActionType,
        OldDepartmentId,
        NewDepartmentId
    )
    SELECT
        d.Id,
        'UPDATE',
        d.DepartmentId,
        i.DepartmentId
    FROM inserted i
    INNER JOIN deleted d
        ON i.Id = d.Id;
END;