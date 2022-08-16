export interface User {
    username: string;
    displayName: string;
    token: string;
    image?: string;
}

export interface LoginUser {
    email: string;
    password: string;
}

export interface RegisterUser {
    email: string;
    password: string;
    displayName: string;
    username: string;
}