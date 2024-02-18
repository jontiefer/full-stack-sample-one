import {Container, Dropdown, Menu} from "semantic-ui-react";
import { NavLink } from "react-router-dom";
import { useAppStore } from "../stores/appStore";
import { observer } from "mobx-react-lite";

// eslint-disable-next-line react-refresh/only-export-components
export default observer(function NavBar() {
    const {authStore: {user, logout}} = useAppStore();

    return (
        <Menu inverted fixed='top'>
            <Container>
                <Menu.Item as={NavLink} to='/' header>
                    <img src='/assets/applogo.png' alt='logo' style={{marginRight: 10}}/>
                    Sample Application
                </Menu.Item>
                {user && (
                    <><Menu.Item as={NavLink} to='/dashboard' name='Dashboard' />
                    <Menu.Item position='right'>
                        <Dropdown pointing='top left' text={user?.username}>
                            <Dropdown.Menu>
                                <Dropdown.Item onClick={logout} text='Logout' icon='power' />
                            </Dropdown.Menu>
                        </Dropdown>
                    </Menu.Item></>
                )}                
            </Container>
        </Menu>
    )
})