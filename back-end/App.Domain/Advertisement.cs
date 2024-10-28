using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Advertisement: BaseEntityId, IDomainAppUser<AppUser>
{
    [MaxLength(255)]
    public string Title { get; set; } = default!;
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid PriceId { get; set; }
    public Price? Price { get; set; }
    
    public Guid LocationId { get; set; }
    public Location? Location { get; set; }
    
    public Guid ServiceId { get; set; }
    public Service? Service { get; set; }
    
    public Guid StatusId { get; set; }
    public Status? Status { get; set; }
    
    public ICollection<Rating>? Ratings { get; set; }
    
}