<template>
    <splitpanes class="default-theme" style="height: auto">
        <pane :min-size="split.minWidth" :size="split.minWidth">
            <a-card @contextDictionary.prevent size="small" class="eip-admin-card-small">
                <div slot="title">
                    <a-space>
                        <a-input-search v-model="txtLog.key" :allowClear="true" placeholder="名称/拼音模糊搜索"
                            @change="txtLogSearch" />
                        <a-tooltip title="刷新">
                            <a-button type="primary" @click="txtLogInit">
                                <a-icon type="redo" />
                            </a-button>
                        </a-tooltip>
                    </a-space>
                </div>
                <a-spin :spinning="txtLog.spinning" :delay="0" :style="txtLog.style">
                    <div>
                        <a-directory-tree v-if="!txtLog.spinning" :expandAction="false" :tree-data="txtLog.data"
                            :defaultExpandedKeys="[txtLog.data.length > 0 ? txtLog.data[0].key : '']" :icon="txtLogIcon"
                            @select="txtLogSelect"></a-directory-tree>
                    </div>
                </a-spin>
            </a-card>
        </pane>
        <pane min-size="50">
            <a-card size="small" class="eip-admin-card-small">
                <div slot="title">
                    <a-space>
                        <a-input-search v-model="txtLog.detail.key" :allowClear="true" placeholder="关键字模糊搜索"
                            @change="txtLogDetailSearch" />
                    </a-space>
                </div>
                <a-spin :spinning="txtLog.detail.spinning">
                    <div :style="txtLog.style">
                        <a-result v-if="txtLog.detail.path == ''" title="点击左侧文件查看详情" sub-title="">
                            <template #icon>
                                <img src="/images/empty.png" />
                            </template>
                            <template #extra>

                            </template>
                        </a-result>
                        <div style="white-space: pre-wrap;" v-else v-html="txtLog.detail.searchKey"></div>
                    </div>

                </a-spin>
            </a-card>
        </pane>
    </splitpanes>
</template>

<script>
import {
    query,
    detail,
} from "@/services/system/log/txt";

import { Splitpanes, Pane } from 'splitpanes'
import 'splitpanes/dist/splitpanes.css'

export default {
    components: {
        Splitpanes,
        Pane
    },
    data() {
        return {
            split: {
                minWidth: (250 / window.innerWidth) * 100,
            },
            txtLog: {
                style: {
                    overflow: "auto",
                    height: this.eipHeaderHeight() - 68 + "px",
                },
                key: null,
                original: [],
                data: [],
                spinning: true,
                right: {
                    item: null,
                    style: "",
                },

                detail: {
                    path: '',
                    spinning: false,
                    log: '',
                    key: '',
                    searchKey: ''
                }
            },

        };
    },
    created() {
        this.txtLogInit();
    },
    mounted() {
    },
    methods: {
        /**
         * 
         */
        txtLogDetailSearch() {
            let val = this.$utils.clone(this.txtLog.detail.log, true)
            val = val + '';
            if (val.includes(this.txtLog.detail.key) && this.txtLog.detail.key !== '') {
                const regExp = new RegExp(this.txtLog.detail.key, 'g')
                this.txtLog.detail.searchKey = val.replace(regExp, '<font style="font-weight: bold;" color="red">' + this.txtLog.detail.key + '</font>')
            } else {
                this.txtLog.detail.searchKey = val
            }
        },

        /**
         * 
         * @param {*} e 
         */
        txtLogSearch(e) {
            var that = this;
            this.txtLog.data = this.$utils.clone(this.txtLog.original, true);
            this.txtLog.data = this.$utils.searchTree(
                this.txtLog.data,
                (item) => {
                    if (that.txtLog.key) {
                        var titlePinyin = pinyin.convert(item.title).toLowerCase();
                        if (
                            item.title &&
                            item.title
                                .toLowerCase()
                                .includes(that.txtLog.key.toLowerCase())
                        ) {
                            return true;
                        } else if (
                            titlePinyin.includes(that.txtLog.key.toLowerCase())
                        ) {
                            return true;
                        } else {
                            return false;
                        }
                    }
                    else {
                        return true;
                    }
                },
                { children: "children" }
            );
        },

        /**
         * 组织架构树
         */
        txtLogInit() {
            let that = this;
            that.txtLog.spinning = true;
            query().then((result) => {
                that.txtLog.data = result.data;
                that.txtLog.original = result.data;
                that.txtLog.spinning = false;
                that.txtLogSearch();
            });
        },

        /**
         * 树图标
         */
        txtLogIcon(props) {
            const { expanded } = props;
            if (props.children.length == 0) {
                return <a-icon type="file" />;
            }
            return <a-icon type={expanded ? "folder-open" : "folder"} />;
        },

        /**
         * 树选中
         */
        txtLogSelect(electedKeys, e) {
            let that = this;
            if (e.node.isLeaf) {
                this.txtLog.detail.path = electedKeys[0];
                this.txtLog.detail.spinning = true;
                detail({
                    filePath: this.txtLog.detail.path
                }).then((result) => {
                    this.txtLog.detail.spinning = false;
                    if (result.code == that.eipResultCode.success) {
                        that.txtLog.detail.log = result.data;
                        that.txtLog.detail.searchKey = result.data;
                    } else {
                        that.$message.error(result.msg);
                    }
                });
            }

        },
    },
};
</script>