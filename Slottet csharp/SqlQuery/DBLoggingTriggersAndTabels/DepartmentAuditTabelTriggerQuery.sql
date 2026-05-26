CREATE TABLE DepartmentAudit(
AuditId INT IDENTITY(1,1) PRIMARY KEY,
DepartmentId INT NOT NULL,
ActionType NVARChar(15),
ChangedAt DateTime2 DEFAULT SYSDATETIME(),

OldDepartmentName NVARChar(40),
NewDepartmentName NVARChar(40)
);


CREATE TRIGGER trg_Department_Audit
ON Departments
AFTER UPDATE, DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	-- Delete --

	INSERT INTO DepartmentAudit(
		DepartmentId,
		ActionType,
		OldDepartmentName
	)

	SELECT
		d.Id,
		'DELETE',
		d.DepartmentName
	FROM DELETED d
	LEFT JOIN inserted i 
		on i.Id = d.Id
	-- Only rows with no match in inserted (DELETE statements;
	-- if a row exists in inserted, it is an UPDATE statement)
	WHERE i.Id IS NULL;



	-- Update --

	INSERT INTO DepartmentAudit(
		DepartmentId,
		ActionType,
		OldDepartmentName,
		NewDepartmentName
	)

	SELECT 
		d.Id,
		'UPDATE',
		d.DepartmentName,
		i.DepartmentName
	FROM inserted i
	-- If deleted id matches inserted id = UPDATE statement
	INNER JOIN deleted d
		on i.Id = d.Id
END;
