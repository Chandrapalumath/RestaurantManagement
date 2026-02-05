import { MenuItemResponse } from "./billing.model";

export interface OrderItemCreateRequest {
    menuItemId: string;
    quantity: number;
}

export interface OrderCreateRequest {
    tableId: string;
    items: OrderItemCreateRequest[];
}

export interface OrderItemResponse {
    menuItemId: string;
    menuItemName: string;
    unitPrice: number;
    quantity: number;
}

export interface OrderResponse {
    orderId: string;
    tableId: string;
    waiterId: string;
    status: string;
    createdAt: string;
    items: OrderItemResponse[];
}

export interface OrderUpdateRequest {
    status: string;
}

export interface OrderMenuItem extends MenuItemResponse {
    quantity: number;
}

export interface OrderResponse {
    orderId: string;
    tableId: string;
    waiterId: string;
    status: string;
    createdAt: string;
    items: OrderItemResponse[];
}