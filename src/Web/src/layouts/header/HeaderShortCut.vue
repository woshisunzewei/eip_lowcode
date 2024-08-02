<template>
  <a-dropdown placement="bottomRight">
    <a-space>
      <a-icon type="unordered-list" />
      <span>快捷菜单</span>
    </a-space>
    <a-menu :class="['avatar-menu']" slot="overlay">
      <eip-empty v-if="shortCuts.length == 0" />
      <draggable :options="dragOptions" @end="dragEnd">
        <a-menu-item :class="['ant-dropdown-menu-item']" v-for="(item, i) in shortCuts" :key="i">
          <a-row>
            <a-col :span="18" @click="open(item)">
              <a-tooltip>
                <template slot="title">{{ item.name }}</template>
                <a-space>
                  <a-icon :type="item.icon" />
                  <span>{{ item.name }}</span>
                </a-space>
              </a-tooltip>
            </a-col>
            <a-col :span="6" @click="del(item)">
              <a-tooltip>
                <template slot="title">拖动排序</template>
                <a-icon type="drag" />
              </a-tooltip>
              <a-divider type="vertical" />
              <a-tooltip>
                <template slot="title">删除</template>
                <a-icon style="color:#ff4d52" type="delete" />
              </a-tooltip>
            </a-col>
          </a-row>
        </a-menu-item>
      </draggable>

      <a-menu-divider />
      <a-menu-item>
        <a-row>
          <a-col :span="8">
            <a-button size="small" @click="add">
              <a-icon type="plus"></a-icon>新增
            </a-button>
          </a-col>
          <a-col :span="8" :offset="6">
            <a-button size="small" type="danger" @click="delAll">
              <a-icon type="delete"></a-icon>清空
            </a-button>
          </a-col>
        </a-row>
      </a-menu-item>
    </a-menu>
  </a-dropdown>
</template>

<script>
import Draggable from "vuedraggable";

import {
  query,
  del,
  delall,
  saveOrderNo
} from "@/services/system/user/shortcut";
import { deleteConfirm } from "@/utils/util";
const dragOptions = {
  sort: true,
  scroll: true,
  scrollSpeed: 2,
  animation: 150,
  ghostClass: "dragable-ghost",
  chosenClass: "dragable-chose",
  dragClass: "dragable-drag"
};
export default {
  name: "HeaderShortCut",
  components: { Draggable },
  data() {
    return {
      shortCuts: [],
      dragOptions: { ...dragOptions, group: this.shortCuts },

    };
  },

  mounted() {
    this.init();
  },
  methods: {
    /**
     * 新增快捷方式
     */
    add() {
      this.$emit("shortCut");
    },

    /**
     * 初始化快捷方式
     */
    init() {
      let that = this;
      query({ type: 0 }).then(result => {
        that.shortCuts = result.data.filter(f => f.openUrl);
      });
    },

    /**
     * 打开菜单
     * @param {*} item
     */
    open(item) {
      const route = this.$route;
      if (route.path != item.openUrl && route.name != item.name) {
        this.$openPage(item.openUrl, item.name);
      }
    },

    /**
     * 删除
     * @param {*} item
     */
    del(item) {
      let that = this;
      deleteConfirm(
        "确定删除快捷菜单:【" + item.name + "】",
        function () {
          del({ id: item.menuId }).then(result => {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.init();
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        that
      );
    },

    /**
     * 删除所有
     */
    delAll() {
      let that = this;
      deleteConfirm(
        "确定删除所有快捷菜单",
        function () {
          delall().then(result => {
            if (result.code === that.eipResultCode.success) {
              that.$message.success(result.msg);
              that.shortCuts = [];
            } else {
              that.$message.error(result.msg);
            }
          });
        },
        that
      );
    },

    /**
     * 拖拽结束事件
     */
    dragEnd(event) {
      event.preventDefault();
      let that = this;
      //调换顺序
      let oldIndex = event.oldIndex; //移动初始位置
      let newIndex = event.newIndex; //运动终止位置
      let diff = Math.abs(newIndex - oldIndex); //插值绝对值
      let index = this.shortCuts[oldIndex];
      if (eval(oldIndex) > eval(newIndex)) {
        for (let i = 0; i < diff; i++) {
          this.shortCuts[oldIndex - i] = this.shortCuts[oldIndex - i - 1];
        }
        this.shortCuts[newIndex] = index;
      } else {
        for (let i = 0; i < diff; i++) {
          this.shortCuts[oldIndex + i] = this.shortCuts[oldIndex + i + 1];
        }
        this.shortCuts[newIndex] = index;
      }
      var ids = this.shortCuts.map(m => m.menuId).join(",");
      saveOrderNo({ id: ids }).then(result => {
        if (result.code !== that.eipResultCode.success) {
          that.$message.error(result.msg);
        }
      });
    }
  }
};
</script>

<style lang="less">
.avatar-menu {
  width: 180px;
}
</style>
