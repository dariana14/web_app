using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class StudentSubject: BaseEntityId, IDomainAppUser<AppUser>
{
    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public Guid SubjectId { get; set; }
    public Subject? Subject { get; set; }

    public bool Accepted { get; set; }
    
    public int SemesterNumber { get; set; }
    
    public ICollection<Grade>? Grades { get; set; }
}