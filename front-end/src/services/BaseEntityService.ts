import { IBaseEntity } from "@/domain/IBaseEntity";
import { BaseService } from "./BaseService";
import { AxiosError } from "axios";
import AccountService from "./AccountService";
import { IJWTResponse } from "@/dto/IJWTResponse";

export abstract class BaseEntityService<TEntity extends IBaseEntity> extends BaseService {

    constructor(
        baseUrl: string,
        protected setJwtResponse: ((data: IJWTResponse | null) => void)) { 
        super(baseUrl);
    }

    async getAll(jwtData: IJWTResponse): Promise<TEntity[] | undefined> {

        try {
            const response = await this.axios.get<TEntity[]>('',
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

                    const response = await this.axios.get<TEntity[]>('',
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

    async find(jwtData: IJWTResponse, id?: string): Promise<TEntity | undefined> {

        try {
            const response = await this.axios.get<TEntity>('/' + id,
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
                if (refreshedJwt) {
                    this.setJwtResponse(refreshedJwt);

                    const response = await this.axios.get<TEntity>('',
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

    async post(jwtData: IJWTResponse, entity: TEntity): Promise<TEntity | undefined> {

        try {
            const response = await this.axios.post<TEntity>('', entity,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            console.log('response', response);
            if (response.status === 201) {
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
                if (refreshedJwt) {
                    this.setJwtResponse(refreshedJwt);

                    const response = await this.axios.post<TEntity>('', entity,
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

    async put(jwtData: IJWTResponse, entityId: string, entity: TEntity) {
        try {
            const response = await this.axios.put<TEntity>('/' + entityId, entity,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            console.log('response', response);
            if (response.status === 204) {
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
                if (refreshedJwt) {
                    this.setJwtResponse(refreshedJwt);

                    const response = await this.axios.put<TEntity>('/' + entityId, entity,
                        {
                            headers: {
                                'Authorization': 'Bearer ' + jwtData.jwt
                            }
                        }
                    );
                    if (response.status === 204) {
                        return response.data;
                    }
                }
            }

            return undefined;
        }
    }

    async delete(jwtData: IJWTResponse, entityId: string) {
        try {
            const response = await this.axios.delete('/' + entityId,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            console.log('response', response);
            if (response.status === 204) {
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
                if (refreshedJwt) {
                    this.setJwtResponse(refreshedJwt);

                    const response = await this.axios.delete('/' + entityId,
                        {
                            headers: {
                                'Authorization': 'Bearer ' + jwtData.jwt
                            }
                        }
                    );
                    if (response.status === 204) {
                        return response.data;
                    }
                }
            }

            return undefined;
        }
    }


}
