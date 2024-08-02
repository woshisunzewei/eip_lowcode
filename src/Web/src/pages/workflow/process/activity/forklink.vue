<template>
  <a-modal width="600px" v-drag :destroyOnClose="true" :maskClosable="false" :title="title" :visible="visible"
    :bodyStyle="{ padding: '1px' }" @cancel="cancel">
    <a-spin :spinning="spinning">
      <a-form-model ref="form" :model="form" :rules="rules" :label-col="config.labelCol"
        :wrapper-col="config.wrapperCol">
        <a-row>
          <a-col>
            <a-form-model-item label="名称" prop="title">
              <a-input v-model="form.title" placeholder="请输入名称" />
            </a-form-model-item>
            <a-form-model-item label="表单" prop="formId">
              <a-select placeholder="请选择表单" allow-clear v-model="form.formId" @change="columnSync">
                <a-select-option v-for="(item, i) in forms" :key="i" :value="item.configId">{{ item.name
                  }}</a-select-option>
              </a-select>
              <a-tag class="copyBtn" @click="valueCopy()" :data-clipboard-text="item.name"
                v-for="(item, index) in columnTable.data" :key="index">{{ item.description }}</a-tag>
            </a-form-model-item>
            <a-form-model-item label="表达式" prop="judge">
              <div class="padding-bottom-sm">
                <a-alert message="请填写条件表达式,支持sql语句判断如>,>=,<,<=,and,or等" type="warning" show-icon />
              </div>
              <a-input v-model="form.judge" :rows="4" type="textarea" placeholder="请输入表达式" />
              <div class="padding-bottom-sm">
                <a-alert
                  message="角色判断:CreateUserId in (select PrivilegeMasterUserId from system_permissionuser where PrivilegeMaster=0 and PrivilegeMasterValue in( '角色1id','角色2id'))"
                  type="warning" show-icon />
              </div>
              <div class="padding-bottom-sm">
                <a-space>
                  <a-button @click="roleTable.visible = true" type="primary">
                    <a-icon type="usergroup-add"></a-icon> 查看角色Id</a-button>
                </a-space>
              </div>
            </a-form-model-item>
          </a-col>
        </a-row>
      </a-form-model>
    </a-spin>
    <template slot="footer">
      <a-space>
        <a-button key="back" @click="cancel" :disabled="loading"><a-icon type="close" />取消</a-button>
        <a-button key="submit" @click="save" type="primary" :loading="loading"><a-icon type="save" />提交</a-button>
      </a-space>
    </template>

    <a-modal :footer="null" :zIndex="1100" width="1000px" v-drag centered :destroyOnClose="true" :maskClosable="false"
      :visible="roleTable.visible" :bodyStyle="{ padding: '1px' }" title="查看角色Id" @cancel="roleTable.visible = false">
      <a-card class="eip-admin-card-small" :bordered="false">
        <eip-search :option="roleTable.search.option" @search="roleSearch"></eip-search>
      </a-card>

      <a-card class="eip-admin-card-small" :bordered="false">
        <a-spin :spinning="roleTable.loading">
          <vxe-table ref="table" id="systemrolelist" size="small" :seq-config="{
    startIndex:
      (roleTable.page.param.current - 1) * roleTable.page.param.size,
  }" :height="roleTable.height" :data="roleTable.data">
            <template #loading>
              <a-spin></a-spin>
            </template>
            <template #empty>
              <eip-empty />
            </template>
            <vxe-column type="seq" title="序号" width="60"></vxe-column>
            <vxe-column title="角色Id" align="center" width="360px" showOverflow="ellipsis">
              <template #default="{ row }">
                {{ row.roleId }}
                <a-tooltip title="复制">
                  <a-button class="copyBtn" type="primary" @click="valueCopy()" :data-clipboard-text="row.roleId"
                    size="small">复制</a-button>
                </a-tooltip>
              </template>
            </vxe-column>
            <vxe-column field="name" title="名称" width="150" showOverflow="ellipsis"></vxe-column>
            <vxe-column field="parentIdsName" title="归属组织" min-width="150" showOverflow="ellipsis"></vxe-column>
            <vxe-column field="isFreeze" title="禁用" align="center" width="80">
              <template v-slot="{ row }">
                <a-switch :checked="row.isFreeze" />
              </template>
            </vxe-column>
            <vxe-column field="remark" title="备注" width="150" showOverflow="ellipsis"></vxe-column>
          </vxe-table>
          <a-pagination class="padding-top-sm float-right" v-model="roleTable.page.param.current" size="small"
            show-size-changer show-quick-jumper :page-size-options="roleTable.page.sizeOptions"
            :show-total="(total) => `共 ${total} 条`" :page-size="roleTable.page.param.size" :total="roleTable.page.total"
            @change="roleInit" @showSizeChange="roleSizeChange"></a-pagination>
        </a-spin>
      </a-card>
    </a-modal>
  </a-modal>
</template>

<script>
import {
  findForm,
  findFormColumns,
} from "@/services/workflow/process/activity/begin";
import {
  query,
} from "@/services/system/role/list";

import Clipboard from "clipboard";
import { mapGetters } from "vuex";
export default {
  computed: {
    ...mapGetters("account", ["systemAgile"]),
  },
  name: "edit",
  data() {
    return {
      modal: {
        bodyStyle: {
          padding: "1px",
        },
      },
      config: {
        labelCol: { span: 4 },
        wrapperCol: { span: 20 },
      },
      form: {
        title: null,
        judge: null,
        formId: undefined,
      },

      rules: {
        formId: [
          {
            required: true,
            message: "请选择表单",
            trigger: ["blur", "change"],
          },
        ],
        title: [
          {
            required: true,
            message: "请输入名称",
            trigger: "blur",
          },
        ],

        judge: [
          {
            required: true,
            message: "请输入表达式",
            trigger: "blur",
          },
        ],
      },
      forms: [],
      loading: false,
      spinning: false,

      roleTable: {
        visible: false,
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "",
            id: "",
            filters: "",
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: "300px",
        search: {
          option: {
            num: 4,
            config: [
              {
                field: "role.Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "role.Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              }
            ],
          },
        }
      },
      columnTable: {
        data: [],
      },
    };
  },

  mounted() {
    this.formInit();
    this.roleInit();
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    data: {
      type: Object,
    },
    title: {
      type: String,
    },
    formId: {
      type: String,
    },
  },

  methods: {
    copy() {
      var clipboard = new Clipboard('.tag-read')
      clipboard.on('success', e => {
        console.log('复制成功')
        // 释放内存
        clipboard.destroy()
      })
      clipboard.on('error', e => {
        // 不支持复制
        console.log('该浏览器不支持自动复制')
        // 释放内存
        clipboard.destroy()
      })
    },
    /**
     * 初始化表单
     */
    formInit() {
      let that = this;
      that.spinning = true;
      findForm({
        configType: 2,
      }).then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.forms = result.data;
          //其他信息
          if (that.data) {
            that.data.props.base.judge = unescape(that.data.props.base.judge);
            that.form = that.$utils.clone(that.data.props.base, true);
            if (!that.form.formId && that.formId) {
              that.form.formId = that.formId;
            }
          } else {
            that.form.formId = that.formId;
          }
        }

        if (that.form.formId) {
          that.columnSync();
        }
        that.spinning = false;
      });
    },
    /**
        * 同步字段
        */
    async columnSync() {
      let that = this;
      that.columnTable.data = [];
      var result = that.systemAgile.filter(f => f.configId == this.form.formId)[0];
      if (result && result.columnJson) {
        //如果具有敏捷开发信息
        let original = JSON.parse(result.columnJson);
        var column = original.filter(
          (f) =>
            !["batch", "text", "divider"].includes(f.type)
        );

        column.forEach((item) => {
          var have = that.columnTable.data.filter((f) => f.name == item.model);
          if (have.length == 0) {
            var d = {}
            d.name = item.model;
            d.description = item.label;
            d.type = item.type
            that.columnTable.data.push(d);
          }
        });

        //子表
        column.filter((f) => f.type == "batch").forEach((item) => {
          var have = that.columnTable.data.filter((f) => f.name == item.model);
          if (have.length == 0) {
            var batchColumns = item.list;
            //循环子表字段
            var batchFields = [];
            batchColumns.forEach(batchColumn => {
              var d = {}
              d.name = batchColumn.model;
              d.description = batchColumn.label;
              d.type = batchColumn.type
              batchFields.push(d);
            })
            var d = {}
            d.name = item.model;
            d.description = item.label;
            d.type = 'batch'

            that.columnTable.data.push(d);
          }
        });
      } else {
        findFormColumns(this.form.formId).then(function (result) {
          if (result.code === that.eipResultCode.success) {
            result.data.forEach((item) => {
              var have = that.columnTable.data.filter((f) => f.name == item.name);
              if (have.length == 0) {
                that.columnTable.data.push(item);
              }
            });
          }
        });
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
    save() {
      let that = this;
      this.$refs.form.validate((valid) => {
        if (valid) {
          that.cancel();
          that.form.judge = escape(that.form.judge);
          that.data.props.base = that.form;
          that.$emit("ok", that.form);
        } else {
          return false;
        }
      });
    },

    /**
     * 
     */
    roleInit() {
      let that = this;
      that.roleTable.loading = true;
      query(that.roleTable.page.param).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.roleTable.data = result.data;
          that.roleTable.page.total = result.total;
        }
        that.roleTable.loading = false;
      });
    },
    /**
         * 
         */
    valueCopy() {
      // 复制数据
      let clipboard = new Clipboard('.copyBtn');
      clipboard.on("success", () => {
        this.$message.success("复制成功");
      });
      clipboard.on("error", () => {
        this.$message.error("复制失败");
      });
      setTimeout(() => {
        // 销毁实例
        clipboard.destroy();
      }, 122);
    },
    /**
    *数量改变
    */
    roleSizeChange(current, pageSize) {
      this.roleTable.page.param.size = pageSize;
      this.roleInit();
    },

    /**
   * 列表查询
   */
    roleSearch(filters) {
      this.roleTable.page.param.current = 1;
      this.roleTable.page.param.filters = filters;
      this.roleInit();
    },
  },
};
</script>