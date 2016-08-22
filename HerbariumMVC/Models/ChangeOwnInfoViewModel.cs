using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HerbariumMVC.Models
{
    public class ChangeOwnInfoViewModel
    {
        public string fName { get; set; }
        public string lName { get; set; }
        public string password1 { get; set; }
        public string password2 { get; set; }

        public string username { get; set; }
    }
}
