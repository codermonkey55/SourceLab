using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GraphQL_WebAPI_SampleNET45.Controllers
{
    public class GraphiQLController : Controller
    {
        // GET: GraphiQL
        public ActionResult Index()
        {
            return View();
        }
    }
}