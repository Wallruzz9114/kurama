import React, { useState, useEffect, Fragment } from 'react';
import { Container } from 'semantic-ui-react';
import axios, { AxiosResponse } from 'axios';
import { IActivity } from '../models/activity';
import ActivityDashboard from '../../feature/activities/dashboard/ActivityDashboard';
import NavBar from '../../feature/nav/NavBar';

const App = () => {
  const [activities, setActivities] = useState<IActivity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<IActivity | null>(
    null
  );
  const [editMode, setEditMode] = useState(false);

  const selectActivity = (id: string) => {
    setSelectedActivity(activities.filter((a) => a.id === id)[0]);
    setEditMode(false);
  };

  const openNewActivityForm = () => {
    setSelectedActivity(null);
    setEditMode(true);
  };

  useEffect(() => {
    axios
      .get<IActivity[]>('http://localhost:5000/api/activities')
      .then((response: AxiosResponse<IActivity[]>) => {
        const fixedActivities: IActivity[] = [];

        response.data.forEach((activity: IActivity) => {
          activity.date = activity.date.split('.')[0];
          fixedActivities.push(activity);
        });

        setActivities(fixedActivities);
      });
  }, []);

  const createActivity = (activity: IActivity) => {
    setActivities([...activities, activity]);
    setSelectedActivity(activity);
    setEditMode(false);
  };

  const updateActivity = (activity: IActivity) => {
    setActivities([
      ...activities.filter((a) => a.id !== activity.id),
      activity,
    ]);
    setSelectedActivity(activity);
    setEditMode(false);
  };

  const deleteActivity = (id: string) =>
    setActivities([...activities.filter((a) => a.id !== id)]);

  return (
    <Fragment>
      <NavBar openNewActivityForm={openNewActivityForm} />
      <Container style={styles.listContainer}>
        <ActivityDashboard
          activities={activities}
          selectActivity={selectActivity}
          selectedActivity={selectedActivity}
          editMode={editMode}
          setEditMode={setEditMode}
          setSelectedActivity={setSelectedActivity}
          createActivity={createActivity}
          updateActivity={updateActivity}
          deleteActivity={deleteActivity}
        />
      </Container>
    </Fragment>
  );
};

const styles = {
  listContainer: { marginTop: '7em' },
};

export default App;
