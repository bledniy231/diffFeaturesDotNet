using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace SecondLabNet
{
    public class GenericList<T> where T: MemberOfZoo
    {
        public delegate void ElementFound(List<T> members);
        public event ElementFound elementFound;
        public void OnElementFound(List<T> members)
        {
            elementFound?.Invoke(members);
        }

        private List<T> genericList = new List<T>();

        public void LoadConfigFromJSON(string path)
        {
            var logger = new Logger<MyEventArgs>();
            var consoleLogger = new ConsoleLogger(logger);
            using (FileLogger fileLogger = new FileLogger("log.txt", logger))
            {
                using (StreamReader streamReader = new StreamReader(path))
                {
                    try
                    {
                        var settings = new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto,
                            NullValueHandling = NullValueHandling.Ignore,
                            Formatting = Newtonsoft.Json.Formatting.Indented
                        };
                        string input = streamReader.ReadToEnd();
                        genericList = JsonConvert.DeserializeObject<List<T>>(input, settings);
                        logger.OnLoggerEvent(new MyEventArgs("..::MembersOfZooListReadingJSON done::.."));
                    }
                    catch (FileNotFoundException e)
                    {
                        logger.OnLoggerEvent(new MyEventArgs($"#####::MembersOfZooListReadingJSON fault::#####\nExeption: {e}"));
                    }
                    catch (FileLoadException e)
                    {
                        logger.OnLoggerEvent(new MyEventArgs($"#####::MembersOfZooListReadingJSON fault::#####\nExeption: {e}"));
                    }
                    catch (Exception e)
                    {
                        logger.OnLoggerEvent(new MyEventArgs($"#####::MembersOfZooListReadingJSON fault::#####\nExeption: {e}"));
                    }
                }
            }
        }

        public void LoadConfigFromXML(string path)
        {
            var logger = new Logger<MyEventArgs>();
            var consoleLogger = new ConsoleLogger(logger);
            using (FileLogger fileLogger = new FileLogger("log.txt", logger))
            {
                try
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>), new Type[] { typeof(T) });
                    using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        using (XmlReader xmlWriter = XmlReader.Create(fs))
                        {
                            genericList = serializer.Deserialize(xmlWriter) as List<T>;
                        }
                    }
                    logger.OnLoggerEvent(new MyEventArgs("..::MembersOfZooListReadingXML done::.."));
                }
                catch (Exception e)
                {
                    logger.OnLoggerEvent(new MyEventArgs($"#####::MembersOfZooListReading" +
                        $"XML fault::#####\nExeption: {e}"));
                }
            }
        }

        public void Add(T partOfList)
        {
            if (partOfList == null)
                throw new ArgumentException(nameof(partOfList));
            genericList.Add(partOfList);
        }

        public bool Delete(string input)
        {
            T elem = genericList.Find(member => member.Name == input);
            bool status = genericList.Remove(elem);
            return status;
        }

        public void SortByName()
        {
            Console.WriteLine($"Sort Employees ThreadID: {Thread.CurrentThread.ManagedThreadId}");
            genericList.Sort((x, y) => x.Name.CompareTo(y.Name));
        }

        public int GetCount()
        {
            return genericList.Count;
        }

        public async void SortBySex()
        {
            var logger = new Logger<MyEventArgs>();
            var consoleLogger = new ConsoleLogger(logger);
            using (FileLogger fileLogger = new FileLogger("log.txt", logger))
            {
                logger.OnLoggerEvent(new MyEventArgs($"..::Сортировка списка животных начата ThreadID: {Thread.CurrentThread.ManagedThreadId}::.."));
                int result = await ExactlySortBySex();
                logger.OnLoggerEvent(new MyEventArgs($"..::Было отсортировано - {result} элементов списка::.."));
                logger.OnLoggerEvent(new MyEventArgs($"..::Сортировка списка животных окончена ThreadID: {Thread.CurrentThread.ManagedThreadId}::."));
            }
        }

        private Task<int> ExactlySortBySex()
        {
            var task = new Task<int>(() => {
                genericList.Sort((x, y) => x.Sex.CompareTo(y.Sex));
                Console.WriteLine($"Sort Animals ThreadID: {Thread.CurrentThread.ManagedThreadId}");
                return genericList.Count;
            });
            task.Start();
            return task;
        }

        public bool CompareByCage(string[] input)
        {
            try
            {
                if (Int16.TryParse(input[0], out Int16 first) && (Int16.TryParse(input[1], out Int16 second)))
                {
                    genericList.Sort((x, y) => x.ID.CompareTo(y.ID));
                    Int16 idFirst = Search(first);
                    Int16 idSecond = Search(second);
                    return OutputCompareByCage(idFirst, idSecond);
                }
                else return false;
            }
            catch (IndexOutOfRangeException e)
            {
                throw new NoTwoArgumentsException($"Вы ввели не два индекса!\n{e}");
            }
            /*else
            {
                genericList.Sort((x, y) => x.Name.CompareTo(y.Name));
                Int16 idFirst = Search(input[0]);
                Int16 idSecond = Search(input[1]);
                OutputCompareByCage(idFirst, idSecond);
                Console.WriteLine("Вы ввели не индексы");
                return false;
            }*/
        }

        public Int16 Search(Int16 input) 
        {
            Int16 left = 0;
            Int16 right = (Int16)(genericList.Count - 1);
            if (left == right) return left;
            while (true)
            {
                if (right - left == 1)
                {
                    if (genericList[left].ID == input) return left;
                    if (genericList[right].ID == input) return right;
                    return -1;
                }
                else
                {
                    Int16 middle = (Int16)(left + (right - left) / 2);
                    if (genericList[middle].ID == input) return middle;
                    if (genericList[middle].ID < input) left = middle;
                    if (genericList[middle].ID > input) right = middle;
                }
            }
        }

        public void Find(string input)
        {
            OnElementFound(genericList.FindAll(member => member.Name == input));
            /*genericList.Sort((x, y) => x.Name.CompareTo(y.Name));
            Int16 left = 0;
            Int16 right = (Int16)(genericList.Count - 1);
            if (left == right) OnElementFound(genericList[left]);
            bool breakFlag = false;
            while (!breakFlag)
            {
                if (right - left == 1)
                {
                    if (genericList[left].Name == input) { OnElementFound(genericList[left]); breakFlag = true; }
                    else if (genericList[right].Name == input) { OnElementFound(genericList[right]); breakFlag = true; }
                    else
                    {
                        Console.WriteLine($"Элемент с именем {input} не найден");
                        breakFlag = true;
                    }
                }
                else
                {
                    Int16 middle = (Int16)(left + (right - left) / 2);
                    if (genericList[middle].Name == input) OnElementFound(genericList[middle]);
                    if (String.Compare(genericList[middle].Name, input) <= 0) left = middle;
                    if (String.Compare(genericList[middle].Name, input) >= 0) right = middle;
                }
            }*/
        }


        public bool OutputCompareByCage(Int16 idFirst, Int16 idSecond)
        {
            if (idFirst >= 0 && idSecond >= 0)
            {
                if (genericList[idFirst].TakeCageVolume == genericList[idSecond].TakeCageVolume) Console.WriteLine($"Клетки выбранных животных одинакового типа {genericList[idFirst].TakeCageVolume}\n");
                else if (genericList[idFirst].TakeCageVolume > genericList[idSecond].TakeCageVolume) Console.WriteLine($"Клетка первого животного({genericList[idFirst].TakeCageVolume}) больше, чем клетка второго({genericList[idSecond].TakeCageVolume})\n");
                else Console.WriteLine($"Клетка второго животного({genericList[idSecond].TakeCageVolume}) больше, чем клетка первого({genericList[idFirst].TakeCageVolume})\n");
                return true;
            }
            else
            {
                throw new AnimalsNotFoundException("Животных с такими индексами не существует!");
            }
        }

        public IEnumerable<T> GetGenericList()
        {
            return genericList;
        }

        public List<T> GetList()
        {
            return genericList;
        }
    }

    public class OutputInFiles<T> where T : MemberOfZoo
    {

        public bool SendToJSONFile(string path, GenericList<T> list)
        {
            var logger = new Logger<MyEventArgs>();
            var consoleLogger = new ConsoleLogger(logger);
            using (FileLogger fileLogger = new FileLogger("log.txt", logger))
            {
                try
                {
                    /*var serializer = new XmlSerializer(typeof(T));
                    using (var writer = new StreamWriter(path))
                    {
                        foreach (var element in list.GetGenericList())
                        {
                            serializer.Serialize(writer, element);
                        }
                    }*/
                    /*using (TextWriter textWriter = new StreamWriter(path))
                        foreach (var element in list.GetGenericList())
                            textWriter.WriteLine(element.GetInfo);*/
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Auto,
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Newtonsoft.Json.Formatting.Indented
                    };
                    string output = JsonConvert.SerializeObject(list.GetList(), settings);
                    //using (TextWriter textWriter = new StreamWriter(path))
                    //textWriter.WriteLine(output);
                    File.WriteAllText(path, output);
                    logger.OnLoggerEvent(new MyEventArgs("..::MembersOfZooListWritingJSON done::.."));
                    return true;
                }
                catch (Exception e)
                {
                    logger.OnLoggerEvent(new MyEventArgs($"#####::MembersOfZooListWritingJSON fault::#####\nExeption: {e}"));
                    return false;
                }
            }
        }

        public bool SendToXMLFile(string path, GenericList<T> list)
        {
            var logger = new Logger<MyEventArgs>();
            var consoleLogger = new ConsoleLogger(logger);
            using (FileLogger fileLogger = new FileLogger("log.txt", logger))
            {
                try
                {
                    XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    XmlWriterSettings xmlSettings = new XmlWriterSettings
                    {
                        Indent = true,
                        OmitXmlDeclaration = true
                    };
                    XmlSerializer serializer = new XmlSerializer(/*typeof(GenericList<T>)*/typeof(List<T>), new Type[] { typeof(T) });
                    using (/*var stringWriter = new StringWriter()*/FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                    {
                        using (XmlWriter xmlWriter = XmlWriter.Create(fs, xmlSettings))
                        {
                            //foreach (var element in list.GetGenericList())
                            serializer.Serialize(xmlWriter, list.GetList(), xmlNamespaces);
                        }
                    }
                    logger.OnLoggerEvent(new MyEventArgs("..::MembersOfZooListWritingXML done::.."));
                    return true;
                }
                catch (Exception e)
                {
                    logger.OnLoggerEvent(new MyEventArgs($"#####::MembersOfZooListWritingXML fault::#####\nExeption: {e}"));
                    return false;
                }
            }
        }
    }
}
