using iText.IO.Font.Constants;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;

namespace AdvanceAjaxCRUD.Helper
{
    public class HeaderEventHandler : IEventHandler
    {
        private readonly string header;

        public HeaderEventHandler(string header)
        {
            this.header = header;
        }

        public void HandleEvent(Event @event)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();
            Rectangle pageSize = page.GetPageSizeWithRotation();
            float leftMargin = pageSize.GetLeft() + 36f; // 36 is a default margin value in iText7

            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(), page.GetResources(), pdfDoc);
            canvas.BeginText()
                .SetFontAndSize(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD), 12)
                .MoveText(leftMargin, pageSize.GetTop() - 30)
                .ShowText(header)
                .EndText();
            canvas.Release();
        }
    }





}
