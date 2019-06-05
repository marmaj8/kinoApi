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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class FilmyController : ApiController
    {
        Models.kinoEntities db = new Models.kinoEntities();

        [HttpGet]
        public IHttpActionResult List()
        {
            try
            {
                List<Models.FilmToSend> list = new List<Models.FilmToSend>();

                foreach (Models.film film in db.film.Where(f => f.seans.FirstOrDefault(s => s.data > System.DateTime.Now) != null))
                {
                    Models.FilmToSend tfilm = new Models.FilmToSend();

                    tfilm.Id = film.id;
                    tfilm.Nazwa = film.nazwa;
                    tfilm.Opis = film.opis;
                    tfilm.Rezyser = film.rezyser;
                    tfilm.Rok = film.rok;
                    tfilm.Dlugosc = film.dlugosc;

                    tfilm.Seanse = new List<Models.SeansBasicToSend>();

                    foreach (Models.seans seans in film.seans.Where(s => s.data > System.DateTime.Now))
                    {
                        Models.SeansBasicToSend tseans = new Models.SeansBasicToSend();
                        tseans.Id = seans.id;
                        tseans.Data = seans.data;
                        tseans.D3 = seans.d3;

                        tfilm.Seanse.Add(tseans);
                    }

                    list.Add(tfilm);
                }
                return Ok(list);
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

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            Models.FilmToSend tfilm = new Models.FilmToSend();
            try
            {
                Models.film film = db.film.FirstOrDefault(f => f.id == id);
                if (film == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje film o nr " + id);

                tfilm.Id = film.id;
                tfilm.Nazwa = film.nazwa;
                tfilm.Opis = film.opis;
                tfilm.Rezyser = film.rezyser;
                tfilm.Rok = film.rok;
                tfilm.Dlugosc = film.dlugosc;

                tfilm.Seanse = new List<Models.SeansBasicToSend>();
                
                foreach (Models.seans seans in film.seans.Where(s => s.data > System.DateTime.Now))
                {
                    Models.SeansBasicToSend tseans = new Models.SeansBasicToSend();
                    tseans.Id = seans.id;
                    tseans.Data = seans.data;
                    tseans.D3 = seans.d3;

                    tfilm.Seanse.Add(tseans);
                }

                if (tfilm.Seanse.Count() == 0)
                    return Content(HttpStatusCode.BadRequest, "Kino nie emituje filmu " + film.nazwa);
                return Ok(tfilm);
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
