using System;
using System.Collections.Generic;
using System.Linq;


namespace Library
{
    public class SortDecorator : Book_rep_decorator
    {
        private readonly Func<BookPreview, object> sorter;

        public SortDecorator(IBookRepository repo, Func<BookPreview, object> sorterFunc) : base(repo)
        {
            sorter = sorterFunc;
        }

        public override List<BookPreview> Get_K_N_ShortList(int n)
        {
            var page = base.Get_K_N_ShortList(n);
            var sorted = page.OrderBy(sorter).ToList();
            return sorted;
        }
    }

}
