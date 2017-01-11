using System;
using System.Web.Mvc;

namespace WindowsAuthenticationSample.Controllers
{
    public class AuthorizationController : Controller
    {
        public const string AspNet_SessionId = "ASP.NET_SessionId";

        // GET: Authorization
        public ActionResult Index()
        {
            return View();
        }

        // GET: Authorization/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Authorization/Create
        public ActionResult LogOut()
        {
            return View();
        }

        // POST: Authorization/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Authorization/Edit/5
        public ActionResult SessionTimeOut(int id)
        {
            //-> Reset ASP.NET Session Cookie.
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId"));

            //-> Or

            //-> Trigger browser to delete ASP.NET Session Cookie.
            System.Web.HttpCookie sessionCookie = new System.Web.HttpCookie(AspNet_SessionId);
            sessionCookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(sessionCookie);

            return View();
        }

        // POST: Authorization/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Authorization/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Authorization/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
