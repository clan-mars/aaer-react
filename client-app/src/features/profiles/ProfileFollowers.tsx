import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import ProfileGrid from "./ProfileGrid";

export default observer(function ProfileFollowers() {
    const { profileStore } = useStore();
    const { loadFollowers: loadFollowers, loadingFollowings, profile } = profileStore;

    useEffect(() => {
        loadFollowers();
    }, [loadFollowers]);

    return <ProfileGrid 
            profiles={profile!.followers} 
            profile={profile} 
            loading={loadingFollowings} 
            text={`People following ${profile?.displayName}`} />
});