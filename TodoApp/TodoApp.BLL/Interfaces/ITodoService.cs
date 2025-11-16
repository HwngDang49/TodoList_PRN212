using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.BLL.Interfaces
{
    public interface ITodoService
    {
        Todo CreateTodo(Guid userId, Guid categoryId, string title, string description, bool isCompleted, DateTime ReminderTime, DateTime createdAt, DateTime updatedAt);
        List<Todo> GetTodos(Guid userId);
        Todo GetTodoById(Guid id);
        bool UpdateTodo(Todo todo);
        bool DeleteTodo(Guid todoId);
        bool ToggleCompleted(Guid todoId);
    }
}
