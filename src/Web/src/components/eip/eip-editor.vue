<template>
  <div>
    <editor v-model="myValue" :init="init" :disabled="disabled" @onClick="onClick" />
  </div>
</template>

<script>
import { upload } from "@/services/common/tinymce";
import Editor from "@tinymce/tinymce-vue";
export default {
  name: 'eip-editor',
  components: {
    Editor
  },
  props: {
    height: {
      type: Number,
      default: 200,
    },
    content_style: {
      type: String,
      default: "",
    },
    placeholder: {
      type: String,
      default: "在这里输入内容",
    },
    value: {
      type: String,
      default: "",
    },
    menubar: {
      type: [String, Boolean],
      default: true,
    },
    disabled: {
      type: Boolean,
      default: false,
    },
    elementpath: {
      type: Boolean,
      default: true,
    },
    resize: {
      type: Boolean,
      default: true,
    },
    statusbar: {
      type: Boolean,
      default: true,
    },
    plugins: {
      type: [String, Array],
      default:
        "print preview searchreplace autolink directionality visualblocks visualchars fullscreen image link media template code codesample table charmap hr pagebreak nonbreaking anchor insertdatetime advlist lists wordcount imagetools textpattern help emoticons autosave  autoresize ",
    },
    toolbar: {
      type: [String, Array],
      default:
        "code undo redo restoredraft | cut copy paste pastetext | forecolor backcolor bold italic underline strikethrough link anchor | alignleft aligncenter alignright alignjustify outdent indent | \
    styleselect formatselect fontselect fontsizeselect | bullist numlist | blockquote subscript superscript removeformat | \
    table image media charmap emoticons hr pagebreak insertdatetime print preview | fullscreen |  lineheight ",
    },
  },
  data() {
    return {
      tinymceId: Math.random(),
      init: {
        forced_root_block: "",
        placeholder: this.placeholder,
        language: "zh_CN",
        base_url: window.location.origin + '/tinymce',
        skin_url: "/tinymce/skins/ui/oxide",
        content_css: '/tinymce/skins/content/default/content.min.css',
        height: this.height,
        min_height: this.height,
        plugins: this.plugins,
        toolbar: this.toolbar,
        menubar: this.menubar,
        elementpath: this.elementpath,//设为false时，可隐藏底栏的元素路径
        resize: this.resize,//调整编辑器大小工具
        statusbar: this.statusbar,
        relative_urls: false,
        convert_urls: false,
        branding: false,
        fontsize_formats: "12px 14px 16px 18px 24px 36px 48px 56px 72px",
        content_style: this.content_style + ' .mce-content-body [contentEditable=false] {cursor: pointer;  background-color: #40a9ff;  color: #fff;  margin: 0 2px;  padding: 0px 2px;}',
        font_formats:
          "微软雅黑=Microsoft YaHei,Helvetica Neue,PingFang SC,sans-serif;苹果苹方=PingFang SC,Microsoft YaHei,sans-serif;宋体=simsun,serif;仿宋体=FangSong,serif;黑体=SimHei,sans-serif;Arial=arial,helvetica,sans-serif;Arial Black=arial black,avant garde;Book Antiqua=book antiqua,palatino;",
        importcss_append: true,
        images_upload_handler: (blobInfo, success, failure) => {
          const formData = new FormData();
          formData.append("correlationId", "tinymce"); //添加file表单数据
          formData.append("relativePath", "tinymce/files"); //添加file表单数据
          formData.append("file", blobInfo.blob());
          upload(formData)
            .then((result) => {
              if (result.code === 200) {
                success(result.data[0].path);
                return;
              } else {
                failure("上传失败");
              }
            })
            .catch(() => {
              failure("上传出错");
            });
        },
        setup: function (editor) {
          editor.on('init', function () {
            this.getBody().style.fontSize = '14px';
            // 在编辑器初始化完成后，设置部分内容不可编辑
            const body = editor.getBody();
            // 获取需要设置为不可编辑的元素
            const elementsToMakeNonEditable = body.querySelectorAll('.non-editable');
            // 遍历元素，并设置为不可编辑
            elementsToMakeNonEditable.forEach((element) => {
              editor.dom.setAttrib(element, 'contenteditable', false);
            });
          })

          // 监听编辑器的 change 事件，获取最新的内容，并设置部分内容不可编辑
          editor.on('change', () => {
            const body = editor.getBody();
            // 获取需要设置为不可编辑的元素
            const elementsToMakeNonEditable = body.querySelectorAll('.non-editable');
            // 遍历元素，并设置为不可编辑
            elementsToMakeNonEditable.forEach((element) => {
              editor.dom.setAttrib(element, 'contenteditable', false);
            });
          });
        }
      },
      myValue: this.value,
    };
  },
  watch: {
    value(newValue) {
      this.myValue = newValue;
    },
    myValue(newValue) {
      this.$emit("input", newValue);
    },
  },
  mounted() {
    tinymce.init({
      deprecation_warnings: false
    });
  },
  methods: {
    /**
     *
     */
    onClick(e) {
      this.$emit("onClick", e, tinymce);
    },

    /**
     * 可以添加一些自己的自定义事件，如清空内容
     */
    clear() {
      this.myValue = "";
    },

    /**
     *光标处插入
     */
    insert(value) {
      tinymce.activeEditor.execCommand("mceInsertContent", false, value);
    },

    /**
         *光标处插入
         */
    insertIndex(value, valueIndex) {
      var activeEditor = tinymce.editors[valueIndex];
      activeEditor.execCommand("mceInsertContent", false, value);
    },

    /**
     *获取文本
     */
    getTxt(index = 0) {
      var activeEditor = tinymce.editors[index];
      var editBody = activeEditor.getBody();
      activeEditor.selection.select(editBody);
      return activeEditor.selection.getContent({ format: "text" });
    },
  },
};
</script>
<style scoped></style>
