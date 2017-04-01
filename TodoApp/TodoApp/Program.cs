using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; //Allows program to read write files
using System.Xml;
using System.Xml.Serialization;

namespace TodoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<todo> todoList = new List<todo>();
            List<todo> todoList = ReadFromXmlFile<List<todo>>(@"C:\Users\carls\Desktop\ToDo.xml");
            Console.WriteLine("Welcome to To-Do App!");
            Console.WriteLine("A command line based program to help track to-do list");
            Console.WriteLine("Created by Carlsen\n");
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
                else if (option == 2)
                {
                    ReadFile();
                }
                else if (option == 3)
                {
                    ReadFile();
                    Console.Write("\nSelect the task number to delete: ");
                    int remove = Convert.ToInt32(Console.ReadLine());
                    RemoveToDo(remove);
                }
                else if (option == 4)
                {
                    break;
                }
                else if (option == 5)
                {
                    ExperimentToDoAdd(todoList);
                }
                else if (option == 6)
                {
                    ViewExperimentToDo(todoList);
                }
                else
                {
                    Console.WriteLine("git gud");
                }
            }
        }

        static int Menu()
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            Console.WriteLine("                                       Main Menu                                            ");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. Add new task to To-Do list");
            Console.WriteLine("2. View To-Do list");
            Console.WriteLine("3. Remove item from To-Do list");
            Console.WriteLine("4. Exit");
            Console.WriteLine("\nEXPERIMENTAL SECTION!!!!");
            Console.WriteLine("Note: All objects created and viewed in the section below are stored in a different file.\n");
            Console.WriteLine("5. [EXPERIMENTAL!] Add new task to experimental To-Do list");
            Console.WriteLine("6. [EXPERIMENTAL!] View experimental To-Do list");
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.Write("\nSelect your option: ");
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
            PostRun();
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
                PostRun();
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
            PostRun();
        }

        static void ExperimentToDoAdd(List<todo>todoList)
        {
            Console.WriteLine("EXPERIMENTAL TODO LIST!!!\n");
            Console.Write("Enter the title of the task: ");
            string title = Console.ReadLine();
            Console.Write("Enter the description of the task: ");
            string desc = Console.ReadLine();
            todo t;
            t = new todo(title, desc);
            todoList.Add(t);
            WriteToXmlFile<List<todo>>(@"C:\Users\carls\Desktop\ToDo.xml", todoList);
            PostRun();
        }
        static void ViewExperimentToDo(List<todo>todoList)
        {
            todo t;
            Console.WriteLine("{0,-5}{1,-30}{2,-75}", "No.", "Title", "Description");
            for (int i = 0; i < todoList.Count; i++)
            {
                t = todoList[i];
                Console.WriteLine("{0,-5}{1,-30}{2,-75}", (i + 1) + ". ", t.Title, t.Desc);
            }
            PostRun();
        }
        /// <summary>
        /// Writes the given object instance to an XML file.
        /// <para>Only Public properties and variables will be written to the file. These can be any type though, even other classes.</para>
        /// <para>If there are public properties/variables that you do not want written to the file, decorate them with the [XmlIgnore] attribute.</para>
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object being written to the file.</typeparam>
        /// <param name="filePath">The file path to write the object instance to.</param>
        /// <param name="objectToWrite">The object instance to write to the file.</param>
        /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new()
        {
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Reads an object instance from an XML file.
        /// <para>Object type must have a parameterless constructor.</para>
        /// </summary>
        /// <typeparam name="T">The type of object to read from the file.</typeparam>
        /// <param name="filePath">The file path to read the object instance from.</param>
        /// <returns>Returns a new instance of the object read from the XML file.</returns>
        public static T ReadFromXmlFile<T>(string filePath) where T : new()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
        static void PostRun()
        {
            Console.WriteLine("\nOperation Completed! Please select an option");
            Console.WriteLine("1. Return to Main Menu");
            Console.WriteLine("2. Exit");
            Console.Write("Selection: ");
            int option = Convert.ToInt32(Console.ReadLine());
            if (option == 2)
            {
                Environment.Exit(0);
            }
        }
    }
}
