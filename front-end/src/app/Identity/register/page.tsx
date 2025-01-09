"use client"
import { IRegisterData } from "@/dto/IRegiserData";
import AccountService from "@/services/AccountService";
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";
import { useContext, useState } from "react";

export default function Login() {
    const router = useRouter();
    const [email, setEmail] = useState("test@gmail.com");
    const [pwd, setPwd] = useState("1234567!Ss");
    const [firstname, setFirstname] = useState("test");
    const [lastname, setLastname] = useState("test1");
    const [selectedRole, setSelectedRole] = useState("Student");


    const [validationError, setvalidationError] = useState("");

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const validateAndRegister = async () => {
        if (email.length < 5 || pwd.length < 6 || firstname.length < 1 || lastname.length < 1) {
            setvalidationError("Invalid input lengths");
            return;
        }

        let isTeacher = true;
        if(selectedRole == "Student"){
            isTeacher = false;
        }

        const registerData = {
            email: email,
            password: pwd,
            firstname: firstname,
            lastname: lastname,
            isTeacher: isTeacher
        } as IRegisterData

        const response = await AccountService.register(registerData);
        if (response.data) {
            setUserInfo(response.data);
            router.push("/");
        }
        if (response.errors && response.errors.length > 0) {
            setvalidationError(response.errors[0]);
            return;
        }
    }

    return (
        <div className="row">
            <div className="col-md-5">
                <h2>Log in</h2>
                <hr />
                <div className="text-danger" role="alert">{validationError}</div>
                <div className="form-floating mb-3">
                    <input
                        value={email}
                        onChange={(e) => { setEmail(e.target.value); setvalidationError(""); }}
                        id="email" type="email" className="form-control" autoComplete="email" placeholder="name@example.com" />
                    <label htmlFor="email" className="form-label">Email</label>
                </div>
                <div className="form-floating mb-3">
                    <input
                        value={pwd}
                        onChange={(e) => { setPwd(e.target.value); setvalidationError(""); }}
                        id="password" type="password" className="form-control" autoComplete="password" placeholder="password" />
                    <label htmlFor="password" className="form-label">Password</label>
                </div>
                <div className="form-floating mb-3">
                <input
                    value={firstname}
                    onChange={(e) => { setFirstname(e.target.value); setvalidationError(""); }}
                    className="form-control" autoComplete="firstname" aria-required="true" placeholder="John" type="text"
                    id="Input_FirstName" maxLength={128} name="firstName" />
                <label htmlFor="Input_FirstName">First name</label>
                </div>
                <div className="form-floating mb-3">
                    <input
                        value={lastname}
                        onChange={(e) => { setLastname(e.target.value); setvalidationError(""); }}
                        className="form-control" autoComplete="lastname" aria-required="true" placeholder="Smith" type="text"
                        id="Input_LastName" maxLength={128} name="lastName" />
                    <label htmlFor="Input_LastName">Last name</label>
                </div>

                <div className="form-group">
                <label htmlFor="options" className="form-label">Select role:</label>
                <select
                    id="options"
                    value={selectedRole}
                    onChange={(e) => {setSelectedRole(e.target.value); setvalidationError("");}}
                    className="form-control"
                >
                    <option key="Student" value="Student">
                            Student
                    </option>
                    <option key="Teacher" value="Teacher">
                            Teacher
                    </option> 
                   
                </select>
                </div>

                <br/>

                <div>
                    <button onClick={(e) => validateAndRegister()} className="w-100 btn btn-lg btn-primary">Register</button>
                </div>
            </div>
        </div>

    );
}