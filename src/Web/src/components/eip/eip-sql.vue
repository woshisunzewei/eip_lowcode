<template>
  <a-drawer width="700px" :visible="visible" :bodyStyle="{ padding: '1px' }" @close="cancel" :title="title"
    :destroyOnClose="true">
    <div class="eip-drawer-body beauty-scroll">
      <a-alert type="error" message="请输入查询Sql【注：必须返回固定字段（UserId，Name，Code）】" />
      <div class="margin-top-sm"></div>
      <eip-editor ref="sqleditor" v-if="tinymce.show" v-model="sql" :height="tinymce.height" :toolbar="tinymce.toolbar"
        :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
      <a-space>
        <a-badge style="cursor: pointer" @click="form" count="{表单}" />
        <a-badge style="cursor: pointer" @click="role" count="{角色}" />
      </a-space>
    </div>

    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary"><a-icon type="save" />提交</a-button>
      </a-space>
    </div>
    <eip-role v-if="range.role.visible" :visible.sync="range.role.visible" :topOrg="range.role.topOrg" title="选择角色"
      @ok="chosenRoleOk"></eip-role>

    <eip-column v-if="range.column.visible" :visible.sync="range.column.visible" :data="column" title="选择字段"
      @ok="chosenColumnOk"></eip-column>
  </a-drawer>
</template>
<script>

export default {
  name: "eip-sql",
  data() {
    return {
      tinymce: {
        plugins: "preview  fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 350,
        show: false,
        menubar: "",
      },
      sql: "",
      range: {
        //范围选择角色
        role: {
          visible: false,
          topOrg: "",
        },
        column: {
          visible: false,
        },
      },
    };
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    chosen: {
      type: Array,
    },
    column: {
      type: Array,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.init();
    setTimeout(() => {
      this.tinymce.show = true;
    }, 10);
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
      that.$emit("ok", {
        content: this.sql,
        contentTxt: this.$refs.sqleditor.getTxt(),
      });
      that.cancel();
    },

    /**
     * 选择角色成功
     */
    chosenRoleOk(roles) {
      var ids = [];
      roles.forEach((item) => {
        ids.push(item.roleId);
      });
      var html = "{[角色]:" + ids.join(",") + "}";
      this.$refs.sqleditor.insert(html);
    },

    /**
     *
     */
    chosenColumnOk(columns) {
      var html = "{[表单]:" + columns[0].title + "}";
      this.$refs.sqleditor.insert(html);
    },

    /**
     * 表单
     */
    form() {
      this.range.column.visible = true;
    },

    /**
     * 角色
     */
    role() {
      this.range.role.visible = true;
    },

    /**
     * 初始化
     */
    init() {
      if (this.chosen && this.chosen.length > 0) {
        this.sql = this.chosen[0].id;
      }
    },
  },
};
</script>