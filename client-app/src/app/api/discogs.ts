import axios, { AxiosResponse } from "axios";
import { Pagination } from "../models/Pagination";

import { Release } from "../models/Release";

const baseURL = 'https://api.discogs.com/database/search?';
axios.defaults.headers['Authorization'] = "Discogs token=mXkQzOGFjFqeXHcpRKqmUDznVJdKLUPQvPVyIdQm";

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody)
}

const Queries = {
    Albums: (artist:string) => requests.get<Pagination>(`${baseURL}artist=${artist}&type=master&format=album`)
}

const discogs = { Queries}
export default discogs;