using System;

namespace Library
{
    class Program
    {
        static void Main(string[] args)
        {
            // Работа с базой данных через адаптер
            try
            {
                // 1. Объект для работы с БД
                Book_rep_DB brdb = new Book_rep_DB();
                Book_rep brdb_adapter = new Book_rep_db_adapter(brdb);

                // a. Получить объект по ID
                Book testBook = brdb_adapter.GetBookByID(5);
                Console.WriteLine("Информация о найденной книге:");
                testBook.PrintFullInfo();

                // b. Получить список k по счету n объектов класса short 
                brdb_adapter.Get_K_N_ShortList(2);

                // c. Добавить объект в список
                Book superBook = new Book("979-5-95947-389-0", "Илиада", "Гомер", "Эпос");
                //brdb_adapter.AddBookInList(superBook);

                // d. Заменить элемент списка по ID
                Book superDuperBook = new Book("979-5-12093-301-9", "Записки Доктора Ватсона", "Артур Конан Дойл", "Детектив");
                //brdb_adapter.ChangeBookByID(superDuperBook, 16);

                // e. Удалить элемент списка по ID
                //brdb_adapter.DeleteBookInList(16);

                // f. Получить количество элементов
                Console.WriteLine($"Количество книг в базе данных: {brdb_adapter.Get_Count()}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                Book_rep_db_connection.Instance.Close();
            }
        }
    }
}
