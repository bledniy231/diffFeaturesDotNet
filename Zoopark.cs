using System;


namespace SecondLabNet

{
    public interface IZoopark<out T>
    {

       string NameOfZoopark { get; set; }

       string Name { get; set; }
    
       Sexs Sex { get; set; }
    }
}
