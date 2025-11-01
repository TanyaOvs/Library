using System;
using System.IO;
using System.Collections.Generic;


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
                List<Book> bookList = brj.ReadJson(fullPath);

                // b. Запись данных в файл
                brj.WriteJson(bookList);

                // c. Получение объекта по ID
                Book foundBook = brj.GetBookByID(bookList, 2);
                Console.WriteLine($"Найденная книга: {foundBook.Title}");

                // d. Получить список k по счету n объектов класса short
                List<BookPreview> bookPreviewList = brj.ReadShortJson(fullPath);
                brj.Get_K_N_ShortList(bookPreviewList, 5);

                // e. Сортировать элементы по выбранному полю (title)
                brj.SortBooksByTitle(bookList);

                // f. Добавить объект в список
                Book firstBook = new Book("979-5-54360-678-0", "Одиссея", "Гомер", "Эпос");
                bookList = brj.AddBookInList(bookList, firstBook);
                brj.PrintBooksList(bookList);

                // g. Заменить элемент списка по ID
                Book secondBook = new Book("979-5-12363-123-0", "Илиада", "Гомер", "Эпос");
                bookList = brj.ChangeBookByID(bookList, secondBook, 2);

                // h. Удалить элемент списка по ID
                bookList = brj.DeleteBookInList(bookList, firstBook);

                // i. Получить количество элементов
                Console.WriteLine($"Количество элементов в списке bookList: {brj.Get_Count(bookList)}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

        }
    }
}
