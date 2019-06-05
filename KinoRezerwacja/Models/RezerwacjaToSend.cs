using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinoRezerwacja.Models
{
    public class RezerwacjaToSend
    {
        public int Id { get; set; }
        public Boolean Kupione { get; set; }

        public int SeansId { get; set; }
        public System.DateTime Data { get; set; }
        public bool D3 { get; set; }

        public int FilmId { get; set; }
        public string Nazwa { get; set; }
        public decimal Rok { get; set; }

        public List<Models.MiejsceToSend> Miejsca { get; set; }
    }
}