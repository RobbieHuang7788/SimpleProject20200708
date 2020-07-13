using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SimpleProject20200708.Models;

namespace SimpleProject20200708.Models
{
    public class FromDBViewModel
    {
        public List<Student> FirstOrder { get; set; }
        public List<Student> SecondOrder { get; set; }

        public SelectList Grade { get; set; }
    }
}