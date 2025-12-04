using System;
using System.Collections.Generic;

namespace Library
{
    using System.Linq;

    public class FilterDecorator : Book_rep_decorator
    {
        private readonly Func<BookPreview, bool> filter;

        public FilterDecorator(IBookRepository repo, Func<BookPreview, bool> filterFunction) : base(repo)
        {
            filter = filterFunction;
        }

        public override List<BookPreview> Get_K_N_ShortList(int n)
        {
            var page = base.Get_K_N_ShortList(n);
            var filtered = page.Where(filter).ToList();
            return filtered;
        }

        public override int Get_Count()
        {
            var allBooks = wrapper.Get_K_N_ShortList(1_000_000);
            return allBooks.Where(filter).Count();
        }
    }

}
