using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Cors;

namespace KinoRezerwacja.Controllers
{
    [System.Web.Mvc.RequireHttps]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [Authorize]
    public class RezerwacjeController : ApiController
    {
        Models.kinoEntities db = new Models.kinoEntities();

        [HttpGet]
        public IHttpActionResult List()
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                List<Models.RezerwacjaToSend> list = new List<Models.RezerwacjaToSend>();

                foreach(Models.rezerwacja rezerwacja in db.rezerwacja.Where(r => r.klient == user))
                {
                    Models.RezerwacjaToSend trezerwacja = new Models.RezerwacjaToSend();
                    trezerwacja.Miejsca = new List<Models.MiejsceToSend>();

                    foreach(Models.miejsce miejsce in rezerwacja.miejsce)
                    {
                        Models.MiejsceToSend tmiejsce = new Models.MiejsceToSend();
                        tmiejsce.Nr = miejsce.nr;
                        tmiejsce.Rzad = miejsce.rzad;
                        tmiejsce.Wolne = false;

                        trezerwacja.Miejsca.Add(tmiejsce);
                    }

                    if (trezerwacja.Miejsca.Count() != 0)
                    {
                        Models.seans seans = rezerwacja.miejsce.First().seans1;
                        Models.film film = seans.film1;

                        trezerwacja.Id = rezerwacja.id;
                        trezerwacja.Kupione = rezerwacja.kupione;

                        trezerwacja.SeansId = seans.id;
                        trezerwacja.Data = seans.data;
                        trezerwacja.D3 = seans.d3;
                        trezerwacja.FilmId = film.id;
                        trezerwacja.Nazwa = film.nazwa;
                        trezerwacja.Rok = film.rok;

                        list.Add(trezerwacja);
                    }
                }
                return Ok(list);
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

        [HttpGet]
        public IHttpActionResult ListActual()
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                List<Models.RezerwacjaToSend> list = new List<Models.RezerwacjaToSend>();

                foreach (Models.rezerwacja rezerwacja in db.rezerwacja.Where(r => r.klient == user))
                {
                    Models.RezerwacjaToSend trezerwacja = new Models.RezerwacjaToSend();
                    trezerwacja.Miejsca = new List<Models.MiejsceToSend>();

                    var miejsca = rezerwacja.miejsce;
                    if (miejsca.Count() != 0)
                        if(miejsca.First().seans1.data > System.DateTime.Now)
                        {
                            foreach (Models.miejsce miejsce in miejsca)
                            {
                                Models.MiejsceToSend tmiejsce = new Models.MiejsceToSend();
                                tmiejsce.Nr = miejsce.nr;
                                tmiejsce.Rzad = miejsce.rzad;
                                tmiejsce.Wolne = false;

                                trezerwacja.Miejsca.Add(tmiejsce);
                            }
                            Models.seans seans = rezerwacja.miejsce.First().seans1;
                            Models.film film = seans.film1;

                            trezerwacja.Id = rezerwacja.id;
                            trezerwacja.Kupione = rezerwacja.kupione;

                            trezerwacja.SeansId = seans.id;
                            trezerwacja.Data = seans.data;
                            trezerwacja.D3 = seans.d3;
                            trezerwacja.FilmId = film.id;
                            trezerwacja.Nazwa = film.nazwa;
                            trezerwacja.Rok = film.rok;

                            list.Add(trezerwacja);
                        }
                }
                return Ok(list);
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

        [HttpGet]
        public IHttpActionResult Rezerwacja( int id)
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                Models.rezerwacja rezerwacja = db.rezerwacja.FirstOrDefault(r => r.id == id);
                if (rezerwacja == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje rezerwacja nr " + id);
                if (rezerwacja.klient != user)
                    return Content(HttpStatusCode.BadRequest, "Nie jesteś posiadaczem rezerwacji nr " + id);

                Models.RezerwacjaToSend trezerwacja = new Models.RezerwacjaToSend();
                
                trezerwacja.Miejsca = new List<Models.MiejsceToSend>();

                var miejsca = rezerwacja.miejsce;
                foreach (Models.miejsce miejsce in miejsca)
                {
                    Models.MiejsceToSend tmiejsce = new Models.MiejsceToSend();
                    tmiejsce.Nr = miejsce.nr;
                    tmiejsce.Rzad = miejsce.rzad;
                    tmiejsce.Wolne = false;

                    trezerwacja.Miejsca.Add(tmiejsce);
                }
                Models.seans seans = rezerwacja.miejsce.First().seans1;
                Models.film film = seans.film1;

                trezerwacja.Id = rezerwacja.id;
                trezerwacja.Kupione = rezerwacja.kupione;

                trezerwacja.SeansId = seans.id;
                trezerwacja.Data = seans.data;
                trezerwacja.D3 = seans.d3;
                trezerwacja.FilmId = film.id;
                trezerwacja.Nazwa = film.nazwa;
                trezerwacja.Rok = film.rok;

                return Ok(trezerwacja);
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

        [HttpPut]
        public IHttpActionResult Add(Models.RezerwacjaToSend nRezerwacja)
        {
            try
            {
                if (nRezerwacja.Miejsca.Count() == 0)
                    return Content(HttpStatusCode.BadRequest, "Brak wybranych miejsc");

                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                Models.rezerwacja rezerwacja = new Models.rezerwacja();

                rezerwacja.klient = user;
                rezerwacja.kupione = false;

                Models.seans seans = db.seans.FirstOrDefault(s => s.id == nRezerwacja.SeansId);
                if (seans == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje seans nr " + nRezerwacja.SeansId);

                nRezerwacja.Miejsca.OrderBy(m => m.Rzad).ThenBy(m => m.Nr);
                foreach(Models.miejsce miejsce in seans.miejsce.OrderBy(m => m.rzad).ThenBy(m => m.nr))
                {
                    Models.MiejsceToSend tmiejsce = nRezerwacja.Miejsca.FirstOrDefault();

                    if (tmiejsce != null)
                    {
                        if(miejsce.rzad == tmiejsce.Rzad && miejsce.nr == tmiejsce.Nr)
                        {
                            if (miejsce.rezerwacja1 != null)
                                return Content(HttpStatusCode.BadRequest, "Miejsce "+miejsce.nr+"w rzedzie "+miejsce.rzad+" jest już zajęte.");

                            miejsce.rezerwacja1 = rezerwacja;
                            nRezerwacja.Miejsca.RemoveAt(0);
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                db.rezerwacja.Add(rezerwacja);
                db.SaveChanges();
                return Ok(rezerwacja.id);
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

        [HttpPost]
        public IHttpActionResult Change(Models.RezerwacjaToSend nRezerwacja)
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                Models.rezerwacja rezerwacja = db.rezerwacja.FirstOrDefault(r => r.id == nRezerwacja.Id);
                if (rezerwacja == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje rezerwacja nr " + nRezerwacja.Id);
                if (rezerwacja.klient != user)
                    return Content(HttpStatusCode.BadRequest, "Nie jesteś posiadaczem rezerwacji nr " + nRezerwacja.Id);
                if (rezerwacja.kupione)
                    return Content(HttpStatusCode.BadRequest, "Wykupiłeś już rezerwacje nr " + nRezerwacja.Id);


                Models.seans seans = db.seans.FirstOrDefault(s => s.id == nRezerwacja.SeansId);
                if (seans == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje seans nr " + nRezerwacja.SeansId);
                
                foreach(Models.miejsce miejsce in seans.miejsce)
                {
                    Boolean nieMa = true;
                    foreach (Models.MiejsceToSend tmiejsce in nRezerwacja.Miejsca)
                    {
                        if (miejsce.nr == tmiejsce.Nr && miejsce.rzad == tmiejsce.Rzad)
                        {
                            miejsce.rezerwacja1 = rezerwacja;
                            nieMa = false;
                        }
                    }
                    if (nieMa && miejsce.rezerwacja1 == rezerwacja)
                        rezerwacja.miejsce.Remove(miejsce);
                }

                db.SaveChanges();
                return Ok(rezerwacja.id);
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

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                User = System.Web.HttpContext.Current.User;
                int user;
                int.TryParse(((ClaimsIdentity)User.Identity).Claims.First(c => c.Type == "Id").Value, out user);

                Models.rezerwacja rezerwacja = db.rezerwacja.FirstOrDefault(r => r.id == id);
                if (rezerwacja == null)
                    return Content(HttpStatusCode.BadRequest, "Nie istnieje rezerwacja nr " + id);
                if (rezerwacja.klient != user)
                    return Content(HttpStatusCode.BadRequest, "Nie jesteś posiadaczem rezerwacji nr " + id);
                if (rezerwacja.kupione)
                    return Content(HttpStatusCode.BadRequest, "Wykupiłeś już rezerwacje nr " + id);


                rezerwacja.miejsce.Clear();

                db.rezerwacja.Remove(rezerwacja);
                db.SaveChanges();
                return Ok(rezerwacja.id);
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
