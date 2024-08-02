<template>
    <div>
        <a-card class="eip-admin-card-small" :bordered="false">
            <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
                <template v-slot:buttons>
                    <a-space>
                        <a-input-search allow-clear v-model="table.key" placeholder="请输入名称" style="width: 200px" />
                        <eip-toolbar @del="del"></eip-toolbar>
                    </a-space>
                </template>
            </vxe-toolbar>
            <vxe-table :loading="table.loading" ref="table" id="workflowbuttonlist" size="small" :height="table.height"
                :export-config="{}" :print-config="{}" :data="onlines">
                <template #loading>
                    <a-spin></a-spin>
                </template>
                <template #empty>
                    <eip-empty />
                </template>
                <vxe-column type="checkbox" width="40" align="center" fixed="left">
                    <template #header="{ checked, indeterminate }">
                        <span @click.stop="$refs.table.toggleAllCheckboxRow()">
                            <a-checkbox :indeterminate="indeterminate" :checked="checked">
                            </a-checkbox>
                        </span>
                    </template>
                    <template #checkbox="{ row, checked, indeterminate }">
                        <span @click.stop="$refs.table.toggleCheckboxRow(row)">
                            <a-checkbox :indeterminate="indeterminate" :checked="checked">
                            </a-checkbox>
                        </span>
                    </template>
                </vxe-column>
                <vxe-column type="seq" title="序号" width="60"></vxe-column>
                <vxe-column field="name" title="名称" width="150">
                </vxe-column>
                <vxe-column field="organizationName" title="组织" width="150" showOverflow="ellipsis">
                </vxe-column>
                <vxe-column field="connectionId" title="连接Id" width="250"></vxe-column>
                <vxe-column field="remoteIp" title="客户端IP" width="120"></vxe-column>
                <vxe-column field="remoteIpAddress" title="归属地址" width="150" showOverflow="ellipsis"></vxe-column>
                <vxe-column field="userAgent" title="浏览器代理" min-width="200" showOverflow="ellipsis"></vxe-column>
                <vxe-column field="osInfo" title="操作系统" width="100"></vxe-column>
                <vxe-column field="browser" title="浏览器" width="100"></vxe-column>
                <vxe-column field="goOnlineTime" title="上线时间" width="160"></vxe-column>
                <vxe-column title="操作" align="center" fixed="right" width="160px">
                    <template #default="{ row }">
                        <a-tooltip title="下线" v-if="visible.del" @click="tableDelRow(row)">
                            <label class="text-red eip-cursor-pointer">下线</label>
                        </a-tooltip>
                    </template>
                </vxe-column>
            </vxe-table>
        </a-card>
    </div>
</template>

<script>
import {
    online,
    kickout
} from "@/services/system/monitor/onlineuser";


import { selectTableRow, deleteConfirm } from "@/utils/util";
import { mapGetters } from "vuex";
export default {
    data() {
        return {
            table: {
                loading: true,
                data: [],
                key: '',
                height: this.eipHeaderHeight() - 68 + "px"
            },
            visible: {
                del: true,
            },

        };
    },
    created() {
        this.tableInit();
    },
    computed: {
        ...mapGetters("account", ["user"]),
        onlines() {
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
            online().then(function (result) {
                if (result.code == that.eipResultCode.success) {
                    that.table.data = result.data;
                }
                that.table.loading = false;
            });
        },

        /**
         * 删除
         */
        tableDelRow(row) {
            let that = this;
            deleteConfirm(
                "下线【" + row.name + "】" + that.eipMsg.delete,
                function () {
                    that.$loading.show({ text: '下线中...' });
                    kickout({ id: row.connectionId }).then((result) => {
                        that.$loading.hide();
                        if (result.code == that.eipResultCode.success) {
                            //判断踢下线及当前用户的LoginId是否一致
                            var loginId = that.user.loginId;
                            if (loginId != row.loginId) {
                                that.tableInit();
                                that.$message.success(result.msg);
                            }
                        } else {
                            that.$message.error(result.msg);
                        }
                    });
                },
                that
            );
        },


        /**
         * 删除
         */
        del() {
            let that = this;
            selectTableRow(
                that.$refs.table,
                function (rows) {
                    //提示是否删除
                    deleteConfirm(
                        '确认将选中用户批量下线',
                        function () {
                            //加载提示
                            that.$loading.show({ text: '下线中...' });
                            let ids = that.$utils.map(rows, (item) => item.connectionId);
                            kickout({ id: ids.join(",") }).then((result) => {
                                that.$loading.hide();
                                if (result.code == that.eipResultCode.success) {
                                    that.tableInit();
                                    that.$message.success(result.msg);
                                } else {
                                    that.$message.error(result.msg);
                                }
                            });
                        },
                        that
                    );
                },
                that,
                false
            );
        },

        /**
         * 权限按钮加载完毕
         */
        toolbarOnload(buttons) {
            this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
        },
    },
};
</script>