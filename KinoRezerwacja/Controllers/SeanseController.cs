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
    public class SeanseController : ApiController
    {
        Models.kinoEntities db = new Models.kinoEntities();

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Models.SeansToSend tseans = new Models.SeansToSend();
            try
            {

                Models.seans seans = db.seans.FirstOrDefault(s => s.id == id);
                if (seans == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje seans o nr " + id);
                if (seans.data <= System.DateTime.Now)
                    return Content(HttpStatusCode.BadRequest, "Seans o nr " + id + " już się odbył");

                tseans.D3 = seans.d3;
                tseans.Data = seans.data;
                tseans.Id = seans.id;

                tseans.FilmId = seans.film1.id;
                tseans.Nazwa = seans.film1.nazwa;
                tseans.Rok = seans.film1.rok;
                tseans.Rezyser = seans.film1.rezyser;
                tseans.Opis = seans.film1.opis;
                tseans.Dlugosc = seans.film1.dlugosc;

                tseans.Miejsca = new List<Models.MiejsceToSend>();

                foreach (Models.miejsce miejsce in seans.miejsce)
                {
                    Models.MiejsceToSend tmiejsce = new Models.MiejsceToSend();
                    tmiejsce.Nr = miejsce.nr;
                    tmiejsce.Rzad = miejsce.rzad;

                    if (miejsce.rezerwacja == null)
                        tmiejsce.Wolne = true;
                    else
                        tmiejsce.Wolne = false;

                    tseans.Miejsca.Add(tmiejsce);
                }
                if (tseans.Miejsca.Count() == 0)
                    return Content(HttpStatusCode.BadRequest, "Seans nr" + id + " jest jeszcze w przygotowaniu");

                return Ok(tseans);
            }
            catch (EntityException ex)
            {
                return Content(HttpStatusCode.InternalServerError, "Problem w połaczeniu z bazą danych");
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}
