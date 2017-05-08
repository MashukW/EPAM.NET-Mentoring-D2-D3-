using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tasks.AdvancedXML.Tests
{
    [TestClass]
    public class ValidationXmlTests
    {
        private static readonly string _pathToFiles = Directory.GetCurrentDirectory();

        private readonly string _sourceCorrectXmlPath = _pathToFiles + "/Xml/books.Correct.xml";
        private readonly string _sourceIncorrectXmlPath = _pathToFiles + "/Xml/books.Incorrect.xml";
        private readonly string _booksSchemeXsdPath = _pathToFiles + "/Xsd/booksScheme.xsd";
        private readonly string _schemeTargetNamespace = "http://library.by/catalog";

        [TestMethod]
        public void ValidationTest_CorrectXml()
        {
            ValidationXml.Validation(_sourceCorrectXmlPath, _schemeTargetNamespace, _booksSchemeXsdPath);
        }

        [TestMethod]
        public void ValidationTest_IncorrectXml()
        {
            ValidationXml.Validation(_sourceIncorrectXmlPath, _schemeTargetNamespace, _booksSchemeXsdPath);
        }
    }
}
