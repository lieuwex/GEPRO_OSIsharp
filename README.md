GEPRI_OSIsharp
==============

C# Library voor Gepri-OSI roosters.

GEBRUIK
==============
Voeg gewoon reference toe en zet dan

    using GEPRI_OSIsharp;

boven aan je bestand.

INHOUD
==============
MagisterRooster:

  /// Vraagt rooster van Gepri-Osi op.
  
  public static List<List<Lesuur>> GetRooster(uint SchoolID, string UserName, string Afdeling)
  
  
  
  /// Filterd de opgegeven rooster en haalt alleen de opgegeven dag eruit
  
  public static List<Lesuur> GetByDay(List<List<Lesuur>> Rooster, DayOfWeek Dag)
  
  
  
  /// Filterd de opgegeven rooster en haalt alleen de opgegeven dagen eruit
  
  public static List<List<Lesuur>> GetByDays(List<List<Lesuur>> Rooster, List<DayOfWeek> Dagen)
  
  
  
  /// Geeft dag als DayOfWeek van dagNummer.
  
  public static DayOfWeek getDay(int dayNumber)
  
  
  
  /// Vraagt de afdelingen op.
  
  public static List<string> GetAfdelingen(uint SchoolID)
  
  
  
LICENSE
==============
GPLv2 (zie LICENSE bestand)
