using Base.Contracts.Domain;

namespace App.BLL.DTO;

public class Grade: IDomainEntityId
{
    public int GradeValue { get; set; }

    public bool IsOverall { get; set; }
    
    public Guid StudentSubjectId { get; set; }
    public Guid Id { get; set; }
}