using Base.Contracts.Domain;
namespace App.DAL.DTO;

public class StudentSubject: IDomainEntityId
{
    public Guid AppUserId { get; set; }
    
    public Guid SubjectId { get; set; }
    
    public bool Accepted { get; set; }
    
    public int SemesterNumber { get; set; }
    public Guid Id { get; set; }
    public String? SubjectName { get; set; }
    
    public String? StudentFullName { get; set; }

}