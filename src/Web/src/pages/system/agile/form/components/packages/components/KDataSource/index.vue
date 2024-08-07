<template>
  <div>
    <a-form-model-item label="数据源">
      <a-badge
        :dot="
          value.dynamicConfig.dataSourceId &&
          value.dynamicConfig.dataSourceId.length > 0
        "
        style="width: 100%"
      >
        <a-button @click="sourceChosen" block
          ><a-icon type="database" /> 选择数据源</a-button
        >
      </a-badge>
    </a-form-model-item>
    <a-form-model-item label="键">
      <a-select
        @change="$forceUpdate()"
        v-model="value.dynamicConfig.key"
        placeholder="请选择键"
      >
        <a-select-option
          v-for="(item, index) in dataSource.outParams"
          :value="item.field"
          :key="'key' + index"
        >
          {{ item.title ? item.title : item.field }}
        </a-select-option>
      </a-select>
    </a-form-model-item>

    <a-form-model-item label="值" v-if="type != 'autoComplete'">
      <a-select
        @change="$forceUpdate()"
        v-model="value.dynamicConfig.value"
        placeholder="请选择值"
      >
        <a-select-option
          v-for="(item, index) in dataSource.outParams"
          :value="item.field"
          :key="'value' + index"
        >
          {{ item.title ? item.title : item.field }}
        </a-select-option>
      </a-select>
    </a-form-model-item>

    <a-form-model-item
      v-if="
        type != 'select' &&
        type != 'checkbox' &&
        type != 'radio' &&
        type != 'autoComplete'
      "
      label="父级"
    >
      <a-select
        @change="$forceUpdate()"
        v-model="value.dynamicConfig.parentId"
        placeholder="请选择值"
      >
        <a-select-option
          v-for="(item, index) in dataSource.outParams"
          :value="item.field"
          :key="'value' + index"
        >
          {{ item.title ? item.title : item.field }}
        </a-select-option>
      </a-select>
    </a-form-model-item>

    <a-form-model-item label="排序">
      <a-select
        @change="$forceUpdate()"
        v-model="value.dynamicConfig.sidx"
        placeholder="请选择排序字段"
      >
        <a-select-option
          v-for="(item, index) in dataSource.outParams"
          :value="item.field"
          :key="'sidx' + index"
        >
          {{ item.title ? item.title : item.field }}
        </a-select-option>
      </a-select>
    </a-form-model-item>

    <a-form-model-item label="排序方式">
      <a-select v-model="value.dynamicConfig.sord">
        <a-select-option :value="'desc'">降序</a-select-option>
        <a-select-option :value="'asc'">升序</a-select-option>
      </a-select>
    </a-form-model-item>
    <a-form-model-item label="数据映射" v-if="type != 'autoComplete'">
      <a-badge :dot="value.dynamicConfig.map.length > 0" style="width: 100%">
        <a-button @click="mapChosen" block
          ><a-icon type="pull-request" /> 配置数据映射
        </a-button>
      </a-badge>
    </a-form-model-item>

    <a-modal
      v-drag
      centered
      width="1000px"
      :bodyStyle="{ padding: '1px' }"
      :dialog-style="{ top: '20px' }"
      :title="dataSource.title"
      :visible="dataSource.visible"
      :maskClosable="false"
      @cancel="dataSource.visible = false"
    >
      <template slot="footer">
        <a-button key="back" @click="dataSource.visible = false">关闭</a-button>
      </template>
      <a-form-model :label-col="labelCol">
        <a-form-model-item :wrapper-col="wrapperCol" label="数据源">
          <a-select
            allow-clear
            @change="dataSourceChange($event, true)"
            v-model="value.dynamicConfig.dataSourceId"
            style="width: 100%"
            placeholder="请选择"
          >
            <div slot="dropdownRender" slot-scope="menu">
              <v-nodes :vnodes="menu" />
              <a-divider style="margin: 4px 0" />
              <div
                style="padding: 4px 8px; cursor: pointer"
                @mousedown="(e) => e.preventDefault()"
                @click="addItem"
              >
                <a-icon type="plus" />新增数据源
              </div>
            </div>
            <a-select-option
              v-for="(oitem, oindex) in dataSource.value"
              :value="oitem.dataSourceId"
              :key="oindex"
            >
              {{ oitem.name }}
            </a-select-option>
          </a-select>
        </a-form-model-item>

        <a-form-model-item :wrapper-col="wrapperCol" label="参数映射">
          <vxe-table
            row-key
            ref="outParams"
            height="450"
            :data="value.dynamicConfig.inParams"
          >
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column title="参数" width="300px" field="title"> </vxe-column>

            <vxe-column align="center" width="350">
              <template #header>
                <span slot="label">
                  过滤条件(固定值添加单引号)
                  <a-tooltip
                    title="过滤条件:固定值添加单引号,需要根据表单字段过滤,点击设置按钮,选择关联字段(实现数据联动)"
                  >
                    <a-icon type="question-circle-o" />
                  </a-tooltip>
                </span>
              </template>
              <template v-slot="{ row }">
                <a-space size="small">
                  <eip-editor
                    style="width: 200px"
                    :ref="'editor_' + row.field"
                    v-model="row.condition"
                    content_style="p{margin:5px} body{margin:4px} .mce-content-body:not([dir=rtl])[data-mce-placeholder]:not(.mce-visualblocks)::before{left:6px}"
                    :height="tinymce.height"
                    :toolbar="tinymce.toolbar"
                    :plugins="tinymce.plugins"
                    :menubar="tinymce.menubar"
                  ></eip-editor>
                  <a-button @click="conditionSetting('condition', row)">
                    <a-icon type="setting" />
                  </a-button>
                </a-space>
              </template>
            </vxe-column>
          </vxe-table>
        </a-form-model-item>
      </a-form-model>
    </a-modal>

    <a-modal
      v-drag
      width="800px"
      :visible="map.visible"
      :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true"
      :maskClosable="false"
      title="配置数据映射"
      @cancel="map.visible = false"
    >
      <template slot="footer">
        <a-button key="back" @click="map.visible = false">关闭</a-button>
      </template>
      <a-row>
        <a-col>
          <a-card class="eip-admin-card-small" size="small">
            <vxe-table
              ref="tableMap"
              id="tableMap"
              size="small"
              :height="500"
              :data="value.dynamicConfig.map"
            >
              <template #loading>
                <a-spin></a-spin>
              </template>
              <template #empty>
                <eip-empty />
              </template>
              <vxe-column
                type="seq"
                title="序号"
                align="center"
                width="60"
              ></vxe-column>
              <vxe-column field="field" title="字段" width="200"> </vxe-column>
              <vxe-column
                field="description"
                title="描述"
                width="130"
                showOverflow="ellipsis"
              >
              </vxe-column>
              <vxe-column
                field="to"
                title="至"
                width="40"
                align="center"
                showOverflow="ellipsis"
              >
              </vxe-column>
              <vxe-column title="映射字段" align="center" width="300">
                <template v-slot="{ row }">
                  <a-space size="small">
                    <eip-editor
                      style="width: 200px"
                      :ref="'editor_map_' + row.field"
                      v-model="row.map"
                      content_style="p{margin:5px} body{margin:4px} .mce-content-body:not([dir=rtl])[data-mce-placeholder]:not(.mce-visualblocks)::before{left:6px}"
                      :height="tinymce.height"
                      :toolbar="tinymce.toolbar"
                      :plugins="tinymce.plugins"
                      :menubar="tinymce.menubar"
                    ></eip-editor>

                    <a-button @click="conditionSetting('map', row)">
                      <a-icon type="unordered-list" />
                    </a-button>
                  </a-space>
                </template>
              </vxe-column>
            </vxe-table>
          </a-card>
        </a-col>
      </a-row>
    </a-modal>

    <a-modal
      v-drag
      :zIndex="99999"
      width="370px"
      :visible="condition.visible"
      :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true"
      :maskClosable="false"
      :title="condition.title"
      @cancel="conditionCancel"
      :footer="null"
    >
      <a-spin :spinning="condition.tree.spinning">
        <a-row>
          <a-col>
            <a-card class="eip-admin-card-small" size="small">
              <a-directory-tree
                default-expand-all
                :style="condition.tree.style"
                :tree-data="condition.tree.data"
                :expandAction="false"
                @select="onSelect"
                v-if="condition.tree.data.length > 0"
              ></a-directory-tree>
              <eip-empty
                :style="condition.tree.style"
                v-if="condition.tree.data.length == 0"
                description="无相关信息"
              />
            </a-card>
          </a-col>
        </a-row>
      </a-spin>
    </a-modal>
    <edit
      ref="edit"
      v-if="edit.visible"
      :visible.sync="edit.visible"
      :title="edit.title"
      @ok="dataSourceInit"
    ></edit>
  </div>
</template>
<script>
import { query } from "@/services/system/datasource/list";
import edit from "@/pages/system/datasource/edit";
/*
 * author 孙泽伟
 * date 2022-03-28
 * description 数据源选择
 */
export default {
  name: "KDataSource",
  components: {
    edit,
    VNodes: {
      functional: true,
      render: (h, ctx) => ctx.props.vnodes,
    },
  },
  data() {
    return {
      tinymce: {
        plugins: "",
        toolbar: "",
        height: 48,
        menubar: "",
      },
      edit: {
        visible: false,
        title: "",
      },
      dataSource: {
        value: [],
        outParams: [],
        title: "数据源配置",
        visible: false,
      }, //数据源
      bodyStyle: {
        padding: "4px",
        "background-color": "#f0f2f5",
      },
      height: this.eipHeaderHeight() - 190 + "px",
      labelCol: { span: 4 },
      wrapperCol: { span: 20 },

      condition: {
        title: "选择字段",
        visible: false,

        tree: {
          type: "",
          row: null,
          style: {
            overflow: "auto",
            height: "400px",
          },
          data: [],
          spinning: false,
        },
      },

      map: {
        visible: false,
      },
      loading: false,
    };
  },
  props: {
    value: {
      type: Object,
      required: true,
    },
    type: {
      type: String,
    },
  },
  created() {
    this.dataSourceInit();
    this.conditionInit();
  },
  methods: {
    /**
     *
     */
    dataSourceChange(value, reloadMap = true) {
      var dataSource = this.dataSource.value.filter(
        (f) => f.dataSourceId == value
      );
      if (reloadMap) {
        this.value.dynamicConfig.inParams = [];
        this.value.dynamicConfig.map = [];
        this.value.dynamicConfig.key = undefined;
        this.value.dynamicConfig.value = undefined;
        this.value.dynamicConfig.sidx = undefined;
        this.value.dynamicConfig.sord = undefined;
      }
      this.dataSource.outParams = [];
      if (dataSource.length > 0) {
        var config = JSON.parse(dataSource[0].config);

        if (this.value.dynamicConfig.inParams.length == 0) {
          this.value.dynamicConfig.inParams = [];
          config.inParams.forEach((item, index) => {
            this.value.dynamicConfig.inParams.push({
              index: index,
              title: item.title,
              field: item.field,
              condition: "",
            });
          });
        }

        config.outParams.forEach((item) => {
          this.dataSource.outParams.push({
            title: item.title,
            field: item.field,
            condition: "",
          });
        });
      }
    },

    /**
     * 数据源选择
     */
    sourceChosen() {
      this.dataSource.visible = true;
      this.dataSourceInit();
      this.conditionInit();
    },

    /**
     * 初始化数据
     */
    dataSourceInit() {
      let that = this;
      if (!this.value.dynamicConfig.dataSourceId) {
        query({ size: 9999 }).then((result) => {
          that.dataSource.value = result.data;
        });
      } else {
        query({ size: 9999 }).then((result) => {
          that.dataSource.value = result.data;
          that.dataSourceChange(that.value.dynamicConfig.dataSourceId, false);
        });
      }
    },

    /**
     * 显示列源选择
     */
    mapChosen() {
      let that = this;
      var columns = that.$utils.clone(this.dataSource.outParams, true);
      //移除已不在里面项
      that.value.dynamicConfig.map.forEach((f, index) => {
        var have = columns.filter((c) => c.field == f.field);
        if (have.length == 0) {
          that.value.dynamicConfig.map.splice(index, 1);
        }
      });
      columns.forEach((item, index) => {
        //判断字段是否存在
        var have = that.value.dynamicConfig.map.filter(
          (f) => f.field == item.field
        );
        if (have.length == 0) {
          that.value.dynamicConfig.map.push({
            index: index,
            field: item.field,
            description: item.title,
            to: "->",
            map: "",
          });
        }
      });

      this.map.visible = true;
    },

    /**
     * 条件配置
     */
    conditionSetting(type, row) {
      //得到所有组件
      this.condition.tree.type = type;
      this.condition.tree.row = row;
      this.condition.visible = true;
    },

    /**
     * 初始化条件
     */
    conditionInit() {
      let that = this;
      that.condition.tree.data = [];
      that.condition.tree.spinning = true;
      let fieldSchema = this.eipAgileDesigner.kfd.getFieldSchema();
      fieldSchema
        .filter((f) => !["batch", "text", "divider"].includes(f.type))
        .forEach((item) => {
          that.condition.tree.data.push({
            key: item.key,
            isLeaf: true,
            title: item.label != "" ? item.label : item.model,
          });
        });

      //得到子表
      fieldSchema
        .filter((f) => f.type == "batch")
        .forEach((item) => {
          var childrens = [];
          if (item.list.length > 0) {
            var children = {
              title:
                item.key + (item.label != "" ? "(" + item.label + ")" : ""),
            };

            item.list.forEach((control) => {
              childrens.push({
                key: item.key + "." + control.key,
                isLeaf: true,
                title: control.label != "" ? control.label : control.model,
              });
            });
            children.children = childrens;
            that.condition.tree.data.push(children);
          }
        });
      that.condition.tree.data.push(this.eipSystemFiled);
      that.condition.tree.spinning = false;
    },

    /** */
    conditionCancel() {
      this.condition.visible = false;
    },

    /**
     * 选择
     */
    onSelect(keys, event) {
      if (event.node.getNodeChildren().length == 0) {
        var treeData = this.condition.tree.data;
        let treeList = this.$utils.toTreeArray(treeData);
        var chosenColumn = this.$utils.find(treeList, (f) => f.key == keys[0]);

        if (typeof this.condition.tree.row.condition != "undefined") {
          let html =
            "<span id='" +
            encodeURIComponent(chosenColumn.key) +
            "' class='non-editable'>" +
            chosenColumn.title +
            "</span>";
          this.$refs["editor_" + this.condition.tree.row.field].insertIndex(
            html,
            this.condition.tree.row.index
          );
        }

        if (typeof this.condition.tree.row.map != "undefined") {
          let html =
            "<span id='" +
            encodeURIComponent(chosenColumn.key) +
            "' class='non-editable'>" +
            chosenColumn.title +
            "</span>";
          this.$refs["editor_map_" + this.condition.tree.row.field].insertIndex(
            html,
            this.condition.tree.row.index
          );
        }

        this.conditionCancel();
      }
    },

    addItem() {
      this.edit.title = "新增数据源配置";
      this.edit.visible = true;
    },
  },
};
</script>
<style lang="less" scoped>
.ant-form-item {
  margin-bottom: 0;
}
</style>
