// LoginAttemptList.tsx
import { observer } from 'mobx-react-lite';
import { useAppStore } from '../../stores/appStore';
import LoginAttempt from './LoginAttempt';
import { Header, List } from 'semantic-ui-react';
import styles from './LoginAttempt.module.css';

const LoginAttemptList = observer(() => {
  const { authStore } = useAppStore();

  return (    
    <div className={styles.loginAttemptsList}>
      {authStore.loginAttempts.length > 0 && (
        <>
            <Header as='h3' style={{color: 'purple'}}>
                <p className={styles.loginAttemptsHeader}>Login Attempts: {authStore.loginAttempts.length}</p>
            </Header>
            <List divided relaxed>
                {authStore.loginAttempts.map((attempt, index) => (
                <LoginAttempt key={index} attempt={attempt} />
                ))}
            </List>            
        </>
      )}
    </div>
  );
});

export default LoginAttemptList;
