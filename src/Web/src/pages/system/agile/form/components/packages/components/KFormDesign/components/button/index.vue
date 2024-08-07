<template>
  <a-drawer width="900px" :title="title" :visible="visible" :bodyStyle="{ padding: '1px' }" @close="cancel"
    :destroyOnClose="true">
    <div class="eip-drawer-body beauty-scroll padding">
      <vxe-toolbar ref="styleToolbar">
        <template v-slot:buttons>
          <a-space>
            <a-button @click="buttonSubmit" type="primary"> 提交 </a-button>
          </a-space>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" row-key :data="button" :height="height">

        <template #loading>
          <a-spin></a-spin>
        </template>

        <template #empty>
          <eip-empty />
        </template>
        <vxe-column title="排序" width="48" align="center">

          <template>
            <span class="drag-btn">
              <a-icon type="drag" />
            </span>
          </template>
        </vxe-column>

        <vxe-column title="新增" width="54px" align="center">

          <template #header>
            <a-button @click="add" size="small" type="primary">
              <a-icon type="plus" />
            </a-button>
          </template>

          <template v-slot="{ row }">
            <a-popconfirm title="确定删除查询配置?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
              <a-button @click.stop="" size="small" type="danger">
                <a-icon type="delete"></a-icon>
              </a-button>
            </a-popconfirm>
          </template>
        </vxe-column>

        <vxe-column title="名称" width="150px">

          <template v-slot="{ row }">
            <a-input placeholder="请输入名称" v-model="row.name" />
          </template>
        </vxe-column>
        <vxe-column title="触发类型" width="100px">

          <template v-slot="{ row }">
            <a-select style="width:80px" default-value="primary" allow-clear v-model="row.triggerType">
              <a-select-option :value="1">方法 </a-select-option>
              <a-select-option :value="2">脚本 </a-select-option>
              <a-select-option :value="3">接口 </a-select-option>
              <a-select-option :value="4">自动化</a-select-option>
            </a-select>
          </template>
        </vxe-column>

        <vxe-column title="触发配置" width="100px" align="center">

          <template v-slot="{ row }">
            <a-badge>
              <a-button size="small" @click="triggerTypeEdit(row)" type="primary">
                <a-icon type="edit" />
              </a-button>
            </a-badge>
          </template>
        </vxe-column>

        <vxe-column title="图标" width="120px" align="center">

          <template v-slot="{ row }">
            <eip-icon :name="row.icon" :theme="row.theme" @click="iconClick(row, $event)"
              @clear="iconClear(row)"></eip-icon>
          </template>
        </vxe-column>
        <vxe-column title="类型" width="120px" align="center">

          <template v-slot="{ row }">
            <a-select style="width: 100px" default-value="primary" allow-clear v-model="row.style">
              <a-select-option value="primary">主按钮 </a-select-option>
              <a-select-option value=""> 次按钮 </a-select-option>
              <a-select-option value="dashed"> 虚线按钮 </a-select-option>
              <a-select-option value="danger"> 危险按钮 </a-select-option>
              <a-select-option value="link"> 链接按钮 </a-select-option>
              <a-select-option value="upload"> 上传按钮 </a-select-option>
            </a-select>
          </template>
        </vxe-column>
      </vxe-table>
    </div>
    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </div>
    <a-drawer width="1200px" placement="right" :bodyStyle="{ padding: '1px' }" :visible="trigger.visible" title="触发配置"
      @close="trigger.visible = false" :destroyOnClose="true">
      <div class="beauty-scroll" style="height: calc(100vh - 110px);">
        <eip-code v-if="trigger.type == 2" :value.sync="trigger.script">
        </eip-code>
        <a-form-model>
          <a-form-model-item :label-col="config.labelCol" :wrapper-col="config.wrapperCol" v-if="trigger.type == 1"
            label="方法" prop="method">
            <a-input v-model="trigger.method" placeholder="请输入方法" allow-clear />
          </a-form-model-item>
          <a-form-model-item :label-col="config.labelCol" :wrapper-col="config.wrapperCol" v-if="trigger.type == 3"
            label="Api地址" prop="script">
            <a-input v-model="trigger.api.path" placeholder="请输入Api地址" allow-clear />
          </a-form-model-item>
          <a-form-model-item :label-col="config.labelCol" :wrapper-col="config.wrapperCol" v-if="trigger.type == 3"
            label="请求方式" prop="script">
            <a-select default-value="primary" allow-clear v-model="trigger.api.type">
              <a-select-option value="post">POST </a-select-option>
              <a-select-option value="get">GET </a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item :label-col="config.labelCol" :wrapper-col="config.wrapperCol" v-if="trigger.type == 3"
            label="确认提示语" prop="confirmMsg">
            <a-input v-model="trigger.api.confirmMsg" allow-clear placeholder="请输入确认提示语:如(是否删除?)" />
          </a-form-model-item>
          <a-form-model-item :label-col="config.labelCol" :wrapper-col="config.wrapperCol" v-if="trigger.type == 3"
            label="数据选择类型" prop="confirmType">
            <a-select default-value="primary" allow-clear v-model="trigger.api.confirmType">
              <a-select-option :value="1">可空 </a-select-option>
              <a-select-option :value="2">一条 </a-select-option>
              <a-select-option :value="3">多条 </a-select-option>
            </a-select>
          </a-form-model-item>

        </a-form-model>
      </div>
      <div class="eip-drawer-toolbar">
        <a-space>
          <a-button key="back" @click="trigger.visible = false"><a-icon type="close" />取消</a-button>
          <a-button key="submit" @click="triggerTypeOk" type="primary"><a-icon type="save" />提交</a-button>
        </a-space>
      </div>
    </a-drawer>

    <designer ref="designer" v-if="designer.visible" :visible.sync="designer.visible" :title="designer.title"
      :configId="designer.configId" @close="designerClose"></designer>

  </a-drawer>
</template>

<script>
import designer from "@/pages/system/agile/automation/designer";
import Sortable from "sortablejs";
export default {
  name: "button-index",
  components: { designer },
  data() {
    return {
      designer: {
        visible: false,
        configId: "",
        title: "",
        bodyStyle: {
          padding: "0",
        },
      },
      height: window.innerHeight - 191,
      table: {
        loading: false,
      },
      config: {
        labelCol: { span: 5 },
        wrapperCol: { span: 17 },
      },
      button: [],
      loading: false,
      trigger: {
        visible: false,
        type: 1,
        script: "",
        method: "",
        api: {
          path: null,
          type: 'post',
          confirmMsg: '',//确认提示信息
          confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
        }
      },
      item: null,
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    data: {
      type: Array,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.init();
  },
  methods: {
    /**
     * 增删改查按钮
     */
    buttonSubmit() {
      var button = [
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "",
          name: "关闭",
          icon: "close",
          theme: "outlined",
          method: "cancel",
          script: null,
        },
        {
          triggerType: 1,//触发执行类型:1方法，2脚本，3接口
          style: "primary",
          name: "提交",
          icon: "save",
          theme: "outlined",
          method: "saveClose",
          script: null,
        }
      ]

      let that = this;
      button.forEach(f => {
        that.button.push(f)
      })
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },
    /**
     * 新增
     */
    add() {
      this.button.push({
        icon: "",
        isFreeze: false,
        triggerType: 1,//触发执行类型:1方法，2脚本，3接口
        method: null,
        script: null,
        api: null,//接口配置
        theme: "outlined",
        style: "primary",
      });
      this.rowDrop();
    },
    /**
     * 拖拽
     */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = this.$refs.table;
        if (xTable) {
          this.sortable1 = Sortable.create(
            xTable.$el.querySelector(".body--wrapper>.vxe-table--body tbody"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.button.splice(oldIndex, 1)[0];
                that.button.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      })
    },
    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.button.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.button.splice(spIndex, 1);
      }
    },

    /**
     * 初始化
     */
    init() {
      let that = this;
      that.button = this.data;
      that.rowDrop();
    },

    /**
     *
     */
    triggerTypeEdit(item) {
      this.trigger.type = item.triggerType;
      if (item.triggerType == 1) {
        this.trigger.method = item.method ? item.method : "";
      }
      else if (item.triggerType == 2) {
        this.trigger.script = item.script ? item.script : "";
      }
      //api
      else if (item.triggerType == 3) {
        if (item.api) {
          this.trigger.api = JSON.parse(item.api);
        } else {
          this.trigger.api = {
            path: null,
            type: 'post',
            confirmMsg: '',//确认提示信息
            confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
          }
        }
      }
      //自动化
      else if (item.triggerType == 4) {
        if (item.api) {
          this.trigger.api = JSON.parse(item.api);
        } else {
          this.trigger.api = {
            path: null,
            type: 'post',
            confirmMsg: '',//确认提示信息
            confirmType: 1,//确认类型,1可不选择数据，2单选，3多选
          }
        }
      }
      this.item = item;
      this.trigger.visible = true;
    },

    /**
     * 
     */
    triggerTypeOk() {
      if (this.trigger.type == 1) {
        this.item.method = this.trigger.method;
      }
      else if (this.trigger.type == 2) {
        this.item.script = this.trigger.script;
      }
      else if (this.trigger.type == 3) {
        this.item.api = JSON.stringify(this.trigger.api);
      }
      this.trigger.visible = false
    },

    /**
     *图标点击
     */
    iconClick(row, data) {
      row.icon = data.name;
      row.theme = data.theme;
    },

    /**
     * 清空图标
     */
    iconClear(row) {
      row.icon = "";
      row.theme = "";
    },
    /**
     * 保存
     */
    save() {
      let that = this;
      that.cancel();
      that.$emit("confirm", this.button);
    },
  },
};
</script>

<style lang="less" scoped>
/deep/ .CodeMirror {
  height: calc(100vh - 110px);
}
</style>
