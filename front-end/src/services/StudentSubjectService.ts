import { IStudentSubject } from "@/domain/IStudentSubject";
import { BaseEntityService } from "./BaseEntityService";
import { IJWTResponse } from "@/dto/IJWTResponse";
import { AxiosError } from "axios";
import AccountService from "./AccountService";

export class StudentSubjectService extends BaseEntityService<IStudentSubject> {
    constructor(setJwtResponse: ((data: IJWTResponse | null) => void)) {
        super('v1/StudentSubjects', setJwtResponse);
    }

    async getAllBySubjectId(id: string): Promise<IStudentSubject[] | undefined> {
        
                try {
                    const response = await this.axios.get<IStudentSubject[]>('/subject/' + id, {}
                    );
        
                    console.log('response', response);
                    if (response.status === 200) {
                        return response.data;
                    }
                    return undefined;
        
                } catch (e) {
                    console.log('error: ', (e as Error).message);
                    }
                    return undefined;
            }
    

    async getAll1(jwtData: IJWTResponse): Promise<IStudentSubject[] | undefined> {
    
            try {
                const response = await this.axios.get<IStudentSubject[]>('/all',
                    {
                        headers: {
                            'Authorization': 'Bearer ' + jwtData.jwt
                        }
                    }
                );
    
                console.log('response', response);
                if (response.status === 200) {
                    return response.data;
                }
                return undefined;
    
            } catch (e) {
                console.log('error: ', (e as Error).message);
    
                //===========================================================================
    
                if ((e as AxiosError).response?.status === 401) {
                    console.error("JWT expired, refreshing!");
                    // try to refresh the jwt
                    let identityService = new AccountService();
                    let refreshedJwt = await identityService.refreshToken(jwtData);
    
                    console.log("refreshed jwt:" + refreshedJwt)
    
                    if (refreshedJwt) {
                        this.setJwtResponse(refreshedJwt);
    
                        const response = await this.axios.get<IStudentSubject[]>('',
                            {
                                headers: {
                                    'Authorization': 'Bearer ' + refreshedJwt.jwt
                                }
                            }
                        );
                        if (response.status === 200) {
                            return response.data;
                        }
                    }
                }
    
                return undefined;
            }
        }

}