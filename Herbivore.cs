using System;

namespace SecondLabNet
{
    [Serializable]
    public class Herbivore : Animal
    {
        public Herbivore(Int16 _id, string _nameOfZoopark, string _name, Sexs _sex, double _height, double _width, double _length) : base(_id, _nameOfZoopark, _name, _sex, _height, _width, _length) { }
        public Herbivore() { }

        public override double CalcCageVolume()
        {
            return Height * Width * Length * 5.0 * 50.0 * 45.0;
        }
        public override Cages TakeCageVolume
        {
            get
            {
                if (CalcCageVolume() > 83362) return Cages.OutdoorEnclosure;
                else if (CalcCageVolume() > 32175) return Cages.Large;
                else if (CalcCageVolume() > 5670) return Cages.Medium;
                else return Cages.Small;
            }
        }

        public override string GetInfo
        {
            get
            {
                return $"ID: {this.ID}, Вид животного: Травоядное, Имя: {this.Name}, Пол: {this.Sex},\n" +
              $"\tВысота: {this.Height} м, Ширина: {this.Width} м, Длинна: {this.Length} м, Объём клетки: {this.TakeCageVolume}\n";
            }
        }
    }
}
