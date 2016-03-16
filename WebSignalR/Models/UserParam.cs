using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSignalR.Models
{
    public class UserParam
    {
        public string Id;
        public string Name;
        public int HP = 1000;
        public int MP = 2000;
        public bool Busy = false;

        public Dictionary<string, int[]> skills;
        //public List<string> skills;

        public UserParam()
        {
            skills = new Dictionary<string, int[]>();
            skills.Add("hit", new int[] { 10, 15, 5 });
            skills.Add("magic", new int[] { 15, 25, 20 });
        }

        public UserParam Clone()
        {
            return new UserParam
            {
                Id = this.Id,
                Name = this.Name,
                HP = this.HP,
                MP = this.MP,
                Busy = this.Busy,
                skills = this.skills
            };
        }
    }
}