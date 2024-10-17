using System;
using System.Collections.Generic;

namespace PrescottAppBackend.Domain.DbModels;

public partial class Reservation
{
    public int Id { get; set; }

    public int BuildingId { get; set; }

    public int AmenityId { get; set; }

    public string? Reason { get; set; }

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }
}
