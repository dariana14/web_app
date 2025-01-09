"use client";

import { useContext, useEffect, useState } from "react";
import { IGrade } from "@/domain/IGrade";
import { GradeService } from "@/services/GradeService";
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";

export default function StudentGrades({ params }: { params: { id: string } }) {
  const studentSubjectId = params.id;

  const { userInfo, setUserInfo } = useContext(AppContext)!;

  const [isLoading, setIsLoading] = useState(true);
  const [grades, setGrades] = useState<IGrade[]>([]);
  const [overallGrade, setOverallGrade] = useState<number | null>(null);

  const gradeService = new GradeService(setUserInfo);

  const router = useRouter();

  useEffect(() => {
    if (!userInfo) {
        router.push("/Identity/login"); 
    }
  }, [userInfo, router]);

  const loadData = async () => {
    if (!studentSubjectId) return;

    const response = await gradeService.getAllBySubjectId(studentSubjectId);
    if (response) {
      setGrades(response);
      setOverallGrade(response.find((g) => g.isOverall)?.gradeValue || null);
    }
    setIsLoading(false);
  };

  useEffect(() => {
    loadData();
  }, [studentSubjectId]);

  const calculateAverageGrade = () => {
    if (grades.length === 0) return null;

    const total = grades.reduce((sum, grade) => sum + grade.gradeValue, 0);
    return (total / grades.length).toFixed(2); 
  };

  if (isLoading) return <h1 className="text-center mt-5">Loading...</h1>;

  if (!grades || grades.length === 0)
    return (
      <div className="container mt-5">
        <h3>No Grades Available</h3>
      </div>
    );

  return (
    <div className="container mt-5" style={{ fontSize: "16px" }}>
      <h3 className="mb-4">Student Grades</h3>
      <br/>
      <br/>

      <div style={{ display: "flex", flexDirection: "row", alignItems: "flex-start", gap: "100px" }}>
        <div>
          {/* Overall Grade Heading */}
          <h4>Overall Grade</h4>
          <p>{overallGrade !== null ? overallGrade : "Not Available"}</p>
        </div>

        <div>
          {/* Average Grade Heading */}
          <h4>Average Grade</h4>
          <p>{calculateAverageGrade() || "Not Available"}</p>
        </div>
      </div>

      <br/>
      <br/>

      {/* Grades List */}
      <div className="card shadow-sm" style={{  width: "30%" }}>
            <div className="card-header bg-primary text-white">
              <h5 className="mb-0">Grades List</h5>
            </div>
            <ul className="list-group list-group-flush">
              {grades.map((grade, index) => (
                <li
                  key={index}
                  className="list-group-item d-flex align-items-center"
                  style={{ fontSize: "14px" }}
                >
                  <strong>{grade.gradeValue}</strong>
                </li>
              ))}
            </ul>
          </div>
    </div>
  );
}
