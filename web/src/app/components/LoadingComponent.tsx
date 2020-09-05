import React from 'react';
import { Dimmer, Loader } from 'semantic-ui-react';

const LoadingComponent: React.FC<{ content?: string }> = (props) => {
  return (
    <Dimmer active inverted={true}>
      <Loader content={props.content} />
    </Dimmer>
  );
};

export default LoadingComponent;
