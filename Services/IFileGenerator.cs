using SpendSmart.Models;

namespace SpendSmart.Services
{
    public interface IFileGenerator
    {
        byte[] GenerateFile(List<Expense> expenses);
        string GetContentType();
        string GetFileExtension();
    }

}
