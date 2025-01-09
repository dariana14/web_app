using Base.Domain;

namespace App.Domain;

public class Grade: BaseEntityId
{
    public int GradeValue { get; set; }

    public bool IsOverall { get; set; }
    
    public Guid StudentSubjectId { get; set; }
    public StudentSubject? StudentSubject { get; set; }
}