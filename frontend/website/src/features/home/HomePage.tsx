import React from 'react';
import { Link } from 'react-router-dom';
import { Container } from 'semantic-ui-react';

const HomePage = () => {
  return (
    <Container style={styles.containerMargin}>
      <h1>Home page</h1>
      <h3>
        Go to <Link to="/activities">activities</Link>
      </h3>
    </Container>
  );
};

const styles = {
  containerMargin: { marginTop: '7em' },
};

export default HomePage;
