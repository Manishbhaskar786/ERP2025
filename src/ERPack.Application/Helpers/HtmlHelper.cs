using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPack.Helpers
{
    public class HtmlHelper
    {
        /// <summary>
        /// Returns first occurence of Inner Html Value of HTML Page matcing XPath tag
        /// </summary>
        /// <param name="sHtml">String containing HTML Data</param>
        /// <param name="sXPath">String containing XPath value to parse Html</param>
        /// <param name="iIndex">Index of the Node to return in case of Multiple node found</param>
        /// <returns>Returns InnerTML value on Successfull, otherwise blank</returns>
        public static string GetNodeHtml(string sHtml, string sXPath, int iIndex)
        {
            try
            {
                HtmlDocument hDoc = new HtmlDocument();
                hDoc.LoadHtml(sHtml);
                return hDoc.DocumentNode.SelectNodes(sXPath)[iIndex].InnerHtml;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// Returns all occurence of Inner Html Value of HTML Page matching XPath tag.
        /// </summary>
        /// <param name="sHtml">String containing HTML Data</param>
        /// <param name="sXPath">String containing XPath value to parse Html</param>
        /// <returns>Returns collection of InnerTML value on Successfull, otherwise null</returns>
        public static HtmlNodeCollection GetNodeHtmlCollection(string sHtml, string sXPath)
        {
            try
            {
                HtmlDocument hDoc = new HtmlDocument();
                hDoc.LoadHtml(sHtml);
                return hDoc.DocumentNode.SelectNodes(sXPath);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
