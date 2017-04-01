using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp
{
    public class todo
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public todo() { }
        public todo(string title, string desc)
        {
            Title = title;
            Desc = desc;
        }
    }
}
