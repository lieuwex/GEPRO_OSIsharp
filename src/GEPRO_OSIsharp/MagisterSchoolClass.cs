//GEPRO-OSIsharp
//(c) 2014 Lieuwe Rooijakkers
//WARNING: DON'T ENTER. CODE IS UNREADABLE
using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace GEPRI_OSIsharp
{
    public partial class Lesuur
    {
        //Like 3th lesson hour
        public Vak Vak { get; internal set; }
        public string Klaslokaal { get; internal set; }
        public Klas Klas { get;  internal set; }
        public bool isGewijzigd { get; internal set; }
        public Lesuur Orgineel { get; internal set; }

        public DayOfWeek Dag { get; internal set; }
        public int Uur { get; internal set; }

        internal static Dictionary<string, string> checkedClassTypes = new Dictionary<string, string>();

        internal Lesuur() { }

        internal Lesuur(uint schoolID, string teacherCode, string classRoom, string className, string subGroupID, int hour, int day)
        {
            this.Klaslokaal = classRoom;
            this.Vak = new Vak()
            {
                DocentCode = teacherCode,
                Naam = className
            };
            this.Dag = getDay(day);

            this.Uur = hour + 1;

            if (!checkedClassTypes.Any(x => x.Key == className))
            {

                string URL = "http://publish.gepro-osi.nl/roosters/rooster.php?docenten=" + teacherCode + "&type=Docentrooster&wijzigingen=1&school=" + schoolID;

                var doc = new HtmlWeb().Load(URL);

                var upperNode = doc.DocumentNode.SelectSingleNode("/html[1]/body[1]/div[1]/table[1]/tr[2]/td[1]/table[4]/tr[1]/td[3]");
                if (upperNode.ChildNodes.Count == 1) throw new HtmlWebException("Error in request. Is the URL legal?");
                //weird errors, so i had to make the code waaaay less readable. fuck.
                var teacherLessonsRaw = new List<IEnumerable<IEnumerable<HtmlNode>>>();
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[6]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[7]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[8]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[9]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[10]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[11]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[12]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[13]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }
                try { teacherLessonsRaw.Add(upperNode.SelectSingleNode("table[1]/tr[14]").ChildNodes.Where(x => x.Name == "td" && x.ChildNodes.Count == 2).Select(x => x.ChildNodes.Where(y => y.Name == "table").ToList()[0].ChildNodes.Where(z => z.Name == "tr").ToList()[0].ChildNodes.Where(a => a.Name == "td" && a.InnerHtml != "&nbsp"))); }
                catch { teacherLessonsRaw.Add(null); }

                List<HtmlNode> teacherLessonRaw;
                try { teacherLessonRaw = teacherLessonsRaw[hour].ToList()[day].ToList(); }
                catch { return; }
                this.Klas = new Klas()
                {
                    Naam = teacherLessonRaw[0].InnerHtml,
                    ClusterID = subGroupID,
                    MentorCode = null
                };
                checkedClassTypes.Add(this.Vak.Naam, teacherLessonRaw[0].InnerHtml);
            }
            else
            {
                this.Klas = new Klas()
                {
                    Naam = checkedClassTypes[this.Vak.Naam],
                    ClusterID = subGroupID,
                    MentorCode = null
                };
            }
        }

        internal Lesuur(uint schoolID, string teacherCode, string classRoom, string className, string subGroupID, int hour, int day, string originalTeacherCode, string originalClassRoom, string originalClassName, string originalSubGroupID)
            : this(schoolID, teacherCode, classRoom, className, subGroupID, hour, day)
        {
            this.isGewijzigd = true;
            this.Orgineel = new Lesuur();
            this.Orgineel.Klaslokaal = originalClassRoom;
            this.Orgineel.Dag = getDay(day);
            this.Orgineel.Uur = Uur;
            this.Orgineel.Klas = new Klas()
            {
                Naam = this.Klas.Naam,
                ClusterID = originalSubGroupID,
                MentorCode = null
            };
            this.Orgineel.Vak = new Vak()
            {
                DocentCode = originalTeacherCode,
                Naam = originalClassName
            };
        }

        public static DayOfWeek getDay(int dayNumber)
        {
            switch(dayNumber)
            {
                case 0: return DayOfWeek.Monday;
                case 1: return DayOfWeek.Tuesday;
                case 2: return DayOfWeek.Wednesday;
                case 3: return DayOfWeek.Thursday;
                case 4: return DayOfWeek.Friday;
                default: return DayOfWeek.Monday;
            }
        }

        public bool IsEmpty()
        {
            if (this.Vak != null || this.Klas != null || this.Orgineel != null)
                return true;
            else
                return false;
        }
    }

    public partial class Vak
    {
        //Like english class
        public string DocentCode { get; internal set; }
        public string Naam { get; internal set; }
    }

    public partial class Klas
    {
        public string Naam { get; internal set; }
        public string ClusterID { get; internal set; }
        public string MentorCode { get; internal set; }
    }
}
