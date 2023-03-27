using System;

namespace SecondLabNet
{
    [Serializable]
    public class Predator : Animal
    {
        public Predator(Int16 _id, string _nameOfZoopark, string _name, Sexs _sex, double _height, double _width, double _length) : base(_id, _nameOfZoopark, _name, _sex, _height, _width, _length) { }
        public Predator() { }
        public override double CalcCageVolume()
        {
            return Height * Width * Length * 10.0 * 35.0 * 35.0;
        }
        public override Cages TakeCageVolume
        {
            get
            {
                if (CalcCageVolume() > 12250) return Cages.OutdoorEnclosure;
                else if (CalcCageVolume() > 6945) return Cages.Large;
                else if (CalcCageVolume() > 1470) return Cages.Medium;
                else return Cages.Small;
            }
        }

        public override string GetInfo
        {
            get { return $"ID: {this.ID}, Вид животного: Хищник, Имя: {this.Name}, Пол: {this.Sex},\n" +
                $"\tВысота: {this.Height} м, Ширина: {this.Width} м, Длинна: {this.Length} м, Объём клетки: {this.TakeCageVolume}\n"; }
        }
    }
}
