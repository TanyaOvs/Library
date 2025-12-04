using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    public class Book_rep
    {
        protected List<Book> bookList;
        protected List<BookPreview> bookPreviewList;

        public List<Book> BookList 
        {
            get {return bookList; }
        }

        public List<BookPreview> BookPreviewList
        {
            get { return bookPreviewList; }
        }

        public Book_rep() 
        {
            bookList = new List<Book>();
            bookPreviewList = new List<BookPreview>();
        }

        public virtual Book GetBookByID(int bookID)
        {
            if (bookList == null || bookList.Count == 0)
                throw new ArgumentException("Список книг пуст или не инициализирован.");

            // Ищем книгу с помощью метода LINQ, который возвращает первый элемент, удовлетворяющий условию с bookID
            Book foundBook = bookList.FirstOrDefault(book => book.BookID == bookID);

            if (foundBook == null)
                throw new ArgumentException($"Книга с ID={bookID} не найдена в списке.");

            return foundBook;
        }
        public void PrintBooksList<T>(List<T> bookList) where T : Book
        {
            if (bookList == null || bookList.Count == 0)
            {
                Console.WriteLine("Список книг пуст.");
                return;
            }

            string separator = "-------------------------";
            Console.WriteLine(separator);
            Console.WriteLine("Список книг:");

            foreach (var book in bookList)
            {
                Console.WriteLine(separator);
                book.PrintFullInfo();
            }

            Console.WriteLine(separator);
        }

        public virtual List<BookPreview> Get_K_N_ShortList(int n)
        {
            const int pageSize = 5; // Количество объектов на одной "странице"

            if (bookPreviewList == null || bookPreviewList.Count == 0)
                throw new ArgumentException("Список книг пуст или не инициализирован!");

            if (n <= 0)
                throw new ArgumentException("Номер страницы должен быть положительным числом!");

            // Вычисляем, сколько элементов нужно пропустить, чтобы начать с нужной "страницы"
            int skipCount = (n - 1) * pageSize;

            if (skipCount >= bookPreviewList.Count)
                throw new ArgumentException("Номер страницы выходит за пределы списка книг!");

            List<BookPreview> result = bookPreviewList.Skip(skipCount).Take(pageSize).ToList();
            PrintBooksList(result);
            return result;
        }

        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (!char.IsDigit(c))
                    return false;
            }
            return !string.IsNullOrEmpty(str);
        }

        public void SortBooksByTitle()
        {
            if (bookList == null || bookList.Count == 0)
            {
                Console.WriteLine("Список пуст или не существует.");
                return;
            }

            for (int i = 0; i < bookList.Count - 1; i++)
            {
                for (int j = i + 1; j < bookList.Count; j++)
                {
                    Book book1 = bookList[i];
                    Book book2 = bookList[j];

                    bool isDigits1 = IsDigitsOnly(book1.Title);
                    bool isDigits2 = IsDigitsOnly(book2.Title);

                    // Если обе книги содержат только цифры в названии, то сортируем как числа
                    if (isDigits1 && isDigits2)
                    {
                        long num1 = long.Parse(book1.Title);
                        long num2 = long.Parse(book2.Title);

                        if (num1 > num2)
                            (bookList[i], bookList[j]) = (bookList[j], bookList[i]);
                    }

                    // Если название первой книги содержит только цифры, то она идет раньше в списке
                    else if (isDigits1 && !isDigits2)
                        (bookList[i], bookList[j]) = (bookList[j], bookList[i]);

                    // Если название второй книги содержит только цифры, то она идёт позже в списке
                    else if (!isDigits1 && isDigits2)
                        continue;

                    // Если названия обеих книг строковые, то выполняем обычную лексикографическую сортировку
                    else if (string.Compare(book1.Title, book2.Title, StringComparison.OrdinalIgnoreCase) > 0)
                        (bookList[i], bookList[j]) = (bookList[j], bookList[i]);
                }
            }

            Console.WriteLine("Список книг успешно отсортирован по названию!");
            PrintBooksList(bookList);
        }

        public virtual void AddBookInList(Book book)
        {
            foreach (Book bookElem in bookList)
            {
                if (Book.CompareBooks(book, bookElem))
                {
                    throw new ArgumentException($"Нельзя добавить книгу {book.Title}: книга с таким же ISBN уже находится в списке!");
                }
            }
            bookList.Add(book);
            Console.WriteLine($"Книга '{book.Title}' добавлена в список!");
        }

        public virtual void ChangeBookByID(Book book, int bookID)
        {
            if (bookID < 0 || bookID > bookList.Max(b => b.BookID))
                throw new ArgumentException("ID выходит за пределы списка bookList!");

            foreach (Book bookElem in bookList)
            {
                if (Book.CompareBooks(book, bookElem))
                {
                    throw new ArgumentException($"Нельзя добавить книгу {book.Title}: книга с таким же ISBN уже находится в списке!");
                }
            }

            foreach (Book bookElem in bookList)
            {
                if (bookElem.BookID == bookID)
                {
                    bookList[bookList.IndexOf(bookElem)] = book;
                    Console.WriteLine($"Книга '{bookElem.Title}' изменилась на '{book.Title}'.");
                    break;
                }
            }
        }

        public virtual void DeleteBookInList(int bookID)
        {
            if (bookID < 0 || bookID > bookList.Max(b => b.BookID))
                throw new ArgumentException("ID выходит за пределы списка bookList!");

            foreach (Book bookElem in bookList)
            {
                if (bookElem.BookID == bookID)
                {
                    bookList.Remove(bookElem);
                    Console.WriteLine($"Книга '{bookElem.Title}' удалена из списка!");
                    break;
                }
            }
        }

        public virtual int Get_Count()
        {
            return bookList.Count;
        }
    }
}
