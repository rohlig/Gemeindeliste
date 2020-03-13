using SQLite;
using System;

namespace Gemeindeliste.Models
{
    public enum Status
    {
        Gemeindeglied,
        Besucher,
        Interessierter,
        Kind
    }
    public class Item : ICloneable
    {
        [PrimaryKey]
        public int Id { get; set; } = -1;
        public string Vorname  {get; set; } = "";
        public string Nachname { get; set; } = "";
        public string Taufdatum { get; set; } = "";
        public string Geburtsdatum { get; set; } = "";
        public string Status { get { return realStatus.ToString(); } set { realStatus = (Status)Enum.Parse(typeof(Status), value); ; } }
        [Ignore]
        public string Name
        {
            get
            {
                return Vorname + " " + Nachname;
            }
        }
        [Ignore]
        public string Alter { get { return Geburtsdatum.Equals("")?"":""+(DateTime.Now.Year - Geb.Year + ((DateTime.Now.Month > Geb.Month) ? 1 : 0)); } set { } }
        [Ignore]
        public string JubNr { get { return Taufdatum.Equals("") ? "" : "" + (DateTime.Now.Year - Tauf.Year + ((DateTime.Now.Month > Tauf.Month)?1:0)); } set { } }
        [Ignore]
        public DateTime Geb { get { return DateTime.ParseExact(Geburtsdatum, "dd.MM.yyyy", null); } }
        [Ignore]
        public DateTime Tauf { get { return DateTime.ParseExact(Taufdatum, "dd.MM.yyyy", null); ; } }
        [Ignore]
        public Status realStatus { get; set; }

        public object Clone()
        {
                return this.MemberwiseClone();
        }
    }
}