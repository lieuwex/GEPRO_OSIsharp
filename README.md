GEPRO_OSIsharp
==============

C# Library voor Gepro-OSI roosters.

GEBRUIK
==============
Voeg gewoon reference toe en zet dan

using GEPRO_OSIsharp;
```csharp
boven aan je bestand.
```
INHOUD
==============
MagisterRooster:

  /// Vraagt rooster van Gepro-Osi op.
```csharp
public static List<List<Lesuur>> GetRooster(uint SchoolID, string UserName, string Afdeling)
```
  /// Filterd de opgegeven rooster en haalt alleen de opgegeven dag eruit
```csharp
public static List<Lesuur> GetByDay(List<List<Lesuur>> Rooster, DayOfWeek Dag)
```
  
  
  /// Filterd de opgegeven rooster en haalt alleen de opgegeven dagen eruit
```csharp
public static List<List<Lesuur>> GetByDays(List<List<Lesuur>> Rooster, List<DayOfWeek> Dagen)
```
  
  
  /// Geeft dag als DayOfWeek van dagNummer.
```csharp
public static DayOfWeek getDay(int dayNumber)
```
  
  
  /// Vraagt de afdelingen op.
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
        var rooster = MagisterRooster.GetRooster(181, "120678", "4V");
        foreach (var day in rooster)
        {
            foreach (var lesuur in day)
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
        foreach (var dag in rooster)
            linqTest.Add(dag.Where(x => x != null && x.isGewijzigd == true).ToList()); //LINQ enabled :)

        Console.ReadLine();
    }
}
```
