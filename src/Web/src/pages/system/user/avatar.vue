<template>
  <a-modal title="头像上传" v-drag :visible="visible" :destroyOnClose="true" :maskClosable="false"
    :confirm-loading="confirmLoading" :bodyStyle="{ padding: '6px' }" :width="816" @ok="save" @cancel="close">
    <div class="info">
      <div class="info-item mt10">
        <div class="line">
          <div class="cropper-content">
            <div class="cropper">
              <vue-Cropper ref="cropper" :img="option.img" :outputSize="option.outputSize" :outputType="option.outputType"
                :info="true" :centerBox="true" :canScale="option.canScale" :full="option.full" :canMove="option.canMove"
                :canMoveBox="option.canMoveBox" :original="option.original" :autoCrop="option.autoCrop"
                :autoCropWidth="option.autoCropWidth" :autoCropHeight="option.autoCropHeight" :fixedBox="option.fixedBox"
                @realTime="realTime"></vue-Cropper>
            </div>
            <div style="margin-left: 14px">
              <div class="show-preview">
                <div :style="previews.div" class="preview" style="border-radius: 50%">
                  <img :src="previews.url" :style="previews.img" />
                </div>
              </div>
            </div>
          </div>
        </div>

        <a-space style="margin-top: 6px">
          <a-upload name="file" accept="image/png, image/jpeg, image/gif, image/jpg" :before-upload="beforeUpload"
            :show-upload-list="false">
            <a-button type="primary" icon="upload">选择图片</a-button>
          </a-upload>

          <a-button-group>
            <a-button type="primary" icon="plus" @click="changeScale(1)">放大</a-button>
            <a-button type="primary" icon="minus" @click="changeScale(-1)">缩小</a-button>
            <a-button type="primary" icon="undo" @click="rotateLeft">左旋转</a-button>
            <a-button type="primary" icon="redo" @click="rotateRight">右旋转</a-button>
          </a-button-group>
        </a-space>
      </div>
    </div>
  </a-modal>
</template>
  
<script>
import Vue from 'vue'
import VueCropper from 'vue-cropper'

Vue.use(VueCropper)

import { uploadHeadImage } from "@/services/system/user/avatar";
export default {
  name: "avatar",

  data() {
    return {
      id: null,
      confirmLoading: false,
      fileName: "", //本机文件地址
      imgFile: "",
      previews: {},
      uploadImgRelaPath: "", //上传后的图片的地址（不带服务器域名）
      option: {
        img: this.headImage, // 需要剪裁的图片
        autoCropWidth: "200",
        autoCropHeight: "200",
        canScale: true, // 图片是否允许滚轮缩放 默认true
        outputSize: 1, //剪切后的图片质量（0.1-1）
        fixed: true, //是否开启截图框宽高固定比例
        full: false, //输出原图比例截图 props名full
        outputType: "png",
        canMove: true,
        original: false,
        canMoveBox: true,
        autoCrop: true,
        fixedBox: true,
        size: {},
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
    headImage: {
      type: String,
    },
    userId: {
      type: String,
    },
  },

  methods: {
    beforeUpload(file) {
      const reader = new FileReader();
      // 转化为base64
      reader.readAsDataURL(file);
      reader.onload = () => {
        // 获取到需要剪裁的图片 展示到剪裁框中
        this.option.img = reader.result;
      };
      return false;
    },
    close() {
      this.$emit("update:visible", false);
    },

    changeScale(num) {
      num = num || 1;
      this.$refs.cropper.changeScale(num);
    },
    rotateLeft() {
      this.$refs.cropper.rotateLeft();
    },
    rotateRight() {
      this.$refs.cropper.rotateRight();
    },

    realTime(data) {
      this.previews = data;
    },

    uploadImg(e, num) {
      var _this = this;
      var file = e.target.files[0];
      _this.fileName = file.name;
      if (!/\.(gif|jpg|jpeg|png|bmp|GIF|JPG|PNG)$/.test(e.target.value)) {
        alert("图片类型必须是.gif,jpeg,jpg,png,bmp中的一种");
        return false;
      }
      var reader = new FileReader();
      reader.onload = (e) => {
        let data;
        if (typeof e.target.result === "object") {
          data = window.URL.createObjectURL(new Blob([e.target.result]));
        } else {
          data = e.target.result;
        }
        if (num === 1) {
          _this.option.img = data;
        } else if (num === 2) {
          _this.example2.img = data;
        }
      };
      reader.readAsArrayBuffer(file);
    },

    /**
     * 重置密码保存
     */
    save() {
      let that = this;
      that.loading = true;
      that.spinning = true;
      that.$message.loading("正在修改头像", 0);
      this.$refs.cropper.getCropData((data) => {
        uploadHeadImage({ id: data }).then((result) => {
          that.$message.destroy();
          if (result.code === that.eipResultCode.success) {
            that.$message.success(result.msg);
            that.$emit("ok", data);
            that.close();
          } else {
            that.$message.error(result.msg);
          }
          that.loading = false;
          that.spinning = false;
        });
      });
    },
  },
};
</script>
  
<style lang="less">
.info {
  width: 800px;
}

.line {
  margin-top: 10px;
}

.oper-dv {
  height: 20px;
  text-align: right;
  margin-right: 100px;
}

.sel-img-dv {
  position: relative;
}

.sel-file {
  position: absolute;
  width: 90px;
  height: 30px;
  opacity: 0;
  cursor: pointer;
  z-index: 2;
}

.sel-btn {
  position: absolute;
  cursor: pointer;
  z-index: 1;
}

.cropper-content {
  display: flex;
  display: -webkit-flex;
}

.cropper {
  width: 470px;
  height: 300px;
}

.show-preview {
  flex: 1;
  -webkit-flex: 1;
  display: flex;
  display: -webkit-flex;
  justify-content: center;
  -webkit-justify-content: center;
}

.preview {
  overflow: hidden;
  border: 1px solid #cccccc;
  background: #ccc;
  margin-left: 40px;
}

.cropper-content .show-preview .preview {
  margin-left: 0;
}

.img_com {
  border: 2px dashed #ccc;
  padding: 0 10px;
  line-height: 100px;
  margin-right: 10px;
  cursor: pointer;
}
</style>
  