<template>
  <a-form-model :layout="form.layout">
    <a-form-model-item label="选择数据对象" extra="当前流程中的节点对象，作为子流程数据源">
      <a-select v-model="drawerData.source" showSearch allowClear :placeholder="`请选择`" @change="sourceChange">
        <a-select-option v-for="item in configList.filter(f => [4, 21, 22].includes(f.type))" :key="item.id"
          :value="item.id">{{ item.title }}</a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="配置子流程" extra="">
      <a-button type="primary" @click="subProcessConfig">
        配置子流程
      </a-button>
    </a-form-model-item>
    <a-form-model-item label="选项" extra="">
      <a-checkbox v-model="drawerData.wait">
        子流程执行完毕后，再开始下一个节点
      </a-checkbox>
    </a-form-model-item>

    <automationDesigner ref="automationDesigner" v-if="options.visible" :visible.sync="options.visible"
      :title="options.title" :configId="options.configId" @close="designerClose"></automationDesigner>

  </a-form-model>

</template>

<script>
import {
  save,
} from "@/services/system/agile/automation/edit";

const automationDesigner = () => import('../../designer')
import { newGuid } from "@/utils/util";
export default {
  name: "subProcess",
  components: {
    automationDesigner
  },
  props: {
    title: {
      type: String,
    },
    configList: {
      required: true,
      type: Array,
      default: () => { return [] }
    },
    drawerData: {
      required: true,
      type: Object
    }
  },
  data() {
    return {
      form: {
        layout: 'horizontal'
      },
      options: {
        visible: false,
        configId: "",
        title: "",
        bodyStyle: {
          padding: "0",
        },
      },
    }
  },
  computed: {

  },
  mounted() {
    this.init();
  },
  methods: {
    init() {
    },
    designerClose() {
    },
    /**
     * 数据源改变
     */
    sourceChange(configId) {
      var table = this.$utils.find(this.configList, item => item.id == configId);
      switch (table.type) {
        case 4:
          var columns = table.data.columns;
          this.drawerData.columns = [];
          columns.forEach(c => this.drawerData.columns.push({
            model: c.model,
            label: c.label
          }))
          break;
      }
    },

    /**
     * 子流程配置
     */
    subProcessConfig() {
      let that = this;
      let form = {
        configId: this.drawerData.configId,
        dataFromName: null,
        name: this.title,
        orderNo: 1,
        isFreeze: false,
        tableId: undefined,
        remark: null,
        configType: undefined,
        saveJson: JSON.stringify({
          type: 301,
          id: newGuid(),
          title: this.title,
          data: {
            columns: this.drawerData.columns
          },
          conditionNodes: [],
          childNode: {}
        })
      }
      this.options.configId = form.configId;
      this.options.title = this.title;
      that.$loading.show({ text: that.eipMsg.saveLoading });
      save(form).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          //保存信息
          that.options.visible = true;
        } else {
          that.$message.error(result.msg);
        }
        that.loading = false;
        that.$loading.hide();
      });

    }
  }
}
</script>

<style scoped>
.getSingleData {
  padding: 0 20px;
  color: #606266;
}

.getMethod {
  padding: 16px 0;
  border-bottom: 1px solid #F1F2F3;
}

.getMethod_title {
  font-size: 14px;
  color: #303233;
}

.getMethod_from {
  margin: 8px 0;
}

.getMethod_select {
  margin-top: 24px;
  font-size: 12px;
}

.getMethod_select .a-select {
  margin: 0 8px;
}

/deep/ .info {
  padding: 16px 0;
  font-size: 12px;
  width: 100% !important
}

/deep/.ant-form-item-label>label {
  font-weight: bold;
}
</style>./subProcess.vue
