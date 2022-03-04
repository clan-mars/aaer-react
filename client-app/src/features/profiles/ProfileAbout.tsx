import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Button, Grid, Header, Tab } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";
import ProfileForm from "./form/ProfileForm";


export default observer(function ProfileAbout() {
    const [editMode, setEditMode] = useState(false);
    const { profileStore: { updateProfileContent, loadActivitiesForUser, isCurrentUser, profile }, userStore, activityStore } = useStore();

    function handleFormSubmit(updatedProfile: Profile) {
        updateProfileContent(updatedProfile).then(() => setEditMode(false)).then(()=> userStore.getUser())
        .then(()=> loadActivitiesForUser(userStore.user?.username!));
    }

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated='left' icon='user' content={`About ${profile?.displayName}`} />
                    {isCurrentUser && 
                        <Button basic content={editMode ? "Cancel" : "Edit Profile"} onClick={() => setEditMode(!editMode)} floated="right" />
                    }
                </Grid.Column>
                <Grid.Column width={16}>
                    {
                        editMode ?
                            <ProfileForm profile={profile!} handleSubmit={handleFormSubmit} />
                            :
                            <span style={{ whiteSpace: 'pre-wrap' }} >
                                {profile?.bio? profile.bio : profile?.displayName + " has not written anything yet!"}
                            </span>
                    }
                </Grid.Column>
            </Grid>


        </Tab.Pane>
    )
})