import { observer } from 'mobx-react-lite';
import React, { FormEvent, useContext, useEffect, useState } from 'react';
import { RouteComponentProps } from 'react-router-dom';
import { Button, Form, Grid, Segment } from 'semantic-ui-react';
import { v4 as uuid } from 'uuid';
import ActivityStore from '../../../app/mobx/activityStore';
import { IActivity } from '../../../app/models/activity';

interface DetailParams {
  id: string;
}

const ActivityForm: React.FC<RouteComponentProps<DetailParams>> = ({ match, history }) => {
  const activityStore = useContext(ActivityStore);
  const [activity, setActivity] = useState<IActivity>({
    id: '',
    title: '',
    category: '',
    description: '',
    date: '',
    city: '',
    venue: '',
  });

  useEffect(() => {
    if (match.params.id && activity.id.length === 0) {
      activityStore
        .loadActivity(match.params.id)
        .then(() => activityStore.activity && setActivity(activityStore.activity));
    }

    return () => {
      activityStore.clearActivity();
    };
  }, [
    activityStore.loadActivity,
    activityStore.clearActivity,
    match.params.id,
    activityStore.activity,
    activity.id.length,
  ]);

  const inputChangeHandler = (event: FormEvent<HTMLInputElement | HTMLTextAreaElement>) =>
    setActivity({
      ...activity,
      [event.currentTarget.name]: event.currentTarget.value,
    });

  const submitForm = () => {
    if (activity.id.length === 0) {
      const newActivity = { ...activity, id: uuid() };
      activityStore
        .createActivity(newActivity)
        .then(() => history.push(`/activities/${newActivity.id}`));
    } else {
      activityStore.updateActivity(activity).then(() => history.push(`/activities/${activity.id}`));
    }
  };

  return (
    <Grid>
      <Grid.Column width={10}>
        <Segment clearing>
          <Form onSubmit={submitForm}>
            <Form.Input
              onChange={inputChangeHandler}
              name="title"
              placeholder="Title"
              value={activity.title}
            />
            <Form.TextArea
              onChange={inputChangeHandler}
              name="description"
              rows={2}
              placeholder="Description"
              value={activity.description}
            />
            <Form.Input
              onChange={inputChangeHandler}
              name="category"
              placeholder="Category"
              value={activity.category}
            />
            <Form.Input
              onChange={inputChangeHandler}
              name="date"
              type="datetime-local"
              placeholder="Date"
              value={activity.date}
            />
            <Form.Input
              onChange={inputChangeHandler}
              name="city"
              placeholder="City"
              value={activity.city}
            />
            <Form.Input
              onChange={inputChangeHandler}
              name="venue"
              placeholder="Venue"
              value={activity.venue}
            />
            <Button
              loading={activityStore.submittingForm}
              floated="right"
              positive
              type="submit"
              content="Submit"
            />
            <Button
              onClick={() => history.push('/activities')}
              floated="right"
              type="submit"
              content="Cancel"
            />
          </Form>
        </Segment>
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityForm);
