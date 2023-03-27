using System;
using System.Collections.Generic;

namespace SecondLabNet
{
    [Serializable]
    public abstract class Animal : MemberOfZoo, IComparable<Animal>
    {
        public Cages Cage { get; set; }
        public abstract double CalcCageVolume();
        public double Height { get; set; }
        public double Width { get; set; }
        public double Length { get; set; }

        public Animal(Int16 _id, string _nameOfZoopark, string _name, Sexs _sex, double _height, double _width, double _length) : base(_id, _nameOfZoopark, _name, _sex)
        {
            Height = _height;
            Width = _width;
            Length = _length;
        }

        public Animal()
        {

        }

        public int CompareTo(Animal anim)
        {
            if (anim is null) throw new ArgumentException("Некорректное значение параметра");
            return TakeCageVolume.CompareTo(anim.TakeCageVolume);
        }
    }

    public class AnimalCompare : IComparer<Animal>
    {
        public int Compare(Animal animal1, Animal animal2)
        {
            return animal1.TakeCageVolume.CompareTo(animal2.TakeCageVolume);
        }
    }
}
