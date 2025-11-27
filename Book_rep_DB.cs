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
        
        public List<BookPreview> Get_K_N_ShortList(int pageNumber, int pageSize)
        {
            List<BookPreview> result = new List<BookPreview>();
            int offset = (pageNumber - 1) * pageSize;

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = @"
            SELECT isbn, title, author, genre, collateral_value, rental_cost
            FROM books
            ORDER BY book_id
            LIMIT @pageSize OFFSET @offset";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("pageSize", pageSize);
            cmd.Parameters.AddWithValue("offset", offset);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string isbn = reader.GetString(0);
                string title = reader.GetString(1);
                string author = reader.GetString(2);
                string genre = reader.GetString(3);
                double collateral = reader.GetDouble(4);
                double rental = reader.GetDouble(5);

                BookPreview bp = new BookPreview(isbn, title, author, genre, collateral, rental);
                result.Add(bp);
            }
            return result;
        }
        
    }
}
