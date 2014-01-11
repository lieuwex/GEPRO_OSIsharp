GEPRO_OSIsharp
==============

C# Library voor Gepro-OSI roosters. Zou te gebruiken moeten zijn met elke school.

Zie, voor een PHP implementatie: https://github.com/stipmonster/api-for-magister-gepro-osi-roosters

Voor de geintereseerden, zie: https://github.com/lieuwex/MataSharp

GEBRUIK
==============
Voeg gewoon reference toe en zet dan
```csharp
using GEPRO_OSIsharp;
```
boven aan je bestand.

INHOUD
==============
MagisterRooster:

Vraag rooster van Gepro-Osi op:
```csharp
public static List<List<Lesuur>> GetRooster(uint SchoolID, string UserName, string Afdeling)
```
Filter de opgegeven rooster en haal alleen de opgegeven dag eruit:
```csharp
public static List<Lesuur> GetByDay(List<List<Lesuur>> Rooster, DayOfWeek Dag)
```
Filter de opgegeven rooster en haal alleen de opgegeven dagen eruit:
```csharp
public static List<List<Lesuur>> GetByDays(List<List<Lesuur>> Rooster, List<DayOfWeek> Dagen)
```
Krijg dag als DayOfWeek van dagNummer:
```csharp
public static DayOfWeek getDay(int dayNumber)
```
Vraag de afdelingen op:
```csharp
public static List<string> GetAfdelingen(uint SchoolID)
```
  
  
LICENSE
==============
GPLv2 (zie LICENSE bestand)

VOORBEELD
==============
```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using GEPRI_OSIsharp;

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
        foreach (var day in rooster)
        {
            foreach (var lesuur in day)
            {
                if (!lesuur.IsEmpty()) Console.WriteLine("Dag: " + lesuur.Dag + ", Uur: " + lesuur.Uur + ":   " + lesuur.Vak.Naam);
            }
        }

        var roosterVandaag = MagisterRooster.GetByDay(rooster, DateTime.Today.DayOfWeek);
        Console.WriteLine("Vandaag:");
        foreach (var day in rooster)
        {
            foreach (var lesuur in day)
            {
                if (!lesuur.IsEmpty()) Console.WriteLine("Uur: " + lesuur.Uur + ":   " + lesuur.Vak.Naam);
            }
        }

        var linqTest = new List<List<Lesuur>>();
        foreach (var dag in rooster)
            linqTest.Add(dag.Where(x => (!x.IsEmpty()) && x.isGewijzigd == true).ToList()); //LINQ enabled :)

        Console.ReadLine();
    }
}
```
