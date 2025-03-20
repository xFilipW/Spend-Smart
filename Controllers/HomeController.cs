using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using UsersApp.Data;
using UsersApp.Models;
using SpendSmart.Models;
using UsersApp.ViewModels;
using SpendSmart.Services;

namespace UsersApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public HomeController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> ExpensesOverview()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var expenses = await _context.Expenses
                .Where(e => e.UserId == user.Id)
                .OrderBy(e => e.Id)
                .ToListAsync();

            return View(expenses);
        }

        public async Task<IActionResult> CreateEditExpense(int? id)
        {
            if (id == null)
                return View(new Expense());

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
                return NotFound();

            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEditExpenseForm(Expense model, IFormFile? attachmentFile, string? existingAttachmentFileName, string? removeAttachment)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            model.UserId = user.Id;
            model.User = user;

            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (!ModelState.IsValid)
            {
                return View("CreateEditExpense", model);
            }

            await HandleExpenseAttachmentAsync(model, attachmentFile, existingAttachmentFileName, removeAttachment);

            SetPaymentDates(model);

            if (model.Id == 0)
            {
                _context.Expenses.Add(model);
            }
            else
            {
                await UpdateExistingExpense(model);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("ExpensesOverview");
        }

        private async Task HandleExpenseAttachmentAsync(Expense model, IFormFile? attachmentFile, string? existingAttachmentFileName, string? removeAttachment)
        {
            if (attachmentFile != null && attachmentFile.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await attachmentFile.CopyToAsync(memoryStream);
                model.AttachmentData = memoryStream.ToArray();
                model.AttachmentFileName = attachmentFile.FileName;
            }
            else if (!string.IsNullOrEmpty(existingAttachmentFileName) && removeAttachment != "true" && model.Id != 0)
            {
                var existingExpense = await _context.Expenses.FindAsync(model.Id);
                if (existingExpense != null)
                {
                    model.AttachmentData = existingExpense.AttachmentData;
                    model.AttachmentFileName = existingExpense.AttachmentFileName;
                }
            }
            else if (removeAttachment == "true")
            {
                model.AttachmentData = null;
                model.AttachmentFileName = null;
            }
        }

        private void SetPaymentDates(Expense model)
        {
            if (model.Status == "Completed" || model.Status == "Delayed")
            {
                model.PaymentDueDate = null;
            }
            else if (model.Status == "Planned")
            {
                model.PaymentDate = null;
            }
        }

        private async Task UpdateExistingExpense(Expense model)
        {
            var existingExpense = await _context.Expenses.FindAsync(model.Id);
            if (existingExpense != null)
            {
                existingExpense.Value = model.Value;
                existingExpense.Description = model.Description;
                existingExpense.Currency = model.Currency;
                existingExpense.Status = model.Status;
                existingExpense.Priority = model.Priority;
                existingExpense.PaymentMethod = model.PaymentMethod;
                existingExpense.PaymentDueDate = model.PaymentDueDate;
                existingExpense.PaymentDate = model.PaymentDate;
                existingExpense.AttachmentData = model.AttachmentData;
                existingExpense.AttachmentFileName = model.AttachmentFileName;
            }
        }

        public async Task<IActionResult> DownloadFile(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null || expense.AttachmentData == null)
            {
                return NotFound();
            }

            return File(expense.AttachmentData, "application/octet-stream", expense.AttachmentFileName);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return RedirectToAction("ExpensesOverview");
        }

        [HttpGet]
        public async Task<IActionResult> GenerateFile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var expenses = await _context.Expenses
                .Where(e => e.UserId == user.Id)
                .OrderBy(e => e.Id)
                .ToListAsync();

            return View(expenses);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateFile(List<int> selectedExpenses, string fileName, string fileType)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                fileName = "ExpensesReport";
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var expenses = await _context.Expenses
                .Where(e => selectedExpenses.Contains(e.Id) && e.UserId == user.Id)
                .ToListAsync();

            if (!expenses.Any())
            {
                return RedirectToAction("GenerateFile");
            }

            IFileGenerator? fileGenerator = fileType switch
            {
                "pdf" => new PdfGenerator(),
                "xls" or "xlsx" => new XlsGenerator(),
                _ => null
            };

            if (fileGenerator == null)
            {
                return BadRequest("Invalid file type selected.");
            }

            var fileData = fileGenerator.GenerateFile(expenses);
            return File(fileData, fileGenerator.GetContentType(), $"{fileName}.{fileGenerator.GetFileExtension()}");
        }

        public async Task<IActionResult> ExpensesSummary()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var expenses = await _context.Expenses
                .Where(e => e.UserId == user.Id)
                .ToListAsync();

            return View(expenses);
        }

        [HttpGet]
        public async Task<IActionResult> GetExpensesData()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var expenses = await _context.Expenses
                .Where(e => e.UserId == user.Id)
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key,
                    TotalAmount = g.Sum(e => e.Value)
                })
                .ToListAsync();

            return Json(expenses);
        }

        public IActionResult ScanReceipt()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
