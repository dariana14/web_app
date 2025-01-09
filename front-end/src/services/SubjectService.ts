import { ISubject } from "@/domain/ISubject";
import { BaseEntityService } from "./BaseEntityService";
import { IJWTResponse } from "@/dto/IJWTResponse";
import { IUser } from "@/domain/IUser";

export class SubjectService extends BaseEntityService<ISubject> {
    constructor(setJwtResponse: ((data: IJWTResponse | null) => void)) {
        super('v1/Subjects/', setJwtResponse);
    }

    async getAllWithoutLogin(): Promise<ISubject[] | undefined> {
    
            try {
                const response = await this.axios.get<ISubject[]>('WithoutLogin', {}
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

        async getAllTeacher(): Promise<IUser[] | undefined> {
    
            try {
                const response = await this.axios.get<IUser[]>('Teachers', {}
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

}