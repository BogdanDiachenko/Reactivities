import React from 'react';
import { Button, Divider, Grid, Header, Item, Reveal, RevealContent, Segment, Statistic } from 'semantic-ui-react';
import { Profile } from '../../../../app/models/profile';
import FollowButtons from '../FollowButtons';

export interface Props{
    profile: Profile;
}

export function ProfileHeader({profile}: Props){
    return (
        <Segment>
            <Grid>
                <Grid.Column width={12}>
                    <Item.Group>
                        <Item>
                            <Item.Image avatar size='small' src={profile.image || '/assets/user.png'}/>
                            <Item.Content verticalAlign='middle'>
                                <Header as="h1" content={profile.displayName}/>
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Grid.Column>
                <Grid.Column width={4}>
                    <Statistic.Group width={2}>
                        <Statistic label='Followers' value={profile.followersCount}/>
                        <Statistic label='Following' value={profile.followingCount}/>
                    </Statistic.Group>
                    <Divider />
                <FollowButtons profile={profile}/>
                </Grid.Column>
            </Grid>
        </Segment>
    )
}