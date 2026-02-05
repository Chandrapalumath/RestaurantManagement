export interface MenuItemCreateRequest {
    name: string;
    price: number;
    isAvailable: boolean;
}

export interface MenuItemResponse {
    id: string;
    name: string;
    price: number;
    isAvailable: boolean;
    rating: number;
    customerId?: string;
}

export interface MenuItemUpdateRequest {
    name?: string;
    price?: number;
    isAvailable?: boolean;
}

export interface UpdateMenuItemRating {
    id: string;
    rating: number;
}