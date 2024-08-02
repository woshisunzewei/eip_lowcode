<template>
    <div class="getSingleData">
        <a-form-model layout="horizontal" :model="drawerData">
            <a-form-model-item label="开始时间">
                <a-date-picker style="width:100%" v-model="drawerData.beginTime" :disabled-date="disabledStartDate"
                    show-time allow-clear format="YYYY-MM-DD HH:mm:ss" placeholder="请选择开始时间"
                    @openChange="handleStartOpenChange" />

            </a-form-model-item>
            <a-form-model-item label="结束时间">
                <a-date-picker style="width:100%" v-model="drawerData.endTime" :disabled-date="disabledEndDate"
                    show-time format="YYYY-MM-DD HH:mm:ss" placeholder="请选择结束时间" :open="endOpen"
                    @openChange="handleEndOpenChange" />

            </a-form-model-item>
            <a-form-model-item label="循环Cron">
                <a-input placeholder="请输入循环Cron" v-model="drawerData.cron">
                    <a-icon slot="suffix" @click="openModal" type="search" />
                </a-input>
            </a-form-model-item>
        </a-form-model>

        <eip-cron ref="innerVueCron" :expression="drawerData.cron" @ok="handleOK"></eip-cron>
    </div>
</template>

<script>

export default {
    name: "timeTrigger",
    components: {
    },
    props: {
        drawerData: {
            required: true,
            type: Object
        }
    },
    data() {
        return {
            endOpen: false
        }
    },
    watch: {
        value(val) {
            this.drawerData.cron = val
        }
    },
    mounted() {
    },
    methods: {
        openModal() {
            this.$refs.innerVueCron.show()
        },
        handleOK(val) {
            this.drawerData.cron = val
        },
        handleEmpty() {
            this.handleOK('')
        },

        disabledStartDate(startValue) {
            const endValue = this.drawerData.endTime;
            if (!startValue || !endValue) {
                return false;
            }
            return startValue.valueOf() > endValue.valueOf();
        },
        disabledEndDate(endValue) {
            const startValue = this.drawerData.beginTime;
            if (!endValue || !startValue) {
                return false;
            }
            return startValue.valueOf() >= endValue.valueOf();
        },
        handleStartOpenChange(open) {
            if (!open) {
                this.endOpen = true;
            }
        },
        handleEndOpenChange(open) {
            this.endOpen = open;
        },

    }
}
</script>

<style scoped>
/deep/.ant-form-item-label>label {
    font-weight: bold;
}

/deep/.ant-input[disabled] {
    border: 1px solid #d9d9d9 !important;
}
</style>