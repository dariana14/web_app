"use client";

import { useEffect, useState } from "react";
import { ISubject } from "@/domain/ISubject";
import { SubjectService } from "@/services/SubjectService";
import { AppContext } from "@/state/AppContext";
import { useContext } from "react";
import { IUser } from "@/domain/IUser";

export default function HomePage() {
    const [subjects, setSubjects] = useState<ISubject[]>([]);
    const [filteredSubjects, setFilteredSubjects] = useState<ISubject[]>([]);
    const [teachers, setTeachers] = useState<IUser[]>([]);
    const [selectedTeacherEmail, setSelectedTeacherEmail] = useState<string>("");

    const [sortOrder, setSortOrder] = useState<"asc" | "desc">("asc");

    const {setUserInfo } = useContext(AppContext)!;
    const subjectService = new SubjectService(setUserInfo!);

    useEffect(() => {
        const loadSubjectsAndTeachers = async () => {
            // Fetch subjects
            const subjectsResponse = await subjectService.getAllWithoutLogin();
            if (subjectsResponse) {
                setSubjects(subjectsResponse);
                setFilteredSubjects(subjectsResponse);
            }

            // Fetch teachers
            const teachersResponse = await subjectService.getAllTeacher();
            if (teachersResponse) {
                setTeachers(teachersResponse);
            }
        };

        loadSubjectsAndTeachers();
    }, []);

    const handleSort = (order: "asc" | "desc") => {
        const sortedSubjects = [...filteredSubjects].sort((a, b) => {
            if (order === "asc") {
                return a.subjectName.localeCompare(b.subjectName);
            } else {
                return b.subjectName.localeCompare(a.subjectName);
            }
        });

        setSortOrder(order);
        setFilteredSubjects(sortedSubjects);
    };

    const handleFilterByTeacher = (email: string) => {
        setSelectedTeacherEmail(email);
        if (email === "") {
            setFilteredSubjects(subjects);
        } else {
            const filtered = subjects.filter(subject => subject.teacherEmail === email);
            setFilteredSubjects(filtered);
        }
    };

    return (
        <div className="container">
            <h2 className="mb-4">Subject List</h2>

            <div className="d-flex justify-content-between mb-4">
            <div className="mb-4">
                <label htmlFor="sortOrder" className="form-label me-2">Sort By:</label>
                <select
                    id="sortOrder"
                    className="form-select d-inline w-auto"
                    value={sortOrder}
                    onChange={(e) => handleSort(e.target.value as "asc" | "desc")}
                >
                    <option value="asc">Aa - Zz</option>
                    <option value="desc">Zz - Aa</option>
                </select>
            </div>

                <div>
                    <select
                        className="form-select"
                        value={selectedTeacherEmail}
                        onChange={(e) => handleFilterByTeacher(e.target.value)}
                    >
                        <option value="">All Teachers</option>
                        {teachers.map(teacher => (
                            <option key={teacher.email} value={teacher.email}>
                                {teacher.firstName} {teacher.lastName}
                            </option>
                        ))}
                    </select>
                </div>
            </div>

            <div className="row">
                {filteredSubjects.length > 0 ? (
                    filteredSubjects.map(subject => (
                        <div key={subject.id} className="col-md-4 mb-4">
                            <div className="card">
                                <div className="card-body">
                                    <h5 className="card-title">{subject.subjectName}</h5>
                                    <p className="card-text">
                                        <strong>Teacher:</strong> {subject.teacherFirstName} {subject.teacherLastName}
                                    </p>
                                    <p className="card-text">
                                        <strong>Email:</strong> {subject.teacherEmail}
                                    </p>
                                </div>
                            </div>
                        </div>
                    ))
                ) : (
                    <p className="text-center">No subjects available</p>
                )}
            </div>
        </div>
    );
}