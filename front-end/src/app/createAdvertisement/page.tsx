"use client"

import { IAdvertisement } from "@/domain/IAdvertisement";
import { ICategory } from "@/domain/ICategory";
import { ILocation } from "@/domain/ILocation";
import { IPetCategory } from "@/domain/IPetCategory";
import { IPrice } from "@/domain/IPrice";
import { IService } from "@/domain/IService";
import { IServicePetCategory } from "@/domain/IServisePetCategory";
import { AdvertisementService } from "@/services/AdvertisementService";
import { CategoryService } from "@/services/CategoryService";
import { LocationService } from "@/services/LocationService";
import { PetCategoryService } from "@/services/PetCategoryService";
import { PriceService } from "@/services/PriceService";
import { ServicePetCategoryService } from "@/services/ServicePetCategoryService";
import { ServiceService } from "@/services/ServiceService";
import { StatusService } from "@/services/StatusService";
import { AppContext } from "@/state/AppContext";
import { useRouter } from "next/navigation";
import { ChangeEvent, useContext, useEffect, useState } from "react";

export default function CreateAdvertisement() {

    const router = useRouter();

    const { userInfo, setUserInfo } = useContext(AppContext)!;

    const priceService = new PriceService(setUserInfo!);
    const locationService = new LocationService(setUserInfo!);
    const serviceService = new ServiceService(setUserInfo!);
    const statusService = new StatusService(setUserInfo!);
    const advertisementService = new AdvertisementService(setUserInfo!);
    const categoryService = new CategoryService(setUserInfo!);
    const petCategoryService = new PetCategoryService(setUserInfo!);
    const servicePetCategoryService = new ServicePetCategoryService(setUserInfo!);

    const [categories, setCategories] = useState([] as ICategory[]);
    const [petCategories, setPetCategories] = useState([] as IPetCategory[]);
    const [activeStatusId, setStatusId] = useState<string>();


    // form handlers
    const [validationError, setValidationError] = useState("");

    const [title, setTitle] = useState("");
    const [price, setPrice] = useState(0.0);
    const [city, setCity] = useState('');
    const [description, setDescription] = useState('');
    const [selectedCategoryId, setSelectedCategoryId] = useState("");
    const [selectedPetCategoryIds, setSelectedPetCategoryIds] = useState<string[]>([]);

    const handleSelectChange = (e: ChangeEvent<HTMLSelectElement>) => {
        const selectedOptions = Array.from(e.target.selectedOptions, option => option.value);
        setSelectedPetCategoryIds(selectedOptions);
        setValidationError("");
    };



    useEffect(() => {

        
        categoryService.getAll(userInfo!).then(
            response => {
                if (response) {
                    setCategories(response);
                }
            }
        );
        petCategoryService.getAll(userInfo!).then(
            response => {
                if (response) {
                    setPetCategories(response);
                }
            }
        );
        statusService.getStatusId(1).then(
            response => {
                if (response) {
                    setStatusId(response);
                }
            }
        );
    }, [userInfo]);

    const onSubmit = async () => {

        // event.preventDefault();

        if (selectedCategoryId.length === 0) {
            setValidationError("You need to choose service category");
            return;
        }
        if (selectedPetCategoryIds.length === 0) {
            setValidationError("You need to choose at least one pet category!");
            return;
        }
        if (title.length < 2 || description.length < 2 || city.length < 2 || price < 1) {
            setValidationError("Invalid input lengths");
            return;
        }

        let createdPrice = await priceService.post(
            userInfo!, 
            {value: price,} as IPrice
        );

        if (createdPrice === undefined) {
            setValidationError("error at creating price");
            return;
        }

        let createdLocation = await locationService.post(
            userInfo!, 
            {
                city: city,
                country: "country"
            } as ILocation
        );

        if (createdLocation === undefined) {
            setValidationError("error at creating location");
            return;
        }

        let createdService = await serviceService.post(
            userInfo!, 
            {
                description: description,
                categoryId: selectedCategoryId
            } as IService
        );

        if (createdService === undefined) {
            setValidationError("error at creating service");
            return;
        }

        let advertisement = {
            title: title,
            priceId: createdPrice?.id,
            locationId: createdLocation?.id,
            statusId: activeStatusId,
            serviceId: createdService?.id,
        } as IAdvertisement;

        let createdAdvertisement = await advertisementService.post(userInfo!, advertisement);

        if (createdAdvertisement === undefined) {
            setValidationError("error at creating advertisement");
            return;
        }

        selectedPetCategoryIds.forEach(async id => {

            let servicePetCategory = {
                serviceId: createdService?.id,
                petCategoryId: id 
            } as IServicePetCategory;

            let createdServicePetCategory = await servicePetCategoryService.post(userInfo!, servicePetCategory)

            if (createdServicePetCategory === undefined) {
                setValidationError("error creating service pet category")
                return;
            }

        });

        router.push("/userAdvertisements");
    }

    if (userInfo === null) router.push("/Identity/login");


    return (
        <>
        <h1>Create</h1>
        <h4>Advertisement</h4>
        <hr />
        <div className="row">
        <div className="col-md-4">
        <div className="text-danger" role="alert">{validationError}</div>
        <div className="form-group">
            <label htmlFor="title" className="form-label">Title</label>
            <input
                value={title}
                onChange={(e) =>  {setTitle(e.target.value); setValidationError("");}}
                id="title" type="text" className="form-control"/>
        </div>
        <div className="form-group">
            <label htmlFor="title" className="form-label">City</label>
            <input
                value={city}
                onChange={(e) =>  {setCity(e.target.value); setValidationError("");}}
                id="title" type="text" className="form-control"/>
        </div>
        <div className="form-group">
            <label htmlFor="price" className="form-label">Price:</label>
            <input
                className="form-control" type="number" id="price"
                value={price}
                onChange={(e) => {setPrice(parseFloat(e.target.value)); setValidationError("");}}/>
        </div>
        <div className="form-group">
            <label htmlFor="description" className="form-label">Description:</label>
            <textarea
                id="description" className="form-control"
                value={description}
                onChange={(e) => {setDescription(e.target.value); setValidationError("");}}/>
        </div>
        <div className="form-group">
            <label htmlFor="options" className="form-label">Select an category:</label>
            <select
                id="options"
                value={selectedCategoryId}
                onChange={(e) => {setSelectedCategoryId(e.target.value); setValidationError("");}}
                className="form-control"
            >
                <option value="" disabled>Select a category</option>
                {categories.map(category => (
                    <option key={category.id} value={category.id}>
                        {category.categoryName}
                    </option>
                ))}
            </select>
        </div>

        <div className="form-group">
            <label htmlFor="petCategories" className="form-label">Select categories:</label>
            <select
                id="prtCategories"
                multiple
                value={selectedPetCategoryIds}
                onChange={handleSelectChange}
                className="form-control">
                    
                {petCategories.map(category => (
                    <option key={category.id} value={category.id}>
                        {category.petCategoryName}
                    </option>
                ))}
            </select>
        </div>
        <br />
        <div className="form-group">
        <button onClick={(e) => onSubmit()} className="btn btn-primary">
            Create Advertisement
        </button>
        </div>

        </div>
        </div>
        </>
    );
}