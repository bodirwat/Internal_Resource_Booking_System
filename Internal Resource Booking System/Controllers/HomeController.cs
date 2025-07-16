using Internal_Resource_Booking_System.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Internal_Resource_Booking_System.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger , ApplicationDbContext applicationDbContext )
        {
            _logger = logger;
            _dbContext = applicationDbContext;
        }

        //==================================================
        // calculate total number of bookings,
        // resources,
        // and show recent bookings 
        //==================================================
        public IActionResult Index()
        {
            var today = DateTime.Today;

            var viewModel = new HomeViewModel
            {
                TotalBookings = _dbContext.Bookings.Count(),
                TotalResources = _dbContext.Resources.Count(),
                BookingsToday = _dbContext.Bookings.Count(b => b.StartTime.Date == today),
                RecentBookings = _dbContext.Bookings
                    .Include(b => b.Resource) 
                    .OrderByDescending(b => b.StartTime)
                    .Take(5)
                    .ToList()
            };

            return View(viewModel);
        }
        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult CreateResource()
        {
            return View();
        }

        //=====================================================================
        //Check if all the input fields 
        // Add the new resource to the database context
        // Save the changes to the database
        // Clear the form inputs after successful submission 
        ///Store a success message to show on the view
       //=====================================================================


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateResource([Bind("Name,Description,Location,Capacity,IsAvailable")] Resource resource)
        {
            //================================================================
            //============check if there is no duplicates resource=======
            //????????????????????????
            bool resourceExists = await _dbContext.Resources.AnyAsync(r =>
               r.Name.ToLower() == resource.Name.ToLower().Trim() &&
               r.Location.ToLower() == resource.Location.ToLower().Trim()

);

            if (resourceExists)
            {
                ModelState.AddModelError(string.Empty, "A resource with the same name and location already exists.");
                return View(resource);
            }   

            if (ModelState.IsValid)
            {
                _dbContext.Add(resource);
                await _dbContext.SaveChangesAsync();
                ModelState.Clear();
                TempData["SuccessMessage"] = "Resource added successfully!";
           
            }
            return View(resource);
        }

        public IActionResult ResourceList()
        {
            var resources =  _dbContext.Resources.ToList();
            return View(resources);
        }


        //===================================details==================================
        // Displays the details of a specific resource, including its upcoming bookings
        // Get the resource from the database, including its related bookings   
        // Prepare the view model to pass data to the view

        //===================================details==================================
        public async Task<IActionResult> Details(int? id)
        {
            var resource = _dbContext.Resources
         .Include(r => r.Bookings)
         .FirstOrDefault(r => r.Id == id);

            if (resource == null)
            {
                return NotFound();
            }

            var viewModel = new ResourceDetailsViewModel
            {
                Resource = resource,
                Bookings = resource.Bookings?.Where(b => b.StartTime >= DateTime.Now)
                                             .OrderBy(b => b.StartTime)
                                             .ToList() ?? new List<Booking>()
            };

            return View(viewModel);
        }


        //============================================================
        // Displays the edit form for a specific resource  
        // If no ID is provided, return 404 Not Found
        // Try to find the resource in the database  
        // If resource doesn't exist, return 404 Not Found
        //============================================================
        public async Task<IActionResult> EditResource(int? id)
        {
            if (id == null) return NotFound();

            var resource = await _dbContext.Resources.FindAsync(id);
            if (resource == null) return NotFound();

            return View(resource);
        }



        //=======================================edit resource======================================
        // Make sure the route ID matches the resource's ID
        // Update the resource in the database
        // Update the resource in the database

        // If the record was updated/deleted by someone else during this process
        // Check if the resource still exists
        //=======================================edit resource======================================


        [HttpPost]
        [ValidateAntiForgeryToken]   // prevents Cross-Site Request Forgery (CSRF) Attack
        public async Task<IActionResult> EditResource(int id, [Bind("Id,Name,Description,Location,Capacity,IsAvailable")] Resource resource)
        {
            if (id != resource.Id) return NotFound();
            //================================================================
            //============check if there is no duplicates resource=======
            //????????????????????????







            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(resource);
                    await _dbContext.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Resource updated successfully!";
            
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dbContext.Resources.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
            }
            return View(resource);
        }






        public async Task<IActionResult> EditBookings(int? id)
        {
            if (id == null) return NotFound();

            var booking = await _dbContext.Bookings.FindAsync(id);
            if (booking == null) return NotFound();

            ViewBag.Resources = new SelectList(_dbContext.Resources, "Id", "Name");

            return View(booking);
        }



        //=======================================edit resource======================================
        // Make sure the route ID matches the resource's ID
        // Update the resource in the database
        // If the record was updated/deleted by someone else during this process
        // Check if the resource still exists
        //=======================================edit resource======================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBookings(int id, [Bind("Id,ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {
            if (id != booking.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Bookings.Update(booking);
                    await _dbContext.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Booking updated successfully!";
        
                    return RedirectToAction("Details", new { id = booking.ResourceId });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_dbContext.Bookings.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
            }
            return View(booking);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var resource = await _dbContext.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            _dbContext.Resources.Remove(resource);
            await _dbContext.SaveChangesAsync();

            TempData["SuccessMessage"] = "Resource deleted successfully!";
            return RedirectToAction(nameof(ResourceList));
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteBooking(int id)
        {
            var booking = _dbContext.Bookings.FirstOrDefault(b => b.Id == id);
            if (booking == null)
            {
                return NotFound();
            }

            _dbContext.Bookings.Remove(booking);
            _dbContext.SaveChanges();

            TempData["SuccessMessage"] = "Booking deleted successfully.";
          
            return RedirectToAction("Details", new { id = booking.ResourceId });
        }

            //======================================add booking===================================================================

        public IActionResult AddBookings(int? resourceId)
        {


            var resource =  _dbContext.Resources.Find(resourceId);


            if (resource == null || !resource.IsAvailable)
            {
               // TempData["ErrorMessage"] = "The selected resource is not available.";
                return RedirectToAction("ResourceList", "Home");
            }




            var resources = _dbContext.Resources.ToList();
            if (!resources.Any())
            {
                TempData["ErrorMessage"] = "No resources available. Please add a resource first.";
                return RedirectToAction("Details", "Home");
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
        public async Task<IActionResult> AddBookings([Bind("ResourceId,StartTime,EndTime,BookedBy,Purpose")] Booking booking)
        {


            if (ModelState.IsValid)
            {
      



                // =====================conflicts====================================================
                //  check if the selected date and time is already present in the database for same resource 
                //if there is a conflict in dates   show error messege in the view
                //==============================================================================

                bool hasConflict = await _dbContext.Bookings.AnyAsync(b =>
                    b.ResourceId == booking.ResourceId &&
                    (booking.StartTime < b.EndTime) && (booking.EndTime > b.StartTime)
                );


                //==================================================
                // if there is a confict, return booking
                // Show a user-friendly message on the form 
                // Repopulate the dropdown for resource selection
                //====================================================
                if (hasConflict) 
                {
                    ModelState.AddModelError(string.Empty, "This resource is already booked during the requested time. Please choose another slot or resource, or adjust your times.");
                    ViewBag.Resources = new SelectList(_dbContext.Resources, "Id", "Name", booking.ResourceId);
                    return View(booking);
                }

                _dbContext.Bookings.Add(booking);
                await _dbContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Booking added successfully!";
                return RedirectToAction("Details", new { id = booking.ResourceId });
            }

            ViewBag.Resources = new SelectList(_dbContext.Resources, "Id", "Name", booking.ResourceId);
            return View(booking);
        }




        //===============================all booking===============================
        //show all list of booking includiing resources
        ////===============================all booking===============================
        public IActionResult AllBookings()
        {
            var bookings = _dbContext.Bookings
                .Include(b => b.Resource)
                .OrderBy(b => b.StartTime)
                .ToList();
            Console.WriteLine($"Bookings count: {bookings.Count}");
            var viewModel = new ResourceDetailsViewModel
            {
                Bookings = bookings
            };
            return View(viewModel);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
