<template>
  <a-modal width="700px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">

        <a-form-model-item label="上级">
          <a-tree-select placeholder="请选择上级" allow-clear v-model="form.parentId" show-search style="width: 100%"
            treeNodeFilterProp="title" :tree-data="data" :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
            @select="treeSelect">
          </a-tree-select>
        </a-form-model-item>
        <a-form-model-item label="名称" prop="name">
          <a-input v-model="form.name" placeholder="请输入名称" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="路由" prop="router">
          <a-input v-model="form.router" placeholder="请输入路由" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="地址" prop="path">
          <a-input v-model="form.path" placeholder="请输入地址" allow-clear />
        </a-form-model-item>
        <a-form-model-item label="打开类型" prop="openType">
          <a-select allow-clear v-model="form.openType">
            <a-select-option :value="0"> 单页面 </a-select-option>
            <a-select-option :value="1"> Iframe </a-select-option>
            <a-select-option :value="2"> 新窗口 </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="图标">
          <eip-icon :name="form.icon" :theme="form.theme" @click="iconClick" @clear="iconClear"></eip-icon>
        </a-form-model-item>
        <a-form-model-item label="禁用">
          <a-switch v-model="form.isFreeze" />
        </a-form-model-item>
        <a-form-model-item label="图片" prop="image">
          <a-input v-model="form.image" placeholder="请输入图片地址" allow-clear>
            <a-avatar slot="prefix" size="small" :src="form.image" v-if="form.image" />
            <a-icon @click="material.visible = true" slot="addonAfter" type="setting" />
          </a-input>
        </a-form-model-item>
        <a-form-model-item label="参数" prop="params">
          <a-input v-model="form.params" type="textarea" placeholder="请输入参数,参数为json格式" />
        </a-form-model-item>
        <a-form-model-item label="选项">
          <a-checkbox :checked="form.isShowMenu" @change="isShowMenu">
            模块显示
          </a-checkbox>
          <a-checkbox :checked="form.haveMenuPermission" @change="haveMenuPermission">
            模块权限
          </a-checkbox>
          <a-checkbox :checked="form.haveButtonPermission" @change="haveButtonPermission">
            模块按钮权限
          </a-checkbox>
          <a-checkbox :checked="form.haveDataPermission" @change="haveDataPermission">
            数据权限
          </a-checkbox>
          <a-checkbox v-if="form.isAgileMenu" :checked="form.isShowMobile" @change="isShowMobile">
            手机端显示
          </a-checkbox>
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
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!menuId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>
        <a-space>
          <a-button v-if="!menuId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="menuId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
    <eip-material v-if="material.visible" :visible.sync="material.visible" @ok="materialOk" />
  </a-modal>
</template>

<script>
import { save, findById } from "@/services/system/menu/edit";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  data() {
    return {
      material: {
        visible: false
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        menuId: newGuid(),
        parentId: this.parentId,
        parentName: this.parentName,
        name: null,
        icon: null,
        theme: null,
        openType: 0,
        path: null,
        router: null,
        isShowMenu: false,
        isAgileMenu: false,
        isShowMobile: false,
        haveMenuPermission: false,
        haveButtonPermission: false,
        haveDataPermission: false,
        haveFieldPermission: false,
        isFreeze: false,
        orderNo: 1,
        params: null,
        remark: null,
        image: null
      },
      rules: {
        name: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],
        params: [
          {
            validator: (rule, value, callback) => {
              if (
                typeof value === "undefined" ||
                value === "" ||
                value === null
              ) {
                callback();
              } else {
                try {
                  var obj = JSON.parse(value);
                  if (typeof obj == "object" && obj) {
                    callback();
                  } else {
                    callback(new Error("请输入正确参数格式,参数为json"));
                  }
                } catch (e) {
                  callback(new Error("请输入正确参数格式,参数为json"));
                }
              }
            },
            trigger: "blur",
          },
        ],
        router: [
          {
            required: true,
            message: "请输入路由",
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
    this.find();
  },

  methods: {
    /**
    * 确认素材
    */
    materialOk(path) {
      this.form.image = path;
    },
    /**
     *
     */
    isShowMenu(e) {
      this.form.isShowMenu = e.target.checked;
    },
    /**
     *
     */
    haveMenuPermission(e) {
      this.form.haveMenuPermission = e.target.checked;
    },
    /**
     *
     */
    haveButtonPermission(e) {
      this.form.haveButtonPermission = e.target.checked;
    },
    /**
     *
     */
    haveDataPermission(e) {
      this.form.haveDataPermission = e.target.checked;
    },
    /**
    *
    */
    isShowMobile(e) {
      this.form.isShowMobile = e.target.checked;
    },
    /**
     *
     */
    haveFieldPermission(e) {
      this.form.haveFieldPermission = e.target.checked;
    },

    /**
     * 树选择
     */
    treeSelect(electedKeys, e) {
      this.form.parentName = e.title;
      this.form.parentId = e.eventKey;
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
          that.form.menuId = that.copy ? newGuid() : that.form.menuId;
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.menuId = newGuid();
              } else {
                that.cancel();
              }
              that.$emit("save", result);
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
     * 根据Id查找
     */
    find() {
      let that = this;
      if (this.menuId) {
        that.spinning = true;
        findById(this.menuId).then(function (result) {
          if (that.copy) {
            result.data.orderNo += 1;
          }
          let form = result.data;
          that.form = form;
          that.form.parentId =
            form.parentName == "" ? undefined : form.parentId;
          that.spinning = false;
        });
      }
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