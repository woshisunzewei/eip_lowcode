<template>
  <a-drawer width="900px" :destroyOnClose="true" :visible="visible" :bodyStyle="{ padding: '1px' }" title="查询配置"
    @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">
        <a-form-model-item label="选项">
          <a-checkbox v-model="options.multiple"> 多选 </a-checkbox>
        </a-form-model-item>
        <a-form-model-item label="数据源"><a-radio-group v-model="options.source.type" @change="sourceTypeChange">
            <a-radio value="dynamic"> 动态数据 </a-radio>
            <a-radio value="custom"> 自定义 </a-radio>
          </a-radio-group>
        </a-form-model-item>
        <a-form-model-item label="数据源" v-if="options.source.type == 'dynamic'">
          <a-input disabled placeholder="点击选择设置数据源" v-model="options.source.title">
            <div @click="dataSourceClick" slot="addonAfter" style="cursor: pointer">
              <a-icon type="search" />
            </div>
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="键" v-if="options.source.type == 'dynamic'">
          <a-select show-search placeholder="请选择类型" v-model="options.source.key">
            <a-select-option v-for="(item, i) in options.source.outParams" :key="i" :value="item.name">
              {{ item.name }} {{ item.description }}
            </a-select-option>
          </a-select></a-form-model-item>
        <a-form-model-item label="值" v-if="options.source.type == 'dynamic'">
          <a-select show-search placeholder="请选择类型" v-model="options.source.value">
            <a-select-option v-for="(item, i) in options.source.outParams" :key="i" :value="item.name">
              {{ item.name }} {{ item.description }}
            </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="排序字段" v-if="options.source.type == 'dynamic'">
          <a-select show-search placeholder="请选择类型" v-model="options.source.sidx">
            <a-select-option v-for="(item, i) in options.source.outParams" :key="i" :value="item.name">
              {{ item.name }} {{ item.description }}
            </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="排序方式" v-if="options.source.type == 'dynamic'">
          <a-select show-search placeholder="请选择类型" v-model="options.source.sord">
            <a-select-option value="DESC"> 降序 </a-select-option>
            <a-select-option value="ASC"> 升序 </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="数据值" v-if="options.source.type == 'custom'">
          <vxe-toolbar ref="customToolbar">
            <template v-slot:buttons>
              <a-space>
                <a-button @click="yesNo" type="primary"> 是否 </a-button>
                <a-button @click="taskStatus" type="primary"> 流程任务状态 </a-button>
              </a-space>
            </template>
          </vxe-toolbar>
          <vxe-table row-key ref="customble" :data="options.source.custom" :height="height">
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column title="新增" width="54px" align="center">
              <template #header>
                <a-button @click="add" size="small" type="primary">
                  <a-icon type="plus" />
                </a-button>
              </template>
              <template v-slot="{ row }">
                <a-popconfirm title="确定删除配置?" ok-text="确定" cancel-text="取消" @confirm="del(row)">
                  <a-button @click.stop="" size="small" type="danger">
                    <a-icon type="delete"></a-icon>
                  </a-button>
                </a-popconfirm>
              </template>
            </vxe-column>
            <vxe-column title="键" width="130px">
              <template v-slot="{ row }">
                <a-input allow-clear placeholder="键" v-model="row.key" />
              </template>
            </vxe-column>
            <vxe-column title="值" width="130px">
              <template v-slot="{ row }">
                <a-input allow-clear placeholder="值" v-model="row.value" />
              </template>
            </vxe-column>
          </vxe-table>
        </a-form-model-item>
      </a-form-model>
    </div>
    <div class="eip-drawer-toolbar">
      <a-space>
        <a-button key="back" @click="cancel"> 取消 </a-button>
        <a-button key="submit" @click="ok" type="primary"> 确定 </a-button>
      </a-space>
    </div>

    <!-- 数据源选择 -->
    <data-source ref="dataSource" :visible.sync="data.source.visible" @cancel="dataSourceCancel" @ok="dataSourceOk">
    </data-source>
  </a-drawer>
</template>

<script>
import dataSource from "../data/source";
export default {
  name: "search-select",
  components: {
    dataSource,
  },
  data() {
    return {
      col: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      style: {
        zIndex: 1030,
      },
      height: window.innerHeight - 322,
      data: {
        source: {
          visible: false,
        },
      },

      //下拉查询参数配置
      options: {
        multiple: true, //多选
        source: {
          type: 'dynamic',//数据源类型
          title: '',
          dataSourceId: null,
          inParams: [],
          outParams: [],
          key: [],//键
          value: [],//值
          sidx: undefined,//排序字段
          sord: "DESC",//排序方式
          custom: [], //自定义数据
        },
      },
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
  },

  methods: {
    /**
    * 是否
    */
    yesNo() {
      var custom = [
        { key: 1, presets: "red", custom: null, value: "是" },
        { key: 0, presets: "green", custom: null, value: "否" },
      ];
      this.options.source.custom = custom;
    },
    /**
    * 任务状态
    */
    taskStatus() {
      var custom = [
        { key: 0, value: "处理中" },
        { key: 2, value: "终止" },
        { key: 4, value: "暂停" },
        { key: 6, value: "即将到期" },
        { key: 8, value: "拒绝" },
        { key: 10, value: "撤销" },
        { key: 12, value: "删除" },
        { key: 14, value: "退回重填" },
        { key: 16, value: "取消" },
        { key: 100, value: "完成" },
      ];
      this.options.source.custom = custom;
    },
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 保存
     */
    ok() {
      this.$emit("ok", this.options);
    },
    /**
     * 新增
     */
    add() {
      this.options.source.custom.push({
        key: null,
        value: null,
      });
    },
    /**
     * 删除
     */
    del(row) {
      let spIndex = -1;
      this.options.source.custom.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.options.source.custom.splice(spIndex, 1);
      }
    },
    /**
     * 数据数据源点击
     */
    dataSourceClick() {
      this.data.source.visible = true;
      this.$refs.dataSource.init(this.options.source);
    },

    /**
     * 数据源配置关闭
     */
    dataSourceCancel() {
      this.data.source.visible = false;
    },

    /**
     * 数据源配置保存
     */
    dataSourceOk(config) {

      this.options.source.dataSourceId = config.dataSourceId;
      this.options.source.inParams = config.inParams;
      this.options.source.outParams = config.outParams;
      this.options.source.title = config.title;

      this.options.source.key = undefined;
      this.options.source.value = undefined;
      this.options.source.sidx = undefined;
      this.options.source.sord = 'DESC';
      this.dataSourceCancel();
    },



    /**
     * 下拉数据源改变
     */
    sourceTypeChange(e) {
      this.options.source.type = e.target.value;
    },

    /**
     * 初始化
     */
    init(options) {
      if (options && typeof (options.source) != 'undefined') {
        this.options = options;
      } else {
        this.options = {
          multiple: true,
          source: {
            type: 'dynamic',//数据源类型
            title: '',
            dataSourceId: null,
            inParams: [],
            outParams: [],
            key: [],//键
            value: [],//值
            sidx: undefined,//排序字段
            sord: "DESC",//排序方式
            custom: [], //自定义数据
          },
        };
      }
    },
  },
};
</script>