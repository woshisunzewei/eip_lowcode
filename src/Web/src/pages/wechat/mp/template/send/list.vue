<template>
  <a-modal
    width="1000px"
    v-drag
    :visible="visible"
    :footer="null"
    :title="title"
    @cancel="cancel"
  >
    <div style="width: 100%">
      <a-card class="eip-admin-card-small" :bordered="false">
        <vxe-toolbar ref="toolbar">
          <template v-slot:buttons>
            <a-space>
              <a-button type="primary" @click="add">
                <a-icon type="plus" />新增
              </a-button>
              <!-- <a-button type="primary" @click="update"
                ><a-icon type="form" /> 编辑
              </a-button>
              <a-button type="danger" @click="del">
                <a-icon type="delete" />删除
              </a-button>

              <a-button type="primary"
                ><a-icon type="codepen" /> 立即发送
              </a-button>-->
            </a-space>
          </template>
        </vxe-toolbar>
        <vxe-table
          :loading="table.loading"
          ref="sendtable"
          id="wechatmptemplatesendlist"
          size="small"
          :height="400"
          :data="table.data"
        >
          <template #loading>
            <a-spin></a-spin>
          </template>
          <template #empty>
            <eip-empty />
          </template>
          <vxe-column type="seq" title="序号" width="60"></vxe-column>
          <vxe-column field="data" title="内容" width="100"></vxe-column>
          <vxe-column field="number" title="总人数" width="80"></vxe-column>
          <vxe-column field="haveSend" title="已发送" width="80"></vxe-column>
          <vxe-column
            field="createTime"
            title="创建时间"
            width="160"
          ></vxe-column>
          <vxe-column field="percent" title="消息发送进度">
            <template #default="{ row }">
              <div style="width: 95%">
                <a-progress :percent="row.percent" status="active" />
              </div>
            </template>
          </vxe-column>
          <vxe-column
            field="operation"
            title="操作"
            align="center"
            width="200px"
          >
            <template #default="{ row }">
              <a-tooltip @click="tableUpdateRow(row)" title="编辑">
                <label class="text-eip eip-cursor-pointer">编辑</label>
              </a-tooltip>
              <a-divider type="vertical" />
              <a-tooltip title="删除" @click="tableDelRow(row)">
                <label class="text-red eip-cursor-pointer">删除</label>
              </a-tooltip>
              <a-divider type="vertical" />
              <a-tooltip title="发送" @click="tableSendRow(row)">
                <label class="eip-cursor-pointer">发送</label>
              </a-tooltip>

              <a-divider type="vertical" />
              <a-tooltip title="预览" @click="tableSendPreviewRow(row)">
                <label class="eip-cursor-pointer">预览</label>
              </a-tooltip>
            </template>
          </vxe-column>
        </vxe-table>
      </a-card>
      <edit
        ref="edit"
        v-if="edit.visible"
        :visible.sync="edit.visible"
        :title="edit.title"
        :templateSendId="edit.templateSendId"
        :templateId="edit.templateId"
        @ok="operateStatus"
      ></edit>
    </div>

    <a-modal
      v-drag
      title="发送预览"
      :visible="preview.visible"
      @ok="previewOk"
      @cancel="preview.visible = false"
    >
      <a-input placeholder="请输入OpenId" v-model="preview.openId" />
    </a-modal>
  </a-modal>
</template>

<script>
import {
  query,
  del,
  send,
  sendpreview,
} from "@/services/wechat/mp/template/send/list";
import edit from "./edit";

import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      interval: null,
      table: {
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 162 + "px",
      },

      edit: {
        visible: false,
        templateSendId: "",
        templateId: this.templateId,
        title: "",
      },

      preview: {
        visible: false,
        templateSendId: "",
        openId: "",
      },
    };
  },
  beforeDestroy() {
    if (this.interval) {
      clearInterval(this.interval);
    }
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
    templateId: {
      type: String,
    },
    title: {
      type: String,
    },
  },
  components: { edit },
  created() {
    let that = this;
    that.tableInit();
    if (that.interval) {
      clearInterval(this.interval);
    }
    that.interval = setInterval(function () {
      that.tableInit();
    }, 2000);
  },
  methods: {
    /**
     * 取消
     */
    cancel() {
      this.$emit("update:visible", false);
    },

    /**
     * 初始化微信授权用户数据
     */
    tableInit() {
      let that = this;
      query(this.templateId).then(function (result) {
        result.data.forEach((item) => {
          if (item.number != 0) {
            item.percent = Number(
              (
                (item.haveSend.toFixed(2) / item.number.toFixed(2)) *
                100
              ).toFixed(2)
            );
          } else {
            item.percent = 0;
          }
        });
        that.table.data = result.data;
        that.table.loading = false;
      });
    },

    /**
     * 修改
     */
    tableUpdateRow(row) {
      this.edit.templateSendId = row.templateSendId;
      this.edit.templateId = this.templateId;
      this.edit.title = "微信公众号模版发送";
      this.edit.visible = true;
    },

    /**
     * 开始发送
     */
    tableSendRow(row) {
      let that = this;
      //提示是否删除
      this.$confirm({
        title: "发送提示",
        content: "是否发送消息,发送后不可测回",
        onOk() {
          that.$message.loading("发送中..");
          send(row.templateSendId).then((result) => {
            if (result.code == that.eipResultCode.success) {
              that.$message.destroy();
              that.$message.success(result.msg);
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        okText: "立即发送",
        cancelText: "取消",
        onCancel() {
          that.$message.destroy();
        },
      });
    },
    previewOk() {
      let that = this;
      if (!this.preview.openId) {
        that.$message.error("请输入OpenId");
        return false;
      }
      that.$message.loading("发送中..");
      sendpreview({
        templateSendId: this.preview.templateSendId,
        openId: this.preview.openId,
      }).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.$message.destroy();
          that.$message.success(result.msg);
        } else {
          that.$message.error(result.msg);
        }
        that.preview.visible = false;
      });
    },
    /**
     * 开始发送
     */
    tableSendPreviewRow(row) {
      this.preview.templateSendId = row.templateSendId;
      this.preview.visible = true;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "微信公众号模版发送" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.templateSendId }).then((result) => {
            if (result.code == that.eipResultCode.success) {
              that.tableInit();
            }
            that.$loading.hide();
            that.$message.success(result.msg);
          });
        },
        that
      );
    },

    /**
     * 新增
     */
    add() {
      this.edit.templateSendId = null;
      this.edit.templateId = this.templateId;
      this.edit.title = "新增微信公众号模版发送";
      this.edit.visible = true;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRow(
        that.$refs.sendtable,
        function (row) {
          that.edit.templateId = that.templateId;
          that.edit.templateSendId = row.templateSendId;
          that.edit.title = "微信公众号模版发送";
          that.edit.visible = true;
        },
        that
      );
    },

    /**
     * 删除
     */
    del() {
      let that = this;
      selectTableRow(
        that.$refs.sendtable,
        function (rows) {
          //提示是否删除
          deleteConfirm(
            that.eipMsg.delete,
            function () {
              //加载提示
              that.$loading.show({ text: that.eipMsg.delloading });
              let ids = that.$utils.map(rows, (item) => item.templateSendId);
              del({ id: ids.join(",") }).then((result) => {
                if (result.code == that.eipResultCode.success) {
                  that.tableInit();
                }
                that.$loading.hide();
                that.$message.success(result.msg);
              });
            },
            that
          );
        },
        that,
        false
      );
    },

    //提示状态信息
    operateStatus(result) {
      if (result.code === this.eipResultCode.success) {
        this.tableInit();
      }
    },
  },
};
</script>