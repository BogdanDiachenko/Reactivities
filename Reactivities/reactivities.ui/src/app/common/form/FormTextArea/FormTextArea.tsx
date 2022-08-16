import { Form, Label } from "semantic-ui-react";
import { useField } from "formik";
import React from "react";

interface FormTextAreaProps {
  placeholder: string;
  name: string;
  rows: number;
  label?: string;
}

export function FormTextArea(props: FormTextAreaProps) {
  const [field, meta] = useField(props.name);
  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <textarea {...field} {...props}></textarea>
      {meta.touched && meta.error ? (
        <Label basic color="red" content={meta.error} />
      ) : null}
    </Form.Field>
  );
}
