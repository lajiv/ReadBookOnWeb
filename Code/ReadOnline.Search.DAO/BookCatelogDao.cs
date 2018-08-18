using Common.IDatabase;
using DbRepository;
using ReadOnline.Search.IDAO;
using ReadOnline.Search.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnline.Search.DAO
{
    public class BookCatelogDao : IBookCatelogDao
    {
        IGetDatabase db = DbFactory.GetDatabase();
        private string dbField = "id,Pid,bookid,ParentName,name,sort,bookcontent";
        private string tableName = "BookCatalog";


        public int InsertCateLog(BookCatelogModel entity)
        {
            string sql = $@"insert into 
                                {tableName}({dbField}) 
                                     values('{entity.id}','{entity.pId}','{entity.BookId}','{entity.ParentName}','{entity.name}','{entity.sort}','{entity.BookContent}')";
            return db.ExecuteNoneQuery(sql);
        }
        public IList<BookCatelogModel> GetBookTree(string bookid)
        {
            string sql = $@"select * from {tableName} where bookid='{bookid}' order by sort";
            return db.FindList<BookCatelogModel>(sql).ToList();
        }

        public int EditNodeName(string newname,string id)
        {
            string sql = $"update {tableName} set name='{newname}' where id='{id}'";
            return db.ExecuteNoneQuery(sql);
        }

        public int DeleteNode(string id)
        {
            string sql = $"delete from {tableName} where id='{id}'";
            return db.ExecuteNoneQuery(sql);
        }
        public  IList<BookCatelogModel> GetSearchResult(string keyvalue, string bookid)
        {
            string sql = $"select * from {tableName} where bookid='{bookid}' and bookcontent like '%{keyvalue}%'";
            return db.FindList<BookCatelogModel>(sql).ToList();
        }

        public BookCatelogModel GetEntity(string id)
        {
            string sql = $"select * from {tableName} where id='{id}'";
            return db.FindEntity<BookCatelogModel>(sql);
        }
    }
}
