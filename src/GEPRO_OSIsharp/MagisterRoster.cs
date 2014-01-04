//GEPRO-OSIsharp
//(c) 2014 Lieuwe Rooijakkers
//WARNING: DON'T ENTER. CODE IS UNREADABLE
using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace GEPRI_OSIsharp
{
    public partial class MagisterRooster
    {
        /// <summary>
        /// Vraagt rooster van Gepro-Osi op.
        /// </summary>
        /// <param name="SchoolID">De school om rooster van op te vragen.</param>
        /// <param name="UserName">De gebruikers-naam op het rooster van op te vragen.</param>
        /// <param name="Department"><para>De afdelingen van de gebruiker om rooster van op te vragen</para>
        /// <para>Tip: Gebruik MagisterRoster.GetAfdelingen(uint) om de afdelingen op te halen.</para></param>
        /// <returns>List van dagen in een List van uren wat de lesuren bevat. </returns>
        public static List<List<Lesuur>> GetRooster(uint SchoolID, string UserName, string Afdeling)
        {
            #region GetLessons
            string URL = "http://publish.gepro-osi.nl/roosters/rooster.php?leerling=" + UserName + "&type=Leerlingrooster&afdeling=" + Afdeling + "&wijzigingen=1&school=" + SchoolID;

            HtmlDocument doc = new HtmlWeb().Load(URL);

            var upperNode = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/table[1]/tr[2]/td[1]/table[4]/tr[1]/td[3]");
            if (upperNode.ChildNodes.Count == 1) throw new HtmlWebException("Error in request. Is the URL legal?");

            var lessonsRaw = StripTable(upperNode);
            #endregion

            #region getOriginalLessons
            URL = "http://publish.gepro-osi.nl/roosters/rooster.php?leerling=" + UserName + "&type=Leerlingrooster&afdeling=" + Afdeling + "&wijzigingen=0&school=" + SchoolID;

            doc = new HtmlWeb().Load(URL);

            upperNode = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/table[1]/tr[2]/td[1]/table[4]/tr[1]/td[3]");
            if (upperNode.ChildNodes.Count == 1) throw new HtmlWebException("Error in request. Is the URL legal?");

            var originalLessonsRaw = StripTable(upperNode);
            #endregion

            var finalList = new List<List<Lesuur>>();
            for(int day = 0; day < 5; day++)
            { //Every day

                var tmpDay = new List<Lesuur>();

                for (int schoolhour = 0; schoolhour < 9; schoolhour++)
                { //Every schoolhour

                    var lessonRaw = lessonsRaw.ToList()[schoolhour].ToList()[day].ToList();
                    var originalLessonRaw = originalLessonsRaw.ToList()[schoolhour].ToList()[day].ToList();

                    bool isChanged = lessonRaw[0].OuterHtml == "<td align=\"left\" width=\"31\" class=\"tableCellNew\">" + lessonRaw[0].InnerHtml + "</td>";

                    if (!isChanged)
                    {
                        if (!lessonRaw.Any(n => n.InnerHtml == "" && lessonRaw.IndexOf(n) != 3)) //Controleerd of de docent en het klaslokaal niet leeg is
                            tmpDay.Add(new Lesuur(SchoolID, lessonRaw[0].InnerHtml, lessonRaw[1].InnerHtml, lessonRaw[2].InnerHtml, lessonRaw[3].InnerHtml, schoolhour, day));
                        else
                            tmpDay.Add(new Lesuur()); //should  give slight speed bonus :D
                    }

                    else
                        tmpDay.Add(new Lesuur(SchoolID, lessonRaw[0].InnerHtml, lessonRaw[1].InnerHtml, lessonRaw[2].InnerHtml, lessonRaw[3].InnerHtml, schoolhour, day, originalLessonRaw[0].InnerHtml, originalLessonRaw[1].InnerHtml, originalLessonRaw[2].InnerHtml, originalLessonRaw[3].InnerHtml));
                }

                finalList.Add(tmpDay);
            }

            return finalList;
        }

        /// <summary>
        /// Filterd de opgegeven rooster en haalt alleen de opgegeven dag eruit
        /// </summary>
        /// <param name="Rooster">De rooster om uit te filteren.</param>
        /// <param name="Dag">De dag om op te vragen.</param>
        /// <returns>Lijst met lesuren van de gevraagde dag</returns>
        public static List<Lesuur> GetByDay(List<List<Lesuur>> Rooster, DayOfWeek Dag)
        {
            switch(Dag)
            {
                case DayOfWeek.Monday: return Rooster[0];
                case DayOfWeek.Tuesday: return Rooster[1];
                case DayOfWeek.Wednesday: return Rooster[2];
                case DayOfWeek.Thursday: return Rooster[3];
                case DayOfWeek.Friday: return Rooster[4];
                default: return Rooster[0];
            }
        }

        /// <summary>
        /// Filterd de opgegeven rooster en haalt alleen de opgegeven dagen eruit
        /// </summary>
        /// <param name="Rooster">De rooster om uit te filteren.</param>
        /// <param name="Dagen">De dagen om op te vragen.</param>
        /// <returns>Lijst met lesuren van de gevraagde dagen</returns>
        public static List<List<Lesuur>> GetByDays(List<List<Lesuur>> Rooster, List<DayOfWeek> Dagen)
        {
            var finalList = new List<List<Lesuur>>();
            foreach (var dag in Dagen)
            {
                switch (dag)
                {
                    case DayOfWeek.Monday: finalList.Add(Rooster[0]); break;
                    case DayOfWeek.Tuesday: finalList.Add(Rooster[1]); break;
                    case DayOfWeek.Wednesday: finalList.Add(Rooster[2]); break;
                    case DayOfWeek.Thursday: finalList.Add(Rooster[3]); break;
                    case DayOfWeek.Friday: finalList.Add(Rooster[4]); break;
                    default: finalList.Add(Rooster[5]); break;
                }
            }
            return finalList;
        }

        internal static List<IEnumerable<IEnumerable<HtmlNode>>> StripTable(HtmlNode upperNode)
        {
            return new List<IEnumerable<IEnumerable<HtmlNode>>>()
            {
                // UNREADBLE PART INBOUND
                upperNode.SelectSingleNode("table[1]/tr[6]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp")),
                upperNode.SelectSingleNode("table[1]/tr[7]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp")),
                upperNode.SelectSingleNode("table[1]/tr[8]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp")),
                upperNode.SelectSingleNode("table[1]/tr[9]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp")),
                upperNode.SelectSingleNode("table[1]/tr[10]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp")),
                upperNode.SelectSingleNode("table[1]/tr[11]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp")),
                upperNode.SelectSingleNode("table[1]/tr[12]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp")),
                upperNode.SelectSingleNode("table[1]/tr[13]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp")),
                upperNode.SelectSingleNode("table[1]/tr[14]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))
            };
        }

        /// <summary>
        /// Geeft dag als DayOfWeek van dagNummer.
        /// </summary>
        /// <param name="dayNumber">De nummer van de dag om de dag van op te halen</param>
        /// <returns>Dag als DayOfWeek</returns>
        public static DayOfWeek getDay(int dayNumber)
        {
            return Lesuur.getDay(dayNumber);
        }

        /// <summary>
        /// Vraagt de afdelingen op.
        /// </summary>
        /// <param name="SchoolID">De ID van de school om de afdelingen van op te vragen.</param>
        /// <returns>List dat strings bevat met de afdelingen.</returns>
        public static List<string> GetAfdelingen(uint SchoolID)
        {
            string URL = "http://publish.gepro-osi.nl/roosters/rooster.php?type=Leerlingrooster&wijzigingen=1&school=" + SchoolID;

            HtmlDocument doc = new HtmlWeb().Load(URL);

            return doc.DocumentNode.SelectSingleNode("//td[@class='tableHeader'][2]/select").ChildNodes.Where(x => x.Name == "#text" && x.InnerHtml != "\n").Select(x => x.InnerHtml).ToList();
        }
    }
}
