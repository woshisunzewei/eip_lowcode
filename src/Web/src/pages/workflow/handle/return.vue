<template>
  <a-modal :zIndex="10001" width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="任务退回" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-form-model ref="form" :model="form" layout="vertical" :rules="rules">
          <a-row>
            <a-col>
              <a-form-model-item label="">
                <a-alert message="任务退回" description="可将任务退回到1个或多个并行的关卡，退回关卡可以直送或按正常审批到本关卡。" type="warning" />
              </a-form-model-item>
              <a-form-model-item label="退回到" prop="activityId">
                <a-select placeholder="请选择退回步骤" v-model="form.activityId">
                  <a-select-option v-for="(item, i) in activity" :key="i" :value="item.activityId">{{ item.title
                    }}</a-select-option>
                </a-select>
              </a-form-model-item>
              <a-form-model-item label="退回理由" prop="comment">
                <a-input allow-clear v-model="form.comment" type="textarea" placeholder="请输入退回理由" />
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
import { workflowEngineReturnActivity } from "@/services/workflow/send/index";
export default {
  components: {},
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: { activityId: undefined, comment: "" },
      activity: [],
      rules: {
        comment: [
          {
            required: true,
            message: "请输入理由",
            trigger: ["blur", "change"],
          },
        ],
        activityId: [
          {
            required: true,
            message: "请选择退回步骤",
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

    processInstanceId: {
      type: String,
    },

    taskId: {
      type: String,
    },
  },
  mounted() {
    this.initReturnActivity();
  },
  methods: {
    /**
     *
     */
    initReturnActivity() {
      let that = this;
      workflowEngineReturnActivity({
        processInstanceId: this.processInstanceId,
        taskId: this.taskId,
      }).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.activity = result.data;
        }
      });
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
          this.$emit("confirm", {
            activityId: that.form.activityId,
            comment: that.form.comment,
          });
        } else {
          return false;
        }
      });
    },
  },
};
</script>