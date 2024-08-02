<template>
    <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
        :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
        <a-spin :spinning="spinning">
            <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
                :wrapper-col="config.wrapperCol">
                <a-form-model-item label="名称" prop="name">
                    <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
                </a-form-model-item>
                <a-form-model-item label="图标" prop="icon">
                    <eip-icon :name="form.icon" :theme="form.theme" @click="iconClick" @clear="iconClear"></eip-icon>
                </a-form-model-item>
                <a-form-model-item label="备注" prop="remark">
                    <a-input v-model="form.remark" type="textarea" placeholder="请输入备注" />
                </a-form-model-item>

                <a-form-model-item label="禁用">
                    <a-switch v-model="form.isFreeze" />
                </a-form-model-item>

                <a-form-model-item label="排序号" prop="orderNo">
                    <a-input-number id="inputNumber" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo"
                        :min="0" :max="999" />
                </a-form-model-item>
            </a-form-model>
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
import { save, findById } from "@/services/system/menu/edit";
import {
    agileSave,
} from "@/services/system/agile/app/worksheet";
import { newGuid } from "@/utils/util";
export default {
    name: "custom",
    data() {
        return {
            config: {
                labelCol: { span: 4 },
                wrapperCol: { span: 20 },
            },
            form: {
                menuId: newGuid(),
                parentId: this.parentId,
                parentName: this.parentName,
                name: null,
                icon: null,
                theme: null,
                openType: 0,
                path: null,
                router: "agileappbuild",
                isShowMenu: true,
                haveMenuPermission: true,
                haveButtonPermission: true,
                haveDataPermission: false,
                haveFieldPermission: false,
                isFreeze: false,
                orderNo: 1,
                params: null,
                remark: null,
                isAgileMenu: true,
                agileMenuType: this.eipAgileMenuType.portal,
                typeId: undefined
            },
            rules: {
                icon: [
                    {
                        required: true,
                        message: "请选择图标",
                        trigger: "blur",
                    },
                ],
                name: [
                    {
                        required: true,
                        message: "请输入名称",
                        trigger: "blur",
                    },
                ],
            },

            loading: false,
            spinning: false,
        };
    },

    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        data: {
            type: Array,
        },
        title: {
            type: String,
        },
        copy: {
            type: Boolean,
            default: false,
        },
        menuId: {
            type: String,
        },
        parentId: {
            type: String,
        },
        parentName: {
            type: String,
        },
    },

    mounted() {
        this.find();
    },

    methods: {
        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },

        /**
         * 保存
         */
        save() {
            let that = this;
            this.$refs.form.validate(async (valid) => {
                if (valid) {
                    that.loading = true;
                    that.spinning = true;
                    that.form.menuId = that.copy ? newGuid() : that.form.menuId;
                    that.form.path = "/appbuild/" + that.form.menuId;
                    var result = await save(that.form);
                    if (result.code !== that.eipResultCode.success) {
                        that.$message.error(result.msg);
                    } else {
                        that.agileSaveForm = {
                            configId: newGuid(),
                            configType: 4,
                            name: that.form.name,
                            orderNo: 1,
                            isFreeze: false,
                            remark: that.form.name,
                            menuId: that.form.menuId
                        };
                        result = await agileSave(that.agileSaveForm);
                        if (result.code === that.eipResultCode.success) {
                            that.$message.success(result.msg);
                            that.cancel();
                            that.$emit("save", result);
                        } else {
                            that.$message.error(result.msg);
                        }
                    }

                    that.loading = false;
                    that.spinning = false;
                } else {
                    return false;
                }
            });
        },

        /**
         * 根据Id查找
         */
        find() {
            let that = this;
            if (this.menuId) {
                that.spinning = true;
                findById(this.menuId).then(function (result) {
                    if (that.copy) {
                        result.data.orderNo += 1;
                    }
                    let form = result.data;
                    that.form = form;
                    that.form.parentId =
                        form.parentName == "" ? undefined : form.parentId;
                    that.spinning = false;
                });
            }
        },

        /**
         *图标点击
         */
        iconClick(icon) {
            this.form.icon = icon.name;
            this.form.theme = icon.theme;
        },

        /**
         * 清空图标
         */
        iconClear() {
            this.form.icon = null;
            this.form.theme = null;
        },
    },
};
</script>