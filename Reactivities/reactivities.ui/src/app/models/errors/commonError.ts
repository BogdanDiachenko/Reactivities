export interface CommonError {
    errors: object[];
    statusCode: number;
    message: string;
    stackTrace: string;
}