using ReadOnline.Search.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnline.Search.IBLL
{
    public interface BookDataIBLL
    {
        int InsertBook(BookModel BookEntity);
        IList<BookModel> GetBookList();
    }
}
