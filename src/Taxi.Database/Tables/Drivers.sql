CREATE TABLE Drivers
(
	Id INT PRIMARY KEY IDENTITY,
	ExperienceYear INT,
	EmployeeId INT
	FOREIGN KEY (EmployeeId) REFERENCES dbo.Employees(Id)
);