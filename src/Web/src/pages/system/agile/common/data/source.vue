<template>
  <a-drawer width="900px" :bodyStyle="{ padding: '1px' }" title="数据源配置" :visible="visible" @close="cancel"
    :destroyOnClose="true">
    <div class="eip-drawer-body beauty-scroll">
      <a-form-model :label-col="labelCol" :wrapper-col="wrapperCol">
        <a-form-model-item label="数据源">
          <a-select allow-clear @change="dataSourceChange($event)" v-model="dataSource.dataSourceId" style="width: 100%"
            placeholder="请选择">
            <div slot="dropdownRender" slot-scope="menu">
              <v-nodes :vnodes="menu" />
              <a-divider style="margin: 4px 0;" />
              <div style="padding: 4px 8px; cursor: pointer;" @mousedown="e => e.preventDefault()" @click="addItem">
                <a-icon type="plus" /> Add item
              </div>
            </div>
            <a-select-option v-for="(oitem, oindex) in dataSource.value" :value="oitem.dataSourceId" :key="oindex">
              {{ oitem.name }} </a-select-option>
          </a-select>
        </a-form-model-item>

        <a-form-model-item label="参数映射">
          <vxe-table row-key height="350" :data="dataSource.inParams">
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column title="参数" width="300px" field="title">
            </vxe-column>

            <vxe-column align="center" width="350">
              <template #header>
                <span slot="label">
                  过滤条件(固定值添加单引号)
                  <a-tooltip title="过滤条件:固定值添加单引号,需要根据表单字段过滤,点击设置按钮,选择关联字段(实现数据联动)">
                    <a-icon type="question-circle-o" />
                  </a-tooltip>
                </span>
              </template>
              <template v-slot="{ row }">
                <a-space size="small">
                  <a-input allow-clear :placeholder="row.title" v-model="row.condition" />
                  <a-button @click="conditionSetting('condition', row)">
                    <a-icon type="setting" />
                  </a-button>
                </a-space>
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
  </a-drawer>
</template>

<script>
import { query } from "@/services/system/datasource/list";
export default {
  name: "data-source",
  components: {
    VNodes: {
      functional: true,
      render: (h, ctx) => ctx.props.vnodes,
    },
  },
  data() {
    return {
      labelCol: { span: 4 },
      wrapperCol: { span: 20 },
      dataSource: {
        value: [],
        title: '',
        dataSourceId: undefined,
        inParams: [],
        outParams: [],
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
   * 数据源改变
   */
    dataSourceChange(value) {
      var dataSource = this.dataSource.value.filter(f => f.dataSourceId == value)
      if (dataSource.length > 0) {
        var config = JSON.parse(dataSource[0].config)
        this.dataSource.title = dataSource[0].name;
        this.dataSource.inParams = [];
        config.inParams.forEach(item => {
          this.dataSource.inParams.push({
            title: item.title,
            key: item.key,
            condition: ''
          })
        })

        this.dataSource.outParams = [];
        config.outParams.forEach(item => {
          this.dataSource.outParams.push({
            name: item.key,
            description: item.title,
            title: item.title,
          })
        })
      }
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
      this.$emit("ok", this.dataSource);
    },


    /**
     * 初始化
     */
    init(config) {
      let that = this;
      if (config) {
        query({ size: 9999 }).then((result) => {
          that.dataSource.value = result.data;
          that.dataSource.dataSourceId = config.dataSourceId;
          that.dataSource.inParams = config.inParams;
          that.dataSource.outParams = config.outParams;
          that.dataSource.title = config.title;
        });
      } else {
        query({ size: 9999 }).then((result) => {
          that.dataSource.value = result.data;
        });
      }
    },
  },
};
</script>