import { observer } from 'mobx-react-lite';
import React, { useContext } from 'react';
import { Button, Container, Menu } from 'semantic-ui-react';
import ActivityStore from '../../app/mobx/activityStore';

const NavBar: React.FC = () => {
  const activityStore = useContext(ActivityStore);

  return (
    <Menu fixed="top" inverted>
      <Container>
        <Menu.Item header>
          <img src="/assets/logo.png" alt="logo" style={styles.logo} />
          KURAMA
        </Menu.Item>
        <Menu.Item name="messages" />
        <Menu.Item>
          <Button onClick={activityStore.openNewActivityForm} positive content="New Activity" />
        </Menu.Item>
      </Container>
    </Menu>
  );
};

const styles = {
  logo: { marginRight: '10px' },
};

export default observer(NavBar);
