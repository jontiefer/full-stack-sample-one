import { ErrorMessage, Form, Formik } from "formik";
import { observer } from "mobx-react-lite";
import { Button, Header } from "semantic-ui-react";
import TextInput from "../../controls/input/TextInput";
import { useAppStore } from "../../stores/appStore"
import * as Yup from 'yup';
import ValidationError from "../errors/ValidationErrors";


const RegisterForm = observer(() => {
    const { authStore } = useAppStore();
    return (
        <Formik
            initialValues={{ username: '', password: '', error: null }}
            onSubmit={(values, { setErrors }) =>
                authStore.register(values).catch(error => setErrors({ error: error }))}
            validationSchema={Yup.object({                
                username: Yup.string().required(),                
                password: Yup.string().required(),
            })}
        >
            {({ handleSubmit, isSubmitting, errors, isValid, dirty }) => (
                <Form className='ui form error' onSubmit={handleSubmit} autoComplete='off'>
                    <Header as='h2' content='Sign Up For Sample Application' color="teal" textAlign="center" />                    
                    <TextInput placeholder="User Name" name='username' />                    
                    <TextInput placeholder="Password" name='password' type='password' />                    
                    <ErrorMessage name='error' render={() => 
                        <ValidationError errors={[errors.error]} />} />
                    <Button
                        disabled={!isValid || !dirty || isSubmitting} 
                        loading={isSubmitting} 
                        positive content='Register' 
                        type="submit" fluid 
                    />
                </Form>
            )}
        </Formik>
    )
})

export default RegisterForm;