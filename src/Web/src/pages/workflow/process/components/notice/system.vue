<template>
  <a-modal width="600px" v-drag centered :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="标题" prop="title">
              <a-input v-model="form.title" placeholder="请输入名称" />
            </a-form-model-item>

            <a-form-model-item label="内容">
              <eip-editor ref="systemEditor" v-if="tinymce.show" v-model="form.content" :height="tinymce.height"
                :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
              <a-space>
                <a-button @click="contentClick" type="primary">
                  <a-icon type="search"></a-icon>业务数据
                </a-button>
              </a-space>
            </a-form-model-item>

            <a-form-model-item label="跳转地址">
              <eip-editor ref="urlEditor" v-if="tinymce.show" v-model="form.urlContent" :height="tinymce.height"
                :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
              <a-space>
                <a-button @click="urlClick" type="primary">
                  <a-icon type="search"></a-icon>业务数据
                </a-button>
              </a-space>

            </a-form-model-item>

            <a-form-model-item label="通知人类型" prop="name">
              <a-select :default-value="0" v-model="form.type" @change="typeChange">
                <a-select-option v-for="(item, index) in type" :key="index" :value="item.key">{{ item.value
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
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
    <eip-organization v-if="range.organization.visible" :visible.sync="range.organization.visible" :chosen="form.range"
      :range="0" title="选择组织架构" @ok="chosenOrganizationOk"></eip-organization>

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

    <eip-column v-if="urlColumn.visible" :visible.sync="urlColumn.visible" :data="column" title="选择字段"
      @ok="urlOk"></eip-column>

    <eip-sql v-if="range.sql.visible" :visible.sync="range.sql.visible" :column="column" :chosen="form.range"
      title="自定义Sql" @ok="chosenSqlOk"></eip-sql>

    <eip-column v-if="content.visible" :visible.sync="content.visible" :data="column" title="业务数据"
      @ok="contentOk"></eip-column>
  </a-modal>
</template>
<script>

export default {
  name: "eip-workflow-notice-system",
  data() {
    return {
      tinymce: {
        plugins: "preview  fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 200,
        show: false,
        menubar: "",
      },
      type: [
        {
          key: 12,
          value: "下一步处理接收人",
        },
        {
          key: 14,
          value: "当前处理人",
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
      content: {
        visible: false,
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        title: "", //标题
        content: "", //内容
        contentTxt: "", //内容
        urlContent: "", //跳转地址路径
        urlContentTxt: "", //跳转地址路径内容
        type: 0, //处理人类型
        rangeTxt: "", //处理人范围
        range: [], //处理人范围
      },

      rules: {
        title: [
          {
            required: true,
            message: "请选择标题",
            trigger: ["blur"],
          },
        ],
      },
      urlColumn: {
        visible: false,
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
    column: {
      type: Array,
    },
    edit: Object,
    title: {
      type: String,
    },
  },
  mounted() {
    this.find();
    setTimeout(() => {
      this.tinymce.show = true;
    }, 10);
  },
  methods: {
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
    contentClick() {
      this.content.visible = true;
    },

    /**
     * 内容选择
     */
    contentOk(value) {
      let html = "<span id='" + encodeURIComponent(value[0].key) + "' class='non-editable'>" + value[0].title + "</span>";
      this.$refs.systemEditor.insertIndex(html, 0);
    },

    /**
     *内容选择点击
     */
    urlClick() {
      this.urlColumn.visible = true;
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
          that.form.contentTxt = that.$refs.systemEditor.getTxt();
          that.form.content = escape(that.form.content);
          that.form.urlContentTxt = that.$refs.urlEditor.getTxt(1);
          that.form.urlContent = escape(that.form.urlContent);
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
        this.edit.config.content = unescape(this.edit.config.content);
        this.form = this.edit.config;
      }
    },
  },
};
</script>