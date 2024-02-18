import axios, { AxiosError, AxiosResponse } from "axios";
import { appStore } from "../stores/appStore";
import { appRouter } from "../router/AppRoutes";
import { UserFormValues } from "../models/user";
import { TestData } from "../models/testData";

const delay = (ms: number): Promise<void> =>
    new Promise(resolve => setTimeout(resolve, ms));

axios.defaults.baseURL = import.meta.env.VITE_API_URL;

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

axios.interceptors.request.use(config => {
    const token = appStore.authStore.token;
  
    if (token && config.headers)
        config.headers.Authorization = `Bearer ${token}`;

    return config;
})

axios.interceptors.response.use(async response => {
    if (process.env.NODE_ENV === 'development') await delay(1000);

    return response;
}, (error: AxiosError) => {
    console.error(`AxiosError: ${error}`);

    if(!error?.response)
    {
        appRouter.navigate('/server-error');
    }

    const { data, status } = error?.response as AxiosResponse;
    
    const currentPath = window.location.pathname;

    if(currentPath == '/login' && (status == 401 || status == 403)) {
        return Promise.reject(error);
    }

    switch(status) {
        case 400:
            appStore.generalStore.setServerError(data);
            appRouter.navigate('/bad-request')
            break;
        case 401:
            appRouter.navigate('/unauthorized');
            break;
        case 403:
            appRouter.navigate('/forbidden');
            break;
        case 404:
            appRouter.navigate('/not-found');
            break;
        case 422:
            appRouter.navigate('/unprocessable-request');
            break;
        case 500:
            appRouter.navigate('/server-error');
            break;
    }

    return Promise.reject(error);
});

const Http = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: object) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: object) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody)
}

const Auth = {        
    login: (user: UserFormValues) => Http.post<{token: string}>('/auth/login', user),
    register: (user: UserFormValues) => Http.post<{token: string}>('/auth/register', user)
}

const Test = {
    testData: () => Http.get<TestData[]>('/test/data')
}

const getLocalIpAddress = async () => {
    try {
        const response = await axios.get('https://api.ipify.org?format=json');

        return response.data.ip;
    } catch (error) {
        console.error('Error retrieving local IP Address', error);
        return null;
    }
}

const apiClient = {
    Auth,
    Test,
    getLocalIpAddress
}

export default apiClient;