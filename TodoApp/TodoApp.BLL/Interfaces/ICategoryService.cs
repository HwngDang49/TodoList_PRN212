using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.BLL.Interfaces
{
    public interface ICategoryService
    {
        Category CreateCategory(Guid userId, string name, DateTime createdAt);
        List<Category> GetCategories(Guid userId);
        Category GetCategoryById(Guid categoryId);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Guid categoryId);
    }
}
