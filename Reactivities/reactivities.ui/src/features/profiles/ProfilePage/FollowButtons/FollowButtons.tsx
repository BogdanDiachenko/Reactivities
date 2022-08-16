import React, { SyntheticEvent } from 'react';
import { Button, Reveal } from 'semantic-ui-react';
import { Profile } from '../../../../app/models/profile';
import { useStore } from '../../../../app/stores';

interface Props{
    profile: Profile;
}

export function FollowButtons({profile}: Props){
    const {profileStore, userStore} = useStore();
    const {updateFollowing, loading} = profileStore;

    if(userStore.user!.username === profile.userName) return null;

    function handleFollow(e: SyntheticEvent, username: string){
        e.preventDefault();
        profile.following ? updateFollowing(username, false) : updateFollowing(username, true) 
    }

    return (
        <Reveal animated='move'>
            <Reveal.Content visible style={{width: '100%'}}>
                <Button 
                    fluid  
                    color='teal' 
                    content={profile.following ? 'Following' : 'Not Following'}/>
            </Reveal.Content>
            <Reveal.Content hidden style={{width: '100%'}}>
                <Button 
                    fluid 
                    color={profile.following ? 'red' : 'green'} 
                    basic 
                    content={profile.following ? 'Unfollow' : 'Follow'}
                    loading={loading}
                    onClick={(e) => handleFollow(e, profile.userName)}
                    />
            </Reveal.Content>
        </Reveal>
    )
}