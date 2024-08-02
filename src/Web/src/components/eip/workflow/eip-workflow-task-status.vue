<template>
    <a-tag v-if="status == 2" color="#2db7f5">
        等待
    </a-tag>
    <a-tag v-else-if="status == 4" color="#108ee9">
        处理中
    </a-tag>
    <a-tag v-else-if="status == 8" color="#c21401">
        暂停
    </a-tag>
    <a-tag v-else-if="status == 10" color="#c21401">
        撤销
    </a-tag>
    <a-tag v-else-if="status == 12" color="#90cf5b">
        退回
    </a-tag>
    <a-tag v-else-if="status == 14" color="#00af57">
        取消
    </a-tag>
    <a-tag v-else-if="status == 16" color="#ff1e02">
        拒绝
    </a-tag>
    <a-tag v-else-if="status == 18" color="#0071be">
        知会已阅
    </a-tag>
    <a-tag v-else-if="status == 20" color="#00215f">
        阅示已阅
    </a-tag>
    <a-tag v-else-if="status == 22" color="#72349d">
        召回
    </a-tag>
    <a-tag v-else-if="status == 24" color="#e2534d">
        删除
    </a-tag>
    <a-tag v-else-if="status == 24" color="#e2534d">
        即将超时
    </a-tag>
    <a-tag v-else-if="status == 30" color="#e2534d">
        阅示已阅待审核
    </a-tag>
    <a-tag v-else-if="status == 100" color="#2db7f5">
        完成
    </a-tag>
    <a-tag v-else color="#2db7f5">
    </a-tag>
</template>

<script>

export default {
    name: 'eip-workflow-status',
    props: {
        status: {
            type: Number
        },
    },
    data() {
        return {

        };
    },

    mounted() {

    },
    methods: {

    },
};
</script>
<style scoped></style>