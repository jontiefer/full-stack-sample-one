import { ServerError } from "../models/serverInfo";

export default class GeneralStore {
    error: ServerError | null = null;    

    setServerError(error: ServerError) {
        this.error = error;
    }
}