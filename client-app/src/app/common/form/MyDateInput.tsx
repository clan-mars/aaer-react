import { useField } from "formik";
import React from "react";
import DatePicker, {ReactDatePickerProps} from "react-datepicker";
import { Form, Label } from "semantic-ui-react";

export default function MyDateInput(props: Partial<ReactDatePickerProps>) {
    const [field, meta, helpers] = useField(props.name!);
    return (
        <Form.Field error={meta.touched && !!meta.error}>
            <DatePicker {... props} {... field} selected={(field.value && new Date(field.value)) || null}
            onChange={value => helpers.setValue(value)}  nextMonthButtonLabel='>' previousMonthButtonLabel='<'
            />
            {meta.touched && meta.error ? (
                <Label basic color='red'>{meta.error}</Label>
            ): null}
        </Form.Field>
    )
}