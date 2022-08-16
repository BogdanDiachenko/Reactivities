import React from "react";
import { Link } from "react-router-dom";
import { Button, Container, Header, Image, Segment } from "semantic-ui-react";
import { useStore } from "../../app/stores";
import LoginForm from "../users/LoginForm";
import { RegisterForm } from "../users/RegisterForm/RegisterForm";

export function HomePage() {
  const { userStore, modalStore } = useStore();

  return (
    <Segment inverted vertical textAlign="center" className="masthead">
      <Container text>
        <Header as="h2" inverted>
          <Image size="massive" src="/assets/logo.png" alt="logo" />
          Reactivities
        </Header>
        {userStore.isLoggedIn ? (
          <>
            <Header inverted as="h2" content="Welcome to Reactivities" />
            <Button as={Link} to="/activities" size="huge" inverted>
              Go to Activities!
            </Button>
          </>
        ) : (
            <>
          <Button onClick={() => modalStore.openModal(<LoginForm/>)} to="/login" size="huge" inverted>
            Login!
            </Button>
            <Button onClick={() => modalStore.openModal(<RegisterForm/>)} to="/register" size="huge" inverted>
            Register!
          </Button>
            </>
        )}
      </Container>
    </Segment>
  );
}
