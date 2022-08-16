import { Form, Label, Select } from "semantic-ui-react";
import { useField } from "formik";
import React from "react";

interface FormSelectInputProps {
  placeholder: string;
  name: string;
  options: any;
  label?: string;
}

export function FormSelectInput(props: FormSelectInputProps) {
  const [field, meta, helpers] = useField(props.name);
  return (
    <Form.Field error={meta.touched && !!meta.error}>
      <label>{props.label}</label>
      <Select
        options={props.options}
        clearable
        value={field.value || null}
        onChange={(event, data) => helpers.setValue(data.value)}
        onBlur={() => helpers.setTouched(true)}
        placeholder={props.placeholder}
      />
      {meta.touched && meta.error ? (
        <Label basic color="red" content={meta.error} />
      ) : null}
    </Form.Field>
  );
}
