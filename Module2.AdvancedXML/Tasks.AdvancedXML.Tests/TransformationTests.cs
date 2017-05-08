using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;

namespace Tasks.AdvancedXML.Tests
{
    [TestClass]
    public class TransformationTests
    {
        private static readonly string _pathToFiles = Directory.GetCurrentDirectory();
        
        private readonly string _sourceCorrectXmlPath = _pathToFiles + "/Xml/books.Correct.xml";
        private readonly string _toHtmlReportsXsltPath = _pathToFiles + "/Xslt/ToHtmlReports.xslt";
        private readonly string _toRssTransformXsltPath = _pathToFiles + "/Xslt/ToRssTransform.xslt";
        private readonly string _resultFileForRss = _pathToFiles + "/Result/rss.xml";
        private readonly string _resultFileForHtmlReport = _pathToFiles + "/Result/report.html";

        [TestMethod]
        public void TransformToRssTest()
        {
            Transformation.TransformToRss(_sourceCorrectXmlPath, _toRssTransformXsltPath, _resultFileForRss);
        }

        [TestMethod]
        public void TransformToHtmlReportTest()
        {
            FileStream output = new FileStream(_resultFileForHtmlReport, FileMode.Create);
            FileStream input = new FileStream(_sourceCorrectXmlPath, FileMode.Open, FileAccess.Read);

            var xsltParameters = new Dictionary<string, object>()
            {
                { "Date", DateTime.Now.ToString("f") }
            };

            Transformation.TransformToHtmlReport(_toHtmlReportsXsltPath, input, output, xsltParameters);
        }
    }
}
