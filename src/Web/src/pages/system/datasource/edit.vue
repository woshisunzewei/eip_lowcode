<template>
  <a-drawer width="800px" :destroyOnClose="true" :visible="visible" :bodyStyle="{ padding: '1px' }" :title="title"
    @close="cancel">
    <div class="eip-drawer-body beauty-scroll">
      <a-spin :spinning="spinning">
        <a-form-model :model="form" :rules="rules" ref="form" :label-col="col.labelCol" :wrapper-col="col.wrapperCol">
          <a-form-model-item label="名称" prop="name">
            <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
          </a-form-model-item>

          <a-form-model-item label="数据源">
            <a-radio-group v-model="source.type" @change="change">
              <a-radio value="table">表</a-radio>
              <a-radio value="view">视图</a-radio>
              <a-radio value="proc">存储过程</a-radio>
              <a-radio value="interface">接口</a-radio>
              <a-radio value="excel">excel</a-radio>
              <a-radio value="sql">sql</a-radio>
              <a-radio value="custom">自定义</a-radio>
            </a-radio-group>
          </a-form-model-item>
          <a-form-model-item v-if="source.type == 'table' || source.type == 'view' || source.type == 'proc'" label="系统"
            prop="dataBaseId">
            <a-select @change="change" allow-clear v-model="source.dataBaseId" placeholder="请选择系统">
              <a-select-option v-for="(item, index) in dataBase" :value="item.dataBaseId" :key="index">
                {{ item.name }} </a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="表" v-if="source.type == 'table'">
            <a-select placeholder="请选择表" allow-clear show-search v-model="source.table.name" @change="tableChange">
              <a-select-option v-for="(item, i) in tables" :key="i" :value="item.name"><label>{{ item.name }}</label> {{
                item.description }} <a-tag v-if="item.isFromAgile" color="red"> 敏 </a-tag></a-select-option>
            </a-select>
          </a-form-model-item>

          <a-form-model-item label="视图" v-if="source.type == 'view'">
            <a-select placeholder="请选择类型" allow-clear show-search v-model="source.view.name" @change="viewChange">
              <a-select-option v-for="(item, i) in views" :key="i" :value="item.name">{{ item.name }} {{
                item.description
              }}</a-select-option>
            </a-select>
          </a-form-model-item>

          <a-form-model-item label="存储过程" v-if="source.type == 'proc'">
            <a-select placeholder="请选择类型" allow-clear show-search v-model="source.proc.name">
              <a-select-option v-for="(item, i) in procs" :key="i" :value="item.name">{{ item.name }} {{
                item.description
              }}</a-select-option>
            </a-select>
          </a-form-model-item>

          <a-form-model-item label="请求类型" v-if="source.type == 'interface'">
            <a-select placeholder="请选择类型" v-model="source.interface.type">
              <a-select-option value="POST">POST</a-select-option>
              <a-select-option value="GET">GET</a-select-option>
            </a-select>
          </a-form-model-item>
          <a-form-model-item label="请求地址" v-if="source.type == 'interface'">
            <a-input v-model="source.interface.url" type="textarea" placeholder="请输入请求地址" />
          </a-form-model-item>
          <a-form-model-item label="输入参数" v-if="source.type != 'custom'">
            <vxe-table row-key ref="inParams" height="200" :data="source.inParams">
              <template #loading>
                <a-spin></a-spin>
              </template>
              <template #empty>
                <eip-empty />
              </template>
              <vxe-column type="seq" title="序号" width="60"></vxe-column>
              <vxe-column title="新增" width="54px" align="center">
                <template #header>
                  <a-button @click="addInParams" size="small" type="primary">
                    <a-icon type="plus" />
                  </a-button>
                </template>
                <template v-slot="{ row }">
                  <a-popconfirm title="确定删除?" ok-text="确定" cancel-text="取消" @confirm="delInParams(row)">
                    <a-button @click.stop="" size="small" type="danger">
                      <a-icon type="delete"></a-icon>
                    </a-button>
                  </a-popconfirm>
                </template>
              </vxe-column>

              <vxe-column title="字段名称" width="240px">
                <template v-slot="{ row }">
                  <a-auto-complete @select="paramsChange(row, $event)" v-model="row.field" :allowClear="true"
                    :filter-option="filterOption" :dropdown-match-select-width="true" placeholder="请选择" size="small"
                    option-label-prop="value">
                    <template slot="dataSource">
                      <a-select-option v-for="(oitem, oindex) in source.columns" :value="oitem.field" :key="oindex">
                        {{ oitem.title ? oitem.title + "【" + oitem.field + "】" : oitem.field }}
                      </a-select-option>
                    </template>
                  </a-auto-complete>
                </template>
              </vxe-column>

              <vxe-column title="类型" width="170px" align="center">
                <template v-slot="{ row }">
                  <a-select v-model="row.dataType" size="small" style="width: 100%" placeholder="请选择">
                    <a-select-option value="varchar"> 字符串 </a-select-option>
                    <a-select-option value="bit"> 布尔 </a-select-option>
                    <a-select-option value="datetime"> 时间 </a-select-option>
                    <a-select-option value="int"> 数值 </a-select-option>
                  </a-select>
                </template>
              </vxe-column>

              <vxe-column title="备注" width="120px">
                <template v-slot="{ row }">
                  <a-input size="small" allow-clear placeholder="备注" v-model="row.title" />
                </template>
              </vxe-column>
            </vxe-table>
          </a-form-model-item>
          <a-form-model-item label="输出参数" v-if="source.type != 'custom'">

            <vxe-table row-key ref="outParams" height="200" :data="source.outParams">
              <template #loading>
                <a-spin></a-spin>
              </template>
              <template #empty>
                <eip-empty />
              </template>
              <vxe-column type="seq" title="序号" width="60"></vxe-column>
              <vxe-column title="新增" width="54px" align="center">
                <template #header>
                  <a-button @click="addOutParams" size="small" type="primary">
                    <a-icon type="plus" />
                  </a-button>
                </template>
                <template v-slot="{ row }">
                  <a-popconfirm title="确定删除?" ok-text="确定" cancel-text="取消" @confirm="delOutParams(row)">
                    <a-button @click.stop="" size="small" type="danger">
                      <a-icon type="delete"></a-icon>
                    </a-button>
                  </a-popconfirm>
                </template>
              </vxe-column>

              <vxe-column width="240px">
                <template #header>
                  字段名称
                  <a-button @click="allOutParams" size="small" type="primary">
                    生成所有字段
                  </a-button>
                </template>
                <template v-slot="{ row }">
                  <a-auto-complete @select="paramsChange(row, $event)" v-model="row.field" :allowClear="true"
                    :filter-option="filterOption" :dropdown-match-select-width="true" placeholder="请选择" size="small"
                    option-label-prop="value">
                    <template slot="dataSource">
                      <a-select-option v-for="(oitem, oindex) in source.columns" :value="oitem.field" :key="oindex">
                        {{ oitem.title ? oitem.title + "【" + oitem.field + "】" : oitem.field }}
                      </a-select-option>
                    </template>
                  </a-auto-complete>
                </template>
              </vxe-column>

              <vxe-column title="字段别名" width="170px">
                <template v-slot="{ row }">
                  <a-input size="small" allow-clear placeholder="字段别名" v-model="row.alias" />
                </template>
              </vxe-column>

              <vxe-column title="备注" width="120px">
                <template v-slot="{ row }">
                  <a-input size="small" allow-clear placeholder="备注" v-model="row.title" />
                </template>
              </vxe-column>

            </vxe-table>
          </a-form-model-item>
          <a-form-model-item label="条件" v-if="source.type != 'custom'">
            <eip-rule :filters="source.filters" :columns="source.inParams" :dataColumns="source.inParams"></eip-rule>
          </a-form-model-item>

          <a-form-model-item label="自定义数据" v-if="source.type == 'custom'">
            <a-input v-model="source.custom" type="textarea" placeholder="请输入自定义数据" />
          </a-form-model-item>

          <a-form-model-item label="数据权限" help="根据配置数据权限动态获取数据">
            <a-tree-select placeholder="请选择归属模块" allow-clear show-search style="width: 100%" treeNodeFilterProp="title"
              :tree-data="menu.data" v-model="source.dataMenuId"
              :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }">
            </a-tree-select>
          </a-form-model-item>

          <a-form-model-item label="选项">
            <a-checkbox>无条件不加载数据</a-checkbox>
          </a-form-model-item>
          <a-form-model-item label="备注" prop="remark">
            <a-input v-model="form.remark" type="textarea" placeholder="请输入备注" />
          </a-form-model-item>

          <a-form-model-item label="禁用">
            <a-switch v-model="form.isFreeze" />
          </a-form-model-item>

          <a-form-model-item label="排序号" prop="orderNo">
            <a-input-number id="inputNumber" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
              :max="999" />
          </a-form-model-item>
        </a-form-model>
      </a-spin>
    </div>
    <div class="eip-drawer-toolbar">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!dataSourceId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>
        <a-space>
          <a-button v-if="!dataSourceId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="dataSourceId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </div>
  </a-drawer>
</template>

<script>
import {
  table,
  view,
  proc,
  column,
} from "@/services/system/agile/common/data/source";
import { save, findById } from "@/services/system/datasource/edit";
import { query } from "@/services/system/database/list";
import { newGuid } from "@/utils/util";
import { menuQuery } from "@/services/system/data/list";
export default {
  name: "data-source",
  data() {
    return {
      menu: {
        data: [],
      },
      save: {
        continue: false,
        retain: false,
      },
      loading: false,
      spinning: false,
      form: {
        dataSourceId: newGuid(),
        name: "",
        isFreeze: false,
        orderNo: 1,
        remark: null,
        config: null
      },

      col: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },

      style: {
        zIndex: 1030,
      },

      filterType: [
        { value: "cn", title: "包含" },
        { value: "eq", title: "等于" },
        { value: "ne", title: "不等" },
        { value: "bw", title: "开始于" },
        { value: "bn", title: "不开始于" },
        { value: "ew", title: "结束于" },
        { value: "en", title: "不结束于" },
        { value: "nc", title: "不包含" },
        { value: "nu", title: "空值" },
        { value: "nn", title: "非空值" },
        { value: "date", title: "等于" },
        { value: "daterange", title: "之间" },
        { value: "lt", title: "小于" },
        { value: "le", title: "小于等于" },
        { value: "gt", title: "大于" },
        { value: "ge", title: "大于等于" },
      ],

      rules: {
        name: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
      },

      source: {
        type: "table",
        dataBaseId: undefined,
        table: {
          loading: false,
          name: undefined,
        },
        proc: {
          name: null,
        },
        view: {
          loading: false,
          name: null,
        },
        interface: {
          type: "POST",
          url: null,
        },
        custom: null,
        columns: [],
        inParams: [],//输入参数
        outParams: [],//输出参数
        dataMenuId: undefined,//数据权限
        noRuleGetData: false,//无条件不加载数据

        filters: {
          groupOp: 'AND',
          rules: [],
          groups: []
        }
      },

      tables: [], //所有列表下拉数据
      views: [], //所有视图下拉
      procs: [], //所有存储过程下拉

      dataBase: []
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
    dataSourceId: {
      type: String,
    },

    copy: {
      type: Boolean,
      default: false,
    },
  },

  mounted() {
    this.initDataBase();
    this.menuInit();
  },

  methods: {
    /**
    * 初始化数据库
    */
    initDataBase() {
      let that = this;
      query().then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.dataBase = result.data;
          that.source.dataBaseId = that.dataBase[0].dataBaseId;
        }
        that.find();
      });
    },
    /**
     * 菜单树
     */
    menuInit() {
      let that = this;
      menuQuery(that.eipPrivilegeAccess.data).then((result) => {
        that.menu.data = result.data;
      });
    },

    /**
     * 搜索
     * @param {*} input 
     * @param {*} option 
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text.toUpperCase().indexOf(input.toUpperCase()) >= 0
      );
    },

    paramsChange(row, event) {
      row.field = event;
      row.alias = event;
      if (this.source.type == 'table') {
        var data = this.$utils.find(this.source.columns, f => f.field == event);
        row.title = data.title;
      }
    },

    addOutParams() {
      this.source.outParams.push({
        title: undefined,
        field: undefined,
        alias: undefined,
        type: 'select',
        dataType: 'varchar'
      });
    },
    addInParams() {
      this.source.inParams.push({
        title: undefined,
        field: undefined,
        type: 'select',
        dataType: 'varchar'
      });
    },

    allOutParams() {
      if (this.source.type == 'table') {
        this.spinning = true;
        this.source.outParams = [];
        this.source.columns.forEach(item => {
          this.source.outParams.push({
            title: item.title,
            field: item.key,
            alias: item.key,
            dataType: item.dataType,
          });
        })
        this.spinning = false;
      }
    },

    delOutParams(row) {
      let spIndex = -1;
      this.source.outParams.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.source.outParams.splice(spIndex, 1);
      }
    },

    delInParams(row) {
      let spIndex = -1;
      this.source.inParams.forEach((item, index) => {
        if (item._X_ROW_KEY == row._X_ROW_KEY) {
          spIndex = index;
        }
      });
      if (spIndex != -1) {
        this.source.inParams.splice(spIndex, 1);
      }
    },
    /**
    * 根据Id查找
    */
    find() {
      let that = this;
      if (this.dataSourceId) {
        that.spinning = true;
        findById(this.dataSourceId).then(function (result) {
          if (that.copy) {
            result.data.orderNo += 1;
          }
          that.form.name = result.data.name;
          that.form.remark = result.data.remark;
          that.form.orderNo = result.data.orderNo;
          that.form.isFreeze = result.data.isFreeze;
          that.form.dataSourceId = result.data.dataSourceId;
          if (result.data.config) {
            that.init(JSON.parse(result.data.config))
          } else {
            that.init();
          }
          that.spinning = false;
        });
      } else {
        that.init();
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
    saveConfirm() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          that.form.dataSourceId = that.copy
            ? newGuid()
            : that.form.dataSourceId;
          save({
            dataSourceId: that.form.dataSourceId,
            name: that.form.name,
            isFreeze: that.form.isFreeze,
            orderNo: that.form.orderNo,
            remark: that.form.remark,
            type: that.source.type,
            dataBaseId: that.source.dataBaseId,
            config: JSON.stringify(that.source)
          }).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.dataSourceId = newGuid();
              } else {
                that.cancel();
              }
              that.$emit("ok", result);
            } else {
              that.$message.error(result.msg);
            }
            that.loading = false;
            that.$loading.hide();
          });
        } else {
          return false;
        }
      });
    },
    /**
     *
     */
    saveContinue() {
      this.save.continue = true;
      this.saveConfirm();
    },

    /**
     *
     */
    saveClose() {
      this.save.continue = false;
      this.saveConfirm();
    },

    /**
     * 类型改变
     */
    change() {
      switch (this.source.type) {
        case "table":
          this.initTable();
          break;
        case "proc":
          this.initProc();
          break;
        case "view":
          this.initView();
          break;
      }
    },

    /**
     * 初始化表
     */
    initTable() {
      let that = this;
      table({
        id: that.source.dataBaseId
      }).then((result) => {
        that.tables = result.data;
      });
    },

    /**
     * 初始化存储过程
     */
    initProc() {
      let that = this;
      proc({
        id: that.source.dataBaseId
      }).then((result) => {
        that.procs = result.data;
      });
    },

    /**
     * 初始化视图
     */
    initView() {
      let that = this;
      view({
        dataBaseId: that.source.dataBaseId,
        id: that.source.dataBaseId
      }).then((result) => {
        that.views = result.data;
      });
    },

    /**
     * 表切换
     */
    tableChange() {
      let that = this;
      that.source.table.loading = true;
      this.source.inParams = [];
      this.source.outParams = [];
      this.source.filters = {
        groupOp: 'AND',
        rules: [],
        groups: []
      }
      column({
        dataBaseId: that.source.dataBaseId,
        name: that.source.table.name,
      }).then((result) => {
        that.source.columns = this.getColumns(result.data);
        that.source.table.loading = false;
      });
    },

    /**
     * 视图切换
     */
    viewChange() {
      let that = this;
      that.source.view.loading = true;
      column({
        dataBaseId: that.source.dataBaseId,
        name: that.source.view.name,
      }).then((result) => {
        that.source.columns = this.getColumns(result.data);
        that.source.view.loading = false;
      });
    },

    /**
     *获取列配置
     */
    getColumns(data) {
      let columns = [];
      data.forEach((element) => {
        var dataType = 'varchar';
        switch (element.dataType) {
          case "datetime":
            dataType = 'datetime';
            break;
          case "int":
          case "decimal":
            dataType = 'int';
            break;
          case "bit":
            dataType = 'bit';
            break;
        }
        let column = {
          field: element.name,
          title: element.description,
          dataType: dataType,
        };
        columns.push(column);
      });
      return columns;
    },

    /**
     * 初始化
     */
    init(config) {
      if (config) {
        this.source = config;
        this.change();
      } else {
        this.initTable();
      }
    },
  },
};
</script>