<template>
    <a-drawer width="80%" :bodyStyle="{ padding: '1px' }" :destroyOnClose="true" :visible="visible" title="组织架构地图模式查看"
        @close="cancel">
        <div ref="appContainer" class="app-container">
            <div style="margin-left:30px;">
                <a-row :gutter="10">
                    <a-col :span="2">
                        <a-switch v-model="horizontal" :width="50" checked-children="横排" un-checked-children="竖排"
                            style="margin-top:8px;" />
                    </a-col>
                    <a-col :span="2">
                        <a-switch v-model="expandAll" :width="50" checked-children="全部展开" un-checked-children="全部折叠"
                            style="margin:8px;" @change="expandChange" />
                    </a-col>
                    <a-col :span="14">
                        <span style="font-size:14px;font-weight:500;">选择背景色:</span>
                        <a-select style="width:200px" v-model="labelClassName" @change="selectChange">
                            <a-select-option value="bg-white">白色</a-select-option>
                            <a-select-option value="bg-orange">橘黄色</a-select-option>
                            <a-select-option value="bg-gold">金色</a-select-option>
                            <a-select-option value="bg-gray">灰色</a-select-option>
                            <a-select-option value="bg-lightpink">浅粉色</a-select-option>
                            <a-select-option value="bg-chocolate">巧克力色</a-select-option>
                            <a-select-option value="bg-tomato">番茄色</a-select-option>
                        </a-select>
                    </a-col>
                </a-row>
            </div>
            <div style="font-size:12px;margin-top:30px;">
                <div :style="scrollTreeStyle" class="a-org-tree">
                    <div v-for="item in treeData.data">
                        <vue2-org-tree :data="item" :horizontal="!horizontal" :collapsable="collapsable"
                            :label-class-name="labelClassName" name="organ" @on-expand="onExpand"
                            @on-node-click="onNodeClick" />
                    </div>
                </div>
            </div>
            <br /><br />
        </div>
    </a-drawer>
</template>

<script>
import Vue2OrgTree from 'vue2-org-tree'
import 'vue2-org-tree/dist/style.css';
import {
    query,
} from "@/services/system/organization/list";
export default {
    components: { Vue2OrgTree },
    name: "tree",
    data() {
        return {
            spinning: false,
            treeData: {
                labelClassName: "bg-color-orange",
                basicInfo: { id: null, label: "---null" },
                basicSwitch: false,
                data: [],
            }
            ,
            horizontal: true, //横版 竖版
            collapsable: false,
            expandAll: true, //是否全部展开
            labelClassName: '白色', // 默认颜色
            scrollTreeStyle: 'width:100%;',
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        title: {
            type: String,
        },
    },
    mounted() {
        this.treeInit();
    },
    methods: {

        treeInit() {
            var that = this;
            that.spinning = true;
            query({
                current: 1,
                size: 500,
            }).then((result) => {
                if (result.code == that.eipResultCode.success) {
                    var dataTree = [];
                    result.data.forEach(item => {
                        dataTree.push({
                            id: item.organizationId,
                            parentId: item.parentId,
                            label: item.name
                        })
                    })
                    that.treeData.data = that.listToTree(dataTree)
                }
                that.expandChange();
                that.spinning = false;
            });
        },

        listToTree(list) {
            let map = {}, node, tree = [], i;
            for (i = 0; i < list.length; i++) {
                map[list[i].id] = i; // 初始化映射
                list[i].children = []; // 初始化children数组
            }
            for (i = 0; i < list.length; i++) {
                node = list[i];
                if (node.parentId !== null) {
                    // 如果当前节点有父节点，则找到父节点的位置并连接
                    list[map[node.parentId]].children.push(node);
                } else {
                    // 如果没有父节点，则为树的根节点
                    tree.push(node);
                }
            }
            return tree;
        },

        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },
        //渲染节点
        renderContent(h, data) {
            return "";
        },

        //鼠标移出
        onMouseout(e, data) {
            this.treeData.basicSwitch = false;
        },
        //鼠标移入
        onMouseover(e, data) {
            this.treeData.basicInfo = data;
            this.treeData.basicSwitch = true;
            var floating = document.getElementsByClassName("floating")[0];
            floating.style.left = e.clientX + "px";
            floating.style.top = e.clientY + "px";
        },
        //点击节点
        NodeClick(e, data) {
        },
        //默认展开
        toggleExpand(data, val) {
            if (Array.isArray(data)) {
                data.forEach(item => {
                    this.$set(item, "expand", val);
                    if (item.children) {
                        this.toggleExpand(item.children, val);
                    }
                });
            } else {
                this.$set(data, "expand", val);
                if (data.children) {
                    this.toggleExpand(data.children, val);
                }
            }
        },
        collapse(list) {
            list.forEach(child => {
                if (child.expand) {
                    child.expand = false;
                }
                child.children && this.collapse(child.children);
            });
        },
        //展开
        onExpand(e, data) {
            if ("expand" in data) {
                data.expand = !data.expand;
                if (!data.expand && data.children) {
                    this.collapse(data.children);
                }
            } else {
                this.$set(data, "expand", true);
            }
        },
        getList() {
            // 后台回去的数据 赋值给data即可
        },

        // 自定义您的点击事件
        onNodeClick(e, data) {
        },

        expandChange() {
            this.collapsable = true
            this.toggleExpand(this.treeData.data, this.expandAll)
        },
        selectChange() {

        }

    }
}
</script>

<style lang="less">
.bg-white {
    background-color: white;
}

.bg-orange {
    background-color: orange;
}

.bg-gold {
    background-color: gold;
}

.bg-gray {
    background-color: gray;
    color: #fff;
}

.bg-lightpink {
    background-color: lightpink;
    color: #000;
}

.bg-chocolate {
    background-color: chocolate;
    color: #fff;
}

.bg-tomato {
    background-color: tomato;
    color: #fff;
}

//.org-tree-node-label-inner {
//  color: #fff;
//  background-color: orange;
//}

.a-org-tree {
    .a-scrollbar__wrap {
        overflow-x: hidden;
    }

    .org-tree-node-label {
        white-space: nowrap;
    }

    .a-tree-node__content {
        background: white;
    }

    .org-tree-node-label .org-tree-node-label-inner {
        padding-top: 8px;
        padding-right: 10px;
        padding-bottom: 5px;
        padding-left: 10px;
        cursor: pointer;
    }

    .horizontal .org-tree-node.is-leaf {
        padding-top: 5px;
        padding-bottom: 5px;
    }
}
</style>