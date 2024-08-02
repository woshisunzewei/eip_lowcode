<template>
  <a-modal width="1200px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <eip-code height="600px" :value.sync="form.js">
    </eip-code>
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
  name: "eip-workflow-event-js",
  data() {
    return {
      cmOptions: {
        tabSize: 4, // tab的空格个数
        theme: 'blackboard', //主题样式
        lineNumbers: true, //是否显示行数
        lineWrapping: true, //是否自动换行
        styleActiveLine: true, //line选择是是否加亮
        matchBrackets: true, //括号匹配
        mode: 'text/x-sparksql', //实现javascript代码高亮
        readOnly: false, //只读
        keyMap: 'default',
        extraKeys: { tab: 'autocomplete' },
        foldGutter: true,
        gutters: ['CodeMirror-linenumbers', 'CodeMirror-foldgutter'],
        hintOptions: {
          completeSingle: false,
          tables: {},
        },
      },
      bodyStyle: {
        paddingTop: "20px",
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: { js: '' },
      rules: {
        js: [
          {
            required: true,
            message: "请输入Js代码",
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
    edit: Object,
    title: {
      type: String,
    },
  },

  mounted() {
    this.find();
  },

  methods: {
    /**
     *
     * @param {*} v
     */
    eventChange(v) {
      this.form.js = v;
    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     *
     */
    find() {
      if (this.edit.config && this.edit.config.js) {
        this.form = this.edit.config;
      }
    },

    /**
     * 保存
     */
    save() {
      this.$emit("ok", this.form);
      this.cancel();
    },
  },
};
</script>