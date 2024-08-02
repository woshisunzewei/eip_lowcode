<template>
    <a-drawer width="1200px" :destroyOnClose="true" :visible="visible" :bodyStyle="{ padding: '1px' }" :title="title"
        @close="cancel">
        <div class="eip-drawer-body">
            <a-spin :spinning="spinning">
                <vxe-table show-header-overflow size="small" :height="height" :data="data">
                    <template #loading>
                        <a-spin></a-spin>
                    </template>
                    <template #empty>
                        <eip-empty />
                    </template>
                    <vxe-column type="seq" title="序号" width="60"></vxe-column>
                    <vxe-column field="name" title="名称" :filters="[{ data: '' }]" :filter-method="filterNameMethod">
                        <template v-slot="{ row }">
                            <a style="text-decoration: underline;" v-if="row.menuId" :href="'/appbuild/' + row.menuId"
                                target="_blank">{{ row.name }}</a>
                            <label v-else>{{ row.name }}</label>
                        </template>

                        <template #filter="{ $panel, column }">
                            <a-input placeholder="请输入名称" v-for="(option, index) in column.filters" :key="index"
                                v-model="option.data" @input="$panel.changeOption($event, !!option.data, option)" />
                        </template>
                    </vxe-column>
                    <vxe-column field="configType" title="类型" sortable align="center" width="120" :filters="configType"
                        :filter-multiple="true">
                        <template v-slot="{ row }">
                            <a-tag v-for="item in configType" :key="item.value" v-if="row.configType == item.value"
                                :color="item.color"> {{ item.label }} </a-tag>
                        </template>
                    </vxe-column>
                    <vxe-column field="isFreeze" title="禁用" sortable align="center" width="100"
                        :filters="[{ label: '是', value: true }, { label: '否', value: false }]" :filter-multiple="false">
                        <template v-slot="{ row }">
                            <a-switch :checked="row.isFreeze" />
                        </template>
                    </vxe-column>
                    <vxe-column field="isDelete" title="删除" sortable align="center" width="100"
                        :filters="[{ label: '是', value: true }, { label: '否', value: false }]" :filter-multiple="false">
                        <template v-slot="{ row }">
                            <a-switch :checked="row.isDelete" />
                        </template>
                    </vxe-column>
                    <vxe-column field="createUserName" title="创建人" sortable width="100"></vxe-column>
                    <vxe-column field="createTime" title="创建时间" sortable width="160"></vxe-column>
                    <vxe-column field="updateUserName" title="修改人" sortable width="100"></vxe-column>
                    <vxe-column field="updateTime" title="修改时间" sortable width="160"></vxe-column>
                </vxe-table>
            </a-spin>
        </div>
        <div class="eip-drawer-toolbar">
            <a-space>
                <a-button key="back" @click="cancel"><a-icon type="close" />关闭</a-button>
            </a-space>
        </div>
    </a-drawer>
</template>

<script>
import { queryKey } from "@/services/system/datasource/agile";
export default {
    name: "datasource-agile",
    data() {
        return {
            configType: [{ label: '列表配置', value: 1, color: "#108ee9" },
            { label: '表单配置', value: 2, color: "#c21401" },
            { label: '工作台', value: 3, color: "#00af57" },
            { label: '移动端自定义', value: 4, color: "#ff1e02" },
            { label: '大屏设计', value: 5, color: "#0071be" }
            ],
            height: this.eipHeaderHeight() - 20 + "px",
            data: [],
            spinning: false,
        };
    },

    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        dataSourceId: String,
        title: {
            type: String,
        },
    },
    created() {
        this.find();
    },
    methods: {
        filterNameMethod({ option, row }) {
            return row.name.includes(option.data)
        },
        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },

        /**
         * 查找权限信息
         */
        find() {
            let that = this;
            this.spinning = true;
            queryKey({ key: this.dataSourceId }).then(function (result) {
                that.spinning = false;
                that.data = result.data;
            });
        },
    },
};
</script>

<style lang="less" scoped></style>