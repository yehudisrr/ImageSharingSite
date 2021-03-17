using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ImageSharePassword.Data
{
    public class ImageDb
    {
        private readonly string _connectionString;
        public ImageDb(string connectionString)
        {
            _connectionString = connectionString;
        }
        public int AddImage(string fileName, string password)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO Images (FileName, TimeUploaded, Password, Views) " +
                              "VALUES (@Filename, GETDATE(), @Password, 0) SELECT SCOPE_IDENTITY()";
            cmd.Parameters.AddWithValue("@Filename", fileName);
            cmd.Parameters.AddWithValue("@Password", password);
            connection.Open();
            return (int)(decimal)cmd.ExecuteScalar();
        }

        public List<Image> GetImages()
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Images";
            connection.Open();
            List<Image> images = new List<Image>();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                images.Add(new Image
                {
                    Id = (int)reader["Id"],
                    FileName = (string)reader["FileName"],
                    TimeUploaded = (DateTime)reader["TimeUploaded"],
                    Password = (string)reader["Password"],
                    Views = (int)reader["Views"]
                });
            }

            return images;

        }
        public void UpdateViewCount(Image image)
        {
            var connection = new SqlConnection(_connectionString);
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "UPDATE Images " +
                                "SET Views = @Views WHERE Id = @Id";
            cmd.Parameters.AddWithValue("@Views", image.Views);
            cmd.Parameters.AddWithValue("@Id", image.Id);
            connection.Open();
            cmd.ExecuteNonQuery();

        }
    }

  
}
