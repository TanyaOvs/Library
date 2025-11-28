using System;
using Npgsql;
using System.Collections.Generic;


namespace Library
{
    public class Book_rep_DB
    {
        private readonly Book_rep_db_connection db = Book_rep_db_connection.Instance;
        public Book_rep_DB() { }

        public Book GetBookByID(int id)
        {
            string sql = "SELECT book_id, isbn, title, author, genre FROM books WHERE book_id = @id";

            using var cmd = new NpgsqlCommand(sql, db.Connection);
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

            string sql = @"
            SELECT isbn, title, author, genre, collateral_value, rental_cost
            FROM books
            ORDER BY book_id
            LIMIT @pageSize OFFSET @offset";

            using var cmd = new NpgsqlCommand(sql, db.Connection);
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

        private bool IsISBNUnique(string isbn, int? excludeBookId = null)
        {
            string sql = "SELECT COUNT(*) FROM books WHERE isbn = @isbn";

            // Если проверка при обновлении, то исключаем текущую книгу по ID
            if (excludeBookId.HasValue)
                sql += " AND book_id != @id";

            using var cmd = new NpgsqlCommand(sql, db.Connection);
            cmd.Parameters.AddWithValue("isbn", isbn);

            if (excludeBookId.HasValue)
                cmd.Parameters.AddWithValue("id", excludeBookId.Value);

            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count == 0;
        }

        public void AddBook(Book book)
        {
            if (!IsISBNUnique(book.ISBN))
                throw new ArgumentException($"Книга с ISBN '{book.ISBN}' уже существует в базе данных!");

            string sql = @"
            INSERT INTO books(isbn, title, author, genre)
            VALUES(@isbn, @title, @author, @genre)
            RETURNING book_id";

            using var cmd = new NpgsqlCommand(sql, db.Connection);
            cmd.Parameters.AddWithValue("isbn", book.ISBN.Trim());
            cmd.Parameters.AddWithValue("title", book.Title);
            cmd.Parameters.AddWithValue("author", book.Author);
            cmd.Parameters.AddWithValue("genre", book.Genre);

            int newId = (int)cmd.ExecuteScalar();
            Console.WriteLine($"В базу данных добавлена новая книга с ID = {newId}");
        }

        public void UpdateBook(int id, Book book)
        {
            if (!IsISBNUnique(book.ISBN, id))
                throw new ArgumentException($"Книга с ISBN '{book.ISBN}' уже существует в базе данных!");

            string sql = @"
            UPDATE books
            SET isbn = @isbn,
                title = @title,
                author = @author,
                genre = @genre
            WHERE book_id = @id";

            using var cmd = new NpgsqlCommand(sql, db.Connection);

            cmd.Parameters.AddWithValue("isbn", book.ISBN);
            cmd.Parameters.AddWithValue("title", book.Title);
            cmd.Parameters.AddWithValue("author", book.Author);
            cmd.Parameters.AddWithValue("genre", book.Genre);
            cmd.Parameters.AddWithValue("id", id);

            if (cmd.ExecuteNonQuery() == 0)
                throw new Exception("Книга для обновления не найдена!");
            Console.WriteLine("База данных успешно обновлена!");
        }

        public void DeleteBook(int id)
        {
            string sql = "DELETE FROM books WHERE book_id = @id";

            using var cmd = new NpgsqlCommand(sql, db.Connection);
            cmd.Parameters.AddWithValue("id", id);

            if (cmd.ExecuteNonQuery() == 0)
                throw new Exception("Книга для удаления не найдена!");

            Console.WriteLine($"Книга с id {id} удалена из базы данных!");
        }

        public int Get_Count()
        {
            string sql = "SELECT COUNT(*) FROM books";
            using var cmd = new NpgsqlCommand(sql, db.Connection);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
}
