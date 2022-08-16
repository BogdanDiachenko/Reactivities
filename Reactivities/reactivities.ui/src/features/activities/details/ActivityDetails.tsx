import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import { useStore } from "../../../app/stores";
import React, { useEffect } from "react";
import { Grid } from "semantic-ui-react";
import { useParams } from "react-router-dom";
import ActivityDetailsHeader from "./ActivityDetailsHeader";
import ActivityDetailsInfo from "./ActivityDetailsInfo";
import ActivityDetailsChat from "./ActivityDetailsChat";
import ActivityDetailsSidebar from "./ActivityDetailsSideBar";

export function ActivityDetails() {
  const { activityStore } = useStore();
  const {
    selectedActivity: activity,
    loadActivity,
    loadingInitial,
    clearSelectedActivity,
  } = activityStore;
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    if (id) loadActivity(id);

    return (() => clearSelectedActivity())
  }, [id, loadActivity, clearSelectedActivity]);

  if (loadingInitial || !activity) return <LoadingComponent />;
  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityDetailsHeader activity={activity} />
        <ActivityDetailsInfo activity={activity} />
        <ActivityDetailsChat activityId={activity.id}/>
      </Grid.Column>
      <Grid.Column width={6}>
        <ActivityDetailsSidebar activity={activity} />
      </Grid.Column>
    </Grid>
  );
}
