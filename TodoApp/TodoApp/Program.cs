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
        public static int postrun = 1;
        public static string path = Directory.GetCurrentDirectory();
        static void Main(string[] args)
        {
            List<todo> todoList = new List<todo>();
            if (!(File.Exists(path + @"\ToDo.xml")))
            {
                WriteToXmlFile<List<todo>>(path + @"\ToDo.xml", todoList);
            }
            else
            {
                todoList = ReadFromXmlFile<List<todo>>(path + @"\ToDo.xml");
            }
            Console.WriteLine("Welcome to To-Do App!");
            Console.WriteLine("A command line based program to help track to-do list");
            Console.WriteLine("Created by Carlsen\n");
            while (true)
            {
                int option = Menu();
                if (option == 1)
                {
                    ToDoAdd(todoList);
                }
                else if (option == 2)
                {
                    ViewToDo(todoList);
                }
                else if (option == 3)
                {
                    RemoveToDo(todoList);
                }
                else if (option == 4)
                {
                    WriteToXmlFile<List<todo>>(path + @"\ToDo.xml", todoList);
                    break;
                }
                else if (option == 5)
                {
                    ViewToDoDev(todoList);
                }
                else
                {
                    Console.WriteLine("git gud");
                }
            }
        }

        static int Menu()
        {
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.WriteLine("                                       Main Menu                                            ");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. Add new task to To-Do list");
            Console.WriteLine("2. View To-Do list");
            Console.WriteLine("3. Remove item from To-Do list");
            Console.WriteLine("4. Exit");
            Console.WriteLine("\nEXPERIMENTAL SECTION!!!!");
            Console.WriteLine("Note: All objects created and viewed in the section below are stored in a different file.\n");
            Console.WriteLine("5. Developmental Version of viewing To-Do list. Use this to view tasks with long descriptions");
            Console.WriteLine("----------------------------------------------------------------------------------------------");
            Console.Write("\nSelect your option: ");
            int option = Convert.ToInt32(Console.ReadLine());
            return option;
        }
        static void ToDoAdd(List<todo>todoList)
        {
            Console.Clear();
            Console.WriteLine("1. Add new task to To-Do list\n");
            Console.Write("Enter the title of the task: ");
            string title = Console.ReadLine();
            Console.Write("Enter the description of the task: ");
            string desc = Console.ReadLine();
            todo t;
            t = new todo(title, desc);
            todoList.Add(t);
            WriteToXmlFile<List<todo>>(path + @"\ToDo.xml", todoList);
            if (postrun == 1)
                PostRun();
        }
        static void ViewToDo(List<todo>todoList)
        {
            Console.Clear();
            Console.WriteLine("2. View To-Do list\n");
            todo t;
            Console.WriteLine("\n{0,-5}{1,-30}{2,-75}", "No.", "Title", "Description");
            for (int i = 0; i < todoList.Count; i++)
            {
                t = todoList[i];
                Console.WriteLine("{0,-5}{1,-30}{2,-75}", (i + 1) + ". ", t.Title, t.Desc);
            }
            if (postrun == 1)
                PostRun();
        }
        static void RemoveToDo(List<todo>todoList)
        {
            Console.WriteLine("3. Remove item from To-Do list\n");
            postrun = 0;
            ViewToDo(todoList);
            postrun = 1;
            Console.Write("\nEnter the Task Number to remove: " );
            int remove = Convert.ToInt32(Console.ReadLine());
            todoList.RemoveAt(remove - 1);
            WriteToXmlFile<List<todo>>(path + @"\ToDo.xml", todoList);
            if (postrun == 1)
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
            else
                Console.Clear();
                postrun = 1;
        }
        static void ViewToDoDev(List<todo> todoList)
        {
            Console.Clear();
            Console.WriteLine("5. Developmental Version of viewing To-Do list\n");
            todo t;
            Console.WriteLine("\n{0,-5}{1,-30}", "No.", "Title");
            for (int i = 0; i < todoList.Count; i++)
            {
                t = todoList[i];
                Console.WriteLine("{0,-5}{1,-30}", (i + 1) + ". ", t.Title);
            }
            Console.WriteLine("Which task do you want to view?");
            Console.Write("\nSelect a task number: ");
            int select = Convert.ToInt32(Console.ReadLine());
            t = todoList[select - 1];
            Console.WriteLine("\nTask Number " + select + "\t\tTitle: " + t.Title);
            Console.WriteLine("\nDescription:\n" + t.Desc);
            if (postrun == 1)
                PostRun();
        }
    }
}
