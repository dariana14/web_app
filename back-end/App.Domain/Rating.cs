using App.Domain.Identity;
using Base.Domain;

namespace App.Domain;

public class Rating: BaseEntityId
{
    public int RatingValue { get; set; } = default!;
    
    public Guid AdvertisementId { get; set; }
    public Advertisement? Advertisement { get; set; }
    
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}