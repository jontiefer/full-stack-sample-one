import { observer } from 'mobx-react-lite';
import { Link } from 'react-router-dom';
import { Button, Container, Header, Segment, Image } from "semantic-ui-react";
import { useAppStore } from '../../stores/appStore';
import axios from 'axios';

const HomeForm = observer(() => {
    const { authStore } = useAppStore();

    return (
        <Segment inverted textAlign='center' vertical className='masthead' >
            <Container text>
                <Header as='h1' inverted>
                    <Image size='massive' src='/assets/applogo.png' alt='logo' style={{ marginBottom: 12 }} />
                    Sample Application - {axios.defaults.baseURL}
                </Header>
                {authStore.isLoggedIn ? (
                    <>
                        <Header as='h2' inverted content={`Welcome back, ${authStore.user!.username}!`} />
                        <Button as={Link} to='/dashboard' size='huge' inverted>
                            User Dashboard
                        </Button>
                    </>
                ) : (
                    <>
                        <Button as={Link} to='/login' size='huge' inverted>
                            Login
                        </Button>
                        <Button as={Link} to='/register' size='huge' inverted>
                            Register
                        </Button>
                    </>
                )}
            </Container>
        </Segment>
    )
})

export default HomeForm;