import React from "react";
import { ErrorMessage, Form, Formik } from "formik";
import { FormTextInput } from "../../../app/common/form";
import { Button, Header } from "semantic-ui-react";
import { useStore } from "../../../app/stores";
import * as Yup from 'yup';
import { ValidationErrors } from "../../error/ValidationErrors";

export function RegisterForm() {
  const { userStore } = useStore();
  return (
    <Formik
      initialValues={{
        password: "",
        email: "",
        displayName: "",
        username: "",
        error: null,
      }}
      onSubmit={(values, { setErrors }) =>
        userStore.register(values).catch((error) => setErrors({ error }))
      }
      validationSchema={Yup.object({
        displayName: Yup.string().required(),
        username: Yup.string().required(),
        email: Yup.string().required().email(),
        password: Yup.string().required(),
      })}
    >
      {({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
        <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
          <Header
            as="h2"
            content="Sign up to Reactivities"
            color="teal"
            text-align="center"
          />
          <FormTextInput
            name="displayName"
            placeholder="Display name"
            autoComplete="off"
          />
          <FormTextInput
            name="username"
            placeholder="Username"
            autoComplete="off"
          />
          <FormTextInput name="email" placeholder="Email" autoComplete="off" />
          <FormTextInput
            name="password"
            placeholder="Password"
            type="password"
            autoComplete="off"
          />
          <ErrorMessage
            name="error"
            render={() => <ValidationErrors errors={errors.error} />}
          />
          <Button
            disabled={!isValid || !dirty || isSubmitting}
            loading={isSubmitting}
            positive
            content="Register"
            type="submit"
            fluid
          ></Button>
        </Form>
      )}
    </Formik>
  );
}
