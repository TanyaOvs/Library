using System.Collections.Generic;

namespace Library
{
    public interface IBookRepository
    {
        List<BookPreview> Get_K_N_ShortList(int n);
        int Get_Count();
    }

    public class Book_rep_adapter : IBookRepository
    {
        private readonly Book_rep_DB inner;

        public Book_rep_adapter(Book_rep_DB db)
        {
            inner = db;
        }

        public List<BookPreview> Get_K_N_ShortList(int n)
        {
            return inner.Get_K_N_ShortList(n);
        }

        public int Get_Count()
        {
            return inner.Get_Count();
        }
    }

    public class Book_rep_file_adapter : IBookRepository
    {
        private readonly Book_rep inner;

        public Book_rep_file_adapter(Book_rep br)
        {
            inner = br;
        }

        public List<BookPreview> Get_K_N_ShortList(int n)
        {
            return inner.Get_K_N_ShortList(n);
        }

        public int Get_Count()
        {
            return inner.Get_Count();
        }
    }
}
