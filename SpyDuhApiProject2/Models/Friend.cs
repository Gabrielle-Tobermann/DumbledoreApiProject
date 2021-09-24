using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyDuhApiProject2.Models
{
    public class Friend
    {
        public Guid SpyId { get; set; }
        public Guid FriendId { get; set; }
        public Guid RelationshipId { get; set; }
    }
}
