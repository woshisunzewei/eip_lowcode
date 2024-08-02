<template>
  <a-modal :zIndex="10001" width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="任务拒绝" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-form-model ref="form" :model="form" layout="vertical" :rules="rules">
          <a-row>
            <a-col>
              <a-form-model-item>
                <a-alert message="拒绝任务" description="任务拒绝后只有运维人员才可恢复，如果希望提交人修改申请内容后再审批，请使用【退回重填】或【退回】功能。"
                  type="warning" />
              </a-form-model-item>

              <a-form-model-item prop="comment">
                <a-textarea :auto-size="{ minRows: 4 }" allow-clear v-model="form.comment" placeholder="请输入拒绝理由" />
              </a-form-model-item>
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
export default {
  components: {},
  name: "refuse",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: { comment: "" },

      rules: {
        comment: [
          {
            required: true,
            message: "请输入拒绝理由",
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
    title: {
      type: String,
    },
  },
  mounted() { },
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
      this.$refs.form.validate((valid) => {
        if (valid) {
          this.$emit("confirm", that.form.comment);
        } else {
          return false;
        }
      });
    },
  },
};
</script>