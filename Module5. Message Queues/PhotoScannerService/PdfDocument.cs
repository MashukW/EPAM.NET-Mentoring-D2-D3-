using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;

namespace PhotoScannerService
{
    public sealed class PdfDocument
    {
        private readonly Document _document;
        private readonly Section _section;
        private readonly PdfDocumentRenderer _render;

        public PdfDocument()
        {
            _document = new Document();
            _section = _document.AddSection();
            _render = new PdfDocumentRenderer();
        }

        public void AddImage(string filePath)
        {
            var image = _section.AddImage(filePath);

            image.Height = _document.DefaultPageSetup.PageHeight;
            image.Width = _document.DefaultPageSetup.PageWidth;
            image.ScaleHeight = 0.75;
            image.ScaleWidth = 0.75;

            _section.AddPageBreak();
        }
        
        public void Save(string filePath)
        {
            _render.Document = _document;
            _render.RenderDocument();
            _render.Save(filePath);
        }

        public static PdfDocument CreateNewDocument() => new PdfDocument();
    }
}
