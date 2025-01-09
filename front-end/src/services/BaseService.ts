import axios, { AxiosInstance } from "axios";

export abstract class BaseService {

    private static hostBaseURL = "http://localhost:5164/api/";

    protected axios: AxiosInstance;

    constructor(baseURL: string) {
        
        this.axios = axios.create (
            {
                baseURL: BaseService.hostBaseURL + baseURL,
                headers: {
                    common: {
                        'Content-Type': 'application/json'
                    }
                }
            }
        );


        this.axios.interceptors.request.use((request) => {
            console.log('Starting Request', JSON.stringify(request, null, 2))
            return request
        })
    }
}
