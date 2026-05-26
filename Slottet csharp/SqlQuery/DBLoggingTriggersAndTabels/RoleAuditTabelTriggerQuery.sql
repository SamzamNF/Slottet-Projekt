CREATE TABLE RoleAudit(
AuditId INT IDENTITY(1,1) PRIMARY KEY,
RoleId INT NOT NULL,
ActionType NVARChar(15),
ChangedAt DateTime2 DEFAULT SYSDATETIME(),

OldRoleName NVARChar(40),
NewRoleName NVARChar(40)
);


CREATE TRIGGER trg_Role_Audit
ON Roles
AFTER UPDATE, DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	-- Delete --

	INSERT INTO RoleAudit(
		RoleId,
		ActionType,
		OldRoleName
	)

	SELECT
		d.Id,
		'DELETE',
		d.RoleName
	FROM DELETED d
	LEFT JOIN inserted i 
		on i.Id = d.Id
	-- Only rows with no match in inserted (DELETE statements;
	-- if a row exists in inserted, it is an UPDATE statement)
	WHERE i.Id IS NULL;



	-- Update --

	INSERT INTO RoleAudit(
		RoleId,
		ActionType,
		OldRoleName,
		NewRoleName
	)

	SELECT 
		d.Id,
		'UPDATE',
		d.RoleName,
		i.RoleName
	FROM inserted i
	-- If deleted id matches inserted id = UPDATE statement
	INNER JOIN deleted d
		on i.Id = d.Id
END;
