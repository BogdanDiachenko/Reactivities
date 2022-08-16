import { useStore } from "../../../app/stores";
import React from "react";
import { Container, Header, Segment } from "semantic-ui-react";

export default function ServerError() {
  const { commonStore } = useStore();

  if (!commonStore.error) return <div>...</div>;

  return (
    <Container>
      <Header as="h1" content="Server error" />
      <Header sub as="h5" color="red" content={commonStore.error.message} />
      {commonStore.error.stackTrace && (
        <Segment>
          <Header as="h4" content="Stack Trace" color="teal" />
          <code className='code' style={{ marginTop: "10px" }}>
            {commonStore.error.stackTrace}
          </code>
        </Segment>
      )}
    </Container>
  );
}
