using System;
using System.ComponentModel.DataAnnotations;

namespace Internal_Resource_Booking_System.Models;

public partial class Booking
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Resource is now now required")]
    public int ResourceId { get; set; }
    [Range(typeof(DateTime), "1/1/1753", "12/31/9999", ErrorMessage = "End Time must be between 1/1/1753 and 12/31/9999")]
    [Required(ErrorMessage = "Start Time is required")]
    public DateTime StartTime { get; set; }

    [Range(typeof(DateTime), "1/1/1753", "12/31/9999", ErrorMessage = "End Time must be between 1/1/1753 and 12/31/9999")]
    [Required(ErrorMessage = "End Time is required")]
    public DateTime EndTime { get; set; }

    [StringLength(100, ErrorMessage = "Booked By name cannot exceed 100 characters")]
    public string? BookedBy { get; set; }

    [StringLength(255, ErrorMessage = "Purpose cannot exceed 255 characters")]
    public string? Purpose { get; set; }

    public virtual Resource? Resource { get; set; }
}


public class ResourceDetailsViewModel
{
    public Resource Resource { get; set; }
    public List<Booking> Bookings { get; set; }
}


