import React from "react";
import { ErrorMessage, Form, Formik } from "formik";
import { FormTextInput } from "../../../app/common/form";
import { Button, Header, Label } from "semantic-ui-react";
import { useStore } from "../../../app/stores";
import { ValidationError } from "yup";

export function LoginForm() {
  const { userStore } = useStore();
  return (
    <Formik
      initialValues={{ password: "", email: "", error: "" }}
      onSubmit={(values, { setErrors }) =>
        userStore
          .login(values)
          .catch((error: ValidationError) => setErrors({error: 'Invalid email or password'}))
      }
    >
      {({ handleSubmit, isSubmitting, errors }) => (
        <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
          <Header as='h2' content='Login to Reactivities' color='teal' text-align='center'/>
          <FormTextInput
            name="email"
            placeholder="Email"
            autoComplete="username"
          />
          <FormTextInput
            name="password"
            placeholder="Password"
            type="password"
            autoComplete="current-password"
          />
          <ErrorMessage 
                        name='error' render={() => 
                        <Label style={{marginBottom: 10}} basic color='red' content={errors.error}/>}
                    />
          <Button
            loading={isSubmitting}
            positive
            content="Login"
            type="submit"
            fluid
          ></Button>
        </Form>
      )}
    </Formik>
  );
}
