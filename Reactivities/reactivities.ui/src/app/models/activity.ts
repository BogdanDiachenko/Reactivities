import { Profile } from "./profile";

export interface Activity {
    title: string;
    category: string;
    description: string;
    city: string;
    venue: string;
    date: Date;
    id: string;
    hostUsername: string;
    isCancelled: boolean;
    attendees: Profile[];
    isGoing: boolean;
    isHost: boolean;
    host?: Profile;
}

export class Activity implements Activity {
    constructor(init?: ActivityFormValues){
        Object.assign(this, init);
    }
}

export class ActivityFormValues {
   id?: string = undefined;
   title: string = '';
   category: string = '';
   description: string = '';
   date: Date | null = null;
   venue: string = '';
   city: string = '';

   constructor(activity?: ActivityFormValues){
        if(activity){
            this.id = activity.id;
            this.title = activity.title;
            this.category = activity.category;
            this.description = activity.description;
            this.date = activity.date;
            this.venue = activity.venue;
            this.city = activity.city;
        }
   }
}