using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KinoRezerwacja.Models
{
    public class SeansBasicToSend
    {
        public int Id { get; set; }
        public System.DateTime Data { get; set; }
        public bool D3 { get; set; }
    }
}