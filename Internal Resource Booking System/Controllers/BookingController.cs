using Internal_Resource_Booking_System.Controllers;
using Internal_Resource_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;

namespace Internal_Resource_Booking_System.Controllers
{


    public class BookingController : Controller
    {
        private readonly ApplicationDbContext  _dbContext;



        public BookingController( ApplicationDbContext applicationDbContext)
        {

           _dbContext = applicationDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult AddBookings(int? resourceId)
        {


            var resources = _dbContext.Resources.ToList();
            if (!resources.Any())
            {
                TempData["ErrorMessage"] = "No resources available. Please add a resource first.";
                return RedirectToAction("Index", "Resource");
            }



            if (resourceId.HasValue && !resources.Any(r => r.Id == resourceId))
            {
                TempData["ErrorMessage"] = "The selected resource does not exist.";
                resourceId = null;
            }

            ViewBag.Resources = new SelectList(resources, "Id", "Name", resourceId);
            var booking = new Booking { ResourceId = resourceId ?? 0 };
            return View(booking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddBookings(Booking booking)
        {
            var resources = _dbContext.Resources.ToList();
            if (!resources.Any())
            {
                TempData["ErrorMessage"] = "No resources available. Please add a resource first.";
                return RedirectToAction("Index", "Resource");
            }




            if (booking.ResourceId == 0 || !resources.Any(r => r.Id == booking.ResourceId))
            {
                ModelState.AddModelError("ResourceId", "Please select a valid resource.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Resources = new SelectList(resources, "Id", "Name", booking.ResourceId);
                return View(booking);
            }

            _dbContext.Bookings.Add(booking);
            _dbContext.SaveChanges();
            TempData["SuccessMessage"] = "Booking added successfully.";
            return RedirectToAction("Index");
        }

    }
}

