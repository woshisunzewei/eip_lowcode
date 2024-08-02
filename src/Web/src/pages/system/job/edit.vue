<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="作业名称" prop="jobName">
              <a-input allow-clear v-model="form.jobName" placeholder="请输入作业名称" />
            </a-form-model-item>

            <!-- <a-form-model-item :default-value="form.triggerState" label="触发器状态" prop="triggerState"
                @change="triggerStateChange">
                <a-select default-value="Normal" style="width: 100%">
                  <a-select-option value="Normal">运行中</a-select-option>
                  <a-select-option value="Paused">暂停中</a-select-option>
                </a-select>
              </a-form-model-item> -->
            <!-- <a-form-model-item label="触发器类型" prop="triggerType">
                <a-select :default-value="form.triggerType" style="width: 100%" @change="triggerTypeChange">
                  <a-select-option value="CronTriggerImpl">CronTriggerImpl</a-select-option>
                  <a-select-option value="SimpleTriggerImpl">SimpleTriggerImpl</a-select-option>
                </a-select>
              </a-form-model-item> -->
            <a-form-model-item label="命名空间" prop="namespaceName">
              <a-input v-model="form.namespaceName" placeholder="请输入命名空间:EIP.Job" />
            </a-form-model-item>
            <a-form-model-item label="类名" prop="className">
              <a-input v-model="form.className" placeholder="请输入类名:DatabaseBackupJob" />
            </a-form-model-item>
            <a-form-model-item label="cron表达式" prop="expression" v-if="form.triggerType == 'CronTriggerImpl'">
              <a-input allow-clear placeholder="corn表达式" v-model="form.expression" @change="handleOK">
                <a-icon slot="suffix" type="edit" @click="openModal" title="corn控件" />
              </a-input>
            </a-form-model-item>
            <!-- <a-form-model-item label="选项" prop="replaceExists">
                <a-checkbox v-model="form.replaceExists">根据JobKey/TriggerKey替换现有任务</a-checkbox>
              </a-form-model-item> -->
            <a-form-model-item label="作业描述" prop="jobDescription">
              <a-textarea allow-clear v-model="form.jobDescription" placeholder="请输入作业描述" />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>
    <eip-cron ref="innerVueCron" :expression="form.expression" @ok="handleOK"></eip-cron>
  </a-modal>
</template>

<script>
import { findByJobName, save } from "@/services/system/job/edit";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  data() {
    return {
      bodyStyle: {
        padding: "4px",
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        jobName: null,
        namespaceName: "EIP.Job",
        className: null,
        expression: null,
        jobDescription: null,

        replaceExists: true,
        jobGroup: null,
        triggerGroup: null,
        triggerName: null,
        triggerState: "Normal",
        triggerType: "CronTriggerImpl",
        parametersJson: ''
      },

      rules: {
        jobName: [
          {
            required: true,
            message: "请输入作业名称",
            trigger: "blur",
          },
        ],

        namespaceName: [
          {
            required: true,
            message: "请输入命名空间",
            trigger: "blur",
          },
        ],
        className: [
          {
            required: true,
            message: "请输入类名",
            trigger: "blur",
          },
        ],
        expression: [
          {
            required: true,
            message: "请输入Cron表达式",
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
    jobName: {
      type: String,
    },
    jobGroup: {
      type: String,
    },
    triggerName: {
      type: String,
    },
    triggerGroup: {
      type: String,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.find();
  },
  methods: {
    openModal() {
      this.$refs.innerVueCron.show()
    },
    handleOK(val) {
      this.form.expression = val
    },
    triggerStateChange(value) {
      this.form.triggerState = value;
    },
    /**
     *类型改变
     */
    triggerTypeChange(value) {
      this.form.triggerType = value;
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
          that.loading = true;
          that.spinning = true;
          that.form.jobGroup = that.jobName ? that.form.jobGroup : that.form.jobName + "组";
          that.form.triggerGroup = that.jobName ? that.form.triggerGroup : that.form.jobName + "触发器组";
          that.form.triggerName = that.jobName ? that.form.triggerName : that.form.jobName + "触发器";
          that.form.parametersJson = that.jobName ? that.form.parametersJson : JSON.stringify([{ key: "correlation", value: newGuid() }])
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.cancel();
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
     * 根据Id查找
     */
    find() {
      let that = this;
      if (this.jobName) {
        that.spinning = true;
        findByJobName({
          jobName: this.jobName,
          jobGroup: this.jobGroup,
          triggerName: this.triggerName,
          triggerGroup: this.triggerGroup,
        }).then(function (result) {
          that.spinning = false;
          that.form.jobName = result.jobName;
          that.form.namespaceName = result.namespaceName;
          that.form.className = result.className;
          that.form.expression = result.expression;
          that.form.jobDescription = result.jobDescription;
          that.form.parametersJson = result.parametersJson;
          that.form.replaceExists = result.replaceExists;
          that.form.jobGroup = result.jobGroup;
          that.form.triggerGroup = result.triggerGroup;
          that.form.triggerName = result.triggerName;
          that.form.triggerState = result.triggerState;
          that.form.triggerType = result.triggerType;
        });
      }
    },
  },
};
</script>

<style scoped>
/deep/.ant-input[disabled] {
  border: 1px solid #d9d9d9 !important;
}
</style>