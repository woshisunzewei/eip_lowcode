<template>
  <div class="option-change-container" ref="container">
    <a-row :gutter="8" class="option-change-box-row">
      <div class="option-change-box" v-for="(val, index) in value" :key="index">
        <a-col :span="9"><a-input v-model="val.label" placeholder="按钮名称" :allowClear="true" /></a-col>
        <a-col :span="9"><eip-icon :name="val.icon" @click="(icon) => { val.icon = icon.name }"
            @clear="val.icon = ''"></eip-icon></a-col>
        <a-col :span="6">
          <a-space>
            <a-tooltip>
              <template slot="title">
                设置
              </template>
              <a-icon @click="handleEdit(index)" type="edit" />
            </a-tooltip>
            <a-tooltip>
              <template slot="title">
                删除
              </template>
              <a-icon @click="handleDelete(index)" type="delete" style="color:red" />
            </a-tooltip>
            <a-tooltip>
              <template slot="title">
                拖动排序
              </template>
              <span class="drag-btn">
                <a-icon type="drag" />
              </span>
            </a-tooltip>

          </a-space>
        </a-col>
      </div>
    </a-row>
    <a-col :span="24"><a @click="handleAdd">添加</a></a-col>
    <a-modal v-drag width="1100px" :bodyStyle="{ padding: '1px' }" :dialog-style="{ top: '20px' }" :title="button.title"
      :visible="button.visible" :maskClosable="false" @cancel="button.visible = false">

      <a-form-model ref="form" :model="button.option" :label-col="config.labelCol">
        <a-row>
          <a-col :span="12">
            <a-form-model-item label="名称" :wrapper-col="config.wrapperCol">
              <a-input allow-clear v-model="button.option.label" placeholder="请输入名称"></a-input>
            </a-form-model-item>

            <a-form-model-item label="按钮类型" :wrapper-col="config.wrapperCol">
              <a-select placeholder="请选择按钮类型" default-value="primary" allow-clear v-model="button.option.type">
                <a-select-option value="primary">主按钮 </a-select-option>
                <a-select-option value=""> 次按钮 </a-select-option>
                <a-select-option value="dashed"> 虚线按钮 </a-select-option>
                <a-select-option value="danger"> 危险按钮 </a-select-option>
                <a-select-option value="link"> 链接按钮 </a-select-option>
                <a-select-option value="upload"> 上传按钮 </a-select-option>
              </a-select>
            </a-form-model-item>
            <a-form-model-item v-if="button.option.trigger.type == 'chosen'" label="弹出宽度(px)"
              :wrapper-col="config.wrapperCol">
              <a-input-number placeholder="请输入弹出宽度" style="width:100%" v-model="button.option.trigger.option.width" />
            </a-form-model-item>

            <a-form-model-item v-if="button.option.trigger.type == 'chosen'" label="数据源"
              :wrapper-col="config.wrapperCol">
              <a-button @click="sourceChosen" block><a-icon type="database" />选择数据源</a-button>
            </a-form-model-item>

            <a-form-model-item v-if="button.option.trigger.type == 'chosen'" label="数据映射"
              :wrapper-col="config.wrapperCol">
              <a-button @click="mapChosen" block><a-icon type="pull-request" /> 配置数据映射 </a-button>
            </a-form-model-item>

            <a-form-model-item label="选项" :wrapper-col="config.wrapperCol">
              <a-checkbox :checked="button.option.isShow" @change="button.option.isShow = !button.option.isShow">
                显示
              </a-checkbox>
              <a-checkbox :checked="button.option.trigger.option.multiple" v-if="button.option.trigger.type == 'chosen'"
                @change="button.option.trigger.option.multiple = !button.option.trigger.option.multiple">
                多选
              </a-checkbox>
              <a-checkbox :checked="button.option.trigger.option.centered" v-if="button.option.trigger.type == 'chosen'"
                @change="button.option.trigger.option.centered = !button.option.trigger.option.centered">
                弹出居中
              </a-checkbox>
              <a-checkbox :checked="button.option.trigger.option.isPaging" v-if="button.option.trigger.type == 'chosen'"
                @change="button.option.trigger.option.isPaging = !button.option.trigger.option.isPaging">
                分页
              </a-checkbox>
            </a-form-model-item>
          </a-col>

          <a-col :span="12">
            <a-form-model-item label="图标" :wrapper-col="config.wrapperCol">
              <eip-icon :name="button.option.icon" @click="(icon) => { button.option.icon = icon.name }"
                @clear="button.option.icon = ''"></eip-icon>
            </a-form-model-item>
            <a-form-model-item label="触发类型" :wrapper-col="config.wrapperCol">
              <a-select placeholder="请选择触发类型" allow-clear v-model="button.option.trigger.type">
                <a-select-option value="add"> 新增 </a-select-option>
                <a-select-option value="reset"> 重置 </a-select-option>
                <a-select-option value="delete"> 删除 </a-select-option>
                <a-select-option value="chosen"> 弹出数据选择 </a-select-option>
                <a-select-option value="export"> 导出 </a-select-option>
                <a-select-option value="import"> 导入 </a-select-option>
                <a-select-option value="customer"> 自定义 </a-select-option>
              </a-select>
            </a-form-model-item>

            <a-form-model-item v-if="button.option.trigger.type == 'chosen'" label="弹出高度(px)"
              :wrapper-col="config.wrapperCol">
              <a-input-number placeholder="请输入弹出高度" style="width:100%" v-model="button.option.trigger.option.height" />
            </a-form-model-item>

            <a-form-model-item v-if="button.option.trigger.type == 'chosen'" label="显示列"
              :wrapper-col="config.wrapperCol">
              <a-button @click="columnsChosen" block><a-icon type="unordered-list" />配置显示列
              </a-button>
            </a-form-model-item>

            <a-form-model-item v-if="button.option.trigger.type == 'chosen'" label="查询条件"
              :wrapper-col="config.wrapperCol">
              <a-button @click="searchChosen" block><a-icon type="search" /> 配置查询条件
              </a-button>
            </a-form-model-item>
          </a-col>
        </a-row>

      </a-form-model>

      <a-modal v-drag width="1000px" :bodyStyle="{ padding: '1px' }" :dialog-style="{ top: '20px' }"
        :title="dataSource.title" :visible="dataSource.visible" :maskClosable="false"
        @cancel="dataSource.visible = false">
        <template slot="footer">
          <a-button key="back" @click="dataSource.visible = false">关闭</a-button>
        </template>
        <a-form-model :label-col="labelCol">
          <a-form-model-item label="数据源" :wrapper-col="wrapperCol">
            <a-select allow-clear @change="dataSourceChange($event, true)"
              v-model="button.option.trigger.option.dynamicConfig.dataSourceId" style="width: 100%" placeholder="请选择">
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

          <a-form-model-item label="参数映射" :wrapper-col="wrapperCol">
            <vxe-table row-key height="350" :data="button.option.trigger.option.dynamicConfig.inParams">
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

      <a-modal :zIndex="99999" v-drag width="470px" centered :visible="condition.visible"
        :bodyStyle="{ padding: '1px' }" :destroyOnClose="true" :maskClosable="false" :title="condition.title"
        @cancel="conditionCancel" :footer="null">
        <a-spin :spinning="condition.tree.spinning">
          <a-row>
            <a-col>
              <a-card class="eip-admin-card-small" size="small">
                <a-directory-tree :expandAction="false" default-expand-all :style="condition.tree.style"
                  :tree-data="condition.tree.data" @select="onSelect"
                  v-if="condition.tree.data.length > 0"></a-directory-tree>
                <eip-empty :style="condition.tree.style" v-if="condition.tree.data.length == 0" description="无相关信息" />
              </a-card>
            </a-col>
          </a-row>
        </a-spin>
      </a-modal>

      <a-modal v-drag width="1100px" :visible="columns.visible" :bodyStyle="{ padding: '1px' }" :destroyOnClose="true"
        :maskClosable="false" title="显示需要查看列" @cancel="columns.visible = false">
        <template slot="footer">
          <a-button key="back" @click="columns.visible = false">关闭</a-button>
        </template>
        <a-row>
          <a-col>
            <a-card class="eip-admin-card-small" size="small">
              <vxe-table ref="tableColumns" id="tableColumns" size="small" :height="500"
                :data="button.option.trigger.option.columns">
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
                <vxe-column title="显示名" align="center" width="280">
                  <template v-slot="{ row }">
                    <a-space size="small">
                      <a-input allow-clear :placeholder="row.description" v-model="row.title" />
                    </a-space>
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

      <a-modal v-drag width="800px" :visible="map.visible" :bodyStyle="{ padding: '1px' }" :destroyOnClose="true"
        :maskClosable="false" title="配置数据映射" @cancel="map.visible = false">
        <template slot="footer">
          <a-button key="back" @click="map.visible = false">关闭</a-button>
        </template>
        <a-row>
          <a-col>
            <a-card class="eip-admin-card-small" size="small">
              <vxe-table ref="tableMap" id="tableMap" size="small" :height="500"
                :data="button.option.trigger.option.map">
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

      <template slot="footer">
        <a-button key="back" @click="button.visible = false">关闭</a-button>
      </template>
    </a-modal>
  </div>
</template>
<script>
import { query } from "@/services/system/datasource/list";
import searchIndex from "@/pages/system/agile/common/search/index";

import Sortable from "sortablejs";
export default {
  name: "KButton",
  components: {
    searchIndex,
    VNodes: {
      functional: true,
      render: (h, ctx) => ctx.props.vnodes,
    },
  },
  data() {
    return {
      dataSource: {
        value: [],
        outParams: [],
        title: "数据源配置",
        visible: false,
        tree: {
          visible: false,
          style: {
            overflow: "auto",
            height: "400px",
          },
          data: [],
          spinning: false,
        },
      },
      labelCol: { span: 4 },
      wrapperCol: { span: 20 },

      //数据源
      config: {
        labelCol: { span: 6 },
        wrapperCol: { span: 16 },
      },

      button: {
        title: "按钮配置",
        visible: false,

        option: {
          label: '',
          type: 'primary', //类型
          icon: 'plus', //图标
          isShow: true,//是否显示
          trigger: {
            type: 'add',
            option: {
              dynamicConfig: {
                dataSourceId: undefined, //数据源Id
                inParams: [], //输入参数赋值
                title: '',
              },
              columns: [], //显示列
              map: [],
              search: {
                labelCol: 8,
                wrapperCol: 16,
                num: 4, //显示个数
                columns: []
              }, //查询条件

              height: 600,
              width: 900,
              centered: false,
              isPaging: false,
              multiple: true
            }
          }
        }
      },


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
    }
  },
  props: {
    value: {
      type: Array,
      required: true,
    },
    list: {
      type: Array,
      required: true,
    }
  },
  created() {
    this.conditionInit();
  },
  mounted() {
    this.rowDrop();
  },
  methods: {
    /**
    * 
    */
    dataSourceChange(value, reloadMap = true) {
      var dataSource = this.dataSource.value.filter(f => f.dataSourceId == value)
      if (reloadMap) {
        this.button.option.trigger.option.dynamicConfig.inParams = [];
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
        if (this.button.option.trigger.option.dynamicConfig.inParams.length == 0) {
          this.button.option.trigger.option.dynamicConfig.inParams = [];
          config.inParams.forEach(item => {
            this.button.option.trigger.option.dynamicConfig.inParams.push({
              title: item.title,
              key: item.key,
              condition: ''
            })
          })
        }

        if (this.button.option.trigger.option.columns.length == 0) {
          config.outParams.forEach(item => {
            this.button.option.trigger.option.columns.push({
              name: item.key,
              description: item.title,
              isSearch: false,
              isShow: false,
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
     * 增加条件（可人工添加字段）
     */
    conditionAdd() {
      this.button.option.trigger.option.source.condition.push({
        op: "eq",
        pcrelation: true,
        apprelation: true,
        condition: null,
        dataType: "",
        description: "",
        name: '',
      })
    },
    /**
     * 数据源关闭
     */
    sourceCancel() {
      this.source.visible = false
    },
    /**
    * 拖拽
    */
    rowDrop() {
      let that = this;
      this.$nextTick(() => {
        const xTable = this.$refs.container;
        if (xTable) {
          this.sortable1 = Sortable.create(
            xTable.querySelector(".option-change-box-row"),
            {
              handle: ".drag-btn",
              onEnd: ({ newIndex, oldIndex }) => {
                const currRow = that.value.splice(oldIndex, 1)[0];
                that.value.splice(newIndex, 0, currRow);
              },
            }
          );
        }
      })
    },
    /**
     * 新增按钮
     */
    handleAdd() {
      const addData = [
        ...this.value,
        {
          label: "按钮" + (this.value.length + 1),
          type: 'primary', //类型
          icon: 'plus', //图标
          isShow: true,//是否显示
          trigger: {
            type: 'add',
            option: {
              dynamicConfig: {
                dataSourceId: undefined, //数据源Id
                inParams: [], //输入参数赋值
                title: '',
              },
              columns: [], //显示列
              map: [],//数据映射
              search: {
                labelCol: 8,
                wrapperCol: 16,
                num: 4, //显示个数
                columns: []
              }, //查询条件
              height: 600,
              width: 900,
              centered: false,
              isPaging: false,
              multiple: true
            }
          }
        },
      ];
      this.$emit("input", addData);
    },

    /**
     * 删除按钮
     * @param {*} deleteIndex 
     */
    handleDelete(deleteIndex) {
      // 删除
      this.$emit(
        "input",
        this.value.filter((val, index) => index !== deleteIndex)
      );
    },

    /**
     * 编辑
     * @param {} item 
     */
    handleEdit(index) {
      var data = this.value[index]
      this.button.option = data;
      this.button.visible = true;
      this.button.title = "按钮配置:" + data.label;
    },

    /**
     * 查询配置点击
     */
    searchChosen() {
      let that = this;
      this.search.columns = [];
      var columns = this.$utils.clone(this.button.option.trigger.option.columns, true).filter(
        (f) => f.isShow || f.isSearch
      );
      columns.forEach((item) => {
        that.search.columns.push({
          name: item.name,
          description: item.description,
          isShow: true,
        });
      });
      this.search.config = this.button.option.trigger.option.search;
      this.search.visible = true;
    },

    /**
     * 查询配置保存
     */
    searchOk(config) {
      this.button.option.trigger.option.search = config;
    },

    /**
     * 显示列源选择
     */
    mapChosen() {
      let that = this;
      var columns = this.$utils.clone(this.button.option.trigger.option.columns, true).filter(
        (f) => f.isShow || f.isSearch
      );
      //移除已不在里面项
      that.button.option.trigger.option.map.forEach((f, index) => {
        var have = columns.filter(c => c.name == f.name);
        if (have.length == 0) {
          that.button.option.trigger.option.map.splice(index, 1);
        }
      })
      columns.forEach((item) => {
        //判断字段是否存在
        var have = that.button.option.trigger.option.map.filter(f => f.name == item.name);
        if (have.length == 0) {
          that.button.option.trigger.option.map.push({
            name: item.name,
            description: item.description,
            to: "->",
            mapcolumn: "",
          });
        }
      });
      this.map.visible = true;
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
      query({ size: 9999 }).then((result) => {
        that.dataSource.value = result.data;
      });
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
      //必须是最后一级
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
  },
};
</script>
<style lang="less" scoped>
.option-change-container {
  width: calc(100% - 8px);
}

.option-change-box {
  height: 38px;
  padding-bottom: 6px;

  .option-delete-box {
    margin-top: 3px;
    background: #ffe9e9;
    color: #f22;
    width: 32px;
    height: 32px;
    line-height: 32px;
    text-align: center;
    border-radius: 50%;
    overflow: hidden;
    transition: all 0.3s;

    &:hover {
      background: #f22;
      color: #fff;
    }
  }

  .option-config-box {
    margin-top: 3px;
    background: @primary-color;
    color: #fff;
    width: 32px;
    height: 32px;
    line-height: 32px;
    text-align: center;
    border-radius: 50%;
    overflow: hidden;
    transition: all 0.3s;

    &:hover {
      background: @primary-color;
      color: #fff;
    }
  }
}
</style>
