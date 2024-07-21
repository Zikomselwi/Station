//using MigraDoc.DocumentObjectModel;
//using MigraDoc.DocumentObjectModel.Tables;
//using MigraDoc.Rendering;
//using PdfSharpCore.Drawing;
//using System.Linq;

//using iTextSharp.text.pdf;
//namespace Station.ViewModel
//{
  

//    public class PdfService
//    {
//        public byte[] GenerateBillPdf(int billNumber, string subscriberName, DateTime billDate, double currentReading, double previousReading, decimal consumptionCost, string subscriptionType)
//        {
//            var document = new Document();
//            var section = document.AddSection();

//            // Add a header
//            var header = section.Headers.Primary;
//            header.AddParagraph("Bill Details");
//            header.Format.Font.Size = 14;
//            header.Format.Alignment = ParagraphAlignment.Center;

//            // Add content
//            var table = section.AddTable();
//            table.AddColumn(Unit.FromCentimeter(6));
//            table.AddColumn(Unit.FromCentimeter(10));
//            table.Format.Alignment = ParagraphAlignment.Left;

//            // Add rows to the table
//            AddRow(table, "Number Bill", billNumber.ToString());
//            AddRow(table, "Subscriber Name", subscriberName);
//            AddRow(table, "Date", billDate.ToString("yyyy-MM-dd"));
//            AddRow(table, "Current Reading", currentReading.ToString());
//            AddRow(table, "Previous Reading", previousReading.ToString());
//            AddRow(table, "Cost Reading", consumptionCost.ToString());
//            AddRow(table, "Subscription Type", subscriptionType);

//            // Render the PDF document
//            var renderer = new PdfDocumentRenderer();
           
//            renderer.RenderDocument();

//            // Save the PDF to a memory stream
//            using (var stream = new MemoryStream())
//            {
//                renderer.PdfDocument.Save(stream, false);
//                return stream.ToArray();
//            }
//        }

//        private void AddRow(Table table, string label, string value)
//        {
//            var row = table.AddRow();
//            row.Cells[0].AddParagraph(label);
//            row.Cells[1].AddParagraph(value);
//        }
//    }

//}
