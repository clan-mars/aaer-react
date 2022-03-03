import { FieldProps, Form, Formik, Field } from 'formik';
import { observer } from 'mobx-react-lite'
import { useEffect } from 'react'
import { Link } from 'react-router-dom';
import { Segment, Header, Comment, Loader } from 'semantic-ui-react'
import { useStore } from '../../app/stores/store';
import * as Yup from 'yup';
import { formatDistanceToNow } from 'date-fns';

interface Props {
    activityId: string;
}

export default observer(function ActivityDetailedChat({ activityId }: Props) {
    const { commentStore } = useStore();
    useEffect(() => {
        if (activityId) {
            commentStore.createHubConnection(activityId);
        }

        return () => {
            commentStore.clearComments();
        }
    }, [commentStore, activityId])

    const validationSchema = Yup.object({
        body: Yup.string().required('Comment text is required')
    })

    return (
        <>
            <Segment
                textAlign='center'
                attached='top'
                inverted
                color='teal'
                style={{ border: 'none' }}
            >
                <Header>Chat about this event</Header>
            </Segment>
            <Segment attached clearing>
                <Formik
                    validationSchema= {validationSchema}
                    onSubmit={(values, { resetForm }) => commentStore.addComment(values).then(() => resetForm())}
                    initialValues={{ body: '' }}>

                    {({ isSubmitting, isValid, handleSubmit }) => (
                        <Form className='ui form'>
                            <Field name='body'>
                                {(props: FieldProps) => (
                                    <div style={{position: 'relative'}}>
                                        <Loader active={isSubmitting} />
                                        <textarea
                                            placeholder='Enter your comment (Enter to submit, Shift + enter for new line)'
                                            rows={2}
                                            {...props.field}
                                            onKeyPress={e => {
                                                if ( e.key === 'Enter' && e.shiftKey) {
                                                    return;
                                                }

                                                if (e.key === 'Enter' && !e.shiftKey) {
                                                    e.preventDefault();
                                                    isValid && handleSubmit();
                                                } 
                                            }}
                                        />
                                    </div>
                                )}
                            </Field>
                        </Form>
                    )}
                </Formik>

                <Comment.Group>

                    {commentStore.comments.map(comment => {
                        return <Comment id={comment.id} key={comment.id}>
                            <Comment.Avatar src={comment.image || '/assets/user.png'} />
                            <Comment.Content>
                                <Comment.Author as={Link} to={`profiles/${comment.username}`}>{comment.displayName}</Comment.Author>
                                <Comment.Metadata>
                                    <div>{formatDistanceToNow(comment.createdAt)}</div>
                                </Comment.Metadata>
                                <Comment.Text style={{whiteSpace: 'pre-wrap'}}>{comment.body}</Comment.Text>
                            </Comment.Content>
                        </Comment>
                    })}
                </Comment.Group>
            </Segment>
        </>

    )
})
