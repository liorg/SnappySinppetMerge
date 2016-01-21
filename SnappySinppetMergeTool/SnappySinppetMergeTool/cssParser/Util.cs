/*
*
* Date Created: 02-Sep-2008
* Author:       SAHAY, Sanjeet, IDC
* Filename:     Util.cs
* Assembly:     Sanjeet.Demos.CSSParser
* Project:      CSSParser



* Purpose:      Add purpose
*
*/
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace Sanjeet.Demos.CSSParser
{
    public static class Util
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static MatchCollection GetMatches(string fileName, string pattern)
        {
            StreamReader reader = null;
            MatchCollection collection = null;
            try
            {
                reader = new StreamReader(fileName);
                string fileText;
                lock (new object())
                {
                    using (reader)
                    {
                        fileText = reader.ReadToEnd();
                    }
                }
                collection = GetMatchesFromString(fileText, pattern);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
            }
            return collection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static MatchCollection GetMatchesFromString(string input, string pattern)
        {
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return regex.Matches(input);
        }
    }
}