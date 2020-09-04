import React, { Component } from 'react';
import { Header, Icon, List } from 'semantic-ui-react';
import './App.css';

class App extends Component {
  state = {
    values: [],
  };

  componentDidMount() {
    this.setState({
      values: [
        { id: 1, name: 'value 1' },
        { id: 2, name: 'value 2' },
      ],
    });
  }

  render() {
    return (
      <div>
        <Header as='h2'>
          <Icon name='users' />
          <Header.Content>Kurama</Header.Content>
        </Header>
        <List>
          {this.state.values.map((value: any) => (
            <List.Item key={value.id}>{value.name}</List.Item>
          ))}
        </List>
      </div>
    );
  }
}

export default App;
