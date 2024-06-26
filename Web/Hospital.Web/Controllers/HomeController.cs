﻿namespace Hospital.Web.Controllers
{
    using System.Diagnostics;

    using Hospital.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return this.View();
        }
        public IActionResult About()
        {
            return this.View();
        }
        public IActionResult Treatment()
        {
            return this.View();
        }
        public IActionResult Doctors()
        {
            return this.View();
        }
        public IActionResult ContactUs()
        {
            return this.View();
        }
        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
