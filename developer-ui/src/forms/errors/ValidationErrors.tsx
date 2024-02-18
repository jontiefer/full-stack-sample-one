import { Message } from "semantic-ui-react";

interface Props {
    errors?: (string | undefined)[]
}

export default function ValidationError({errors}: Props) {
    return (
        <Message error>
            {errors && (
                <Message.List>
                    {errors.map((err: string | undefined, i: any) => (
                        err && <Message.Item key={i}>{err}</Message.Item>
                    ))}
                </Message.List>
            )}
        </Message>
    )
}