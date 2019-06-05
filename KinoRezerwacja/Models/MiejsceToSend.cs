using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinoRezerwacja.Models
{
    public class MiejsceToSend
    {
        public int Rzad { get; set; }
        public int Nr { get; set; }
        public Boolean Wolne { get; set; }
    }
}