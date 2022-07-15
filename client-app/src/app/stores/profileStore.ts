import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { Photo, Profile } from "../models/profile";
import { store } from "./store";

export default class ProfileStore {
    profile: Profile | null = null;
    loadingProfile = false;
    uploading = false;
    loading = false;
    loadingFollowings = false;
    followings: Profile[] = [];
    followers: Profile[] = [];

    constructor() {
        makeAutoObservable(this);
    }

    get isCurrentUser() {
        if (store.userStore.user && this.profile) {
            return store.userStore.user.username === this.profile.username;
        }

        return false;
    }

    loadProfile = async (username: string) => {
        this.loadingProfile = true;
        try {
            const profile = await agent.Profiles.get(username);
            runInAction(() => {
                this.profile = profile;
                this.loadingProfile = false;
            })
        } catch (error) {
            console.log("failed to get profile " + username);
            console.log(error);
            runInAction(() => this.loadingProfile = false);
        }
    }

    loadFollowings = async () => {
        this.loadingFollowings = true;
        try {
            const followings = await agent.Profiles.getFollowing(this.profile!.username);
            runInAction(() => {
                this.profile!.followings = followings;
                this.loadingFollowings = false;
            })
        } catch (error) {
            console.log("failed to get profile " + this.profile!.username);
            console.log(error);
            runInAction(() => this.loadingProfile = false);
        }
    }

    loadFollowers = async () => {
        this.loadingFollowings = true;
        try {
            const followers = await agent.Profiles.getFollowers(this.profile!.username);
            runInAction(() => {
                this.profile!.followers = followers;
                this.loadingFollowings = false;
            })
        } catch (error) {
            console.log("failed to get profile " + this.profile!.username);
            console.log(error);
            runInAction(() => this.loadingProfile = false);
        }
    }

    uploadPhoto = async (file: Blob) => {
        this.uploading = true;
        try {
            const response = await agent.Profiles.uploadPhoto(file);
            const photo = response.data;
            runInAction(() => {
                if (this.profile) {
                    this.profile.photos?.push(photo);
                    if (photo.isMain && store.userStore.user) {
                        store.userStore.setImage(photo.url);
                        this.profile.image = photo.url;
                    }
                }
            })
        } catch (error) {
            console.log(error);
            runInAction(() => this.uploading = false);
        }
    }

    setMainPhoto = async (photo: Photo) => {
        this.loading = true;
        try {
            await agent.Profiles.setMainPhoto(photo.id);
            store.userStore.setImage(photo.url);
            if (this.profile) {
                const refreshedProfile = await agent.Profiles.get(this.profile.username);
                runInAction(() => {
                    if (this.profile) {
                        this.profile = refreshedProfile;
                        this.loading = false;
                    }
                });
            }

            runInAction(() => {
                this.loading = false;
            });

        } catch (error) {
            console.log(error);
            runInAction(() => this.loading = false);
        }
    }

    deletePhoto = async (photo: Photo) => {
        this.loading = true;
        try {
            await agent.Profiles.deletePhoto(photo.id);
            if (this.profile) {
                const refreshedProfile = await agent.Profiles.get(this.profile.username);
                runInAction(() => {
                    this.profile = refreshedProfile;
                });
            }

            runInAction(() => {
                this.loading = false;
            });

        } catch (error) {
            console.log(error);
            runInAction(() => this.loading = false);
        }
    }

    updateProfileContent = async (profile: Profile) => {
        this.loading = true;
        try {
            await agent.Profiles.update(profile);
            if (this.profile) {
                const refreshedProfile = await agent.Profiles.get(this.profile.username);
                runInAction(() => {
                    this.profile = refreshedProfile;
                    this.loading = false;
                });
            } else {
                runInAction(() => {
                    this.loading = false;
                });
            }
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            });
        }
    }

    updateFollowing = async (username: string, following:boolean) => {
        this.loading = true;
        try {
            if (following) {
                await agent.Profiles.follow(username);
            } else {
                await agent.Profiles.unfollow(username);
            }
            await store.activityStore.updateAttendeeFollowing(username);

            runInAction(() => {
                if (this.profile) {
                    this.loadProfile(this.profile.username);
                }

                this.loading = false;
            });

        } catch (error) {
            console.log(error);
            runInAction(() => this.loading = false);
        }
    }

    loadActivitiesForUser = async (username: string) => {
        try {
            this.loading = true;
            const response = await agent.Activities.listForUser(username);
            runInAction(() => {
                if (this.profile) {
                    this.profile.activities = response;
                }

                this.loading = false;
            });

        } catch (error) {
            console.log(error);
            runInAction(() => this.loading = false);
        }
    }
}