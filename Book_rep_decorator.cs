using System.Collections.Generic;

namespace Library
{
    public abstract class Book_rep_decorator : IBookRepository
    {
        protected IBookRepository wrapper;

        public Book_rep_decorator(IBookRepository repo)
        {
            wrapper = repo;
        }

        public virtual List<BookPreview> Get_K_N_ShortList(int n)
        {
            return wrapper.Get_K_N_ShortList(n);
        }

        public virtual int Get_Count()
        {
            return wrapper.Get_Count();
        }
    }

}
