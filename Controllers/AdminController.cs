using Microsoft.AspNetCore.Mvc;
using WebTest.Models.Tables;

namespace WebTest.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Categories()
        {
            using (var db = new Task2tmdtContext())
            {
                // phải xử lý trong cái using
                // bước 1 tạo đối tượng
                var indexViewModel = new CategoriesViewModel();
                // bước 2 load dữ liệu từ db
                var categories = db.Categories.ToList();
                // bước 3 gắn lại dữ liệu vào đối tượng
                indexViewModel.categories = categories;
                // bước 4 return l
                return View(indexViewModel);
            }
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        public partial class CategoriesViewModel
        {
            public List<Category> categories { get; set; }
        }

        public partial class FormSaveNewCategory
        {
            public string name { get; set; }
            public string description { get; set; }
        }
        public partial class FormSaveUpdateCategory
        {
            public int Id { get; set; }  // Cần có ID để xác định category cần cập nhật
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public partial class UpdateCategoriesViewModel
        {
            public Category category { get; set; }
        }
        public IActionResult UpdateCategory(int id)
        {
            using (var db = new Task2tmdtContext())
            {
                var category = db.Categories.Where(e => e.Id == id).FirstOrDefault();
                if (category != null)
                {
                    var viewModel = new UpdateCategoriesViewModel();
                    viewModel.category = category;
                    return View(viewModel);
                }
                return new RedirectResult(url: "/admin/categories");
            }
        }
        
        public RedirectResult SaveNewCategory(FormSaveNewCategory formData)
        {
            if (formData.name != null)
            {
                using (var db = new Task2tmdtContext())
                {
                    // push data vào database
                    db.Categories.Add(new Category
                    {
                        Name = formData.name,
                        Description = formData.description
                    });
                    db.SaveChanges();
                    // back to categories views
                    return new RedirectResult(url: "/admin/categories");
                }
            }
            return new RedirectResult(url: "/admin/createCategory");
        }
        public RedirectResult DeleteCategory(int id)
        {
            using (var db = new Task2tmdtContext())
            {
                // Xóa tất cả news liên quan trước
                var relatedNews = db.News.Where(n => n.CategoryId == id).ToList();
                db.News.RemoveRange(relatedNews);
                db.SaveChanges(); // Lưu lại trước khi xóa danh mục

                // Xóa danh mục
                var category = db.Categories.Where(e => e.Id == id).FirstOrDefault();
                if (category != null)
                {
                    db.Categories.Remove(category);
                    db.SaveChanges();
                }

                return new RedirectResult(url: "/admin/categories");
            }
        }

        public RedirectResult SaveUpdateCategory(FormSaveUpdateCategory formData)
        {
            using (var db = new Task2tmdtContext())
            {
                var category = db.Categories.Where(c => c.Id == formData.Id).FirstOrDefault();
                if (category != null)
                {
                    // Cập nhật thông tin category
                    category.Name = formData.Name;
                    category.Description = formData.Description;
                    db.SaveChanges(); // Lưu thay đổi
                }
            }
            return new RedirectResult(url: "/admin/categories");
        }


    }

}
