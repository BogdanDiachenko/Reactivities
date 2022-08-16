import { makeAutoObservable, reaction, runInAction } from "mobx";
import { agent } from "../api";
import { Photo, Profile, UserActivity } from "../models/profile";
import { store } from "./store";

export default class ProfileStore{
    profile: Profile | null = null;
    loadingProfile = false;
    uploading = false;
    loading = false;
    followings: Profile[] = [];
    loadingFollowings = false;
    activeTab = 0;
    userActivities: UserActivity[] = [];
    loadingActivities = false;
    
    constructor(){
        makeAutoObservable(this);

        reaction(
            () => this.activeTab,
            activeTab => {
                if(activeTab === 3 || activeTab === 4){
                    const requestingFollowers = activeTab === 3 ? true : false;
                    this.loadFollowings(requestingFollowers);
                } else {
                    this.followings = [];
                }
            }
    )
    }

    setActiveTab = (activeTab: any) => {
        this.activeTab = activeTab;
    }

    get isCurrentUser(){
        if(store.userStore.user && this.profile){
            return store.userStore.user.username === this.profile.userName;
        }
    }

    loadProfile = async (username: string) => {
        this.loadingProfile = true;
        try{
            const profile = await agent.Profiles.get(username);
            runInAction(() => this.profile = profile)
        } catch(error) {
            console.log(error);
        } finally {
            runInAction(() => this.loadingProfile = false);
        }
    } 

    uploadPhoto = async (file: Blob) => {
        this.uploading = true;
        try {
            const response = await agent.Profiles.uploadPhoto(file);
            const photo = response.data;
            runInAction(() => {
                if(this.profile){
                    this.profile.photos.push(photo);
                    if(photo.isMain && store.userStore.user){
                        store.userStore.setImage(photo.url);
                        this.profile.image = photo.url;
                    }
                }
            });
        } catch(error) {
            console.log(error);
        } finally {
            runInAction(() => this.uploading = false);
        }
    }

    setMainPhoto = async (photo: Photo) => {
        this.loading = true;
        try{ 
            await agent.Profiles.setMainPhoto(photo.id);
            store.userStore.setImage(photo.url);
            runInAction(() => {
                if(this.profile && this.profile.photos){
                    this.profile.photos.find(p => p.isMain)!.isMain = false;
                    this.profile.photos.find(p => p.id === photo.id)!.isMain = true;
                    this.profile.image = photo.url;
                }
            })
        } catch (error){
            console.log(error);
        } finally {
            runInAction(() => this.loading = false)
        }
    }

    deletePhoto = async (photo: Photo) => {
        this.loading = true;
        try{
            await agent.Profiles.deletePhoto(photo.id);
            runInAction(() => {
                if(this.profile && this.profile.photos){
                    this.profile.photos = this.profile.photos.filter( ph => ph.id !== photo.id);
                }
            })
        } catch(error){
            console.log(error);
        } finally {
            runInAction(() => this.loading = false);
        }
    }

    updateFollowing = async(username: string, following: boolean) => {
        this.loading = true;
        try{
            await agent.Profiles.updateFollowing(username);
            store.activityStore.updateAttendeeFollowing(username);
            runInAction(() => {
                if(this.profile && this.profile.userName !== store.userStore.user!.username && this.profile.userName === username){
                    following ? this.profile.followersCount ++ : this.profile.followersCount--;
                    this.profile.following = !this.profile.following;
                }
                if(this.profile && this.profile!.userName === store.userStore.user!.username){
                    following ? this.profile.followingCount++ : this.profile.followingCount--; 
                }
                this.followings.forEach(profile => {
                    if(profile.userName === username){
                        profile.following ? profile.followersCount-- : profile.followersCount++;
                        profile.following = !profile.following;
                    }
                })
            })
        } catch(error){
            console.log(error);
        } finally {
            runInAction(() => this.loading = false)
        }
    }

    loadFollowings =  async(requestingFollowers: boolean) => {
        this.loadingFollowings = true;
        try {
            const followings = await agent.Profiles.followings(this.profile!.userName, requestingFollowers);
            runInAction(() => this.followings = followings);
        } catch (error){
            console.log(error);
        } finally {
            runInAction(() => this.loadingFollowings = false);
        }
    }

    loadUserActivities = async(username: string, predicate?: string) => {
        this.loadingActivities = true;
        try{
            var activities = await agent.Profiles.activities(username, predicate!);
            runInAction(() => this.userActivities = activities);
        } catch(error) {
            console.log(error);
        } finally {
            runInAction(() => this.loadingActivities = false);
        }
    }
}