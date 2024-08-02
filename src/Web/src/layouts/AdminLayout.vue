<template>
  <a-layout :class="['admin-layout', 'beauty-scroll']">
    <drawer v-if="isMobile" v-model="drawerOpen">
      <side-menu
        :theme="theme.mode"
        :menuData="menuData"
        :collapsed="false"
        :collapsible="false"
        :systemLogo="systemLogo"
        :systemTitle="systemTitle"
        :systemShortName="systemShortName"
        @menuSelect="onMenuSelect"
      />
    </drawer>
    <side-menu
      :systemLogo="systemLogo"
      :systemShortName="systemShortName"
      :systemTitle="systemTitle"
      :class="[fixedSideBar ? 'fixed-side' : '']"
      :theme="theme.mode"
      v-else-if="layout === 'side' || layout === 'mix'"
      :menuData="sideMenuData"
      :collapsed="collapsed"
      :collapsible="true"
    />
    <div
      v-if="fixedSideBar && !isMobile"
      :style="`width: ${sideMenuWidth}; min-width: ${sideMenuWidth};max-width: ${sideMenuWidth};`"
      class="virtual-side"
    ></div>
    <drawer v-if="!hideSetting" v-model="showSetting" placement="right">
      <div class="setting" slot="handler">
        <a-icon v-if="showSetting" type="close" />
        <a-icon v-if="!showSetting" type="setting" spin :rotate="180" />
      </div>
      <setting />
    </drawer>
    <a-layout class="admin-layout-main beauty-scroll">
      <admin-header
        :class="[
          {
            'fixed-tabs': fixedTabs,
            'fixed-header': fixedHeader,
            'multi-page': multiPage,
          },
        ]"
        :style="headerStyle"
        :menuData="headMenuData"
        :breadcrumb="headBreadCrumb"
        :collapsed="collapsed"
        :systemLogo="systemLogo"
        :systemTitle="systemTitle"
        :systemShortName="systemShortName"
        @toggleCollapse="toggleCollapse"
      />
      <a-layout-header
        :class="[
          'virtual-header',
          {
            'fixed-tabs': fixedTabs,
            'fixed-header': fixedHeader,
            'multi-page': multiPage,
          },
        ]"
        v-show="fixedHeader"
      ></a-layout-header>
      <a-layout-content
        class="admin-layout-content"
        :style="`min-height: ${minHeight}px;`"
      >
        <div style="position: relative">
          <slot></slot>
        </div>
      </a-layout-content>
    </a-layout>
  </a-layout>
</template>

<script>
import AdminHeader from "./header/AdminHeader";
import Drawer from "../components/tool/Drawer";
import SideMenu from "../components/menu/SideMenu";
import Setting from "../components/setting/Setting";
import { mapState, mapMutations, mapGetters } from "vuex";
import { getI18nKey } from "@/utils/routerUtil";
import { header } from "@/services/system/config/index";
export default {
  name: "AdminLayout",
  components: { Setting, SideMenu, Drawer, AdminHeader },
  data() {
    return {
      minHeight: window.innerHeight - 102,
      collapsed: false,
      showSetting: false,
      drawerOpen: false,

      systemTitle: "",
      systemShortName: "",
      systemLogo: "",
    };
  },
  provide() {
    return {
      adminLayout: this,
    };
  },
  watch: {
    $route(val) {
      this.setActivated(val);
    },
    layout() {
      this.setActivated(this.$route);
    },
    isMobile(val) {
      if (!val) {
        this.drawerOpen = false;
      }
    },
  },
  computed: {
    ...mapState("setting", [
      "isMobile",
      "theme",
      "layout",
      "footerLinks",
      "copyright",
      "fixedHeader",
      "fixedSideBar",
      "fixedTabs",
      "hideSetting",
      "multiPage",
      "systemConfig",
    ]),
    ...mapGetters("setting", [
      "firstMenu",
      "subMenu",
      "menuData",
      "systemConfig",
    ]),
    ...mapGetters("account", ["user"]),
    sideMenuWidth() {
      return this.collapsed ? "48px" : "200px";
    },
    headerStyle() {
      let width =
        this.fixedHeader && this.layout !== "head" && !this.isMobile
          ? `calc(100% - ${this.sideMenuWidth})`
          : "100%";
      let position = this.fixedHeader ? "fixed" : "static";
      return `width: ${width}; position: ${position};`;
    },
    headBreadCrumb() {
      let routes = this.$route.matched;
      let breadcrumb = [];
      routes.forEach((route) => {
        if (route.path.length !== 0) {
          breadcrumb.push(this.$t(getI18nKey(route.path)));
        }
      });
      let pageTitle = this.page && this.page.title;
      if (this.customTitle || pageTitle) {
        breadcrumb[breadcrumb.length - 1] = this.customTitle || pageTitle;
      }
      return breadcrumb;
    },
    headMenuData() {
      const { layout, menuData, firstMenu } = this;
      return layout === "mix" ? firstMenu : menuData;
    },
    sideMenuData() {
      const { layout, menuData, subMenu } = this;
      return layout === "mix" ? subMenu : menuData;
    },
  },
  methods: {
    ...mapMutations("account", ["setUser"]),
    ...mapMutations("setting", [
      "correctPageMinHeight",
      "setActivatedFirst",
      "setSystemName",
      "setSystemConfig",
    ]),
    toggleCollapse() {
      this.collapsed = !this.collapsed;
    },

    onMenuSelect() {
      this.toggleCollapse();
    },

    setActivated(route) {
      if (this.layout === "mix") {
        let matched = route.matched;
        matched = matched.slice(0, matched.length - 1);
        const { firstMenu } = this;
        for (let menu of firstMenu) {
          if (matched.findIndex((item) => item.path === menu.fullPath) !== -1) {
            this.setActivatedFirst(menu.fullPath);
            break;
          }
        }
      }
    },

    /**
     * 获取系统配置
     */
    initConfig() {
      let that = this;
      header().then((result) => {
        if (result.code === this.eipResultCode.success) {
          let data = result.data;
          that.setSystemConfig(data);
          that.systemTitle = data.systemTitle;
          that.systemShortName = data.systemShortName;
          that.systemLogo = data.systemLogo;
          that.setSystemName(that.systemTitle);
        } else {
          that.$message.error(result.msg);
        }
      });
    },
  },
  created() {
    this.correctPageMinHeight(this.minHeight - 24);
    this.setActivated(this.$route);
    this.initConfig();
  },
  beforeDestroy() {
    this.correctPageMinHeight(-this.minHeight + 24);
  },
};
</script>

<style lang="less" scoped>
.admin-layout {
  .side-menu {
    &.fixed-side {
      position: fixed;
      height: 100vh;
      left: 0;
      top: 0;
    }
  }

  .virtual-side {
    transition: all 0.2s;
  }

  .virtual-header {
    transition: all 0.2s;
    opacity: 0;

    &.fixed-tabs.multi-page:not(.fixed-header) {
      height: 0;
    }
  }

  .admin-layout-main {
    .admin-header {
      top: 0;
      right: 0;
      overflow: hidden;
      transition: all 0.2s;

      &.fixed-tabs.multi-page:not(.fixed-header) {
        height: 0;
      }
    }
  }

  .admin-layout-content {
    padding: 25px 5px 5px;
    /*overflow-x: hidden;*/
    /*min-height: calc(100vh - 64px - 122px);*/
  }

  .setting {
    background-color: @primary-color;
    color: @base-bg-color;
    border-radius: 5px 0 0 5px;
    line-height: 40px;
    font-size: 22px;
    width: 40px;
    height: 40px;
    box-shadow: -2px 0 8px @shadow-color;
  }
}
</style>
