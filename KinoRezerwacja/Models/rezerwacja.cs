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
    
    public partial class rezerwacja
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rezerwacja()
        {
            this.miejsce = new HashSet<miejsce>();
        }
    
        public int id { get; set; }
        public int klient { get; set; }
        public bool kupione { get; set; }
    
        public virtual klient klient1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<miejsce> miejsce { get; set; }
    }
}