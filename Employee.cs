using System;
using System.Collections.Generic;

namespace SecondLabNet
{
    [Serializable]
    public class Employee : MemberOfZoo, IComparable<Employee>
    {
        public DateTime Birthday { get; set; }
        public override string JobTitle { get; set; }

        public Employee(Int16 _id, string _nameOfZoopark, string _name, Sexs _sex, DateTime _birthday, string _jobTitle) : base(_id, _nameOfZoopark, _name, _sex)
        {
            Birthday = _birthday;
            JobTitle = _jobTitle;
        }

        public Employee() { }

        public int CompareTo(Employee emp)
        {
            if (emp is null) throw new ArgumentException("Некорректное значение параметра");
            return Birthday.CompareTo(emp.Birthday);
        }

        public override string GetInfo
        {
            get
            {
                return $"ID: {this.ID}, Имя: {this.Name}, Пол: {this.Sex},\n" +
                                $"\tДата рождения: {this.Birthday}, Должность: {this.JobTitle}\n";
            }
        }
    }

    public class BirthdaySort : IComparer<Employee>
    {
        public int Compare(Employee emp1, Employee emp2)
        {
            return emp1.Birthday.CompareTo(emp2.Birthday);
        }
    }
}
