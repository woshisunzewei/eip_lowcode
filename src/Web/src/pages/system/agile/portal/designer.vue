<template>
    <a-drawer width="100%" placement="right" :destroyOnClose="true" :visible="visible" :closable="false"
        :bodyStyle="{ padding: '0' }" @close="cancel">
        <a-config-provider>
            <div class="eip-form-designer-container">
                <div class="operating-area">
                    <div class="left-btn-box">
                        <a-tooltip title="清空">
                            <a @click="clearLayoutData">
                                <a-icon type="retweet" />
                                <span>清空</span>
                            </a>
                        </a-tooltip>
                        <a-divider type="vertical" />
                        <a-tooltip title="保存">
                            <a @click="saveLayoutData">
                                <a-icon type="save" />
                                <span>保存</span>
                            </a>
                        </a-tooltip>
                        <a-divider type="vertical" />
                        <a-tooltip title="发布">
                            <a @click="publicLayoutData">
                                <a-icon type="build" />
                                <span>发布</span>
                            </a>
                        </a-tooltip>
                    </div>
                    <div class="right-btn-box">
                        <div class="margin-right-xl">
                            <a-tooltip :title="title"><span class="text-bold">{{ title }}</span></a-tooltip>
                            <a-divider type="vertical" />
                            <a-tooltip title="关闭">
                                <a @click="cancel" style="color: #ff4d4f">
                                    <a-icon type="close" />
                                    <span>关闭</span>
                                </a>
                            </a-tooltip>
                        </div>
                    </div>
                </div>
                <div class="content toolbars-top">
                    <aside class="left">
                        <a-collapse>
                            <a-collapse-panel :header="item.title" :key="index" v-for="(item, index) in initLayout">
                                <ul>
                                    <li style="cursor: pointer;" v-for="(val, index) in item.items"
                                        @click="addPanelItem(val)" :key="index">
                                        {{ val.name }}
                                    </li>
                                </ul>
                            </a-collapse-panel>
                        </a-collapse>
                    </aside>
                    <section>
                        <a-card size="small" :bodyStyle="{ padding: '1px' }">
                            <div class="edit-container beauty-scroll" :style="{ height: height }">
                                <!-- 自定义布局项 -->
                                <grid-layout :layout.sync="layout" :row-height="40">
                                    <grid-item @resized="sizeAutoChange(item)" v-for="item in layout" :x="item.x"
                                        :y="item.y" :w="item.w" :h="item.h" :i="item.i" :key="item.i"
                                        :class="{ 'active-item': item.i === activeId }"
                                        @click.native="handleClick(item)">
                                        <person v-if="item.name == '个人信息'" :editPersonStyle="true" :id="item.i">
                                        </person>
                                        <wait-matter v-if="item.name === '待办事项'" :id="item.i"></wait-matter>
                                        <notice v-if="item.name === '公告栏'" :id="item.i">
                                        </notice>
                                        <document v-if="item.name === '待办公文'" :id="item.i">
                                        </document>
                                        <quick-operation v-if="item.name === '快捷操作'" :id="item.i"></quick-operation>
                                        <often-app v-if="item.name === '常用应用'" :id="item.i"></often-app>
                                        <often-apply v-if="item.name === '常用流程'" :id="item.i"></often-apply>
                                        <remind v-if="item.name === '督办提醒'" :id="item.i"></remind>
                                        <meeting v-if="item.name === '我的会议'" :id="item.i"></meeting>
                                        <my-task v-if="item.name === '我的任务'" :id="item.i"></my-task>
                                        <process v-if="item.name === '待办流程'" :id="item.i"></process>
                                        <my-document v-if="item.name === '督办公文'" :id="item.i"></my-document>
                                        <span title="复制" class="drawing-item-copy" @click="addPanelItem(item)">
                                            <a-icon type="copy"></a-icon></span>
                                        <span title="删除" class="drawing-item-delete"
                                            @click.stop="deletePanelItem(item.i)">
                                            <a-icon type="delete"></a-icon></span>
                                    </grid-item>
                                </grid-layout>

                                <div v-if="layout.length == 0" style="text-align: center;">
                                    <img src="@/assets/empty.png" />
                                </div>
                            </div>
                        </a-card>
                    </section>
                </div>
            </div>
        </a-config-provider>
    </a-drawer>
</template>

<script>

import "@/pages/system/agile/form/components/styles/form-design.less";
import notice from './components/Notice'
import document from './components/Document'
import quickOperation from './components/quickOperation'
import oftenApp from './components/oftenApp'
import oftenApply from './components/oftenApply'
import VueGridLayout from 'vue-grid-layout'
import Person from './components/Person'
import waitMatter from './components/WaitMatter'
import Remind from './components/Remind'
import Meeting from './components/Meeting'
import MyTask from './components/MyTask'
import Process from './components/Process'
import MyDocument from './components/MyDocument'

import {
    editSave,
    editPublic,
    findById
} from "@/services/system/agile/custom/designer";

export default {
    data() {
        return {
            activeId: null,
            height: this.eipHeaderHeight() + 30 + "px",
            initLayout: [],
            layout: [],
            panels: [],
            addPanelDialog: false,
            checkAll: false,
            isIndeterminate: true,

            layouyId: 100
        }
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        configId: String,
        title: {
            type: String,
        },
    },
    created() {
        this.initPanelsLayout();
        this.initPanel()
    },

    methods: {
        handleClick(item) {
            this.activeId = item.i
        },
        sizeAutoChange(item) {
            if (item.type === 'unit') {
                this.$refs[item.i] && this.$refs[item.i][0] && this.$refs[item.i][0].chart && this.$refs[item.i][0].chart.resize()
            }
        },

        /**
         * 
         */
        async initPanelsLayout() {
            var layouts = [{
                title: '基础控件',
                items: [
                    { x: 0, y: 0, w: 5, h: 5, i: null, name: '个人信息', type: 'base' },
                    { x: 5, y: 5, w: 12, h: 8, i: null, name: '公告栏', type: 'base' },
                    { x: 0, y: 15, w: 5, h: 9, i: null, name: '常用应用', type: 'base' },
                    { x: 0, y: 24, w: 5, h: 16, i: null, name: '常用流程', type: 'base' },
                    { x: 17, y: 31, w: 7, h: 9, i: null, name: '我的会议', type: 'base' },
                    { x: 17, y: 21, w: 7, h: 10, i: null, name: '我的任务', type: 'base' },
                    { x: 17, y: 0, w: 7, h: 5, i: null, name: '督办提醒', type: 'base' },
                    { x: 5, y: 21, w: 12, h: 8, i: null, name: '督办公文', type: 'base' },
                    { x: 5, y: 13, w: 12, h: 8, i: null, name: '待办公文', type: 'base' },
                    { x: 5, y: 0, w: 12, h: 5, i: null, name: '待办事项', type: 'base' },
                    { x: 5, y: 29, w: 12, h: 8, i: null, name: '待办流程', type: 'base' },
                    { x: 0, y: 8, w: 5, h: 7, i: null, name: '快捷操作', type: 'base' }
                ]
            }]

            this.initLayout = layouts
        },
        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },
        /**
         * 初始化面板项
         */
        initPanel() {
            let that = this;
            that.$message.loading("加载中,请稍等...", 0);
            findById(this.configId).then((result) => {
                if (result.data.saveJson) {
                    that.layout = JSON.parse(result.data.saveJson)
                    that.layouyId += that.layout.length;
                }
                that.$message.destroy();
            });
        },
        /**
         * 
         * @param {*} source 
         */
        deepClone(source) {
            if (!source && typeof source !== 'object') {
                throw new Error('error arguments', 'deepClone')
            }
            const targetObj = source.constructor === Array ? [] : {}
            Object.keys(source).forEach(keys => {
                if (source[keys] && typeof source[keys] === 'object') {
                    targetObj[keys] = deepClone(source[keys])
                } else {
                    targetObj[keys] = source[keys]
                }
            })
            return targetObj
        },
        /**
         * 添加面板
         */
        addPanelItem(item) {
            let clone = this.deepClone(item)
            let x = 0, y = 0, i = this.layouyId
            if (this.layout.length) {
                let lastItem = this.layout[this.layout.length - 1]
                y = lastItem.y + lastItem.h
            }
            let row = { ...clone, i, x, y }
            this.layout.push(row)
            this.activeId = this.layouyId
            this.layouyId++
        },

        /**
         * 保存最新面板布局参数
         */
        saveLayoutData() {
            let that = this;
            let form = {
                configId: this.configId,
                saveJson: JSON.stringify(this.layout),
            };
            that.$loading.show({ text: "保存中,请稍等..." });
            editSave(form).then(function (result) {
                that.$loading.hide();
                if (result.code === that.eipResultCode.success) {
                    that.$message.success(result.msg);
                } else {
                    that.$message.error(result.msg);
                }
            });
        },

        /**
         * 
         */
        clearLayoutData() {
            this.layout = [];
        },

        /**
        * 发布最新面板布局参数
        */
        publicLayoutData() {
            let that = this;
            const modal = this.$confirm({
                okText: "确定",
                okType: "danger",
                cancelText: "取消",
                title: "自定义发布提示",
                content: "若已发布会覆盖原来结构，且不可恢复？",
                onOk() {
                    modal.destroy();
                    let form = {
                        configId: that.configId,
                        saveJson: JSON.stringify(that.layout),
                        publicJson: JSON.stringify(that.layout),
                    };
                    that.$loading.show({ text: "发布中,请稍等..." });
                    editPublic(form).then(function (result) {
                        that.$loading.hide();
                        if (result.code === that.eipResultCode.success) {
                            that.$message.success(result.msg);
                            that.$emit("public", form);
                        } else {
                            that.$message.error(result.msg);
                        }
                    });
                },
                onCancel() { },
            });
        },

        /**
         * 根据面板id删除面板
         * @param {*} panelId 
         */
        deletePanelItem(panelId) {
            const deleteName = []
            const layout = Array.from(this.layout)
            for (const attr in layout) {
                if (layout[attr].i === panelId) {
                    deleteName.push(layout[attr].name)
                    delete layout[attr]
                }
            }
            this.layout = layout.filter(item => {
                if (item !== undefined) {
                    return item
                }
            })
        }
    },

    components: {
        notice,
        document,
        GridLayout: VueGridLayout.GridLayout,
        GridItem: VueGridLayout.GridItem,
        quickOperation,
        oftenApp,
        oftenApply,
        Person,
        waitMatter,
        Remind,
        Meeting,
        MyTask,
        Process,
        MyDocument,
    }
}
</script>

<style lang="less" scoped>
.edit-container {
    background-color: #ebeef5;
    background-image: radial-gradient(#ccc 4%, transparent 0);
    background-size: 45px 45px;
    overflow-y: auto;
}

/* 自定义布局项 */
.vue-grid-layout {
    width: 100%;
}

.eip-form-designer-container .content section {
    -webkit-box-flex: 1;
    -ms-flex: 1;
    flex: 1;
    max-width: calc(100% - 270px);
    -webkit-user-select: none;
    -moz-user-select: none;
    -ms-user-select: none;
    user-select: none;
    margin: 0 8px 0;
    -webkit-box-shadow: 0px 0px 1px 1px #e8e8e8;
    box-shadow: 0px 0px 1px 1px #e8e8e8;
}

.eip-form-designer-container .content aside.right .ant-form-item {
    margin-bottom: 0;
    padding: 6px 0;
    border-bottom: 1px solid #e8e8e8;
}

.eip-form-designer-container .vue-grid-item.active-item {
    border: 1px solid #1890ff;
}

.vue-grid-item:hover {

    .drawing-item-copy,
    .drawing-item-delete {
        display: block;
    }
}

.vue-grid-item {
    position: relative;
    height: 100%;
    border: 1px dashed #e2e0e0;

    .active-item {
        border: 1px solid #1890ff;

        .drawing-item-copy,
        .drawing-item-delete {
            display: block;
        }
    }
}


.drawing-item-copy,
.drawing-item-delete {
    display: none;
    position: absolute;
    top: -10px;
    width: 22px;
    height: 22px;
    line-height: 22px;
    text-align: center;
    border-radius: 50%;
    font-size: 12px;
    border: 1px solid;
    cursor: pointer;
    z-index: 20;
}

.drawing-item-copy {
    right: 56px;
    border-color: #1890ff;
    color: #1890ff;
    background: #fff;

    &:hover {
        background: #1890ff;
        color: #fff;
    }
}

.drawing-item-delete {
    right: 20px;
    border-color: #f56c6c;
    color: #f56c6c;
    background: #fff;

    &:hover {
        background: #f56c6c;
        color: #fff;
    }
}
</style>