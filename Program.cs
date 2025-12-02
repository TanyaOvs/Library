using System;
using System.IO;


namespace Library
{
    class Program
    {
        static void Main(string[] args)
        {
            // Работа с JSON
            try
            {
                // Чтение объектов класса Book в List на основе JSON файла
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "books.json");

                // 1. Объект для работы с JSON
                Book_rep_json brj = new Book_rep_json();

                // a. Создание списка на основе JSON
                brj.ReadJson(fullPath);

                // b. Запись данных в файл
                brj.WriteJson();

                // c. Получение объекта по ID
                Book foundBook = brj.GetBookByID(2);
                Console.WriteLine($"Найденная книга: {foundBook.Title}");

                // d. Получить список k по счету n объектов класса short
                brj.ReadShortJson(fullPath);
                brj.Get_K_N_ShortList(5);

                // e. Сортировать элементы по выбранному полю (title)
                brj.SortBooksByTitle();

                // f. Добавить объект в список
                Book firstBook = new Book("979-5-54360-678-0", "Одиссея", "Гомер", "Эпос");
                brj.AddBookInList(firstBook);
                brj.PrintBooksList(brj.BookList);

                // g. Заменить элемент списка по ID
                Book secondBook = new Book("979-5-12363-123-0", "Илиада", "Гомер", "Эпос");
                brj.ChangeBookByID(secondBook, 2);

                // h. Удалить элемент списка по ID
                brj.DeleteBookInList(3);

                // i. Получить количество элементов
                Console.WriteLine($"Количество элементов в списке bookList: {brj.Get_Count()}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

        }
    }
}
