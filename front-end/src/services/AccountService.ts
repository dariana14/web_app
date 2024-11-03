import axios from "axios";
import { IRegisterData } from "@/dto/IRegiserData";
import { IJWTResponse } from "@/dto/IJWTResponse";
import { IResultObject } from "@/dto/IResultObject";

export default class AccountService {
    constructor() {

    }

    private static httpClient = axios.create({
        baseURL: 'http://localhost:5164/api/v1/identity/Account/',
    });

    static async login(email: string, pwd: string): Promise<IResultObject<IJWTResponse>> {
        const loginData = {
            email: email,
            password: pwd
        }
        try {
            const response = await AccountService.httpClient.post<IJWTResponse>("login", loginData);
            if (response.status < 300) {
                return {
                    data: response.data
                }
            }
            return {
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error: any) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }

    async refreshToken(data: IJWTResponse): Promise<IJWTResponse | undefined> {
        console.log("Refreshing token with:", data.refreshToken);

    try {
        const response = await AccountService.httpClient.post<IJWTResponse>(
            'refreshTokenData', 
            data
        );

        if (response.status === 200 && response.data) {
            console.log('Successfully refreshed token:', response.data);
            return response.data;
        } else {
            console.warn('Unexpected response status:', response.status);
            return undefined;
        }
    } catch (e) {
        if (axios.isAxiosError(e)) {
            console.error('Axios error response:', e.response?.data || e.message);
        } else {
            console.error('Unexpected error:', (e as Error).message);
        }
        return undefined;
    }
    }

    static async register(data: IRegisterData): Promise<IResultObject<IJWTResponse>> {
        try {
            const response = await AccountService.httpClient.post<IJWTResponse>('register', data);

            console.log('register response', response);
            if (response.status === 200) {
                return {
                    data: response.data
                }
            }
            return {
                errors: [response.status.toString() + " " + response.statusText]
            }
        } catch (error) {
            return {
                errors: [JSON.stringify(error)]
            };
        }
    }

}