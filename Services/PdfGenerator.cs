using iTextSharp.text.pdf;
using iTextSharp.text;
using SpendSmart.Models;

namespace SpendSmart.Services
{
    public class PdfGenerator : IFileGenerator
    {
        public byte[] GenerateFile(List<Expense> expenses)
        {
            using (var memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4.Rotate(), 40, 40, 40, 40);
                PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                document.Add(new Paragraph("SpendSmart", titleFont) { Alignment = Element.ALIGN_LEFT });
                document.Add(new Paragraph("\n"));
                document.Add(new Paragraph("Expense Report", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14)) { Alignment = Element.ALIGN_CENTER });
                document.Add(new Paragraph($"Generated on: {DateTime.Now:dd.MM.yyyy HH:mm:ss}\n\n"));

                PdfPTable table = new PdfPTable(9) { WidthPercentage = 100 };
                table.SetWidths(new float[] { 1.5f, 1.5f, 3f, 2f, 2f, 2.2f, 2.2f, 1.5f, 1.8f });

                string[] columnHeaders = { "Value", "Currency", "Description", "Category", "Status", "Payment Due Date", "Payment Date", "Priority", "Payment Method" };
                foreach (string column in columnHeaders)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column, headerFont))
                    {
                        BackgroundColor = new BaseColor(200, 200, 200),
                        Padding = 6,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE
                    };
                    table.AddCell(cell);
                }

                foreach (var expense in expenses)
                {
                    table.AddCell(new PdfPCell(new Phrase(expense.Value.ToString(), cellFont)) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    table.AddCell(new PdfPCell(new Phrase(expense.Currency, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(expense.Description, cellFont)) { HorizontalAlignment = Element.ALIGN_LEFT });
                    table.AddCell(new PdfPCell(new Phrase(expense.Category, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(expense.Status, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(expense.PaymentDueDate?.ToString("yyyy-MM-dd") ?? "Paid", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(expense.PaymentDate?.ToString("yyyy-MM-dd") ?? "Not paid", cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(expense.Priority, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell(new Phrase(expense.PaymentMethod, cellFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                }

                document.Add(table);
                document.Close();
                return memoryStream.ToArray();
            }
        }

        public string GetContentType() => "application/pdf";
        public string GetFileExtension() => "pdf";
    }

}
