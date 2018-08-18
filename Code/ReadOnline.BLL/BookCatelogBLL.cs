using ReadOnline.Search.DAO;
using ReadOnline.Search.IBLL;
using ReadOnline.Search.IDAO;
using ReadOnline.Search.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnline.Search.BLL
{
    public class BookCatelogBLL : BookCatelogIBLL
    {
        IBookCatelogDao catelogDao = new BookCatelogDao();

        public int DeleteNode(string id)
        {
            return catelogDao.DeleteNode(id);
        }

        public int EditNodeName(string newname, string id)
        {
            return catelogDao.EditNodeName(newname, id);
        }

        public IList<BookCatelogModel> GetBookTree(string bookid)
        {
          return  catelogDao.GetBookTree(bookid);
        }

        public BookCatelogModel GetEntity(string id)
        {
            return catelogDao.GetEntity(id);
        }

        public IList<BookCatelogModel> GetSearchResult(string keyvalue, string id)
        {
            return catelogDao.GetSearchResult(keyvalue, id);
        }

        public int InsertCatelog(BookCatelogModel entity)
        {
            return catelogDao.InsertCateLog(entity);
        }
    }
}
