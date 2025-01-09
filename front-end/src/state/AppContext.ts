import { IJWTResponse } from "@/dto/IJWTResponse";
import { createContext } from "react";

    // export interface IUserInfo {
    //     "token": string,
    //     "refreshToken": string,
    //     "firstName": string,
    //     "lastName": string
    // }

export interface IUserContext {
    userInfo: IJWTResponse | null,
    setUserInfo: (userInfo: IJWTResponse | null) => void
}


export const AppContext = createContext<IUserContext | null>(null);