using ReadOnline.Search.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReadOnline.Search.BLL;
using ReadOnline.Search.Model;

namespace WebReadOnline.Controllers
{
    public class HomeController : Controller
    {
        private BookDataIBLL bookData = new BookDataBLL();
        public ActionResult Index()
        {
            IList<BookModel> bookList=bookData.GetBookList();
            ViewBag.bookList = bookList;
            return View();
        }
        public ActionResult InsertBook(string bookname)
        {
  
            if (string.IsNullOrEmpty(bookname))
            {
                return Json(-1,JsonRequestBehavior.AllowGet);
            }else
            {
                BookModel bm = new BookModel();
                bm.Id = Guid.NewGuid().ToString();
                bm.BookName= bookname;
                bm.CreateTime = DateTime.Now;
                return Json(bookData.InsertBook(bm),JsonRequestBehavior.AllowGet);
            }
           
        }
    }
}