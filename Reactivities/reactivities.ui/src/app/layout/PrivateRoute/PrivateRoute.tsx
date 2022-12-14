import React from 'react';
import { Redirect, Route, RouteComponentProps, RouteProps } from 'react-router-dom';
import { useStore } from '../../stores';

interface Props extends RouteProps {
    component: React.ComponentType<RouteComponentProps<any>> | React.ComponentType<any>
}

export function PrivateRoute({component: Component, ...rest}: Props){
    const {userStore: {isLoggedIn}} = useStore();

    return (
        <Route 
            {...rest}
            render={(props) => isLoggedIn ? <Component {...props} /> : <Redirect to="/" />} 
        />
    )
}