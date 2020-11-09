import { observer } from 'mobx-react-lite';
import React, { Fragment, useContext } from 'react';
import { Item, Label } from 'semantic-ui-react';
import ActivityStore from '../../../app/mobx/activityStore';
import { IActivity } from '../../../app/models/activity';
import ActivityListItem from './ActivityListItem';

const ActivityList: React.FC = () => {
  const activityStore = useContext(ActivityStore);

  return (
    <Fragment>
      {activityStore.activitiesSortedByDate.map(([group, activities]) => (
        <Fragment>
          <Label size="large" color="blue">
            {group}
          </Label>
          <Item.Group divided>
            {activities.map((activity: IActivity) => (
              <ActivityListItem key={activity.id} activity={activity} />
            ))}
          </Item.Group>
        </Fragment>
      ))}
    </Fragment>
  );
};

export default observer(ActivityList);
