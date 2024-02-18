import { observer } from "mobx-react-lite";
import { Container, Header, Segment } from "semantic-ui-react";
import { useAppStore } from "../../stores/appStore";

interface RequestErrorProps {
    statusCode: 400 | 422 | 500;
}
// eslint-disable-next-line react-refresh/only-export-components
export default observer(function RequestError({ statusCode }: RequestErrorProps) {
    const {generalStore} = useAppStore();
    
    const getErrorTitle = (statusCode: number): string => {
        switch(statusCode) {
            case 400:
                return "Bad Request (400)";
            case 422:
                return "Unprocessable Entity (422)";
            case 500:
                return "Internal Server Error (500)";
            default:
                return "Unknown Error";
        }
    };

    return (
        <Container>
            <Header as='h1' content={getErrorTitle(statusCode)} />
            <Header sub as='h5' color="red" content={generalStore.error?.message} />
            {generalStore.error?.details && (
                <Segment>
                    <Header as='h4' content='Error Details' color="teal" />
                    <code style={{marginTop: '10px'}}>{generalStore.error.details}</code>
                </Segment>
            )}
        </Container>
    )
})