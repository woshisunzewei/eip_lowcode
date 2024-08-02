<template>
  <a-modal :zIndex="10001" width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="退回重填" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-form-model ref="form" :model="form" layout="vertical" :rules="rules">
          <a-row>
            <a-col>
              <a-form-model-item>
                <a-alert message="退回重填" description="将任务退回给发起人，发起人修改后可重新提交也可撤销申请。" type="warning" />
              </a-form-model-item>

              <a-form-model-item label="退回理由" prop="comment">
                <a-input allow-clear v-model="form.comment" type="textarea" placeholder="退回理由" />
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
            message: "请输入退回理由",
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