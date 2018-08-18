using ReadOnline.Search.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadOnline.Search.Model;
using ReadOnline.Search.IDAO;
using ReadOnline.Search.DAO;

namespace ReadOnline.Search.BLL
{
    public class BookDataBLL : BookDataIBLL
    {
        IBookDataDao bookDao = new BookDataDao();
        public IList<BookModel> GetBookList()
        {
           return bookDao.GetBookList();
        }

        public int InsertBook(BookModel BookEntity)
        {
            return bookDao.InsertBook(BookEntity);
        }
    }
}
