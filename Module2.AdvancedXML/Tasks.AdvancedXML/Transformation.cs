using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Tasks.AdvancedXML
{
    public class Transformation
    {
        public static void TransformToRss(string sourceXml, string pathToXsltTemplate, string pathToResultFile)
        {
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(pathToXsltTemplate);

            FileStream fileStream = new FileStream(pathToResultFile, FileMode.Create);

            xslt.Transform(sourceXml, new XsltArgumentList(), fileStream);
        }

        public static void TransformToHtmlReport(string pathToXsltTemplate, Stream input, Stream output,
            Dictionary<string, object> parameters)
        {
            #region CheckArguments

            if (string.IsNullOrWhiteSpace(pathToXsltTemplate))
            {
                throw new ArgumentException(nameof(pathToXsltTemplate));
            }

            if (input == null)
            {
                throw new ArgumentException(nameof(input));
            }

            if (output == null)
            {

                throw new ArgumentException(nameof(output));
            }

            if (parameters == null)
            {
                throw new ArgumentException(nameof(parameters));
            }

            #endregion
            
            var xsltSettings = new XsltSettings
            {
                EnableScript = true
            };

            XslCompiledTransform xslt = new XslCompiledTransform();
            XmlReader xmlReader = XmlReader.Create(pathToXsltTemplate);
            xslt.Load(xmlReader, xsltSettings, null);

            XPathDocument document = new XPathDocument(input);
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = false,
                Indent = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                CloseOutput = false
            };

            XmlWriter writer = XmlWriter.Create(output, xmlWriterSettings);
            XsltArgumentList xsltArgumentList = GetArgumentList(parameters);
            xslt.Transform(document, xsltArgumentList, writer);
        }

        private static XsltArgumentList GetArgumentList(Dictionary<string, object> parameters)
        {
            XsltArgumentList argumentList = new XsltArgumentList();

            foreach (var parameter in parameters)
            {
                argumentList.AddParam(parameter.Key, "", parameter.Value);
            }

            return argumentList;
        }
    }
}
