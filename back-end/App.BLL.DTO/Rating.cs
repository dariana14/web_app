using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Rating: IDomainEntityId
{
    public Guid Id { get; set; }
    
    public Guid AdvertisementId { get; set; }
    
    public Guid AppUserId { get; set; }

    public int RatingValue { get; set; } = default!;
}