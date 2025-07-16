#  Internal Resource Booking System
ASP.NET Core web Resource booking and scheduling system for managing and booking shared resources like rooms and equipment.

####  Features

###  Resource Management
- Add, edit, delete, and view resources
- Each resource has name, location, capacity, and availability

###  Booking Management


- Book available resources for specific time ranges
- View all bookings
- Show upcoming bookings per resource
- Booking conflict check to prevent overlapping bookings

### Validation
-validation
- End time must be after start time
- No double booking allowed for the same time slot

###  User Interface

- Clean and responsive layout
- Easy navigation between resources and bookings

---

##  Getting Started

###  Prerequisites

- Visual Studio
- ASP.NET Core Workload (.NET 8)
- SQL Server LocalDB 


### Installation

1. **Clone the project:**
2. git clone https://github.com/bodirwat/Internal_Resource_Booking_System.git

3. Update Connection String

"ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=InternalResourceBookingDB;Trusted_Connection=false;TrustServerCertificate=true;MultipleActiveResultSets=true;User"
  }

4. Apply the migrations and create the database.

5. Run the application







### Screenshot
# this section shows the screenshot of running application




### Home Page
This shows the main Page.

![Home Page](Screenshots/Homepage.png)

##  Screenshots

---

###  Home Page
This shows the main page of the application.

![Home Page](Screenshots/Homepage.png)

---

###  Add New Resource
Form to add a new resource. All fields are required and must be entered correctly.

![Add Resource](Screenshots/AddResource.png)

---

###  Resource List
Displays a list of all resources with options to view, edit, or delete each.

![Resource List](Screenshots/ResourceList.png)

---

###  Resource Details with Bookings
Shows details of a selected resource, with no bookings made for it.

![Resource Details](Screenshots/ResourceDetailsnBookings.png)

---

###  Resource Details with Bookings
Shows details of a selected resource, including bookings made for it.

![Resource Details](Screenshots/ResourceDetailsBookingAdded.png)

---



###  Edit Resource
Allows the user to update the resource name, location, capacity, etc.

![Edit Resource](Screenshots/EditResource.png)

---

###  Delete Resource
Allows the user to delete a resource from the system (with confirmation).

![Delete Resource](Screenshots/DeleteResource.png)

---

###  Add Booking to a Resource
Form to add a new booking. Prevents overlaps and validates times.

![Add Booking](Screenshots/AddBookingTimeValidate.png)

---

###  Edit Booking
Allows the user to change the time or purpose of an existing booking.

![Edit Booking](Screenshots/EditBooking.png)

---

###  Delete Booking
Allows the user to delete a Booking from the system (with confirmation).

![Edit Booking](Screenshots/deleteBooking.png)




---


###  All Bookings
Displays a list of all bookings across all resources.

![All Bookings](Screenshots/AllBookings.png)


