using SQLite;
using System;

namespace Gemeindeliste.Models
{
    public enum Status
    {
        Gemeindeglied,
        Besucher,
        Kind
    }
    public class Item : ICloneable
    {
        [PrimaryKey]
        public int Id { get; set; } = -1;
       
        public string Name
        {
            get
            {
                return Vorname + " " + Nachname;
            }
            //set zur kompatibilität mit Altversion
            set
            {
                var split = value.Split(' ');
                if (value.Split(' ').Length == 2)
                {
                    Vorname = split[0];
                    Nachname = split[1];
                }
                else
                {
                    Vorname = value;
                }
            }
        }
        public string Vorname { get; set; } = "";
        public string Nachname { get; set; } = "";
        public string Geburtsdatum { get; set; } = "";
        public string Alter { get { return Geburtsdatum.Equals("")?"":""+(DateTime.Now.Year - Geb.Year + ((DateTime.Now.Month > Geb.Month) ? 1 : 0)); } set { } }
        public string JubNr { get { return Taufdatum.Equals("") ? "" : "" + (DateTime.Now.Year - Tauf.Year + ((DateTime.Now.Month > Tauf.Month)?1:0)); } set { } }
        public DateTime Geb { get { return DateTime.ParseExact(Geburtsdatum, "dd.MM.yyyy", null); } }
        public DateTime Tauf { get { return DateTime.ParseExact(Taufdatum, "dd.MM.yyyy", null); ; } }
        public string Taufdatum { get; set; } = "";
     //   public bool Gemeindeglied { get; set; } = false;
        [Ignore]
        public Status realStatus { get; set; }
        public String Status { get { return realStatus.ToString(); } set { realStatus = (Status)Enum.Parse(typeof(Status), value); ; } }
        public object Clone()
        {
                return this.MemberwiseClone();
        }
    }
}