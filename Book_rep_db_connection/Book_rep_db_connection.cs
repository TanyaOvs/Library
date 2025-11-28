using Npgsql;

namespace Library
{
    public class Book_rep_db_connection
    {
        private static Book_rep_db_connection instance;
        private readonly string connectionString;
        private NpgsqlConnection connection;

        private Book_rep_db_connection()
        {
            connectionString = "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=Library";
        }

        public static Book_rep_db_connection Instance
        {
            get
            {
                if (instance == null)
                    instance = new Book_rep_db_connection();
                return instance;
            }
        }

        public NpgsqlConnection Connection
        {
            get
            {
                if (connection == null)
                {
                    connection = new NpgsqlConnection(connectionString);
                    connection.Open();
                }
                else if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }
                return connection;
            }
        }

        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }
    }
}
