using ClosedXML.Excel;
using SpendSmart.Models;

namespace SpendSmart.Services
{
    public class XlsGenerator : IFileGenerator
    {
        public byte[] GenerateFile(List<Expense> expenses)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Expenses");

                var headers = new string[]
                {
                "Value", "Currency", "Description", "Category", "Status", "Payment Due Date",
                "Payment Date", "Priority", "Payment Method"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cell(1, i + 1).Value = headers[i];
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                    worksheet.Cell(1, i + 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                int row = 2;
                foreach (var expense in expenses)
                {
                    worksheet.Cell(row, 1).Value = expense.Value;
                    worksheet.Cell(row, 2).Value = expense.Currency;
                    worksheet.Cell(row, 3).Value = expense.Description;
                    worksheet.Cell(row, 4).Value = expense.Category;
                    worksheet.Cell(row, 5).Value = expense.Status;
                    worksheet.Cell(row, 6).Value = expense.PaymentDueDate?.ToString("yyyy-MM-dd") ?? "Paid";
                    worksheet.Cell(row, 7).Value = expense.PaymentDate?.ToString("yyyy-MM-dd") ?? "Not paid";
                    worksheet.Cell(row, 8).Value = expense.Priority;
                    worksheet.Cell(row, 9).Value = expense.PaymentMethod;
                    row++;
                }

                worksheet.Columns().AdjustToContents();

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        public string GetContentType() => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public string GetFileExtension() => "xlsx";
    }

}
