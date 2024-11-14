using Colors.Net.StringColorExtensions;
using Colors.Net;
using ConsoleTables;
using Hw_12.Entities;
using Hw_12.Repositories;
using Hw_12.Service;
using System.Threading.Tasks;

TaskRepository taskRepository = new TaskRepository();
TaskService taskService = new TaskService(taskRepository);
UserRepository userRepository = new UserRepository();
UserService userService = new UserService(userRepository);
ProgressBar _progressBar = new ProgressBar();
bool loggedIn = false;

while (true)
{
    Console.Clear();
    ColoredConsole.WriteLine("*********Welcome ToDO List*********".DarkGreen());
    ColoredConsole.WriteLine("1.Register".DarkBlue());
    ColoredConsole.WriteLine("2.Login".DarkBlue());
    ColoredConsole.WriteLine("3.Exit".DarkRed());

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            ColoredConsole.WriteLine("Enter FullName : ".DarkYellow());
            string fullName = Console.ReadLine();
            ColoredConsole.WriteLine("Enter Username : ".DarkYellow());
            string userName = Console.ReadLine();
            ColoredConsole.WriteLine("Enter Password : ".DarkYellow());
            string password = Console.ReadLine();

            try
            {
                userService.Register(fullName, userName, password);
                ColoredConsole.WriteLine("Successful.".DarkGreen());
            }
            catch (Exception ex)
            {
                ColoredConsole.WriteLine($"Error: {ex.Message}".DarkRed());
            }
            Console.ReadKey();
            break;

        case "2":
            ColoredConsole.WriteLine("Enter Username: ".DarkYellow());
            string username = Console.ReadLine();
            ColoredConsole.WriteLine("Enter Password: ".DarkYellow());
            string pass = Console.ReadLine();

            try
            {
                userService.Login(username, pass);
                var currentUser = userService.GetCurrentUser(); 
                if (currentUser == null)
                {
                    ColoredConsole.WriteLine("User Not Logged".DarkRed());
                    Console.ReadKey();
                    break;
                }
                loggedIn = true;
                ColoredConsole.WriteLine("Login Successful.".DarkGreen());
            }
            catch (Exception ex)
            {
                ColoredConsole.WriteLine($"Error: {ex.Message}".DarkRed());
            }
            Console.ReadKey();
            break;

        case "3":
            return;
        default:
            ColoredConsole.WriteLine("Invalid".DarkRed());
            Console.ReadKey();
            break;
    }

    if (loggedIn)
    {
        bool inMenu = true;
        while (inMenu)
        {
            try
            {

                Console.Clear();
                ColoredConsole.WriteLine("*********Welcome ToDO List*********".DarkGreen());
                ColoredConsole.WriteLine("1. Add Task".DarkBlue());
                ColoredConsole.WriteLine("2. Update Task".DarkBlue());
                ColoredConsole.WriteLine("3. Delete Task".DarkBlue());
                ColoredConsole.WriteLine("4. View All Tasks".DarkBlue());
                ColoredConsole.WriteLine("5. Search Tasks by Title".DarkBlue());
                ColoredConsole.WriteLine("6. Change Task State".DarkBlue());
                ColoredConsole.WriteLine("7. Exit".DarkRed());
                var currentUser = userService.GetCurrentUser();

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                      
                        ColoredConsole.WriteLine("Enter Title : ".DarkYellow());
                        string title = Console.ReadLine();
                        ColoredConsole.WriteLine("Enter Description : ".DarkYellow());
                        string description = Console.ReadLine();
                        ColoredConsole.WriteLine("Enter New TimeToDone (yyyy-MM-dd HH:mm): ".DarkCyan());
                        DateTime timeToDone = DateTime.Parse(Console.ReadLine());
                        ColoredConsole.WriteLine("Enter Order : ".DarkYellow());
                        int order = int.Parse(Console.ReadLine());
                        try
                        {
                            taskService.AddTask(title, description, timeToDone, order, currentUser.Id);
                            ColoredConsole.WriteLine("Successfully".DarkGreen());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error : {ex.Message}");
                        }
                        Console.ReadKey();
                        break;

                    case "2":
                        ColoredConsole.WriteLine("Enter Id To Update: ".DarkCyan());
                        int updateId = int.Parse(Console.ReadLine());
                        var taskUpdate = taskService.GetTaskById(updateId, currentUser.Id);

                        if (taskUpdate == null)
                        {
                            ColoredConsole.WriteLine("Error: Task Not Found".DarkRed());
                            Console.ReadKey();
                            break;
                        }
                        ColoredConsole.WriteLine("Enter New Task Title: ".DarkCyan());
                        string newTitle = Console.ReadLine();
                        ColoredConsole.WriteLine("Enter New Task Description: ".DarkCyan());
                        string newDescription = Console.ReadLine();
                        ColoredConsole.WriteLine("Enter New TimeToDone (yyyy-MM-dd HH:mm): ".DarkCyan());
                        DateTime newTimeToDone = DateTime.Parse(Console.ReadLine());
                        ColoredConsole.WriteLine("Enter New Task Order: ".DarkCyan());
                        int newOrder = int.Parse(Console.ReadLine());
                        try
                        {
                            taskService.UpdateTask(updateId, newTitle, newDescription, newTimeToDone, newOrder, currentUser.Id);
                            ColoredConsole.WriteLine("Successfully");
                        }
                        catch (Exception ex)
                        {
                            ColoredConsole.WriteLine($"Error : {ex.Message}".DarkRed());
                        }
                        Console.ReadKey();
                        break;
                    case "3":
                        ColoredConsole.WriteLine("Enter ID : ".DarkMagenta());
                        int deleteId = int.Parse(Console.ReadLine());
                        var taskDelete = taskService.GetTaskById(deleteId, currentUser.Id);
                        if (taskDelete == null)
                        {
                            ColoredConsole.WriteLine("Error: Task Not Found.".DarkRed());
                            Console.ReadKey();
                            break;
                        }
                        try
                        {
                            taskService.DeleteTask(deleteId,currentUser.Id);
                            ColoredConsole.WriteLine("Successfully".DarkGreen());
                        }
                        catch (Exception ex)
                        {
                            ColoredConsole.WriteLine($"Error : {ex.Message}".DarkRed());
                        }
                        Console.ReadKey();
                        break;

                    case "4":
                        ColoredConsole.WriteLine("All Tasks: ".DarkGreen());
                        var tasks = taskService.GetAllTasks(currentUser.Id);
                        var table = ConsoleTable.From<Task>(tasks);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        table.Write();
                        Console.ResetColor();

                        //foreach (var task in tasks)
                        //{
                        //    ColoredConsole.WriteLine($"Id: {task.Id}, Title: {task.Title}, Description: {task.Description}, Time to Done: {task.TimeToDone}, State: {task.State}".DarkCyan());
                        //}
                        Console.ReadKey();
                        break;

                    case "5":
                        ColoredConsole.WriteLine("Enter Title : ".DarkCyan());
                        string searchTitle = Console.ReadLine();

                        try
                        {
                            var searchResults = taskService.SearchTasks(searchTitle,currentUser.Id);
                            if (searchResults.Count > 0)
                            {
                                ColoredConsole.WriteLine("Search Found : ".DarkGreen());
                                foreach (var task in searchResults)
                                {
                                    ColoredConsole.WriteLine($"|Id: {task.Id} | | Title: {task.Title} | | Description: {task.Description} | |TimeToDone: {task.TimeToDone} | | State: {task.State} |".DarkYellow());
                                }
                            }
                            else
                            {
                                ColoredConsole.WriteLine("Not Found".DarkRed());
                            }
                        }
                        catch (Exception ex)
                        {
                            ColoredConsole.WriteLine($"Error : {ex.Message}".DarkRed());
                        }
                        Console.ReadKey();
                        break;

                    case "6":
                        ColoredConsole.Write("Enter Id : ".DarkGray());
                        int taskChangeState = int.Parse(Console.ReadLine());
                        var taskState = taskService.GetTaskById(taskChangeState, currentUser.Id);
                        if (taskState == null)
                        {
                            ColoredConsole.WriteLine("Error: Task Not Found".DarkRed());
                            Console.ReadKey();
                            break;
                        }
                        ColoredConsole.WriteLine("Enter new state (1 = Pending, 2 = Done, 3 = Cancelled) : ".DarkGreen());
                        int newState = int.Parse(Console.ReadLine());


                        try
                        {
                            taskService.ChangeTaskState(taskChangeState, newState);
                            ColoredConsole.WriteLine("Successfully".DarkGreen());
                        }
                        catch (Exception ex)
                        {
                            ColoredConsole.WriteLine($"Error : {ex.Message}".DarkRed());
                        }
                        Console.ReadKey();
                        break;

                    case "7":
                        userService.Logout();
                        inMenu = false;
                        ColoredConsole.WriteLine("Logged Out".DarkRed());
                        Console.ReadKey();
                        break;

                    default:
                        ColoredConsole.WriteLine("Invalid".DarkRed());
                        Console.ReadKey();
                        break;
                }
            }
            catch (FormatException fe)
            {
                ColoredConsole.WriteLine($"Error: {fe.Message}".DarkRed());
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                ColoredConsole.WriteLine($"Error: {ex.Message}".DarkRed());
                Console.ReadKey();
            }
            finally
            {
                _progressBar.DisPlay();
            }
        }
    }
}