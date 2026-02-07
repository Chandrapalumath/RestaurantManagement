export interface TableResponse {
    id: string;
    tableName: string;
    size: number;
    isOccupied: boolean;
}

export interface TableCreateRequest {

    tableName: string;
    size: number;
}