import { BaseEntityService } from "./BaseEntityService";
import { IJWTResponse } from "@/dto/IJWTResponse";
import { IGrade } from "@/domain/IGrade";

export class GradeService extends BaseEntityService<IGrade> {
    constructor(setJwtResponse: ((data: IJWTResponse | null) => void)) {
        super('v1/Grades', setJwtResponse);
    }

    async getAllBySubjectId(id: string): Promise<IGrade[] | undefined> {
            
        try {
            const response = await this.axios.get<IGrade[]>('/studentSubject/' + id, {}
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