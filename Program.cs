using System;
using System.IO;


namespace Library
{
    class Program
    {
        static void Main(string[] args)
        { 
            // Работа с YAML
            try
            {
                // Чтение объектов класса Book в List на основе yaml 
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "books.yaml");

                // 1. Объект для работы с yaml
                Book_rep_yaml bry = new Book_rep_yaml();

                // a. Чтение всех значений из yaml (создание списка книг)
                bry.ReadYaml(fullPath);

                // b. Запись данных в файл
                bry.WriteYaml();

                // c. Получение объекта по ID
                Book foundBook = bry.GetBookByID(5);
                Console.WriteLine($"Найденная книга: {foundBook.Title}");

                // d. Получить список k по счету n объектов класса short
                bry.ReadShortYaml(fullPath);
                bry.Get_K_N_ShortList(2);

                // e. Сортировать элементы по выбранному полю (title)
                bry.SortBooksByTitle();

                // f. Добавить объект в список
                Book firstBook = new Book("979-5-54360-678-0", "Одиссея", "Гомер", "Эпос");
                bry.AddBookInList(firstBook);
                bry.PrintBooksList(bry.BookList);

                // g. Заменить элемент списка по ID
                Book secondBook = new Book("979-5-12363-123-0", "Илиада", "Гомер", "Эпос");
                bry.ChangeBookByID(secondBook, 2);

                // h. Удалить элемент списка по ID
                bry.DeleteBookInList(9);

                // i. Получить количество элементов
                Console.WriteLine($"Количество элементов в списке bookList: {bry.Get_Count()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
