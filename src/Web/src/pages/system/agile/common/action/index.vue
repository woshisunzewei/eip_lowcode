<template>
    <a-form-model :label-col="labelCol" :wrapper-col="wrapperCol">
        <a-form-model-item label="双击查看详情">
            <a-switch v-model="config.cellDbClick" />
        </a-form-model-item>
    </a-form-model>
</template>
<script>
export default {
    data() {
        return {
            labelCol: { span: 2 },
            wrapperCol: { span: 14 },
        };
    },
    props: {
        config: {
            type: Object,
            default: () => {
                return {
                    cellDbClick: false, //双击
                };
            },
        },
    },
    methods: {},
};
</script>