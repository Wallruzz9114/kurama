import React, { useContext } from 'react';
import { Grid } from 'semantic-ui-react';
import ActivityList from './ActivityList';
import ActivityDetails from '../details/ActivityDetails';
import ActivityForm from '../form/ActivityForm';
import { observer } from 'mobx-react-lite';
import ActivityStore from '../../../app/mobx/activityStore';

const ActivityDashboard: React.FC = () => {
  const activityStore = useContext(ActivityStore);

  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityList />
      </Grid.Column>
      <Grid.Column width={6}>
        {activityStore.selectedActivity && !activityStore.editMode && (
          <ActivityDetails />
        )}
        {activityStore.editMode && (
          <ActivityForm key={activityStore.selectedActivity?.id || 0} />
        )}
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityDashboard);
