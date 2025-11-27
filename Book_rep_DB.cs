using System;
using System.Collections.Generic;
using Npgsql;

namespace Library
{
    public class Book_rep_DB
    {
        private readonly string connectionString;

        public Book_rep_DB(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Book GetBookByID(int id)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT book_id, isbn, title, author, genre FROM books WHERE book_id = @id";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);

            using var reader = cmd.ExecuteReader();
            if (!reader.Read())
                throw new Exception($"Книга с ID={id} не найдена!");

            return new Book(
                reader.GetInt32(0),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetString(3),
                reader.GetString(4)
            );
        }

    }
}
