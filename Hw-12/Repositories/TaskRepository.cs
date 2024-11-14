using Hw_12.Configuration;
using Hw_12.Contains;
using System.Threading.Tasks;


namespace Hw_12.Repositories
{
    public class TaskRepository : ITask
    {
        TaskDbContext _context = new TaskDbContext();
        public void Add(Task task)
        {
            _context.Add(task);
            _context.SaveChanges();
        }
        public void Delete(int id)
        {
            var task = _context.Tasks.Where(t => t.Id == id).FirstOrDefault();
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }
        public Task Get(int id)
        {
            return _context.Tasks.Where(t => t.Id == id).FirstOrDefault();
        }
        public List<Task> GetAll()
        {
            var tasks = _context.Tasks.ToList();        
            return tasks;
        }
        public void Update(Task task)
        {
            //var updateTask = _context.Tasks.Where(t => t.Id == task.Id).FirstOrDefault();
            var updateTask = _context.Tasks.Find(task.Id);

            if (updateTask != null)
            {
              updateTask.Title = task.Title;
                updateTask.Description = task.Description;
                updateTask.TimeToDone = task.TimeToDone;
                updateTask.State = task.State;
                updateTask.Order = task.Order;
                _context.SaveChanges();
            }
        }
        public List<Task> SearchTasksByTitle(string title)
        {
            return _context.Tasks
                .Where(t => t.Title.ToLower().Contains(title.ToLower())) 
                .ToList();
        }
        public void ChangeTaskState(int taskId, EnumState newState)
        {
            var task = _context.Tasks.Find(taskId); 
            if (task != null)
            {
                task.State = newState; 
                _context.SaveChanges();
            }
        }

    }
}
