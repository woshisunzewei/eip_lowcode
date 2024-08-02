<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
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
              <a-select optionFilterProp="label" allow-clear placeholder="请选择数据来源" show-search
                v-model="form.dataFromName">
                <a-select-option :label="item.name + item.description" v-for="(item, i) in dataFrom.source" :key="i"
                  :value="item.name">
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
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button @click="cancel"><a-icon type="close" />关闭</a-button>
        <a-button key="submit" @click="saveConfirm" type="primary" :loading="loading"><a-icon
            type="save" />提交</a-button>
      </a-space>
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
} from "@/services/system/agile/list/edit";
import { query } from "@/services/system/database/list";
import { newGuid } from "@/utils/util";
import { mapMutations, mapGetters } from "vuex";
export default {
  name: "edit",
  data() {
    return {
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        configId: newGuid(),
        dataFrom: 0,
        dataFromName: [],
        dataBaseId: undefined,
        name: null,
        orderNo: 1,
        configType: 1,
        type: undefined,
        isFreeze: false,
        remark: null,
        editConfigId: undefined,
        cardConfigId: undefined,
      },
      dataFrom: {
        name: "表",
        source: [],

        apiUrl: "",
        apiType: '',//Post.GET
        apiPaging: true
      },
      rules: {
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

      dataBase: []
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
  },
  computed: {
    ...mapGetters("account", ["systemAgile"]),
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
    ...mapMutations("account", ["setSystemAgile"]),
    /**
     * 保存
     */
    saveConfirm() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          that.form.configId = that.copy ? newGuid() : that.form.configId;

          if (that.form.dataFrom == 3) {
            that.form.dataFromName = JSON.stringify({
              type: that.dataFrom.apiType,
              url: that.dataFrom.apiUrl,
              paging: that.dataFrom.apiPaging
            });
          }
          table(that.form).then((result) => {
            if (result.code === that.eipResultCode.success) {
              save(that.form).then(function (result) {
                if (result.code === that.eipResultCode.success) {

                  var systemAgileData = that.systemAgile.filter(f => f.configId == that.form.configId);
                  if (systemAgileData && systemAgileData.length > 0) {
                    systemAgileData[0].columnJson = null;
                    systemAgileData[0].publicJson = null;
                    that.setSystemAgile(that.systemAgile);
                  }

                  that.$message.success(result.msg);
                  that.$emit("ok", result);
                  that.cancel();
                } else {
                  that.$message.error(result.msg);
                }
                that.loading = false;
                that.$loading.hide();
              });
            } else {
              that.$message.error(result.msg);
              that.loading = false;
              that.$loading.hide();
            }
          });
        } else {
          return false;
        }
      });
    },

    /**
     * 根据Id查找
     */
    find() {
      let that = this;
      if (this.configId) {
        that.spinning = true;
        findById(this.configId).then(function (result) {
          let form = result.data;
          that.form.configId = form.configId;
          that.form.dataFromName = form.dataFromName;
          that.form.dataFrom = form.dataFrom;
          that.form.name = form.name;
          that.form.orderNo = form.orderNo;
          that.form.dataBaseId = form.dataBaseId == that.eipEmptyGuid ? undefined : form.dataBaseId;
          if (that.copy) {
            that.form.orderNo += 1;
          }
          that.form.isFreeze = form.isFreeze;
          that.form.remark = form.remark;
          that.form.type = form.type;

          if (form.dataFrom == 3) {
            let dataFromObj = JSON.parse(form.dataFromName);
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