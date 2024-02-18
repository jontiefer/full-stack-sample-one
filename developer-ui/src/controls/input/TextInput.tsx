import {useField} from "formik";
import {Form, Label} from "semantic-ui-react";

interface TextInputProps {
    placeholder: string;
    name: string;
    label?: string;
    type?: string;
}

export default function TextInput(props: TextInputProps) {
    const [field, meta] = useField(props.name);

    return (
        <Form.Field error={meta.touched && !!meta.error}>
            <label>{props.label}</label>
            <input {...field} {...props}/>
            {meta.touched && meta.error ? (
                <Label basic color='red'>{meta.error}</Label>
            ) : null}
        </Form.Field>
    )
}
