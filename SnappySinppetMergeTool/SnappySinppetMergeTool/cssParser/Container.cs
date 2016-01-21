/*
*
* Date Created: 02-Sep-2008
* Author:       SAHAY, Sanjeet, IDC
* Filename:     Container.cs
* Assembly:     Sanjeet.Demos.CSSParser
* Project:      CSSParser



* Purpose:      Add purpose
*
*/
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Sanjeet.Demos.CSSParser
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Container : IContainer
    {
        private static string styleText;
        protected string styleSheetPath = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="styleSheetPath"></param>
        protected Container(string styleSheetPath)
        {
            this.styleSheetPath = styleSheetPath;
            switch (styleText)
            {
                case null:
                    Debug.WriteLine("StyleText From Disk");
                    styleText = this.GetStylesText();
                    break;
            }
        }

        #region IContainer Members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IDictionary<string, string> GetProperties(string name)
        {
            //break the expression into a string that only contains a specific class/element/elementID name
            //e.g. .H1{color: blue; font-style: italic;}
            // string pattern = @"(" + name + @").*?\}";
            // fixed for id l.g
            string pattern = @"(" + name + @").*?\}";
            MatchCollection collection = Util.GetMatchesFromString(styleText, pattern);
            return this.FillDictionary(collection, name);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetStylesText()
        {
            StreamReader reader = new StreamReader(this.styleSheetPath);
            string styleSheetText;
            using (reader)
            {
                styleSheetText = reader.ReadToEnd();
            }

            return styleSheetText;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        private IDictionary<string, string> FillDictionary(MatchCollection collection, string uniqeName = "")
        {
            StringBuilder matchValue = new StringBuilder();
            foreach (Match match in collection)
            {
                if (match.Success)
                {
                  
                    var patternWholeWord = @"(?:^|\W)" + uniqeName + @"(?:$|\W)";
                    Regex reg = new Regex(patternWholeWord);
                    if ( reg.IsMatch(match.Value))
                        matchValue.Append(match.Value);
                }
            }
            return this.PopulateProperties(matchValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="matchValue"></param>
        /// <returns></returns>
        private IDictionary<string, string> PopulateProperties(StringBuilder matchValue)
        {
            //break the expression into a string that doesn't contain {}
            MatchCollection properitesSection = Util.GetMatchesFromString(matchValue.ToString(), @"(?<=\{).*?(?=\})");
            IDictionary<string, string> propeties = new Dictionary<string, string>();
            foreach (Match match in properitesSection)
            {
                if (match.Success)
                {
                    //break the expression into a string[] containing each line of property value
                    //e.g. font-family: 'Segoe UI';
                    string[] propertiesPart = match.Value.Split(';');
                    if (propertiesPart.Length > 0)
                    {
                        for (int i = 0; i < propertiesPart.Length; i++)
                        {
                            //break the expression into a string[] containing attribute values
                            //e.g. font-family and 'Segoe UI'
                            string[] attributes = propertiesPart[i].Split(':');
                            if (attributes.Length == 2)
                            {
                                if (!propeties.ContainsKey(attributes[0].Trim()))
                                    propeties.Add(attributes[0].Trim(), attributes[1].Trim().TrimEnd(';'));
                            }
                            //l.g fix for url background
                            if (attributes.Length == 4)
                            {

                                if (!propeties.ContainsKey(attributes[0].Trim()) && attributes[0].Trim() == "background")
                                    propeties.Add(attributes[0].Trim(), attributes[1].Trim() + " " + attributes[2].Trim() + " " + attributes[3].Trim());
                            }
                        }
                    }
                }
            }
            return propeties;
        }
    }
}