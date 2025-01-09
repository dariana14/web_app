"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { IStudentSubject } from "@/domain/IStudentSubject";
import { ISubject } from "@/domain/ISubject";
import { StudentSubjectService } from "@/services/StudentSubjectService";
import { SubjectService } from "@/services/SubjectService";
import { AppContext } from "@/state/AppContext";
import { useContext } from "react";
import { useRouter } from "next/navigation";

export default function UserStudent() {
    const [isLoading, setIsLoading] = useState(true);
    const [acceptedSubjects, setAcceptedSubjects] = useState<IStudentSubject[]>([]);
    const [declaredSubjects, setDeclaredSubjects] = useState<IStudentSubject[]>([]);
    const [subjects, setSubjects] = useState<ISubject[]>([]);

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const studentSubjectService = new StudentSubjectService(setUserInfo!);
    const subjectService = new SubjectService(setUserInfo!);

    //form
    const [validationError, setValidationError] = useState("");
    const [selectedSubjectId, setSelectedSubjectId] = useState("");

    const router = useRouter();

    useEffect(() => {
        if (!userInfo) {
            router.push("/Identity/login"); // Redirect to login page
        }
      }, [userInfo, router]);

    const loadData = async () => {
        const response = await subjectService.getAllWithoutLogin();
        if (response) {
            setSubjects(response);
        }

        const response1 = await studentSubjectService.getAll(userInfo!);
        if (response1) {
            setAcceptedSubjects(response1.filter(a => a.accepted == true));
            setDeclaredSubjects(response1.filter(a => a.accepted == false));
        }

        setIsLoading(false);
    };

    useEffect(() => {
        loadData();
    }, []);

    const drawAcceptedSubject = (subject: IStudentSubject) => {
        return (
            <li className="list-group-item d-flex justify-content-between align-items-center bg-light shadow-sm p-3 mb-3 rounded" style={{ width: "50%", textAlign: "left" }}>
                <div className="d-flex flex-column">
                    <h6 className="mb-1 text-primary fw-bold">{subject.subjectName}</h6>
                </div>
                <Link href={`/studentGrades/${subject.id}`}>
                    <button className="btn btn-outline-primary">See Grades</button>
                </Link>
            </li>
        );
    };

    const drawDeclaredSubject = (subject: IStudentSubject) => {
        return (
            <li className="list-group-item bg-light shadow-sm p-3 mb-3 rounded" style={{ width: "50%", textAlign: "left" }}>
                <h5 className="mb-1 text-secondary">{subject.subjectName}</h5>
            </li>
        );
    };

    const onSubmit = async () => {
        if (selectedSubjectId.length === 0) {
            setValidationError("You need to choose subject");
            return;
        }

        let subject = await subjectService.find(userInfo!, selectedSubjectId);

        let studentSubject = {
            subjectId: selectedSubjectId,
            semesterNumber: subject?.semesterNumber,
            accepted: false,
        } as IStudentSubject;

        let createdStudentSubject = await studentSubjectService.post(userInfo!, studentSubject);

        if (createdStudentSubject === undefined) {
            setValidationError("Subject is already declared");
            return;
        }

        setValidationError("");
        loadData();
    };

    if (isLoading) return (<h1 className="text-center text-primary">LOADING...</h1>);

    if (acceptedSubjects === undefined || declaredSubjects === undefined || subjects === undefined) return (<p className="text-center text-danger">Undefined data</p>);

    return (
        <div className="container mt-5">
            <h3 className="text-primary mb-4" style={{ width: "50%", textAlign: "left" }}>Your Subjects</h3>

            <ul className="list-group mb-5">
                {acceptedSubjects.map(subject =>
                    drawAcceptedSubject(subject))}
            </ul>

            <h3 className="text-secondary mb-4" style={{ width: "50%", textAlign: "left" }}>Declared Subjects</h3>

            <ul className="list-group mb-5">
                {declaredSubjects.map(subject =>
                    drawDeclaredSubject(subject))}
            </ul>

            <hr />

            <div className="text-danger mb-3" role="alert" style={{ width: "50%", textAlign: "left" }}>{validationError}</div>

            <h3 className="text-primary mb-4" style={{ width: "50%", textAlign: "left" }}>Declare New</h3>

            <div className="form-group mb-3" style={{ width: "30%", textAlign: "left" }}>
                <label htmlFor="options" className="form-label">Select subject:</label>
                <select
                    id="options"
                    value={selectedSubjectId}
                    onChange={(e) => { setSelectedSubjectId(e.target.value); setValidationError(""); }}
                    className="form-control"
                >
                    <option value="" disabled>Select subject</option>
                    {subjects.map(subject => (
                        <option key={subject.id} value={subject.id}>
                            {subject.subjectName}
                        </option>
                    ))}
                </select>
            </div>

            <div className="form-group" style={{ width: "50%", textAlign: "left" }}>
                <button onClick={(e) => onSubmit()} className="btn btn-primary">
                    Declare
                </button>
            </div>
        </div>
    );
}
