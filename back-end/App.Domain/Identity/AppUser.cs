using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Microsoft.AspNetCore.Identity;

namespace App.Domain.Identity;

public class AppUser : IdentityUser<Guid>, IDomainEntityId
{
    [MinLength(1)]
    [MaxLength(64)]
    public string FirstName { get; set; } = default!;

    [MinLength(1)]
    [MaxLength(64)]
    public string LastName { get; set; } = default!;
    
    public bool IsTeacher { get; set; } = default!;
    
    public ICollection<Subject>? Subjects { get; set; }
    
    public ICollection<StudentSubject>? StudentSubjects { get; set; }
    public ICollection<AppRefreshToken>? RefreshTokens { get; set; }
}