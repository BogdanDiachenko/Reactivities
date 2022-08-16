import { ActivityFormValues } from "../../../app/models";
import { useStore } from "../../../app/stores";
import React, { useEffect, useState } from "react";
import { Button, Segment } from "semantic-ui-react";
import { useHistory, useParams, Link } from "react-router-dom";
import { LoadingComponent } from "../../../app/layout/LoadingComponent";
import { v4 as uuid } from "uuid";
import { Formik, Form } from "formik";
import * as Yup from "yup";
import {
  FormDateInput,
  FormTextArea,
  FormTextInput,
} from "../../../app/common/form";
import { FormSelectInput, categoryOptions } from "../../../app/common/form";

export function ActivityForm() {
  const history = useHistory();
  const { activityStore } = useStore();
  const {
    createActivity,
    updateActivity,
    loadActivity,
    loadingInitial,
  } = activityStore;

  const { id } = useParams<{ id: string }>();

  const [activity, setActivity] = useState<ActivityFormValues>(new ActivityFormValues());

  const validationSchema = Yup.object({
    title: Yup.string().required(requiredError("title")),
    description: Yup.string().required(requiredError("description")),
    category: Yup.string().required(requiredError("category")),
    date: Yup.date().required(requiredError("date")).nullable(),
    venue: Yup.string().required(requiredError("venue")),
    city: Yup.string().required(requiredError("city")),
  });

  useEffect(() => {
    if (id)
      loadActivity(id).then((activity) => {
        setActivity(new ActivityFormValues(activity));
      });
  }, [id, loadActivity]);

  function handleFormSubmit(activity: ActivityFormValues) {
    if (!activity.id) {
      let newActivity = {
        ...activity,
        id: uuid(),
      };
      createActivity(newActivity).then(() => {
        console.log(activity);
        history.push(`/activities/${newActivity.id}`);
      }
      );
    } else {
      updateActivity(activity).then(() =>
        history.push(`/activities/${activity.id}`)
      );
    }
  }

  function requiredError(fieldName: string) {
    return `The activity ${fieldName} is required`;
  }
  if (loadingInitial) return <LoadingComponent content="Loading activity..." />;

  return (
    <Segment clearing>
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={activity}
        onSubmit={(values) => handleFormSubmit(values)}
      >
        {({ handleSubmit, isValid, isSubmitting, dirty }) => (
          <Form className="ui form" onSubmit={handleSubmit} autoComplete="off">
            <FormTextInput placeholder="Title" name="title" />
            <FormTextArea
              rows={3}
              placeholder="Description"
              name="description"
            />
            <FormSelectInput
              options={categoryOptions}
              placeholder="Category"
              name="category"
            />
            <FormDateInput
              placeholderText="Date"
              name="date"
              showTimeSelect
              timeCaption="time"
              dateFormat="MMMM d, yyyy h:mm aa"
            />
            <FormTextInput placeholder="City" name="city" />
            <FormTextInput placeholder="Venue" name="venue" />
            <Button
              disabled={isSubmitting || !dirty || !isValid}
              loading={isSubmitting}
              floated="right"
              positive
              type="submit"
              content="Submit"
            />
            <Button
              as={Link}
              to="/activities"
              floated="right"
              type="button"
              content="Cancel"
            />
          </Form>
        )}
      </Formik>
    </Segment>
  );
}
