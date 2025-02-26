using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebTest.Models;
using WebTest.Models.Tables;

namespace WebTest.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        using (var db = new Task2tmdtContext())
        {
            // phải xử lý trong cái using
            // bước 1 tạo đối tượng
            var indexViewModel = new IndexViewModel();
            // bước 2 load dữ liệu từ db
            var categories = db.Categories.ToList(); 
            // bước 3 gắn lại dữ liệu vào đối tượng
            indexViewModel.categories = categories;
            // bước 4 return l
            return View(indexViewModel);
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
public partial class IndexViewModel
{
    public List<Category> categories { get; set; }
}
