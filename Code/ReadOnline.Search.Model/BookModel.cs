using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadOnline.Search.Model
{
   public class BookModel
    {
        public string Id { get; set; }
        public string BookName { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
