import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Link } from "react-router-dom";
import { Item, Label, Tab, Image, Card } from "semantic-ui-react";
import { Profile } from "../../app/models/profile";
import { useStore } from "../../app/stores/store";

interface Props {
    profile: Profile
}

export default observer(function ProfileEvents({ profile }: Props) {
    const {profileStore} = useStore();
    const { loadActivitiesForUser, loading} = profileStore;
    useEffect(() => {
        loadActivitiesForUser(profile.username);
    }, [loadActivitiesForUser]);

    return (
        <Tab.Pane loading={loading}>
            <Card.Group>
                {
                    profile.activities!.map(activity => (
                        activity.hostUsername === profile.username &&
                        <Card key={activity.id}>
                            <Card.Header as={Link} to={`/activities/${activity.id}`}>{activity.title}
                                <Image size='tiny' src={`/assets/categoryImages/${activity.category}.jpg`} />
                            </Card.Header>

                            {activity.isCancelled &&
                                <Card.Content>
                                    <Label ribbon color='red' content='Cancelled' />
                                </Card.Content>
                            }

                            <Card.Content extra>
                                {activity.attendees.length} going
                            </Card.Content>
                        </Card>
                    ))
                }
            </Card.Group>
        </Tab.Pane>
    );
})