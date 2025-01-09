"use client"
import { IJWTResponse } from "@/dto/IJWTResponse";
import { AppContext} from "@/state/AppContext";
import { useState } from "react";

export default function AppState({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {

    const [userInfo, setUserInfo] = useState<IJWTResponse | null>(null);

    return (
        <AppContext.Provider value={{ userInfo, setUserInfo }}>
            {children}
        </AppContext.Provider>
    );
}