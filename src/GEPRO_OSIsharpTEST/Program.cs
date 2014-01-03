//GEPRO-OSIsharp
//(c) 2014 Lieuwe Rooijakkers
//GEPRI_OSIsharp Showcase/Test Program :D
using System;
using System.Collections.Generic;
using System.Linq;
using GEPRI_OSIsharp;

namespace GEPRI_OSIsharpTEST
{
    class Program
    {
        static void Main(string[] args)
        {
            var rooster = MagisterRooster.GetRooster(181,"120678","4V");
            foreach(var day in rooster)
            {
                foreach(var lesuur in day)
                {
                    if (lesuur != null) Console.WriteLine("Dag: " + lesuur.Dag + ", Uur: " + lesuur.Uur + ":   " + lesuur.Vak.Naam);
                }
            }

            var roosterVandaag = MagisterRooster.GetByDay(rooster, DateTime.Today.DayOfWeek);
            Console.WriteLine("Vandaag:");
            foreach (var day in rooster)
            {
                foreach (var lesuur in day)
                {
                    if (lesuur != null) Console.WriteLine("Uur: " + lesuur.Uur + ":   " + lesuur.Vak.Naam);
                }
            }

            var linqTest = new List<List<Lesuur>>();
            foreach(var dag in rooster)
                linqTest.Add(dag.Where(x => x != null && x.isGewijzigd == true).ToList()); //LINQ enabled :)

            Console.ReadLine();
        }
    }
}
