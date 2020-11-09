import { observer } from 'mobx-react-lite';
import React from 'react';
import { NavLink } from 'react-router-dom';
import { Button, Container, Menu } from 'semantic-ui-react';

const NavBar: React.FC = () => {
  return (
    <Menu fixed="top" inverted>
      <Container>
        <Menu.Item header exact as={NavLink} to="/">
          <img src="/assets/logo.png" alt="logo" style={styles.logo} />
          kurama
        </Menu.Item>
        <Menu.Item name="Activities" as={NavLink} to="/activities" />
        <Menu.Item>
          <Button as={NavLink} to="/new-activity" positive content="New Activity" />
        </Menu.Item>
      </Container>
    </Menu>
  );
};

const styles = {
  logo: { marginRight: '10px' },
};

export default observer(NavBar);
