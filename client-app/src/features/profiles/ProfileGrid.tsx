
import { Card, Grid, Header, Tab } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import ProfileCard from "./ProfileCard";

interface Props {
    profile: Profile|null
    profiles: Profile[];
    loading: boolean;
    text: string;
}

export default function ProfileGrid({ profiles, loading, text }: Props) {
    if (!profiles) profiles= [];
    return (
        <Tab.Pane loading={loading}>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated='left' icon='user' content={text}/>
                </Grid.Column>
                <Grid.Column width={16}>
                    <Card.Group itemsPerRow={4}>
                        {profiles.map(profile => (
                            <ProfileCard key={profile.username} profile={profile}/>
                        ))}
                    </Card.Group>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    );
}