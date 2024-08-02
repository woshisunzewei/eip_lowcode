<template>
  <a-form-model :layout="form.layout">
    <a-form-model-item label="API URL " prop="name" extra="设置筛选条件，仅使满足条件的记录进入流程。">
      <a-row type="flex">
        <a-space>
          <a-col style="width:100px">
            <a-select v-model="drawerData.method">
              <a-select-option value="GET">
                GET
              </a-select-option>
              <a-select-option value="POST">
                POST
              </a-select-option>
            </a-select></a-col>
          <a-col style="width:560px">
            <div class="flex justify-start ">
              <eip-editor class="flex-sub " ref="requestUrl" v-if="tinymce.show" v-model="drawerData.url"
                :height="tinymce.height" :toolbar="tinymce.toolbar" :plugins="tinymce.plugins"
                :statusbar="tinymce.statusbar" content_style="p{margin:4px} body{margin:0 0 0 4px}"
                :menubar="tinymce.menubar" placeholder="请输入接口地址"></eip-editor>
              <a-popover placement="bottomLeft" v-model="requestUrlVisible">
                <div slot="content" style="width: 600px;">
                  <a-input-search placeholder="搜索流程节点对象下的字段" style="width: 100%;padding-bottom: 4px;" />
                  <a-collapse :expand-icon-position="expandIconPosition" style="max-height: 400px;overflow: auto;">
                    <a-collapse-panel v-for="(item, index) in configList" :key="index" :header="item.title">
                      <ul class="conditionFieldBox show">
                        <li @click="urlParamClick(item, itemc)" v-for="itemc in item.data.columns" :key="itemc.model"
                          class="flexRow ThemeHoverBGColor3">
                          <div class="ellipsis"><span v-if="itemc.type" class="field">[{{ itemc.type }}]</span><span
                              title="自动编号">{{
    itemc.label }}</span></div>
                        </li>
                      </ul>
                      <span slot="extra" class="Gray_9e">
                        {{ item.type == '0' ? '工作表“' + item.data.tableName + '”' : '' }}
                      </span>
                    </a-collapse-panel>
                  </a-collapse>
                </div>
                <a-button type="primary" icon="diff"></a-button>
              </a-popover>
            </div>
          </a-col>
        </a-space>
      </a-row>
    </a-form-model-item>
    <a-form-model-item label="ContentType">
      <a-select v-model="drawerData.contentType">
        <a-select-option value="application/x-www-form-urlencoded">
          application/x-www-form-urlencoded
        </a-select-option>
        <a-select-option value="application/json">
          application/json
        </a-select-option>
        <a-select-option value="application/text">
          application/text
        </a-select-option>
        <a-select-option value="application/xml">
          application/xml
        </a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="Headers">
      <div class="flex justify-start ">
        <eip-editor class="flex-sub " ref="requestHeaders" v-if="tinymce.show" v-model="drawerData.headers"
          :height="tinymce.height" :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :statusbar="tinymce.statusbar"
          content_style="p{margin:4px} body{margin:0 0 0 4px}" :menubar="tinymce.menubar"
          placeholder="请输入请求头"></eip-editor>
        <a-popover placement="bottomLeft" v-model="requestHeadersVisible">
          <div slot="content" style="width: 600px;">
            <a-input-search placeholder="搜索流程节点对象下的字段" style="width: 100%;padding-bottom: 4px;" />
            <a-collapse :expand-icon-position="expandIconPosition" style="max-height: 400px;overflow: auto;">
              <a-collapse-panel v-for="(item, index) in configList" :key="index" :header="item.title">
                <ul class="conditionFieldBox show">
                  <li @click="headerParamClick(item, itemc)" v-for="itemc in item.data.columns" :key="itemc.model"
                    class="flexRow ThemeHoverBGColor3">
                    <div class="ellipsis"><span v-if="itemc.type" class="field">[{{ itemc.type }}]</span><span
                        title="自动编号">{{
    itemc.label }}</span></div>
                  </li>
                </ul>
                <span slot="extra" class="Gray_9e">
                  {{ item.type == '0' ? '工作表“' + item.data.tableName + '”' : '' }}
                </span>
              </a-collapse-panel>
            </a-collapse>
          </div>
          <a-button type="primary" icon="diff"></a-button>
        </a-popover>
      </div>
    </a-form-model-item>
    <a-form-model-item label="Body" v-if="drawerData.method == 'POST'">
      <div class="flex justify-start ">
        <eip-editor class="flex-sub " ref="requestBody" v-if="tinymce.show" v-model="drawerData.body" :height="200"
          :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :statusbar="tinymce.statusbar"
          content_style="p{margin:4px} body{margin:0 0 0 4px}" :menubar="tinymce.menubar"
          placeholder="请输入Body Json格式"></eip-editor>
        <a-popover placement="bottomLeft" v-model="requestBodyVisible">
          <div slot="content" style="width: 600px;">
            <a-input-search placeholder="搜索流程节点对象下的字段" style="width: 100%;padding-bottom: 4px;" />
            <a-collapse :expand-icon-position="expandIconPosition" style="max-height: 400px;overflow: auto;">
              <a-collapse-panel v-for="(item, index) in configList" :key="index" :header="item.title">
                <ul class="conditionFieldBox show">
                  <li @click="bodyParamClick(item, itemc)" v-for="itemc in item.data.columns" :key="itemc.model"
                    class="flexRow ThemeHoverBGColor3">
                    <div class="ellipsis"><span class="field" v-if="itemc.type">[{{ itemc.type }}]</span><span
                        title="自动编号">{{
    itemc.label }}</span></div>
                  </li>
                </ul>
                <span slot="extra" class="Gray_9e">
                  {{ item.type == '0' ? '工作表“' + item.data.tableName + '”' : '' }}
                </span>
              </a-collapse-panel>
            </a-collapse>
          </div>
          <a-button type="primary" icon="diff"></a-button>
        </a-popover>
      </div>
    </a-form-model-item>
    <a-form-model-item label="返回参数列表" prop="name" extra="向URL发送请求测试获取参数列表；请求中的动态参数将取测试值">
      <a-button type="primary" @click="apiTest"><a-icon type="save" />测试API</a-button>

      <a-descriptions bordered class="eip-descriptions" size="small" :column="1" v-if="drawerData.test.code">
        <a-descriptions-item label="返回代码">{{
    drawerData.test.code
  }}</a-descriptions-item>
        <a-descriptions-item label="返回信息">{{
      drawerData.test.msg
    }}</a-descriptions-item>
        <a-descriptions-item label="返回数据">{{
          drawerData.test.data
          }}</a-descriptions-item>
      </a-descriptions>
    </a-form-model-item>

    <a-form-model-item label="请求超时或请求失败时">
      <a-radio-group v-model="drawerData.errorType">
        <a-radio :value="1">继续执行</a-radio>
        <a-radio :value="2">中止执行</a-radio>
      </a-radio-group>
    </a-form-model-item>
  </a-form-model>

</template>

<script>

import {
  eventApi,
} from "@/services/system/agile/run/edit";
export default {
  name: "customerRequest",
  components: {
  },
  props: {
    tables: {
      required: true,
      type: Array,
      default: () => { return [] }
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
      expandIconPosition: 'right',
      requestUrlVisible: false,
      requestBodyVisible: false,
      requestHeadersVisible: false,
      form: {
        layout: 'horizontal'
      },
      tinymce: {
        plugins: "preview fullscreen code",
        toolbar: "",
        height: 32,
        statusbar: false,
        show: true,
        menubar: "",
      }
    }
  },
  computed: {

  },
  mounted() {
  },
  methods: {
    isArray(arr) {
      return typeof arr === "object" && Array.isArray(arr);
    },
    /**
           *
           * @param {*} value
           */
    async eventApiConfirm(value) {
      var apiResult = await eventApi(value);
      return apiResult;
    },

    /**
     * 测试
     */
    apiTest() {
      let that = this;
      if (!that.drawerData.url) {
        that.$message.error("请配置接口地址");
        return;
      }
      that.$loading.show({ text: "加载中,请稍等..." });
      that.eventApiConfirm({
        url: this.$refs.requestUrl.getTxt(),
        type: this.drawerData.method,
        contentType: this.drawerData.contentType,
        param: this.$refs.requestBody.getTxt(2),
      }).then(result => {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.drawerData.test.code = result.code;
          that.drawerData.test.msg = result.msg;
          if (result.data) {
            //是否为数组还是对象(只会显示一个参数)
            if (that.isArray(result.data) && result.data.length > 0) {
              that.drawerData.test.data = [result.data[0]];
              that.drawerData.test.array = true;
            } else {
              that.drawerData.test.data = result.data;
              that.drawerData.test.array = false;
            }
          }

        } else {
          that.$message.error("操作异常：" + result.msg);
        }
      }, error => {
        that.$message.error("操作异常：" + error);
        that.$loading.hide();
      });
    },

    /**
      * 选择
      * @param {*} item 
      * @param {*} itemc 
      */
    urlParamClick(item, itemc) {
      let html = "<span id='" + encodeURIComponent(JSON.stringify({
        id: item.id,
        model: itemc.model
      })) + "' class='non-editable'>" + itemc.label + "</span>"
      this.$refs.requestUrl.insertIndex(html, 0);
      this.requestUrlVisible = false;
    },

    /**
      * 选择
      * @param {*} item 
      * @param {*} itemc 
      */
    headerParamClick(item, itemc) {
      let html = "<span id='" + encodeURIComponent(JSON.stringify({
        id: item.id,
        model: itemc.model
      })) + "' class='non-editable'>" + itemc.label + "</span>"
      this.$refs.requestHeaders.insertIndex(html, 1);
      this.requestHeadersVisible = false;
    },

    /**
      * 选择
      * @param {*} item 
      * @param {*} itemc 
      */
    bodyParamClick(item, itemc) {
      let html = "<span id='" + encodeURIComponent(JSON.stringify({
        id: item.id,
        model: itemc.model
      })) + "' class='non-editable'>" + itemc.label + "</span>"
      this.$refs.requestBody.insertIndex(html, 2);
      this.requestBodyVisible = false;
    },
  }
}
</script>

<style scoped>
.conditionFieldBox {
  background-color: #f5f5f5;
  display: none;
  height: 0;
  overflow: hidden;
  transition: all 20s ease-in-out
}

.conditionFieldBox.show {
  display: block;
  height: 100%
}

.conditionFieldBox li {
  align-items: center;
  cursor: pointer;
  height: 36px;
  padding: 0 16px 0 39px
}

.conditionFieldBox li:not(:hover) {
  background-color: transparent !important
}

.conditionFieldBox li .field {
  color: #8f8f8f;
  margin-right: 5px
}

.conditionFieldBox li:not(.conditionFieldNull):hover span {
  color: #fff
}

.ThemeHoverBGColor3:hover {
  background-color: #1e88e5 !important
}

.ellipsis {
  overflow: hidden;
  text-overflow: ellipsis;
  vertical-align: top;
  white-space: nowrap
}

.flexRow {
  display: flex;
  min-width: 0
}

/deep/ .info {
  padding: 16px 0;
  font-size: 12px;
  width: 100% !important
}

/deep/ .ant-collapse-content>.ant-collapse-content-box {
  padding: 0 !important;
}

/deep/ .ant-collapse-icon-position-right>.ant-collapse-item>.ant-collapse-header {
  padding: 6px 36px 6px 14px !important;
}

/deep/.ant-form-item-label>label {
  font-weight: bold;
}
</style>
