using Microsoft.EntityFrameworkCore;

namespace AcademicRecordsApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            //Scaffold-DbContext "Server=DESKTOP-58IIM9O\SQLEXPRESS;Database=AcademicRecordsDB; Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
        }
    }
}