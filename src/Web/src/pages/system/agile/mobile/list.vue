<template>
  <div style="width: 100%">

    <div style="width: 250px; float: left">
      <a-card size="small" class="eip-admin-card-small">
        <div :style="typeTree.style">
          <a-menu @select="menuSelect" mode="inline" :default-selected-keys="['3']"
            :style="{ height: '100%', borderRight: 0 }">
            <a-menu-item key="3">
              <a-icon type="appstore" />
              工作台
            </a-menu-item>
            <a-menu-item key="4">
              <a-icon type="user" />
              我的
            </a-menu-item>
            <a-menu-item key="5">
              <a-icon type="code-sandbox" />
              自定义
            </a-menu-item>
          </a-menu>
        </div>
      </a-card>
    </div>
    <div class="eip-admin-card-tree-left">
      <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false">
        <eip-search :option="table.search.option" @search="tableSearch"></eip-search>
      </a-card>

      <a-card class="eip-admin-card-small" :bordered="false">
        <vxe-toolbar ref="toolbar" :refresh="{ query: tableInit }">
          <template v-slot:buttons>
            <eip-toolbar @del="del" @update="update" @add="add" @designer="designerConfig"
              @onload="toolbarOnload"></eip-toolbar>
          </template>
        </vxe-toolbar>
        <a-spin :spinning="table.loading">
          <div class="template-view beauty-scroll" :style="{ height: table.height, overflow: 'auto' }">
            <div class="el-main sa-p-0 sa-m-0">
              <div class="template-wrap sa-flex sa-flex-wrap">
                <a-layout v-if="table.data.length == 0">
                  <a-layout-content :style="{
                    margin: '0',
                    background: '#fff',
                    minHeight: '280px',
                  }">
                    <a-result title="数据为空" sub-title="">
                      <template #icon>
                        <img src="/images/empty.png" />
                      </template>
                      <template #extra>

                      </template>
                    </a-result>
                  </a-layout-content></a-layout>
                <div class="item" v-for="(row, index) in table.data" :key="index">
                  <div class="el-carousel ">
                    <div style="height: 480px;">
                      <div slot="prevArrow" slot-scope="props" class="custom-slick-arrow" style="left: 10px;zIndex: 1">
                        <a-icon type="left-circle" />
                      </div>
                      <div slot="nextArrow" slot-scope="props" class="custom-slick-arrow" style="right: 10px">
                        <a-icon type="right-circle" />
                      </div>
                      <div>
                        <img :src="row.thumbnail">
                      </div>
                    </div>
                  </div>
                  <div class="footer">
                    <div class="name">{{ row.name }}</div>
                    <div class="memo sa-flex">
                      <div class="label">备注：</div>
                      <div>{{ row.remark }}</div>
                    </div>
                    <div class="memo sa-flex">
                      <div class="label">禁用：</div>
                      <div> <a-switch :checked="row.isFreeze" @change="isFreezeChange(row)" /></div>
                    </div>
                    <div class="update-time sa-flex">
                      <div class="label">更新时间：</div>
                      <div>{{ row.updateTime }}</div>
                    </div>
                    <div class="oper sa-flex sa-row-between">
                      <a-button type="link" v-if="visible.designer" @click="tableDesignerRow(row)"> 装修 </a-button>
                      <div class="sa-flex">
                        <a-button type="link" v-if="visible.update" @click="tableUpdateRow(row)"> 编辑
                        </a-button>
                        <a-button type="link" v-if="visible.copy" @click="tableCopyRow(row)"> 复制
                        </a-button>
                        <a-button type="link" style="color:red" v-if="visible.del" @click="tableDelRow(row)"> 删除
                        </a-button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
          <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current" show-size-changer
            show-quick-jumper showSizeChanger showQuickJumper :page-size-options="table.page.sizeOptions"
            :show-total="(total) => `共 ${total} 条`" :page-size="table.page.param.size" :total="table.page.total"
            @change="tableInit" @showSizeChange="tableSizeChange"></a-pagination>
        </a-spin>
      </a-card>
    </div>
    <edit ref="edit" v-if="edit.visible" :visible.sync="edit.visible" :data="edit.data" :title="edit.title"
      :configId="edit.configId" :copy="edit.copy" :configType="table.page.param.configType" @ok="operateStatus"></edit>

    <designer ref="designer" v-if="designer.visible" :visible.sync="designer.visible" :data="designer.data"
      :title="designer.title" :configId="designer.configId" @reload="tableInit"></designer>
  </div>
</template>

<script>
import {
  query,
  del,
  isFreeze
} from "@/services/system/agile/form/list";
import edit from "./edit";
import designer from "./designer";

import { selectTableRow, deleteConfirm } from "@/utils/util";
export default {
  data() {
    return {
      typeTree: {
        style: {
          height: this.eipHeaderHeight() - 26 + "px",
        },
        data: [],
        spinning: true,
        right: {
          item: null,
          style: "",
        },
      },
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "desc",
            sidx: "",
            filters: "",
            configType: "3",
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
                field: "remark",
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
        copy: true,
        update: true,
        del: true,
        designer: true,
      },
      detail: {
        data: {},
        visible: false,
      },

      edit: {
        visible: false,
        configId: "",
        title: "",
        data: [],
        copy: false,
      },

      designer: {
        visible: false,
        configId: "",
        title: "",
        data: [],
      },
    };
  },
  components: { edit, designer },
  created() {
    this.tableInit();
  },
  mounted() {
    // this.$refs.table.connect(this.$refs.toolbar);
  },
  methods: {
    /**
     *
     */
    isFreezeChange(row) {
      if (this.visible.update) {
        let that = this;
        this.$message.loading("保存中...", 0);
        isFreeze({ id: row.configId }).then((result) => {
          that.$message.destroy();
          if (result.code == that.eipResultCode.success) {
            that.reload(false);
            that.$message.success(result.msg);
          } else {
            that.$message.error(result.msg);
          }
        });
      }
    },
    /**
     * 菜单选择
     */
    menuSelect({ item, key, selectedKeys }) {
      this.table.page.param.configType = key;
      this.table.page.param.current = 1;
      this.tableInit();
    },

    /**
     * 列表查询
     */
    tableSearch(filters) {
      this.table.page.param.current = 1;
      this.table.page.param.filters = filters;
      this.tableInit();
    },

    /**
     * 初始化列表数据
     */
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
     * 更新
     */
    tableUpdateRow(row) {
      this.edit.title = "编辑移动构建-" + row.name;
      this.edit.copy = false;
      this.edit.configId = row.configId;
      this.edit.visible = true;
    },

    /**
     * 复制
     */
    tableCopyRow(row) {
      let that = this;
      that.edit.title = "复制移动构建-" + row.name;
      that.edit.configId = row.configId;
      that.edit.copy = true;
      that.edit.visible = true;
    },

    /**
     * 删除
     */
    tableDelRow(row) {
      let that = this;
      deleteConfirm(
        "确定删除【" + row.name + "】" + that.eipMsg.delete,
        function () {
          that.$loading.show({ text: that.eipMsg.delloading });
          del({ id: row.configId }).then((result) => {
            if (result.code == that.eipResultCode.success) {
              that.reload();
            }
            that.$loading.hide();
            that.$message.success(result.msg);
          });
        },
        that
      );
    },

    /**
     * 设计
     */
    tableDesignerRow(row) {
      let that = this;
      that.designer.title = row.name;
      that.designer.visible = true;
      that.designer.configId = row.configId;
    },

    /**
     * 新增
     */
    add() {
      this.edit.configId = null;
      this.edit.title = "新增移动构建";
      this.edit.visible = true;
      this.edit.copy = false;
    },

    /**
     * 修改
     */
    update() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.edit.title = "编辑移动构建-" + row.name;
          that.edit.configId = row.configId;
          that.edit.visible = true;
          this.edit.copy = false;
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
          //提示是否删除
          deleteConfirm(
            that.eipMsg.delete,
            function () {
              //加载提示
              that.$loading.show({ text: that.eipMsg.delloading });
              let ids = that.$utils.map(rows, (item) => item.configId);
              del({ id: ids.join(",") }).then((result) => {
                if (result.code == that.eipResultCode.success) {
                  that.reload();
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
    /**
     * 设计
     */
    designerConfig() {
      let that = this;
      selectTableRow(
        that.$refs.table,
        function (row) {
          that.designer.title = row.name;
          that.designer.visible = true;
          that.designer.configId = row.configId;
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
     * 权限按钮加载完毕
     */
    toolbarOnload(buttons) {
      this.visible.designer =
        buttons.filter((f) => f.method == "designer").length > 0;
      this.visible.update =
        buttons.filter((f) => f.method == "update").length > 0;
      this.visible.del = buttons.filter((f) => f.method == "del").length > 0;

      this.visible.copy =
        buttons.filter((f) => f.method == "copy").length > 0;
    },
  },
};
</script>

<style lang="less" scoped>
.el-container {
  height: 100%;
  position: relative;
}

.el-container.is-vertical {
  flex-direction: column;
}

.el-container {
  display: flex;
  flex-direction: row;
  flex: 1;
  flex-basis: auto;
  box-sizing: border-box;
  min-width: 0;
}

.el-main {
  --el-main-padding: 20px;
  display: block;
  flex: 1;
  flex-basis: auto;
  overflow: auto;
  box-sizing: border-box;
}

.sa-padding-0,
.sa-p-0 {
  padding: 0 !important;
}

.template-view .template-wrap {
  padding: 10px 20px;
}

.sa-flex-wrap {
  flex-wrap: wrap;
}

.sa-flex {
  display: flex;
  flex-direction: row;
  align-items: center;
}

.template-view .template-wrap {
  padding: 10px 20px
}

.template-view .template-wrap .item {
  position: relative;
  width: 246px;
  height: 480px;
  border: 1px solid #f3f3f3;
  box-shadow: 0 0 4px #59595933;
  box-sizing: border-box;
  border-radius: 4px;
  margin-bottom: 20px;
  overflow: hidden;
  margin-right: 20px
}

.template-view .template-wrap .item img {
  width: 100%
}

.template-view .template-wrap .item:hover {
  transform: scale(1.02);
  box-shadow: 0 4px 16px #5959593d
}

.template-view .template-wrap .item:hover .footer {
  opacity: 1
}

.template-view .template-wrap .item .image-slot {
  height: 200px
}

.template-view .template-wrap .item .footer {
  position: absolute;
  bottom: 0;
  width: 100%;
  height: fit-content;
  padding: 0 12px;
  background: #fdfdfd;
  transition: all .5s;
  opacity: 0
}

.template-view .template-wrap .item .footer .name {
  padding: 12px 0 4px;
  color: #262626;
  font-size: 16px
}

.template-view .template-wrap .item .footer .platform {
  height: 28px;
  font-size: 14px;
  color: #8c8c8c
}

.template-view .template-wrap .item .footer .memo,
.template-view .template-wrap .item .footer .update-time {
  height: 20px;
  font-size: 12px;
  color: #8c8c8c
}

.template-view .template-wrap .item .footer .label {
  flex-shrink: 0
}

.template-view .template-wrap .item .footer .oper {
  height: 36px
}

.template-view .template-wrap .item .footer .oper .el-button+.el-button {
  margin-left: 8px
}

.simg .el-image {
  width: 100%;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}

.el-image {
  position: relative;
  display: inline-block;
  overflow: hidden;
}

.sa-row-between {
  justify-content: space-between;
}

.sa-flex {
  display: flex;
  flex-direction: row;
  align-items: center;
}

.ant-btn-link {
  border-color: transparent;
  background: transparent;
  padding: 2px;
  height: auto;
}
</style>
