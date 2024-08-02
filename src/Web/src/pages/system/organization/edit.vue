<template>
  <a-modal
    width="600px"
    v-drag
    :destroyOnClose="true"
    :maskClosable="false"
    :visible="visible"
    :bodyStyle="{ padding: '1px' }"
    :title="title"
    @cancel="cancel"
  >
    <a-spin :spinning="spinning">
      <a-form-model
        ref="form"
        :model="form"
        :rules="rules"
        :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol"
      >
        <a-form-model-item label="上级" prop="parentId">
          <a-tree-select
            placeholder="请选择上级"
            allow-clear
            v-model="form.parentId"
            show-search
            style="width: 100%"
            treeNodeFilterProp="title"
            :tree-data="organization.data"
            :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
            @select="organizationSelect"
          >
          </a-tree-select>
        </a-form-model-item>
        <a-form-model-item label="名称" prop="name">
          <a-input v-model="form.name" placeholder="请输入名称" />
        </a-form-model-item>
        <a-form-model-item label="简称" prop="shortName">
          <a-input v-model="form.shortName" placeholder="请输入简称" />
        </a-form-model-item>
        <a-form-model-item label="位置" prop="shortName">
          <a-button type="primary" @click="showAMap" icon="environment"
            >打开地图选择</a-button
          >
          <br />
          <a-tag color="blue"> 经度：{{ form.longitude }} </a-tag>
          <a-tag color="purple"> 纬度：{{ form.latitude }} </a-tag>
        </a-form-model-item>
        <a-form-model-item label="类型" prop="nature">
          <a-select default-value="0" v-model="form.nature">
            <a-select-option
              v-for="(item, index) in eipOrganizationNatures"
              :key="index"
              :value="item.value"
            >
              {{ item.name }}
            </a-select-option>
          </a-select>
        </a-form-model-item>
        <a-form-model-item label="负责人" prop="mainSupervisor">
          <a-input v-model="form.mainSupervisor" placeholder="请输入负责人" />
        </a-form-model-item>
        <a-form-model-item label="联系方式" prop="mainSupervisorContact">
          <a-input
            v-model="form.mainSupervisorContact"
            placeholder="请输入负责人联系方式"
          />
        </a-form-model-item>
        <a-form-model-item label="排序号" prop="orderNo">
          <a-input-number
            id="inputNumber"
            style="width: 100%"
            placeholder="请输入排序号"
            v-model="form.orderNo"
            :min="0"
            :max="999"
          />
        </a-form-model-item>
        <a-form-model-item label="禁用" prop="isFreeze">
          <a-switch v-model="form.isFreeze" />
        </a-form-model-item>
        <a-form-model-item label="备注" prop="remark">
          <a-input
            v-model="form.remark"
            type="textarea"
            placeholder="请输入备注"
          />
        </a-form-model-item>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!organizationId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button
            v-if="!organizationId"
            key="submit"
            @click="saveContinue"
            :loading="loading"
            ><a-icon type="save" />提交并继续创建</a-button
          >
          <a-button v-if="organizationId" @click="cancel"
            ><a-icon type="close" />关闭</a-button
          >
          <a-button
            key="submit"
            @click="saveClose"
            type="primary"
            :loading="loading"
            ><a-icon type="save" />提交</a-button
          >
        </a-space>
      </div>
    </template>
    <amap
      v-if="showMap"
      :lng="form.longitude"
      :lat="form.latitude"
      @cancel="showMap = false"
      @map-confirm="confirmLocation"
      :visible="showMap"
    ></amap>
  </a-modal>
</template>

<script>
import {
  save,
  findById,
  organizationRemoveChildren,
} from "@/services/system/organization/edit";
import { newGuid } from "@/utils/util";
import amap from "./map";
export default {
  components: { amap },
  name: "edit",
  data() {
    return {
      showMap: false,
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        organizationId: newGuid(),
        parentId: this.parentId,
        parentName: this.parentName,
        name: null,
        nature: 0,
        shortName: null,
        mainSupervisor: null,
        mainSupervisorContact: null,
        orderNo: 1,
        isFreeze: false,
        remark: null,
        latitude: null,
        longitude: null,
      },
      organization: { data: [] },
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
    };
  },

  mounted() {
    this.treeInit();
    this.find();
  },

  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    title: {
      type: String,
    },
    organizationId: {
      type: String,
    },
    parentId: {
      type: String,
    },
    parentName: {
      type: String,
    },
  },

  methods: {
    confirmLocation(e) {
      this.form.latitude = e.lat;
      this.form.longitude = e.lng;
      this.showMap = false;
    },
    showAMap() {
      this.showMap = true;
    },
    /**
     * 树
     */
    treeInit() {
      let that = this;
      organizationRemoveChildren(
        this.organizationId ? this.organizationId : this.eipEmptyGuid
      ).then((result) => {
        that.organization.data = result.data;
      });
    },

    /**
     * 树选择
     */
    organizationSelect(electedKeys, e) {
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
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.organizationId = newGuid();
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
      if (this.organizationId) {
        that.spinning = true;
        findById(this.organizationId).then(function (result) {
          that.form = result.data;
          that.form.parentId = that.form.parentName
            ? that.form.parentId
            : undefined;
          that.spinning = false;
        });
      }

      organizationRemoveChildren(
        this.organizationId ? this.organizationId : this.eipEmptyGuid
      ).then((result) => {
        that.organization.data = result.data;
      });
    },
  },
};
</script>
