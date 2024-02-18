// testDataStore.ts
import { makeAutoObservable } from "mobx";
import apiClient from "../api/apiClient";
import { TestData } from "../models/testData";

class TestDataStore {
    testData: TestData[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    loadTestData = async () => {
        try {
            const data = await apiClient.Test.testData();
            this.testData = data;
        } catch (error) {
            console.error("Failed to load test data", error);
        }
    }
}

export default TestDataStore;
