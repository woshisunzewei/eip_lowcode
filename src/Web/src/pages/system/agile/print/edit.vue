<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :zIndex="1010" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="名称" prop="name">
              <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
            </a-form-model-item>
            <a-form-model-item label="数据来源" prop="dataFrom">
              <a-radio-group prop="dataFrom" @change="dataFromChange" v-model="form.dataFrom">
                <a-radio :value="0">表</a-radio>
                <a-radio :value="1">视图</a-radio>
                <a-radio :value="2">存储过程</a-radio>
                <a-radio :value="3">接口</a-radio>
              </a-radio-group>
            </a-form-model-item>
            <a-form-model-item v-if="form.dataFrom != 3" label="系统" prop="dataBaseId">
              <a-select @change="dataFromChange" allow-clear v-model="form.dataBaseId" placeholder="请选择系统">
                <a-select-option v-for="(item, index) in dataBase" :value="item.dataBaseId" :key="index">
                  {{ item.name }} </a-select-option>
              </a-select>
            </a-form-model-item>
            <a-form-model-item v-if="form.dataFrom != 3" :label="dataFrom.name" prop="dataFromName">
              <a-select allow-clear placeholder="请选择数据来源" show-search :filter-option="filterOption"
                v-model="form.dataFromName">
                <a-select-option v-for="(item, i) in dataFrom.source" :key="i" :value="item.name">
                  <label>{{ item.name }}</label>
                  {{ item.description }} <a-tag v-if="item.isFromAgile" color="red"> 敏 </a-tag>
                </a-select-option>
              </a-select>
            </a-form-model-item>

            <a-form-model-item v-if="form.dataFrom == 3" label="请求方式" prop="apiType">
              <a-select v-model="dataFrom.apiType">
                <a-select-option value="POST"> POST </a-select-option>
                <a-select-option value="GET"> GET </a-select-option>
              </a-select>
            </a-form-model-item>
            <a-form-model-item v-if="form.dataFrom == 3" label="分页" prop="apiPaging">
              <a-radio-group v-model="dataFrom.apiPaging">
                <a-radio :value="true">是</a-radio>
                <a-radio :value="false">否</a-radio>
              </a-radio-group>

            </a-form-model-item>
            <a-form-model-item v-if="form.dataFrom == 3" label="接口地址" prop="apiUrl">
              <a-input allow-clear v-model="dataFrom.apiUrl" type="textarea" placeholder="请输入接口地址" />
              <a-tag color="pink" v-if="dataFrom.apiPaging">
                默认会带上请求Token认证头及表单所有数据，统一返回数据格式为<br />{
                <br /> "data": {
                <br /> "total":0,
                <br /> "data": []
                <br /> },
                <br /> "code": 200,
                <br /> "msg": "操作成功"
                <br /> }<br />若返回代码为500则不会继续往下面执行
              </a-tag>

              <a-tag color="pink" v-if="!dataFrom.apiPaging">
                默认会带上请求Token认证头及表单所有数据，统一返回数据格式为<br />{
                <br /> "data": [],
                <br /> "code": 200,
                <br /> "msg": "操作成功"
                <br /> }<br />若返回代码为500则不会继续往下面执行
              </a-tag>
            </a-form-model-item>
            <a-form-model-item label="排序号" prop="orderNo">
              <a-input-number id="inputNumber" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
                :max="999" />
            </a-form-model-item>
            <a-form-model-item label="禁用" prop="isFreeze">
              <a-switch v-model="form.isFreeze" />
            </a-form-model-item>
            <a-form-model-item label="备注" prop="remark">
              <a-input allow-clear v-model="form.remark" type="textarea" placeholder="请输入备注" />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!configId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!configId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="configId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>

<script>

import {
  table,
  view,
  proc,
  save,
  findById,
} from "@/services/system/agile/print/edit";
import { query } from "@/services/system/database/list";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 5 },
        wrapperCol: { span: 17 },
      },
      form: {
        configId: newGuid(),
        dataFrom: 0,
        dataFromName: [],
        dataBaseId: undefined,
        name: null,
        orderNo: 1,
        isFreeze: false,
        remark: null,
        configType: 6,
      },

      rules: {

        configType: [
          {
            required: true,
            message: "请选择表单类型",
          },
        ],
        name: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
      },
      loading: false,
      spinning: false,
      save: {
        continue: false,
        retain: false,
      },

      dataFrom: {
        name: "表",
        source: [],

        apiUrl: "",
        apiType: '',//Post.GET
        apiPaging: true
      },
      dataBase: []
    };
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    configId: {
      type: String,
    },
    copy: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },
  },
  mounted() {
    this.initDataBase();
    this.find();

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
          that.form.dataBaseId = that.dataBase[0].dataBaseId;
        }
        that.find();
      });
    },
    /**
     *
     * @param {搜索} input
     * @param {*} option
     */
    filterOption(input, option) {
      return (
        option.componentOptions.children[0].text
          .toLowerCase()
          .indexOf(input.toLowerCase()) >= 0
      );
    },

    /**
     * 数据来源改变
     */
    dataFromChange(e) {
      this.form.dataFromName = [];
      this.initDataFrom();
    },

    /**
     * 初始化数据来源
     */
    initDataFrom() {
      switch (this.form.dataFrom) {
        case 0:
          this.dataFrom.name = "表";
          this.initTable();
          break;
        case 1:
          this.dataFrom.name = "视图";
          this.initView();
          break;
        case 2:
          this.dataFrom.name = "存储过程";
          this.initProc();
          break;
      }
    },

    /**
     * 初始化表
     */
    initTable() {
      let that = this;
      table({
        id: this.form.dataBaseId
      }).then((result) => {
        that.dataFrom.source = result.data;
      });
    },

    /**
     * 初始化存储过程
     */
    initProc() {
      let that = this;
      proc({
        id: this.form.dataBaseId
      }).then((result) => {
        that.dataFrom.source = result.data;
      });
    },

    /**
     * 初始化视图
     */
    initView() {
      let that = this;
      view({
        id: this.form.dataBaseId
      }).then((result) => {
        that.dataFrom.source = result.data;
      });
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
          if (that.form.configType == 1) {
            save(that.form).then(function (result) {
              if (result.code === that.eipResultCode.success) {
                that.$message.success(result.msg);
                if (that.save.continue) {
                  //不保留上次内容
                  if (!that.save.retain) {
                    that.$refs.form.resetFields();
                  }
                  that.form.configId = newGuid();
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
            save(that.form).then(function (result) {
              if (result.code === that.eipResultCode.success) {
                that.$message.success(result.msg);
                if (that.save.continue) {
                  //不保留上次内容
                  if (!that.save.retain) {
                    that.$refs.form.resetFields();
                  }
                  that.form.configId = newGuid();
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
          }
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
     * 根据Id查找
     */
    async find() {
      let that = this;
      if (this.configId) {
        that.spinning = true;
        findById(this.configId).then(function (result) {
          that.form = result.data;
          if (that.copy) {
            that.form.orderNo += 1;
          }
          if (that.form.dataFrom == 3) {
            let dataFromObj = JSON.parse(that.form.dataFromName);
            that.dataFrom.apiType = dataFromObj.type;
            that.dataFrom.apiUrl = dataFromObj.url;
            that.dataFrom.apiPaging = dataFromObj.paging;
          }

          that.initDataFrom();
          that.spinning = false;
        });
      } else {
        that.initTable();
      }

    },
  },
};
</script>