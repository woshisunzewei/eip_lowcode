<template>
  <div>
    <a-layout id="components-layout-demo-custom-trigger" style="background: #fff">
      <a-layout-sider :width="280" :style="{ overflow: 'auto', height: height }" class="beauty-scroll">
        <a-card size="small" :bordered="false" class="eip-admin-card-small">
          <a-spin :spinning="menu.loading">
            <vxe-toolbar style="padding-top: 1px;">
              <template #buttons>
                <a-input-search allowClear placeholder="名称关键字" v-model="menu.filterName" style="width: 150px"
                  @change="menuSearch" @search="menuSearch" />
              </template>
              <template #tools>
                <a-space>
                  <a-dropdown>
                    <a-button icon="plus"></a-button>
                    <a-menu slot="overlay">
                      <a-menu-item @click="groupAdd">
                        <a-icon type="snippets" />分组
                      </a-menu-item>
                      <a-menu-divider />
                      <a-menu-item @click="worksheetAdd()">
                        <a-icon type="table" />工作表
                      </a-menu-item>
                      <a-menu-item @click="listAdd()">
                        <a-icon type="ordered-list" />列表
                      </a-menu-item>
                      <a-menu-item @click="portalAdd()">
                        <a-icon type="control" />门户
                      </a-menu-item>
                      <a-menu-item @click="iframeAdd()">
                        <a-icon type="pic-right" />嵌套页
                      </a-menu-item>
                    </a-menu>
                  </a-dropdown>

                  <a-tooltip :title="menu.expand ? '关闭展开' : '展开所有'">
                    <a-button :icon="menu.expand ? 'fullscreen-exit' : 'fullscreen'" @click="menuExpand"></a-button>
                  </a-tooltip>

                  <a-dropdown>
                    <a-tooltip title="点击刷新">
                      <a-button @click="initMenu" title="刷新" icon="reload"></a-button>
                    </a-tooltip>
                  </a-dropdown>
                </a-space>
              </template>
            </vxe-toolbar>
            <vxe-table resizable border="outer" ref="xTable"
              :tree-config="{ transform: true, rowField: 'id', parentField: 'parent', reserve: true, trigger: 'row' }"
              :row-config="{ keyField: 'id', isHover: true }" :data="menu.list" @cell-click="menuClick"
              :height="menu.height">
              <template #loading>
                <a-spin></a-spin>
              </template>
              <template #empty>
                <eip-empty />
              </template>
              <vxe-column field="text" title="名称" tree-node fixed="left" showOverflow="ellipsis">
                <template #default="{ row }">
                  <a-icon v-if="row.icon" :type="row.icon" /><span>{{ row.title }}</span><a-icon
                    v-if="!row.extend.isShowMenu" type="eye-invisible" /><span class="margin-left-xs" v-html="row.text"
                    style="cursor: pointer;"></span>

                  <!-- <span v-if="menu.list.filter(f => f.parent == row.id).length != 0">（{{ menu.list.filter(f => f.parent ==
                    row.id).length }}）</span> -->
                </template>
              </vxe-column>
              <!-- <vxe-column field="agileMenuType" title="类型" align="center" width="50">
                <template v-slot="{ row }">
                  <a-tag color="#f50" v-if="row.agileMenuType == eipAgileMenuType.group">
              组
            </a-tag>
            <a-tag color="#2db7f5" v-else-if="row.agileMenuType == eipAgileMenuType.worksheet">
              表
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.iframe">
              嵌
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.custom">
              自
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.list">
              列
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.form">
              单
            </a-tag>
            <a-tag color="#87d068" v-else-if="row.agileMenuType == eipAgileMenuType.page">
              页
            </a-tag>
            <a-tag color="#108ee9" v-else>
              系
            </a-tag>
                </template>
              </vxe-column> -->
              <vxe-column field="type" title="操作" align="center" width="50">
                <template #default="{ row }">
                  <a-dropdown>
                    <a-icon type="ellipsis"></a-icon>
                    <a-menu slot="overlay">
                      <a-menu-item @click="groupAdd(row)">
                        <a-icon type="snippets" />分组
                      </a-menu-item>
                      <a-menu-divider />
                      <a-menu-item @click="worksheetAdd(row)">
                        <a-icon type="table" />工作表
                      </a-menu-item>
                      <a-menu-item @click="listAdd(row)">
                        <a-icon type="ordered-list" />列表
                      </a-menu-item>

                      <a-menu-item @click="pageAdd(row)">
                        <a-icon type="pic-right" />页面
                      </a-menu-item>
                      <a-menu-item @click="portalAdd(row)">
                        <a-icon type="control" />门户
                      </a-menu-item>
                      <a-menu-item @click="iframeAdd(row)">
                        <a-icon type="pic-right" />嵌套页
                      </a-menu-item>
                      <a-menu-divider />
                      <a-menu-item @click="menuEdit(row)">
                        <a-icon type="edit" />修改
                      </a-menu-item>
                      <a-menu-item @click="menuCopy(row)">
                        <a-icon type="copy" />复制
                      </a-menu-item>
                      <a-menu-item @click="menuMove(row)">
                        <a-icon type="swap" />移动
                      </a-menu-item>
                      <a-menu-item @click="menuShow(row)">
                        <a-icon :type="row.extend.isShowMenu
                          ? 'eye-invisible'
                          : 'eye'
                          " />{{ row.extend.isShowMenu ? "隐藏" : "显示" }}
                      </a-menu-item>
                      <a-menu-divider />
                      <a-menu-item @click="menuDel(row)">
                        <div style="cursor: pointer; color: red">
                          <a-icon type="delete" />
                          删除
                        </div>
                      </a-menu-item>
                    </a-menu>
                  </a-dropdown>
                </template>
              </vxe-column>
            </vxe-table>
          </a-spin>
        </a-card>
      </a-layout-sider>
      <a-layout>
        <a-layout-content :style="{
          margin: '0 5px',
          background: '#fff',
          minHeight: '280px',
        }
          ">
          <build ref="build" :mode="1"></build>

          <a-result v-if="result" title="请选择左侧菜单，或点击新增项" sub-title="">
            <template #icon>
              <img src="/images/empty.png" />
            </template>
            <template #extra>
              <a-dropdown>
                <a-button type="primary"><a-icon type="plus"></a-icon> 新增项
                </a-button>
                <a-menu slot="overlay">
                  <a-menu-item @click="groupAdd">
                    <a-icon type="snippets" />分组
                  </a-menu-item>
                  <a-menu-divider />
                  <a-menu-item @click="worksheetAdd()">
                    <a-icon type="table" />工作表
                  </a-menu-item>
                  <a-menu-item @click="listAdd()">
                    <a-icon type="ordered-list" />报表
                  </a-menu-item>
                  <a-menu-item @click="portalAdd()">
                    <a-icon type="control" />门户</a-menu-item>
                  <a-menu-item @click="iframeAdd()">
                    <a-icon type="pic-right" />嵌套页
                  </a-menu-item>
                </a-menu>
              </a-dropdown>
            </template>
          </a-result>
        </a-layout-content>
      </a-layout>
    </a-layout>

    <group ref="group" v-if="setting.view.group.visible" :visible.sync="setting.view.group.visible"
      :title="setting.view.group.title" :menuId="setting.view.group.menuId" :parentId="setting.view.group.parentId"
      :parentName="setting.view.group.parentName" :copy="setting.view.group.copy" @save="groupSave"></group>

    <portal ref="portal" v-if="setting.view.portal.visible" :visible.sync="setting.view.portal.visible"
      :title="setting.view.portal.title" :menuId="setting.view.portal.menuId" :copy="setting.view.portal.copy"
      :parentId="setting.view.portal.parentId" :parentName="setting.view.portal.parentName" @save="portalSave">
    </portal>

    <worksheet ref="worksheet" v-if="setting.view.worksheet.visible" :visible.sync="setting.view.worksheet.visible"
      :title="setting.view.worksheet.title" :menuId="setting.view.worksheet.menuId" :copy="setting.view.worksheet.copy"
      :parentId="setting.view.worksheet.parentId" :parentName="setting.view.worksheet.parentName" @save="worksheetSave">
    </worksheet>

    <list ref="list" v-if="setting.view.list.visible" :visible.sync="setting.view.list.visible"
      :title="setting.view.list.title" :menuId="setting.view.list.menuId" :copy="setting.view.list.copy"
      :parentId="setting.view.list.parentId" :parentName="setting.view.list.parentName" @save="initMenu">
    </list>

    <page ref="page" v-if="setting.view.page.visible" :visible.sync="setting.view.page.visible"
      :title="setting.view.page.title" :menuId="setting.view.page.menuId" :copy="setting.view.page.copy"
      :parentId="setting.view.page.parentId" :parentName="setting.view.page.parentName" @save="initMenu">
    </page>

    <ifr ref="iframe" v-if="setting.view.iframe.visible" :visible.sync="setting.view.iframe.visible"
      :title="setting.view.iframe.title" :menuId="setting.view.iframe.menuId" :copy="setting.view.iframe.copy"
      :parentId="setting.view.iframe.parentId" :parentName="setting.view.iframe.parentName" @save="iframeSave">
    </ifr>

    <move ref="move" v-if="setting.view.move.visible" :visible.sync="setting.view.move.visible"
      :title="setting.view.move.title" :menuId="setting.view.move.menuId" @save="iframeSave">
    </move>
  </div>
</template>
<script>
import page from "./components/page";
import group from "./components/group";
import worksheet from "./components/worksheet";
import ifr from "./components/iframe";
import portal from "./components/portal";
import list from "./components/list";
import move from "./components/move";
import build from "./build";


import {
  menuQuery,
  menuDel,
  menuShow,
  menuAgile
} from "@/services/system/agile/app/index";
import { deleteConfirm } from "@/utils/util";
import { mapMutations } from "vuex";
export default {
  components: {
    page,
    list,
    group,
    worksheet,
    ifr,
    portal,
    build,
    move
  },
  data() {
    return {
      height: this.eipHeaderHeight() - 10 + "px",
      bodyStyle: {
        padding: 4,
      },
      result: true,
      setting: {
        view: {
          group: {
            visible: false,
            menuId: null,
            parentId: null,
            parentName: null,
            copy: false,
            title: "",
          },
          portal: {
            visible: false,
            menuId: null,
            title: null,
            parentId: null,
            parentName: null,
            copy: false,
          },
          worksheet: {
            visible: false,
            menuId: null,
            title: null,
            parentId: null,
            parentName: null,
            copy: false,
          },
          list: {
            visible: false,
            menuId: null,
            title: null,
            parentId: null,
            parentName: null,
            copy: false,
          },
          form: {
            visible: false,
            menuId: null,
            parentId: null,
            parentName: null,
            copy: false,
            title: "",
          },
          page: {
            visible: false,
            menuId: null,
            parentId: null,
            parentName: null,
            copy: false,
            title: "",
          },
          iframe: {
            visible: false,
            menuId: null,
            title: null,
            parentId: null,
            parentName: null,
            copy: false,
          },
          move: {
            visible: false,
            menuId: null,
            title: null,
          },
          build: {
            visible: false,
            menuId: null,
            title: null,
            menu: null,
          },
        }
      },

      menu: {
        height: this.eipHeaderHeight() - 61 + "px",
        loading: true,
        data: [],
        list: [],
        filterName: null,
        expand: false,
      },
    };
  },
  props: {
    visible: {
      type: Boolean,
      default: false,
    },
  },
  created() {
    this.initMenu();
  },
  methods: {
    ...mapMutations("account", ["setSystemAgile"]),
    /**
     * 菜单点击
     */
    menuClick({ row }) {
      if (row.extend.agileMenuType != this.eipAgileMenuType.group) {
        this.result = false;
        this.$refs.build.reloadSetting(row.extend);
      }
    },
    /**
     * 
     */
    menuExpand() {
      if (this.menu.expand) {
        this.$refs.xTable.clearTreeExpand()
      } else {
        this.$refs.xTable.setAllTreeExpand(true)
      }
      this.menu.expand = !this.menu.expand;
    },

    /**
     * 菜单搜索
     */
    menuSearch() {
      let that = this;
      const filterName = this.$utils.toValueString(this.menu.filterName).trim().toLowerCase()
      if (filterName) {
        const filterRE = new RegExp(filterName, 'gi')
        const searchProps = ['text']
        const rest = this.menu.data.filter(item => searchProps.some(key => that.$utils.toValueString(item[key]).toLowerCase().indexOf(filterName) > -1))
        this.menu.list = rest.map(row => {
          const item = Object.assign({}, row)
          searchProps.forEach(key => {
            item[key] = that.$utils.toValueString(item[key]).replace(filterRE, match => `<span class="keyword-lighten">${match}</span>`)
          })
          return item
        })
      } else {
        this.menu.list = this.menu.data
      }
    },

    /**
     * 添加组
     */
    groupAdd(row) {
      this.setting.view.group.visible = true;
      this.setting.view.group.menuId = null;
      this.setting.view.group.copy = false;
      if (row) {
        this.setting.view.group.parentId = row.id;
        this.setting.view.group.parentName = row.title;
      }
      this.setting.view.group.title = "新增组";
    },

    /**
     * 分组保存成功
     */
    groupSave() {
      this.initMenu();
    },
    /**
        * 添加门户页面
        */
    portalAdd(item) {
      this.setting.view.portal.parentId = item ? item.id : null;
      this.setting.view.portal.parentName = item ? item.text : null;
      this.setting.view.portal.menuId = null;
      this.setting.view.portal.copy = false;
      this.setting.view.portal.visible = true;
      this.setting.view.portal.title = "新增门户";
    },
    /**
     * 编辑
     */
    menuEdit(item) {
      //判断类型
      if (item.extend.agileMenuType == this.eipAgileMenuType.group) {
        this.setting.view.group.visible = true;
        this.setting.view.group.menuId = item.id;
        this.setting.view.group.copy = false;
        this.setting.view.group.title = "编辑组-" + item.text;
      }
      if (item.extend.agileMenuType == this.eipAgileMenuType.worksheet) {
        this.setting.view.worksheet.visible = true;
        this.setting.view.worksheet.menuId = item.id;
        this.setting.view.worksheet.copy = false;
        this.setting.view.worksheet.title = "编辑工作表-" + item.text;
      }
      if (item.extend.agileMenuType == this.eipAgileMenuType.iframe) {
        this.setting.view.iframe.visible = true;
        this.setting.view.iframe.menuId = item.id;
        this.setting.view.iframe.copy = false;
        this.setting.view.iframe.title = "编辑嵌套页-" + item.text;
      }
      if (item.extend.agileMenuType == this.eipAgileMenuType.portal) {
        this.setting.view.portal.visible = true;
        this.setting.view.portal.menuId = item.id;
        this.setting.view.portal.copy = false;
        this.setting.view.portal.title = "编辑自定义-" + item.text;
      }
      if (item.extend.agileMenuType == this.eipAgileMenuType.list) {
        this.setting.view.list.visible = true;
        this.setting.view.list.menuId = item.id;
        this.setting.view.list.copy = false;
        this.setting.view.list.title = "编辑列表-" + item.text;
      }
      if (item.extend.agileMenuType == this.eipAgileMenuType.form) {
        this.setting.view.form.visible = true;
        this.setting.view.form.menuId = item.id;
        this.setting.view.form.copy = false;
        this.setting.view.form.title = "编辑表单-" + item.text;
      }
      if (item.extend.agileMenuType == this.eipAgileMenuType.page) {
        this.setting.view.page.visible = true;
        this.setting.view.page.menuId = item.id;
        this.setting.view.page.copy = false;
        this.setting.view.page.title = "编辑页面-" + item.text;
      }
    },

    /**
     * 菜单显示
     * @param {*} item
     */
    menuShow(item) {
      let that = this;
      menuShow({ id: item.id }).then((result) => {
        if (result.code == that.eipResultCode.success) {
          that.initMenu();
        }
        that.$message.destroy();
        that.$message.success(result.msg);
      });
    },
    /**
        * 自定义保存成功
        */
    portalSave() {
      this.initMenu();
    },
    /**
     *添加工作表
     */
    worksheetAdd(item) {
      this.setting.view.worksheet.parentId = item ? item.id : null;
      this.setting.view.worksheet.parentName = item ? item.text : null;
      this.setting.view.worksheet.menuId = null;
      this.setting.view.worksheet.copy = false;
      this.setting.view.worksheet.visible = true;
      this.setting.view.worksheet.title = "新增工作表";
    },
    /**
        *添加列表配置
        */
    listAdd(item) {
      this.setting.view.list.parentId = item ? item.id : null;
      this.setting.view.list.parentName = item ? item.text : null;
      this.setting.view.list.menuId = null;
      this.setting.view.list.copy = false;
      this.setting.view.list.visible = true;
      this.setting.view.list.title = "新增列表";
    },

    pageAdd(item) {
      this.setting.view.page.parentId = item ? item.id : null;
      this.setting.view.page.parentName = item ? item.text : null;
      this.setting.view.page.menuId = null;
      this.setting.view.page.copy = false;
      this.setting.view.page.visible = true;
      this.setting.view.page.title = "新增页面";
    },

    /**
     * 复制
     * @param {*} item
     */
    menuCopy(item) {
      //判断类型
      if (item.extend.agileMenuType == this.eipAgileMenuType.group) {
        this.setting.view.group.visible = true;
        this.setting.view.group.menuId = item.id;
        this.setting.view.group.copy = true;
        this.setting.view.group.title = "复制组-" + item.text;
      }
      if (item.extend.agileMenuType == this.eipAgileMenuType.worksheet) {
        this.setting.view.worksheet.visible = true;
        this.setting.view.worksheet.menuId = item.id;
        this.setting.view.worksheet.copy = true;
        this.setting.view.worksheet.title = "复制工作表-" + item.text;
      }
      if (item.extend.agileMenuType == this.eipAgileMenuType.iframe) {
        this.setting.view.iframe.visible = true;
        this.setting.view.iframe.menuId = item.id;
        this.setting.view.iframe.copy = true;
        this.setting.view.iframe.title = "复制嵌套页-" + item.text;
      }
    },

    /**
     * 菜单移动
     */
    menuMove(item) {
      this.setting.view.move.menuId = item.id;
      this.setting.view.move.visible = true;
      this.setting.view.move.title = "移动-" + item.text;
    },

    /**
     * 添加工作表保存成功
     */
    worksheetSave() {
      this.initMenu();
    },

    /**
     *添加框架页
     */
    iframeAdd(item) {
      this.setting.view.iframe.parentId = item ? item.id : null;
      this.setting.view.iframe.parentName = item ? item.text : null;
      this.setting.view.iframe.menuId = null;
      this.setting.view.iframe.copy = false;
      this.setting.view.iframe.visible = true;
      this.setting.view.iframe.title = "新增嵌套页";
    },

    /**
     * 添加框架页保存成功
     */
    iframeSave() {
      this.initMenu();
    },

    /**
     * 树删除
     */
    menuDel(item) {
      let that = this;
      deleteConfirm(
        "确定删除【" + item.text + "】",
        function () {
          that.$message.loading("删除中...", 0);
          menuDel({ id: item.id }).then((result) => {
            if (result.code == that.eipResultCode.success) {
              that.initMenu();
            }
            that.$message.destroy();
            that.$message.success(result.msg);
          });
        },
        that
      );
    },

    /**
     * 初始化菜单
     */
    initMenu() {
      let that = this;
      that.menu.loading = true;
      menuQuery().then((result) => {
        that.menu.data = result;
        that.menu.loading = false;
        that.menuSearch();
      });

      menuAgile().then(function (result) {
        if (result.code === that.eipResultCode.success) {
          that.setSystemAgile(result.data);
        }
      });
    },
  },
};
</script>
<style lang="less" scoped>
/deep/ .ant-layout-header {
  line-height: 48px !important;
  height: 48px !important;
}

.avatar {
  color: #fff;
}

#components-layout-demo-custom-trigger .trigger:hover {
  color: @primary-color;
}

.header-item:hover {
  background-color: @primary-color;
}

.pointer {
  cursor: pointer;
}

/deep/ .ant-tabs-bar {
  margin: 0 !important;
}

.ant-dropdown-menu {
  width: 150px;
}

.ant-layout-content {
  background: #f5f5f9;
}

/deep/ .ant-layout-sider {
  background: #fff !important;
}

/deep/ .ant-menu-inline {
  border-right: 0 !important;
}

.keyword-lighten {
  color: #000;
  background-color: #FFFF00;
}
</style>