<template>
  <div class="add_box">
    <div class="add_container"></div>
    <div class="add_icon_1"></div>
    <div class="add_icon_2"></div>
    <a-popover
      placement="rightTop"
      trigger="hover"
      :getPopupContainer="(triggerNode) => triggerNode.parentNode"
    >
      <div slot="content">
        <div class="add_popper" style="width: 300px; padding: 0 6px">
          <div v-for="(item, index) in addData" :key="index">
            <span class="add_node_span">{{ item.title }}</span>
            <div class="add_node">
              <div
                @click="addNode(cell.type, cell.name)"
                v-for="(cell, index) in item.listData"
                :key="index"
              >
                <img :src="cell.imgUrl" />
                {{ cell.name }}
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="add_hover"></div>
    </a-popover>
  </div>
</template>

<script>
import { addData } from "./data/WorkFlowData";

export default {
  props: {
    WrapOrBranch: {
      required: true,
      type: String,
      default: "isWrap",
    },
    index: {
      required: false,
      type: Number,
      default: -1,
    },
  },
  data() {
    return {
      addData,
    };
  },
  methods: {
    addNode(type, name) {
      if (type === 8 || type === 9) {
        this.$emit("addBranchNode", this.WrapOrBranch, this.index, type, name);
      } else {
        this.$emit("addWrapNode", this.WrapOrBranch, this.index, type, name);
      }
    },
  },
};
</script>

<style lang="less" scoped>
.add_box {
  width: 100px;
  position: relative;
  height: 72px;
  display: flex;
  justify-content: center;
  align-items: center;
  cursor: pointer;
}

.add_box::before {
  content: "";
  position: absolute;
  top: 0;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 2px;
  background-color: #dedede;
}

.add_container {
  position: relative;
  z-index: 1;
  width: 8px;
  height: 8px;
  border-radius: 50%;
  transition: 0.3s all;
  background-color: #0089ff;
}

.add_icon_1 {
  z-index: 1;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 2px;
  height: 0;
  transition: 0.3s all;
  background-color: #fff;
}

.add_icon_2 {
  z-index: 1;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 0;
  height: 2px;
  transition: 0.3s all;
  background-color: #fff;
}

.add_box:hover .add_container {
  width: 30px;
  height: 30px;
}

.add_box:hover > .add_icon_1 {
  height: 12px;
}

.add_box:hover > .add_icon_2 {
  width: 12px;
}

.add_hover {
  z-index: 2;
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  width: 30px;
  height: 30px;
  border-radius: 50%;
}

.add_node_span {
  font-size: 12px;
  color: #fff;
}

.add_node {
  margin-top: 8px;
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
  color: #fff;
}

.add_node > div {
  cursor: pointer;
  box-sizing: border-box;
  width: 140px;
  padding: 4px 6px;
  margin-bottom: 10px;
  display: flex;
  align-items: center;
  border-radius: 4px;
  border: 1px solid #2e2e2f;
}

.add_node > div:hover {
  background-color: #3b3b3b;
  border: 1px solid #585859;
}

.add_node > div > img {
  margin-right: 6px;
  border-radius: 50%;
}

.add_box {
  /deep/ .ant-popover-inner-content {
    padding: 0 !important;
  }

  /deep/
    .ant-popover-placement-rightTop
    > .ant-popover-content
    > .ant-popover-arrow {
    border-bottom-color: #3b3b3b;
    border-left-color: #3b3b3b;
  }
}
</style>
