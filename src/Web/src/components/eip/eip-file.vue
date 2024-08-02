<template>
  <span>
    <span
      :key="index"
      v-for="(sfileItem, index) in files"
      @click="prevFile(sfileItem)"
    >
      <a-tooltip placement="bottomLeft">
        <template slot="title">
          {{ sfileItem.name }}
          <a-icon
            :title="'下载' + sfileItem.name"
            @click="downloadFile(sfileItem)"
            class="margin-left"
            type="vertical-align-bottom"
          />
        </template>

        <img
          v-if="
            sfileItem.type == 'img' ||
            ['jpg', 'png', 'jpeg', 'gif'].includes(
              sfileItem.name.substring(sfileItem.name.lastIndexOf('.') + 1)
            )
          "
          :src="sfileItem.url"
          v-real-img="sfileItem.url"
          class="eip-table-file"
        />

        <img
          v-else-if="
            [
              'txt',
              'apk',
              'bt',
              'exe',
              'flash',
              'mp3',
              'mp4',
              'pdf',
              'psd',
              'ttf',
              'zip',
            ].includes(
              sfileItem.name.substring(sfileItem.name.lastIndexOf('.') + 1)
            )
          "
          :src="
            '/images/material/' +
            sfileItem.name.substring(sfileItem.name.lastIndexOf('.') + 1) +
            '.png'
          "
          class="eip-table-file"
        />

        <img
          v-else-if="
            ['doc', 'docx'].includes(
              sfileItem.name.substring(sfileItem.name.lastIndexOf('.') + 1)
            )
          "
          src="/images/material/doc.png"
          class="eip-table-file"
        />

        <img
          v-else-if="
            ['xls', 'xlsx'].includes(
              sfileItem.name.substring(sfileItem.name.lastIndexOf('.') + 1)
            )
          "
          src="/images/material/xls.png"
          class="eip-table-file"
        />

        <img
          v-else-if="
            ['htm', 'html'].includes(
              sfileItem.name.substring(sfileItem.name.lastIndexOf('.') + 1)
            )
          "
          src="/images/material/htm.png"
          class="eip-table-file"
        />

        <img
          v-else-if="
            ['ppt', 'pptx'].includes(
              sfileItem.name.substring(sfileItem.name.lastIndexOf('.') + 1)
            )
          "
          src="/images/material/ppt.png"
          class="eip-table-file"
        />

        <img v-else src="/images/material/file.png" class="eip-table-file" />
      </a-tooltip>
    </span>
    <a-drawer
      :destroyOnClose="true"
      :maskClosable="false"
      width="100%"
      :visible="prev.visible"
      :footer="null"
      :title="prev.title"
      @close="prev.visible = false"
      :bodyStyle="{ padding: '0px' }"
    >
      <a-spin :spinning="spinning">
        <vue-office-docx
          v-if="['word'].includes(prev.type)"
          :src="prev.url"
          @rendered="renderedHandler"
          @error="errorHandler"
          :style="{ height: height + 'px' }"
        />
        <vue-office-excel
          v-if="['excel'].includes(prev.type)"
          :src="prev.url"
          @rendered="renderedHandler"
          @error="errorHandler"
          :style="{ height: height + 'px' }"
        />
        <vue-office-pdf
          v-if="['pdf'].includes(prev.type)"
          :src="prev.url"
          @rendered="renderedHandler"
          @error="errorHandler"
          :style="{ height: height + 'px' }"
        />
      </a-spin>
    </a-drawer>

    <div class="images" v-viewer="{ movable: false }" style="display: none">
      <img
        ref="viewer"
        :alt="previewImageName"
        v-viewer
        style="width: 100%"
        :src="previewImageUrl"
      />
    </div>
  </span>
</template>

<script>
import Vue from "vue";
import Viewer from "v-viewer";
import "viewerjs/dist/viewer.css";

Vue.use(Viewer);
Viewer.setDefaults({
  Options: {
    inline: true, // 启用 inline 模式
    button: true, // 显示右上角关闭按钮
    navbar: true, // 显示缩略图导航
    title: true, // 显示当前图片的标题
    toolbar: true, // 显示工具栏
    tooltip: true, // 显示缩放百分比
    movable: true, // 图片是否可移动
    zoomable: true, // 图片是否可缩放
    //'rotatable': true, // 图片是否可旋转
    //'scalable': true, // 图片是否可翻转
    transition: true, // 使用 CSS3 过度
    //'fullscreen': true, // 播放时是否全屏
    keyboard: true, // 是否支持键盘
    url: "data-source", // 设置大图片的 url
  },
});
//引入相关样式
import "@vue-office/docx/lib/index.css";
import "@vue-office/excel/lib/index.css";
//引入VueOffice组件
import VueOfficeDocx from "@vue-office/docx";
import VueOfficeExcel from "@vue-office/excel";
import VueOfficePdf from "@vue-office/pdf";
export default {
  name: "eip-file",
  data() {
    return {
      prev: {
        title: "",
        type: "",
        visible: false,
      },
      spinning: true,
      previewImageUrl: "",
      previewImageName: "",
      height: this.eipHeaderHeight() + 36,
    };
  },
  components: {
    VueOfficeDocx,
    VueOfficeExcel,
    VueOfficePdf,
  },
  props: {
    files: {
      type: Array,
    },
  },
  methods: {
    /**
     * 渲染完成
     */
    renderedHandler() {
      this.spinning = false;
    },

    /**
     * 渲染失败
     */
    errorHandler() {
      this.$message.error("打开失败");
      this.spinning = false;
    },
    /**
     * 下载文件
     * @param {*} file
     */
    downloadFile(file) {
      let that = this;
      that.$message.loading("下载中,请稍等...", 0);
      fetch(file.url)
        .then((response) => {
          response.blob().then((blob) => {
            that.$XSaveFile({
              filename: file.name.split(".")[0],
              type: file.name.substring(file.name.lastIndexOf(".") + 1),
              content: blob,
            });
            that.$message.destroy();
          });
        })
        .catch((err) => {
          that.$message.destroy();
          that.$message.error(err);
        });
    },

    /**
     * 预览文件
     * @param {*} file
     */
    prevFile(sfileItem) {
      var type = sfileItem.name.substring(sfileItem.name.lastIndexOf(".") + 1);
      if (["jpg", "png", "jpeg", "gif"].includes(type)) {
        this.previewImageName = sfileItem.name;
        this.previewImageUrl = sfileItem.url;
        const viewer = this.$el.querySelector(".images").$viewer;
        viewer.show();
      }

      if (["pdf"].includes(type)) {
        this.prev.type = "pdf";
        this.prev.url = sfileItem.url;
        this.prev.title = sfileItem.name;
        this.prev.visible = true;
      }

      if (["doc", "docx"].includes(type)) {
        this.prev.type = "word";
        this.prev.url = sfileItem.url;
        this.prev.title = sfileItem.name;
        this.prev.visible = true;
      }

      if (["xls", "xlsx"].includes(type)) {
        this.prev.type = "excel";
        this.prev.url = sfileItem.url;
        this.prev.title = sfileItem.name;
        this.prev.visible = true;
      }

      if (["htm", "html"].includes(type)) {
        this.prev.type = "html";
        this.prev.url = sfileItem.url;
        this.prev.title = sfileItem.name;
        this.prev.visible = true;
      }

      if (["ppt", "pptx"].includes(type)) {
        this.prev.type = "ppt";
        this.prev.url = sfileItem.url;
        this.prev.title = sfileItem.name;
        this.prev.visible = true;
      }
    },
  },
};
</script>

<style lang="less" scoped>
.eip-table-file {
  max-width: 200;
  height: 40px;
  padding: 2px;
  cursor: pointer;
}
</style>
