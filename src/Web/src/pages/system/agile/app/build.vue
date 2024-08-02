<template>
  <div>
    <a-card v-if="(form.agileMenuType == eipAgileMenuType.worksheet ||
      form.agileMenuType == eipAgileMenuType.list ||
      form.agileMenuType == eipAgileMenuType.page) &&
      form.openType == 0" :loading="loading" :headStyle="{ borderBottom: '0', paddingRight: '13px' }"
      :bodyStyle="{ padding: 0 }" :bordered="false" size="small">
      <template slot="extra"> </template>
      <div style="display: flex; align-items: flex-start; justify-content: center">
        <div style="width: 100%" v-if="mode == 1 || (mode == 0 && list.datas.length > 1)">
          <div
            style="font-size: 17px;color: #333;font-weight: 700; height: 48px;line-height: 48px;padding-left:10px;border-bottom: 1px solid #f0f0f0;">
            <a-icon :type="form.icon"></a-icon><span style="margin-left: 10px">{{ form.name }}</span>

            <a-space class="margin-left">
              <a-tooltip v-if="form.agileMenuType == eipAgileMenuType.list" title="数据源">
                <a-button type="primary" @click="designerListDataSource" size="small"> <a-icon
                    type="database" />数据源</a-button>
              </a-tooltip>
              <a-tooltip
                v-if="form.agileMenuType == eipAgileMenuType.worksheet || form.agileMenuType == eipAgileMenuType.page"
                title="表单">
                <a-button type="primary" @click="designerForm" size="small"> <a-icon type="form" />表单</a-button>
              </a-tooltip>
              <a-tooltip
                v-if="form.agileMenuType == eipAgileMenuType.worksheet || form.agileMenuType == eipAgileMenuType.list"
                title="列表">
                <a-button type="primary" @click="designerView" size="small"> <a-icon type="table" />列表</a-button>
              </a-tooltip>
              <a-tooltip v-if="form.agileMenuType == eipAgileMenuType.worksheet" title="流程">
                <a-button type="primary" @click="designerWorkflow" size="small"> <a-icon type="branches" />流程</a-button>
              </a-tooltip>
              <a-tooltip
                v-if="form.agileMenuType == eipAgileMenuType.worksheet || form.agileMenuType == eipAgileMenuType.list"
                title="按钮">
                <a-button type="primary" @click="buttonEdit" size="small"> <a-icon type="appstore" />按钮</a-button>
              </a-tooltip>

              <a-tooltip
                v-if="form.agileMenuType == eipAgileMenuType.worksheet || form.agileMenuType == eipAgileMenuType.page"
                title="自动化">
                <a-button type="primary" @click="automation" size="small"> <a-icon type="fork" />自动化</a-button>
              </a-tooltip>
            </a-space>
          </div>

          <designer-form ref="designerForm" v-if="setting.view.form.visible" :visible.sync="setting.view.form.visible"
            :title="setting.view.form.title" :configId="setting.view.form.configId" :publicField="true"
            @close="designerFormClose"></designer-form>

          <designer-list ref="designerList" v-if="setting.view.list.visible" :visible.sync="setting.view.list.visible"
            :title="form.name" :configId="configId" @public="listPublic"></designer-list>

          <designer-list-data-source ref="designerListDataSource" v-if="setting.view.listDataSource.visible"
            :visible.sync="setting.view.listDataSource.visible" :title="form.name" :configId="configId"
            @ok="listPublic"></designer-list-data-source>

          <designer-workflow ref="designerWorkflow" v-if="setting.view.workflow.visible"
            :visible.sync="setting.view.workflow.visible" :title="setting.view.workflow.title" :processId="form.menuId"
            :agile="setting.view.workflow.agile"></designer-workflow>

        </div>
      </div>
      <div :key="item.id" v-for="item in list.datas">
        <run-list
          v-if="(form.agileMenuType == eipAgileMenuType.worksheet || form.agileMenuType == eipAgileMenuType.list) && form.openType == 0"
          ref="runList" :name="form.name" :configId="item.configId" :menuId="form.menuId"
          :haveDataPermission="form.haveDataPermission"
          :haveCard="mode == 1 || (mode == 0 && list.datas.length > 1)"></run-list>
      </div>
    </a-card>

    <a-card :bordered="false" class="eip-admin-card-small" v-if="form.agileMenuType == eipAgileMenuType.portal"
      :loading="loading" :headStyle="{ borderBottom: '0', paddingRight: '13px' }" :bodyStyle="{ padding: 0 }"
      size="small">

      <div slot="title" v-if="mode == 1">
        <a-button @click="setting.view.portal.visible = true" type="primary"> <a-icon
            type="setting"></a-icon>自定义界面</a-button>
      </div>

      <run-portal ref="runCustom" v-if="setting.view.portal.configId" :key="setting.view.portal.key"
        :configId="setting.view.portal.configId"></run-portal>

      <designer-portal v-if="setting.view.portal.visible" :title="form.name" :configId="setting.view.portal.configId"
        ref="designerCustom" :visible.sync="setting.view.portal.visible" @public="listPublic"></designer-portal>
    </a-card>

    <run-iframe v-if="form.agileMenuType == eipAgileMenuType.iframe" :key="form.menuId"
      :url="form.iframePath"></run-iframe>

    <run-page v-if="form.agileMenuType == eipAgileMenuType.page && setting.view.page.configId" :menuId="form.menuId"
      :config="{ editConfigId: setting.view.page.configId }" :height="mode == 1 ? 200 : 154"
      :options="setting.view.page.options" :update="setting.view.page.update"></run-page>

    <button-list v-if="setting.view.buttonList.visible" :visible.sync="setting.view.buttonList.visible"
      :menuId="form.menuId" :title="setting.view.buttonList.title" @confirm="buttonConfirm"></button-list>

    <automation-list v-if="setting.view.automation.visible" :visible.sync="setting.view.automation.visible"
      :tableId="setting.view.automation.tableId" :title="setting.view.automation.title"></automation-list>
  </div>
</template>
<script>
import { findByMenuId } from "@/services/system/agile/app/designer";
import { findById } from "@/services/system/menu/edit";

import runPage from "@/pages/system/agile/run/page";
import runList from "@/pages/system/agile/run/list";
import runIframe from "@/pages/system/agile/run/iframe";
import runPortal from "@/pages/system/agile/run/portal";

import designerForm from "@/pages/system/agile/form/designer";
import designerList from "@/pages/system/agile/list/designer";
import designerPortal from "@/pages/system/agile/portal/designer";
import designerWorkflow from "@/pages/workflow/process/designer";

import designerListDataSource from "@/pages/system/agile/list/edit";

import buttonList from "@/pages/system/agile/app/components/button/index";
import automationList from "@/pages/system/agile/app/components/automation/index";
import {
  queryPage,
} from "@/services/system/agile/run/list";
import { newGuid } from "@/utils/util";
import { mapMutations, mapGetters } from "vuex";
export default {
  components: {
    runPage,
    runIframe,
    runList,
    runPortal,
    designerForm,
    designerList,
    designerPortal,
    designerListDataSource,
    designerWorkflow,
    buttonList,
    automationList,
  },
  name: "build",
  computed: {
    ...mapGetters("account", ["systemAgile"]),
  },
  data() {
    return {
      loading: true,
      configId: "",
      table: {
        page: {
          param: {
            current: 1,
            size: this.eipPage.size,
            sord: "asc",
            sidx: "OrderNo",
            filters: "",
          },
          total: 1000,
          sizeOptions: this.eipPage.sizeOptions,
        },
        loading: true,
        data: [],
        height: window.innerHeight - 318,
        search: {
          option: {
            labelCol: 8,
            wrapperCol: 16,
            num: 4,
            config: [],
          },
        },
      },

      setting: {
        view: {
          form: {
            visible: false,
            title: "",
            configId: ""
          },
          page: {
            configId: "",
            options: null,
            update: {
              RelationId: undefined,
            }
          },
          list: {
            visible: false,
            title: ""
          },

          automation: {
            visible: false,
            title: "",
            tableId: ""
          },
          listDataSource: {
            visible: false,
            title: ""
          },

          buttonList: {
            visible: false,
            title: "",
          },
          portal: {
            visible: false,
            title: "",
            configId: undefined,
            key: null
          },
          workflow: {
            visible: false,
            title: "",
            agile: {
              configId: "",
              name: "",
              icon: "",
              theme: "",
              useType: 1,
            },
          },

        },

        menu: [],
      },

      form: {
        menuId: null,
        name: null,
        icon: null,
        openType: null,
        iframePath: null,
        agileMenuType: null,
        haveDataPermission: false
      },

      list: {
        visible: false,
        datas: [],
      },
    };
  },
  props: {
    mode: {
      type: Number,
      default: 0,
    }, //模式:0操作，1
  },
  created() {
    const route = this.$route;
    if (this.mode == 0) {
      let that = this;
      that.$loading.show({ text: "加载中,请稍等...", background: 'none' });
      let menuId = route.meta.menuId;
      findById(menuId).then(function (result) {
        that.reloadSetting(result.data);
      });
    } else {
      this.hideLoading();
    }
  },
  methods: {

    /**
     * 列表发布
     */
    listPublic() {
      let that = this;
      that.reloadSetting(that.setting.menu);
    },

    /**
     *
    */
    designerFormClose() {
      this.setting.view.form.visible = false;
    },

    /**
     * 编辑
     */
    designerForm() {
      let that = this;
      that.$loading.show({ text: "加载中,请稍等..." });
      findByMenuId({ menuId: this.form.menuId, configType: 2 }).then(function (
        result
      ) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.setting.view.form.configId = result.data[0].configId;
          that.setting.view.form.title = result.data[0].name
          that.setting.view.form.visible = true;
        } else {
          that.$message.error(result.msg);
        }
      });
    },

    /**
     * 
     */
    designerWorkflow() {
      let that = this;
      that.$loading.show({ text: "加载中,请稍等..." });
      findByMenuId({ menuId: this.form.menuId, configType: 2 }).then(function (
        result
      ) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.setting.view.workflow.agile = result.data[0];
          that.setting.view.workflow.title = result.data[0].name
          that.setting.view.workflow.visible = true;
        } else {
          that.$message.error(result.msg);
        }
      });
    },
    /**
     * 按钮
     */
    buttonConfirm() {
      this.$refs.runList[0].initButton();
    },

    /**
     * 重载配置
     */
    reloadSetting(menu) {
      let that = this;
      this.setting.menu = menu;
      this.form = {
        menuId: menu.menuId,
        name: menu.name,
        icon: menu.icon,
        openType: menu.openType,
        iframePath: menu.iframePath,
        agileMenuType: menu.agileMenuType,
        haveDataPermission: menu.haveDataPermission
      };

      //若是表则加载对应配置项
      if (menu.agileMenuType == this.eipAgileMenuType.worksheet) {
        findByMenuId({ menuId: this.form.menuId, configType: 1 }).then(
          function (result) {
            that.hideLoading();
            if (result.code === that.eipResultCode.success) {
              result.data.forEach(item => {
                item.id = newGuid();
              })
              that.list.datas = result.data;
              that.configId = that.list.datas[0].configId;
              that.$forceUpdate()
            } else {
              that.$message.error(result.msg);
            }
          }
        );
      }

      //若是iframe
      else if (menu.agileMenuType == this.eipAgileMenuType.iframe) {
        that.hideLoading();
      }
      //类型是自定义界面
      else if (menu.agileMenuType == this.eipAgileMenuType.portal) {
        findByMenuId({ menuId: this.form.menuId, configType: 4 }).then(
          function (result) {
            that.hideLoading();
            if (result.code === that.eipResultCode.success) {
              that.setting.view.portal.configId = result.data[0].configId;
              that.setting.view.portal.key = Date.now()
            } else {
              that.$message.error(result.msg);
            }
          }
        );
      }
      //列表
      else if (menu.agileMenuType == this.eipAgileMenuType.list) {
        findByMenuId({ menuId: this.form.menuId, configType: 1 }).then(
          function (result) {
            that.hideLoading();
            if (result.code === that.eipResultCode.success) {
              result.data.forEach(item => {
                item.id = newGuid();
              })
              that.list.datas = result.data;
              if (that.list.datas.length > 0) {
                that.configId = that.list.datas[0].configId;
              } else {
                that.configId = null;
              }
              that.$forceUpdate()
            } else {
              that.$message.error(result.msg);
            }
          }
        );
      }

      //页面
      else if (menu.agileMenuType == this.eipAgileMenuType.page) {
        findByMenuId({ menuId: this.form.menuId, configType: 2 }).then(
          function (result) {
            that.hideLoading();
            if (result.code === that.eipResultCode.success) {
              if (result.data.length > 0) {

                var systemAgileData = that.systemAgile.filter(f => f.configId == result.data[0].configId);
                if (systemAgileData && systemAgileData.length > 0) {
                  let editConfig = systemAgileData[0];
                  var publicJson = JSON.parse(editConfig.publicJson);
                  that.setting.view.page.options = publicJson.config;
                  //获取数据
                  queryPage({
                    configId: result.data[0].configId
                  }).then(function (queryResult) {
                    if (queryResult.data.length > 0) {
                      that.setting.view.page.update.RelationId = queryResult.data[0].RelationId;
                    }
                    that.setting.view.page.configId = result.data[0].configId;
                  });
                }
              } else {
                that.setting.view.page.configId = null;
              }
              that.$forceUpdate()
            } else {
              that.$message.error(result.msg);
            }
          }
        );
      }
      else {
        that.hideLoading();
      }
    },

    hideLoading() {
      this.loading = false;
      this.table.loading = false;
      this.$message.destroy();
      this.$loading.hide();
    },

    /**
     *
     * @param {*} item
     */
    designerView() {
      if (this.configId) {
        this.setting.view.list.visible = true;
      }
    },

    /**
     * 
     */
    designerListDataSource() {
      this.setting.view.listDataSource.visible = true;
    },

    /**
     * 自定义按钮
     */
    buttonEdit() {
      if (this.configId) {
        this.setting.view.buttonList.visible = true;
        this.setting.view.buttonList.title = "自定义按钮";
      }
    },

    /**
     * 
     */
    automation() {
      let that = this;
      that.$loading.show({ text: "加载中,请稍等..." });
      findByMenuId({ menuId: this.form.menuId, configType: 2 }).then(function (
        result
      ) {
        that.$loading.hide();
        if (result.code === that.eipResultCode.success) {
          that.setting.view.automation.tableId = result.data[0].configId;
          that.setting.view.automation.title = result.data[0].name
          that.setting.view.automation.visible = true;
        } else {
          that.$message.error(result.msg);
        }
      });
    },
  },
};
</script>

<style lang="less" scoped>
.tab-add {
  width: 61px;
  line-height: 17px;
  padding: 11px 10px 7px 10px;
  text-align: center;
  border-bottom: 1px solid #f0f0f0;
}

.pointer {
  cursor: pointer;
}

.ant-dropdown-menu {
  width: 150px;
}

/deep/ .ant-tabs-bar {
  margin: 0;
}
</style>