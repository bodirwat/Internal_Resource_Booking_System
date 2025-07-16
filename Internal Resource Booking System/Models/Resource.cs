using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Internal_Resource_Booking_System.Models;

public partial class Resource
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Location is required.")]
    [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters.")]
    public string Location { get; set; } = null!;

    [Required(ErrorMessage = "Capacity is required.")]
    [Range(1, 1000, ErrorMessage = "Capacity must be at least 1.")]
    public int Capacity { get; set; }

    [Required(ErrorMessage = "Availability is required.")]
    public bool IsAvailable { get; set; } = true;

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}
public class HomeViewModel
{
    public int TotalBookings { get; set; }
    public int TotalResources { get; set; }
    public int BookingsToday { get; set; }
    public List<Booking> RecentBookings { get; set; }
}
