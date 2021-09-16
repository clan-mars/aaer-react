import { observer } from "mobx-react-lite";
import { Tab } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import ProfileAbout from "./ProfileAbout";
import ProfileEvents from "./ProfileEvents";
import ProfileFollowing from "./ProfileFollowing";
import ProfilePhotos from "./ProfilePhotos";

interface Props {
    profile: Profile;
}

export default observer(function ProfileContent({ profile }: Props) {
    console.log(profile);
    const panes = [
        { menuItem: 'About', render: () => <ProfileAbout /> },
        { menuItem: 'Photos', render: () => <ProfilePhotos profile={profile} /> },
        { menuItem: 'Events', render: () => <ProfileEvents profile={profile} /> },
        { menuItem: 'Followers', render: () => <ProfileFollowing profiles={profile.followers}/> },
        { menuItem: 'Following', render: () => <ProfileFollowing profiles={profile.followings}/> }
    ]

    return (
        <Tab menu={{ fluid: true, vertical: true }} menuPosition='right' panes={panes} />
    )
})