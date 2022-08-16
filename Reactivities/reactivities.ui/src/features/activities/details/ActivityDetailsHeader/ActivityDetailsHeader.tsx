import React from "react";
import { Link } from "react-router-dom";
import { Button, Item, Label, Segment } from "semantic-ui-react";
import { Activity } from "../../../../app/models/activity";
import {
  StyledImage,
  WhiteHeader,
  WhiteParagraph,
} from "./ActivityDetailsHeader.styled";
import { format } from "date-fns";
import { useStore } from "../../../../app/stores";

const activityImageTextStyle = {
  position: "absolute",
  bottom: "5%",
  left: "5%",
  width: "100%",
  height: "auto",
};

interface ActivityDetailsHeaderProps {
  activity: Activity;
}

export function ActivityDetailsHeader({ activity}: ActivityDetailsHeaderProps) {
  const {activityStore: {updateAttendance, cancelActivity, loading}} = useStore();

  if(!activity.attendees) {console.log('return');return <></> }
  if(!activity.host) return <></> 
  return (
      <Segment.Group>
        <Segment basic attached="top" style={{ padding: "0" }}>
          {activity.isCancelled && (
            <Label style={{position: 'absolute', zIndex: 1000, left:-14, top:20}} ribbon color='red' content='Cancelled'></Label>
          )}
          <StyledImage
            src={`/assets/categoryImages/${activity.category}.jpg`}
            fluid
          />
          <Segment style={activityImageTextStyle} basic>
            <Item.Group>
              <Item>
                <Item.Content>
                  <WhiteHeader>{activity.title}</WhiteHeader>
                  <WhiteParagraph>
                    {format(activity.date, "dd/MM/yyyy")}
                  </WhiteParagraph>
                  <WhiteParagraph>
                    Hosted by <strong><Link to={`/profiles/${activity.host.userName}`}>{activity.host.displayName}</Link></strong>
                  </WhiteParagraph>
                </Item.Content>
              </Item>
            </Item.Group>
          </Segment>
        </Segment>
        <Segment clearing attached="bottom">
          {activity.isHost ? (
          <> 
          <Button color={activity.isCancelled ? 'green' : 'red'}
            floated='left'
            basic
            content={activity.isCancelled ? 'Re-activate Activity' : 'Cancel Activity'}
            onClick={cancelActivity}
            loading={loading}
          />
          <Button
          disabled={activity.isCancelled}
          as={Link}
          to={`/manage/${activity.id}`}
          color="orange"
          floated="right"
          content="Manage Event"
        /></>
         ) : (activity.isGoing ? (
            <Button disabled={loading} loading={loading} onClick={updateAttendance} content="Cancel attendance" />
          ) : (
            <Button disabled={loading || activity.isCancelled} loading={loading} onClick={updateAttendance} color="teal" content="Join activity" />
          ))}
          
        </Segment>
      </Segment.Group>
    );
}