Use Auth

CREATE TABLE dbo.UserCredentials(
	Email nchar(20) NULL,
	Password nchar(100) NULL,
	Role nchar(20) NULL
)
GO
INSERT dbo.UserCredentials (Email, Password, Role) VALUES (N'OraB@edu.com        ', N'1000:R7NZZO/G2dy+pcXZyzLckojQGgLZ9dkA:6JBPH+dsuFvwbKcyK5owrRlxuoI=                                  ', N'Teacher             ')
INSERT dbo.UserCredentials (Email, Password, Role) VALUES (N'WileyK@edu.com      ', N'1000:BJ00ZTEVjd/Q7hK798/sTH2AQtfjndkW:FyIEiZgb029olMxBCYuFu9imDjg=                                  ', N'Teacher             ')
INSERT dbo.UserCredentials (Email, Password, Role) VALUES (N'AdminTest@edu.com   ', N'1000:R7NZZO/G2dy+pcXZyzLckojQGgLZ9dkA:6JBPH+dsuFvwbKcyK5owrRlxuoI=                                  ', N'Coordinator         ')
INSERT dbo.UserCredentials (Email, Password, Role) VALUES (N'TeacherTest@edu.com ', N'1000:R7NZZO/G2dy+pcXZyzLckojQGgLZ9dkA:6JBPH+dsuFvwbKcyK5owrRlxuoI=                                  ', N'Teacher             ')
GO


CREATE PROCEDURE dbo.Auth
	@Email AS NCHAR(50),
	@Password AS NCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP (1) * 
	FROM UserCredentials
	Where Email = @Email
	AND Password = @Password;

	End
GO
USE master
GO
ALTER DATABASE Auth SET  READ_WRITE 
GO
