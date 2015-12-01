using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nirvana.Models
{
    public class Comment
    {
        [Key]
        public ApplicationUser User { get; set; }
        public string UserComment { get; set; }
        public int CommentId { get; set; }
        public DateTime Date { get; set; }
        public int ActId { get; set; }
    }
}