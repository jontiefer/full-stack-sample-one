import { ErrorMessage, Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { Button, Header, Label, Segment } from "semantic-ui-react";
import TextInput from "../../controls/input/TextInput";
import { useAppStore } from "../../stores/appStore";
import LoginAttemptList from "../../controls/auth/LoginAttemptList";

const LoginForm = observer(() => {
    const { authStore } = useAppStore();

    return (
        <Formik
            initialValues={{ username: '', password: '', error: null }}
            onSubmit={(values, { setErrors }) =>
                authStore.login(values).catch(() => setErrors({ error: 'Invalid User Name or Password' }))}
        >
            {({ handleSubmit, isSubmitting, errors }) => (
                <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Login to Sample Application' color="teal" textAlign="center" />
                    <TextInput placeholder="User Name" name='username' />
                    <TextInput placeholder="Password" name='password' type='password' />
                    <ErrorMessage name='error' render={() => 
                        <Label style={{ marginBottom: 10 }} basic color='red' content={errors.error} />} />                    
                    <Button loading={isSubmitting} positive content='Login' type="submit" fluid />                    
                    <Segment basic style={{ marginTop: '20px' }} >
                        <LoginAttemptList />
                    </Segment>                    
                </Form>
            )}
        </Formik>
    )
})

export default LoginForm;