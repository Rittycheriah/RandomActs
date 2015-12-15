using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Nirvana.Models
{
    public class Rank
    {
        [Key]
        public int Rank_Id { get; set; }
        public int Rank_Code { get; set; }
        public string Name { get; set; }
        public bool CommentFeat { get; set; }
        public bool SocialMedia { get; set; }
        public int BasePtsAllowance { get; set; }
        public int MinimumPtReq { get; set; }

        [Required]
        public ApplicationUser User { get; set; }
    }
}