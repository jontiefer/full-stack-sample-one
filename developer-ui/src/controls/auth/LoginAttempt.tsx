// LoginAttempt.tsx
import { observer } from 'mobx-react-lite';
import { List } from 'semantic-ui-react';
import styles from './LoginAttempt.module.css';

interface LoginAttemptProps {
    attempt: {
        ip: string;
        timestamp: Date;
    }
}

const LoginAttempt: React.FC<LoginAttemptProps> = observer(({ attempt }) => {    
    return (
        <List.Item className={styles.loginAttemptsItem}>
            <List.Content>
                <List.Description>Login Attempt from {attempt.ip} at {attempt.timestamp.toLocaleString()}</List.Description>
            </List.Content>
        </List.Item>
    );
});

export default LoginAttempt;
