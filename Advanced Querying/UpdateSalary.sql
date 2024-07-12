USE SoftUni
CREATE PROCEDURE UpdateSalary @percent INT
AS
UPDATE Employees SET Salary = Salary + (Salary/100*@percent)