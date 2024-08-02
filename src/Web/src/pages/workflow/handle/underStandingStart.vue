<template>
  <a-modal :zIndex="10001" width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" title="任务知会" @cancel="cancel">
    <div style="padding: 10px">
      <a-spin :spinning="spinning">
        <a-form-model ref="form" :model="form" layout="vertical" :rules="rules">
          <a-row>
            <a-col>
              <a-form-model-item label="">
                <a-alert message="任务知会" description="知会他人了解任务。向知会人员发送通知并产生一个知会待办项，知会人员查看后任务消失，系统记录知会的发起和查看过程信息。"
                  type="warning" />
              </a-form-model-item>
              <a-form-model-item label="知会人员" prop="user">
                <a-input v-model="form.userName" placeholder="请选择人员" disabled>
                  <a-icon @click="user.visible = true" style="cursor: pointer" slot="addonAfter" type="search" />
                </a-input>
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

    <eip-user v-if="user.visible" :visible.sync="user.visible" :topOrg="user.topOrg" title="选择人员"
      @ok="chosenUserOk"></eip-user>
  </a-modal>
</template>

<script>

export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        user: null,
        userName: "",
      },
      user: {
        visible: false,
        topOrg: "",
      },
      rules: {
        userName: [
          {
            required: true,
            message: "请选择知会人员",
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
  },
  mounted() { },
  methods: {
    chosenUserOk(data) { },
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