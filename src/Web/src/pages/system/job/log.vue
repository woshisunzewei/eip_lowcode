<template>
    <a-drawer :title="title" placement="right" :closable="true" :width="1080" :visible="visible"
        :bodyStyle="{ padding: '1px' }" @close="close" :destroyOnClose="true">
        <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
            <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
        </a-card>

        <a-card class="eip-admin-card-small" :bordered="false">
            <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
            </vxe-toolbar>
            <vxe-table :loading="table.loading" ref="table" id="systemlogoperation"
                :column-config="{ isCurrent: true, isHover: true }" :row-config="{ isCurrent: true, isHover: true }"
                :seq-config="{
                    startIndex: (table.page.param.current - 1) * table.page.param.size,
                }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data">
                <template #loading>
                    <a-spin></a-spin>
                </template>
                <template #empty>
                    <eip-empty />
                </template>
                <vxe-column type="seq" title="序号" width="60"></vxe-column>
                <vxe-column field="createTime" title="时间" width="160"></vxe-column>
                <vxe-column field="message" title="内容" min-width="120" showOverflow="ellipsis"></vxe-column>
            </vxe-table>
            <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
                show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
                :page-size="table.page.param.size" :total="table.page.total" @change="tableInit"
                @showSizeChange="tableSizeChange"></a-pagination>
        </a-card>
    </a-drawer>
</template>
<script>
import { query } from "@/services/system/job/log";
export default {
    name: "smsdetail",
    data() {
        return {
            table: {
                page: {
                    param: {
                        current: 1,
                        size: this.eipPage.size,
                        sord: "desc",
                        sidx: "",
                        filters: "",
                        correlation: this.correlation
                    },
                    total: 0,
                    sizeOptions: this.eipPage.sizeOptions,
                },
                loading: true,
                data: [],
                height: window.innerHeight - 212,

                search: {
                    option: {
                        num: 4,
                        config: [
                            {
                                field: "message",
                                op: "cn",
                                placeholder: "请输入内容",
                                title: "内容",
                                value: "",
                                type: "text",
                            },
                            {
                                field: "createTime",
                                op: "daterange",
                                placeholder: "请选择时间",
                                title: "时间",
                                open: false,
                                value: [],
                                type: "datetime",
                                options: {
                                    mode: 'date',
                                    format: 'YYYY-MM-DD HH:mm:ss'
                                }
                            }
                        ],
                    },
                },
            },
        };
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        title: {
            type: String,
        },
        correlation: {
            type: String,
        },
    },
    mounted() {
        this.tableInit();
    },

    methods: {
        close() {
            this.$emit("update:visible", false);
        },

        /**
         * 根据Id查找
         */
        tableInit() {
            let that = this;
            that.table.loading = true;
            query(that.table.page.param).then(function (result) {
                if (result.code == that.eipResultCode.success) {
                    that.table.data = result.data;
                    that.table.page.total = result.total;
                }
                that.table.loading = false;
            });
        },

        /**
    * 列表排序改变
    */
        tableSort({ sortList }) {
            this.table.page.param.current = 1;
            this.table.page.param.sidx = sortList[0].property;
            this.table.page.param.sord = sortList[0].order;
            this.tableInit();
        },
        /**
         *数量改变
         */
        tableSizeChange(current, pageSize) {
            this.table.page.param.size = pageSize;
            this.tableInit();
        },
        /**
         * 列表查询
         */
        tableSearch(filters) {
            this.table.page.param.current = 1;
            this.table.page.param.filters = filters;
            this.tableInit();
        },
    },
};
</script>
<style scoped></style>