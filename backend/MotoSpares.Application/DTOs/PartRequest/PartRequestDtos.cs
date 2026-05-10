using System.ComponentModel.DataAnnotations;

namespace MotoSpares.Application.DTOs.PartRequest;

public class CreatePartRequestDto
{
    [Required]
    [StringLength(100)]
    public string RequestedPartName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }
}

public class UpdatePartRequestStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}

public class PartRequestResponseDto
{
    public int RequestId { get; set; }
    public string RequestedPartName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public Guid CustomerId { get; set; }
}
