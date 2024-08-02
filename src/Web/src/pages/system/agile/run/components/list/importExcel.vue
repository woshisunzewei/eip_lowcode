<template>
    <a-modal v-drag :zIndex="1001" :title="title" :visible="visible" @cancel="cancel" :destroyOnClose="true"
        :footer="null">
        <div style="text-align: center;">
            <a-space>
                <a-button type="danger" @click="importBusinessData(0)">
                    清空后导入
                </a-button>
                <a-button type="primary" @click="importBusinessData(1)">
                    累加导入
                </a-button>
            </a-space>
        </div>
    </a-modal>
</template>

<script>

export default {
    components: {},
    name: "import-excel",
    computed: {
    },
    data() {
        return {

        };
    },

    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        title: {
            type: String,
        }
    },

    mounted() {
    },
    methods: {

        /**
        * 取消
        */
        cancel() {
            this.$emit("update:visible", false);
        },

        /**
         * 导入数据
         * @param {*} type 
         */
        importBusinessData(type) {
            this.$emit("importBusinessData", type);
        }
    },
};
</script>

<style lang="less">
.keyword-lighten {
    color: #000;
    background-color: #FFFF00;
}
</style>