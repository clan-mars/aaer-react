import { Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { Button } from "semantic-ui-react";
import * as Yup from 'yup';
import MyTextArea from "../../../app/common/form/MyTextArea";
import MyTextInput from "../../../app/common/form/MyTextInput";
import LoadingComponent from "../../../app/layout/LoadingComponent";
import { Profile } from "../../../app/models/profile";
import { useStore } from "../../../app/stores/store";

interface Props {
    profile: Profile,
    handleSubmit: (profile:Profile) => void
}

export default observer(function ProfileForm({profile, handleSubmit}:Props) {

    const {profileStore: {loading}} = useStore();
    const validationSchema = Yup.object({
        displayName: Yup.string().required('The display name is required'),
        bio: Yup.string().optional()
    })

    if (loading) return <LoadingComponent content='Loading profile...' />

    return (
            <Formik validationSchema={validationSchema} enableReinitialize initialValues={profile} onSubmit={handleSubmit}>
                {({ handleSubmit, isValid, isSubmitting, dirty }) => (
                    <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                        <MyTextInput name='displayName' placeholder='Displayname' />
                        <MyTextArea placeholder='Bio' name='bio' rows={10} />
                        <Button
                            disabled={isSubmitting || !dirty || !isValid}
                            floated='right' positive type='submit' content='Update' />
                    </Form>
                )}
            </Formik>

    )
})