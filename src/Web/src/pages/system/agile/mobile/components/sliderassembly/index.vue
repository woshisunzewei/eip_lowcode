<template>
  <div class="sliderassembly">
    <a-collapse @change="collapseChange" :defaultActiveKey="collapseDefaultActiveKey">
      <a-collapse-panel :header="items.title" :key="index" v-for="(items, index) in datas">
        <div class="componList" draggable="true" @dragstart="drag($event)" @dragend="dragends($event)"
          :data-name="item.name" v-for="(item, ind) in items.comList" :key="ind">
          <i class="iconfont" :class="item.icon" v-if="item.icon" />
          <van-icon :name="item.vanIcon" v-else />
          <p>{{ item.text }}</p>
        </div>
      </a-collapse-panel>
    </a-collapse>
  </div>
</template>

<script>
export default {
  name: "sliderassembly",
  props: {
    pointer: Object,
  },
  data() {
    return {
      activeNames: [0, 1] /* 侧边栏组件显示 */,
      datas: [
        {
          title: "基础组件",
          comList: [
            // {
            //   text: "商品搜索",
            //   type: "1-1",
            //   icon: "icon-shangpinsousuo",
            //   name: "commoditysearch",
            // },
            {
              text: "标题文本",
              type: "1-3",
              icon: "icon-Component-biaotiwenzi",
              name: "captiontext",
            },

            {
              text: "图片广告",
              type: "1-3",
              icon: "icon-tupianguanggao",
              name: "pictureads",
            },
            {
              text: "图文导航",
              type: "1-4",
              icon: "icon-icon_tupiandaohang",
              name: "graphicnavigation",
            },
            {
              text: "底部导航",
              type: "1-5",
              icon: "icon-daohang",
              name: "tabBar",
            },
            {
              text: "魔方",
              type: "1-6",
              icon: "icon-mofang",
              name: "magiccube",
            },
            {
              text: "公告",
              type: "1-7",
              icon: "icon-gonggao",
              name: "notice",
            },

            {
              text: "视频",
              type: "1-8",
              icon: "icon-shipin",
              name: "videoss",
            },
            {
              text: "富文本",
              type: "1-10",
              icon: "icon-fuwenben",
              name: "richtext",
            },
            {
              text: "辅助分割",
              type: "1-11",
              icon: "icon-Component-fuzhufenge",
              name: "auxiliarysegmentation",
            },

            // {
            //   text: "店铺信息",
            //   type: "1-12",
            //   icon: "icon-dianpuxinxi",
            //   name: "storeinformation",
            // },
            {
              text: "单元格",
              type: "1-13",
              icon: "icon-jinrudianpu",
              name: "entertheshop",
            },
            // {
            //   text: "社群涨粉",
            //   type: "1-14",
            //   icon: "icon-kuaisuzhangfen",
            //   name: "communitypowder",
            // },
            /* {
              text: 'xxx',
              type: '1-17',
              icon: 'icon-yunying',
              name: ''
            }, */
            /* {
              text: 'xxx',
              type: '1-19',
              icon: 'icon-weibiaoti-_huaban',
              name: ''
            }, */
            /* {
              text: 'xxxx',
              type: '1-18',
              icon: 'icon-gexinghuatuijian',
              name: ''
            }, */
            {
              text: "关注公众号",
              type: "1-15",
              icon: "icon-gongzhonghao",
              name: "follow",
            },
            {
              text: "悬浮",
              type: "1-16",
              icon: "icon-wangye",
              name: "suspension",
            },
            // {
            //   text: "自定义模块",
            //   type: "demo",
            //   icon: "icon-zidingyimokuai",
            //   name: "custommodule",
            // },
          ],
        },
        {
          title: "工作流",
          comList: [
            // {
            //   text: "商品",
            //   type: "2-1",
            //   icon: "icon-goods",
            //   name: "listswitching",
            // },
            {
              text: "文章",
              type: "2-2",
              icon: "icon-dianpubijikapian",
              name: "storenotecard",
            },
            {
              text: "表单",
              type: "2-3",
              vanIcon: "orders-o",
              name: "investigate",
            },
            {
              text: "工作流",
              type: "2-4",
              vanIcon: "cluster-o",
              name: "workflow",
            },
            {
              text: "模块",
              type: "2-5",
              vanIcon: "newspaper-o",
              name: "mobilemenu",
            },
            {
              text: "报表",
              type: "2-6",
              vanIcon: "chart-trending-o",
              name: "report",
            },
          ],
        }
      ],
    };
  },
  computed: {
    collapseDefaultActiveKey() {
      // 计算当前展开的控件列表
      const defaultActiveKey = window.localStorage.getItem(
        "collapseDefaultActiveKey"
      );
      if (defaultActiveKey) {
        return defaultActiveKey.split(",");
      } else {
        return ["0"];
      }
    },
  },
  methods: {
    /**
     * 当用户开始拖动元素或选择文本时触发此事件
     *
     * @param {Object} event event对象
     */
    drag(event) {
      /* 开启穿透 */
      this.pointer.show = true;
      /* 传递参数 */
      event.dataTransfer.setData("componentName", event.target.dataset.name);
    },

    /**
     * 当拖动操作结束时（释放鼠标按钮或按下退出键），会触发此事件
     *
     * @param {Object} event event对象
     */
    dragends() {
      /* 关闭穿透 */
      this.pointer.show = false;
    },

    collapseChange(val) {
      // 点击collapse时，保存当前collapse状态
      window.localStorage.setItem("collapseDefaultActiveKey", val);
    },
  },
};
</script>

<style scoped lang="less">
/* 组件 */
.sliderassembly {
  box-shadow: 0px 0px 1px 1px #ccc;
  width: 278px;
  height: 100%;
  overflow-y: scroll;
  box-sizing: border-box;
  background: #fff;

  /* 滚动条 */
  &::-webkit-scrollbar {
    width: 1px;
  }

  &::-webkit-scrollbar-thumb {
    background-color: @primary-color;
  }

  /deep/.a-collapse-item__header,
  /deep/.a-collapse-item__wrap {
    border-bottom: 0 !important;
  }

  /* 组件列表 */
  .componList {
    display: inline-flex;
    flex-direction: column;
    justify-content: center;
    width: 80px;
    height: 88px;
    margin-bottom: 8px;
    align-items: center;
    cursor: all-scroll;
    transition: all 0.3s;

    &:hover {
      background: @primary-color;
      border-radius: 2px;
      font-weight: 700;

      i,
      p,
      span {
        color: #fff;
      }
    }

    /* 图标 */
    i {
      font-size: 32px;
      width: 32px;
      height: 32px;
      line-height: 32px;
      color: #b0a8a8;
      margin-top: 4px;
    }

    /* 标题 */
    p {
      font-size: 12px;
      color: #323233;
      margin-top: 4px;
    }

    /* 数量 */
    span {
      color: #7d7e80;
      margin-top: 4px;
      font-size: 10px;
    }
  }
}

/deep/.ant-collapse {
  border: 0;
}

/deep/ .ant-collapse-header {
  padding: 7px 0 7px 40px !important;
}
</style>
