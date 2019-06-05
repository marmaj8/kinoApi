using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KinoRezerwacja.Controllers
{
    [System.Web.Mvc.RequireHttps]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class KlienciController : ApiController
    {
        Models.kinoEntities db = new Models.kinoEntities();

        [HttpPut]
        public IHttpActionResult Register(Models.KlientToSend nKlient)
        {
            try
            {
                Models.klient klient = db.klient.FirstOrDefault(k => k.email == nKlient.Email);
                if (klient != null)
                    return Content(HttpStatusCode.BadRequest, "W bazie istnieje użytkownik o podanym Emailu");

                klient = new Models.klient();
                klient.imie = nKlient.Imie;
                klient.nazwisko = nKlient.Nazwisko;
                klient.email = nKlient.Email;
                klient.haslo = nKlient.Haslo;

                db.klient.Add(klient);
                db.SaveChanges();

                return Ok("Konto założone");
            }
            catch (EntityException ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Problem z bazą danych");
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
