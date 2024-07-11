using System.Data.SqlClient;

namespace ADO_Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=DESKTOP-58IIM9O\\SQLEXPRESS;Database=SoftUni;User Id=DESKTOP-58IIM9O\\XidZ01;Password=;Trusted_Connection=True;";
            string query = "SELECT EmployeeId, FirstName, LastName, JobTitle FROM Employees WHERE DepartmentID = @departmentId";
            int departmentId = 7;

            using SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@departmentId", departmentId);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[1]} {reader[2]}: {reader[3]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}