import React, { useContext } from 'react';
import { Menu, Container, Button } from 'semantic-ui-react';
import ActivityStore from '../../app/mobx/activityStore';
import { observer } from 'mobx-react-lite';

const NavBar: React.FC = () => {
  const activityStore = useContext(ActivityStore);

  return (
    <Menu fixed='top' inverted>
      <Container>
        <Menu.Item header>
          <img src='/assets/images/logo.png' alt='logo' style={styles.logo} />
          KURAMA
        </Menu.Item>
        <Menu.Item name='messages' />
        <Menu.Item>
          <Button
            onClick={activityStore.openNewActivityForm}
            positive
            content='New Activity'
          />
        </Menu.Item>
      </Container>
    </Menu>
  );
};

const styles = {
  logo: { marginRight: '10px' },
};

export default observer(NavBar);
