using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinoRezerwacja.Models
{
    public class FilmToSend
    {
        public int Id { get; set; }
        public string Nazwa { get; set; }
        public decimal Rok { get; set; }
        public string Rezyser { get; set; }
        public string Opis { get; set; }
        public Nullable<decimal> Dlugosc { get; set; }

        public List<SeansBasicToSend> Seanse { get; set; }
    }
}