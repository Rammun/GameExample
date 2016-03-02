using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSignalR.Models
{
    public class UserBattlesViewModel
    {
        public UserParam UserParam1 { get; set; }
        public UserParam UserParam2 { get; set; }
        public List<UserParam> Users { get; set; }
    }
}