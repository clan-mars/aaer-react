import { ChatComment } from "../models/comment";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr"
import { makeAutoObservable, runInAction } from "mobx";
import { store } from "./store";

export default class CommentStore {
    comments: ChatComment[] = [];
    hubConnection: HubConnection | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    isConnected = () => { return this.hubConnection != null }

    createHubConnection = (activityId: string) => {
        if (store.activityStore.selectedActivity) {
            this.hubConnection = new HubConnectionBuilder().withUrl('http://localhost:5000/chat?activityId=' + activityId, {
                accessTokenFactory: () => store.userStore.user?.token!
            }).withAutomaticReconnect()
                .configureLogging(LogLevel.Debug)
                .build();

            this.hubConnection.start().catch(error => console.log('Connection fail: ' + error));

            this.hubConnection.on('LoadComments', (comments: ChatComment[]) => {
                runInAction(() => {
                    comments.forEach(comment => {
                        comment.createdAt = new Date(comment.createdAt+'Z');
                    })
                    this.comments = comments;
                });
            });

            this.hubConnection.on('ReceiveComment', (comment: ChatComment) => {
                runInAction(() => {
                    comment.createdAt = new Date(comment.createdAt);
                    this.comments.unshift(comment);
                });
            });
        }
    }

    stopHubConnection = () => {
        this.hubConnection?.stop().catch(error => console.log('Failed to stop hub: ' + error));
    }

    clearComments = () => {
        this.comments = [];
        this.stopHubConnection();
    }

    addComment = async (values: any) => {
        values.activityId = store.activityStore.selectedActivity?.id;
        try {
            this.hubConnection?.invoke("SendComment", values);
        } catch (error) {
            console.log("Add comment error: ")
            console.log(error)
        }
    }
}