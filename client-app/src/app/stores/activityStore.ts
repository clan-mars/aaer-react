import { format } from "date-fns";
import { makeAutoObservable, runInAction } from "mobx"
import agent from "../api/agent";
import { Activity, ActivityFormValues } from "../models/Activity";
import { Profile } from "../models/profile";
import { store } from "./store";

interface ActivityState {
    loading: boolean;
    loadingInitial: boolean;
    editMode: boolean;
    selectedActivity: Activity | undefined;
}

export default class ActivityStore {
    activityRegistry = new Map<string, Activity>();
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;
    currentState: ActivityState = {
        editMode : false,
        loading : false,
        loadingInitial : false,
        selectedActivity : undefined
    };

    constructor() {
        makeAutoObservable(this)
    }

    get activitiesByDate() {
        return Array.from(this.activityRegistry.values()).sort(
            (a, b) => a.date!.getTime() - b.date!.getTime()
        );
    }

    get groupedActivities() {
        return Object.entries(
            this.activitiesByDate.reduce((activities, activity) => {
                const date = format(activity.date!, 'dd MMM yyyy');
                activities[date] = activities[date] ? [...activities[date], activity] : [activity];
                return activities;
            }, {} as { [key: string]: Activity[] })
        )
    }

    loadActivities = async () => {
        try {
            this.setLoadingInitial(true);
            const response = await agent.Activities.list();

            response.forEach(activity => {
                this.addActivity(activity);
            });
            this.setLoadingInitial(false)


        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false)
        }
    }

    loadActivitiesForUser = async (username: string) => {
        try {
            this.setLoadingInitial(true);
            const response = await agent.Activities.listForUser(username);

            response.forEach(activity => {
                activity.date = new Date(activity.date!);
                this.addActivity(activity);
            });
            this.setLoadingInitial(false)


        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false)
        }
    }

    private addActivity = (activity: Activity) => {
        activity.date = new Date(activity.date!);
        this.activityRegistry.set(activity.id, activity);
    }

    private getActivity = (id: string) => {
        return this.activityRegistry.get(id);
    }

    loadActivity = async (id: string) => {
        let activity = this.getActivity(id);
        if (activity) {
            runInAction(() => {
                this.selectedActivity = activity;
                this.currentState.selectedActivity = activity;
            });

            this.setLoadingInitial(false);
            return activity;
        }

        this.setLoadingInitial(true);
        try {
            activity = await agent.Activities.details(id);
            this.addActivity(activity);
            runInAction(() => {
                this.selectedActivity = activity;
                this.currentState.selectedActivity = activity;
            });
            this.setLoadingInitial(false);
            return activity;
        } catch (error) {
            console.log(error);
            this.setLoadingInitial(false);
        }
    }

    attendActivity = async () => {
        return await this.doUpdate(agent.Activities.attend);
    }

    unattendActivity = async () => {
        return await this.doUpdate(agent.Activities.unattend);
    }

    cancelActivity = async () => {
        return await this.updateCancelled(true);
    }

    uncancelActivity = async () => {
        return await this.updateCancelled(false);
    }

    updateCancelled = async (isCancelled: boolean) => {
        if (this.selectedActivity!.isCancelled === isCancelled) return;
        if (this.currentState.selectedActivity!.isCancelled === isCancelled) return;

        this.loading = true;
        try {
            const user = store.userStore.user;
            this.selectedActivity!.isCancelled = isCancelled;
            this.currentState.selectedActivity!.isCancelled = isCancelled;
            await agent.Activities.updateHosted(user!.username, this.selectedActivity!);
            var updatedActivity = await agent.Activities.details(this.selectedActivity!.id);
            updatedActivity.date = new Date(updatedActivity.date!);
            runInAction(() => {
                this.activityRegistry.set(updatedActivity.id, updatedActivity);
                this.selectedActivity = updatedActivity;
                this.currentState.selectedActivity = updatedActivity;
            })

        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => this.loading = false);
        }
    }

    doUpdate = async (callback: any) => {
        this.loading = true;
        try {
            await callback(this.selectedActivity!.id);
            var updatedActivity = await agent.Activities.details(this.selectedActivity!.id);
            updatedActivity.date = new Date(updatedActivity.date!);
            runInAction(() => {
                this.activityRegistry.set(updatedActivity.id, updatedActivity);
                this.selectedActivity = updatedActivity;
                this.currentState.selectedActivity = updatedActivity;
                this.currentState.loading = false;
                this.loading = false;
            })

        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => this.loading = false);
        }
    }

    updateAttendance = async () => {
        const user = store.userStore.user;
        this.loading = true;
        try {
            await agent.Activities.attend(this.selectedActivity!.id);
            runInAction(() => {
                let activity = this.currentState.selectedActivity;
                if (activity?.isGoing) {
                    activity.attendees = activity.attendees?.filter(a => a.username !== user?.username);
                    activity.isGoing = false;
                } else {
                    const attendee = new Profile(user!);
                    activity?.attendees?.push(attendee);
                    activity!.isGoing = true;
                }

                this.activityRegistry.set(activity!.id, activity!);
            })

        } catch (error) {
            console.log(error);
        } finally {
            runInAction(() => {
                this.loading = false;
                this.currentState.loading = false;
            });
        }
    }

    setLoadingInitial = (state: boolean) => {
        this.currentState.loadingInitial = state;
        this.loadingInitial = state;
    }

    createActivity = async (activity: ActivityFormValues) => {
        const user = store.userStore.user;
        const attendee = new Profile(user!);

        try {
            this.loading = true;
            await agent.Activities.create(activity);
            const newActivity = new Activity(activity);
            newActivity.hostUsername = user!.username;
            newActivity.attendees = [attendee];
            this.addActivity(newActivity);

            runInAction(() => {
                this.selectedActivity = newActivity;
                this.loading = false;
                this.currentState.selectedActivity = newActivity;
                this.currentState.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
                this.currentState.loading = false;
            })
        }
    }

    updateActivity = async (activity: Activity) => {
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                if (activity.id) {
                    let updatedActivity = { ...this.getActivity(activity.id), ...activity };
                    this.activityRegistry.set(activity.id, updatedActivity);
                    this.selectedActivity = updatedActivity;
                }
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
                this.currentState.loading = false;
            })
        }
    }

    deleteActivity = async (id: string) => {
        try {
            this.loading = true;
            await agent.Activities.delete(id);
            runInAction(() => {
                this.activityRegistry.delete(id);
                this.loading = false;
                this.currentState.loading = false;
            })
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
                this.currentState.loading = false;
            })
        }
    }

    clearSelectedActivity = () => {
        this.selectedActivity = undefined;
    }

}

