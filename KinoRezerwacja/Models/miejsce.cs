//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KinoRezerwacja.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class miejsce
    {
        public int id { get; set; }
        public int rzad { get; set; }
        public int nr { get; set; }
        public int seans { get; set; }
        public Nullable<int> rezerwacja { get; set; }
    
        public virtual rezerwacja rezerwacja1 { get; set; }
        public virtual seans seans1 { get; set; }
    }
}