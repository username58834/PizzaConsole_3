using Microsoft.VisualBasic.FileIO;
using PizzaConsole_3;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using System;
using System.Text;

namespace PizzaConsole_3
{
    internal class Program
    {
        static internal List<PizzaClass> pizzas = new List<PizzaClass>();
        static void DrawMenu()
        {
            Console.WriteLine(
                "------------------------------------\n" +
                "| 1 - Додати об'єкт                |\n" +
                "| 2 - Переглянути всі об'єкти      |\n" +
                "| 3 - Знайти об'єкт                |\n" +
                "| 4 - Продемонструвати поведінку   |\n" +
                "| 5 - Видадити об'єкт              |\n" +
                "| 0 - Вийти з застосунку           |\n" +
                "------------------------------------"
            );
        }

        static string FormatAnswer(string regex, string message)
        {
            string text = "";

            try
            {
                text = Console.ReadLine();
                while (!Regex.IsMatch(text, regex))
                {
                    Console.WriteLine(message);
                    text = Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return text;
        }

        static void ChooseIngredients(ref PizzaClass pizza)
        {
            int command;
            while (true)
            {
                try
                {
                    Console.WriteLine("\nTo change ingredients write their number\n\n" +
                        $"{pizza.ShowIngredientsOnly()}" +
                        "0 - Add and close\n");

                    command = int.Parse(Console.ReadLine());
                    if (command == 0)
                    {
                        break;
                    }
                    else if (command < Enum.GetValues(typeof(Ingredients)).Length)
                    {
                        pizza.ChangeIngredients((Ingredients)(command));
                    }
                    else
                    {
                        throw new Exception($"Write a number between 0 and {Enum.GetValues(typeof(Ingredients)).Length - 1}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        static void Add()
        {
            int type = -1;
            string name = "";
            float price = 0;
            double weight = 0;

            Console.WriteLine(
                "Enter a number to call the following constructor:\n" +
                "0 - PizzaClass()\n" +
                "1 - PizzaClass(string name)\n" +
                "2 - PizzaClass(string name, float price)\n" +
                "3 - PizzaClass(string name, float price, double weight)"
            );
            while(!int.TryParse(Console.ReadLine(), out type) || type < 0 || type > 3)
            //do
            {                
                //if(!int.TryParse(Console.ReadLine(), out type))
                //{
                    Console.WriteLine("Enter a number between 0 and 3");
                //}
            }// while (type < 0 || type > 3);

            PizzaClass pizza = null;
            do
            {
                try
                {
                    if (type > 0)
                    {
                        Console.WriteLine("\nEnter  name:");
                        name = Console.ReadLine();
                    }
                    if (type > 1)
                    {
                        Console.WriteLine("Enter  price:");
                        while (!float.TryParse(Console.ReadLine(), out price))
                        {
                            Console.WriteLine("Write a number");
                        }
                    }
                    if (type > 2)
                    {
                        Console.WriteLine("Enter  weight:");
                        while (!double.TryParse(Console.ReadLine(), out weight))
                        {
                            Console.WriteLine("Write a number");
                        }
                    }

                    switch (type)
                    {
                        case 0:
                            pizza = new PizzaClass() { Weight = new Random().NextDouble() + 0.1};
                            break;
                        case 1:
                            pizza = new PizzaClass(name);
                            break;
                        case 2:
                            pizza = new PizzaClass(name, price);
                            break;
                        case 3:
                            pizza = new PizzaClass(name, price, weight);
                            break;
                        default:
                            pizza = new PizzaClass();
                            break;
                    }

                    ChooseIngredients(ref pizza);
                    pizzas.Add(pizza);
                    Console.WriteLine($"Pizza with the following parameters \"{pizza.Info}\" was added");
                } 
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            } while (pizza == null);
        }
        static void ShowAllDetailed()
        {
            Console.WriteLine($"You have {pizzas.Count} pizzas");
            foreach (PizzaClass pizza in pizzas)
            {
                Console.WriteLine($"{pizza.Show()}");
            }
        }
        static void Find()
        {
            Console.WriteLine("Choose type of searching\n0 - Search by name\n1 - Search by maximum price");
            int command = 2;
            string name;
            float price;
            bool was = false;

            while (true)
            {
                try
                {
                    command = int.Parse(Console.ReadLine());
                    if (command < 0 || command > 1)
                    {
                        throw new Exception("Write a number between 0 and 1");
                    }
                    else break;
                }

                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }

            try
            {
                if (command == 0)
                {
                    Console.WriteLine("Enter  name:");
                    name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                    while (name.Length < 3 || name.Length > 12)
                    {
                        Console.WriteLine("Write from 3 to 12 characters. Only Latin letters are allowed");
                        name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                    }

                    foreach (PizzaClass pizza in pizzas)
                    {
                        if (pizza.Name == name)
                        {
                            Console.WriteLine($"\n{pizza.Show()}");
                            was = true;
                        }
                    }
                }
                else if (command == 1)
                {
                    Console.WriteLine("Enter  maximum price:");
                    price = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,2})?$", "Only numbers are allowed. The price must be greater than 0"));
                    while (price <= 0)
                    {
                        Console.WriteLine("Only numbers are allowed. The price must be greater than 0");
                        price = float.Parse(FormatAnswer(@"^(0|[1-9]\d*)(\.\d{0,2})?$", "Only numbers are allowed. The price must be greater than 0"));
                    }

                    foreach (PizzaClass pizza in pizzas)
                    {
                        if (pizza.Price <= price)
                        {
                            Console.WriteLine($"\n{pizza.Show()}");
                            was = true;
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            if (!was)
            {
                Console.WriteLine("\nNo items found");
            }
        }

        static void Delete()
        {
            int command;
            if (pizzas.Count == 0)
            {
                Console.WriteLine("Nothing to delete");
            }
            else
            {
                Console.WriteLine("Choose type of searching\n0 - Delete by name\n1 - Delete by index");

                while (true)
                {
                    try
                    {
                        command = int.Parse(Console.ReadLine());
                        if (command < 0 || command > 1)
                        {
                            throw new Exception("Write a number between 0 and 1");
                        }
                        else
                        {
                            if (command == 0) DeleteByName();
                            if (command == 1) DeleteByIndex();
                            break;
                        }
                    }

                    catch (Exception ex) { Console.WriteLine(ex.Message); }
                }
            }
        }

        static int GetIndex()
        {
            int command;
            Console.WriteLine($"Write a number between {0} and {pizzas.Count - 1}");
            while (true)
            {
                try
                {
                    command = int.Parse(Console.ReadLine());
                    if (command < 0 || command >= pizzas.Count)
                    {
                        throw new Exception($"Write a number between {0} and {pizzas.Count - 1}");
                    }
                    else
                    {
                        return command;
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }
            }
        }
        static void DeleteByIndex()
        {
            pizzas.RemoveAt(GetIndex());
            Console.WriteLine("The item was successfully deleted");
        }

        static void DeleteByName()
        {
            string name = "";
            Console.WriteLine("Enter  name:");
            try
            {
                name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                while (name.Length < 3 || name.Length > 12)
                {
                    Console.WriteLine("Write from 3 to 12 characters. Only Latin letters are allowed");
                    name = FormatAnswer("^[A-Za-z ]*$", "Write from 3 to 12 characters. Only Latin letters are allowed");
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            bool was = false;
            for (int i = 0; i < pizzas.Count; i++)
            {
                if (name == pizzas[i].Name)
                {
                    pizzas.RemoveAt(i);
                    i--;
                    was = true;
                }
            }
            if (was) Console.WriteLine("The items were successfully deleted");
            else Console.WriteLine("Nothing to delete");
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int command;
            int N = 0;
            float money = 0;
            string text = "";

            Console.WriteLine("Write maximum number of elements:");

            while (!int.TryParse(Console.ReadLine(), out N) || N <= 0)
            {

                Console.WriteLine("Write a number greater than 0.\n");
            }

            Console.WriteLine("Enter  money:");

            while (!float.TryParse(Console.ReadLine(), out money) || !Regex.IsMatch(money.ToString(), @"^(0|[1-9]\d*)(\.\d{0,2})?$") || money <= 0)
            {
                Console.WriteLine("Only numbers are allowed. The amount of money must be greater than 0.\nFormat: any digits before the decimal point, and up to 2 digits after it.");
            }

            DrawMenu();

            while (true)
            {
                try
                {
                    Console.WriteLine($"\nYou have {money.ToString("F2")}$\nEnter a number between 0-5");

                    command = int.Parse(Console.ReadLine());
                    switch (command)
                    {
                        case 0:
                            return;
                        case 1:
                            if (pizzas.Count < N) Add();
                            else throw new Exception("You have already reached the limit");
                            break;
                        case 2:
                            ShowAllDetailed();
                            break;
                        case 3:
                            Find();
                            break;
                        case 4:
                            if (pizzas.Count > 0)
                            {
                                for (int i = 0; i < pizzas.Count; i++)
                                {
                                    pizzas[i].ChangeState();
                                    Console.Write($"{(pizzas[i].State == States.Spoiled ? "" : $"{i}: ")} {pizzas[i].Name} Status -> {pizzas[i].State}");
                                    if (pizzas[i].ThrowAwayIfSpoiled())
                                    {
                                        pizzas.RemoveAt(i);
                                        i--;
                                        Console.WriteLine(" -> The pizza was thrown away\n");
                                    }
                                    else Console.WriteLine(" -> Everything is good\n");
                                }

                                if (pizzas.Count > 0)
                                {
                                    do
                                    {
                                        Console.WriteLine("Do you want to receive a detailed review? (y/n)");
                                        text = Console.ReadLine();
                                    } while (text != "y" && text != "n");

                                    Console.WriteLine("To buy a pizza ");

                                    if (text == "n")
                                    {
                                        if (pizzas[GetIndex()].Sell(ref money))
                                        {
                                            Console.WriteLine($"The pizza was bought. Money left {money.ToString("F2")}$");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Not enough money. Money left {money.ToString("F2")}$");
                                        }
                                    }
                                    else
                                    {
                                        if (pizzas[GetIndex()].Sell(ref money, ref text))
                                        {
                                            Console.WriteLine($"{text}\nThe pizza was bought. {money.ToString("F2")}$");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{text}\nNot enough money. {money.ToString("F2")}$");
                                        }
                                    }
                                }
                                else Console.WriteLine("No fresh pizzas left");
                            }
                            else
                            {
                                Console.WriteLine($"Nothing to buy");
                            }
                            break;
                        case 5:
                            Delete();
                            break;
                        default:
                            Console.WriteLine("Unknown command");
                            break;
                    }

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n// Menu omitted to save screen space");
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}
