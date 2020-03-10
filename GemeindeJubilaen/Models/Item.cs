using SQLite;
using System;

namespace GemeindeJubiläen.Models
{
    public class Item : ICloneable
    {
        [PrimaryKey]
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
        public string Geburtsdatum { get; set; } = "";
        public string Alter { get { return Geburtsdatum.Equals("")?"":""+(DateTime.Now.Year - Geb.Year + ((DateTime.Now.Month > Geb.Month) ? 1 : 0)); } set { } }
        public string JubNr { get { return Taufdatum.Equals("") ? "" : "" + (DateTime.Now.Year - Tauf.Year + ((DateTime.Now.Month > Tauf.Month)?1:0)); } set { } }

        public DateTime Geb { get { return DateTime.ParseExact(Geburtsdatum, "dd.MM.yyyy", null); } }
        public DateTime Tauf { get { return DateTime.ParseExact(Taufdatum, "dd.MM.yyyy", null); ; } }
        public string Taufdatum { get; set; } = "";
        public bool Gemeindeglied { get; set; } = false;
        public String Status { get { return (Gemeindeglied) ? "Gemeindeglied" : "Besucher"; } set { Gemeindeglied = value.Equals(Gemeindeglied); } }
        public object Clone()
        {
                return this.MemberwiseClone();
        }
    }
}