using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Nirvana.Models
{
    public class RankDefinitions
    {
        [Key]
        public int RankingCode { get; set; }
        public string RankingName { get; set;  }
        public bool RankingComments { get; set; }
        public bool RankingSocial { get; set; }
        public int RankingBasePts { get; set;}
        public int RankingMinPt { get; set; }
    }
}