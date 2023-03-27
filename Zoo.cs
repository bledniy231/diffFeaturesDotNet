using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace SecondLabNet
{
    public enum Cages
    {
        Small,
        Medium,
        Large,
        OutdoorEnclosure
    }
    public enum Sexs
    {
        Male,
        Female
    }

    [XmlRoot(ElementName = "MemberOfZoo", Namespace = "SecondLabNet")]
    [XmlInclude(typeof(Predator))]
    [XmlInclude(typeof(Herbivore))]
    [XmlInclude(typeof(Employee))]
    [Serializable]
    public abstract class MemberOfZoo : IZoopark<MemberOfZoo>, IEquatable<MemberOfZoo>
    {
        public Int16 ID { get; set; }
        public string NameOfZoopark { get; set; }
        public string Name { get; set; }
        public Sexs Sex { get; set; }
        [XmlIgnore]
        public virtual string JobTitle { get; set; }
        [JsonIgnore]
        public virtual Cages TakeCageVolume { get; }
        [JsonIgnore]
        public virtual string GetInfo { get; }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            MemberOfZoo objAsMemberOfZoo = obj as MemberOfZoo;
            if (objAsMemberOfZoo == null) return false;
            else return Equals(objAsMemberOfZoo);
        }
        public bool Equals(MemberOfZoo other)
        {
            if (other == null) return false;
            return (this.ID.Equals(other.ID));
        }
        public override int GetHashCode()
        {
            return ID;
        }

        public MemberOfZoo(Int16 _id, string _nameOfZoo, string _name, Sexs _sex)
        {
            ID = _id;
            NameOfZoopark = _nameOfZoo;
            Name = _name;
            Sex = _sex;
        }

        public MemberOfZoo() { }
    }
}
