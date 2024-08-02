<template>
    <a-modal width="600px" v-drag centered :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
        :bodyStyle="{ padding: '1px' }" @cancel="cancel">
        <vxe-table ref="batchTable" size="small" :height="height" :data="button">
            <template #loading>
                <a-spin></a-spin>
            </template>
            <template #empty>
                <eip-empty />
            </template>
            <vxe-column type="seq" align="center" title="序号" width="60"></vxe-column>
            <vxe-column field="label" title="按钮名称" width="380" showOverflow="ellipsis">
                <template v-slot="{ row }">
                    <a-button :type="row.type"> <a-icon :type="row.icon" /> {{ row.label }}</a-button>
                </template>
            </vxe-column>
            <vxe-column field="isShow" title="显示" align="center" width="80">
                <template v-slot="{ row }">
                    <a-switch :checked="row.isShow" @change="row.isShow = !row.isShow" />
                </template>
            </vxe-column>
        </vxe-table>
        <template slot="footer">
            <a-space>
                <a-button key="back" @click="cancel"><a-icon type="close" />取消</a-button>
                <a-button key="submit" @click="save" type="primary"><a-icon type="save" />提交</a-button>
            </a-space>
        </template>
    </a-modal>
</template>
<script>
export default {
    name: "eip-workflow-batch-button",
    data() {
        return {
            height: this.eipHeaderHeight() - 22 + "px",
        };
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        button: {
            type: Array,
        },
        title: {
            type: String,
        },
    },
    mounted() {
    },
    methods: {
        /**
         * 保存
         */
        save() {
            this.$emit("ok", this.button);
            this.cancel();
        },

        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },
    },
};
</script>