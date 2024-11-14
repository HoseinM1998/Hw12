using Colors.Net.StringColorExtensions;
using Colors.Net;
using Hw_12.Contains;
using Hw_12.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw_12.Service
{
    public class TaskService
    {
        private readonly TaskRepository _taskRepository;
        public TaskService(TaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void AddTask(string title, string description, DateTime timeToDone, int order) 
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
            };
            _taskRepository.Add(task);
        }

        public List<Task> GetAllTasks()
        {
            var tasks = _taskRepository.GetAll();
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

        public Task GetTaskById(int taskId)
        {
            return _taskRepository.Get(taskId);
        }

        public void UpdateTask(int id, string title, string description, DateTime timeToDone, int order)
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
                _taskRepository.Update(task);
            }

        }
        public void DeleteTask(int id)
        {
            _taskRepository.Delete(id);
        }
        public List<Task> SearchTasks(string title)
        {
            try
            {
                var tasks = _taskRepository.SearchTasksByTitle(title);
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



