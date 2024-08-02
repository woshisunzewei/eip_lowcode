<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">

        <a-form-model-item label="名称" prop="name">
          <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="系统" prop="dataBaseId">
          <a-select allow-clear v-model="form.dataBaseId" placeholder="请选择系统"> <a-select-option
              v-for="(item, index) in dataBase" :value="item.dataBaseId" :key="index">
              {{ item.name }} </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="表名" prop="dataFromName" help="编辑不可调整">
          <a-input v-model="form.dataFromName" placeholder="请输入表名" allow-clear>

            <a-tooltip slot="addonAfter" title="点击生成跳转翻译">
              <a :href="'https://fanyi.baidu.com/translate?query=' + form.name + '&lang=auto2zh#zh/en/' + form.name"
                target="_blank">翻译</a>
            </a-tooltip>
          </a-input>
        </a-form-model-item>

        <a-form-model-item label="图标" prop="icon">
          <eip-icon :name="form.icon" :theme="form.theme" @click="iconClick" @clear="iconClear"></eip-icon>
        </a-form-model-item>
        <a-form-model-item label="图片" prop="image">
          <a-input v-model="form.image" placeholder="请输入图片地址" allow-clear>
            <a-avatar slot="prefix" size="small" :src="form.image" v-if="form.image" />
            <a-icon @click="material.visible = true" slot="addonAfter" type="setting" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="打开类型" prop="openType">
          <a-select allow-clear v-model="form.openType">
            <a-select-option :value="0"> 当前页 </a-select-option>
            <a-select-option :value="2"> 新窗口 </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="禁用">
          <a-switch v-model="form.isFreeze" />
        </a-form-model-item>

        <a-form-model-item label="手机端显示">
          <a-switch v-model="form.isShowMobile" />
        </a-form-model-item>

        <a-form-model-item label="排序号" prop="orderNo">
          <a-input-number id="inputNumber" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
            :max="999" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="remark">
          <a-input v-model="form.remark" type="textarea" placeholder="请输入备注" />
        </a-form-model-item>
      </a-form-model>
    </a-spin>

    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>

    <eip-material v-if="material.visible" :visible.sync="material.visible" @ok="materialOk" />

  </a-modal>
</template>

<script>
import {
  table,
  agileSave,
  agileSaveType,
  save,
  findById,
  listPublic,
  findByMenuId,
} from "@/services/system/agile/app/worksheet";
import { query } from "@/services/system/database/list";
import { newGuid } from "@/utils/util";
import jsonFormat from "@/pages/system/agile/form/components/packages/components/KFormDesign/config/jsonFormat";
export default {
  name: "worksheet",
  data() {
    return {
      material: {
        visible: false
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      type: [],
      form: {
        menuId: newGuid(),
        parentId: this.parentId,
        parentName: this.parentName,
        name: null,
        icon: null,
        image: null,
        theme: null,
        openType: 0,
        path: null,
        router: "agileappbuild",
        isShowMenu: true,
        isShowMobile: true,
        haveMenuPermission: true,
        haveButtonPermission: true,
        haveDataPermission: false,
        haveFieldPermission: true,
        isFreeze: false,
        orderNo: 1,
        params: null,
        remark: null,
        isAgileMenu: true,
        agileMenuType: this.eipAgileMenuType.worksheet,
        dataFromName: '',
        dataBaseId: undefined,
      },
      rules: {
        icon: [
          {
            required: true,
            message: "请选择图标",
            trigger: "blur",
          },
        ],

        name: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          }
        ],
        dataBaseId: [
          {
            required: true,
            message: "请选择数据库",
            trigger: "blur",
          }
        ],
        dataFromName: [
          {
            required: true,
            message: "请输入表名",
            trigger: "blur",
          }
        ],
      },

      loading: false,
      spinning: false,
      agileSaveForm: {
        configId: undefined,
        name: undefined,
        dataBaseId: undefined,
        orderNo: undefined,
        remark: undefined,
      },
      listForm: {},
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
    title: {
      type: String,
    },
    copy: {
      type: Boolean,
      default: false,
    },
    menuId: {
      type: String,
    },
    parentId: {
      type: String,
    },
    parentName: {
      type: String,
    },
  },

  mounted() {
    this.initDataBase();
  },

  methods: {
    /**
     * 初始化数据库
     */
    initDataBase() {
      let that = this;
      that.spinning = true;
      query().then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.dataBase = result.data;
        }
        that.find();
      });
    },
    /**
  * 确认素材
  */
    materialOk(path) {
      this.form.image = path;
    },

    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
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
     * 生成拼音
     */
    dataFromNameSet() {
      if (!this.menuId) {
        this.form.dataFromName = pinyin.convert(this.form.name);
      }
    },

    /**
     * 保存
     */
    async save() {
      let that = this;
      this.$refs.form.validate(async (valid) => {
        if (valid) {
          //判断分类
          that.loading = true;
          that.$loading.show({ text: that.eipMsg.saveLoading });
          that.form.menuId = that.copy ? newGuid() : that.form.menuId;
          //生成path

          if (that.menuId) {
            var result = await save(that.form);
            if (result.code !== that.eipResultCode.success) {
              that.$message.error(result.msg);
              return false;
            }

            that.agileSaveForm.name = that.form.name;
            that.agileSaveForm.orderNo = that.form.orderNo;
            that.agileSaveForm.remark = that.form.remark ? that.form.remark : that.form.name;
            that.agileSaveForm.dataBaseId = that.form.dataBaseId;
            result = await agileSaveType(that.agileSaveForm);
            if (result.code == that.eipResultCode.success) {

              that.agileSaveForm.dataFromName = that.form.dataFromName;
              result = await table(that.agileSaveForm);
              if (result.code != that.eipResultCode.success) {
                that.$message.error(result.msg);
                that.loading = false;
                that.$loading.hide();
              }

              that.$message.success(result.msg);
              that.cancel();
              that.$emit("save", result);
            }
            that.loading = false;
            that.$loading.hide();
          } else {
            var formSaveJson = jsonFormat
            that.agileSaveForm = {
              configId: newGuid(),
              dataFromName: that.form.dataFromName,
              configType: 2,
              dataFrom: 0,
              name: that.form.name,
              orderNo: 1,
              isFreeze: false,
              remark: that.form.remark ? that.form.remark : that.form.name,
              formCategory: 1,
              menuId: that.form.menuId,
              saveJson: formSaveJson,
              publicJson: formSaveJson,
              columnJson: JSON.stringify([]),
              dataBaseId: that.form.dataBaseId
            };
            result = await agileSave(that.agileSaveForm);
            if (result.code != that.eipResultCode.success) {
              that.$message.error(result.msg);
              that.loading = false;
              that.$loading.hide();
            }

            result = await table(that.agileSaveForm);
            if (result.code != that.eipResultCode.success) {
              that.$message.error(result.msg);
              that.loading = false;
              that.$loading.hide();
            }

            result = await that.saveList();
            if (result.code != that.eipResultCode.success) {
              that.$message.error(result.msg);
              that.loading = false;
              that.$loading.hide();
            }

            result = await that.saveForm();
            if (result.code != that.eipResultCode.success) {
              that.$message.error(result.msg);
              that.loading = false;
              that.$loading.hide();
            }

            result = await that.jsonPublic();
          }
        }
      });
    },

    /**
     * 列表配置
     */
    async saveList() {
      let that = this;
      var listJson = this.$utils.clone(this.eipTableConfig, true);
      that.listForm = {
        configId: newGuid(),
        dataFrom: 0,
        dataFromName: that.form.dataFromName,
        dataBaseId: that.form.dataBaseId,
        name: that.form.name,
        orderNo: 1,
        configType: 1,
        isFreeze: false,
        remark: that.form.name,
        editConfigId: that.agileSaveForm.configId,
        saveJson: JSON.stringify(listJson),
        publicJson: JSON.stringify(listJson),
        menuId: that.form.menuId,
      };
      return await agileSave(that.listForm);
    },

    /**
     * 保存菜单
     */
    async saveForm() {
      let that = this;
      that.form.path = "/appbuild/" + that.form.menuId;
      return await save(that.form);
    },

    /**
     * 发布
     */
    async jsonPublic() {
      let that = this;
      var result = await listPublic(that.listForm);

      if (result.code != that.eipResultCode.success) {
        that.$message.error(result.msg);
        that.loading = false;
        that.$loading.hide();
      }
      result = await listPublic(that.agileSaveForm);

      if (result.code != that.eipResultCode.success) {
        that.$message.error(result.msg);
        that.loading = false;
        that.$loading.hide();
      } else {
        that.$message.success(result.msg);
        that.cancel();
        that.$emit("save", result);
      }
      that.loading = false;
      that.$loading.hide();
    },

    /**
     * 根据Id查找
     */
    async find() {
      let that = this;

      if (this.menuId) {
        var result = await findById(this.menuId);
        if (that.copy) {
          result.data.orderNo += 1;
        }
        let form = result.data;
        form.parentId = form.parentName == "" ? undefined : form.parentId;
        result = await findByMenuId({ menuId: this.menuId, configType: 2 });
        if (result.code === this.eipResultCode.success) {
          var agileData = result.data[0];
          form.dataFromName = agileData.dataFromName
          form.dataBaseId = agileData.dataBaseId
          that.agileSaveForm = {
            configId: agileData.configId,
            name: agileData.name,
            orderNo: form.orderNo,
            remark: form.remark,
            dataBaseId: agileData.dataBaseId,
          };
        }
        that.form = form;
      }
      that.spinning = false;
    },

    /**
     *图标点击
     */
    iconClick(icon) {
      this.form.icon = icon.name;
      this.form.theme = icon.theme;
    },

    /**
     * 清空图标
     */
    iconClear() {
      this.form.icon = null;
      this.form.theme = null;
    },
  },
};
</script>