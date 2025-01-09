"use client"

import { useState, useEffect, useContext } from "react";
import { IGrade } from "@/domain/IGrade";
import { IStudentSubject } from "@/domain/IStudentSubject";
import { StudentSubjectService } from "@/services/StudentSubjectService";
import { GradeService } from "@/services/GradeService";
import { AppContext } from "@/state/AppContext";
import { ISubject } from "@/domain/ISubject";
import { SubjectService } from "@/services/SubjectService";
import { useRouter } from "next/navigation";

export default function SubjectStudentsAndGrades({ params }: { params: { id: string } }) {
    const subjectId = params.id;
    const [isLoading, setIsLoading] = useState(true);
    const [studentSubjects, setStudentSubjects] = useState<IStudentSubject[]>([]);
    const [gradesMap, setGradesMap] = useState<{ [key: string]: IGrade[] }>({});
    const [subject, setSubject] = useState<ISubject>();

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const studentSubjectService = new StudentSubjectService(setUserInfo!);
    const gradeService = new GradeService(setUserInfo!);
    const subjectService = new SubjectService(setUserInfo!);

    const router = useRouter();

    useEffect(() => {
        if (!userInfo) {
            router.push("/Identity/login"); 
        }
    }, [userInfo, router]);
    
    const loadData = async () => {

        const response = await subjectService.find(userInfo!, subjectId)
        if (response) {
            setSubject(response);
        }

        const studentSubjectsResponse = await studentSubjectService.getAllBySubjectId(subjectId);

        if (studentSubjectsResponse) {
            setStudentSubjects(studentSubjectsResponse);
            const gradesMapResponse: { [key: string]: IGrade[] } = {};
            for (const studentSubject of studentSubjectsResponse) {
                const gradesResponse = await gradeService.getAllBySubjectId(studentSubject.id);
                gradesMapResponse[studentSubject.id] = gradesResponse || [];
            }

            setGradesMap(gradesMapResponse);
        }

        setIsLoading(false);
    };

    useEffect(() => {
        loadData();
    }, []);

    const calculateAverage = (grades: IGrade[]) => {
        const numericGrades = grades.filter(g => !g.isOverall).map(g => g.gradeValue);
        if (numericGrades.length === 0) return undefined;
        return (numericGrades.reduce((sum, grade) => sum + grade, 0) / numericGrades.length).toFixed(2);
    };

    const [newGrade, setNewGrade] = useState<number>();
    const [newOverallGrade, setNewOverallGrade] = useState<number>();
    const [validationError, setValidationError] = useState<string>("");

    const onAddGrade = async (studentSubjectId: string) => {
        if (newGrade === undefined || newGrade.toString() === "NaN") {
            setValidationError("Grade value cannot be empty");
            return;
        }

        if (newGrade < 0 || newGrade > 5) {
            setValidationError("Grade can be between 0-5");
            return;
        }

        const grade = {
            studentSubjectId,
            gradeValue: newGrade,
            isOverall: false,
        } as IGrade;

        const createdGrade = await gradeService.post(userInfo!, grade);
        if (createdGrade === undefined) {
            setValidationError("Error adding grade" + newGrade.toString());
            return;
        }

        setValidationError("");
        loadData();
    };

    const onAddOverallGrade = async (studentSubjectId: string) => {
        if (newOverallGrade === undefined || newOverallGrade.toString() === "NaN") {
            setValidationError("Grade value cannot be empty");
            return;
        }

        if (newOverallGrade < 0 || newOverallGrade > 5) {
            setValidationError("Grade can be between 0-5");
            return;
        }

        const grade = {
            studentSubjectId,
            gradeValue: newOverallGrade,
            isOverall: true,
        } as IGrade;

        const createdGrade = await gradeService.post(userInfo!, grade);
        if (createdGrade === undefined) {
            setValidationError("Error adding overall grade");
            return;
        }

        setValidationError("");
        loadData();
    };

    if (isLoading) return (<h1>Loading...</h1>);

    return (
        <div>
            <h3>{subject?.subjectName}</h3>
            <table className="table">
                <thead>
                    <tr>
                        <th>Student (Email)</th>
                        <th>Overall Grade</th>
                        <th>Grades</th>
                        <th>Average Grade</th>
                        <th style={{ width: "15%" }}> </th>
                        <th style={{ width: "15%" }}> </th>
                    </tr>
                </thead>
                <tbody>
                    {studentSubjects.map(studentSubject => {
                        const grades = gradesMap[studentSubject.id] || [];
                        const overallGrade = grades.find(g => g.isOverall)?.gradeValue;
                        const averageGrade = calculateAverage(grades);
                        const gradeValues = grades.filter(g => !g.isOverall).map(g => g.gradeValue).join(", ");

                        return (
                            <tr key={studentSubject.id}>
                                <td>{studentSubject.studentFullName}</td>
                                <td>{overallGrade !== undefined ? overallGrade : "N/A"}</td>
                                <td>{gradeValues}</td>
                                <td>{averageGrade !== undefined ? averageGrade : "N/A"}</td>
                                <td>
                                    <div className="form-group">
                                        <label>New Grade:</label>
                                        <input
                                            type="number"
                                            className="form-control"
                                            onChange={e => setNewGrade(parseInt(e.target.value))}
                                        />
                                        <button
                                            className="btn btn-primary mt-1"
                                            onClick={() => onAddGrade(studentSubject.id)}
                                        >
                                            Add Grade
                                        </button>
                                    </div>
                                </td>
                                <td>
                                    {overallGrade === undefined && (
                                        <div className="form-group">
                                            <label>Overall Grade:</label>
                                            <input
                                                type="number"
                                                className="form-control"
                                                onChange={e => setNewOverallGrade(parseInt(e.target.value))}
                                            />
                                            <button
                                                className="btn btn-primary mt-1"
                                                onClick={() => onAddOverallGrade(studentSubject.id)}
                                            >
                                                Add Overall Grade
                                            </button>
                                        </div>
                                    )}
                                </td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>

            {validationError && <div className="alert alert-danger">{validationError}</div>}
        </div>
    );
}