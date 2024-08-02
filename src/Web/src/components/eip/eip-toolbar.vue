<template>
  <a-space>
    <template v-for="(button, index) in buttons">
      <a-tooltip :title="button.remark">
        <a-button v-if="button.style != 'upload'" :shape="button.shape" :type="button.style" :key="index"
          @click="click(button.method)">
          <a-icon :type="button.icon" :theme="button.theme" />
          {{ button.name }}</a-button>
      </a-tooltip>
      <a-upload v-if="button.style == 'upload'" :key="index"
        accept="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet, application/vnd.ms-excel"
        :showUploadList="false" @change="uploadChange(button)" :customRequest="uploadCustomRequest">
        <a-button type="primary" :shape="button.shape">
          <a-icon :type="button.icon" :theme="button.theme" /> {{ button.name }}
        </a-button>
      </a-upload>
    </template>
    <a-dropdown v-if="mores.length > 0">
      <a-menu slot="overlay">
        <a-menu-item @click="click(button.method)" v-for="(button, indexm) in mores" :key="indexm">
          <a-icon :type="button.icon" :theme="button.theme" />
          {{ button.name }}
        </a-menu-item>
      </a-menu>
      <a-button v-show="mores.length > 0">
        更多 <a-icon type="down" />
      </a-button>
    </a-dropdown>
  </a-space>
</template>

<script>
import { menubutton } from "@/services/common/usercontrol/toolbar";
export default {
  name: "eip-toolbar",
  data() {
    return {
      buttons: [],
      mores: [],
      upload: {
        file: null,
      },
    };
  },
  props: {
    option: {
      type: Object,
    },
  },
  created() {
    this.init();
  },
  methods: {
    /**
     * 按钮初始化
     */
    init() {
      let that = this;
      const route = this.$route;
      menubutton({ menuId: route.meta.menuId }).then((result) => {
        if (result.code == that.eipResultCode.success) {
          var buttons = result.data;
          for (let index = 0; index < buttons.length; index++) {
            const element = buttons[index];
            if (index < 8) {
              that.buttons.push(element);
            } else {
              that.mores.push(element);
            }
          }
          //触发加载完毕事件
          this.$emit("onload", buttons);
        }
      });
    },

    /**
     * 点击触发
     */
    click(method) {
      this.$emit(method);
    },

    /**
     *
     */
    uploadCustomRequest(data) {
      this.upload.file = data;
    },

    /**
     * 上传改变
     */
    uploadChange(button) {
      this.$emit(button.method, this.upload.file);
    },
  },
};
</script>