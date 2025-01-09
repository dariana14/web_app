"use client"

import { IStudentSubject } from "@/domain/IStudentSubject";
import { ISubject } from "@/domain/ISubject";
import { StudentSubjectService } from "@/services/StudentSubjectService";
import { SubjectService } from "@/services/SubjectService";
import { AppContext } from "@/state/AppContext";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { useContext, useEffect, useState } from "react";

export default function UserTeacher() {
    const [isLoading, setIsLoading] = useState(true);
    const [declaredSubjects, setDeclaredSubjects] = useState<IStudentSubject[]>([]);
    const [subjects, setSubjects] = useState<ISubject[]>([]);

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const studentSubjectService = new StudentSubjectService(setUserInfo!);
    const subjectService = new SubjectService(setUserInfo!);

    const [title, setTitle] = useState("");
    const [validationError, setValidationError] = useState("");

    const router = useRouter();

    useEffect(() => {
        if (!userInfo) {
            router.push("/Identity/login"); 
        }
      }, [userInfo, router]);

    const loadData = async () => {
        const response = await subjectService.getAll(userInfo!);
        if (response) {
            setSubjects(response);
        }

        const subjects = await studentSubjectService.getAll1(userInfo!);
        if (subjects) {
            setDeclaredSubjects(subjects.filter(a => a.accepted == false));
        }

        setIsLoading(false);
    };

    useEffect(() => {
        loadData();
    }, []);

    const getStudentSubjectsBySubjectId = (id: string, all: IStudentSubject[]) => {
        return all.filter(subject => subject.subjectId === id);
    };

    const drawStudentSubject = (subject: IStudentSubject) => {
        return (
            <li className="list-group-item d-flex justify-content-between align-items-center">
                <h5 className="mb-1">{subject.studentFullName}</h5>
                <button onClick={(e) => onAccept(subject)} className="btn btn-primary">
                    Accept
                </button>
            </li>
        );
    };

    const drawSubject = (subject: ISubject) => {
        return (
            <div className="card mb-4" style={{ width: "50%", marginRight: "auto"}}>
                <div className="card-body">
                    <h5 className="card-title">{subject.subjectName}</h5>
                    <Link href={"/subjectStudentsAndGrades/" + subject.id}>
                        <button className="btn btn-primary mb-3">Manage Grades</button>
                    </Link>

                    <h6>Declarations</h6>
                    <ul className="list-group">
                        {getStudentSubjectsBySubjectId(subject.id, declaredSubjects).length > 0 ? (
                            getStudentSubjectsBySubjectId(subject.id, declaredSubjects).map(subject =>
                                drawStudentSubject(subject)
                            )
                        ) : (
                            <li className="list-group-item">No declarations available</li>
                        )}
                    </ul>
                </div>
            </div>
        );
    };

    const onAccept = async (subject: IStudentSubject) => {
        let studentSubject = {
            id: subject.id,
            appUserId: subject.appUserId,
            subjectId: subject.subjectId,
            semesterNumber: subject.semesterNumber,
            accepted: true,
            subjectName: subject?.subjectName
        } as IStudentSubject;

        let createdStudentSubject = await studentSubjectService.put(userInfo!, subject.id, studentSubject);

        if (createdStudentSubject === undefined) {
            return;
        }

        loadData();
    };

    const onSubmit = async () => {
        if (title.length < 2) {
            setValidationError("Subject name must be at least 2 characters long.");
            return;
        }

        setValidationError(""); 

        let newSubject = {
            subjectName: title,
            semesterNumber: 12
        } as ISubject;

        await subjectService.post(userInfo!, newSubject);
        setTitle(""); 
        loadData();
    };

    if (isLoading) return (<h1>LOADING</h1>);

    if (declaredSubjects === undefined || subjects === undefined) return (<>undefined</>);

    return (
        <div className="container">
            <h2 className="text-left mb-4">Your Subjects</h2>

            {subjects.map(subject => drawSubject(subject))}

            <br/>
            <br/>

            <div className="card mt-5" style={{ width: "50%", marginRight: "auto" }}>
                <div className="card-body">
                    <h4 className="card-title">Add New Subject</h4>
                    <div className="form-group">
                        <label htmlFor="title" className="form-label">Name</label>
                        <input
                            value={title}
                            onChange={(e) => { setTitle(e.target.value); setValidationError(""); }}
                            id="title" type="text" className="form-control" placeholder="Enter subject name" />
                    </div>
                    {validationError && <p className="text-danger mt-2">{validationError}</p>}
                    <button onClick={(e) => onSubmit()} className="btn btn-primary mt-3">
                        Add Subject
                    </button>
                </div>
            </div>
        </div>
    );
}
