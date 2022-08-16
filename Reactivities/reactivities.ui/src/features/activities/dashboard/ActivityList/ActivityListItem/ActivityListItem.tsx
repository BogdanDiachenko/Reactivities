import React from "react";
import { Activity } from "@models";
import { useStore } from "../../../../../app/stores";
import { SyntheticEvent, useState } from "react";
import { Link } from "react-router-dom";
import {
  Button,
  Icon,
  Item,
  ItemContent,
  ItemDescription,
  ItemGroup,
  ItemImage,
  Label,
  Segment,
  SegmentGroup,
} from "semantic-ui-react";
import { format } from "date-fns";
import { ActivityAttendee } from "../../ActivityAttendee/ActivityAttendee";

interface ActivityListItemProps {
  activity: Activity;
}

export default function ActivityListItem({ activity }: ActivityListItemProps) {
  const { activityStore } = useStore();
  const { deleteActivity } = activityStore;

  const [target, setTarget] = useState("");

  function handleActivityDelete(
    e: SyntheticEvent<HTMLButtonElement>,
    id: string
  ) {
    setTarget(e.currentTarget.name);
    deleteActivity(id);
  }

  return (
    <SegmentGroup>
      <Segment>
        {activity.isCancelled && (
          <Label attached='top' color='red' content='Cancelled' style={{textAlign: 'center'}}/>
        )}
        <ItemGroup>
          <Item>
            <ItemImage size="tiny" circular src={(activity.host && activity.host.image) || "/assets/user.png"} style={{marginBottom: 3}}/>
            <ItemContent>
              <Item.Header as={Link} to={`/activities/${activity.id}`}>
                {activity.title}
              </Item.Header>
              {activity.host &&
                <ItemDescription>
                  Hosted by <Link to={`/profiles/${activity.host.userName}`}>{activity.host.displayName} </Link>
                </ItemDescription>
              }
              {activity.isHost && (
                <Item.Description>
                  <Label basic color="green">
                    You are hosting this activity
                  </Label>
                </Item.Description>
              )}
              {activity.isGoing && !activity.isHost && (
                <Item.Description>
                  <Label basic color="orange">
                    You are going to this activity
                  </Label>
                </Item.Description>
              )}
            </ItemContent>
          </Item>
        </ItemGroup>
      </Segment>
      <Segment>
        <span>
          <Icon name="clock" /> {format(activity.date, "dd/MM/yyyy h:mm aa")}
          <Icon name="marker" /> {activity.venue}
        </span>
      </Segment>
      <Segment secondary>
        {activity.attendees && (
          <ActivityAttendee attendees={activity.attendees} />
        )}
      </Segment>
      <Segment clearing>
        <span>{activity.description}</span>
        <Button
          as={Link}
          to={`/activities/${activity.id}`}
          color="teal"
          floated="right"
          content="View"
        />
      </Segment>
    </SegmentGroup>
  );
}
