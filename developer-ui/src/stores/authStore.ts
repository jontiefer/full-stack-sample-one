import { makeAutoObservable, reaction, runInAction } from "mobx";
import { User, UserFormValues } from "../models/user";
import apiClient from "../api/apiClient";
import { appRouter } from "../router/AppRoutes";

interface LoginAttempt {
    ip: string;
    timestamp: Date;
}
  
export default class AuthStore {
    user: User | null = null;    
    token: string | null = localStorage.getItem('jwt');
    loginAttempts: LoginAttempt[] = [];

    
    constructor() {
        makeAutoObservable(this);       
        
        reaction(
            () => this.token,
            token => {
                if (token) {
                    localStorage.setItem('jwt', token);
                }
                else {
                    localStorage.removeItem('jwt');
                }
            }
        )

        this.loadLoginAttempts();
    }

    loadLoginAttempts() {
        const attempts = localStorage.getItem('loginAttempts');

        if (attempts) {
          const parsedAttempts = JSON.parse(attempts);

          this.loginAttempts = parsedAttempts.map((attempt: {ip: string, timestamp: string }) => ({                        
            ...attempt,
            timestamp: new Date(attempt.timestamp)
          }));          
        }
      }

    saveLoginAttempts(ip: string, timestamp: Date) {
        this.loginAttempts.push({ ip, timestamp });

        localStorage.setItem('loginAttempts', JSON.stringify(this.loginAttempts));
    }

    clearLoginAttempts() {
        localStorage.removeItem('loginAttempts');

        this.loginAttempts = [];
    }

    get isLoggedIn() {
         return !!this.user;
    }

    setToken = (token: string | null) => {
        this.token = token;
    }
    
    login = async (credentials: UserFormValues) => {
        try {
            const response = await apiClient.Auth.login(credentials);
            
            runInAction(() => {
                this.setToken(response.token);
                this.user = { username: credentials.username, token: response.token };
                
                this.clearLoginAttempts();
            });
            
            appRouter.navigate('/dashboard');
        } catch (error) {
            const ip = await apiClient.getLocalIpAddress();

            runInAction(() => {
                this.saveLoginAttempts(ip, new Date());                
            });

            console.log(`Error logging into Dashboard.  Message: ${error}`);            
            throw error;
        }
    }

    register = async (credentials: UserFormValues) => {
        try {
            const response = await apiClient.Auth.register(credentials);
            this.setToken(response.token);
            runInAction(() => this.user = { username: credentials.username, token: response.token });                              

            appRouter.navigate('/dashboard');
        }
        catch (error) {
            console.log(`Error register user.  Message: ${error}`);            
            throw error;
        }
    }    

    logout = () => {
        this.setToken(null);
        this.user = null;
        appRouter.navigate('/');
    }
}