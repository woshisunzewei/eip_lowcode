<template>
  <a-modal width="700px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">

        <a-form-model-item label="标题" prop="title">
          <a-input v-model="form.title" placeholder="请输入标题" allow-clear />
        </a-form-model-item>

        <a-form-model-item label="开始时间" prop="beginTime">
          <a-date-picker style="width: 100%" v-model="form.beginTime" format="YYYY-MM-DD HH:mm:ss" :show-time="true" />
        </a-form-model-item>

        <a-form-model-item label="置顶" prop="isTop">
          <a-switch v-model="form.isTop" />
        </a-form-model-item>

        <a-form-model-item label="发布时间" prop="publicTime">
          <a-date-picker style="width: 100%" v-model="form.publicTime" format="YYYY-MM-DD HH:mm:ss" :show-time="true" />
        </a-form-model-item>
        <a-form-model-item label="结束时间" prop="endTime">
          <a-date-picker style="width: 100%" v-model="form.endTime" format="YYYY-MM-DD HH:mm:ss" :show-time="true" />
        </a-form-model-item>
        <a-form-model-item label="排序号" prop="orderNo">
          <a-input-number id="orderNo" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
            :max="999" />
        </a-form-model-item>

        <a-form-model-item label="简介" prop="introduction">
          <a-input v-model="form.introduction" type="textarea" placeholder="请输入简介" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="内容" prop="msg">
          <eip-editor ref="editor" v-if="tinymce.show" v-model="form.msg" :height="tinymce.height"></eip-editor>
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!noticeId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!noticeId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="noticeId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>
<script>
import { save, findById } from "@/services/system/notice/edit";
import { newGuid } from "@/utils/util";
import moment from "moment";
export default {
  name: "edit",
  data() {
    return {
      tinymce: {
        plugins: "preview  fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 200,
        show: false,
        menubar: "",
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },

      form: {
        noticeId: newGuid(),
        publicTime: null,
        title: null,
        orderNo: 1,
        isTop: false,
        introduction: null,
        msg: null,
        endTime: null,
        beginTime: null,
      },

      rules: {
        title: [
          {
            required: true,
            message: "请输入标题",
            trigger: "blur",
          },
        ],
        publicTime: [
          {
            required: true,
            message: "请选择发布时间",
            trigger: ["blur", "change"],
          },
        ],
        beginTime: [
          {
            required: true,
            message: "请选择开始时间",
            trigger: ["blur", "change"],
          },
        ],
        endTime: [
          {
            required: true,
            message: "请选择结束时间",
            trigger: ["blur", "change"],
          },
        ],
      },

      loading: false,
      spinning: false,
      save: {
        continue: false,
        retain: false,
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    noticeId: {
      type: String,
    },
    title: {
      type: String,
    },
  },

  mounted() {
    setTimeout(() => {
      this.tinymce.show = true;
    }, 10);
    this.find();
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
    saveConfirm() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.spinning = true;
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.noticeId = newGuid();
              } else {
                that.cancel();
              }
              that.$emit("ok", result);
            } else {
              that.$message.error(result.msg);
            }
            that.loading = false;
            that.spinning = false;
          });
        } else {
          return false;
        }
      });
    },
    /**
     *
     */
    saveContinue() {
      this.save.continue = true;
      this.saveConfirm();
    },

    /**
     *
     */
    saveClose() {
      this.save.continue = false;
      this.saveConfirm();
    },
    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      if (this.noticeId) {
        that.spinning = true;
        findById(this.noticeId).then(function (result) {
          if (result.code == that.eipResultCode.success) {
            var data = result.data;
            if (data.publicTime) {
              data.publicTime = moment(data.publicTime);
            }
            if (data.beginTime) {
              data.beginTime = moment(data.beginTime);
            }
            if (data.endTime) {
              data.endTime = moment(data.endTime);
            }
            that.form = data;
          }
          that.spinning = false;
        });
      }
    },
  },
};
</script>

<style lang="less">
.edui-default {
  line-height: 20px !important;
}
</style>
