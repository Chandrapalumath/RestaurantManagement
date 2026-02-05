export interface LoginResponse {
    token: string;
    fullName: string;
    role: string;
}

export interface UserCredentials {
    email?: string | null;
    password?: string | null;
}