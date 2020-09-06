import React, { useContext } from 'react';
import { Card, Image, Button } from 'semantic-ui-react';
import ActivityStore from '../../../app/mobx/activityStore';
import { observer } from 'mobx-react-lite';

const ActivityDetails: React.FC = () => {
  const activityStore = useContext(ActivityStore);

  return (
    <Card fluid>
      <Image
        src={`/assets/images/categoryImages/${activityStore.selectedActivity?.category}.jpg`}
        wrapped
        ui={false}
      />
      <Card.Content>
        <Card.Header>{activityStore.selectedActivity?.title}</Card.Header>
        <Card.Meta>
          <span className='date'>{activityStore.selectedActivity?.date}</span>
        </Card.Meta>
        <Card.Description>
          {activityStore.selectedActivity?.description}
        </Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Button.Group widths={2}>
          <Button
            onClick={() => {
              activityStore.selectedActivity != null
                ? activityStore.openEditForm(activityStore.selectedActivity.id)
                : console.log('Selected activity is undefined');
            }}
            basic
            color='blue'
            content='Edit'
          />
          <Button
            onClick={activityStore.cancelSelectedActivity}
            basic
            color='yellow'
            content='Cancel'
          />
        </Button.Group>
      </Card.Content>
    </Card>
  );
};

export default observer(ActivityDetails);
