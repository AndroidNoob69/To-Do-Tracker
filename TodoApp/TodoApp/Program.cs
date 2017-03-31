using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; //Allows program to read write files

namespace TodoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to To-Do App!");
            Console.WriteLine("A command line based program to help track to-do list");
            Console.WriteLine("Created by Carlsen \n \n \n");
            while (true)
            {
                int option = Menu();
                if (option == 1)
                {
                    Console.WriteLine("Please enter the description of the task to add \n");
                    string task = Console.ReadLine();
                    Console.WriteLine("Adding To-Do... \n \n");
                    if (AddToDo(task) == true)
                        Console.WriteLine("Task added to To-Do list! \n");
                    else
                        Console.WriteLine("An error has occured while adding object in To-Do list");
                }
                if (option == 2)
                {
                    ReadFile();
                }
                if (option == 3)
                {
                    ReadFile();
                    Console.Write("\nSelect the task number to delete: ");
                    int remove = Convert.ToInt32(Console.ReadLine());
                    RemoveToDo(remove);
                }
                if (option == 4)
                {
                    break;
                }
            }
        }

        static int Menu()
        {
            Console.WriteLine("What would you like to do?");
            Console.WriteLine("1. Add new task to To-Do list");
            Console.WriteLine("2. View To-Do list");
            Console.WriteLine("3. Remove item from To-Do list");
            Console.WriteLine("4. Exit");
            Console.Write("Select your option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            return option;
        }
        static bool AddToDo(string task)
        {
            Console.WriteLine("Opening file....(Step 1/3)");
            TextWriter file = new StreamWriter(@"C:\Users\carls\Desktop\ToDo.txt", true); //Hardcoded path, not recommended
            Console.WriteLine("Writing to file....(Step 2/3)");
            file.WriteLine(task);
            Console.WriteLine("Closing File....(Step 3/3)");
            file.Close();
            return true;
        }
        
        static void ReadFile()
        {
            if (new FileInfo(@"C:\Users\carls\Desktop\ToDo.txt").Length == 0)
            {
                Console.WriteLine("\nTo-Do is empty! WhooHoo!\n");
            }
            else
            {
                Console.WriteLine("\nTo-Do List: \n");
                int counter = 0;
                string line;

                // Read the file and display it line by line.
                StreamReader file = new StreamReader(@"C:\Users\carls\Desktop\ToDo.txt");
                while ((line = file.ReadLine()) != null)
                {
                    Console.WriteLine((counter + 1) + ". " + line);
                    counter++;
                }

                file.Close();
                Console.WriteLine("");
            }
        }
        
        static void RemoveToDo(int remove)
        {
            string line = null;
            int line_number = 0;
            int line_to_delete = remove;
            int taskexists = 0;
            Console.WriteLine("\nRemoving Task....");

            using (StreamReader reader = new StreamReader(@"C:\Users\carls\Desktop\ToDo.txt"))
            {
                using (StreamWriter writer = new StreamWriter(@"C:\Users\carls\Desktop\ToDo2.txt"))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        line_number++;

                        if (line_number == line_to_delete)
                        {
                            Console.WriteLine("Task Found! Deleting.");
                            taskexists = 1;
                            continue;
                        }
                        
                        writer.WriteLine(line);
                    }
                }
            }
            if (taskexists == 0)
            {
                Console.WriteLine("Specified task does not exist! Please try again!");
            }
            Console.WriteLine("Applying clean up hack.... \n");
            File.Delete(@"C:\Users\carls\Desktop\ToDo.txt");
            File.Move(@"C:\Users\carls\Desktop\ToDo2.txt", @"C:\Users\carls\Desktop\ToDo.txt");
        }
    }
}
