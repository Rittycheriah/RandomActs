using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nirvana.Models
{
    public class Likes
    {
        [Key]
        public ApplicationUser User { get; set; }
        public bool Liked { get; set; }
        public int ActId { get; set; }
    }
}