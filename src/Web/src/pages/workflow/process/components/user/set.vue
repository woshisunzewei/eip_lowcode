<template>
  <a-modal
    width="600px"
    v-drag
    centered
    :destroyOnClose="true"
    :maskClosable="false"
    :visible="visible"
    :bodyStyle="{ padding: '1px' }"
    :title="title"
    @cancel="cancel"
  >
    <a-spin :spinning="spinning">
      <a-form-model
        ref="form"
        :model="form"
        :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol"
      >
        <div style="padding: 10px" size="small">
          <a-form-model-item label="处理人类型">
            <a-select
              :default-value="0"
              v-model="form.type"
              @change="typeChange"
            >
              <a-select-option
                v-for="(item, index) in type"
                :key="index"
                :value="item.key"
                >{{ item.value }}</a-select-option
              >
            </a-select>
          </a-form-model-item>

          <a-form-model-item
            label="类型值选择"
            v-if="
              form.type != 0 &&
              form.type != 22 &&
              form.type != 24 &&
              form.type != 26 &&
              form.type != 100
            "
          >
            <a-input
              v-model="form.rangeTxt"
              placeholder="请选择类型值"
              disabled
            >
              <a-icon
                @click="rangeClick"
                style="cursor: pointer"
                slot="addonAfter"
                type="search"
              />
            </a-input>
          </a-form-model-item>

          <a-form-model-item label="范围选择">
            <a-radio-group v-model="form.organizationRange">
              <a-radio
                v-for="(item, index) in eipOrganizationNatures"
                :key="index"
                :value="item.value"
                >{{ item.name }}</a-radio
              >
              <a-radio :value="100">无</a-radio>
            </a-radio-group>
          </a-form-model-item>

          <a-form-model-item label="处理人员模式">
            <a-radio-group v-model="form.pattern">
              <a-radio :value="0">单选</a-radio>
              <a-radio :value="1">多选</a-radio>
            </a-radio-group>
          </a-form-model-item>

          <a-form-model-item label="显示名称">
            <a-input
              v-model="form.name"
              placeholder="请输入显示名称"
              allow-clear
            ></a-input>
          </a-form-model-item>
        </div>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"
          ><a-icon type="close" />取消</a-button
        >
        <a-button key="submit" @click="save" type="primary" :loading="loading"
          ><a-icon type="save" />提交</a-button
        >
      </a-space>
    </template>
    <eip-organization
      v-if="range.organization.visible"
      :visible.sync="range.organization.visible"
      :chosen="form.range"
      :range="0"
      title="选择组织架构"
      @ok="chosenOrganizationOk"
    ></eip-organization>

    <eip-user
      v-if="range.user.visible"
      :visible.sync="range.user.visible"
      :topOrg="range.user.topOrg"
      :chosen="form.range"
      title="选择人员"
      @ok="chosenUserOk"
    ></eip-user>

    <eip-role
      v-if="range.role.visible"
      :visible.sync="range.role.visible"
      :topOrg="range.role.topOrg"
      :chosen="form.range"
      title="选择角色"
      @ok="chosenRoleOk"
    ></eip-role>

    <eip-post
      v-if="range.post.visible"
      :visible.sync="range.post.visible"
      :topOrg="range.post.topOrg"
      :chosen="form.range"
      title="选择岗位"
      @ok="chosenPostOk"
    ></eip-post>

    <eip-group
      v-if="range.group.visible"
      :visible.sync="range.group.visible"
      :topOrg="range.group.topOrg"
      :chosen="form.range"
      title="选择组"
      @ok="chosenGroupOk"
    ></eip-group>

    <eip-column
      v-if="range.column.visible"
      :visible.sync="range.column.visible"
      :data="column"
      :chosen="form.range"
      title="选择字段"
      @ok="chosenColumnOk"
    ></eip-column>

    <eip-sql
      v-if="range.sql.visible"
      :visible.sync="range.sql.visible"
      :column="column"
      :chosen="form.range"
      title="自定义Sql"
      @ok="chosenSqlOk"
    ></eip-sql>
  </a-modal>
</template>

<script>
export default {
  name: "eip-workflow-user-set",
  data() {
    return {
      config: {
        labelCol: {
          span: 4,
        },
        wrapperCol: {
          span: 20,
        },
      },
      form: {
        type: 0, //处理人类型
        rangeTxt: "", //处理人范围
        range: [], //处理人范围
        organizationRange: 100, //处理人与被处理人相同范围选择
        pattern: 1, //处理人员模式
        name: "所有成员", //显示名称
      },
      type: [
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
      loading: false,
      spinning: false,
    };
  },
  components: {},
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    edit: {
      type: Object,
    },
    column: {
      type: Array,
    },
    title: {
      type: String,
    },
  },

  mounted() {
    this.initSet();
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
     *初始化值
     */
    initSet() {
      if (this.edit) {
        this.form = this.edit;
      }
    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 范围选择
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
     * 选择组织架构成功
     */
    chosenOrganizationOk(organizations) {
      var value = this.$utils
        .map(organizations, (item) => item.title)
        .join(",");
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
     * 保存
     */
    save() {
      switch (this.form.type) {
        case 2: //组织
        case 4: //人员
        case 6: //角色
        case 8: //岗位
        case 10: //组
        case 28: //表单人员
        case 90: //自定义Sql
          if (this.form.range.length == 0) {
            this.$message.error("请选择类型值");
            return false;
          }
          break;
        default:
      }
      this.$emit("ok", this.form);
      this.cancel();
    },
  },
};
</script>
