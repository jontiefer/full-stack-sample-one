import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

interface AuthFailureProps {
    statusCode: 401 | 403;
}

export default function AuthFailure({ statusCode }: AuthFailureProps ) {
    return (
        <Segment placeholder>
            <Header icon>
                <Icon name='exclamation circle' /> 
                {statusCode == 401 ?
                 "Invalid Credentials.  You Are Not Authorized To View This Page. (401)" :
                 "You Are Forbidden From Viewing This Page (403)."}
            </Header>
            <Segment.Inline>
                <Button as={Link} to='/'>
                    Return to Home
                </Button>
            </Segment.Inline>
        </Segment>
    )
}