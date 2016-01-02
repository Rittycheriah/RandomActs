using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nirvana.Models
{
    public class Comment
    {
        public virtual ApplicationUser User { get; set; }

        [Key]
        public int CommentId { get; set; }
        public string UserComment { get; set; }
        public DateTime Date { get; set; }
        public int ActId { get; set; }
    }
}