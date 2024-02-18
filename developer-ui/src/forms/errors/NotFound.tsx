import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

export default function NotFound() {
    return (
        <Segment placeholder>
            <Header icon>
                <Icon name='question circle' />
                Cannot Find Requested Page
            </Header>
            <Segment.Inline>
                <Button as={Link} to='/'>
                    Return to Home
                </Button>
            </Segment.Inline>
        </Segment>
    )
}