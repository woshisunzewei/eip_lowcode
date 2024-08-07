<template>
  <div>
    <a-form-item v-if="typeof value.dialog !== 'undefined' && typeof value.dialog.width !== 'undefined'" label="弹出类型">
      <a-select v-model="value.dialog.type">
        <a-select-option value="modal">
          对话框
        </a-select-option>
        <a-select-option value="drawer">
          抽屉
        </a-select-option>
      </a-select>
    </a-form-item>

    <a-form-item v-if="typeof value.dialog !== 'undefined' && typeof value.dialog.width !== 'undefined'" label="弹出框宽度">
      <a-input-group compact>
        <a-input-number style="width:174px" v-model="value.dialog.width" placeholder="请输入弹出宽度" />
        <a-select v-model="value.dialog.widthUnit" style="width:60px">
          <a-select-option value="px">
            px
          </a-select-option>
          <a-select-option value="%">
            %
          </a-select-option>
        </a-select>
      </a-input-group>
    </a-form-item>
    <a-form-item label="弹出选项"
      v-if="typeof value.dialog !== 'undefined' && typeof value.dialog.centered !== 'undefined'">
      <a-checkbox v-if="value.dialog.type == 'modal'" v-model="value.dialog.centered">弹出居中</a-checkbox>
      <a-checkbox v-model="value.dialog.maskClosable">点击蒙层是否允许关闭</a-checkbox>
    </a-form-item>
    <a-form-item label="弹出层级数" v-if="typeof value.dialog !== 'undefined' && typeof value.dialog.zIndex !== 'undefined'">
      <a-tooltip>
        <div slot="title">
          数值越大越在顶部
        </div>
        <a-input-number class="eip-width-full" :min="1000" v-model="value.dialog.zIndex" />
      </a-tooltip>
    </a-form-item>
    <a-form-model-item label="数据源">
      <a-badge :dot="value.dynamicConfig.dataSourceId != undefined" style="width:100%">
        <a-button @click="sourceChosen" block><a-icon type="database" /> 选择数据源</a-button>
      </a-badge>
    </a-form-model-item>
    <a-form-model-item label="显示列"> <a-badge :dot="value.dynamicConfig.dataSourceId != undefined" style="width:100%">
        <a-button @click="columnsChosen" block><a-icon type="unordered-list" /> 配置显示列
        </a-button></a-badge>
    </a-form-model-item>
    <a-form-model-item label="数据映射">
      <a-badge :dot="value.map.length > 0" style="width:100%">
        <a-button @click="mapChosen" block><a-icon type="pull-request" /> 配置数据映射 </a-button>
      </a-badge>
    </a-form-model-item>
    <a-form-model-item label="查询条件">
      <a-badge :dot="value.search.columns.length > 0" style="width:100%">
        <a-button @click="searchChosen" block><a-icon type="search" /> 配置查询条件
        </a-button></a-badge>
    </a-form-model-item>

    <a-modal v-drag width="1000px" :bodyStyle="{ padding: '1px' }" :dialog-style="{ top: '20px' }"
      :title="dataSource.title" :visible="dataSource.visible" :maskClosable="false"
      @cancel="dataSource.visible = false">
      <template slot="footer">
        <a-button key="back" @click="dataSource.visible = false">关闭</a-button>
      </template>
      <a-form-model :label-col="labelCol" :wrapper-col="wrapperCol">
        <a-form-model-item label="数据源">
          <a-select allow-clear @change="dataSourceChange($event, true)" v-model="value.dynamicConfig.dataSourceId"
            style="width: 100%" placeholder="请选择">
            <div slot="dropdownRender" slot-scope="menu">
              <v-nodes :vnodes="menu" />
              <a-divider style="margin: 4px 0;" />
              <div style="padding: 4px 8px; cursor: pointer;" @mousedown="e => e.preventDefault()" @click="addItem">
                <a-icon type="plus" />新增数据源
              </div>
            </div>
            <a-select-option v-for="(oitem, oindex) in dataSource.value" :value="oitem.dataSourceId" :key="oindex">
              {{ oitem.name }} </a-select-option>
          </a-select>
        </a-form-model-item>

        <a-form-model-item label="参数映射">
          <vxe-table row-key height="350" :data="value.dynamicConfig.inParams">

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
    </a-modal>

    <a-modal centered :zIndex="99999" width="370px" v-drag :visible="condition.visible" :bodyStyle="{ padding: '1px' }"
      :destroyOnClose="true" :maskClosable="false" :title="condition.title" @cancel="conditionCancel" :footer="null">
      <a-spin :spinning="condition.tree.spinning">
        <a-row>
          <a-col>
            <a-card class="eip-admin-card-small" size="small">
              <a-directory-tree :expandAction="false" default-expand-all :style="condition.tree.style"
                :tree-data="condition.tree.data" @select="onSelect"
                v-if="condition.tree.data.length > 0"></a-directory-tree>
              <a-empty :style="condition.tree.style" v-if="condition.tree.data.length == 0" description="无相关信息" />
            </a-card>
          </a-col>
        </a-row>
      </a-spin>
    </a-modal>

    <a-modal width="1300px" v-drag :visible="columns.visible" :bodyStyle="{ padding: '1px' }" :destroyOnClose="true"
      :maskClosable="false" title="显示需要查看列" @cancel="columns.visible = false">

      <template slot="footer">
        <a-button key="back" @click="columns.visible = false">关闭</a-button>
      </template>
      <a-row>
        <a-col>
          <a-card class="eip-admin-card-small" size="small">
            <vxe-table ref="tableColumns" id="tableColumns" size="small" :height="500" :data="value.columns">

              <template #loading>
                <a-spin></a-spin>
              </template>

              <template #empty>
                <eip-empty />
              </template>
              <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>

              <vxe-column field="name" title="字段" width="150">
              </vxe-column>

              <vxe-column field="isSearch" title="查询" width="60" align="center">

                <template v-slot="{ row }">
                  <a-switch checked-children="是" un-checked-children="否" v-model="row.isSearch" />
                </template>
              </vxe-column>
              <vxe-column field="isShow" title="显示" width="70" align="center">

                <template v-slot="{ row }">
                  <a-switch checked-children="是" un-checked-children="否" v-model="row.isShow" />
                </template>
              </vxe-column>
              <vxe-column title="显示名" align="center" width="200">

                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-input allow-clear :placeholder="row.description" v-model="row.title" />
                  </a-space>
                </template>
              </vxe-column>
              <vxe-column field="format" title="格式化" align="center" width="120">
                <template v-slot="{ row }">
                  <a-select allow-clear placeholder="格式化" v-model="row.format" style="width: 100px">
                    <a-select-option v-for="item in eipFormat" :value="item.value" :key="item.value">{{ item.label
                      }}</a-select-option>
                  </a-select>
                </template>
              </vxe-column>
              <vxe-column title="掩码显示" align="center" width="160">

                <template v-slot="{ row }">
                  <a-input-group compact>
                    <select class="ant-select-selection ant-select-selection--single" v-model="row.mask"
                      placeholder="掩码显示" style="width: 100%">
                      <option value=""></option>
                      <option v-for="item in eipMask" :value="item.value" :title="item.label" :key="item.value">{{
                        item.label }}
                      </option>
                    </select>
                  </a-input-group>
                </template>
              </vxe-column>
              <vxe-column field="fixed" title="固定" align="center" width="120">

                <template v-slot="{ row }">
                  <a-select allow-clear placeholder="请选择固定" v-model="row.fixed" style="width: 100px">
                    <a-select-option value="left">左</a-select-option>
                    <a-select-option value="right">右</a-select-option>
                  </a-select>
                </template>
              </vxe-column>
              <vxe-column title="宽度(px)" align="center" width="110">

                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-input-number placeholder="请输入宽度" v-model="row.width" />
                  </a-space>
                </template>
              </vxe-column>
              <vxe-column field="sord" title="排序" width="60" align="center">

                <template v-slot="{ row }">
                  <a-switch checked-children="是" un-checked-children="否" v-model="row.sord" />
                </template>
              </vxe-column>
              <vxe-column field="sordType" title="排序类型" align="center" width="120">

                <template v-slot="{ row }">
                  <a-select allow-clear v-model="row.sordType" style="width: 100px" placeholder="请选择排序类型">
                    <a-select-option value="desc">降序</a-select-option>
                    <a-select-option value="asc">升序</a-select-option>
                  </a-select>
                </template>
              </vxe-column>
            </vxe-table>
          </a-card>
        </a-col>
      </a-row>
    </a-modal>

    <a-modal width="800px" v-drag :visible="map.visible" :bodyStyle="{ padding: '1px' }" :destroyOnClose="true"
      :maskClosable="false" title="配置数据映射" @cancel="map.visible = false">

      <template slot="footer">
        <a-button key="back" @click="map.visible = false">关闭</a-button>
      </template>
      <a-row>
        <a-col>
          <a-card class="eip-admin-card-small" size="small">
            <vxe-table ref="tableMap" id="tableMap" size="small" :height="500" :data="value.map">

              <template #loading>
                <a-spin></a-spin>
              </template>

              <template #empty>
                <eip-empty />
              </template>
              <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>

              <vxe-column field="name" title="字段" width="200">
              </vxe-column>
              <vxe-column field="description" title="描述" width="130" showOverflow="ellipsis">
              </vxe-column>
              <vxe-column field="to" title="至" width="40" align="center" showOverflow="ellipsis">
              </vxe-column>
              <vxe-column title="映射字段" align="center" width="350">

                <template v-slot="{ row }">
                  <a-space size="small">
                    <a-input allow-clear :placeholder="row.description" v-model="row.mapcolumn" />
                    <a-button @click="conditionSetting('map', row)">
                      <a-icon type="setting" />
                    </a-button>
                  </a-space>
                </template>
              </vxe-column>
            </vxe-table>
          </a-card>
        </a-col>
      </a-row>
    </a-modal>

    <a-drawer v-if="search.visible" width="1000px" :bodyStyle="{ padding: '1px' }" title="查询条件"
      :visible="search.visible" @close="search.visible = false" :destroyOnClose="true">
      <search-index ref="search" v-if="search.visible" :visible.sync="search.visible" :config="search.config"
        :columns="search.columns" @ok="searchOk" />
    </a-drawer>

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :title="edit.title" @ok="dataSourceInit"></edit>
  </div>
</template>

<script>
import { query } from "@/services/system/datasource/list";
import searchIndex from "@/pages/system/agile/common/search/index";
import edit from "@/pages/system/datasource/edit";
/*
 * author 孙泽伟
 * date 2022-03-28
 * description 数据源选择
 */
export default {
  name: "KDataList",
  components: {
    edit,
    searchIndex,
    VNodes: {
      functional: true,
      render: (h, ctx) => ctx.props.vnodes,
    },
  },
  data() {
    return {
      edit: {
        visible: false,
        title: "",
      },
      dataSource: {
        value: [],
        outParams: [],
        title: "数据源配置",
        visible: false
      },//数据源
      bodyStyle: {
        padding: "4px",
        "background-color": "#f0f2f5",
      },
      labelCol: { span: 4 },
      wrapperCol: { span: 20 },

      condition: {
        title: "选择字段",
        visible: false,

        tree: {
          row: null,
          style: {
            overflow: "auto",
            height: "400px",
          },
          data: [],
          spinning: false,
        },
      },

      loading: false,

      columns: {
        visible: false,
      },

      map: {
        visible: false,
      },

      search: {
        visible: false,
        columns: [],
        config: {
          labelCol: 8,
          wrapperCol: 16,
          num: 4, //显示个数
          columns: [], //查询条件
        },
      },
    };
  },
  props: {
    value: {
      type: Object,
      required: true,
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
      var dataSource = this.dataSource.value.filter(f => f.dataSourceId == value)
      if (reloadMap) {
        this.value.dynamicConfig.inParams = [];
        this.value.map = [];
        this.value.columns = [];
        this.value.search = {
          labelCol: 8,
          wrapperCol: 16,
          num: 6, //显示个数
          columns: []
        } //查询条件;
      }
      if (dataSource.length > 0) {
        var config = JSON.parse(dataSource[0].config)
        if (this.value.dynamicConfig.inParams.length == 0) {
          this.value.dynamicConfig.inParams = [];
          config.inParams.forEach(item => {
            this.value.dynamicConfig.inParams.push({
              title: item.title,
              key: item.key,
              condition: ''
            })
          })
        }

        if (this.value.columns.length == 0) {
          config.outParams.forEach(item => {
            this.value.columns.push({
              name: item.key,
              description: item.title,
              format: undefined,
              mask: '',
              isSearch: true,
              isShow: true,
              sord: false,
              sordType: undefined,
              title: item.title,
              fixed: undefined,
              width: 100,
            })
          })
        }
      }
    },

    /**
     * 查询配置点击
     */
    searchChosen() {
      let that = this;
      this.search.columns = [];
      var columns = this.$utils.clone(this.value.columns, true).filter(
        (f) => f.isShow || f.isSearch
      );
      columns.forEach((item) => {
        that.search.columns.push({
          name: item.name,
          description: item.description,
          isShow: item.isShow,
        });
      });
      this.search.config = this.value.search;
      this.search.visible = true;
    },

    /**
     * 查询配置保存
     */
    searchOk(config) {
      this.value.search = config;
    },

    /**
    * 显示列源选择
    */
    mapChosen() {
      let that = this;
      var columns = this.$utils.clone(this.value.columns, true);
      //移除已不在里面项
      that.value.map.forEach((f, index) => {
        var have = columns.filter(c => c.name == f.name);
        if (have.length == 0) {
          that.value.map.splice(index, 1);
        }
      })
      columns.forEach((item) => {
        //判断字段是否存在
        var have = that.value.map.filter(f => f.name == item.name);
        if (have.length == 0) {
          that.value.map.push({
            name: item.name,
            description: item.title,
            to: "->",
            mapcolumn: "",
          });
        }
      });
      this.map.visible = true;
    },
    /**
         * 初始化条件
         */
    conditionInit() {
      let that = this;

      that.condition.tree.spinning = true;
      let fieldSchema = this.eipAgileDesigner.kfd.getFieldSchema();
      fieldSchema
        .filter((f) => !["batch", "text", "divider"].includes(f.type))
        .forEach((item) => {
          that.condition.tree.data.push({
            key: item.model,
            isLeaf: true,
            title: item.model + (item.label != "" ? "(" + item.label + ")" : ""),
          });
        })

      //得到子表
      fieldSchema
        .filter((f) => f.type == "batch")
        .forEach((item) => {
          var childrens = [];
          if (item.list.length > 0) {
            var children = {
              title: item.model + (item.label != "" ? "(" + item.label + ")" : ""),
            }

            item.list.forEach((control) => {
              childrens.push({
                key: item.model + "." + control.model,
                isLeaf: true,
                title: control.model + (control.label != "" ? "(" + control.label + ")" : ""),
              });
            });
            children.children = childrens;
            that.condition.tree.data.push(children);
          }
        });
      that.condition.tree.data.push(this.eipSystemFiled)
      that.condition.tree.spinning = false;
    },

    /**
     * 显示列源选择
     */
    columnsChosen() {
      this.columns.visible = true;
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
          that.dataSourceChange(that.value.dynamicConfig.dataSourceId, false)
        });
      }
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

    /** */
    conditionCancel() {
      this.condition.visible = false;
    },

    /**
     * 选择
     */
    onSelect(keys, event) {
      if (event.node.getNodeChildren().length == 0) {
        if (typeof this.condition.tree.row.condition != "undefined") {
          this.condition.tree.row.condition = keys[0];
        }
        if (typeof this.condition.tree.row.mapcolumn != "undefined") {
          this.condition.tree.row.mapcolumn = keys[0];
        }
        this.conditionCancel();
      }
    },

    addItem() {
      this.edit.title = "新增数据源配置";
      this.edit.visible = true;
    }
  },
};
</script>

<style lang="less" scoped>
.ant-form-item {
  margin-bottom: 0;
}
</style>
