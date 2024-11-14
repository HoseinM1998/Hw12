using Colors.Net.StringColorExtensions;
using Colors.Net;
using Hw_12.Contains;
using Hw_12.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hw_12.Entities;

namespace Hw_12.Service
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        public TaskService(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void AddTask(string title, string description, DateTime timeToDone, int order, int userId) 
        {
            if (timeToDone <= DateTime.Now)
            {
                throw new ArgumentException("Error The DateTime Is Now");
            }
            if (_taskRepository.GetAll().Any(t => t.Order == order))
            {
                order = _taskRepository.GetAll().Max(t => t.Order) + 1;
            }
            var task = new Task
            {
                Title = title,
                Description = description,
                TimeToDone = timeToDone,
                Order = order,
                State = EnumState.InPending,
                UserID = userId
            };
            _taskRepository.Add(task);
        }

        public List<Task> GetAllTasks(int userId)
        {
            
            var tasks = _taskRepository.GetAll().Where(t => t.UserID == userId).ToList();
            foreach (var task in tasks)
            {
                if (task.TimeToDone <= DateTime.Now && task.State == EnumState.InPending)
                {
                    task.State = EnumState.Cancceled;
                    _taskRepository.Update(task);
                }
            }
            return tasks.OrderBy(t => t.TimeToDone).ThenBy(t => t.Order).ToList();
        }

        public Task GetTaskById(int taskId, int userId)
        {
            var task = _taskRepository.Get(taskId);
            if (task != null && task.UserID == userId)
            {
                return task;
            }
            else
            {
                throw new ArgumentException("NotFound Or NotCurrentUser");
            }
        }

        public void UpdateTask(int id, string title, string description, DateTime timeToDone, int order,int userId)
        {

            var task = _taskRepository.Get(id);
            if (task != null)
            {

                if (_taskRepository.GetAll().Any(t => t.Order == order && t.Id != id))
                {
                    order = _taskRepository.GetAll().Max(t => t.Order) + 1;
                }
                if (timeToDone <= DateTime.Now)
                {
                    throw new ArgumentException("Error The DateTime Is Now");
                }
                task.Title = title;
                task.Description = description;
                task.TimeToDone = timeToDone;
                task.Order = order;
                task.UserID = userId;

                _taskRepository.Update(task);
            }

        }
        public void DeleteTask(int id, int userId)
        {
            var task = _taskRepository.Get(id);
            if (task == null)
            {
                throw new ArgumentException("Not Found");
            }

            if (task.UserID != userId)
            {
                throw new Exception("You CanNot Delete TaskID");
            }

            _taskRepository.Delete(id);
        }
        public List<Task> SearchTasks(string title,int userId)
        {
            try
            {
                var tasks = _taskRepository.SearchTasksByTitle(title).Where(t => t.UserID == userId)
                                   .ToList(); ;
                return tasks;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in searching tasks: {ex.Message}", ex);
            }
        }

        public void ChangeTaskState(int taskId, int status)
        {           
            if (status < 1 || status > 3)
            {
                throw new ArgumentException("Error|1.IsPending 2.Done 3.Cancelled");
            }
            var newState = (EnumState)status;
            _taskRepository.ChangeTaskState(taskId, newState);
        }
    }
}



