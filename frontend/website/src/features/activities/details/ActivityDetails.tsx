import { observer } from 'mobx-react-lite';
import React, { useContext, useEffect } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Grid } from 'semantic-ui-react';
import LoadingComponent from '../../../app/components/LoadingComponent';
import ActivityStore from '../../../app/mobx/activityStore';
import ActivityDetailsChat from './ActivityDetailsChat';
import ActivityDetailsHeader from './ActivityDetailsHeader';
import ActivityDetailsInfo from './ActivityDetailsInfo';
import ActivityDetailsSidebar from './ActivityDetailsSidebar';

interface DetailParams {
  id: string;
}

const ActivityDetails: React.FC<RouteComponentProps<DetailParams>> = ({ match, history }) => {
  const activityStore = useContext(ActivityStore);

  useEffect(() => {
    activityStore.loadActivity(match.params.id);
  }, [activityStore.loadActivity, match.params.id, history]);

  if (activityStore.loadingInitial) return <LoadingComponent content="Loading activity..." />;

  if (!activityStore.activity) return <h2>Activity not found</h2>;

  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityDetailsHeader activity={activityStore.activity} />
        <ActivityDetailsInfo activity={activityStore.activity} />
        <ActivityDetailsChat />
      </Grid.Column>
      <Grid.Column width={6}>
        <ActivityDetailsSidebar />
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityDetails);
