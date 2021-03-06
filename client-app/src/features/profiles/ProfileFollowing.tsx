import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { useStore } from "../../app/stores/store";
import ProfileGrid from "./ProfileGrid";

export default observer(function ProfileFollowings() {
    const { profileStore } = useStore();
    const { loadFollowings, loadingFollowings, profile } = profileStore;

    useEffect(() => {
        loadFollowings();
    }, [loadFollowings]);

    return <ProfileGrid 
            profiles={profile!.followings} 
            profile={profile} 
            loading={loadingFollowings} 
            text={`People ${profile!.displayName} follows`} />
});