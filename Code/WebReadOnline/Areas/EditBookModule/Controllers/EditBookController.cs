using ReadOnline.Search.BLL;
using ReadOnline.Search.IBLL;
using ReadOnline.Search.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebReadOnline.Areas.EditBookModule.Controllers
{
    public class EditBookController : Controller
    {
        // GET: EditBookModule/EditBool
        BookCatelogIBLL catelogIBLL = new BookCatelogBLL();
        public ActionResult Index()
        {
            string Bookid = "";
            if (!string.IsNullOrEmpty(Request.QueryString["Bookid"]))
            {
                Bookid = Request.QueryString["Bookid"];
            }
            ViewBag.Bookid = Bookid;
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ContentResult AddCatelog()
        {
            string content = "";
            string booktitle = "";
            string Bookid = "";
            string ParentName = "";
            int sort = 0;
            string pid = "";
            string Id = Guid.NewGuid().ToString();
            if(string.IsNullOrEmpty(Request["ParentName"]))
            {
                ParentName = Request["ParentName"];
            }
            if (!string.IsNullOrEmpty(Request["content"]))
            {
                content = Request["content"];
            }
            if (!string.IsNullOrEmpty(Request["booktitle"]))
            {
                booktitle = Request["booktitle"];
            }
            if (!string.IsNullOrEmpty(Request["pId"]))
            {
                pid = Request["pId"];
            }
            if (!string.IsNullOrEmpty(Request["sort"]))
            {
                sort = Convert.ToInt32(Request["sort"]);
            }
            if (!string.IsNullOrEmpty(Request["Bookid"]))
            {
                Bookid = Request["Bookid"].ToString();
            }
            BookCatelogModel catelogModel = new BookCatelogModel();
            catelogModel.id = Id;
            catelogModel.pId = pid;
            catelogModel.ParentName = ParentName;
            catelogModel.BookId = Bookid;
            catelogModel.name = booktitle;
            catelogModel.BookContent = content;
            catelogModel.sort = sort;

            int result = catelogIBLL.InsertCatelog(catelogModel);
            return Content(result.ToString());
        }
        [HttpPost]
        public JsonResult GetCatelogTree(string bookId)
        {
            IList<BookCatelogModel> booktree = catelogIBLL.GetBookTree(bookId);
            return Json(booktree);
        }
        [HttpGet]
        public ActionResult EditNodeName( string NewNodeName, string Id)
        { 
            int result = catelogIBLL.EditNodeName(NewNodeName, Id);
            return Content(result.ToString());
        }
        [HttpGet]
        public ActionResult DeleteNode(string Id)
        {
   
            int result = catelogIBLL.DeleteNode(Id);
            return Content(result.ToString());
        }
    }
}