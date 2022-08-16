import { User, LoginUser, RegisterUser } from '../models'
import { makeAutoObservable, runInAction } from 'mobx'
import { history } from '../..';
import { agent } from '../api/agent'
import { store } from './store';
export class UserStore {
    user: User | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn() {
        return !!this.user
    }

    login = async (creds: LoginUser) => {
        try {
            const user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            history.push('/activities');
            store.modalStore.closeModal();
        } catch (error) {
            console.log(error);
            throw error
        }
    }

    register = async (creds: RegisterUser) => {
        try {
            const user = await agent.Account.register(creds);
            store.commonStore.setToken(user.token);
            runInAction(() => this.user = user);
            history.push('/activities');
            store.modalStore.closeModal();
        } catch (error) {
            throw error
        }
    }

    logout = () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem('jwt')
        this.user = null;
        history.push('/')
    }

    getUser = async () => {
        try {
            const user = await agent.Account.currentUser();
            runInAction(() => this.user = user);
        } catch(error) {
            console.log(error);
        }
    }

    setImage = (image: string) => {
        if(this.user){
            this.user.image = image;
        }       
    }
}