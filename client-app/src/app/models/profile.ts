import { Activity } from "./Activity";
import { User } from "./user";

export interface Profile {
    username: string;
    displayName: string;
    image?: string;
    bio?: string;
    photos?: Photo[];
    activities?: Activity[];
    followers: Profile[];
    followings: Profile[];
}

export class Profile implements Profile {
    
    constructor(user: User) {
        this.username = user.username;
        this.displayName = user.displayName;
        this.image = user.image;
        this.activities = [];
        this.followers = [];
        this.followings = [];
    }
}

export interface Photo {
    id: string;
    url: string;
    isMain: boolean;
}