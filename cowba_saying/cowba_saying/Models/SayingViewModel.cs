using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace cowba_saying.Models
{
    public class SayingViewModel
    {
         
        public HttpPostedFileBase file { get; set; }
        public string Saying { get; set; }
        public string Name { get; set; }
    }
}