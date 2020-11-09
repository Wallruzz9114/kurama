import { observer } from 'mobx-react-lite';
import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { Button, Item, Label, Segment } from 'semantic-ui-react';
import ActivityStore from '../../../app/mobx/activityStore';
import { IActivity } from '../../../app/models/activity';

const ActivityList: React.FC = () => {
  const activityStore = useContext(ActivityStore);

  return (
    <Segment clearing>
      <Item.Group divided>
        {activityStore.activitiesSortedByDate.map((activity: IActivity) => (
          <Item key={activity.id}>
            <Item.Content>
              <Item.Header as="a">{activity.title}</Item.Header>
              <Item.Meta>{activity.dateTime}</Item.Meta>
              <Item.Description>
                <div>{activity.description}</div>
                <div>
                  {activity.city}, {activity.venue}
                </div>
              </Item.Description>
              <Item.Extra>
                <Button
                  as={Link}
                  to={`/activities/${activity.id}`}
                  floated="right"
                  content="View"
                  color="blue"
                />
                <Button
                  name={activity.id}
                  loading={activityStore.target === activity.id && activityStore.submittingForm}
                  onClick={(event: React.MouseEvent<HTMLButtonElement, MouseEvent>) =>
                    activityStore.deleteActivity(event, activity.id)
                  }
                  floated="right"
                  content="Delete"
                  color="red"
                />
                <Label basic content={activity.category} />
              </Item.Extra>
            </Item.Content>
          </Item>
        ))}
      </Item.Group>
    </Segment>
  );
};

export default observer(ActivityList);
