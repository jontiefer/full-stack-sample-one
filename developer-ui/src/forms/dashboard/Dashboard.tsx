import { Card, Header, Segment } from "semantic-ui-react";
import { useAppStore } from "../../stores/appStore";
import { observer } from "mobx-react-lite";
import { useEffect } from "react";

const Dashboard: React.FC = observer(() => {
    const { authStore, testDataStore } = useAppStore();
    const userName = authStore.user!.username ;

    useEffect(() => {
        testDataStore.loadTestData();
    }, [testDataStore]);

    return (
        <Segment>            
            <div style={{ display: 'flex', alignItems: 'center', marginBottom: '20px' }}>            
                <img src='/assets/dashboard.png' alt="Dashboard" style={{ width: '128px', height: '128px', marginRight: '20px' }} />
                <Header as='h1' size='huge'>
                    Welcome to your Dashboard, {userName}!!
                </Header>
            </div>
            
            <Card.Group itemsPerRow={3}>
                {testDataStore.testData.map(item => (
                    <Card key={item.testId}>
                        <Card.Content>
                            <Card.Header>{item.testName}</Card.Header>
                            <Card.Description>
                                <strong>Test ID:</strong> {item.testId}<br />
                                <strong>Test Info:</strong> {item.testInfo}
                            </Card.Description>
                        </Card.Content>
                    </Card>
                ))}
            </Card.Group>
        </Segment>
    )
});

export default Dashboard;