using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KinoRezerwacja.Models
{
    [MetadataType(typeof(klientExtension))]
    partial class klient
    {
    }

    public class klientExtension
    {

        [Required]
        [Display(Name = "Imię użytkownika")]
        [StringLength(50, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 2)]
        [RegularExpression("[a-zA-Z]{2,}", ErrorMessage = "Tylko litery")]
        public string imie { get; set; }

        [Required]
        [Display(Name = "Nazwisko użytkownika")]
        [StringLength(50, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 2)]
        [RegularExpression("[a-zA-Z]{2,}", ErrorMessage = "Tylko litery")]
        public string nazwisko { get; set; }

        [Required]
        [Display(Name = "Adres e-mail")]
        [StringLength(100, ErrorMessage = "{0} nie może być dłuższy niż {1}")]
        [EmailAddress]
        public string email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        [StringLength(100, ErrorMessage = "{0} musi zawierać co najmniej następującą liczbę znaków: {2}.", MinimumLength = 5)]
        public string haslo { get; set; }

    }
}