using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Nirvana.Models
{
    public class NirvanaContext
    {
        public virtual DbSet<RandomActsModel> Acts { get; set; }
        public virtual IDbSet<Comment> Comments { get; set; }
        public virtual IDbSet<Rank> Ranks { get; set; }
        public virtual IDbSet<Likes> Likes { get; set; }
    }
}