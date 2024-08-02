<template>
  <div style="width: 100%">
    <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
      <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
    </a-card>

    <a-card class="eip-admin-card-small" :bordered="false">
      <vxe-toolbar ref="toolbar" custom print export :refresh="{ query: tableInit }">
        <template v-slot:buttons>
          <eip-toolbar @del="del" @update="update" @add="add" @copy="copy" @designer="designerConfig"
            @onload="toolbarOnload"></eip-toolbar>
        </template>
      </vxe-toolbar>
      <vxe-table :loading="table.loading" ref="table" id="workflowprocesslist" size="small" :seq-config="{
        startIndex: (table.page.param.current - 1) * table.page.param.size,
      }" :height="table.height" :export-config="{}" :print-config="{}" :data="table.data" v-viewer>
        <template #loading>
          <a-spin></a-spin>
        </template>
        <template #empty>
          <eip-empty />
        </template>
        <vxe-column type="checkbox" width="40" align="center" fixed="left">
          <template #header="{ checked, indeterminate }">
            <span @click.stop="$refs.table.toggleAllCheckboxRow()">
              <a-checkbox :indeterminate="indeterminate" :checked="checked">
              </a-checkbox>
            </span>
          </template>
          <template #checkbox="{ row, checked, indeterminate }">
            <span @click.stop="$refs.table.toggleCheckboxRow(row)">
              <a-checkbox :indeterminate="indeterminate" :checked="checked">
              </a-checkbox>
            </span>
          </template>
        </vxe-column>
        <vxe-column type="seq" title="序号" width="60"></vxe-column>
        <vxe-column field="icon" title="名称" width="200">
          <template v-slot="{ row }">
            {{ row.name }} <a-tag v-if="row.isAgile > 0" color="red"> 敏
            </a-tag>
          </template>
        </vxe-column>
        <vxe-column field="shortName" title="简称" width="200" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="thumbnail" title="流程图" align="center" width="80">
          <template v-slot="{ row }">
            <img v-if="row.thumbnail" style="width: 32px; height: 32px; " :src="row.thumbnail" />
          </template>
        </vxe-column>
        <vxe-column field="icon" title="图标" width="60" align="center">
          <template v-slot="{ row }">
            <a-icon v-if="row.icon" :type="row.icon" :theme="row.theme" />
          </template>
        </vxe-column>

        <vxe-column field="icon" title="图片" width="60" align="center">
          <template v-slot="{ row }">
            <a-avatar shape="square" v-if="row.image" :src="row.image" />
          </template>
        </vxe-column>

        <vxe-column field="typeName" title="类别" width="150"></vxe-column>
        <vxe-column field="version" title="版本号" width="100"></vxe-column>
        <vxe-column field="showLibrary" title="流程库显示" width="100" align="center">
          <template v-slot="{ row }">
            <a-switch :checked="row.showLibrary" />
          </template>
        </vxe-column>
        <vxe-column field="isFreeze" title="冻结" width="100" align="center">
          <template v-slot="{ row }">
            <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" />
          </template>
        </vxe-column>
        <vxe-column field="remark" title="备注" min-width="150" showOverflow="ellipsis"></vxe-column>
        <vxe-column field="orderNo" title="排序" align="center" width="80"></vxe-column>
        <vxe-column field="createUserName" title="创建人" width="100"></vxe-column>
        <vxe-column field="createTime" title="创建时间" width="160"></vxe-column>
        <vxe-column field="updateUserName" title="修改人" width="100"></vxe-column>
        <vxe-column field="updateTime" title="修改时间" width="160"></vxe-column>
        <vxe-column title="操作" align="center" fixed="right" width="160px">
          <template #default="{ row }">
            <a-tooltip @click="tableUpdateRow(row)" title="编辑" v-if="visible.update">
              <label class="text-eip eip-cursor-pointer">编辑</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.del && row.isAgile == 0" />
            <a-tooltip title="删除" v-if="visible.del && row.isAgile == 0" @click="tableDelRow(row)">
              <label class="text-red eip-cursor-pointer">删除</label>
            </a-tooltip>
            <a-divider type="vertical" v-if="visible.designer && row.isAgile == 0" />
            <a-tooltip title="设计" v-if="visible.designer && row.isAgile == 0" @click="tableDesignerRow(row)">
              <label class="eip-cursor-pointer">设计</label>
            </a-tooltip>
          </template>
        </vxe-column>
      </vxe-table>

      <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
        show-quick-jumper :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
        :page-size="table.page.param.size" :total="table.page.total" @change="tableInit"
        @showSizeChange="tableSizeChange"></a-pagination>
    </a-card>

    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="edit.data" :title="edit.title"
      :copy="edit.copy" :processId="edit.processId" @ok="operateStatus"></edit>

    <designer ref="designer" v-if="designer.visible" :title="designer.title" :processId="designer.processId"
      :visible.sync="designer.visible" @save="reload"></designer>
  </div>
</template>

<script>
import Vue from 'vue'
import Viewer from 'v-viewer'
import 'viewerjs/dist/viewer.css'
Vue.use(Viewer)
Viewer.setDefaults({
  Options: {
    'inline': true, // 启用 inline 模式
    'button': true, // 显示右上角关闭按钮
    'navbar': true, // 显示缩略图导航
    'title': true, // 显示当前图片的标题
    'toolbar': true, // 显示工具栏
    'tooltip': true, // 显示缩放百分比
    'movable': true, // 图片是否可移动
    'zoomable': true, // 图片是否可缩放
    //'rotatable': true, // 图片是否可旋转
    //'scalable': true, // 图片是否可翻转
    'transition': true, // 使用 CSS3 过度
    //'fullscreen': true, // 播放时是否全屏
    'keyboard': true, // 是否支持键盘
    'url': 'data-source' // 设置大图片的 url
  }
})
import {
  query,
  del,
} from "@/services/workflow/process/list";
import edit from "./edit";
import designer from "./designer";
import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "process.OrderNo",
            filters: "",
            type: undefined,
          },
          total: 0,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: this.eipHeaderHeight() - 162 + "px",
        search: {
          option: {
            num: 6,
            config: [
              {
                field: "Name",
                op: "cn",
                placeholder: "请输入名称",
                title: "名称",
                value: "",
                type: "text",
              },
              {
                field: "ShortName",
                op: "cn",
                placeholder: "请输入简称",
                title: "简称",
                value: "",
                type: "text",
              },
              {
                field: "Remark",
                op: "cn",
                placeholder: "请输入备注",
                title: "备注",
                value: "",
                type: "text",
              },
            ],
          },
        },
      },
      visible: {
        update: true,
        del: true,
        designer: true,
      },
      edit: {
        visible: false,
        processId: "",
        copy: false,
        title: "",
      },

      designer: {
        visible: false,
        processId: "",
        title: "",
        bodyStyle: {
          padding: "1px",
          "padding-bottom": "4px",
        },
      },
    };
  },
  components: { edit, designer },
  created() {
    this.tableInit();
  },
  mounted() {
    this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     *
     */
    designerClose() {
      this.designer.visible = false;
    },

    /**
     * 设计
     */
    tableDesignerRow(row) {
      let that = this;
      that.designer.processId = row.processId;
      that.designer.title = "流程配置:" + row.name + " " + row.version;
      that.designer.visible = true;
    },

    /**
     * 流程查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    //初始化流程数据
    tableInit() {
      let that = this;
      that.table.loading = true;
      query(that.table.page.param).then(function (result) {
        if (result.code == that.eipResultCode.success) {
          that.table.data = result.data;
          that.table.page.total = result.total;
        }
        that.table.loading = false;
      });
    },
    /**
     *数量改变
     */
    tableSizeChange(current, pageSize) {
      this.table.page.param.size = pageSize;
      this.tableInit();
    },

    /**
     * 树更新
     */
    tableUpdateRow(row) {
      this.edit.processId = row.processId;
      this.edit.title = "流程配置-" + row.name;
      this.edit.visible = true;
    },
    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "流程配置【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.processId }).then((result) => {
            that.$loading.hide();
            if (result.code == that.eipResultCode.success) {
              that.reload();
              that.$message.success(result.msg);
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        that
      );
    },

    /**
     * 新增
     */
    add() {
      this.edit.copy = false;
      this.edit.processId = null;
      this.edit.title = "新增流程配置";
      this.edit.visible = true;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.copy = false;
          that.edit.processId = row.processId;
          that.edit.title = "流程配置-" + row.name;
          that.edit.visible = true;
        },
        that
      );
    },
    /**
     * 复制
     */
    copy() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.processId = row.processId;
          that.edit.copy = true;
          that.edit.title = "复制流程配置-" + row.name;
          that.edit.visible = true;
        },
        that
      );
    },
    /**
     * 预览
     */
    preview() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.processId = row.processId;
          that.edit.title = "流程预览-" + row.name;
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
        that.$refs.table,
        function (rows) {
          //判断是否具有敏捷开发
          var isAgile = rows.filter(f => f.isAgile > 0).length > 0;
          if (isAgile) {
            that.$message.error('选择数据具有敏捷开发流程,请取消后重试');
          } else {
            //提示是否删除
            deleteConfirm(
              that.eipMsg.delete,
              function () {
                //加载提示
                that.$loading.show({ text: that.eipMsg.delloading });
                let ids = that.$utils.map(rows, (item) => item.processId);
                del({ id: ids.join(",") }).then((result) => {
                  that.$loading.hide();
                  if (result.code == that.eipResultCode.success) {
                    that.reload();
                    that.$message.success(result.msg);
                  } else {
                    that.$message.error(result.msg);
                  }
                });
              },
              that
            );
          }

        },
        that,
        false
      );
    },
    /**
     * 设计
     */
    designerConfig() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.designer.processId = row.processId;
          that.designer.title = row.name;
          that.designer.visible = true;
        },
        that
      );
    },

    /**
     * 编辑关闭
     */
    designerCancel() {
      this.designer.visible = false;
    },

    //提示状态信息
    operateStatus(result) {
      if (result.code === this.eipResultCode.success) {
        this.reload();
      }
    },

    /**
     * 重新加载
     */
    reload() {
      this.table.page.param.current = 1;
      this.tableInit();
    },

    /**
     * 权限加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.designer =
        buttons.filter((f) => f.method == "designer").length > 0;
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;
    },
  },
};
</script>