using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzWinApp_Web.Models
{
    public class TestItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}



