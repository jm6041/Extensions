using DocumentFormat.OpenXml.Packaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace OpenXml.Extensions
{
    /// <summary>
    /// OpenXml扩展
    /// </summary>
    public static class OpenXmlExtension
    {
        /// <summary>
        /// OpenXml转换为<see cref="XDocument"/>对象
        /// </summary>
        /// <param name="part">OpenXml</param>
        /// <returns><see cref="XDocument"/>对象</returns>
        public static XDocument GetXDocument(this OpenXmlPart part)
        {
            XDocument xdoc = part.Annotation<XDocument>();
            if (xdoc != null)
            {
                return xdoc;
            }
            using (StreamReader sr = new StreamReader(part.GetStream()))
            {
                using (XmlReader xr = XmlReader.Create(sr))
                {
                    xdoc = XDocument.Load(xr);
                }
            }
            part.AddAnnotation(xdoc);
            return xdoc;
        }
    }
}
