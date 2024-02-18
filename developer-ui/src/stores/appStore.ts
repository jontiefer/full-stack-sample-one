import { createContext, useContext } from "react";
import AuthStore from "./authStore";
import GeneralStore from "./generalStore";
import TestDataStore from "./testDataStore";

interface AppStore {
    generalStore: GeneralStore;
    authStore: AuthStore;
    testDataStore: TestDataStore;       
}

export const appStore: AppStore = {
    generalStore: new GeneralStore(),
    authStore: new AuthStore(),
    testDataStore: new TestDataStore()
}

export const AppStoreContext = createContext(appStore);

export function useAppStore() {
    return useContext(AppStoreContext);
}