import React from "react";
import { Link } from "react-router-dom";
import { Card, Label, Tab } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";

interface Props {
    profiles: Profile[];
}
export default function ProfileFollowing({profiles}: Props) {
    console.log(profiles);
    return (
        
        <Tab.Pane>
            {
            profiles &&
                profiles.map(profile => (
                    <Card key={profile.displayName}>
                        <Card.Header>
                            <Label as={Link} to={`/profiles/${profile.username}`}>{profile.displayName}</Label>
                        </Card.Header>
                    </Card>
                ))
            }
        </Tab.Pane>
    );

}