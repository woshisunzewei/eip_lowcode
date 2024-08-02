<template>
    <a-modal width="370px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
        :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">

        <a-spin :spinning="tree.spinning" :style="tree.style">
            <a-row>
                <a-col>
                    <a-directory-tree v-if="!tree.spinning"
                        :defaultExpandedKeys="[tree.data.length > 0 ? tree.data[0].key : '']" :style="tree.style"
                        :expandAction="false" :tree-data="tree.data" :icon="treeIcon"
                        :defaultCheckedKeys="tree.checkedKeys" :checkedKeys="tree.checkedKeys" @select="onCheck">
                    </a-directory-tree>

                    <eip-empty :style="tree.style" v-if="tree.data.length == 0" description="无模块信息" />
                </a-col>
            </a-row>
        </a-spin>
        <template slot="footer">
            <a-space>
                <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
                <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon
                        type="save" />提交</a-button>
            </a-space>
        </template>
    </a-modal>
</template>

<script>
import {
    menuQuery,
} from "@/services/system/menu/list";
import {
    saveMove,
} from "@/services/system/menu/edit";
export default {
    name: "menu-move",
    data() {
        return {
            tree: {
                style: {
                    overflow: "auto",
                    height: "500px",
                },
                data: [],
                spinning: false,
                checkedKeys: [],
            },

            loading: false,
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
        menuId: {
            type: String,
        }
    },
    mounted() {
        this.menuInit();
    },
    methods: {

        /**
         * 树图标
         */
        treeIcon(props) {
            const { slots } = props;
            return <a-icon type={slots.icon} />;
        },

        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },

        /**
         * 初始化菜单
         */
        menuInit() {
            let that = this;
            that.tree.spinning = true;
            menuQuery(this.menuId, true).then((result) => {
                that.tree.data = result.data;
                that.tree.spinning = false;
            });
        },

        /**
         * 选择
         */
        onCheck(checkedKeys, event) {
            this.tree.checkedKeys = [];
            checkedKeys.forEach((item) => {
                this.tree.checkedKeys.push(item);
            });

        },

        /**
         * 保存
         */
        save() {
            let that = this;
            var checkedKeys = this.tree.checkedKeys;
            if (checkedKeys.length == 0) {
                this.$message.error('请选择需要移动的菜单')
                return;
            }
            that.loading = true;
            that.$loading.show({ text: that.eipMsg.saveLoading });
            saveMove({
                menuId: this.menuId,
                parentId: this.tree.checkedKeys[0]
            }).then(function (result) {
                if (result.code === that.eipResultCode.success) {
                    that.$message.success(result.msg);
                    that.cancel();
                    that.$emit("save", result);
                } else {
                    that.$message.error(result.msg);
                }
                that.loading = false;
                that.$loading.hide();
            });
        },
    },
};
</script>