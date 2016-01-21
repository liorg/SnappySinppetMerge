using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnappySinppetMergeTool
{
    class Program
    {
        static void Main(string[] args)
        {
            MergeParser.Merge(@"merge\css.css", @"merge\template.html");
        }
    }
}
