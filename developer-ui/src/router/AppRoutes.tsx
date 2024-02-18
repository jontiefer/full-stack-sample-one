import { createBrowserRouter, Navigate, RouteObject } from "react-router-dom";
import AuthRequired from "./AuthRequired";
import RequestError from "../forms/errors/RequestError";
import NotFound from "../forms/errors/NotFound";
import AuthFailure from "../forms/errors/AuthFailure";
import App from "../layout/App";
import Dashboard from "../forms/dashboard/Dashboard";
import LoginForm from "../forms/auth/LoginForm";
import RegisterForm from "../forms/auth/RegisterForm";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            {element: <AuthRequired />, children: [
                {path: 'dashboard', element: <Dashboard />},                
            ]},
            
            {path: 'login', element: <LoginForm />},
            {path: 'register', element: <RegisterForm />},
            {path: 'not-found', element: <NotFound />},
            {path: 'server-error', element: <RequestError statusCode={500} />},
            {path: 'bad-request', element: <RequestError statusCode={400} />},
            {path: 'unprocessable-request', element: <RequestError statusCode={422} />},
            {path: 'unauthorized', element: <AuthFailure statusCode={401} />},
            {path: 'forbidden', element: <AuthFailure statusCode={403} />},
            {path: '*', element: <Navigate replace to='/not-found' />},
        ]
    }
]

export const appRouter = createBrowserRouter(routes);