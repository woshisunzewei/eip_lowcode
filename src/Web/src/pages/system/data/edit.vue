<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :visible="visible"
    :bodyStyle="{ padding: '1px' }" :title="title" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item ref="menuName" label="归属模块" prop="menuName">
              <a-tree-select placeholder="请选择归属模块" allow-clear v-model="form.menuName" show-search style="width: 100%"
                treeNodeFilterProp="title" :tree-data="data" :dropdown-style="{ maxHeight: '400px', overflow: 'auto' }"
                @select="treeSelect"></a-tree-select>
            </a-form-model-item>
            <a-form-model-item label="名称" prop="name">
              <a-input v-model="form.name" placeholder="请输入名称" />
            </a-form-model-item>

            <a-form-model-item label="表达式">
              <eip-editor ref="editor" v-if="tinymce.show" v-model="form.ruleHtml" :height="tinymce.height"
                :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
              <a-tag @click="all">所有</a-tag>
              <a-tag @click="currentUser">当前用户</a-tag>
              <a-tag @click="currentUserName">当前用户名</a-tag>
              <a-tag @click="currentOrganization">所在组织</a-tag>
              <a-tag @click="currentChildrenOrganization">所在组织及下级组织</a-tag>
              <a-tag @click="currentPost">所在岗位</a-tag>
              <a-tag @click="currentGroup">所在工作组</a-tag>
            </a-form-model-item>

            <a-form-model-item label="禁用" prop="isFreeze">
              <a-switch v-model="form.isFreeze" />
            </a-form-model-item>
            <a-form-model-item ref="orderNo" label="排序号" prop="orderNo">
              <a-input-number id="inputNumber" style="width: 100%" placeholder="请输入排序号" v-model="form.orderNo" :min="0"
                :max="999" />
            </a-form-model-item>
            <a-form-model-item label="备注" prop="remark">
              <a-input v-model="form.remark" type="textarea" placeholder="请输入备注" />
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <div class="flex justify-between align-center">
        <div>
          <a-checkbox v-if="!dataId" v-model="save.retain">
            继续创建时，保留本次提交内容
          </a-checkbox>
        </div>

        <a-space>
          <a-button v-if="!dataId" key="submit" @click="saveContinue" :loading="loading"><a-icon
              type="save" />提交并继续创建</a-button>
          <a-button v-if="dataId" @click="cancel"><a-icon type="close" />关闭</a-button>
          <a-button key="submit" @click="saveClose" type="primary" :loading="loading"><a-icon
              type="save" />提交</a-button>
        </a-space>
      </div>
    </template>
  </a-modal>
</template>
<script>
import { save, findById } from "@/services/system/data/edit";
import { newGuid } from "@/utils/util";
export default {
  name: "edit",
  data() {
    return {
      tinymce: {
        plugins: "preview  fullscreen code",
        toolbar: "undo redo |fullscreen|code",
        height: 200,
        show: false,
        menubar: "",
      },

      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        dataId: newGuid(),
        menuId: null,
        menuName: null,
        name: null,
        ruleHtml: null,
        orderNo: 1,
        isFreeze: false,
        remark: null,
      },

      rules: {
        menuName: [
          {
            required: true,
            message: "请选择归属模块",
            trigger: ["blur", "change"],
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
    dataId: {
      type: String,
    },
  },
  mounted() {
    this.find();
    setTimeout(() => {
      this.tinymce.show = true;
    }, 10);
  },
  methods: {
    /**
     * 树选择
     */
    treeSelect(electedKeys, e) {
      this.form.menuName = e.title;
      this.form.menuId = e.eventKey;
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
          that.form.dataId = that.copy ? newGuid() : that.form.dataId;
          save(that.form).then(function (result) {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              if (that.save.continue) {
                //不保留上次内容
                if (!that.save.retain) {
                  that.$refs.form.resetFields();
                }
                that.form.dataId = newGuid();
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
      if (this.dataId) {
        that.spinning = true;
        findById(this.dataId).then(function (result) {
          that.form = result.data;
          that.spinning = false;
        });
      } else {
        that.spinning = false;
      }
    },

    /**
     * 所有
     */
    all() {
      let html = "<span id='" + encodeURIComponent("所有") + "' class='non-editable'>所有</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 当前用户
     */
    currentUser() {
      let html = "<span id='" + encodeURIComponent("当前用户") + "' class='non-editable'>当前用户</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
    * 当前用户名
    */
    currentUserName() {
      let html = "<span id='" + encodeURIComponent("当前用户名") + "' class='non-editable'>当前用户名</span>";
      this.$refs.editor.insertIndex(html, 0);
    },

    /**
     * 所在组织
     */
    currentOrganization() {
      let html = "<span id='" + encodeURIComponent("所在组织") + "' class='non-editable'>所在组织</span>";
      this.$refs.editor.insertIndex(html, 0);
    },
    /**
        * 所在岗位
        */
    currentPost() {
      let html = "<span id='" + encodeURIComponent("所在岗位") + "' class='non-editable'>所在岗位</span>";
      this.$refs.editor.insertIndex(html, 0);
    },
    /**
    * 所在组织
    */
    currentGroup() {
      let html = "<span id='" + encodeURIComponent("所在工作组") + "' class='non-editable'>所在工作组</span>";
      this.$refs.editor.insertIndex(html, 0);
    },
    /**
     * 所在组织及下级组织
     */
    currentChildrenOrganization() {
      let html = "<span id='" + encodeURIComponent("所在组织及下级组织") + "' class='non-editable'>所在组织及下级组织</span>";
      this.$refs.editor.insertIndex(html, 0);
    },
  },
};
</script>