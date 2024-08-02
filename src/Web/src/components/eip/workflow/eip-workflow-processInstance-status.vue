<template>
    <a-tag v-if="status == 0" color="#108ee9">
        处理中
    </a-tag>
    <a-tag v-else-if="status == 2" color="#2db7f5">
        终止
    </a-tag>
    <a-tag v-else-if="status == 4" color="#c21401">
        暂停
    </a-tag>
    <a-tag v-else-if="status == 6" color="#90cf5b">
        即将到期
    </a-tag>
    <a-tag v-else-if="status == 8" color="#00af57">
        拒绝
    </a-tag>
    <a-tag v-else-if="status == 10" color="#ff1e02">
        撤销
    </a-tag>
    <a-tag v-else-if="status == 12" color="#0071be">
        删除
    </a-tag>
    <a-tag v-else-if="status == 14" color="#00215f">
        退回
    </a-tag>
    <a-tag v-else-if="status == 16" color="#72349d">
        取消
    </a-tag>
    <a-tag v-else-if="status == 100" color="#2db7f5">
        完成
    </a-tag>
    <a-tag v-else color="#2db7f5">
    </a-tag>
</template>

<script>

export default {
    name: 'eip-workflow-processInstance-status',
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