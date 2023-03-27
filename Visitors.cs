using System;

namespace SecondLabNet
{
    interface IVisiting<in T>
    {
        void Visit(T reaction);
    }
    class NameOfVisitor : IVisiting<Person>
    {
        public void Visit(Person person)
        {
            Console.WriteLine($"Зоопарк посетил {person.Name}");
        }
    }
    class Person
    {
        public string Name { get; set; }
        public Person(string name) => Name = name;
    }
    class ChildPerson : Person
    {
        public ChildPerson(string name) : base(name) { }
    }

}
