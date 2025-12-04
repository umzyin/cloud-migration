using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace AzWinApp.Models
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository()
        {
            _connectionString = ConfigurationManager
                .ConnectionStrings["DefaultConnection"]
                .ConnectionString;
        }

        public IEnumerable<Employee> GetAll()
        {
            var employees = new List<Employee>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT Id, Name, Department FROM Employees", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Department = reader.IsDBNull(2) ? null : reader.GetString(2)
                        });
                    }
                }
            }

            return employees;
        }
    }
}
