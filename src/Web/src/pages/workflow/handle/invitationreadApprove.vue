<template>
  <a-modal :zIndex="10001" width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="督办" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-form-model ref="form" :model="form" layout="vertical" :rules="rules">
          <a-row>
            <a-col>
              <a-form-model-item label="意见" prop="comment">
                <a-textarea v-model="form.comment" placeholder="请输入意见" :rows="4" />
              </a-form-model-item>

              <a-form-model-item label="附件">
                <a-upload :file-list="files.fileList" @change="filesHandleChange" :customRequest="filesBeforeUpload"
                  :remove="removeFile">
                  <a-button> <a-icon type="upload" /> 点击上传 </a-button>
                </a-upload></a-form-model-item>
            </a-col>
          </a-row>
        </a-form-model>
      </a-spin>
    </div>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
  </a-modal>
</template>

<script>
import { upload, fileDel } from "@/services/workflow/handle/invitationreadSure";
export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      files: {
        loading: false,
        imageUrl: "",
        file: null,
        fileList: [],
        check: false,
      },
      form: {
        comment: null,
      },
      rules: {
        comment: [
          {
            required: true,
            message: "请输入意见",
            trigger: ["blur", "change"],
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
    title: {
      type: String,
    },

    taskId: {
      type: String,
    },
  },
  mounted() { },
  methods: {
    /**
     *移除文件
     * @param {*} file
     */
    removeFile(file) {
      let that = this;
      fileDel({ id: file.fileId }).then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
      });
    },
    /**
     * 值改变
     * @param {*} info
     */
    filesHandleChange(info) {
      this.files.file = info.file;
      this.files.fileList = info.fileList;
    },
    /**
     * 自定义上传文件
     * @param {*} data
     */
    filesBeforeUpload(data) {
      var file = data.file;
      let that = this;
      this.files.loading = true;
      let fd = new FormData(); //表单格式
      fd.append("file", file); //添加file表单数据
      fd.append("correlationId", this.taskId); //添加file表单数据
      fd.append("single", false); //添加file表单数据
      upload(fd).then((result) => {
        if (result.code === that.eipResultCode.success) {
          that.$message.success(result.msg);
          that.files.file.status = "done";
          that.files.file.fileId = result.data[0].fileId;
          that.files.fileList[that.files.fileList.length - 1].url =
            result.data[0].path;

          that.files.loading = false;
        } else {
          that.$message.error(result.msg);
        }
      });
      return false;
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
    save() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.$emit("confirm", that.form.comment);
          that.cancel();
        } else {
          return false;
        }
      });
    },
  },
};
</script>