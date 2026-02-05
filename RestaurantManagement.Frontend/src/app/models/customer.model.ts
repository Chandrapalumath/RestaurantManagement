export interface CustomerResponse {
    id: string;
    name: string;
    mobileNumber: string;
}

export interface CustomerCreateRequest {
    name: string;
    mobileNumber: string;
}