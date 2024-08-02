<template>
  <div class="mobilemenu">
    <div padding v-for="(item, indexitem) in menus" :key="indexitem">
      <div style="background-color: #fff;    padding: 12px 10px;"> {{ item.name }}</div>
      <van-grid :column-num="datas.columnnum" :border="datas.border">
        <van-grid-item v-for="(childrenitem, childrenindex) in item.children" :key="childrenindex">
          <div class="grid-item-box">
            <img style="height: 48px;width: 48px" :src="childrenitem.icon" />
            <label class="text">{{ childrenitem.name }}</label>
          </div>
        </van-grid-item>
      </van-grid>
    </div>
    <!-- 删除组件 -->
    <slot name="deles" />
  </div>
</template>

<script>
import {
  menu
} from "@/services/account/login";
export default {
  name: 'mobilemenu',
  data() {
    return {
      menus: []
    }
  },
  props: {
    datas: Object,
  },
  created() {
    this.initMenu();
  },
  methods: {
    /**
     * 初始化菜单
     */
    initMenu() {
      let that = this;
      menu({
        isShowMobile: true
      }).then(res => {
        for (var i = 0; i < res.length; i++) {
          var item = res[i];
          var menu = {
            icon: item.meta.image,
            name: item.name,
            children: []
          };
          //下面一级
          var menuj = [];
          //判断是否还具有下级
          for (var j = 0; j < item.children.length; j++) {
            var itemj = item.children[j];

            var menucj = {
              name: itemj.name,
              icon: itemj.meta.image,
            };

            menuj.push(menucj);
          }
          menu.children = menuj;
          that.menus.push(menu);
        }
      }).then(result => { }).catch(err => {

      })
    }
  },
}
</script>

<style scoped lang="less">
.mobilemenu {
  position: relative;

  .grid-item-box {
    flex: 1;
    // position: relative;
    /* #ifndef APP-NVUE */
    display: flex;
    /* #endif */
    flex-direction: column;
    align-items: center;
    justify-content: center;
    padding: 15px 0;
  }
}
</style>
