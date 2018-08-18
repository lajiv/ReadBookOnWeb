using ReadOnline.Search.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnline.Search.IDAO
{
    public interface IBookCatelogDao
    {
        int InsertCateLog(BookCatelogModel entity);
        IList<BookCatelogModel> GetBookTree(string bookid);
        int EditNodeName(string newname,string id);
        int DeleteNode(string id);
        IList<BookCatelogModel> GetSearchResult(string keyvalue, string bookid);
        BookCatelogModel GetEntity(string id);
    }
}
