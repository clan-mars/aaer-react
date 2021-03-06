import axios, { AxiosError, AxiosResponse } from "axios";

import { toast } from "react-toastify";
import { URLSearchParams } from "url";
import { history } from "../..";
import { Activity, ActivityFormValues } from "../models/Activity";
import { PaginatedResult } from "../models/pagination";
import { Photo, Profile } from "../models/profile";
import { User, UserFormValues } from "../models/user";
import { store } from "../stores/store";

const sleep = (delay: number) => {
    return new Promise((resolve) => {
        setTimeout(resolve, delay)
    });
}

axios.defaults.baseURL = 'http://localhost:5000/api';

axios.interceptors.request.use(config => {
    const token = store.commonStore.token;
    if (token) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response => {
    try {
        await sleep(1000 * Math.random());
        const pagination = response.headers['pagination'];
        if (pagination) {
            response.data = new PaginatedResult(response.data, JSON.parse(pagination));
            return response as AxiosResponse<PaginatedResult<any>>;
        }

        return response;
    } catch (error) {
        console.log("Sleep error: " + error);
        return await Promise.reject(error);
    }
}, (error: AxiosError) => {
    const { data, status, config } = error.response!;
    switch (status) {
        case 400:

            if (typeof data === 'string') {
                toast.error(data);
            }
            else if (config.method === 'get' && data.errors.hasOwnProperty('id')) {
                history.push('/not-found');
            }
            else if (data.errors) {
                const modalStateErrors = [];
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        modalStateErrors.push(data.errors[key])
                    }
                }
                throw modalStateErrors.flat();
            }


            break;
        case 401:
            toast.error('unauthorised');
            break;
        case 404:
            history.push('/not-found')
            break;
        case 500:
            store.commonStore.setServerError(data);
            history.push('/server-error');
            break;
    }

    return Promise.reject(error);
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    getPaginated: <T>(url: string, params: URLSearchParams) => axios.get<T>(url, {params}).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
}

const Activities = {
    list: (params: URLSearchParams) => requests.getPaginated<PaginatedResult<Activity[]>>('/activities', params),
    listForUser: (id: string) => requests.get<Activity[]>(`/users/${id}/activities`),
    details: (id: string) => requests.get<Activity>(`/activities/${id}`),
    create: (activity: ActivityFormValues) => requests.post('/activities', activity),
    update: (activity: ActivityFormValues) => requests.put(`/activities/${activity.id}`, activity),
    delete: (id: string) => requests.del<void>(`/activities/${id}`),
    attend: (id: string) => requests.post<void>(`/activities/${id}/attend`, {}),
    unattend: (id: string) => requests.del<void>(`/activities/${id}/attend`),
    updateHosted: (username: string, activity: Activity) => requests.put<void>(`/users/${username}/activities/${activity.id}`, activity)
}

const Account = {
    current: () => requests.get<User>('/account'),
    login: (user: UserFormValues) => requests.post<User>('/account/login', user),
    register: (user: UserFormValues) => requests.post<User>('/account/register', user)
}

const Profiles = {
    get: (username: string) => requests.get<Profile>(`/profiles/${username}`),
    uploadPhoto: (file: Blob) => {
        let formData = new FormData();
        formData.append('File', file);
        return axios.post<Photo>('photos', formData, {
            headers: { 'Content-type': 'multipart/form-data' }
        })
    },
    setMainPhoto: (id:string) => requests.post(`/photos/${id}/setMain`, {}),
    deletePhoto: (id:string) => requests.del(`/photos/${id}`),
    update: (profile:Profile) => requests.put(`/profiles/`, profile),
    getFollowers: (id:string) => requests.get<Profile[]>(`users/${id}/followers`),
    getFollowing: (id:string) => requests.get<Profile[]>(`users/${id}/following`),
    follow: (id:string) => requests.post<void>(`/follow/${id}`, {}),
    unfollow: (id:string) => requests.del<void>(`/follow/${id}`)
}

const agent = {
    Activities,
    Account,
    Profiles
}

export default agent;