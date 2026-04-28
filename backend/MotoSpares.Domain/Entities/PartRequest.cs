using System.ComponentModel.DataAnnotations;
using MotoSpares.Domain.Enums;

namespace MotoSpares.Domain.Entities;

public class PartRequest
{
    public int RequestId { get; set; }

    [Required]
    [StringLength(100)]
    public string RequestedPartName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? RequestDescription { get; set; }

    public DateTime RequestDate { get; set; }

    public PartRequestStatus RequestStatus { get; set; }

    public ICollection<UserPartRequest> UserPartRequests { get; set; } = new List<UserPartRequest>();
}
