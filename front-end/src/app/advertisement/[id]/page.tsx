"use client"

import { IAdvertisement } from "@/domain/IAdvertisement";
import { IRating } from "@/domain/IRating";
import { IServicePetCategory } from "@/domain/IServisePetCategory";
import { AllAdvertisementService } from "@/services/AllAdvertisementsService";
import { RatingService } from "@/services/RatingService";
import { ServicePetCategoryService } from "@/services/ServicePetCategoryService";
import { AppContext } from "@/state/AppContext";
import { CRating } from "@coreui/react-pro";
import { useContext, useEffect, useState } from "react";

export default function Advertisement({params} : {params : {id: string}}) {

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const allAdvertisementService = new AllAdvertisementService(setUserInfo);
    const servicePetCategoryService = new ServicePetCategoryService(setUserInfo);
    const ratingService = new RatingService(setUserInfo);

    let advertisementId = params.id;

    const [advertisement, setAdvertisement] = useState<IAdvertisement>();
    const [servicePetCategories, setServicePetCategories] = useState([] as IServicePetCategory[]);

    // rating
    const [avarageRating, setAvarageRating] = useState<number>(0.0);
    const [ratings, setRatings] = useState<IRating[]>([]);
    const [userRating, setUserRating] = useState<number>(0);
    const [hasRated, setHasRated] = useState<boolean>(false); 
    const [ratingSubmitted, setRatingSubmitted] = useState(false);
    const [isOwner, setIsOwner] = useState<boolean>(false);


    useEffect(()  => {
        allAdvertisementService.find(userInfo!, advertisementId).then(
            response => {
                if (response) {
                    setAdvertisement(response);

                    if (userInfo && response.appUserId === userInfo.userId) {
                        setIsOwner(true);
                    }
                }
            
            }
        );

        ratingService.getAllByAdvertisementId(advertisementId).then(
            response => {
                if (response) {
                    setAvarageRating(ratingService.getAverageRating(response));
                }
            }
        );

        if(userInfo){
            ratingService.getAllByAdvertisementAndUserId(userInfo!, advertisementId).then(
                response => {
                    if (response) {
                        setRatings(response);  
                                  
                    }                    
                }
            );
        }

        

    }, [advertisementId, userInfo]);

    useEffect(() => {
        if (advertisement) {
            servicePetCategoryService.getAllByServiceId(advertisement?.serviceId!).then(
                response => {
                    if (response) {
                        setServicePetCategories(response);
                    }
                }
            );
        }

        if (ratings.length > 0){
            setHasRated(true);
            const rating = ratings[0] as IRating;
            setUserRating(rating.ratingValue)
        }    

    }, [advertisement]);

    const submitRating = async () => {

        if (userRating && userInfo) {
            try {
                let createdRating = await ratingService.post(
                    userInfo!, 
                    {
                        ratingValue: userRating,
                        advertisementId: advertisementId,
                    } as IRating
                );
                if(createdRating == undefined){
                    return(<>undefined rating</>)
                }
                setRatingSubmitted(true);
                setHasRated(true);
            } catch (error) {
                console.error("Error submitting rating", error);
            }
        }
    };


    if (servicePetCategories === undefined || advertisement === undefined) return (<></>)

    return (
        <>


        <div className="text-center">
            <h2>{advertisement?.title}</h2>
            <div className="mt-4">
            <CRating value={Math.round(avarageRating)} readOnly={true} />
            </div>
            
        </div>

        <hr />
        <dl className="row">
            <dt className = "col-sm-2">
                Description
            </dt>
            <dd className = "col-sm-10">
                {advertisement?.description}
            </dd>
            <dt className = "col-sm-2">
                Category
            </dt>
            <dd className = "col-sm-10">
                {advertisement?.categoryName}
            </dd>
            <dt className = "col-sm-2">
                Price
            </dt>
            <dd className = "col-sm-10">
                {advertisement?.priceValue}
            </dd>
            <dt className = "col-sm-2">
                Location
            </dt>
            <dd className = "col-sm-10">
                {advertisement?.city}
            </dd>
            <dt className = "col-sm-2">
                Status
            </dt>
            <dd className = "col-sm-10">
                {advertisement?.statusName === 1 ? "active" : "paused"}
            </dd>
        </dl>

        <div className="row">
            <h6>Pet categories</h6>
            {servicePetCategories.map(category =>
                <div className="column">
                    {category.petCategoryName} 
                </div>
            )}
        </div>


        {userInfo && !isOwner && (
                <div>
                    
                    <br />
                    <hr />
                    <h4>Add feedback</h4>
                    {hasRated ? (
                        <p>You rated this service with {userRating} stars.</p>
                    ) : (
                        <>
                            <CRating
                                value={userRating}
                                onChange={(rating) => {
                                    setUserRating(rating || 0);
                                }}
                            />

                            <button
                                className="btn btn-success mt-2"
                                onClick={submitRating}
                                disabled={ratingSubmitted}
                            >
                                Submit
                            </button>
                        </>
                    )}
                    {ratingSubmitted && <p className="text-success">Thank you for your feetback</p>}
                </div>
            )}


        </>
    );
}