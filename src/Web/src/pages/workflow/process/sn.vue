<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel" :centered="true">
    <a-spin :spinning="spinning">
      <div class="padding">
        <a-space class="margin-bottom-sm">
          <a-button size="small" @click="titleY" type="primary">年</a-button>
          <a-button size="small" @click="titleM" type="primary">月</a-button>
          <a-button size="small" @click="titleD" type="primary">日</a-button>
          <a-button size="small" @click="titleH" type="primary">时</a-button>
          <a-button size="small" @click="titleMm" type="primary">分</a-button>
          <a-button size="small" @click="titleS" type="primary">秒</a-button>
          <a-button size="small" @click="titleSs" type="primary">毫秒</a-button>
          <a-button size="small" @click="titleThree" type="primary">000</a-button>
          <a-button size="small" @click="titleFour" type="primary">0000</a-button>
          <a-button size="small" @click="titleIdGen" type="primary">雪花唯一</a-button>
        </a-space>
        <eip-editor ref="editor" v-if="tinymce.show" v-model="form.sn" :height="tinymce.height"
          :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
      </div>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>
<script>
export default {
  name: "sn",
  data() {
    return {
      tinymce: {
        plugins: "preview  fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 200,
        show: false,
        menubar: "",
      },

      form: {
        sn: "",
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
    sn: {
      type: String,
    },
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
     * 当前年
     */
    titleY() {
      let html = "<span id='" + encodeURIComponent("年") + "' class='non-editable'>年</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前月
     */
    titleM() {
      let html = "<span id='" + encodeURIComponent("月") + "' class='non-editable'>月</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前日
     */
    titleD() {
      let html = "<span id='" + encodeURIComponent("日") + "' class='non-editable'>日</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前时
     */
    titleH() {
      let html = "<span id='" + encodeURIComponent("时") + "' class='non-editable'>时</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前分
     */
    titleMm() {
      let html = "<span id='" + encodeURIComponent("分") + "' class='non-editable'>分</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前秒
     */
    titleS() {
      let html = "<span id='" + encodeURIComponent("秒") + "' class='non-editable'>秒</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前毫秒
     */
    titleSs() {
      let html = "<span id='" + encodeURIComponent("毫秒") + "' class='non-editable'>毫秒</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 三位数
     */
    titleThree() {
      let html = "<span id='" + encodeURIComponent("000") + "' class='non-editable'>000</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 四位年数
     */
    titleFour() {
      let html = "<span id='" + encodeURIComponent("0000") + "' class='non-editable'>0000</span>";
      this.$refs.editor.insertIndex(html, 0);
    },
    titleIdGen() {
      let html = "<span id='" + encodeURIComponent("雪花唯一") + "' class='non-editable'>雪花唯一</span>";
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
      this.form.sn = this.sn;
    },
  },
};
</script>