export interface BillGenerateRequest {
    customerId: string;
    ordersId: string[];
}

export interface BillResponse {
    billId: string;
    customerId: string;
    waiterId: string;
    subTotal: number;
    discountPercent: number;
    discountAmount: number;
    taxPercent: number;
    taxAmount: number;
    grandTotal: number;
    isPaymentDone: boolean;
    generatedAt: string;
}

export interface MenuItemResponse {
    id: string;
    name: string;
    price: number;
    isAvailable: boolean;
    rating: number;
    customerId?: string;
}