<template>
  <a-drawer v-if="jsonData.showFormType == 'drawer'" title="预览" :visible="visible" @close="handleCancel"
    :destroyOnClose="true" :maskClosable="jsonData.config.maskClosable" :bodyStyle="{ padding: '1px' }"
    :width="jsonData.config.showFormWidth + jsonData.config.showFormWidthUnit">
    <div class="eip-drawer-body " style="overflow: hidden;height: calc(100vh - 110px) ">
      <k-form-build :value="jsonData" @change="handleChange" @submit="handleSubmit" ref="KFormBuild" />
      <jsonModel ref="jsonModel" />
    </div>
    <div class="eip-drawer-toolbar">
      <div class="flex justify-between align-center">
        <div></div>
        <a-space>
          <a-button @click="handleCancel">关闭</a-button>
          <a-button @click="handleGetData" type="primary">获取数据</a-button>
        </a-space>
      </div>
    </div>
  </a-drawer>

  <a-modal v-else :width="jsonData.config.showFormWidth + jsonData.config.showFormWidthUnit" v-drag
    :destroyOnClose="true" :maskClosable="jsonData.config.maskClosable" :centered="jsonData.config.modalCentered"
    :visible="visible" :bodyStyle="{ padding: '1px' }" title="预览" @cancel="handleCancel">
    <k-form-build :value="jsonData" @change="handleChange" @submit="handleSubmit" ref="KFormBuild" />
    <jsonModel ref="jsonModel" />
    <template slot="footer">
      <a-space>
        <a-button @click="handleCancel">关闭</a-button>
        <a-button @click="handleGetData" type="primary">获取数据</a-button>
      </a-space>
    </template>
  </a-modal>
</template>
<script>
/*
 * author kcz
 * date 2019-11-20
 */
import jsonModel from "../KFormDesign/module/jsonModal";
import { dialogStyle, bodyStyle } from "../../config/modal.js";
export default {
  name: "KFormPreview",
  data() {
    return {
      visible: false,
      previewWidth: 850,
      jsonData: {
        "config": {
          "hideRequiredMark": false,
          "customStyle": "",
          "labelLayout": "flex",
          "labelWidth": 100,
          "labelBold": true,
          "showFormType": "modal",
          "showFormWidthUnit": "px",
          "showFormWidth": "600",
          "modalCentered": false,
          "maskClosable": false,
          "zIndex": 1000
        }
      },
      dialogStyle,
      bodyStyle
    };
  },
  components: {
    jsonModel
  },
  methods: {
    /**
     * 按钮触发提交
     * @param {*} p
     */
    handleSubmit(p) {
      p()
        .then(res => {
          console.log(res, "获取数据成功");
          this.$refs.jsonModel.jsonData = res;
          this.$refs.jsonModel.visible = true;
        })
        .catch(err => {
          console.error(err, "获取数据失败");
        });
    },
    /**
     * 手动验证获取表单数据
     */
    async handleGetData() {
      this.$refs.KFormBuild.getData()
        .then(res => {
          console.log(res, "获取数据成功");
          this.$refs.jsonModel.jsonData = res;
          this.$refs.jsonModel.visible = true;
        })
        .catch(err => {
          console.log(err, "获取数据失败");
        });
    },
    /**
     * 监听表单change 事件
     * @param {*} value
     * @param {*} key
     */
    handleChange(value, key) {
      console.log(value, key);
    },
    handleCancel() {
      this.visible = false;
    }
  }
};
</script>
