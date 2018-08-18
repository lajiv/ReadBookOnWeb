using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.IDatabase;
using DbRepository;
using ReadOnline.Search.IDAO;
using ReadOnline.Search.Model;

namespace ReadOnline.Search.DAO
{
    public class BookDataDao : IBookDataDao
    {
        IGetDatabase db = DbFactory.GetDatabase();
        public int InsertBook(BookModel BookEntity)
        {
            string insertSql = $"insert into BookList(Id,BookName,CreateTime) values('{BookEntity.Id}','{BookEntity.BookName}','{BookEntity.CreateTime}')";
            return db.ExecuteNoneQuery(insertSql);
        }
        public  IList<BookModel> GetBookList()
        {
            string sql = $@"select * from BookList";
            return db.FindList<BookModel>(sql).ToList();
        }
    }
}
