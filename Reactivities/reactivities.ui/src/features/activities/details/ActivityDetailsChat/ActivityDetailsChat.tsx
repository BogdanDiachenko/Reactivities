import { Formik, Form, setNestedObjectValues, Field, FieldProps} from "formik";
import React, { useEffect } from "react";
import { Link } from "react-router-dom";
import { Segment, Header, Comment, Button, Loader } from "semantic-ui-react";
import { FormTextArea } from "../../../../app/common/form";
import { useStore } from "../../../../app/stores";
import * as Yup from 'yup';
import { formatDistanceToNow } from "date-fns";

interface Props{
  activityId: string;
}

export function ActivityDetailsChat({activityId}: Props) {
  const {commentStore} = useStore();

  useEffect(() => {
    if(activityId){
      commentStore.createHubConnection(activityId);
    }

    return () => {
      commentStore.clearComments();
    }
  }, [commentStore, activityId])

  return (
    <>
      <Segment
        textAlign="center"
        attached="top"
        inverted
        color="teal"
        style={{ border: "none" }}
      >
        <Header>Chat about this event</Header>
      </Segment>
      <Segment attached clearing>
        <Comment.Group>
        <Formik
            onSubmit={(values, {resetForm}) => commentStore.addComment(values).then(() => resetForm())}
            initialValues={{body: ''}}
            validationSchema={Yup.object({
              body: Yup.string().required()
            })}
          >
            {({isSubmitting, isValid, handleSubmit}) => <Form className='ui form'>
            <Field name='body'>
            {(props: FieldProps) => (
                <div style={{position: 'relative'}}>
                  <Loader active={isSubmitting} />
                  <textarea 
                    placeholder='Enter your comment ("Enter" to submit "Enter" + "SHIFT" to new line)'
                    rows={2}
                    {...props.field}
                    onKeyPress={e => {
                      if(e.key === "Enter" && !e.shiftKey){
                        e.preventDefault();
                        isValid && handleSubmit();
                      }
                    }}
                    />
                </div>
              )}
            </Field>
          </Form>}
          </Formik>
          {commentStore.comments.map((comment) => (
            <Comment key={comment.id}>
              <Comment.Avatar src={comment.image || '/assets/user.png'} />
              <Comment.Content>
                <Comment.Author as={Link} to={`/profiles/${comment.username}`}>{comment.displayName}</Comment.Author>
                <Comment.Metadata>
                  <div>{formatDistanceToNow(comment.createdAt)}</div>
                </Comment.Metadata>
                <Comment.Text style={{whiteSpace: 'pre-wrap'}}>{comment.body}</Comment.Text>
              </Comment.Content>
           </Comment>
          ))}
        </Comment.Group>
      </Segment>
    </>
  );
}