using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyDuhApiProject2.Models
{
    public class Enemy
    {
        public Guid SpyId { get; set; }
        public Guid EnemyId { get; set; }
        public Guid RelationshipId { get; set; }
    }
}
