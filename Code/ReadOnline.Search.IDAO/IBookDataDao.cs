using ReadOnline.Search.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnline.Search.IDAO
{
    public  interface IBookDataDao
    {
        int InsertBook(BookModel bookEntity);
        IList<BookModel> GetBookList();
    }
}
