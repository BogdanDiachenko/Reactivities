import React from "react";

interface Props {
  errors: any;
}

export function ValidationErrors({ errors }: Props) {
  return (
    <>
      <h3 style={{marginLeft: 20}}>Validations errors occured:</h3>
      {errors && (
        <ul >
          {errors.map((err: any, i: any) => (
            <li key={i}>{err}</li>
          ))}
        </ul>
      )}
    </>
  );
}
