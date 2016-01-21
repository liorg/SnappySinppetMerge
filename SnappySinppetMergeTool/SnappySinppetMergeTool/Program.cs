using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnappySinppetMergeTool
{
    class Program
    {
        static void Main(string[] args)
        {
          var content=  MergeParser.Merge(@"merge\css.css", @"merge\template.html");
            File.WriteAllText(@"merge\output.html",content);
        }
    }
}
