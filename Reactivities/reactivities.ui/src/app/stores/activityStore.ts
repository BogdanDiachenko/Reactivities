import { Activity, ActivityFormValues } from "../models";
import { agent } from "../../app/api";
import { makeAutoObservable, reaction, runInAction } from "mobx";
import { format } from 'date-fns'
import { store } from "./store";
import { Profile } from "../models/profile";
import { Pagination, PagingParams } from "../models/pagination";

export class ActivityStore {
    activities = new Map<string, Activity>();
    selectedActivity: Activity | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;
    pagination: Pagination | null = null;
    pagingParams = new PagingParams();
    predicate = new Map().set('all', true);

    constructor() {
        makeAutoObservable(this);

        reaction(
            () => this.predicate.keys(),
            () => {
                this.pagingParams = new PagingParams();
                this.activities.clear();
                this.loadActivities();
            }
        )
    }

    get activitiesByDate() {
        return Array.from(this.activities.values()).sort((a, b) =>
            a.date.getTime() - b.date.getTime());
    }

    get groupedActivities() {
        return Object.entries(
            this.activitiesByDate.reduce((activities, activity) => {
                const date = format(activity.date, 'dd/MMM/yyyy');
                activities[date] = activities[date] ? [...activities[date], activity] : [activity];
                return activities;
            }, {} as { [key: string]: Activity[] })
        )
    }

    get axiosParams(){
        const params = new URLSearchParams();
        params.append('pageNumber', this.pagingParams.pageNumber.toString());
        params.append('pageSize', this.pagingParams.pageSize.toString());
        this.predicate.forEach((value, key) => {
            if(key === "startDate"){
                params.append(key, (value as Date).toISOString());
            } else {
                params.append(key, value);
            }
        })
        
        return params;
    }

    setPredicate = (predicate: string, value: string | Date) => {
        const resetPredicate = () => {
            this.predicate.forEach((value, key) => {
                if(key != 'startDate') this.predicate.delete(key);
            })
        }
        switch(predicate){
            case 'all':
                resetPredicate();
                this.predicate.set('all', true);
                break;
            case 'isGoing':
                resetPredicate();
                this.predicate.set('isGoing', true);
                break;
            case 'isHost':
                resetPredicate();
                this.predicate.set('isHost', true);
                break;
            case 'startDate':
                this.predicate.delete('startDate');
                this.predicate.set('startDate', value);
                break;
        }
    }

    setPagingParams = (params: PagingParams) => this.pagingParams = params; 

    setPagination = (pagination: Pagination) => this.pagination = pagination;

    setLoadingInitial = (state: boolean) => {
        this.loadingInitial = state;
    }

    setLoading = (state: boolean) => {
        this.loading = state;
    }

    loadActivities = async () => {
        this.setLoadingInitial(true);
        try {
            const result = await agent.Activities.list(this.axiosParams);

            result.data.forEach(activity => {
                this.setActivity(activity);
            });
            
            this.setPagination(result.pagination);
        } catch (error) {
            console.log(error);
        } finally {
            this.setLoadingInitial(false);
        }
    }

    loadActivity = async (id: string) => {
        let activity = this.activities.get(id);
        if (activity) {
            this.selectedActivity = activity;
            return activity;
        } else {
            this.setLoadingInitial(true);
            try {
                activity = await agent.Activities.details(id);
                this.setActivity(activity);
                runInAction(() => {
                    this.selectedActivity = activity;
                });
                return activity;
            } catch (error) {
                console.log(error);
            } finally {
                this.setLoadingInitial(false);
            }
        }
            
    }

    createActivity = async (activity: ActivityFormValues) => {
        const user = store.userStore.user;
        const attendee = new Profile(user!);

        try {
            await agent.Activities.create(activity);
            const newActivity = new Activity(activity);
            newActivity.hostUsername = user!.username;
            newActivity.attendees = [attendee];
            this.setActivity(newActivity);

            runInAction(() => {
                this.selectedActivity = newActivity;
            })
        } catch (error) {
            console.log(error);
        }
    }

    updateActivity = async (activity: ActivityFormValues) => {
        try {
            await agent.Activities.update(activity);
            runInAction(() => {
                if(activity.id){
                    let updatedActivity = {...this.activities.get(activity.id), ...activity}
                    this.activities.set(activity.id, updatedActivity as Activity);
                    this.selectedActivity = updatedActivity as Activity;
                }
            })
        } catch (error) {
            console.log(error);
        }
    }

    deleteActivity = async (id: string) => {
        this.setLoading(true);
        try {
            await agent.Activities.delete(id);
            runInAction(() => {
                this.activities.delete(id);
            })
        } catch (error) {
            console.log(error);
        } finally {
            this.setLoading(false);
        }
    }

    private setActivity = (activity: Activity) => {
        const user = store.userStore.user;

        if(user){
            activity.isGoing = activity.attendees!.some(
                a => a.userName === user.username
            );
            activity.isHost = activity.hostUsername === user.username;
            activity.host = activity.attendees!.find(
                x => x.userName === activity.hostUsername
            );
        }
        
        activity.date = new Date(activity.date)
        this.activities.set(activity.id, activity);
    }

    updateAttendance = async () => {
        const user = store.userStore.user;
        this.loading = true;

        try {
            await agent.Activities.attend(this.selectedActivity!.id);
            runInAction(() => {
                if(this.selectedActivity!.isGoing) {
                    this.selectedActivity!.attendees = this.selectedActivity!.attendees!.filter(u => u.userName !== user!.username);
                    this.selectedActivity!.isGoing = false;
                } else {
                    const attendee = new Profile(user!);
                    this.selectedActivity!.attendees!.push(attendee);
                    this.selectedActivity!.isGoing = true;
                }
                this.activities.set(this.selectedActivity!.id, this.selectedActivity!)
            })
        } catch (error){
            console.log(error);
        } finally {
            console.log(this.selectedActivity);
            runInAction(() => this.loading = false)
        }
    }

    cancelActivity = async () => {
        this.loading = true;
        try {   
            await agent.Activities.attend(this.selectedActivity!.id);
            runInAction(() => {
                this.selectedActivity!.isCancelled = !this.selectedActivity!.isCancelled;
                this.activities.set(this.selectedActivity!.id, this.selectedActivity!);
            })
        } catch(error) {
            console.log(error);
        } finally {
            this.loading = false;
        }
    }

    clearSelectedActivity = () => this.selectedActivity = undefined;

    updateAttendeeFollowing = (username: string) => {
        this.activities.forEach((activity) => {
            activity.attendees.forEach((attendee) => {
                if(attendee.userName === username){
                    attendee.following ? attendee.followersCount-- : attendee.followersCount++;
                    attendee.following = !attendee.following;
                }
            })
        })
    }
}