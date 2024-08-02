<template>
    <div>
        <a-card class="eip-admin-card-small" :bordered="false">
            <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
                <template v-slot:buttons>
                    <a-space>
                        <a-input-search allow-clear v-model="table.key" placeholder="请输入控制器/方法/路径/描述/开发者"
                            style="width: 400px" />
                    </a-space>
                </template>
            </vxe-toolbar>
            <vxe-table :loading="table.loading" ref="table" id="workflowbuttonlist" size="small" :height="table.height"
                :export-config="{}" :print-config="{}" :data="api">
                <template #loading>
                    <a-spin></a-spin>
                </template>
                <template #empty>
                    <eip-empty />
                </template>
                <vxe-column type="seq" title="序号" width="60"></vxe-column>
                <vxe-column field="controller" title="控制器" width="200">
                </vxe-column>
                <vxe-column field="action" title="方法" width="250">
                </vxe-column>
                <vxe-column field="url" title="路径">
                </vxe-column>
                <vxe-column field="description" title="描述" width="350"></vxe-column>
                <vxe-column field="byDeveloperCode" title="开发者" width="120"></vxe-column>
                <vxe-column field="byDeveloperTime" title="开发时间" width="160"></vxe-column>
            </vxe-table>
        </a-card>
    </div>
</template>

<script>
import {
    query
} from "@/services/system/monitor/api";
export default {
    data() {
        return {
            table: {
                loading: true,
                data: [],
                key: '',
                height: this.eipHeaderHeight() - 68 + "px"
            }
        };
    },
    created() {
        this.tableInit();
    },
    computed: {
        api() {
            var arr = [];
            this.table.data.forEach((item) => arr.push(item));
            if (this.table.key) {
                arr = arr.filter(
                    (item) =>
                        item.controller.includes(this.table.key) ||
                        item.action.includes(this.table.key) ||
                        item.url.includes(this.table.key) ||
                        item.description.includes(this.table.key) ||
                        item.byDeveloperCode.includes(this.table.key)
                );
            }
            return arr;
        },
    },
    mounted() {
        this.$refs.table.connect(this.$refs.toolbar);
    },
    methods: {
        /**
         * 
         */
        tableInit() {
            let that = this;
            that.table.loading = true;
            query().then(function (result) {
                if (result.code == that.eipResultCode.success) {
                    that.table.data = result.data;
                }
                that.table.loading = false;
            });
        }
    },
};
</script>