using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nirvana.Models
{
    public class Comment
    {
        public ApplicationUser User { get; set; }
        public string UserComment { get; set; }
        public int CommentId { get; set; }
        public DateTime Date { get; set; }
    }
}