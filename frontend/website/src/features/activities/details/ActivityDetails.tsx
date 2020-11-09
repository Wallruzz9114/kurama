import { observer } from 'mobx-react-lite';
import React, { useContext, useEffect } from 'react';
import { Link, RouteComponentProps, useHistory } from 'react-router-dom';
import { Button, Card, Image } from 'semantic-ui-react';
import LoadingComponent from '../../../app/components/LoadingComponent';
import ActivityStore from '../../../app/mobx/activityStore';

interface DetailParams {
  id: string;
}

const ActivityDetails: React.FC<RouteComponentProps<DetailParams>> = ({ match }) => {
  const activityStore = useContext(ActivityStore);
  const history = useHistory();

  useEffect(() => {
    activityStore.loadActivity(match.params.id);
  }, [activityStore.loadActivity, match.params.id]);

  if (activityStore.loadingInitial || !activityStore.activity)
    return <LoadingComponent content="Loading activity..." />;

  return (
    <Card fluid>
      <Image
        src={`/assets/categoryImages/${activityStore.activity?.category}.jpg`}
        wrapped
        ui={false}
      />
      <Card.Content>
        <Card.Header>{activityStore.activity?.title}</Card.Header>
        <Card.Meta>
          <span className="date">{activityStore.activity?.dateTime}</span>
        </Card.Meta>
        <Card.Description>{activityStore.activity?.description}</Card.Description>
      </Card.Content>
      <Card.Content extra>
        <Button.Group widths={2}>
          <Button
            as={Link}
            to={`/manage/${activityStore.activity.id}`}
            basic
            color="blue"
            content="Edit"
          />
          <Button
            onClick={() => history.push('/activities')}
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
