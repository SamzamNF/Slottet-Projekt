CREATE TABLE PhoneAudit(
AuditId INT IDENTITY(1,1) PRIMARY KEY,
PhoneId INT NOT NULL,
ActionType NVARChar(15),
ChangedAt DateTime2 DEFAULT SYSDATETIME(),

OldPhoneNumber NVARChar(40),
NewPhoneNumber NVARChar(40)
);


CREATE TRIGGER trg_Phone_Audit
ON Phones
AFTER UPDATE, DELETE
AS 
BEGIN
	SET NOCOUNT ON;

	-- Delete --

	INSERT INTO PhoneAudit(
		PhoneId,
		ActionType,
		OldPhoneNumber
	)

	SELECT
		d.Id,
		'DELETE',
		d.PhoneNumber
	FROM DELETED d
	LEFT JOIN inserted i 
		on i.Id = d.Id
	-- Only rows with no match in inserted (DELETE statements;
	-- if a row exists in inserted, it is an UPDATE statement)
	WHERE i.Id IS NULL;



	-- Update --

	INSERT INTO PhoneAudit(
		PhoneId,
		ActionType,
		OldPhoneNumber,
		NewPhoneNumber
	)

	SELECT 
		d.Id,
		'UPDATE',
		d.PhoneNumber,
		i.PhoneNumber
	FROM inserted i
	-- If deleted id matches inserted id = UPDATE statement
	INNER JOIN deleted d
		on i.Id = d.Id
END;
