using System;
using System.Threading;

namespace SecondLabNet
{
    class InputManager
    {
        public static void ProcessInput<T>(GenericList<Animal> animalsList, GenericList<Employee> employeesList, byte command) where T: GenericList<MemberOfZoo>
        {
            var logger = new Logger<MyEventArgs>();
            var consoleLogger = new ConsoleLogger(logger);
            using (FileLogger fileLogger = new FileLogger("log.txt", logger))
            {
                Action consoleOutputInfoAnimals = () =>
                {
                    Console.SetWindowSize(110, 40);
                    foreach (var element in animalsList.GetGenericList())
                        Console.WriteLine(element.GetInfo);
                };
                Action consoleOutputInfoEmployees = () =>
                {
                    Console.SetWindowSize(110, 40);
                    foreach (var element in employeesList.GetGenericList())
                        Console.WriteLine(element.GetInfo);
                };

                switch (command)
                {
                    case 1:
                        consoleOutputInfoAnimals();
                        break;


                    case 2:
                        consoleOutputInfoEmployees();
                        break;


                    /*case 3:
                        await Task.Run(() => animalsList.SortBySex());
                        //Console.WriteLine("\nСписок животных отсортирован по полу");
                        consoleOutputInfoAnimals();
                        //logger.OnLoggerEvent(new MyEventArgs("..::Sort by Sex done::.."));
                        break;*/


                    case 4:
                        logger.OnLoggerEvent(new MyEventArgs($"..::Сортировка списка сотрудников по имени началась ThreadID: {Thread.CurrentThread.ManagedThreadId}::.."));
                        Thread sortByName = new Thread(() => employeesList.SortByName());
                        sortByName.Start();
                        sortByName.Join();
                        logger.OnLoggerEvent(new MyEventArgs($"..::Было отсортировано - {employeesList.GetCount()} элементов списка::.."));
                        logger.OnLoggerEvent(new MyEventArgs($"..::Сортировка списка сотрудников окончена ThreadID: {Thread.CurrentThread.ManagedThreadId}::.."));
                        //Console.WriteLine("\nСписок сотрудников зоопарка отсортирован по имени");
                        //consoleOutputInfoEmployees();
                        //logger.OnLoggerEvent(new MyEventArgs("..::Sort by Name done::.."));
                        break;


                    case 5:
                        Console.Write("\nВведите индексы двух животных из списка: ");
                        string[] input = Console.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        try
                        {
                            if (animalsList.CompareByCage(input))
                                logger.OnLoggerEvent(new MyEventArgs("..::Comparing by Cage done::.."));
                        }
                        catch (AnimalsNotFoundException e)
                        {
                            logger.OnLoggerEvent(new MyEventArgs($"#####::Comparing by Cage fault::#####\nExeption: {e}"));
                        }
                        catch (NoTwoArgumentsException e)
                        {
                            logger.OnLoggerEvent(new MyEventArgs($"#####::Comparing by Cage fault::#####\nExeption: {e}"));
                        }
                        break;


                    case 9:
                        OutputInFiles<Animal> outputAnimalJSON = new OutputInFiles<Animal>();
                        Func<string, GenericList<Animal>, bool> animalsListWritingJSON = outputAnimalJSON.SendToJSONFile;
                        if (animalsListWritingJSON("List of animals.json", animalsList))
                            Console.WriteLine("Данные о животных записаны в файл List of animals.json");
                        else
                            Console.WriteLine("Произошла ошибка при записи списка животных в файл!");

                        OutputInFiles<Employee> outputEmployeeJSON = new OutputInFiles<Employee>();
                        Func<string, GenericList<Employee>, bool> employeesListWritingJSON = outputEmployeeJSON.SendToJSONFile;
                        if (employeesListWritingJSON("List of employees.json", employeesList))
                            Console.WriteLine("Данные о сотрудниках записаны в файл List of employees.json");
                        else
                            Console.WriteLine("Произошла ошибка при записи списка сотрудников в файл!");
                        break;


                    case 8:
                        Console.Write("Введите имя животного или работника зоопарка: ");
                        string member = Console.ReadLine();
                        if (animalsList.Delete(member))
                        {
                            Console.WriteLine("Животное удалено!\n");
                            logger.OnLoggerEvent(new MyEventArgs("..::AnimalDelete done::.."));
                            break;
                        }
                        else if (employeesList.Delete(member))
                        {
                            Console.WriteLine("Работник удалён!\n");
                            logger.OnLoggerEvent(new MyEventArgs("..::EmployeeDelete done::.."));
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Работник или животное с таким именем не найдены!");
                            logger.OnLoggerEvent(new MyEventArgs("#####::MemberOfZooDelete fault::#####"));
                            break;
                        }


                    /*case 10:
                        OutputInFiles<Animal> outputAnimalXML = new OutputInFiles<Animal>();
                        Func<string, GenericList<Animal>, bool> animalsListWritingXML = outputAnimalXML.SendToXMLFile;
                        if (animalsListWritingXML("C:\\Users\\User\\source\\repos\\C#\\ThirdCourse\\SecondLabNET\\SecondLabNet\\bin\\Debug\\List_of_animals.xml", animalsList))
                            Console.WriteLine("Данные о животных записаны в файл List of animals.xml");
                        else
                            Console.WriteLine("Произошла ошибка при записи списка животных в файл!");

                        OutputInFiles<Employee> outputEmployeeXML = new OutputInFiles<Employee>();
                        Func<string, GenericList<Employee>, bool> employeesListWritingXML = outputEmployeeXML.SendToXMLFile;
                        if (employeesListWritingXML("C:\\Users\\User\\source\\repos\\C#\\ThirdCourse\\SecondLabNET\\SecondLabNet\\bin\\Debug\\List_of_employees.xml", employeesList))
                            Console.WriteLine("Данные о сотрудниках записаны в файл List of employees.xml");
                        else
                            Console.WriteLine("Произошла ошибка при записи списка сотрудников в файл!");
                        break;*/


                    default:
                        Console.WriteLine("\nНеверная команда\n");
                        break;
                }
            }
        }

    }
}
