import { observer } from 'mobx-react-lite';
import React, { useContext } from 'react';
import { Button, Card, Image } from 'semantic-ui-react';
import ActivityStore from '../../../app/mobx/activityStore';

const ActivityDetails: React.FC = () => {
  const activityStore = useContext(ActivityStore);

  return (
    <Card fluid>
      <Image
        src={`/assets/categoryImages/${activityStore.selectedActivity?.category}.jpg`}
        wrapped
        ui={false}
      />
      <Card.Content>
        <Card.Header>{activityStore.selectedActivity?.title}</Card.Header>
        <Card.Meta>
          <span className="date">{activityStore.selectedActivity?.dateTime}</span>
        </Card.Meta>
        <Card.Description>{activityStore.selectedActivity?.description}</Card.Description>
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
            color="blue"
            content="Edit"
          />
          <Button
            onClick={activityStore.cancelSelectedActivity}
            basic
            color="yellow"
            content="Cancel"
          />
        </Button.Group>
      </Card.Content>
    </Card>
  );
};

export default observer(ActivityDetails);
