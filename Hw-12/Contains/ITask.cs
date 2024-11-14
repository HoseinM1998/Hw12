namespace Hw_12.Contains
{
    public interface ITask
    {
        public void Add(Task task);
        public List<Task> GetAll();
        public Task Get(int id);
        public void Update(Task task);
        public void Delete(int id);

    }
}
