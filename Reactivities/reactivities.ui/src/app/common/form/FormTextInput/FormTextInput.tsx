import { Form, Label } from "semantic-ui-react";
import { useField } from "formik";
import React from "react";

interface FormTextInputProps {
  placeholder: string;
  name: string;
  label?: string;
  type?: string;
  autoComplete?: string;
}

export function FormTextInput(props: FormTextInputProps) {
  const [field, meta] = useField(props.name);
  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <input {...field} {...props}></input>
      {meta.touched && meta.error ? (
        <Label basic color="red" content={meta.error} />
      ) : null}
    </Form.Field>
  );
}
