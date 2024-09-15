using InterviewTest.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ListController : ControllerBase
    {
        private readonly string connectionString = new SqliteConnectionStringBuilder() { DataSource = "./SqliteDB.db" }.ToString();

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            var employees = new List<Employee>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var queryCmd = connection.CreateCommand();
                queryCmd.CommandText = @"SELECT Name, Value FROM Employees";
                using (var reader = queryCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        employees.Add(new Employee
                        {
                            Name = reader.GetString(0),
                            Value = reader.GetInt32(1)
                        });
                    }
                }
            }

            return employees;
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var insertCmd = connection.CreateCommand();
                insertCmd.CommandText = "INSERT INTO Employees (Name, Value) VALUES (@name, @value)";
                insertCmd.Parameters.AddWithValue("@name", employee.Name);
                insertCmd.Parameters.AddWithValue("@value", employee.Value);
                insertCmd.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpPut("{name}")]
        public IActionResult Update(string name, Employee employee)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var updateCmd = connection.CreateCommand();
                updateCmd.CommandText = "UPDATE Employees SET Value = @value WHERE Name = @name";
                updateCmd.Parameters.AddWithValue("@name", name);
                updateCmd.Parameters.AddWithValue("@value", employee.Value);
                updateCmd.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var deleteCmd = connection.CreateCommand();
                deleteCmd.CommandText = "DELETE FROM Employees WHERE Name = @name";
                deleteCmd.Parameters.AddWithValue("@name", name);
                deleteCmd.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpGet("increment-values")]
        public IActionResult IncrementValues()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var updateCmd = connection.CreateCommand();
                updateCmd.CommandText = @"
                    UPDATE Employees
                    SET Value = 
                        CASE
                            WHEN Name LIKE 'E%' THEN Value + 1
                            WHEN Name LIKE 'G%' THEN Value + 10
                            ELSE Value + 100
                        END";
                updateCmd.ExecuteNonQuery();
            }

            return Ok();
        }

        [HttpGet("sum-values")]
        public IActionResult GetSumOfValues()
        {
            int totalValue = 0; // Initialize to 0 to ensure it has a value even if the query returns null

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var sumCmd = connection.CreateCommand();
                sumCmd.CommandText = @"
                SELECT SUM(Value) 
                FROM Employees 
                WHERE Name LIKE 'A%' OR Name LIKE 'B%' OR Name LIKE 'C%'";

                var result = sumCmd.ExecuteScalar();

                // Check if the result is not null or DBNull before converting
                if (result != DBNull.Value && result != null)
                {
                    totalValue = Convert.ToInt32(result);
                }
            }


            if (totalValue >= 11171)
            {
                return Ok(totalValue);
            }
            else
            {
                return Ok("The total sum of values is less than 11171");
            }
        }
    }
}
