using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace AzWinApp_Web.Models
{
    public class TestItemRepository
    {
        private readonly string _connectionString;

        public TestItemRepository()
        {
            // Uses your existing connection string
            _connectionString = ConfigurationManager
                .ConnectionStrings["DefaultConnection"]
                .ConnectionString;
        }

        public IEnumerable<TestItem> GetAll()
        {
            var items = new List<TestItem>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT Id, Name, Description, CreatedBy, CreatedOn
                    FROM TestItems
                    ORDER BY CreatedOn DESC";

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        items.Add(new TestItem
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            CreatedBy = reader.GetString(3),
                            CreatedOn = reader.GetDateTime(4)
                        });
                    }
                }
            }

            return items;
        }

        public void Insert(TestItem item)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
            INSERT INTO TestItems (Name, Description, CreatedBy, CreatedOn)
            VALUES (@Name, @Description, @CreatedBy, @CreatedOn);";

                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Description",
                    (object)item.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", item.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedOn", item.CreatedOn);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Update(TestItem item)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
            UPDATE TestItems
            SET Name = @Name,
                Description = @Description
            WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", item.Id);
                cmd.Parameters.AddWithValue("@Name", item.Name);
                cmd.Parameters.AddWithValue("@Description",
                    (object)item.Description ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
            DELETE FROM TestItems
            WHERE Id = @Id;";

                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public TestItem GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT Id, Name, Description, CreatedBy, CreatedOn
                    FROM TestItems
                    WHERE Id = @Id";

                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return null;

                    return new TestItem
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                        CreatedBy = reader.GetString(3),
                        CreatedOn = reader.GetDateTime(4)
                    };
                }
            }
        }
    }
}
