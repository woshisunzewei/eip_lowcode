<template>
    <a-modal v-drag :destroyOnClose="true" :maskClosable="false" width="90%" :visible="visible" :title="title"
        :footer="null" @cancel="cancel" :dialogStyle="{ top: modal.top }">
        <vxe-toolbar ref="toolbar" style="padding-top: 0;" custom print export>
            <template #buttons>
                <a-input-search placeholder="关键字搜索" v-model="filterName" style="width: 200px" @change="searchEvent"
                    @search="searchEvent" />
            </template>
        </vxe-toolbar>
        <vxe-table :loading="table.loading" ref="runBatch" :border="true" :column-config="{ isHover: true }"
            :row-config="{ isHover: true }" :height="table.height" :export-config="{}" :print-config="{}"
            :data="table.list">
            <template #loading>
                <a-spin></a-spin>
            </template>
            <template #empty>
                <eip-empty />
            </template>
            <vxe-column type="seq" title="序号" width="60"></vxe-column>
            <template v-for="(item, i) in table.columns">
                <vxe-column :key="i" type="html" :field="item.field" :title="item.title" :width="item.width"
                    showOverflow="ellipsis"> </vxe-column>
            </template>
        </vxe-table>
    </a-modal>
</template>

<script>
import { mapGetters } from "vuex";
import {
    businessDataBatch,
    menuAgile,
} from "@/services/system/agile/run/edit";

export default {
    components: {},
    name: "batch",
    computed: {
        ...mapGetters("account", ["user", "systemAgile"]),
    },
    data() {
        return {
            //表格配置信息
            filterName: '',
            table: {
                height: (this.eipHeaderHeight() - 240) + "px",
                loading: false,
                data: [],
                list: [],
                columns: [],
                title: ''
            },
            modal: {
                bodyStyle: {
                    padding: "14px",
                },
            },

            editConfig: {
                formJson: "",
                columnJson: {},
            },
        };
    },

    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        configId: {
            type: String,
        },
        title: {
            type: String,
        },
        relationId: {
            type: String,
        },
        model: {
            type: String,
        },
    },

    mounted() {
        this.init();
        this.$refs.runBatch.connect(this.$refs.toolbar);
    },
    methods: {

        /**
        * 取消
        */
        cancel() {
            this.$emit("update:visible", false);
        },

        /**
         * 初始化
         */
        async init() {
            let that = this;
            this.table.loading = true;
            var systemAgileData = that.systemAgile.filter(f => f.configId == that.configId);
            if (systemAgileData && systemAgileData.length > 0) {
                that.editConfig = systemAgileData[0];
            } else {
                systemAgileData = await menuAgile({ configId: that.configId });
                if (systemAgileData.code === that.eipResultCode.success) {
                    that.editConfig = systemAgileData.data[0];
                } else {
                    that.$message.error('获取配置错误')
                }
            }
            var publicJson = JSON.parse(that.editConfig.publicJson);
            that.modal.width =
                publicJson.config.width +
                (typeof publicJson.config.unit != "undefined"
                    ? publicJson.config.unit
                    : "px");

            that.modal.top = publicJson.config.top + "px";
            that.modal.centered = publicJson.config.centered;

            var control = this.getOptionByModel(this.model);
            control.list.forEach(item => {
                var column = {
                    field: item.model,
                    title: item.label,
                    width: item.options.width
                };
                that.table.columns.push(column);
            })
            businessDataBatch({
                configId: that.configId,
                table: this.model,
                id: this.relationId,
            }).then(function (result) {
                if (result.code == that.eipResultCode.success) {
                    that.table.data = result.data;
                    that.searchEvent();
                } else {
                    that.$message.error(result.msg);
                }
                that.table.loading = false;
            })
        },
        searchEvent() {
            let that = this;
            const filterName = this.$utils.toValueString(this.filterName).trim().toLowerCase()
            if (filterName) {
                const filterRE = new RegExp(filterName, 'gi')
                const searchProps = this.table.columns.map(m => m.field)
                const rest = this.table.data.filter(item => searchProps.some(key => that.$utils.toValueString(item[key]).toLowerCase().includes(filterName)))
                this.table.list = rest.map(row => {
                    const item = Object.assign({}, row)
                    searchProps.forEach(key => {
                        item[key] = that.$utils.toValueString(item[key]).replace(filterRE, match => `<span class="keyword-lighten">${match}</span>`)
                    })
                    return item
                })
            } else {
                this.table.list = this.table.data
            }
        },
        /**
        *
        * 根据名称获取控件属性
        */
        getOptionByModel(model) {
            var option = null;
            // 递归遍历控件树
            const traverse = (array) => {
                array.forEach((element) => {
                    if (model == element.model && element.type == "batch") {
                        option = element;
                    }
                    if (["grid", "tabs", "selectInputList", "collapse"].includes(element.type)) {
                        // 栅格布局 and 标签页
                        element.columns.forEach((item) => {
                            traverse(item.list);
                        });
                    } else if (element.type === "card" || element.type === "batch") {
                        // 卡片布局 and  动态表格
                        traverse(element.list);
                    } else if (element.type === "table") {
                        // 表格布局
                        element.trs.forEach((item) => {
                            item.tds.forEach((val) => {
                                traverse(val.list);
                            });
                        });
                    }
                });
            };
            var publicJson = JSON.parse(this.editConfig.publicJson);
            traverse(publicJson.list);
            return option;
        },
    },
};
</script>

<style lang="less">
.keyword-lighten {
    color: #000;
    background-color: #FFFF00;
}
</style>