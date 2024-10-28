import { IRating } from "@/domain/IRating";
import { BaseEntityService } from "./BaseEntityService";
import { IJWTResponse } from "@/dto/IJWTResponse";
import { AxiosError } from "axios";
import AccountService from "./AccountService";

    export class RatingService extends BaseEntityService<IRating> {
    constructor(setJwtResponse: ((data: IJWTResponse | null) => void)) {
        super('v1/Ratings', setJwtResponse);
    }

    async getAllByAdvertisementAndUserId(jwtData: IJWTResponse, advertisementId: string): Promise<IRating[] | undefined> {

        try {
            const response = await this.axios.get<IRating[]>('user/' + advertisementId,
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

                    const response = await this.axios.get<IRating[]>('user/' + advertisementId,
                        {
                            headers: {
                                'Authorization': 'Bearer ' + jwtData.jwt
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

    getAverageRating(ratings: IRating[]): number {
        if (ratings.length === 0) {
            return 0;
        }
    
        const totalRating = ratings.reduce((acc, rating) => acc + rating.ratingValue, 0);
        return totalRating / ratings.length;
    }
    

    async getAllByAdvertisementId(advertisementId: string): Promise<IRating[] | undefined> {

        try {
            const response = await this.axios.get<IRating[]>('advertisement/' + advertisementId,{});

            console.log('response', response);
            if (response.status === 200) {
                return response.data;
            }
            return undefined;

        } catch (e) {
            console.log('error: ', (e as Error).message);

            return undefined;
        }
    }
}