using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnline.Search.Model
{
    public class BookCatelogModel
    {
        /// <summary>
        /// 目录主键
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 上级目录主键
        /// </summary>
        public string pId { get; set; }
        /// <summary>
        /// 上级标题名称
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// 书籍Id
        /// </summary>
        public string BookId { get; set; }
        /// <summary>
        /// 书的标题
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 书内容
        /// </summary>
        public string BookContent { get; set; }
        /// <summary>
        /// 排序  
        /// </summary>
        public int sort { get; set; }
    }
}
