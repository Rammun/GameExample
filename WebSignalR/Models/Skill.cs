using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSignalR.Models
{
    public class Skill
    {
        public string Name { get; set; }
        public int costHP { get; set; }
        public int costMP { get; set; }
        public int LowDamage { get; set; }
        public int HighDamage { get; set; }
    }
}