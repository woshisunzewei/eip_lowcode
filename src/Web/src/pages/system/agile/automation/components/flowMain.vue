<template>
  <div>
    <!-- 主流程节点 -->
    <div class="node_wrap" v-if="config.type !== 8 && config.type !== 9">
      <!-- ==================================================节点内容================================================== -->
      <div
        :class="[
          'wrap_content',
          contentFocus === config.id ? 'wrap_content_active' : '',
        ]"
      >
        <a-popconfirm title="确定删除吗？" @confirm="delWrapNode">
          <div v-if="config.type !== 0" class="wrap_content_del">删除节点</div>
        </a-popconfirm>
        <div @click.stop="changeNode(config)">
          <div class="wrap_content_container">
            <img :src="require(`@/assets/img/${config.type}.png`)" />
            <div
              class="wrap_content_container_title"
              v-show="showInput !== config.id"
              @click.stop="changeTitle(config)"
            >
              {{ config.title }}
            </div>
            <input
              class="input_1"
              ref="input"
              type="text"
              v-show="showInput === config.id"
              v-model="config.title"
              @blur="showInput = ''"
            />
          </div>
          <div class="wrap_content_content">
            <span>请设置触发事件</span>
            <span>〉</span>
          </div>
        </div>
      </div>
      <!-- ==================================================节点内容================================================== -->
      <flowAdd
        WrapOrBranch="isWrap"
        @addWrapNode="addWrapNode"
        @addBranchNode="addBranchNode"
      />
    </div>
    <!-- 分支节点 -->
    <div
      class="branch_wrap"
      v-if="
        (config.type === 8 || config.type === 9) &&
        config.conditionNodes &&
        config.conditionNodes.length !== 0
      "
    >
      <div class="branch_wrap_container">
        <div class="add_branch" @click="addCondition">添加条件</div>
        <div
          class="branch_wrap_col"
          v-for="(item, index) in config.conditionNodes"
          :key="index"
        >
          <!-- ==================================================节点内容================================================== -->
          <div
            :class="[
              'wrap_content',
              contentFocus === item.id ? 'wrap_content_active' : '',
            ]"
          >
            <a-popconfirm title="确定删除吗？" @confirm="delBranchNode(index)">
              <div class="wrap_content_del">删除节点</div>
            </a-popconfirm>
            <div @click.stop="changeNode(item, index)">
              <div class="wrap_content_container">
                <img :src="require(`@/assets/img/${config.type}.png`)" />
                <div
                  class="wrap_content_container_title"
                  v-show="showInput !== item.id"
                  @click.stop="changeTitle(item, index)"
                >
                  {{ item.title }}
                </div>
                <input
                  class="input_1"
                  ref="input"
                  type="text"
                  v-show="showInput === item.id"
                  v-model="item.title"
                  @blur="showInput = ''"
                />
              </div>
              <div class="wrap_content_content">
                <span>请设置触发事件</span>
                <span>〉</span>
              </div>
            </div>
          </div>
          <!-- ==================================================节点内容================================================== -->
          <flowAdd
            WrapOrBranch="isBranch"
            :index="index"
            @addWrapNode="addWrapNode"
            @addBranchNode="addBranchNode"
          />
          <flowMain
            v-if="item.childNode && JSON.stringify(item.childNode) !== '{}'"
            :config.sync="item.childNode"
            @changeNode="changeNode"
            :contentFocus="contentFocus"
          />
          <div class="cover_top_left" v-if="index === 0"></div>
          <div
            class="cover_top_right"
            v-if="index === config.conditionNodes.length - 1"
          ></div>
          <div class="cover_bottom_left" v-if="index === 0"></div>
          <div
            class="cover_bottom_right"
            v-if="index === config.conditionNodes.length - 1"
          ></div>
        </div>
      </div>
      <flowAdd
        WrapOrBranch="isWrap"
        @addWrapNode="addWrapNode"
        @addBranchNode="addBranchNode"
      />
    </div>
    <flowMain
      v-if="config.childNode && JSON.stringify(config.childNode) !== '{}'"
      :config.sync="config.childNode"
      @changeNode="changeNode"
      :contentFocus="contentFocus"
    />
  </div>
</template>

<script>
import flowAdd from "./flowAdd.vue";
import { del } from "@/services/system/agile/automation/list";
import { newGuid } from "@/utils/util";
export default {
  name: "flowMain",
  components: { flowAdd },
  props: {
    config: {
      required: true,
      type: Object,
      default: () => {
        return {};
      },
    },
    contentFocus: {
      required: true,
      type: String,
      default: "",
    },
  },
  data() {
    return {
      showInput: "",
    };
  },
  methods: {
    // 新增分支
    addCondition() {
      this.config.conditionNodes.push({
        id: newGuid(),
        title: "条件",
        childNode: {},
        data: this.returnData(8),
      });
      this.$forceUpdate();
    },
    // 删除主节点
    delWrapNode() {
      //判断类型，如果是子流程还需要删除流程
      if (this.config.type == 31) {
        let that = this;
        that.$loading.show({ text: that.eipMsg.delloading });
        del({ id: this.config.data.configId }).then((result) => {
          that.$loading.hide();
        });
      }
      this.$emit("update:config", this.config.childNode);
      this.$forceUpdate();
    },
    // 删除分支节点
    delBranchNode(index) {
      if (this.config.conditionNodes.length > 2) {
        this.config.conditionNodes.splice(index, 1);
      } else {
        this.config.conditionNodes = [];
      }
      this.$forceUpdate();
    },
    // 新增主节点
    addWrapNode(data, index, type, title) {
      let temp = {};
      if (data === "isWrap") {
        // 在主节点下新增主节点
        if (this.config.childNode) {
          temp = {
            id: newGuid(),
            type,
            title,
            childNode: this.config.childNode,
            conditionNodes: [],
            data: this.returnData(type),
          };
        } else {
          temp = {
            id: newGuid(),
            type,
            title,
            childNode: {},
            conditionNodes: [],
            data: this.returnData(type),
          };
        }
        this.config.childNode = temp;
      } else if (data === "isBranch") {
        // 在分支节点下新增主节点
        if (this.config.conditionNodes[index].childNode) {
          temp = {
            id: newGuid(),
            type,
            title,
            childNode: this.config.conditionNodes[index].childNode,
            conditionNodes: [],
            data: this.returnData(type),
          };
        } else {
          temp = {
            id: newGuid(),
            type,
            title,
            childNode: {},
            conditionNodes: [],
            data: this.returnData(type),
          };
        }
        this.config.conditionNodes[index].childNode = temp;
      }
      this.$forceUpdate();
    },

    /**
     * 根据类型返回Data
     * @param {} type
     */
    returnData(type) {
      switch (type) {
        case 1: //新增数据
          return {
            table: undefined,
            addType: 1,
            addMultipleData: undefined,
            addData: [], //所有新增字段信息
          };
        case 2: //编辑数据
          return {
            updateObj: undefined,
            updateData: [], //所有新增字段信息
          };
        case 3: //获取单条数据
          return {
            getType: 1, //获取方式
            getTypeTable: undefined,
            conditionFilters: {
              groupOp: "AND",
              rules: [],
              groups: [],
            },
            sortRules: 1,
            sortRulesSelect: undefined,
          };
        case 4: //获取多条数据
          return {
            getType: 1, //获取方式
            getTypeTable: undefined,
            columns: [],
            conditionFilters: {
              groupOp: "AND",
              rules: [],
              groups: [],
            },
            getCustomerRequest: null,
            sortRules: 1,
            sortRulesSelect: undefined,
          };
        case 5: //删除
          return {
            getTypeTable: undefined,
            columns: [],
            conditionFilters: {
              groupOp: "AND",
              rules: [],
              groups: [],
            },
            sortRules: 1,
            sortRulesSelect: undefined,
          };
        case 6: //发送站内通知
          return {
            userType: undefined,
            content: "",
            rangeTxt: "",
            range: [],
            name: "",
          };
        case 8:
        case 9:
          return {
            getTypeTable: undefined,
            columns: [],
            conditionFilters: {
              groupOp: "AND",
              rules: [],
              groups: [],
            },
          };
        case 21: //自定义请求
          return {
            method: "POST",
            url: undefined,
            headers: "", //请求头
            body: "", //请求体
            errorType: 1, //请求超时或请求失败时 1继续执行，2中止流程
            contentType: "application/json",
            test: {
              code: null,
              msg: null,
              data: null,
              array: false,
            },
          };
        case 31: //子流程
          return {
            source: undefined, //子流程数据源
            columns: [], //数据源字段
            configId: newGuid(), //子流程Id
            wait: true, //子流程执行完毕后，再开始下一个节点
            params: [], //向子流程的流程参数传递初始值，供子流程执行时使用
          };
        default:
          return {};
      }
    },

    // 新增分支节点
    addBranchNode(data, index, type, title) {
      let temp = {};
      debugger;
      if (data === "isWrap") {
        // 在主节点下新增分支节点
        if (this.config.childNode) {
          temp = this.config.childNode;
          this.config.childNode = {
            id: newGuid(),
            type,
            title,
            data: {},
            conditionNodes: [
              {
                id: newGuid(),
                type,
                title: "条件1",
                childNode: {},
                data: this.returnData(type),
              },
              {
                id: newGuid(),
                type,
                title: "其他情况",
                childNode: {},
                data: this.returnData(type),
              },
            ],
            childNode: temp,
          };
        } else {
          this.config.childNode = {
            id: newGuid(),
            type,
            title,
            data: {},
            conditionNodes: [
              {
                id: newGuid(),
                type,
                title: "条件1",
                childNode: {},
                data: this.returnData(type),
              },
              {
                id: newGuid(),
                type,
                title: "其他情况",
                childNode: {},
                data: this.returnData(type),
              },
            ],
            childNode: {},
          };
        }
      } else if (data === "isBranch") {
        // 在分支节点下新增分支节点
        if (this.config.conditionNodes[index].childNode) {
          temp = this.config.conditionNodes[index].childNode;
          this.config.conditionNodes[index].childNode = {
            id: newGuid(),
            type,
            title,
            data: {},
            conditionNodes: [
              {
                id: newGuid(),
                type,
                title: "条件1",
                childNode: {},
                data: this.returnData(type),
              },
              {
                id: newGuid(),
                type,
                title: "其他情况",
                childNode: {},
                data: this.returnData(type),
              },
            ],
            childNode: temp,
          };
        } else {
          this.config.conditionNodes[index].childNode = {
            id: newGuid(),
            type,
            title,
            data: {},
            conditionNodes: [
              {
                id: newGuid(),
                type,
                title: "条件1",
                childNode: {},
                data: this.returnData(type),
              },
              {
                id: newGuid(),
                type,
                title: "其他情况",
                childNode: {},
                data: this.returnData(type),
              },
            ],
            childNode: {},
          };
        }
      }
      this.$forceUpdate();
    },
    // 修改节点标题
    changeTitle(data, index = -1) {
      this.showInput = data.id;
      this.$nextTick(() => {
        if (index === -1) {
          this.$refs.input.focus();
        } else {
          this.$refs.input[index].focus();
        }
      });
    },
    // 节点点击事件
    changeNode(data, index = -1) {
      this.$emit("changeNode", data, index);
    },
  },
};
</script>

<style scoped>
/* 单模块 */

.node_wrap {
  display: flex;
  flex-direction: column;
  align-items: center;
}

/* 分支模块 */

.branch_wrap {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.branch_wrap_container {
  z-index: 1;
  position: relative;
  display: flex;
  border-top: 2px solid #dedede;
  border-bottom: 2px solid #dedede;
}

.branch_wrap_container::before {
  z-index: 1;
  content: "";
  position: absolute;
  top: 0;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 2px;
  background-color: #fafafa;
}

.add_branch {
  z-index: 2;
  position: absolute;
  top: 0;
  left: 50%;
  padding: 0 12px;
  padding-left: 14px;
  font-size: 12px;
  font-weight: bold;
  height: 28px;
  color: #666;
  cursor: pointer;
  border-radius: 14px;
  line-height: 26px;
  border: 2px solid #dedede;
  background-color: #fff;
  transition: 0.3s all;
  transform: translate(-50%, -50%);
}

.add_branch:hover {
  color: #0089ff;
  border-color: #0089ff;
}

.branch_wrap_col {
  padding: 40px 20px 0;
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.branch_wrap_col::before {
  z-index: 1;
  content: "";
  position: absolute;
  top: 0;
  bottom: 0;
  left: 50%;
  transform: translateX(-50%);
  width: 2px;
  background-color: #dedede;
}

/* 节点内容 */

.wrap_content {
  box-sizing: border-box;
  position: relative;
  z-index: 1;
  width: 200px;
  margin: 0 auto;
  padding: 8px 10px;
  cursor: pointer;
  border-radius: 8px;
  border: 2px solid #fff;
  box-shadow: 0 0 10px #e3e4e7;
  background-color: #fff;
}

.wrap_content_active {
  border: 2px solid #ff5219;
}

.wrap_content_del {
  opacity: 0;
  transition: 0.3s all;
  position: absolute;
  top: -20px;
  right: -2px;
  color: #999;
  font-size: 12px;
  font-weight: bold;
}

.wrap_content:hover .wrap_content_del {
  opacity: 1;
}

.wrap_content_container {
  display: flex;
  align-items: center;
}

.wrap_content_container > img {
  width: 22px;
  height: 22px;
  margin-right: 8px;
  border-radius: 50%;
}

.wrap_content_container_title {
  padding: 2px 0;
  flex: 1;
  font-size: 14px;
  line-height: 14px;
  color: #303233;
  font-weight: bold;
  border: 1px solid #fff;
}

.input_1 {
  padding: 2px 0;
  flex: 1;
  width: 0;
  border: 1px solid #999;
  border-radius: 0;
  outline: none;
  font-weight: bold;
  font-size: 14px;
  line-height: 14px;
  color: #303233;
}

.wrap_content_content {
  padding: 10px 8px;
  margin-top: 8px;
  font-size: 14px;
  color: #a3a3a3;
  font-weight: normal;
  border-radius: 4px;
  display: flex;
  background-color: #f7f7f7;
}

.wrap_content_content > span:first-child {
  flex: 1;
}

/* 遮盖掉部分边框线 */

.cover_top_left {
  position: absolute;
  top: -2px;
  left: -1px;
  width: 50%;
  height: 2px;
  background-color: #fafafa;
}

.cover_top_right {
  position: absolute;
  top: -2px;
  right: -1px;
  width: 50%;
  height: 2px;
  background-color: #fafafa;
}

.cover_bottom_left {
  position: absolute;
  bottom: -2px;
  left: -1px;
  width: 50%;
  height: 2px;
  background-color: #fafafa;
}

.cover_bottom_right {
  position: absolute;
  bottom: -2px;
  right: -1px;
  width: 50%;
  height: 2px;
  background-color: #fafafa;
}
</style>
