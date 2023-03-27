using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SecondLabNet
{
    class Program
    {
        private static byte PrintMenu()
        {
            Console.Write("Предлагаемые действия со списками животных и сотрудников:\n" +
                                "\t1 - Вывести данные всех животных на экран\n" +
                                "\t2 - Вывести данные всех сотрудников на экран\n" +
                                "\t3 - Отсортировать список животных по полу и вывести на экран\n" +
                                "\t4 - Отсортировать список сотрудников по имени и вывести на экран\n" +
                                "\t5 - Сравнить клетки двух животных\n" +
                                "\t6 - Найти животное по имени\n" +
                                "\t7 - Найти сотрудника по имени\n" +
                                "\t8 - Удалить первое вхождение элемента по имени\n" +
                                "\t9 - Записать содержимое коллекций в файлы JSON\n" +
                                "\t10 - Записать содержимое коллекций в файлы XML\n" +
                                "Если желаете прекратить работу программы, нажмите Enter, не вводя команды\n\n" +
                                "Вводимая команда: ");
            if (byte.TryParse(Console.ReadLine(), out var command))
                return command;
            return 0;
        }

        private static byte HowToLoadConfig()
        {
            Console.Write("Из какого файла загрузить списки животных и сотрудников?\n" +
                                "\t1 - Загрузить из файлов с расширением JSON\n" +
                                "\t2 - Загрузить из файлов с расширением XML\n\n" +
                                "Вводимая команда: ");
            if (byte.TryParse(Console.ReadLine(), out var howToLoadConfig))
                return howToLoadConfig;
            return 0;
        }

        private static async Task SortAnimalsAsync(GenericList<Animal> animalsList)
        {
            await Task.Run(() => animalsList.SortBySex());
        }

        static void Main(string[] args)
        {
            Console.SetWindowSize(100, 20);
            var animalsList = new GenericList<Animal>();
            var employeesList = new GenericList<Employee>();
            /*animalsList.Add(new Predator(1, "Zoo", "Лев", Sexs.Female, 1.2, 0.57, 2.03));
            animalsList.Add(new Predator(2, "Zoo", "Тигр", Sexs.Male, 1.44, 0.70, 2.33));
            animalsList.Add(new Predator(3, "Zoo", "Енот", Sexs.Male, 0.27, 0.30, 0.56));
            animalsList.Add(new Predator(4, "Zoo", "Лесной кот", Sexs.Male, 0.23, 0.15, 0.46));
            animalsList.Add(new Predator(5, "Zoo", "Рысь", Sexs.Female, 0.9, 0.45, 1.4));
            animalsList.Add(new Herbivore(6, "Zoo", "Носорог", Sexs.Male, 1.9, 1.05, 4.2));
            animalsList.Add(new Herbivore(7, "Zoo", "Жираф", Sexs.Female, 2, 0.88, 5.2));
            animalsList.Add(new Herbivore(9, "Zoo", "Жираф", Sexs.Male, 2.17, 0.9, 5.7));
            animalsList.Add(new Herbivore(8, "Zoo", "Коза", Sexs.Female, 0.65, 0.30, 0.99));
            employeesList.Add(new Employee(1, "Zoo", "Глаша", Sexs.Female, DateTime.Parse("21.06.1994"), "Рабочий кормокухни"));
            employeesList.Add(new Employee(2, "Zoo", "Маша", Sexs.Female, DateTime.Parse("01.01.1982"), "Специалист по закупкам"));
            employeesList.Add(new Employee(6, "Zoo", "Маша", Sexs.Female, DateTime.Parse("11.11.1975"), "Специалист по закупкам"));
            employeesList.Add(new Employee(3, "Zoo", "Фёдр Михалыч", Sexs.Male, DateTime.Parse("15.05.1968"), "Директор"));
            employeesList.Add(new Employee(4, "Zoo", "Васька-карандаш", Sexs.Male, DateTime.Parse("04.04.2010"), "Дворник-принеси-подай"));
            employeesList.Add(new Employee(5, "Zoo", "Александр Петрович", Sexs.Male, DateTime.Parse("20.06.1976"), "Водитель"));*/
            byte howToLoadConfig = 0;
            do
            {
                howToLoadConfig = HowToLoadConfig();
            } while (howToLoadConfig < 1 || howToLoadConfig > 2);
            if (howToLoadConfig == 1)
            {
                animalsList.LoadConfigFromJSON("List of animals.json");
                employeesList.LoadConfigFromJSON("List of employees.json");
            }
            else
            {
                animalsList.LoadConfigFromXML("C:\\Users\\User\\source\\repos\\C#\\ThirdCourse\\SecondLabNET\\SecondLabNet\\bin\\Debug\\List_of_animals.xml");
                employeesList.LoadConfigFromXML("C:\\Users\\User\\source\\repos\\C#\\ThirdCourse\\SecondLabNET\\SecondLabNet\\bin\\Debug\\List_of_employees.xml");
            }

            byte command = 0;
            do
            {
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
                Console.Clear();
                Console.SetWindowSize(100, 20);
                command = PrintMenu();
                if (command == 6)
                {
                    animalsList.elementFound += (members) => { Console.WriteLine("Какое счастье! Животинки найдены!"); };
                    animalsList.elementFound += (members) => 
                    {
                        foreach (var member in members)
                            Console.WriteLine($"Животное, которое вы искали содержится в {member.TakeCageVolume}, его индекс в списке {member.ID}");
                    };
                    Console.Write("Введите имя животного: ");
                    animalsList.Find(Console.ReadLine());
                }
                else if (command == 7)
                {
                    employeesList.elementFound += (members) => { Console.WriteLine("Какое счастье! Сотрудники найдены!"); };
                    employeesList.elementFound += (members) => 
                    {
                        foreach (var member in members)
                            Console.WriteLine($"Сотрудник, которого вы искали, {member.JobTitle}, его индекс в списке {member.ID}");
                    };
                    Console.Write("Введите имя сотрудника: ");
                    employeesList.Find(Console.ReadLine());
                }
                else if (command == 3)
                {
                    Console.WriteLine($"Main: ThreadID: {Thread.CurrentThread.ManagedThreadId}");
                    SortAnimalsAsync(animalsList);
                }
                else if (command == 10)
                {
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
                }
                else InputManager.ProcessInput<GenericList<MemberOfZoo>>(animalsList, employeesList, command);
            } while (command != 0);


            /*IEnumerable<MembersOfZoo> animList = animalsList.GetGenericList();
            foreach (var anim in animList)
            {
                Console.WriteLine($"Зоопарк: {anim.NameOfZoopark}");
            }

            IVisiting<Person> firstPerson = new NameOfVisitor();
            IVisiting<ChildPerson> firstPersonChild = firstPerson;
            firstPersonChild.Visit(new ChildPerson("Вася Пупкин"));*/
            /*Person p1 = new Person("Тест 1");
            Person p2 = new Person("Тест 2");
            Person p3 = new Person("Тест 2");
            Person p4 = new Person("Тест 3");
            List<Person> persons = new List<Person>();
            persons.Add(p1);
            persons.Add(p2);
            persons.Add(p3);
            persons.Add(p4);*/



            /*string output = JsonConvert.SerializeObject(persons, Formatting.Indented);
            using (TextWriter textWriter = new StreamWriter("Test1.json"))
                textWriter.WriteLine(output);*/


            Console.ReadKey();
        }
    }
}
