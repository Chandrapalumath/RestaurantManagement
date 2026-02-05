export interface ReviewResponse {
    reviewId: string;
    customerId: string;
    rating: number;
    comment?: string;
    createdAt: string;
    customerName?: string;
}

export interface ReviewCreateRequest {
    customerId: string | null;
    rating: number;
    comment?: string;
}

export interface CustomerResponse {
    id: string;
    name: string;
    mobileNumber: string;
}