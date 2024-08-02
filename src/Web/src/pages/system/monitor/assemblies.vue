<template>
    <div>
        <a-card class="eip-admin-card-small" :bordered="false">
            <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
                <template v-slot:buttons>
                    <a-space>
                        <a-input-search allow-clear v-model="table.key" placeholder="请输入名称" style="width: 200px" />
                    </a-space>
                </template>
            </vxe-toolbar>
            <vxe-table :loading="table.loading" ref="table" id="workflowbuttonlist" size="small" :height="table.height"
                :export-config="{}" :print-config="{}" :data="assemblies">
                <template #loading>
                    <a-spin></a-spin>
                </template>
                <template #empty>
                    <eip-empty />
                </template>
                <vxe-column type="seq" title="序号" width="60"></vxe-column>
                <vxe-column field="name" title="名称" width="350">
                </vxe-column>
                <vxe-column field="version" title="版本号" width="150">
                </vxe-column>
                <vxe-column field="clrVersion" title="Clr运行时" width="150"></vxe-column>
                <vxe-column field="location" title="位置路径"></vxe-column>
            </vxe-table>
        </a-card>
    </div>
</template>
  
<script>
import {
    query
} from "@/services/system/monitor/assemblies";
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
        assemblies() {
            var arr = [];
            this.table.data.forEach((item) => arr.push(item));
            if (this.table.key) {
                arr = arr.filter(
                    (item) =>
                        item.name.includes(this.table.key)
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