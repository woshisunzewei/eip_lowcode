<!--
 * @Description: 对上传文件组件进行封装
 * @Author: kcz
 * @Date: 2020-03-17 12:53:50
 * @LastEditors: kcz
 * @LastEditTime: 2022-10-26 21:14:12
 -->
<template>
  <div :style="{ width: record.options.width }">
    <Upload
      v-if="!record.options.drag"
      :disabled="record.options.disabled || parentDisabled"
      :name="config.uploadFileName || record.options.fileName"
      :headers="config.uploadFileHeaders || record.options.headers"
      :data="config.uploadFileData || optionsData"
      :action="config.uploadFile || record.options.action"
      :multiple="record.options.multiple"
      :fileList="fileList"
      @preview="handlePreview"
      @download="handleDownload"
      @change="handleChange"
      :remove="remove"
      :accept="record.options.accept"
      :beforeUpload="beforeUpload"
      :showUploadList="{
        showRemoveIcon: true,
        showDownloadIcon: true,
      }"
    >
      <Button
        v-if="fileList.length < record.options.limit"
        :disabled="record.options.disabled || parentDisabled"
      >
        <a-icon type="upload" /> {{ record.options.placeholder }}
      </Button>
    </Upload>
    <UploadDragger
      v-else
      :class="{ 'hide-upload-drag': !(fileList.length < record.options.limit) }"
      :disabled="record.options.disabled || parentDisabled"
      :name="config.uploadFileName || record.options.fileName"
      :headers="config.uploadFileHeaders || record.options.headers"
      :data="config.uploadFileData || optionsData"
      :action="config.uploadFile || record.options.action"
      :multiple="record.options.multiple"
      :fileList="fileList"
      @preview="handlePreview"
      :preview-file="previewFile"
      @download="handleDownload"
      @change="handleChange"
      :accept="record.options.accept"
      :remove="remove"
      :beforeUpload="beforeUpload"
      :showUploadList="{
        showRemoveIcon: true,
        showDownloadIcon: true,
      }"
    >
      <p class="ant-upload-drag-icon">
        <a-icon type="cloud-upload" />
      </p>
      <p class="ant-upload-text">单击或拖动文件到此区域</p>
    </UploadDragger>

    <a-drawer
      :destroyOnClose="true"
      :maskClosable="false"
      width="100%"
      :visible="visible"
      :footer="null"
      :title="title"
      @close="visible = false"
      :bodyStyle="bodyStyle"
    >
      <a-spin :spinning="spinning">
        <vue-office-docx
          v-if="['doc', 'docx'].includes(type)"
          :src="url"
          @rendered="renderedHandler"
          @error="errorHandler"
          :style="{ height: height + 'px' }"
        />
        <vue-office-excel
          v-if="['xls', 'xlsx'].includes(type)"
          :src="url"
          @rendered="renderedHandler"
          @error="errorHandler"
          :style="{ height: height + 'px' }"
        />
        <vue-office-pdf
          v-if="['pdf'].includes(type)"
          @rendered="renderedHandler"
          @error="errorHandler"
          :src="url"
          :style="{ height: height + 'px' }"
        />
      </a-spin>
    </a-drawer>
    <div class="images" v-viewer="{ movable: false }" style="display: none">
      <img ref="viewer" :alt="title" v-viewer style="width: 100%" :src="url" />
    </div>
  </div>
</template>
<script>
/*
 * author kcz
 * date 2019-12-31
 * description 上传文件组件
 */
import { message } from "ant-design-vue";
import { pluginManager } from "../../utils/index";

const Upload = pluginManager.getComponent("upload");
const UploadDragger = pluginManager.getComponent("uploadDragger");
const Button = pluginManager.getComponent("aButton").component;
//引入相关样式
import "@vue-office/docx/lib/index.css";
import "@vue-office/excel/lib/index.css";

//引入VueOffice组件
import VueOfficeDocx from "@vue-office/docx";
import VueOfficeExcel from "@vue-office/excel";
import VueOfficePdf from "@vue-office/pdf";
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

export default {
  name: "KUploadFile",
  // eslint-disable-next-line vue/require-prop-types
  props: ["record", "value", "config", "parentDisabled", "dynamicData"],
  components: {
    Upload: Upload.component,
    UploadDragger: UploadDragger.component,
    Button,
    VueOfficeDocx,
    VueOfficeExcel,
    VueOfficePdf,
  },
  data() {
    return {
      spinning: true,
      fileList: [],
      url: "",
      visible: false,
      type: "",
      title: "",
      bodyStyle: {
        padding: "0px",
      },
      height: this.eipHeaderHeight() + 36,
    };
  },
  watch: {
    value: {
      // value 需要深度监听及默认先执行handler函数
      handler(val) {
        if (val) {
          this.setFileList();
        }
      },
      immediate: true,
      deep: true,
    },
  },

  computed: {
    optionsData() {
      try {
        return JSON.parse(this.record.options.data);
      } catch (err) {
        console.error(err);
        return {};
      }
    },
  },
  methods: {
    setFileList() {
      // 当传入value改变时，fileList也要改变
      // 如果传入的值为字符串，则转成json
      if (typeof this.value === "string") {
        this.fileList = JSON.parse(this.value);
        // 将转好的json覆盖组件默认值的字符串
        this.handleSelectChange();
      } else {
        this.fileList = this.value.filter((f) => f.status != "removed");
      }
    },
    handleSelectChange() {
      setTimeout(() => {
        const arr = this.fileList.map((item) => {
          if (typeof item.response !== "undefined") {
            const res = item.response;
            return {
              type: "file",
              name: item.name,
              status: item.status,
              uid: res.data.fileId || Date.now(),
              url: res.data.url || "",
            };
          } else {
            return {
              type: "file",
              name: item.name,
              status: item.status,
              uid: item.uid,
              url: item.url || "",
            };
          }
        });

        this.$emit("change", arr);
        this.$emit("input", arr);
      }, 10);
    },

    /**
     * 下载
     * @param {} file
     */
    handleDownload(file) {
      const downloadWay = this.record.options.downloadWay;
      const dynamicFun = this.record.options.dynamicFun;
      if (downloadWay === "a") {
        // 使用a标签下载
        const a = document.createElement("a");
        a.href = file.url || file.thumbUrl;
        a.download = file.name;
        a.click();
      } else if (downloadWay === "ajax") {
        // 使用ajax获取文件blob，并保持到本地
        this.getBlob(file.url || file.thumbUrl).then((blob) => {
          this.saveAs(blob, file.name);
        });
      } else if (downloadWay === "dynamic") {
        // 触发动态函数
        this.dynamicData[dynamicFun](file);
      }
    },

    /**
     * 预览
     * @param {*} file
     */
    handlePreview(file) {
      // 下载文件
      this.title = file.name;
      var type = file.name.split(".").pop();
      switch (type) {
        case "doc":
        case "docx":
        case "xls":
        case "xlsx":
        case "pdf":
          this.visible = true;
          this.url = file.url;
          this.type = type;
          break;
        case "jpg":
        case "png":
        case "jpeg":
        case "gif":
          this.url = file.url;
          const viewer = this.$el.querySelector(".images").$viewer;
          viewer.show();
          break;
      }
    },
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
     * 获取 blob
     * url 目标文件地址
     */
    getBlob(url) {
      return new Promise((resolve) => {
        const xhr = new XMLHttpRequest();

        xhr.open("GET", url, true);
        xhr.responseType = "blob";
        xhr.onload = () => {
          if (xhr.status === 200) {
            resolve(xhr.response);
          }
        };

        xhr.send();
      });
    },
    /**
     * 自定义预览
     * @param {*} file
     */
    previewFile(file) {
      console.log("Your upload file:", file);
      // Your process logic. Here we just mock to the same file
      return fetch("https://next.json-generator.com/api/json/get/4ytyBoLK8", {
        method: "POST",
        body: file,
      })
        .then((res) => res.json())
        .then(({ thumbnail }) => thumbnail);
    },
    /**
     * 保存 blob
     * filename 想要保存的文件名称
     */
    saveAs(blob, filename) {
      if (window.navigator.msSaveOrOpenBlob) {
        navigator.msSaveBlob(blob, filename);
      } else {
        const link = document.createElement("a");
        const body = document.querySelector("body");
        link.href = window.URL.createObjectURL(blob);
        link.download = filename;

        // fix Firefox
        link.style.display = "none";
        body.appendChild(link);

        link.click();
        body.removeChild(link);

        window.URL.revokeObjectURL(link.href);
      }
    },
    remove(file) {
      const index = this.fileList.indexOf(file);
      const newFileList = this.fileList.slice();
      newFileList.splice(index, 1);
      this.fileList = newFileList;
      this.handleSelectChange();
    },
    beforeUpload(e, files) {
      if (files.length + this.fileList.length > this.record.options.limit) {
        message.warning(`最大上传数量为${this.record.options.limit}`);
        files.splice(this.record.options.limit - this.fileList.length);
      }
    },
    handleChange(info) {
      this.fileList = info.fileList;
      if (info.file.status === "done") {
        const res = info.file.response;
        if (res.code === 200) {
          this.handleSelectChange();
        } else {
          this.fileList.pop();
          message.error(`文件上传失败`);
        }
      } else if (info.file.status === "error") {
        message.error(`文件上传失败`);
      }
    },
  },
};
</script>
