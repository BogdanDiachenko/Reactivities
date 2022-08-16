import React from 'react';
import { Link } from "react-router-dom";
import { Card, Icon, Image } from "semantic-ui-react";
import { Profile } from "../../../app/models/profile";
import FollowButtons from '../ProfilePage/FollowButtons';

interface Props{
    profile: Profile
}

export function ProfileCard({profile}: Props){
    return (
        <Card as={Link} to={`profiles/${profile.userName}`}>
            <Image src={profile.image || '/assets/user.png'}/>
            <Card.Content>
                <Card.Header>{profile.displayName}</Card.Header>
                <Card.Description>
                Bio here
                </Card.Description>
            </Card.Content>
            <Card.Content extra>
                <Icon name="user"/>
                {profile.followersCount} Followers
            </Card.Content>
            <FollowButtons profile={profile}/>
        </Card>
    )
}