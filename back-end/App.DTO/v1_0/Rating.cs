using Base.Contracts.Domain;

namespace App.DTO.v1_0;

public class Rating: IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid AdvertisementId { get; set; }
    
    public Guid AppUserId { get; set; }

    public int RatingValue { get; set; } = default!;
}