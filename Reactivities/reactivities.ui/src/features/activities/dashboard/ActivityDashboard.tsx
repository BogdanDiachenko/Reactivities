import React, { useEffect, useState } from "react";
import { Button, Grid, Header, Loader } from "semantic-ui-react";
import ActivityList from "./ActivityList";
import { useStore } from "../../../app/stores";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import ActivityFilters from "./ActivityFilters";
import { PagingParams } from "../../../app/models/pagination";
import InfiniteScroll from "react-infinite-scroller";
import { ActivityListItem } from "./ActivityList/ActivityListItem";
import { ActivityListItemPlaceholder } from "./ActivityList/ActivityListItemPlaceholder";


export function ActivityDashboard() {
  const { activityStore } = useStore();
  const { loadActivities: loadActivties, activities, setPagingParams, pagination } = activityStore;
  const [loadingNext, setLoadingNext] = useState(false);

  useEffect(() => {
    if (activities.size <= 1) loadActivties();
  }, [activityStore]);

  function handleLoadingNext(){
      setLoadingNext(true);
      setPagingParams(new PagingParams(pagination!.currentPage + 1));
      loadActivties().then(() => setLoadingNext(false));
  }

  return (
    <Grid>
      <Grid.Column width={10}>
        {activityStore.loadingInitial && !loadingNext ? (
          <>
            <ActivityListItemPlaceholder />
            <ActivityListItemPlaceholder />
          </>
        ) : (
          <>
            <InfiniteScroll 
              pageStart={0}
              loadMore={handleLoadingNext}
              hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
              initialLoad={false}
            >
              <ActivityList />
            </InfiniteScroll>
          </>
        )}
      </Grid.Column>

      <Grid.Column width={4}>
        <ActivityFilters />
      </Grid.Column>
      
      <Grid.Column width={10}>
        <Loader active={loadingNext}/>
      </Grid.Column>
    </Grid>
  );
}
