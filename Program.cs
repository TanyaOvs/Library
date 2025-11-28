using System;
using System.IO;
using System.Collections.Generic;


namespace Library
{
    class Program
    {
        static void Main(string[] args)
        {
            // Работа с базой данных
            try
            {
                // Строка для подключения к БД
                string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=password;Database=Library";

                // 1. Объект для работы с БД
                Book_rep_DB brdb = new Book_rep_DB(connectionString);

                // a. Получить объект по ID
                Book testBook = brdb.GetBookByID(5);
                testBook.PrintFullInfo();

                // b. Получить список k по счету n объектов класса short 
                List<BookPreview> bpList = brdb.Get_K_N_ShortList(4, 2);

                string beautifulSeparator = "-------------";
                Console.WriteLine(beautifulSeparator);
                Console.WriteLine("Список книг: ");

                foreach (BookPreview bp in bpList)
                {
                    Console.WriteLine(beautifulSeparator);
                    bp.PrintShortInfo();
                }
                Console.WriteLine(beautifulSeparator);

                // c. Добавить объект в список
                Book superBook = new Book("979-5-12363-123-0", "Илиада", "Гомер", "Эпос");
                //brdb.AddBook(superBook);

                // d. Заменить элемент списка по ID
                Book superDuperBook = new Book("979-5-12993-321-9", "Записки Доктора Ватсона", "Артур Конан Дойл", "Детектив");
                //brdb.UpdateBook(12, superDuperBook);

                // e. Удалить элемент списка по ID
                //brdb.DeleteBook(12);

                // f. Получить количество элементов
                Console.WriteLine($"Количество книг в базе данных: {brdb.Get_Count()}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            
        }
    }
}
