using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpyDuhApiProject2.Models
{
    public class Skill
    {
        public string Name { get; set; }
        public Guid SpyId { get; set; }
        public Guid SkillId { get; set; }
    }
}
