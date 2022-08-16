import { Activity, CommonError, User, LoginUser, RegisterUser, ActivityFormValues } from '../models';
import axios, { AxiosError, AxiosResponse } from 'axios';
import { history } from '../..';
import { toast } from 'react-toastify';
import { store } from '../../app/stores';
import { Photo, Profile, UserActivity } from '../models/profile';
import { PaginatedResult } from '../models/pagination';
import { request } from 'http';

function sleep(delay: number) {
    return new Promise((resolve) => {
        setTimeout(resolve, delay);
    })
}

axios.defaults.baseURL = process.env.REACT_APP_API_URL;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.request.use(config => {
    if (config.headers === undefined) config.headers = {};
    const token = store.commonStore.token;
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response => {
    if(process.env.NODE_ENV === 'development')  await sleep(1000);

    const pagination = response.headers['pagination'];

    if(pagination){
        response.data = new PaginatedResult(response.data, JSON.parse(pagination));
        return response as AxiosResponse<PaginatedResult<any>>;
    }
    
    return response;
},
    (error: AxiosError<CommonError>) => {
        const { data, status } = error.response!;
        switch (status) {
            case 400:
                if (data.errors) {
                    const modelStateErrors = [];
                    for (const i in data.errors) {
                        if (data.errors[i]) {
                            modelStateErrors.push(data.errors[i]);
                        }
                    }
                    throw modelStateErrors.flat();
                }
                toast.error(data);
                break;
            case 401:
                toast.error("Unauthorized");
                break;
            case 404:
                history.push("/not-found")
                break;
            case 500:
                store.commonStore.setServerError(data);
                history.push('/server-error');
                break;
        }

        return Promise.reject(error);
    })

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Activities = {
    list: (params: URLSearchParams) => axios.get<PaginatedResult<Activity[]>>('/activities', {params}).then(responseBody),
    details: (id: string) => requests.get<Activity>(`/activities/${id}`),
    create: (activity: ActivityFormValues) => requests.post<void>('/activities', activity),
    update: (activity: ActivityFormValues) => requests.put<void>(`/activities/${activity.id}`, activity),
    delete: (id: string) => requests.delete<void>(`/activities/${id}`),
    attend: (id: string) => requests.post<void>(`/activities/${id}/attend`, {})
}

const Account = {
    currentUser: () => requests.get<User>('/account'),
    login: (user: LoginUser) => requests.post<User>('/account/login', user),
    register: (user: RegisterUser) => requests.post<User>('/account/register', user)
}

const Profiles = {
    get: (username: string) => requests.get<Profile>(`/profiles/${username}`),
    uploadPhoto: (file: Blob) => {
        let formData = new FormData();
        formData.append('File', file);
        return axios.post<Photo>('photos', formData, {
            headers: {'Content-type': 'multipart/form-data'}
        })
    },
    setMainPhoto: (id: string) => requests.post(`/photos/${id}/setMain`, {}),
    deletePhoto: (id: string) => requests.delete(`/photos/${id}`),
    updateFollowing: (username: string) => requests.post(`/follow/${username}`, {}),
    followings: (username: string, requestingFollowers: boolean) => 
        requests.get<Profile[]>(`/follow/${username}?requestingFollowers=${requestingFollowers}`),
    activities: (username: string, predicate: string) => 
        requests.get<UserActivity[]>(`/profiles/${username}/activities?predicate=${predicate}`),
}

export const agent = {
    Activities,
    Account,
    Profiles,
}