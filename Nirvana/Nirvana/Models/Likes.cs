using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Nirvana.Models
{
    public class Likes
    {
        // until a person likes something, a like does not exist
        // unlike ==s delete
        [Required]
        public ApplicationUser User { get; set; }
        public RandomActsModel Act { get; set; }

        [Key]
        public int LikeId { get; set; }
    }
}