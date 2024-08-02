<template>
    <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
        :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
        <a-spin :spinning="spinning">
            <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
                :wrapper-col="config.wrapperCol">
                <a-form-model-item label="名称" prop="name">
                    <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
                </a-form-model-item>
                <a-form-model-item label="系统" prop="dataBaseId">
                    <a-select allow-clear v-model="form.dataBaseId" placeholder="请选择系统"> <a-select-option
                            v-for="(item, index) in dataBase" :value="item.dataBaseId" :key="index">
                            {{ item.name }} </a-select-option>
                    </a-select>
                </a-form-model-item>
                <a-form-model-item label="表名" prop="dataFromName" help="编辑不可调整">
                    <a-input v-model="form.dataFromName" placeholder="请输入表名" allow-clear>
                        <a-tooltip slot="addonAfter" @click="dataFromNameSet" title="点击生成拼音表名">
                            <a-icon type="font-colors" />
                        </a-tooltip>
                    </a-input>
                </a-form-model-item>
                <a-form-model-item label="打开类型" prop="openType">
                    <a-select allow-clear v-model="form.openType">
                        <a-select-option :value="0"> 当前页 </a-select-option>
                        <a-select-option :value="2"> 新窗口 </a-select-option>
                    </a-select>
                </a-form-model-item>
                <a-form-model-item label="手机端显示">
                    <a-switch v-model="form.isShowMobile" />
                </a-form-model-item>
                <a-form-model-item label="图标" prop="icon">
                    <eip-icon :name="form.icon" :theme="form.theme" @click="iconClick" @clear="iconClear"></eip-icon>
                </a-form-model-item>
                <a-form-model-item label="图片" prop="image">
                    <a-input v-model="form.image" placeholder="请输入图片地址" allow-clear>
                        <a-avatar slot="prefix" size="small" :src="form.image" v-if="form.image" />
                        <a-icon @click="material.visible = true" slot="addonAfter" type="setting" />
                    </a-input>
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
        <eip-material v-if="material.visible" :visible.sync="material.visible" @ok="materialOk" />
    </a-modal>
</template>

<script>
import { save, findById } from "@/services/system/menu/edit";
import {
    table,
    agileSave,
    findByMenuId
} from "@/services/system/agile/app/worksheet";
import { query } from "@/services/system/database/list";
import { newGuid } from "@/utils/util";
export default {
    name: "group",
    data() {
        return {
            material: {
                visible: false
            },
            config: {
                labelCol: { span: 4 },
                wrapperCol: { span: 20 },
            },
            form: {
                configId: newGuid(),
                menuId: newGuid(),
                parentId: this.parentId,
                parentName: this.parentName,
                name: null,
                icon: null,
                image: null,
                theme: null,
                openType: 0,
                path: null,
                router: "agileappbuild",
                isShowMenu: true,
                isShowMobile: true,
                haveMenuPermission: true,
                haveButtonPermission: true,
                haveDataPermission: false,
                haveFieldPermission: true,
                isFreeze: false,
                orderNo: 10,
                params: undefined,
                remark: null,
                isAgileMenu: true,
                agileMenuType: this.eipAgileMenuType.page,
                dataFromName: '',
                dataBaseId: undefined,
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
                dataBaseId: [
                    {
                        required: true,
                        message: "请选择数据库",
                        trigger: "blur",
                    }
                ],
                dataFromName: [
                    {
                        required: true,
                        message: "请输入表名",
                        trigger: "blur",
                    }
                ],
            },
            pages: [],
            loading: false,
            spinning: false,

            dataBase: []
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
        this.initDataBase();
        this.find();
    },

    methods: {
        /**
     * 初始化数据库
     */
        initDataBase() {
            let that = this;
            query().then(function (result) {
                if (result.code == that.eipResultCode.success) {
                    that.dataBase = result.data;
                }
                that.find();
            });
        },
        /**
            * 生成拼音
            */
        dataFromNameSet() {
            if (!this.menuId) {
                this.form.dataFromName = pinyin.convert(this.form.name);
            }
        },
        /**
     * 确认素材
     */
        materialOk(path) {
            this.form.image = path;
        },
        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },

        /**
         * 保存
         */
        async save() {
            let that = this;
            this.$refs.form.validate(async (valid) => {
                if (valid) {
                    that.loading = true;
                    that.spinning = true;
                    that.form.path = "/appbuild/" + that.form.menuId;
                    that.form.menuId = that.copy ? newGuid() : that.form.menuId;
                    var result = await save(that.form);
                    if (result.code !== that.eipResultCode.success) {
                        that.$message.error(result.msg);
                        return false;
                    }

                    if (!that.menuId) {
                        result = await that.saveList();
                        if (result.code != that.eipResultCode.success) {
                            that.$message.error(result.msg);
                            that.loading = false;
                            that.$loading.hide();
                        }
                    }
                    that.$message.success(result.msg);
                    that.cancel();
                    that.$emit("save", result);
                    that.loading = false;
                    that.spinning = false;
                } else {
                    return false;
                }
            });
        },

        /**
           * 列表配置
           */
        async saveList() {
            let that = this;
            that.listForm = {
                configId: that.form.configId,
                dataFrom: 0,
                dataBaseId: that.form.dataBaseId,
                dataFromName: that.form.dataFromName,
                name: that.form.name,
                orderNo: 1,
                configType: 2,
                isFreeze: false,
                remark: that.form.name,
                editConfigId: null,
                saveJson: null,
                publicJson: null,
                menuId: that.form.menuId,
                formCategory: 1
            };

            await agileSave(that.listForm);

            var result = await table(that.form);
            if (result.code != that.eipResultCode.success) {
                that.$message.error(result.msg);
                that.loading = false;
                that.$loading.hide();
            }

            return result
        },

        /**
         * 根据Id查找
         */
        async find() {
            let that = this;
            if (this.menuId) {
                that.spinning = true;
                var result = await findById(this.menuId);

                if (that.copy) {
                    result.data.orderNo += 1;
                }
                let form = result.data;

                that.form.parentId =
                    form.parentName == "" ? undefined : form.parentId;
                that.spinning = false;

                result = await findByMenuId({ menuId: this.menuId, configType: 2 });
                if (result.code === this.eipResultCode.success) {
                    var agileData = result.data[0];
                    form.dataFromName = agileData.dataFromName
                    form.dataBaseId = agileData.dataBaseId
                }

                that.form = form;
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