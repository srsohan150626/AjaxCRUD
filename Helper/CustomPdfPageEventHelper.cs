using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AdvanceAjaxCRUD.Helper
{
    public class CustomPdfPageEventHelper : PdfPageEventHelper
    {
        private int _pageCount;

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            // Create a new PdfPTable to hold the footer
            PdfPTable footerTable = new PdfPTable(2);

            // Set the table width to the page
            footerTable.TotalWidth = document.PageSize.Width;
            //_pageCount++;
            writer.PageCount = _pageCount;


            // Add page number
            //PdfPCell pageNumberCell = new PdfPCell(new Phrase($"Page {writer.PageNumber - 1} of "));
            // Add page number and total page count
            PdfPCell pageNumberCell = new PdfPCell(new Phrase($"Page {writer.PageNumber}"));
            pageNumberCell.HorizontalAlignment = Element.ALIGN_CENTER;
            pageNumberCell.Border = Rectangle.NO_BORDER;
            footerTable.AddCell(pageNumberCell);

            // Add page count placeholder
            PdfPCell pageCountCell = new PdfPCell(new Phrase(" "));
            pageCountCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            pageCountCell.Border = Rectangle.NO_BORDER;
            footerTable.AddCell(pageCountCell);

            // Update the page count placeholder with the actual total number of pages
            string pageCountText = $"{writer.PageNumber - 1}";
            pageCountCell.Phrase = new Phrase(pageCountText, pageNumberCell.Phrase.Font);

            // Position the footer at the bottom of the page
            footerTable.WriteSelectedRows(0, -1, document.LeftMargin, document.BottomMargin, writer.DirectContent);
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);

            // Set the total number of pages to be displayed
            writer.PageCount = writer.PageNumber - 1;
        }
    }
}
