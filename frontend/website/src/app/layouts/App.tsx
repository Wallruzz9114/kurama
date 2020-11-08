import { observer } from 'mobx-react-lite';
import React, { Fragment, useContext, useEffect } from 'react';
import { Container } from 'semantic-ui-react';
import ActivityDashboard from '../../features/activities/dashboard/ActivityDashboard';
import NavBar from '../../features/nav/NavBar';
import LoadingComponent from '../components/LoadingComponent';
import ActivityStore from '../mobx/activityStore';

const App = () => {
  const activityStore = useContext(ActivityStore);

  useEffect(() => {
    activityStore.loadActivities();
  }, [activityStore]);

  return activityStore.loadingInitial ? (
    <LoadingComponent content="Loading activities..." />
  ) : (
    <Fragment>
      <NavBar />
      <Container style={styles.listContainer}>
        <ActivityDashboard />
      </Container>
    </Fragment>
  );
};

const styles = {
  listContainer: { marginTop: '7em' },
};

export default observer(App);
