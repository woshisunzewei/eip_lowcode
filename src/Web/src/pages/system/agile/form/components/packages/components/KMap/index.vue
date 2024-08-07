<template>
    <div>
        <a-form-item v-if="typeof value.dialog !== 'undefined' && typeof value.dialog.width !== 'undefined'"
            label="弹出类型">
            <a-select v-model="value.dialog.type">
                <a-select-option value="modal">
                    对话框
                </a-select-option>
                <a-select-option value="drawer">
                    抽屉
                </a-select-option>
            </a-select>
        </a-form-item>

        <a-form-item v-if="typeof value.dialog !== 'undefined' && typeof value.dialog.width !== 'undefined'"
            label="弹出框宽度">
            <a-input-group compact>
                <a-input-number style="width:174px" v-model="value.dialog.width" placeholder="请输入弹出宽度" />
                <a-select v-model="value.dialog.widthUnit" style="width:60px">
                    <a-select-option value="px">
                        px
                    </a-select-option>
                    <a-select-option value="%">
                        %
                    </a-select-option>
                </a-select>
            </a-input-group>
        </a-form-item>
        <a-form-item label="弹出选项"
            v-if="typeof value.dialog !== 'undefined' && typeof value.dialog.centered !== 'undefined'">
            <a-checkbox v-if="value.dialog.type == 'modal'" v-model="value.dialog.centered">弹出居中</a-checkbox>
            <a-checkbox v-model="value.dialog.maskClosable">点击蒙层是否允许关闭</a-checkbox>
        </a-form-item>
        <a-form-item label="弹出层级数"
            v-if="typeof value.dialog !== 'undefined' && typeof value.dialog.zIndex !== 'undefined'">
            <a-tooltip>
                <div slot="title">
                    数值越大越在顶部
                </div>
                <a-input-number class="eip-width-full" :min="1000" v-model="value.dialog.zIndex" />
            </a-tooltip>
        </a-form-item>
        <a-form-model-item label="数据映射">
            <a-badge :dot="value.map.length > 0" style="width:100%">
                <a-button @click="mapChosen" block><a-icon type="pull-request" /> 配置数据映射 </a-button>
            </a-badge>
        </a-form-model-item>

        <a-modal v-drag centered width="800px" :visible="map.visible" :bodyStyle="{ padding: '1px' }"
            :destroyOnClose="true" :maskClosable="false" title="配置数据映射" @cancel="map.visible = false">
            <template slot="footer">
                <a-button key="back" @click="map.visible = false">关闭</a-button>
            </template>
            <a-row>
                <a-col>
                    <a-card class="eip-admin-card-small" size="small">
                        <vxe-table ref="tableMap" id="tableMap" size="small" :height="500" :data="value.map">
                            <template #loading>
                                <a-spin></a-spin>
                            </template>
                            <template #empty>
                                <eip-empty />
                            </template>
                            <vxe-column type="seq" title="序号" align="center" width="60"></vxe-column>
                            <vxe-column field="name" title="字段" width="100">
                            </vxe-column>
                            <vxe-column field="description" title="描述" width="130" showOverflow="ellipsis">
                            </vxe-column>
                            <vxe-column field="to" title="至" width="40" align="center" showOverflow="ellipsis">
                            </vxe-column>
                            <vxe-column title="映射字段" align="center" width="300">
                                <template v-slot="{ row }">
                                    <a-space size="small">
                                        <a-input disabled allow-clear v-model="row.mapcolumn" />
                                        <a-button @click="conditionSetting('map', row)">
                                            <a-icon type="unordered-list" />
                                        </a-button>
                                    </a-space>
                                </template>
                            </vxe-column>
                        </vxe-table>
                    </a-card>
                </a-col>
            </a-row>
        </a-modal>

        <a-modal v-drag width="370px" centered :visible="condition.visible" :bodyStyle="{ padding: '1px' }"
            :destroyOnClose="true" :maskClosable="false" :title="condition.title" @cancel="conditionCancel"
            :footer="null">
            <a-spin :spinning="condition.tree.spinning">
                <a-row>
                    <a-col>
                        <a-card class="eip-admin-card-small" size="small">
                            <a-directory-tree default-expand-all :style="condition.tree.style"
                                :tree-data="condition.tree.data" :expandAction="false" @select="onSelect"
                                v-if="condition.tree.data.length > 0"></a-directory-tree>
                            <eip-empty :style="condition.tree.style" v-if="condition.tree.data.length == 0"
                                description="无相关信息" />
                        </a-card>
                    </a-col>
                </a-row>
            </a-spin>
        </a-modal>
    </div>
</template>
<script>


/*
 * author 孙泽伟
 * date 2022-03-28
 * description 地图选择
 */
export default {
    name: "KMap",
    data() {
        return {
            bodyStyle: {
                padding: "4px",
                "background-color": "#f0f2f5",
            },
            height: this.eipHeaderHeight() - 190 + "px",
            labelCol: { span: 4 },
            wrapperCol: { span: 20 },

            condition: {
                title: "选择字段",
                visible: false,

                tree: {
                    type: "",
                    row: null,
                    style: {
                        overflow: "auto",
                        height: "400px",
                    },
                    data: [],
                    spinning: false,
                },
            },

            map: {
                visible: false,
            },
            loading: false,
        };
    },
    props: {
        value: {
            type: Object,
            required: true,
        },
    },
    methods: {

        /**
        * 显示列源选择
        */
        mapChosen() {
            let that = this;
            var columns = [{ key: "lng", title: "经度" }, { key: "lat", title: "纬度" }, { key: "address", title: "地址" },]
            columns.forEach((item) => {
                //判断字段是否存在
                var have = that.value.map.filter(f => f.name == item.key);
                if (have.length == 0) {
                    that.value.map.push({
                        name: item.key,
                        description: item.title,
                        to: "->",
                        mapcolumn: "",
                    });
                }
            });

            this.map.visible = true;
            this.conditionInit();
        },

        /**
         * 条件配置
         */
        conditionSetting(type, row) {
            //得到所有组件
            this.condition.tree.type = type;
            this.condition.tree.row = row;
            this.condition.visible = true;
        },

        /**
         * 初始化条件
         */
        conditionInit() {
            let that = this;
            this.condition.tree.data = [];

            that.condition.tree.spinning = true;
            let fieldSchema = this.eipAgileDesigner.kfd.getFieldSchema();
            fieldSchema
                .filter((f) => !["batch", "text", "divider"].includes(f.type))
                .forEach((item) => {
                    that.condition.tree.data.push({
                        key: item.model,
                        isLeaf: true,
                        title: item.model + (item.label != "" ? "(" + item.label + ")" : ""),
                    });
                })

            //得到子表
            fieldSchema
                .filter((f) => f.type == "batch")
                .forEach((item) => {
                    var childrens = [];
                    if (item.list.length > 0) {
                        var children = {
                            title: item.model + (item.label != "" ? "(" + item.label + ")" : ""),
                        }

                        item.list.forEach((control) => {
                            childrens.push({
                                key: item.model + "." + control.model,
                                isLeaf: true,
                                title: control.model + (control.label != "" ? "(" + control.label + ")" : ""),
                            });
                        });
                        children.children = childrens;
                        that.condition.tree.data.push(children);
                    }

                });

            that.condition.tree.spinning = false;
        },

        /** */
        conditionCancel() {
            this.condition.visible = false;
        },

        /**
         * 选择
         */
        onSelect(keys, event) {
            if (this.condition.tree.type == "map") {
                this.condition.tree.row.mapcolumn = keys[0];
            }
            this.conditionCancel();
        },

    },
};
</script>
<style lang="less" scoped>
.ant-form-item {
    margin-bottom: 0;
}
</style>