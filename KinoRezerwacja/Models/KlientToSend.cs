using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinoRezerwacja.Models
{
    public class KlientToSend
    {
        public int id { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Email { get; set; }
        public string Haslo { get; set; }
    }
}