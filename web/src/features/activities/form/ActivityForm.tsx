import React, { useState, FormEvent, useContext } from 'react';
import { Segment, Form, Button } from 'semantic-ui-react';
import { IActivity } from '../../../app/models/activity';
import { v4 as uuid } from 'uuid';
import ActivityStore from '../../../app/mobx/activityStore';
import { observer } from 'mobx-react-lite';

const ActivityForm: React.FC = () => {
  const activityStore = useContext(ActivityStore);

  const initializeForm = () =>
    activityStore.selectedActivity
      ? activityStore.selectedActivity
      : {
          id: '',
          title: '',
          category: '',
          description: '',
          date: '',
          city: '',
          venue: '',
        };

  const [activity, setActivity] = useState<IActivity>(initializeForm);

  const inputChangeHandler = (
    event: FormEvent<HTMLInputElement | HTMLTextAreaElement>
  ) =>
    setActivity({
      ...activity,
      [event.currentTarget.name]: event.currentTarget.value,
    });

  const submitForm = () =>
    activity.id.length === 0
      ? activityStore.createActivity({ ...activity, id: uuid() })
      : activityStore.updateActivity(activity);

  return (
    <Segment clearing>
      <Form onSubmit={submitForm}>
        <Form.Input
          onChange={inputChangeHandler}
          name='title'
          placeholder='Title'
          value={activity.title}
        />
        <Form.TextArea
          onChange={inputChangeHandler}
          name='description'
          rows={2}
          placeholder='Description'
          value={activity.description}
        />
        <Form.Input
          onChange={inputChangeHandler}
          name='category'
          placeholder='Category'
          value={activity.category}
        />
        <Form.Input
          onChange={inputChangeHandler}
          name='date'
          type='datetime-local'
          placeholder='Date'
          value={activity.date}
        />
        <Form.Input
          onChange={inputChangeHandler}
          name='city'
          placeholder='City'
          value={activity.city}
        />
        <Form.Input
          onChange={inputChangeHandler}
          name='venue'
          placeholder='Venue'
          value={activity.venue}
        />
        <Button
          loading={activityStore.submittingForm}
          floated='right'
          positive
          type='submit'
          content='Submit'
        />
        <Button
          onClick={activityStore.cancelForm}
          floated='right'
          type='submit'
          content='Cancel'
        />
      </Form>
    </Segment>
  );
};

export default observer(ActivityForm);