<!--
 * @Description: 对上传图片组件进行封装
 * @Author: kcz
 * @Date: 2020-03-17 12:53:50
 * @LastEditors: kcz
 * @LastEditTime: 2022-10-26 21:14:16
 -->
<template>
  <div :style="{ width: record.options.width }" class="upload-img-box-9136076486841527">
    <Upload :name="config.uploadImageName || record.options.fileName"
      :headers="config.uploadImageHeaders || record.options.headers" :data="config.uploadImageData || optionsData"
      :action="config.uploadImage || record.options.action" :multiple="record.options.multiple"
      :listType="record.options.listType" :disabled="record.options.disabled || parentDisabled" :fileList="fileList"
      accept="image/*" @change="handleChange" @preview="handlePreview" :remove="remove" :beforeUpload="beforeUpload">
      <Button v-if="record.options.listType !== 'picture-card' &&
        fileList.length < record.options.limit
      " :disabled="record.options.disabled || parentDisabled">
        <a-icon type="upload" /> {{ record.options.placeholder }}
      </Button>
      <div v-if="record.options.listType === 'picture-card' &&
        fileList.length < record.options.limit
      " :disabled="record.options.disabled || parentDisabled">
        <a-icon type="plus" />
        <div class="ant-upload-text">{{ record.options.placeholder }}</div>
      </div>
    </Upload>
    <div class="images" v-viewer="{ movable: false }" style="display: none">
      <img ref="viewer" alt="图片详情" v-viewer style="width: 100%" :src="previewImageUrl" />
    </div>

  </div>
</template>
<script>
import Vue from 'vue'
import Viewer from 'v-viewer'
import 'viewerjs/dist/viewer.css'

Vue.use(Viewer)
Viewer.setDefaults({
  Options: {
    'inline': true, // 启用 inline 模式
    'button': true, // 显示右上角关闭按钮
    'navbar': true, // 显示缩略图导航
    'title': true, // 显示当前图片的标题
    'toolbar': true, // 显示工具栏
    'tooltip': true, // 显示缩放百分比
    'movable': true, // 图片是否可移动
    'zoomable': true, // 图片是否可缩放
    //'rotatable': true, // 图片是否可旋转
    //'scalable': true, // 图片是否可翻转
    'transition': true, // 使用 CSS3 过度
    //'fullscreen': true, // 播放时是否全屏
    'keyboard': true, // 是否支持键盘
    'url': 'data-source' // 设置大图片的 url
  }
})
/*
 * author kcz
 * date 2019-12-31
 * description 上传图片组件
 */
import { pluginManager } from "../../utils/index";
import { message } from "ant-design-vue";
import {
  eventApi
} from "@/services/system/agile/run/edit";
const Upload = pluginManager.getComponent("upload");
const Button = pluginManager.getComponent("aButton").component;
export default {
  name: "KUploadImg",
  // eslint-disable-next-line vue/require-prop-types
  props: ["record", "value", "config", "parentDisabled"],
  components: {
    Upload: Upload.component,
    Button
  },
  data() {
    return {
      fileList: [],
      previewImageUrl: ""
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
      deep: true
    }
  },
  computed: {
    optionsData() {
      try {
        return JSON.parse(this.record.options.data);
      } catch (err) {
        console.error(err);
        return {};
      }
    }
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
        this.fileList = this.value.filter(f => f.status != 'removed');
      }
    },
    handleSelectChange() {
      setTimeout(() => {
        const arr = this.fileList.map(item => {
          if (typeof item.response !== "undefined") {
            const res = item.response;
            return {
              type: "img",
              name: item.name,
              status: item.status,
              uid: item.uid,
              url: res.data.url || "",
              id: res.data.fileId
            };
          } else {
            return {
              type: "img",
              name: item.name,
              status: item.status,
              uid: item.uid,
              url: item.url || "",
              id: item.fileId
            };
          }
        });
        this.$emit("change", arr);
        this.$emit("input", arr);
      }, 10);
    },

    /**
     * 预览图片
     * @param {*} file 
     */
    handlePreview(file) {
      this.previewImageUrl = file.url || file.thumbUrl;
      const viewer = this.$el.querySelector('.images').$viewer
      viewer.show()
    },

    remove(file) {
      if (this.record.options.remove) {
        eventApi({
          url: this.record.options.remove,
          type: 'POST',
          contentType: "application/x-www-form-urlencoded",
          param: JSON.stringify({
            id: file.id,
          }),
        }).then(apiResult => {
        });
      }

      this.handleSelectChange();
    },
    beforeUpload(e, files) {
      if (files.length + this.fileList.length > this.record.options.limit) {
        message.warning(`最大上传数量为${this.record.options.limit}`);
        files.splice(this.record.options.limit - this.fileList.length);
      }
    },
    handleChange(info) {
      // 上传数据改变时
      this.fileList = info.fileList;
      if (info.file.status === "done") {
        const res = info.file.response;
        if (res.code === 200) {
          this.handleSelectChange();
        } else {
          this.fileList.pop();
          message.error(`图片上传失败`);
        }
      } else if (info.file.status === "error") {
        message.error(`图片上传失败`);
      }
    }
  }
};
</script>
