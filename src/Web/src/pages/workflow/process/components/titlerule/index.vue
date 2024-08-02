<template>
  <a-modal width="600px" v-drag centered :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @cancel="cancel">
    <a-spin :spinning="spinning">
      <div class="padding">
        <a-space class="margin-bottom-sm">
          <a-button size="small" @click="titleWorkflowTypeName" type="primary">流程类型名称</a-button>
          <a-button size="small" @click="titleCurrentUser" type="primary">当前用户</a-button>
          <a-button size="small" @click="titleCurrentUserOrganization" type="primary">当前用户组织名称</a-button>
          <a-button size="small" @click="titleField" type="primary">表单名称</a-button>
          <a-button size="small" @click="titleDatetime" type="primary">当前时间</a-button>
          <a-button size="small" @click="titleColumn" type="primary">表单字段</a-button>
        </a-space>
        <eip-editor ref="editor" v-if="tinymce.show" v-model="form.titleRule" :height="tinymce.height"
          :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
      </div>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>

    <eip-column v-if="content.visible" :visible.sync="content.visible" :data="column" title="业务数据"
      @ok="contentOk"></eip-column>
  </a-modal>
</template>
<script>

export default {
  name: "eip-workflow-title-rule",
  data() {
    return {
      bodyStyle: {
        padding: "1px",
      },
      tinymce: {
        plugins: "preview  fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 200,
        show: false,
        menubar: "",
      },

      form: {
        titleRule: "您有一条待审批流程",
      },
      content: {
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
    * 取消
    */
    cancel() {
      this.$emit("update:visible", false);
    },
    /**
     * 内容选择
     */
    contentOk(value) {
      let html = "<span id='" + encodeURIComponent(value[0].key) + "' class='non-editable'>" + value[0].title + "</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前用户
     */
    titleCurrentUser() {
      let html = "<span id='" + encodeURIComponent("当前用户名") + "' class='non-editable'>当前用户名</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前用户组织架构名称
     */
    titleCurrentUserOrganization() {
      let html = "<span id='" + encodeURIComponent("当前用户组织架构名称") + "' class='non-editable'>当前用户组织架构名称</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 流程类型名称
     */
    titleWorkflowTypeName() {
      let html = "<span id='" + encodeURIComponent("流程类型名称") + "' class='non-editable'>流程类型名称</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前时间
     */
    titleDatetime() {
      let html = "<span id='" + encodeURIComponent("当前时间") + "' class='non-editable'>当前时间</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     *
     */
    titleColumn() {
      this.content.visible = true;
    },

    /**
     * 表单名称
     */
    titleField() {
      let html = "<span id='" + encodeURIComponent("表单名称") + "' class='non-editable'>表单名称</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 保存
     */
    save() {
      let that = this;
      that.$emit("ok", that.form);
      that.cancel();
    },

    /**
     *
     */
    find() {
      this.form = this.edit;
    },
  },
};
</script>