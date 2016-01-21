using System.Collections.Generic;

namespace Sanjeet.Demos.CSSParser
{
    public interface IContainer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDictionary<string, string> GetProperties(string name);
        string GetStylesText();
    }
}