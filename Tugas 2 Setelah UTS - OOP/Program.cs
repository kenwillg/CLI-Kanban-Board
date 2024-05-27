using Tugas2___OOP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Globalization;

Console.WriteLine("INIT");

try
{
    var optionsBuilder = new DbContextOptionsBuilder<MyDbDataContext>();
    optionsBuilder.UseMySQL(StrCon.MySQLStrCon);

    MyDbDataContext myDbDataContext = new MyDbDataContext(optionsBuilder.Options);

    myDbDataContext.Database.EnsureCreated();

    Console.WriteLine("Database created successfully!");

    while (true) // Loop for main menu
    {
        Console.Clear();
        Console.WriteLine("\n=============================");
        Console.WriteLine("Kanban Board App");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Login");
        Console.WriteLine("3. Quit");
        Console.Write("Select an option: ");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                RegisterUser(myDbDataContext);
                break;
            case "2":
                User loggedInUser = LoginUser(myDbDataContext);
                if (loggedInUser != null)
                {
                    MainMenu(myDbDataContext, loggedInUser);
                }
                break;
            case "3":
                return;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}


static string GetPassword()
{
    string password = "";
    ConsoleKeyInfo key;

    do
    {
        key = Console.ReadKey(true);

        // Ignore any key other than Enter (13) or Backspace (8)
        if (key.Key != ConsoleKey.Enter && key.Key != ConsoleKey.Backspace)
        {
            password += key.KeyChar;
            Console.Write("*"); // Display asterisk instead of the actual character
        }
        else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
        {
            // If Backspace is pressed, remove the last character from the password
            password = password.Substring(0, (password.Length - 1));
            Console.Write("\b \b"); // Clear the last character on the console
        }
    } while (key.Key != ConsoleKey.Enter);

    Console.WriteLine(); // Move to the next line after password entry

    return password;
}

static User LoginUser(MyDbDataContext context)
{
    Console.Write("Enter username: ");
    string username = Console.ReadLine();

    Console.Write("Enter password: ");
    string password = GetPassword();

    User user = context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

    if (user != null)
    {
        Console.WriteLine("Login successful!");
        return user;
    }
    else
    {
        Console.WriteLine("Invalid username or password.");
        return null;
    }
}

static void RegisterUser(MyDbDataContext context)
{
    Console.Write("Enter username: ");
    string username = Console.ReadLine();

    Console.Write("Enter Password: ");
    string password = GetPassword();

    // Prompt the user to re-type the password
    string reTypedPassword;
    do
    {
        Console.Write("Re-type password: ");
        reTypedPassword = GetPassword();

        if (reTypedPassword != password)
        {
            Console.WriteLine("Passwords do not match. Please try again.");
        }
    } while (reTypedPassword != password);

    DateTime dateOfBirth;
    bool isValidDate = false;

    do
    {
        Console.Write("Enter date of birth (yyyy-MM-dd): ");
        string dateOfBirthInput = Console.ReadLine();

        if (DateTime.TryParseExact(dateOfBirthInput, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
        {
            isValidDate = true;
        }
        else
        {
            Console.WriteLine("Invalid date format. Please enter the date in yyyy-MM-dd format.");
        }
    } while (!isValidDate);

    // Present choices for app usage
    Console.WriteLine("Choose the reason for using the app:");
    Console.WriteLine("1. Education as a Student");
    Console.WriteLine("2. Education as an Educator");
    Console.WriteLine("3. Corporate Work");
    Console.WriteLine("4. Freelance Work");
    Console.Write("Select an option: ");
    string appUsageOption = Console.ReadLine();

    string appUsage = "";

    // Map the user's choice to the corresponding reason for using the app
    switch (appUsageOption)
    {
        case "1":
            appUsage = "Education as a Student";
            break;
        case "2":
            appUsage = "Education as an Educator";
            break;
        case "3":
            appUsage = "Corporate Work";
            break;
        case "4":
            appUsage = "Freelance Work";
            break;
        default:
            Console.WriteLine("Invalid option. Defaulting to 'Education as a Student'.");
            appUsage = "Education as a Student";
            break;
    }

    User user = new User
    {
        Username = username,
        Password = password,
        DateOfBirth = dateOfBirth,
        AppUsage = appUsage
    };

    try
    {
        context.Users.Add(user);
        context.SaveChanges();

        Console.WriteLine("User registered successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.Message}");
    }
}


static void MainMenu(MyDbDataContext context, User loggedInUser)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("\n=============================");
        Console.WriteLine($"Welcome, {loggedInUser.Username}!");
        Console.WriteLine("Kanban Board App");
        Console.WriteLine("1. Create new board");
        Console.WriteLine("2. Continue board");
        Console.WriteLine("3. Duplicate board");
        Console.WriteLine("4. Logout");
        Console.Write("Select an option: ");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                CreateBoard(context, loggedInUser);
                break;
            case "2":
                ContinueBoard(context, loggedInUser);
                break;
            case "3":
                DuplicateBoard(context, loggedInUser);
                break;
            case "4":
                loggedInUser = null;
                return; // Back to main menu
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
}


static void RunBoard(MyDbDataContext context, Board selectedBoard)
{
    while (true)
    {
        Console.Clear();

        Console.WriteLine($"Board: {selectedBoard.BoardName}");
        Console.WriteLine("=========================================");

        // Display tasks in "To Do" section
        Console.WriteLine("To Do:");
        if (selectedBoard.ToDoTasks != null && selectedBoard.ToDoTasks.Any())
        {
            foreach (var task in selectedBoard.ToDoTasks)
            {
                Console.WriteLine($"- {task.Title}");
            }
        }
        else
        {
            Console.WriteLine("- No tasks in this section.");
        }

        Console.WriteLine("-----------------------------------------");

        // Display tasks in "Doing" section
        Console.WriteLine("Doing:");
        if (selectedBoard.DoingTasks != null && selectedBoard.DoingTasks.Any())
        {
            foreach (var task in selectedBoard.DoingTasks)
            {
                Console.WriteLine($"- {task.Title}");
            }
        }
        else
        {
            Console.WriteLine("- No tasks in this section.");
        }

        Console.WriteLine("-----------------------------------------");

        // Display tasks in "Done" section
        Console.WriteLine("Done:");
        if (selectedBoard.DoneTasks != null && selectedBoard.DoneTasks.Any())
        {
            foreach (var task in selectedBoard.DoneTasks)
            {
                Console.WriteLine($"- {task.Title}");
            }
        }
        else
        {
            Console.WriteLine("- No tasks in this section.");
        }


        Console.WriteLine("=========================================");

        Console.WriteLine("Select an action:");
        Console.WriteLine("1. Create new task");
        Console.WriteLine("2. Rename task");
        Console.WriteLine("3. Move task");
        Console.WriteLine("4. Delete task");
        Console.WriteLine("5. Exit board");

        Console.Write("Select an option: ");
        string boardOption = Console.ReadLine();

        switch (boardOption)
        {
            case "1":
                CreateTask(context, selectedBoard);
                break;
            case "2":
                RenameTask(context, selectedBoard);
                break;
            case "3":
                MoveTask(context, selectedBoard);
                break;
            case "4":
                DeleteTask(context, selectedBoard);
                break;
            case "5":
                return; // Exit the method if the user chooses to exit
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
}

static void ContinueBoard(MyDbDataContext context, User user)
{
    while (true)
    {
        Console.WriteLine("\n============================");
        Console.WriteLine("Choose a board to continue:");

        var boards = context.Boards.Where(b => b.UserId == user.Id).ToList();
        if (boards.Any())
        {
            foreach (var board in boards)
            {
                Console.WriteLine($"{board.Id}. {board.BoardName}");
            }

            Console.Write("Select a board: ");
            int boardId = int.Parse(Console.ReadLine());

            var selectedBoard = context.Boards
                .Include(b => b.ToDoTasks)
                .Include(b => b.DoingTasks)
                .Include(b => b.DoneTasks)
                .FirstOrDefault(b => b.Id == boardId && b.UserId == user.Id);

            if (selectedBoard != null)
            {
                RunBoard(context, selectedBoard);
            }
            else
            {
                Console.WriteLine("Invalid board ID.");
            }
        }
        else
        {
            Console.WriteLine("No boards available to continue.");
            return;
        }

        Console.WriteLine("\nDo you want to choose another board or quit?");
        Console.WriteLine("1. Choose another board");
        Console.WriteLine("2. Quit");
        Console.Write("Select an option: ");
        string continueOption = Console.ReadLine();

        if (continueOption != "1")
        {
            break;
        }

    }
}

static void DuplicateBoard(MyDbDataContext context, User user)
{
    // Display all boards of the user
    Console.WriteLine("\n============================");
    Console.WriteLine("Choose a board to duplicate:");

    var boards = context.Boards.Where(b => b.UserId == user.Id).ToList();
    if (boards.Any())
    {
        foreach (var board in boards)
        {
            Console.WriteLine($"{board.Id}. {board.BoardName}");
        }

        Console.Write("Select a board: ");
        int boardId = int.Parse(Console.ReadLine());

        var selectedBoard = context.Boards
            .Include(b => b.ToDoTasks)
            .Include(b => b.DoingTasks)
            .Include(b => b.DoneTasks)
            .FirstOrDefault(b => b.Id == boardId && b.UserId == user.Id);

        if (selectedBoard != null)
        {
            // Create a deep copy of the selected board
            Board newBoard = selectedBoard.BoardClone();

            // Prompt the user to name the new board
            Console.Write("Enter a name for the new board: ");
            newBoard.BoardName = Console.ReadLine();

            // Detach tasks from the original board and assign new IDs
            newBoard.ToDoTasks = newBoard.ToDoTasks.Select(task => CloneTask(task)).ToList();
            newBoard.DoingTasks = newBoard.DoingTasks.Select(task => CloneTask(task)).ToList();
            newBoard.DoneTasks = newBoard.DoneTasks.Select(task => CloneTask(task)).ToList();

            // Add the new board to the context and save changes
            context.Boards.Add(newBoard);
            context.SaveChanges();

            Console.WriteLine("Board duplicated successfully!");
        }
        else
        {
            Console.WriteLine("Invalid board ID.");
        }
    }
    else
    {
        Console.WriteLine("No boards available to duplicate.");
    }
}

static Tugas2___OOP.Task CloneTask(Tugas2___OOP.Task originalTask)
{
    return new Tugas2___OOP.Task
    {
        Title = originalTask.Title,
        // Exclude the original primary key
        // Ensure the properties related to task placement (e.g., Board IDs) are correctly reset or assigned later if needed
    };
}

static void CreateBoard(MyDbDataContext context, User user)
{
    Board board = new Board
    {
        UserId = user.Id
    };

    Console.Write("Insert board name: ");
    board.BoardName = Console.ReadLine();

    Console.Write("Insert board description: ");
    board.BoardDesc = Console.ReadLine();

    context.Boards.Add(board);
    context.SaveChanges();

    Console.WriteLine("Board created successfully!");
}

static void CreateTask(MyDbDataContext context, Board board)
{
    try
    {
        Tugas2___OOP.Task task = new Tugas2___OOP.Task();

        Console.Write("Insert task name: ");
        task.Title = Console.ReadLine();

        Console.WriteLine("Where to put?");
        Console.WriteLine("1. To-do");
        Console.WriteLine("2. Doing");
        Console.WriteLine("3. Done");
        Console.Write("Select an option: ");
        string option = Console.ReadLine();

        switch (option)
        {
            case "1":
                task.ToDoBoardId = board.Id;
                break;
            case "2":
                task.DoingBoardId = board.Id;
                break;
            case "3":
                task.DoneBoardId = board.Id;
                break;
            default:
                Console.WriteLine("Invalid option.");
                return; // Exit the method without saving if option is invalid
        }

        // Add the task to the context
        context.Tasks.Add(task);

        // Save changes to the database
        context.SaveChanges();

        Console.WriteLine("Task created successfully!");
    }
    catch (Exception ex)
    {
        // Display the error message
        Console.WriteLine("An error occurred while saving changes to the database.");
        Console.WriteLine($"Error message: {ex.Message}");

        // Check if there's an inner exception
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
        }
    }
}

static void RenameTask(MyDbDataContext context, Board board)
{
    // Display tasks in each section
    Console.WriteLine("\n==================");
    Console.WriteLine("Tasks in To Do:");
    DisplayTasks(board.ToDoTasks);
    Console.WriteLine("\n==================");
    Console.WriteLine("Tasks in Doing:");
    DisplayTasks(board.DoingTasks);
    Console.WriteLine("\n==================");
    Console.WriteLine("Tasks in Done:");
    DisplayTasks(board.DoneTasks);
    Console.WriteLine("==================\n");

    // Get task to rename
    Console.WriteLine("Enter the ID of the task you want to rename:");
    int taskId = int.Parse(Console.ReadLine());

    var task = context.Tasks.FirstOrDefault(t => t.Id == taskId);
    if (task != null)
    {
        Console.Write("Insert new name: ");
        task.Title = Console.ReadLine();

        context.SaveChanges();
        Console.WriteLine("Task renamed successfully!");
    }
    else
    {
        Console.WriteLine("Task not found.");
    }
}

static void MoveTask(MyDbDataContext context, Board board)
{
    // Display tasks in each section
    Console.WriteLine("\n==================");
    Console.WriteLine("Tasks in To Do:");
    DisplayTasks(board.ToDoTasks);
    Console.WriteLine("\n==================");
    Console.WriteLine("Tasks in Doing:");
    DisplayTasks(board.DoingTasks);
    Console.WriteLine("\n==================");
    Console.WriteLine("Tasks in Done:");
    DisplayTasks(board.DoneTasks);
    Console.WriteLine("==================\n");

    // Get task to move
    Console.WriteLine("Enter the ID of the task you want to move:");
    if (!int.TryParse(Console.ReadLine(), out int taskId))
    {
        Console.WriteLine("Invalid task ID.");
        return;
    }

    var task = context.Tasks.Find(taskId);
    if (task != null)
    {
        Console.WriteLine("Where to move?");
        Console.WriteLine("1. To-do");
        Console.WriteLine("2. Doing");
        Console.WriteLine("3. Done");
        Console.Write("Select an option: ");
        string option = Console.ReadLine();

        // Clear current board associations
        task.ToDoBoardId = null;
        task.DoingBoardId = null;
        task.DoneBoardId = null;

        // Move task to the selected list by setting the appropriate foreign key
        switch (option)
        {
            case "1":
                task.ToDoBoardId = board.Id;
                break;
            case "2":
                task.DoingBoardId = board.Id;
                break;
            case "3":
                task.DoneBoardId = board.Id;
                break;
            default:
                Console.WriteLine("Invalid option.");
                return;
        }

        // Update the task state to Modified
        context.Entry(task).State = EntityState.Modified;

        // Save changes
        try
        {
            context.SaveChanges();
            Console.WriteLine("Task moved successfully!");

            // Reload the board entity to reflect changes
            context.Entry(board).Reload();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while moving the task: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine("Task not found.");
    }
}
static void DeleteTask(MyDbDataContext context, Board board)
{
    // Display tasks in each section
    Console.WriteLine("Tasks in To Do:");
    DisplayTasks(board.ToDoTasks);
    Console.WriteLine("Tasks in Doing:");
    DisplayTasks(board.DoingTasks);
    Console.WriteLine("Tasks in Done:");
    DisplayTasks(board.DoneTasks);

    // Get task to delete
    Console.WriteLine("Enter the ID of the task you want to delete:");
    int taskId = int.Parse(Console.ReadLine());

    var task = context.Tasks.FirstOrDefault(t => t.Id == taskId);
    if (task != null)
    {
        context.Tasks.Remove(task);
        context.SaveChanges();
        Console.WriteLine("Task deleted successfully!");
    }
    else
    {
        Console.WriteLine("Task not found.");
    }
}

static void DisplayTasks(IEnumerable<Tugas2___OOP.Task> tasks)
{
    foreach (var task in tasks)
    {
        Console.WriteLine($"Task ID: {task.Id}, Title: {task.Title}");
    }
}