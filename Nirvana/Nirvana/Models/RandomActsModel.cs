using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nirvana.Models
{
    public class RandomActsModel
    {
        [Key]
        public ApplicationUser Owner { get; set; }
        public int RandomActId { get; set; }
        public string RandomActTitle { get; set; }
        public string RandomActDescription { get; set; }
        public int PointsEarned { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public DateTime Date { get; set; }
        public string PicURL { get; set; }

        public RandomActsModel()
        {
            Comments = new List<Comment>();
        }
    }
} 
