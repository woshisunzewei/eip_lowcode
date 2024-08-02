<template>
    <a-modal width="700px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible"
        :title="title" :bodyStyle="{ padding: '1px' }" @cancel="cancel">
        <a-spin :spinning="spinning">
            <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
                :wrapper-col="config.wrapperCol">
                <a-row>
                    <a-col>
                        <div class="margin-top-sm"></div>
                        <a-form-model-item label="公众号" prop="officialaccountAppId">
                            <a-select v-model="form.officialaccountAppId" placeholder="请选择公众号">
                                <a-select-option v-for="(item, index) in officialAccount" :key="index"
                                    :value="item.appId">{{ item.name
                                    }}</a-select-option>
                            </a-select>
                        </a-form-model-item>
                        <a-form-model-item label="模版Id" prop="code">
                            <a-input v-model="form.code" placeholder="请输入模版Id"></a-input>
                        </a-form-model-item>
                        <a-form-model-item label="模板内容">
                            <eip-editor ref="officialaccountTemplateContent" v-if="tinymce.show"
                                v-model="form.officialaccountTemplateContent" :height="tinymce.height"
                                :toolbar="tinymce.toolbar" :plugins="tinymce.plugins"
                                :menubar="tinymce.menubar"></eip-editor>
                        </a-form-model-item>
                        <a-form-model-item label="模板参数">
                            <vxe-table ref="tableweixinoffiaccount" id="workflowactivityweixinoffiaccount" size="small"
                                :height="height" :data="form.param">
                                <template #loading>
                                    <a-spin></a-spin>
                                </template>
                                <template #empty>
                                    <eip-empty />
                                </template>
                                <vxe-column width="60" align="center">
                                    <template #header>
                                        <a-button type="primary" size="small" @click="add">
                                            <a-icon type="plus" />
                                        </a-button>
                                    </template>
                                    <template #default="{ row }">
                                        <a-popconfirm title="确定删除参数?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
                                            <a-button @click.stop="" size="small" type="danger">
                                                <a-icon type="delete" />
                                            </a-button>
                                        </a-popconfirm>
                                    </template>
                                </vxe-column>
                                <vxe-column field="name" title="参数名" width="110">
                                    <template v-slot="{ row }">
                                        <a-input placeholder="参数名" v-model="row.name" />
                                    </template>
                                </vxe-column>

                                <vxe-column title="参数值" align="center" width="180">
                                    <template v-slot="{ row }">
                                        <a-input placeholder="参数值" v-model="row.value" />
                                    </template>
                                </vxe-column>

                                <vxe-column title="颜色" align="center" width="80">
                                    <template v-slot="{ row }">
                                        <a-popover title="选择颜色">
                                            <div slot="content">
                                                <div style="text-align: center">
                                                    <eip-color :value="row.color"
                                                        @change="(value) => { row.color = value }"></eip-color>
                                                </div>
                                            </div>
                                            <a-tag :color="row.color">
                                                {{ row.color }}
                                            </a-tag>
                                        </a-popover>
                                    </template>
                                </vxe-column>
                            </vxe-table>
                        </a-form-model-item>

                        <a-form-model-item label="跳转地址">
                            <eip-editor ref="urlEditor" v-if="tinymce.show" v-model="form.urlContent"
                                :height="tinymce.height" :toolbar="tinymce.toolbar" :plugins="tinymce.plugins"
                                :menubar="tinymce.menubar"></eip-editor>
                            <a-space>
                                <a-button @click="urlClick" type="primary">
                                    <a-icon type="search"></a-icon>业务数据
                                </a-button>
                            </a-space>
                        </a-form-model-item>

                        <a-form-model-item label="跳转小程序" prop="miniAppId">
                            <a-select allow-clear v-model="form.miniAppId" placeholder="请选择跳转小程序">
                                <a-select-option v-for="(item, index) in mini" :key="index" :value="item.appId">{{
                                    item.name
                                    }}</a-select-option>
                            </a-select>
                        </a-form-model-item>

                        <a-form-model-item label="通知人类型" prop="name">
                            <a-select v-model="form.type" @change="typeChange" placeholder="请选择通知人类型">
                                <a-select-option v-for="(item, index) in type" :key="index" :value="item.key">{{
                                    item.value
                                    }}</a-select-option>
                            </a-select>
                        </a-form-model-item>
                        <a-form-model-item label="类型值选择" v-if="form.type != 0 &&
                            form.type != 12 &&
                            form.type != 22 &&
                            form.type != 24 &&
                            form.type != 26 &&
                            form.type != 100
                        ">
                            <a-input v-model="form.rangeTxt" placeholder="请选择类型值" disabled>
                                <a-icon @click="rangeClick" style="cursor: pointer" slot="addonAfter" type="search" />
                            </a-input>
                        </a-form-model-item>
                    </a-col>
                </a-row>
            </a-form-model>
        </a-spin>
        <template slot="footer">
            <a-space>
                <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
                <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon
                        type="save" />提交</a-button>
            </a-space>
        </template>
        <eip-organization v-if="range.organization.visible" :visible.sync="range.organization.visible"
            :chosen="form.range" :range="0" title="选择组织架构" @ok="chosenOrganizationOk"></eip-organization>

        <eip-user v-if="range.user.visible" :visible.sync="range.user.visible" :topOrg="range.user.topOrg"
            :chosen="form.range" title="选择人员" @ok="chosenUserOk"></eip-user>

        <eip-role v-if="range.role.visible" :visible.sync="range.role.visible" :topOrg="range.role.topOrg"
            :chosen="form.range" title="选择角色" @ok="chosenRoleOk"></eip-role>

        <eip-post v-if="range.post.visible" :visible.sync="range.post.visible" :topOrg="range.post.topOrg"
            :chosen="form.range" title="选择岗位" @ok="chosenPostOk"></eip-post>

        <eip-group v-if="range.group.visible" :visible.sync="range.group.visible" :topOrg="range.group.topOrg"
            :chosen="form.range" title="选择组" @ok="chosenGroupOk"></eip-group>

        <eip-column v-if="range.column.visible" :visible.sync="range.column.visible" :data="column" :chosen="form.range"
            title="选择字段" @ok="chosenColumnOk"></eip-column>

        <eip-sql v-if="range.sql.visible" :visible.sync="range.sql.visible" :column="column" :chosen="form.range"
            title="自定义Sql" @ok="chosenSqlOk"></eip-sql>

        <eip-column v-if="chosenColumn.visible" :visible.sync="chosenColumn.visible" :data="column" title="业务数据"
            @ok="urlOk"></eip-column>
    </a-modal>
</template>
<script>

import { query } from "@/services/wechat/account/list";
export default {
    name: "eip-workflow-notice-wei-xin-mini-program",
    data() {
        return {
            tinymce: {
                plugins: "preview  fullscreen code",
                toolbar: "undo redo |fullscreen|code",
                height: 160,
                show: false,
                menubar: "",
            },
            height: 184,

            type: [
                {
                    key: 12,
                    value: "当前活动处理人",
                },
                {
                    key: 0,
                    value: "所有成员",
                },
                {
                    key: 2,
                    value: "组织",
                },
                {
                    key: 4,
                    value: "人员",
                },
                {
                    key: 6,
                    value: "角色",
                },
                {
                    key: 8,
                    value: "岗位",
                },
                {
                    key: 10,
                    value: "组",
                },
                {
                    key: 22,
                    value: "发起者",
                },
                {
                    key: 24,
                    value: "发起者直线领导",
                },
                {
                    key: 26,
                    value: "提交人直线领导",
                },
                {
                    key: 28,
                    value: "表单人员",
                },
                {
                    key: 90,
                    value: "自定义",
                },
                {
                    key: 100,
                    value: "程序指定",
                },
            ],
            range: {
                //范围选择组织架构
                organization: {
                    visible: false,
                },
                //范围选择人员
                user: {
                    visible: false,
                    topOrg: "",
                },
                //范围选择角色
                role: {
                    visible: false,
                    topOrg: "",
                },
                //范围选择岗位
                post: {
                    visible: false,
                    topOrg: "",
                },
                //范围选择组
                group: {
                    visible: false,
                    topOrg: "",
                },
                column: {
                    visible: false,
                },
                //自定义Sql
                sql: {
                    visible: false,
                },
            },
            chosenColumn: {
                visible: false,
            },
            config: {
                labelCol: { span: 4 },
                wrapperCol: { span: 20 },
            },
            form: {
                officialaccountAppId: undefined, //微信公众号Id
                officialaccountTemplateContent: "", //公众号模版内容
                name: "",
                code: "", //模版代码
                type: 12, //处理人类型
                rangeTxt: "", //处理人范围
                range: [], //处理人范围
                miniAppId: undefined, //小程序Id
                urlContent: "", //跳转地址路径
                urlContentTxt: "", //跳转地址路径内容
                param: [],
            },

            rules: {
                officialaccountAppId: [
                    {
                        required: true,
                        message: "请选择公众号",
                        trigger: ["blur", "change"],
                    },
                ],
            },

            loading: false,
            spinning: false,

            templateParam: [
                {
                    title: "流程字段",
                    children: [
                        { title: "流水号", key: "流水号", isLeaf: true },
                        { title: "流程名称", key: "流程名称", isLeaf: true },
                        { title: "活动名称", key: "活动名称", isLeaf: true },
                        { title: "任务Id", key: "任务Id", isLeaf: true },
                        { title: "流程Id", key: "流程Id", isLeaf: true },
                        { title: "活动Id", key: "活动Id", isLeaf: true },
                        { title: "自定义数据", key: "自定义数据", isLeaf: true },
                        { title: "当前处理人Id", key: "当前处理人Id", isLeaf: true },
                        { title: "当前处理人姓名", key: "当前处理人姓名", isLeaf: true },
                        { title: "发起人Id", key: "发起人Id", isLeaf: true },
                        { title: "发起人姓名", key: "发起人姓名", isLeaf: true },
                    ],
                },
                {
                    title: "表单字段",
                    children: [
                        { title: "记录ID", key: "RelationId", isLeaf: true },
                        { title: "创建用户Id", key: "CreateUserId", isLeaf: true },
                        { title: "创建用户名", key: "CreateUserName", isLeaf: true },
                    ],
                },
            ],

            officialAccount: [], //微信公众号
            mini: [], //微信小程序
        };
    },

    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        column: {
            type: Array,
        },
        edit: Object,
        title: {
            type: String,
        },
    },
    mounted() {
        this.initWeichatAccount();
        this.find();
        setTimeout(() => {
            this.tinymce.show = true;
        }, 10);
    },
    methods: {
        /**
         *
         */
        initWeichatAccount() {
            let that = this;
            query().then(function (result) {
                if (result.code == that.eipResultCode.success) {
                    that.officialAccount = result.data.filter((f) => f.type == 2);
                    that.mini = result.data.filter((f) => f.type == 1);
                }
            });
        },

        /**
         * 处理人类型改变
         */
        typeChange() {
            this.form.rangeTxt = "";
            this.form.name = "";
            switch (this.form.type) {
                case 0:
                case 22:
                case 24:
                case 26:
                case 90:
                case 100:
                    this.form.name = this.type.filter(
                        (f) => f.key == this.form.type
                    )[0].value;
                    break;
            }
        },

        /**
         *范围点击
         */
        rangeClick() {
            switch (this.form.type) {
                case 2: //组织
                    this.range.organization.visible = true;
                    break;
                case 4: //人员
                    this.range.user.visible = true;
                    break;
                case 6: //角色
                    this.range.role.visible = true;
                    break;
                case 8: //岗位
                    this.range.post.visible = true;
                    break;
                case 10: //组
                    this.range.group.visible = true;
                    break;
                case 28: //表单人员
                    this.range.column.visible = true;
                    break;
                case 90: //自定义Sql
                    this.range.sql.visible = true;
                    break;
                default:
            }
        },

        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },
        /**
         * 选择组织架构成功
         */
        chosenOrganizationOk(organizations) {
            var value = this.$utils.map(organizations, (item) => item.title).join(",");
            let that = this;
            that.form.range = [];
            that.form.rangeTxt = value;
            that.form.name = value;
            this.$utils.each(organizations, (item, key) => {
                that.form.range.push({
                    id: item.key,
                    name: item.title,
                });
            });
        },

        /**
         * 选择人员成功
         */
        chosenUserOk(users) {
            var value = this.$utils.map(users, (item) => item.name).join(",");
            let that = this;
            that.form.range = [];
            that.form.rangeTxt = value;
            that.form.name = value;
            this.$utils.each(users, (item, key) => {
                that.form.range.push({
                    id: item.userId,
                    name: item.name,
                });
            });
        },

        /**
         * 选择角色成功
         */
        chosenRoleOk(roles) {
            var value = this.$utils.map(roles, (item) => item.name).join(",");
            let that = this;
            that.form.range = [];
            that.form.rangeTxt = value;
            that.form.name = value;
            this.$utils.each(roles, (item, key) => {
                that.form.range.push({
                    id: item.roleId,
                    name: item.name,
                });
            });
        },

        /**
         *
         */
        chosenGroupOk(groups) {
            var value = this.$utils.map(groups, (item) => item.name).join(",");
            let that = this;
            that.form.range = [];
            that.form.rangeTxt = value;
            that.form.name = value;
            this.$utils.each(groups, (item, key) => {
                that.form.range.push({
                    id: item.groupId,
                    name: item.name,
                });
            });
        },

        /**
         *
         */
        chosenPostOk(posts) {
            var value = this.$utils.map(posts, (item) => item.name).join(",");
            let that = this;
            that.form.range = [];
            that.form.rangeTxt = value;
            that.form.name = value;
            this.$utils.each(posts, (item, key) => {
                that.form.range.push({
                    id: item.postId,
                    name: item.name,
                });
            });
        },

        /**
         *
         */
        chosenColumnOk(columns) {
            var value = this.$utils.map(columns, (item) => item.title).join(",");
            let that = this;
            that.form.range = [];
            that.form.rangeTxt = value;
            that.form.name = value;
            this.$utils.each(columns, (item, key) => {
                that.form.range.push({
                    id: item.key,
                    name: item.title,
                });
            });
        },

        /**
         *
         */
        chosenSqlOk(sql) {
            let that = this;
            that.form.range = [];
            that.form.rangeTxt = sql.contentTxt;
            that.form.name = "自定义";
            that.form.range.push({
                id: sql.contentTxt,
                name: "自定义",
            });
        },

        /**
         *内容选择点击
         */
        urlClick() {
            this.chosenColumn.visible = true;
        },

        /**
         * 内容选择
         */
        urlOk(value) {
            let html = "<span id='" + encodeURIComponent(value[0].key) + "' class='non-editable'>" + value[0].title + "</span>";
            this.$refs.urlEditor.insertIndex(html, 1);
        },

        /**
         * 保存
         */
        save() {
            let that = this;
            this.$refs.form.validate((valid) => {
                if (valid) {
                    that.form.urlContentTxt = that.$refs.urlEditor.getTxt(1);
                    that.form.urlContent = escape(that.form.urlContent);
                    that.form.officialaccountTemplateContent = escape(
                        that.form.officialaccountTemplateContent
                    );
                    that.$emit("ok", that.form);
                    that.cancel();
                } else {
                    return false;
                }
            });
        },

        /**
         *
         */
        find() {
            if (this.edit.config) {
                this.edit.config.urlContent = unescape(this.edit.config.urlContent);
                this.edit.config.officialaccountTemplateContent = unescape(
                    this.edit.config.officialaccountTemplateContent
                );
                this.form = this.edit.config;
            }
        },

        /**
         * 添加
         */
        add() {
            this.form.param.push({
                name: "",
                value: "", //是否启用
                color: "#000000",
            });
        },
        /**
         *删除
         */
        del(row) {
            var delIndex = -1;
            this.form.param.forEach((item, index) => {
                if (item._X_ROW_KEY == row._X_ROW_KEY) {
                    delIndex = index;
                }
            });
            if (delIndex != -1) {
                this.form.param.splice(delIndex, 1);
            }
        },
    },
};
</script>