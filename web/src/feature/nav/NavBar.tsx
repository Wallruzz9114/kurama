import React from 'react';
import { Menu, Container, Button } from 'semantic-ui-react';

interface IProps {
  openNewActivityForm: () => void;
}

const NavBar: React.FC<IProps> = (props) => (
  <Menu fixed='top' inverted>
    <Container>
      <Menu.Item header>
        <img src='/assets/images/logo.png' alt='logo' style={styles.logo} />
        KURAMA
      </Menu.Item>
      <Menu.Item name='messages' />
      <Menu.Item>
        <Button
          onClick={props.openNewActivityForm}
          positive
          content='New Activity'
        />
      </Menu.Item>
    </Container>
  </Menu>
);

const styles = {
  logo: { marginRight: '10px' },
};

export default NavBar;
