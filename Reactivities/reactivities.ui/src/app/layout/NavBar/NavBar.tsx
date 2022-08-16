import React from "react";
import { Button, Container, Dropdown, Image, Menu } from "semantic-ui-react";
import { StyledImage } from "./NavBar.styled";
import { Link, NavLink } from "react-router-dom";
import { useStore } from "../../stores";

export function NavBar() {
  const { userStore } = useStore();
  const { user, logout } = userStore;

  return (
    <Menu inverted fixed="top">
      <Container>
        <Menu.Item  as={NavLink} to="/" exact header>
          <StyledImage src="/assets/logo.png" alt="logo" />
          Reactivities
        </Menu.Item>
        <Menu.Item
          color='violet'
          as={NavLink}
          to="/activities"
          name="Activities"
        />
        <Menu.Item>
          <Button
            as={NavLink}
            to="/createActivity"
            positive
            content="Create Activity"
          ></Button>
        </Menu.Item>
        {user && (
          <Menu.Item position="right">
            {user.image ? (
              <Image src={user.image} avatar spaced right="true" />
            ) : (
              <Image src="/assets/user.png" avatar spaced right="true" />
            )}
            <Dropdown pointing="top left" text={user.displayName}>
              <Dropdown.Menu>
                <Dropdown.Item
                  as={Link}
                  to={`/profiles/${user.username}`}
                  text="My profile"
                  icon="user"
                />
                <Dropdown.Item onClick={logout} text="Logout" icon="power" />
              </Dropdown.Menu>
            </Dropdown>
          </Menu.Item>
        )}
      </Container>
    </Menu>
  );
}
