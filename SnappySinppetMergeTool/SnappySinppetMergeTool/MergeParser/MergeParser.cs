using HtmlAgilityPack;
using Sanjeet.Demos.CSSParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SnappySinppetMergeTool
{
    public class MergeParser
    {
        public static string Merge(string cssFilepath,string htmlFilePath)
        {
            string body = "";
            Regex reg = new Regex(@"\#\w+");
            IContainer container = new DefaultContainer(cssFilepath);
            var alltext = container.GetStylesText();
            MatchCollection itemsids = reg.Matches(alltext);
            Dictionary<string, IDictionary<string, string>> css = new Dictionary<string, IDictionary<string, string>>();
            foreach (Match itemsid in itemsids)
            {
                var ddd = itemsid.ToString();
                var idname = ddd.Substring(1, ddd.Length - 1).ToString();
                if (!css.ContainsKey(idname))
                {
                    var data = container.GetProperties(idname);
                    css.Add(idname, data);
                }
                Console.WriteLine(itemsid);
            }

            HtmlDocument html = new HtmlDocument();


             body = File.ReadAllText(htmlFilePath);
            html.LoadHtml(body);

            foreach (var item in html.DocumentNode.DescendantNodes())
            {
                if (String.IsNullOrEmpty(item.Id)) continue;
                foreach (var cssIdKey in css)
                {
                    if (item.Id == cssIdKey.Key)
                    {
                        StringBuilder sb = new StringBuilder();
                        foreach (var cssValue in cssIdKey.Value)
                          sb.AppendFormat("{0}:{1};", cssValue.Key, cssValue.Value.Replace("\"", "'"));
                        
                        item.Attributes.Add("style", sb.ToString());
                    }
                }
            }
            using (StringWriter wr = new StringWriter())
            {
                html.Save(wr);
                body = wr.ToString();
            }
            return body;
        }
    }
}
