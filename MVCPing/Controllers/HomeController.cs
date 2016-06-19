using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;
using WcfPing;

namespace MVCPing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Ping()
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress myEndpoint = new EndpointAddress("http://localhost:51620/PingService.svc");
            ChannelFactory<IPingService> myChannelFactory = new ChannelFactory<IPingService>(myBinding, myEndpoint);

            IPingService instance = myChannelFactory.CreateChannel();
            // Call Service.
            object s = instance.Ping();
            myChannelFactory.Close();

            return View(s);
        }

        public ActionResult Ping2()
        {
            Assembly ass = Assembly.GetAssembly(typeof(IPingService));
            object s = CallInterfaceFromAssembly(ass);


            return View("Ping", s);
        }

        private string CallInterfaceFromAssembly(Assembly ass)
        {
            if (ass == null ) return String.Empty;

            var names = (from type in ass.GetTypes()
                         from method in type.GetMethods(
                           BindingFlags.Public | BindingFlags.NonPublic |
                           BindingFlags.Instance | BindingFlags.Static)
                         select method).Where(x => x.Name == "Ping").ToList();

            string r = string.Empty;
            foreach (MethodInfo name in names)
            {
                r += name.Name;
            }


            return r;
        }

    }
}