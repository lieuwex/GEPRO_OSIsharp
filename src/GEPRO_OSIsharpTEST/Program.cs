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
            Console.WriteLine("School ID? :");
            uint schoolID = Convert.ToUInt32(Console.ReadLine());
            Console.WriteLine("Naam/Leerlingnummer? :");
            string userName = Console.ReadLine();

            var afdelingen = MagisterRooster.GetAfdelingen(schoolID);

            for (int i = 0; i < afdelingen.Count; i++)
                Console.WriteLine(i + " " + afdelingen[i]);

            var afdeling = afdelingen[Convert.ToInt32(Console.ReadLine())];

            Console.ForegroundColor = ConsoleColor.Green; Console.Clear();

            var rooster = MagisterRooster.GetRooster(schoolID, userName, afdeling);
            foreach(var day in rooster)
            {
                foreach(var lesuur in day)
                {
                    if (!lesuur[0].IsEmpty()) Console.WriteLine("Dag: " + lesuur[0].Dag + ", Uur: " + lesuur[0].Uur + ":   " + lesuur[0].Vak.Naam);
                }
            }

            var roosterVandaag = MagisterRooster.GetByDay(rooster, DateTime.Today.DayOfWeek);
            Console.WriteLine("Vandaag:");
            foreach (var day in rooster)
            {
                for (int i = 0; i < day.Count; i++ )
                {
                    if (!day[i].Any(x => x.IsEmpty())) Console.WriteLine("Uur: " + day[i][0].Uur + ":   " + day[i][0].Vak.Naam);
                }
            }

            var linqTest = new List<List<Lesuur>>();
            foreach (var dag in rooster)
                linqTest.Add(dag.Where(x => x.Any(y => !y.IsEmpty()) && x.Any(y => y.isGewijzigd == true)).ToList()[0]); //LINQ enabled :)

            Console.ReadLine();
        }
    }
}
