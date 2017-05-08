using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;

namespace Tasks.AdvancedXML
{
    public static class ValidationXml
    {
        public static void Validation(string pathToXml, string schemeTargetNamespace, string pameToScheme)
        {
            if (string.IsNullOrWhiteSpace(pathToXml))
            {
                throw new ArgumentException(nameof(pathToXml));
            }

            XmlReader xmlReader = XmlReader.Create(pathToXml, CreateSettings(schemeTargetNamespace, pameToScheme));

            while (xmlReader.Read())
            {

            }
        }

        private static XmlReaderSettings CreateSettings(string schemeTargetNamespace, string pameToScheme)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(schemeTargetNamespace, pameToScheme);
            settings.ValidationEventHandler +=
                (sender, args) =>
                {
                    Debug.WriteLine("[{0}:{1}] {2}", args.Exception.LineNumber, args.Exception.LinePosition,
                        args.Message);
                };

            settings.ValidationFlags = settings.ValidationFlags | XmlSchemaValidationFlags.ReportValidationWarnings;
            settings.ValidationType = ValidationType.Schema;

            return settings;
        }
    }
}
