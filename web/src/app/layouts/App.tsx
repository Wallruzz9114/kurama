import React, { useState, useEffect, Fragment, SyntheticEvent } from 'react';
import { Container } from 'semantic-ui-react';
import { IActivity } from '../models/activity';
import ActivityDashboard from '../../feature/activities/dashboard/ActivityDashboard';
import NavBar from '../../feature/nav/NavBar';
import activitiesService from '../api/agent';
import LoadingComponent from '../components/LoadingComponent';

const App = () => {
  const [activities, setActivities] = useState<IActivity[]>([]);
  const [selectedActivity, setSelectedActivity] = useState<IActivity | null>(
    null
  );
  const [editMode, setEditMode] = useState(false);
  const [loading, setLoading] = useState(true);
  const [submittingForm, setSubmittingForm] = useState(false);
  const [target, setTarget] = useState('');
  const selectActivity = (id: string) => {
    setSelectedActivity(activities.filter((a) => a.id === id)[0]);
    setEditMode(false);
  };

  const openNewActivityForm = () => {
    setSelectedActivity(null);
    setEditMode(true);
  };

  useEffect(() => {
    activitiesService
      .listAll()
      .then((response) => {
        const fixedActivities: IActivity[] = [];

        response.forEach((activity: IActivity) => {
          activity.date = activity.date.split('.')[0];
          fixedActivities.push(activity);
        });

        setActivities(fixedActivities);
      })
      .then(() => setLoading(false));
  }, []);

  const createActivity = (activity: IActivity) => {
    setSubmittingForm(true);
    activitiesService
      .create(activity)
      .then(() => {
        setActivities([...activities, activity]);
        setSelectedActivity(activity);
        setEditMode(false);
      })
      .then(() => setSubmittingForm(false));
  };

  const updateActivity = (activity: IActivity) => {
    setSubmittingForm(true);
    activitiesService
      .update(activity)
      .then(() => {
        setActivities([
          ...activities.filter((a) => a.id !== activity.id),
          activity,
        ]);
        setSelectedActivity(activity);
        setEditMode(false);
      })
      .then(() => setSubmittingForm(false));
  };

  const deleteActivity = (
    event: SyntheticEvent<HTMLButtonElement>,
    id: string
  ) => {
    setSubmittingForm(true);
    setTarget(event.currentTarget.name);
    activitiesService
      .delete(id)
      .then(() => {
        setActivities([...activities.filter((a) => a.id !== id)]);
      })
      .then(() => setSubmittingForm(false));
  };

  return loading ? (
    <LoadingComponent content='Loading activities...' />
  ) : (
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
          submittingForm={submittingForm}
          target={target}
        />
      </Container>
    </Fragment>
  );
};

const styles = {
  listContainer: { marginTop: '7em' },
};

export default App;
