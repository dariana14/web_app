using App.Domain.Identity;
using Base.Contracts.Domain;
using Base.Domain;

namespace App.Domain;

public class Subject: BaseEntityId, IDomainAppUser<AppUser>
{
    public String SubjectName { get; set; } = default!;

    public int SemesterNumber { get; set; }

    public Guid AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
    
    public ICollection<StudentSubject>? StudentSubjects { get; set; }

}