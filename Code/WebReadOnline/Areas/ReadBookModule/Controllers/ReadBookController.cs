using ReadOnline.Search.BLL;
using ReadOnline.Search.IBLL;
using ReadOnline.Search.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebReadOnline.Areas.ReadBookModule.Controllers
{
    public class ReadBookController : Controller
    {
        BookCatelogIBLL catelogIBLL = new BookCatelogBLL();
        // GET: ReadBookModule/ReadBook
        public ActionResult Index()
        {
            string Bookid = Request.QueryString["Bookid"];
            ViewBag.Bookid = Bookid;
            IList<BookCatelogModel> booktree = catelogIBLL.GetBookTree(Bookid);
            ViewBag.bookTreeList = Newtonsoft.Json.JsonConvert.SerializeObject(booktree);
            return View();
        }
        public ActionResult GetSearchResult(string keyValue,  string bookid)
        {
            IList<BookCatelogModel> searchResult = catelogIBLL.GetSearchResult(keyValue, bookid);
            if (searchResult != null)
            {
                
                searchResult = GetRedFont(searchResult,keyValue);
            }
            ViewBag.searchResult = searchResult;
            ViewBag.KeyValue = keyValue;
            return PartialView("SearchTitle");
        }
        [HttpPost]
        public ActionResult GetKeyWordsContent(IList<BookCatelogModel> treeAry, string keyvalue)
        {
            IList<BookCatelogModel> newTreeAry = new List<BookCatelogModel>();
            foreach (BookCatelogModel item in treeAry)
            {
               item.BookContent=DKeyWordRed(item.BookContent, keyvalue);
            }
            return Content(Newtonsoft.Json.JsonConvert.SerializeObject(treeAry));
        }
        public IList<BookCatelogModel> GetRedFont(IList<BookCatelogModel> searchResult,string keyvalue)
        {
            int frontlen = 13;
            int backlen = 17;
            foreach (BookCatelogModel item in searchResult)
            {
                item.BookContent=DCharSign(item.BookContent, keyvalue);
                item.BookContent = ReplaceHtmlTag(item.BookContent);
                int begin = item.BookContent.IndexOf("###");
                int end = item.BookContent.IndexOf("$$$");
                if ((end + 3 + backlen) > item.BookContent.Length)
                {
                    end = end + 3;
                }
                else
                {
                    while (item.BookContent[end + 3 + backlen] == '$')//向后取15个字符可能会出现为$的情况
                    {
                        end = end + 1;
                    }
                }
                if (begin <= frontlen)
                {
                    item.BookContent = item.BookContent.Length > (end + 3 + backlen) ? item.BookContent.Substring(0, end + 3) : item.BookContent;
                }
                else
                {
                    while (item.BookContent[begin - frontlen] == '$')//向后取15个字符可能会出现为$的情况
                    {
                        begin = begin + 1;
                    }
                    item.BookContent = item.BookContent.Length > (end + 3 + backlen) ? item.BookContent.Substring(begin - frontlen, end - begin + 3 + frontlen + backlen) : item.BookContent;
                }

                if ((!String.IsNullOrEmpty(item.BookContent)) && item.BookContent.Contains("###") && item.BookContent.Contains("$$$"))//命中词汇标红
                {
                    item.BookContent = item.BookContent.Replace("###", "<span style=\"color:red;margin: 0px\">");
                    item.BookContent = item.BookContent.Replace("$$$", "</span>");
                    //isChange = true;
                }
            }
            return searchResult;
        }
        private string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
        private string DKeyWordRed(string content,string keyvalue)
        {
            if (!string.IsNullOrEmpty(content))
                return content.Replace(keyvalue, "<span style=\"color:red;margin: 0px\">" + keyvalue + "</span>");
            else
                return content;
        }
        /// <summary>
        /// 对关键字进行$$$ ###标记
        /// </summary>
        /// <param name="content"></param>
        /// <param name="keyvalue"></param>
        /// <returns></returns>
        private string DCharSign(string content ,string keyvalue)
        {
            if (!string.IsNullOrEmpty(content))
                return content.Replace(keyvalue, "###" + keyvalue + "$$$");
            else
                return content;
        }
    }
}