CREATE TABLE Cars
(
	Id INT PRIMARY KEY IDENTITY,
	RegistrationNumber INT,
	BodyNumber INT NOT NULL,
	EngineNumber INT NOT NULL,
	IssueYear INT NOT NULL,
	Mileage INT,
	LastTI DATETIME2,
	ModelId INT
	FOREIGN KEY (ModelId) REFERENCES dbo.Models(Id),
	DriverId INT
	FOREIGN KEY (DriverId) REFERENCES dbo.Employees(Id),
	MechanicId INT
	FOREIGN KEY (MechanicId) REFERENCES dbo.Employees(Id),
);