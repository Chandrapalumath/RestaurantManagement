export interface SettingsResponse {
    taxPercent: number;
    discountPercent: number;
    updatedAt: string;
}

export interface SettingsUpdateRequest {
    taxPercent?: number;
    discountPercent?: number;
}