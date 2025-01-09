using Base.Domain;

namespace App.Domain;

public class Semester: BaseEntityId
{

    public int Number { get; set; }
    
    public ICollection<StudentSubject>? StudentSubjects { get; set; }

    public ICollection<Subject>? Subjects { get; set; }

}