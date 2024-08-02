<template>
  <a-modal width="1070px" :zIndex="1003" v-drag centered :visible="visible" :bodyStyle="{ padding: '4px' }"
    :destroyOnClose="true" :maskClosable="false" title="素材选择" @cancel="cancel">
    <a-spin :spinning="spinning">
      <div class="flex justify-between align-center margin-top">
        <div>
          <a-breadcrumb>
            <a-breadcrumb-item v-for="(item, index) in breadcrumb" :key="index"><a @click="breadcrumbClick(item)"
                v-if="index != breadcrumb.length" href="javascript:;">{{
    item.name }}</a>
              <label v-else>{{ item.name }}</label></a-breadcrumb-item>
          </a-breadcrumb>
        </div>
        <div>
          <a-space>
            <label> 已全部加载，共 {{ this.data.length }} 个</label>
            <a-input-search @search="initData" v-model="search.name" allow-clear :loading="spinning"
              placeholder="搜索您的文件" style="width: 200px" /></a-space>
        </div>
      </div>
      <div class="demo-file-list-group margin-top">
        <div class="eip-file-list-group">
          <div class="eip-file-list">
            <div class="eip-file-list-header">
              <div class="eip-file-list-check-group">

              </div>
              <div class="eip-file-list-check-group">
                <!-- <a-checkbox :indeterminate="check.indeterminate" :checked="check.all" @change="checkAllChange">
                      {{ check.title }}
                    </a-checkbox> -->
              </div>
            </div>
            <div class="eip-file-list-body  beauty-scroll" :style="{ height: height, overflow: 'auto' }">
              <div v-if="data.length > 0" @click="itemClick(item)" class="eip-file-list-item"
                :class="item.checked ? 'checked' : ''" v-for="(item, index) in data" :key="index">
                <div>
                  <a-tooltip>
                    <template slot="title">
                      <div>名称： {{ item.name }}.{{ item.suffix }}</div>
                      <div>创建时间： {{ item.createTime }}</div>
                      <div>创建人： {{ item.createUserName }}</div>
                    </template>
                    <div class="eip-file-list-item-body ant-dropdown-trigger">
                      <div v-viewer class="eip-file-list-item-icon">
                        <viewer class="container">
                          <img style="max-width: 84px;max-height: 84px;" :class="['png',
    'jpeg',
    'jpg',
    'gif'
  ].includes(item.suffix) ? '' : 'eip-file-list-item-icon-image'" v-real-img="item.path" />
                        </viewer>
                      </div>
                      <div class="eip-file-list-item-title">{{ item.name }}
                      </div>
                    </div>
                  </a-tooltip>
                  <div v-if="item.type != 0" @click.stop="checkClick(item)" class="eip-file-list-item-check"><i
                      class="eip-file-icon-check checked"></i>
                  </div>
                </div>
              </div>
              <div v-if="data.length == 0">
                <eip-empty />
              </div>
            </div>
          </div>
        </div>
      </div>
    </a-spin>
    <!-- 按钮 -->
    <span slot="footer">
      <a-button @click="cancel">关闭</a-button>
      <a-button @click="ok" type="primary">确定</a-button>
    </span>
  </a-modal>
</template>

<script>
import Vue from 'vue'
import Viewer from 'v-viewer'
import 'viewerjs/dist/viewer.css'
Vue.use(Viewer)
import {
  query,
} from "@/services/system/material/list";
export default {
  name: "eip-material",
  data() {
    return {
      height: '500px',
      loading: false,

      data: [],
      breadcrumb: [{
        id: null,
        name: '全部文件'
      }],

      check: {
        all: false,
        indeterminate: false,//半选中
        title: '全选'
      },
      search: {
        parentId: null,//当前上级
        sord: 'asc',
        sidx: 'type',
        name: ''
      }
    };
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    }
  },
  created() {
    this.search.parentId = null;
    this.initData();
  },
  methods: {
    /**
       * 导航点击
       * @param {*} item 
       */
    breadcrumbClick(item) {
      var breadcrumb = [];
      for (let i = 0; i < this.breadcrumb.length; i++) {
        var f = this.breadcrumb[i];
        if (f.id != item.id) {
          breadcrumb.push(f)
        } else {
          breadcrumb.push(f);
          break;
        }
      }
      this.breadcrumb = breadcrumb;
      this.search.parentId = item.id;
      this.check = {
        all: false,
        indeterminate: false,//半选中
        title: '全选'
      }
      this.initData();
    },

    /**
         * 
         * @param {*} item 
         */
    checkClick(item) {
      this.data.forEach(element => {
        element.checked = false;
      });
      item.checked = !item.checked;
      //统计总共选中
      var totalChecked = this.data.filter(f => f.checked).length;
      this.check.title = totalChecked != 0 ? '已选中' + totalChecked + "个文件/文件夹" : '全选';
      this.check.indeterminate = totalChecked == 0 ? false : (totalChecked != this.data.length);
      this.check.all = (totalChecked == this.data.length);
    },

    /**
        * 点击
        */
    itemClick(item) {
      //如果点击是文件夹则打开下级
      if (item.type == 0) {
        this.search.parentId = item.materialId;
        //生成面包屑导航
        this.breadcrumb.push({
          id: item.materialId,
          name: item.name
        })
        this.check = {
          all: false,
          indeterminate: false,//半选中
          title: '全选'
        }
        this.initData();
      } else {
        //判断类别,若是图片就直接打开
        switch (item.suffix) {
          case 'jpg':
          case 'png':
          case 'jpeg':
          case 'gif':
            break;
          default:
            this.$message.error('暂不支持预览')
            break;
        }
      }
    },
    /**
     * 初始化数据
     */
    initData() {
      let that = this;
      that.spinning = true;
      var beforeCheck = this.data.filter(f => f.checked);
      this.data = [];//之前是否具有已选中
      query(this.search).then((result) => {
        //判断类型

        result.data = result.data.filter(f => ['png',
          'jpeg',
          'jpg',
          'gif'
        ].includes(f.suffix) || f.type == 0)

        result.data.forEach(element => {
          if (element.type == 0) {
            element.path = "/images/material/dir.png"
          } else {
            switch (element.suffix) {
              case "txt":
                element.path = "/images/material/txt.png"
                break;
              case "doc":
              case "docx":
                element.path = "/images/material/doc.png"
                break;
              case "xls":
              case "xlsx":
                element.path = "/images/material/xls.png"
                break;
              case "apk":
                element.path = "/images/material/apk.png"
                break;
              case "bt":
                element.path = "/images/material/bt.png"
                break;
              case "exe":
                element.path = "/images/material/exe.png"
                break;
              case "flash":
                element.path = "/images/material/flash.png"
                break;
              case "htm":
              case "html":
                element.path = "/images/material/htm.png"
                break;
              case "mp3":
                element.path = "/images/material/mp3.png"
                break;
              case "mp4":
                element.path = "/images/material/mp4.png"
                break;
              case "pdf":
                element.path = "/images/material/pdf.png"
                break;
              case "ppt":
              case "pptx":
                element.path = "/images/material/ppt.png"
                break;
              case "psd":
                element.path = "/images/material/psd.png"
                break;
              case "ttf":
                element.path = "/images/material/ttf.png"
                break;
              case "zip":
                element.path = "/images/material/zip.png"
                break;
              case "jpg":
              case "png":
              case "jpeg":
              case "gif":
                break;
              default:
                element.path = "/images/material/file.png"
                break;
            }
          }
          //判断是否需要选中
          element.checked = beforeCheck.filter(f => f.materialId == element.materialId).length > 0;
        });

        that.data = result.data;
        if (result.data.length > 0) {
          var totalChecked = that.data.filter(f => f.checked).length;
          that.check.title = totalChecked != 0 ? '已选中' + totalChecked + "个文件/文件夹" : '全选';
          that.check.indeterminate = totalChecked == 0 ? false : (totalChecked != that.data.length);
          that.check.all = (totalChecked == that.data.length);
        }

        that.spinning = false;
      });
    },

    /* 传递图片地址 */
    ok() {
      //查看是否选择了图片
      var checks = this.data.filter(f => f.checked);
      if (checks.length > 0) {
        this.$emit('ok', checks[0].path)
        this.$emit("update:visible", false);
      } else {
        this.$message.error('请选择图片')
      }
    },

    // 关闭弹框
    cancel() {
      this.$emit("update:visible", false);
    },
  },
};
</script>

<style lang="less" scoped>
.eip-file-list-group .eip-file-icon-check {
  width: 16px;
  height: 16px;
  border-radius: 2px;
  display: inline-block;
  border: 1px solid #d9d9d9;
  background: #fff;
  box-sizing: border-box;
  transition: all .3s;
  position: relative;
  cursor: pointer
}

.eip-file-list-group .eip-file-icon-check:hover {
  border-color: #1890ff
}

.eip-file-list-group .eip-file-icon-check:before {
  content: "";
  width: 5.71428571px;
  height: 9.14285714px;
  display: table;
  border: 2px solid #fff;
  border-top: 0;
  border-left: 0;
  transform: rotate(45deg) scale(0) translate(-50%, -50%);
  opacity: 0;
  transition: all .1s cubic-bezier(.71, -.46, .88, .6), opacity .1s;
  box-sizing: border-box;
  position: absolute;
  top: 50%;
  left: 21.5%
}

.eip-file-list-group .eip-file-icon-check.checked {
  background: #1890ff;
  border-color: #1890ff
}

.eip-file-list-group .eip-file-icon-check.checked:before {
  transform: rotate(45deg) scale(1) translate(-50%, -50%);
  opacity: 1;
  transition: all .2s cubic-bezier(.12, .4, .29, 1.46) .1s
}

.eip-file-list-group .eip-file-icon-check:after {
  content: "";
  width: 8px;
  height: 8px;
  display: table;
  transform: translate(-50%, -50%) scale(1);
  opacity: 0;
  transition: all .1s cubic-bezier(.71, -.46, .88, .6), opacity .1s;
  box-sizing: border-box;
  position: absolute;
  top: 50%;
  left: 50%
}

.eip-file-list-group .eip-file-icon-check.indeterminate:after {
  background: #1890ff;
  opacity: 1
}

.eip-file-list-group .eip-file-list-selector {
  display: none;
  position: absolute;
  box-sizing: border-box;
  background: rgba(0, 120, 215, .2);
  border: 1px solid #0078d7;
  z-index: 999
}

.eip-file-list {
  position: relative;
  box-sizing: border-box
}

.eip-file-list .eip-file-list-header {
  padding: 0 16px;
  border-bottom: 1px solid #f0f0f0;
  box-sizing: border-box;
  line-height: 36px;
  display: flex
}

.eip-file-list .eip-file-list-header .eip-file-list-check-group {
  display: inline-flex;
  align-items: center;
  cursor: pointer
}

.eip-file-list .eip-file-list-header .eip-file-list-check-group .eip-file-list-check {
  margin-right: 14px
}

.eip-file-list .eip-file-list-body {
  padding: 14px 14px 0 0
}

.eip-file-list .eip-file-list-item {
  width: 110px;
  padding: 6px 4px;
  margin: 0 0 14px 14px;
  display: inline-block;
  box-sizing: border-box;
  border: 1px solid transparent;
  transition: all .3s;
  border-radius: 6px;
  text-align: center;
  position: relative;
  cursor: pointer
}

.eip-file-list .eip-file-list-item .eip-file-list-item-icon {
  max-height: 84px;
  display: flex;
  align-items: center;
  justify-content: center;
  position: relative
}

.eip-file-list .eip-file-list-item .eip-file-list-item-icon>img {
  max-width: 84px;
  max-height: 84px;
  border-radius: 2px;
  display: inline-block;
  pointer-events: none
}

.eip-file-list .eip-file-list-item .eip-file-list-item-icon>.eip-file-list-item-icon-image {
  width: 56px;
  height: 56px;
  object-fit: cover
}

.eip-file-list .eip-file-list-item .eip-file-list-item-title {
  margin-top: 4px;
  overflow: hidden;
  white-space: nowrap;
  word-break: break-all;
  text-overflow: ellipsis;
  box-sizing: border-box;
  transition: color .3s
}

.eip-file-list .eip-file-list-item .eip-file-list-item-title:hover {
  color: #1890ff
}

.eip-file-list .eip-file-list-item .eip-file-list-item-check {
  border-radius: 50%;
  background: #fff;
  transition: all .3s;
  position: absolute;
  top: 6px;
  right: 6px;
  opacity: 0
}

.eip-file-list .eip-file-list-item .eip-file-list-item-check .eip-file-icon-check {
  width: 18px;
  height: 18px;
  border-radius: 50%;
  display: block;
  opacity: .4
}

.eip-file-list .eip-file-list-item:hover,
.eip-file-list .eip-file-list-item.active,
.eip-file-list .eip-file-list-item.checked {
  background: #e6f7ff
}

.eip-file-list .eip-file-list-item:hover .eip-file-list-item-check,
.eip-file-list .eip-file-list-item.active .eip-file-list-item-check,
.eip-file-list .eip-file-list-item.checked .eip-file-list-item-check {
  opacity: 1
}

.eip-file-list .eip-file-list-item.active,
.eip-file-list .eip-file-list-item.checked {
  border-color: #91d5ff
}

.eip-file-list .eip-file-list-item.active .eip-file-list-item-check .eip-file-icon-check,
.eip-file-list .eip-file-list-item.checked .eip-file-list-item-check .eip-file-icon-check {
  opacity: 1
}

.eip-file-list-table-item-sort {
  width: 8px;
  height: 12px;
  margin-left: 6px;
  position: relative;
  display: inline-block;
  flex-shrink: 0
}

.eip-file-list-table-item-sort:before,
.eip-file-list-table-item-sort:after {
  content: "";
  width: 0;
  height: 0;
  display: block;
  border: 4px solid hsla(0, 0%, 60%, .6);
  border-left-color: transparent;
  border-right-color: transparent
}

.eip-file-list-table-item-sort:before {
  border-top-color: transparent;
  margin: -4px 0 4px
}

.eip-file-list-table-item-sort:after {
  border-bottom-color: transparent
}

.eip-file-list-table-item-sort.eip-file-list-sort-asc:before {
  border-bottom-color: #1890ff
}

.eip-file-list-table-item-sort.eip-file-list-sort-desc:after {
  border-top-color: #1890ff
}

.eip-file-list-table {
  position: relative
}

.eip-file-list-table .eip-file-list-table-item {
  line-height: 44px;
  border-top: 1px solid #f0f0f0;
  transition: all .3s;
  position: relative
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-body {
  display: flex;
  align-items: center
}

.eip-file-list-table .eip-file-list-table-item:first-child {
  border-top-color: transparent;
  margin-top: -1px
}

.eip-file-list-table .eip-file-list-table-item:last-child {
  border-bottom: 1px solid #f0f0f0
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-check-group {
  width: 36px;
  padding-left: 16px;
  box-sizing: border-box;
  line-height: 0;
  flex-shrink: 0
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-name {
  display: flex;
  align-items: center;
  padding: 0 10px;
  box-sizing: border-box;
  position: relative;
  overflow: hidden;
  min-width: 110px;
  flex: 1
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-icon {
  flex: 1;
  display: flex;
  align-items: center;
  overflow: hidden
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-icon-image {
  width: 24px;
  height: 24px;
  flex-shrink: 0;
  cursor: pointer
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-icon-image:hover+.eip-file-list-table-item-title {
  color: #1890ff
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-title {
  padding-left: 10px;
  transition: color .3s;
  box-sizing: border-box;
  text-overflow: ellipsis;
  word-break: break-all;
  white-space: nowrap;
  overflow: hidden;
  cursor: pointer
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-title:hover {
  color: #1890ff
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-tool-group {
  flex-shrink: 0;
  box-sizing: border-box;
  align-items: center;
  display: none
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-tool-group .eip-file-list-item-tool {
  font-size: 17px;
  margin-left: 16px;
  color: #1890ff;
  cursor: pointer
}

.eip-file-list-table .eip-file-list-table-item:hover .eip-file-list-table-item-tool-group {
  display: flex
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-col {
  padding: 0 10px;
  box-sizing: border-box
}

.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item {
  line-height: 36px
}

.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-name>div {
  overflow: hidden;
  white-space: nowrap;
  word-break: break-all;
  text-overflow: ellipsis
}

.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-name,
.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-col {
  transition: color .3s;
  cursor: pointer
}

.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-name:hover,
.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-col:hover {
  color: #1890ff
}

.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item:hover,
.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item.active,
.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item.checked {
  background: #e6f7ff;
  border-color: #91d5ff
}

.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item:hover+.eip-file-list-table-item,
.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item.active+.eip-file-list-table-item,
.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item.checked+.eip-file-list-table-item {
  border-top-color: #91d5ff
}

.eip-file-item-context-menu {
  min-width: 100px
}

.demo-file-list-group {
  position: relative;
  overflow-x: auto;
}

.demo-file-list-group .demo-file-list-empty {
  position: absolute;
  top: 100px;
  left: 50%;
  transform: translate(-50%)
}
</style>
