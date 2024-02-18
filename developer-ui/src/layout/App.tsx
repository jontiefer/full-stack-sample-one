import { Container } from 'semantic-ui-react';
import NavBar from "./NavBar"
import { observer } from 'mobx-react-lite';
import { Outlet, ScrollRestoration, useLocation } from 'react-router-dom';
import HomeForm from "../forms/home/HomeForm";

const App = observer(() => {
  const location = useLocation();
  
  return (
    <>
      <ScrollRestoration />      
      {location.pathname === '/' ? <HomeForm /> : (
        <>
          <NavBar />
          <Container style={{ marginTop: '7em' }}>
            <Outlet />
          </Container>
        </>
      )}
    </>
  );
})

export default App;
