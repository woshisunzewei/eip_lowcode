<template>
  <a-drawer v-if="options.showFormType == 'drawer'" :width="width" :visible="visible" :destroyOnClose="true"
    :closable="true" :title="title" :maskClosable="options.maskClosable" :bodyStyle="{ padding: '1px' }"
    @close="cancel">
    <page :options="options" :config="config" :update="update" :copy="copy" :customerFormValue="customerFormValue"
      :disabled="disabled" :fromType="fromType" :isWorkflow="isWorkflow" :workflowData="workflowData" :title="title"
      @cancel="cancel" @ok="ok" @logContract="logContract">
    </page>
  </a-drawer>

  <a-modal v-else v-drag :width="width" :visible="visible" :destroyOnClose="true" :centered="options.modalCentered"
    :title="title" :maskClosable="options.maskClosable" :bodyStyle="{ padding: '1px' }" @cancel="cancel" :footer="null">
    <page :disabled="disabled" :options="options" :config="config" :update="update" :copy="copy"
      :customerFormValue="customerFormValue" :fromType="fromType" :isWorkflow="isWorkflow" :workflowData="workflowData"
      :title="title" :automation="automation" @cancel="cancel" @ok="ok" @logContract="logContract">
    </page>
  </a-modal>
</template>

<script>
import page from "./page";
export default {
  components: {
    page
  },
  name: "edit",
  data() {
    return {
      width: this.options.showFormWidth + this.options.showFormWidthUnit
    };
  },

  props: {

    //是否为工作流
    isWorkflow: {
      type: Boolean,
      default: false,
    },

    //流程数据
    workflowData: {
      type: Object,
      default: () => {
        return {
          processId: undefined, //流程实例Id,发起流程时用
          processInstanceId: undefined,
          taskId: undefined,
          activityId: undefined,
          type: 1, //处理类型1审核，2知会，3加签，4阅示，5流程监控，6阅示审批处理,有通过和拒绝按钮
        };
      },
    },

    visible: {
      type: Boolean,
      default: false,
    },

    config: {
      type: Object,
      default: () => {
        return {
          configId: undefined, //配置id
        };
      },
    },

    update: {
      type: Object,
      default: () => {
        return {
          RelationId: undefined, //配置id
        };
      },
    },

    /**
     * 表单配置属性
     */
    options: {
      type: Object
    },

    //编辑信息,如记录IDId
    copy: {
      type: Boolean,
      default: false,
    },

    //禁用
    disabled: {
      type: Boolean,
      default: false,
    },

    title: {
      type: String,
    },

    //自定义表单数据
    customerFormValue: {
      type: Object
    },

    //自动化编辑字段,若有值则以此为准
    automation: {
      type: Object,
    },

    //来源类型:可用于通过不同来源类型,处理不同业务逻辑
    fromType: {
      type: String,
    }
  },

  methods: {
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    ok(result) {
      this.$emit("ok", result);
    },

    logContract(drawerRightShow) {
      if (drawerRightShow && this.config.showFormWidthUnit == ('px')) {
        this.width = (parseFloat(this.config.showFormWidth) + 400) + 'px'
      } else {
        this.width = this.config.showFormWidth + this.config.showFormWidthUnit;
      }
    }
  },
};
</script>

<style lang="less" scoped>
.title {
  font-weight: 600 !important;
  font-size: 16px;
  color: #304265;
}

/deep/.ant-timeline-item-content {
  margin: 0 0 0 30px !important
}

.close {
  color: #ff4d4f !important;
  cursor: pointer;

  .text {
    font-size: 14px;
  }
}
</style>
