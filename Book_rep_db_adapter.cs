using System;
using System.Collections.Generic;

namespace Library
{
    public class Book_rep_db_adapter : Book_rep
    {
        private Book_rep_DB brdb_adapter;
        public Book_rep_db_adapter(Book_rep_DB brdb)
        {
            brdb_adapter = brdb;
        }

        public override Book GetBookByID(int bookID)
        {
            return brdb_adapter.GetBookByID(bookID);
        }

        public override void Get_K_N_ShortList(int n)
        {
            List<BookPreview> bpList = brdb_adapter.Get_K_N_ShortList(n);

            string beautifulSeparator = "-------------";
            Console.WriteLine(beautifulSeparator);
            Console.WriteLine("Список книг: ");

            foreach (BookPreview bp in bpList)
            {
                Console.WriteLine(beautifulSeparator);
                bp.PrintShortInfo();
            }
            Console.WriteLine(beautifulSeparator);
        }

        public override void AddBookInList(Book book)
        {
            brdb_adapter.AddBook(book);
        }

        public override void ChangeBookByID(Book book, int bookID)
        {
            brdb_adapter.UpdateBook(book, bookID);
        }

        public override void DeleteBookInList(int bookID)
        {
            brdb_adapter.DeleteBook(bookID);
        }

        public override int Get_Count()
        {
            return brdb_adapter.Get_Count();
        }
    }
}
