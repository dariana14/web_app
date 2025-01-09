using Base.Contracts.Domain;

namespace App.DAL.DTO;

public class Subject: IDomainEntityId
{
    public String SubjectName { get; set; } = default!;
    public Guid AppUserId { get; set; }
    
    public int SemesterNumber { get; set; }
    public Guid Id { get; set; }
    
    public String? TeacherFirstName { get; set; }

    public String? TeacherLastName { get; set; }

    public String? TeacherEmail { get; set; }
}