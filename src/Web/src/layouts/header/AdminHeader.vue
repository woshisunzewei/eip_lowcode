<template>
  <a-layout-header :class="[headerTheme, 'admin-header']">
    <div :class="['admin-header-wide', layout, pageWidth]">
      <router-link
        v-if="isMobile || layout === 'head'"
        :to="to"
        :class="['logo', isMobile ? null : 'pc', headerTheme]"
      >
        <img style="margin-left: 10px" v-if="systemLogo" :src="systemLogo" />
        <h1 v-if="!isMobile" style="margin-left: 10px">{{ systemTitle }}</h1>
        <div v-if="isMobile">
          <h1 style="font-size: 20px">{{ systemShortName }}</h1>
        </div>
      </router-link>
      <a-icon
        v-if="layout !== 'head' && !isMobile"
        class="trigger"
        :type="collapsed ? 'menu-unfold' : 'menu-fold'"
        @click="toggleCollapse"
      />
      <div class="admin-header-menu" v-if="layout == 'side'">
        <a-breadcrumb>
          <a-breadcrumb-item :key="index" v-for="(item, index) in breadcrumb">
            <span>{{ item }}</span>
          </a-breadcrumb-item>
        </a-breadcrumb>
      </div>
      <div
        v-if="layout !== 'side' && !isMobile"
        class="admin-header-menu"
        :style="`width: ${menuWidth};`"
      >
        <i-menu
          class="head-menu"
          :theme="headerTheme"
          mode="horizontal"
          :options="menuData"
          @select="onSelect"
        />
      </div>
      <div :class="['admin-header-right', headerTheme]">
        <div class="header-item" @click="fullscreen">
          <a-icon :type="fullScreenIcon" />
        </div>

        <header-short-cut
          ref="headerShortCut"
          class="header-item"
          @shortCut="shortCutModal"
        />

        <header-notice ref="headerNotice" class="header-item" />

        <header-avatar @connectionClose="connectionClose" class="header-item" />
      </div>
    </div>

    <short-cut
      ref="shortcut"
      v-if="shortcut.visible"
      :visible.sync="shortcut.visible"
      :title="shortcut.title"
      @ok="shortCutOk"
    ></short-cut>
  </a-layout-header>
</template>

<script>
import HeaderNotice from "./HeaderNotice";
import ShortCut from "@/pages/system/user/shortcut";
import HeaderAvatar from "./HeaderAvatar";
import HeaderShortCut from "./HeaderShortCut";
import IMenu from "@/components/menu/menu";
import { mapState, mapMutations, mapGetters } from "vuex";
export default {
  name: "AdminHeader",
  components: {
    IMenu,
    HeaderAvatar,
    HeaderShortCut,
    ShortCut,
    HeaderNotice,
  },
  props: [
    "collapsed",
    "menuData",
    "breadcrumb",
    "systemTitle",
    "systemShortName",
    "systemLogo",
  ],
  data() {
    return {
      to: "/",
      shortcut: {
        visible: false,
        title: "",
      },
      isFullscreen: false,
      fullScreenIcon: "fullscreen",
    };
  },

  computed: {
    ...mapState("setting", [
      "theme",
      "isMobile",
      "layout",
      "lang",
      "pageWidth",
    ]),
    ...mapGetters("account", ["routesConfig"]),
    /**
     *
     */
    headerTheme() {
      if (
        this.layout == "side" &&
        this.theme.mode == "dark" &&
        !this.isMobile
      ) {
        return "light";
      }
      return this.theme.mode;
    },

    /**
     * 菜单宽度
     */
    menuWidth() {
      const { layout } = this;
      //得到右侧宽度
      var width = this.systemTitle.length * 20;
      width = this.systemLogo ? width + 42 : width;
      const headWidth =
        layout === "head"
          ? "100% - " + (width > 200 ? width : 200) + "px"
          : layout === "mix"
          ? "100% - 70px"
          : "100%";
      const extraWidth = "320px";
      return `calc(${headWidth} - ${extraWidth})`;
    },
  },

  created() {
    this.routerTo();
  },

  methods: {
    /**
     * 得到第一个路由地址
     */
    routerTo() {
      var routesConfig = this.routesConfig;
      var routes = [];
      // 递归遍历控件树
      const traverse = (array) => {
        array.forEach((element) => {
          if (element.path) {
            routes.push(element);
          } else {
            // 栅格布局 and 标签页
            element.children.forEach((item) => {
              if (item.path) {
                routes.push(item);
              } else {
                traverse(item.children);
              }
            });
          }
        });
      };
      traverse(routesConfig);
      this.to = routes[0].path;
    },
    /**
     * 连接关闭
     */
    connectionClose() {
      this.$refs.headerNotice.connectionStop();
    },

    /**
     * 快捷方式
     */
    shortCutModal() {
      this.shortcut.visible = true;
      this.shortcut.title = "快捷方式";
    },

    /**
     * 保存完毕,重新刷新快捷方式
     */
    shortCutOk() {
      this.$refs.headerShortCut.init();
    },
    /**
     * 全屏
     */
    fullscreen() {
      if (!document.fullscreenElement) {
        //进入页面全屏
        document.documentElement.requestFullscreen();
      } else {
        if (document.exitFullscreen) {
          //退出全屏
          document.exitFullscreen();
        }
      }
      this.isFullscreen = !this.isFullscreen;
      this.fullScreenIcon = this.isFullscreen
        ? "fullscreen-exit"
        : "fullscreen";
    },
    toggleCollapse() {
      this.$emit("toggleCollapse");
    },
    onSelect(obj) {
      this.$emit("menuSelect", obj);
    },
    ...mapMutations("setting", ["setLang"]),
  },
};
</script>

<style lang="less" scoped>
@import "index";
</style>
