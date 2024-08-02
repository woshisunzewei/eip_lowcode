<template>
    <div class="groupOp-group-container tree box" style="width: 100%;">
        <a-card class="box-card" style="margin-top: 4px" :bodyStyle="{ padding: '0 2px 0 10px' }">
            <div style="padding: 4px">
                <a-row>
                    <a-row class="child">
                        <a-radio-group button-style="solid" v-model="filters.groupOp" size="small">
                            <a-radio-button value="AND">且</a-radio-button>
                            <a-radio-button value="OR">或</a-radio-button>
                        </a-radio-group>
                        <a-button size="small" style="float: right; margin-left: 10px" type="primary" icon="gateway"
                            @click="addGroup">分组</a-button>
                        <a-button size="small" style="float: right; margin-left: 10px" type="primary" icon="plus"
                            @click="add">条件</a-button>
                    </a-row>

                    <template v-for="(item, index) in filters.rules">
                        <a-row :key="'condition' + index" class="child">

                            <a-select style="width: 150px;padding-right: 10px;" @select="paramsChange(item, $event)"
                                v-model="item.field" :allowClear="true" optionFilterProp="label" show-search
                                placeholder="请选择" size="small">
                                <a-select-option v-for="(citem, cindex) in columns" :value="citem.field" :key="cindex"
                                    :label="citem.title">
                                    {{ citem.title ? citem.title : citem.field }} </a-select-option>
                            </a-select>

                            <a-space>
                                <a-select @change="opControlsChange" v-model="item.op" size="small" placeholder="请选择"
                                    :allowClear="true" optionFilterProp="label" show-search style=" width: 100px">
                                    <a-select-option v-for="(oitem, oindex) in item.ops" :value="oitem.value"
                                        :key="oindex" :label="oitem.title">
                                        {{ oitem.title }}</a-select-option>
                                </a-select>

                                <a-input v-if="['input'].includes(item.type) && !['nu', 'nn'].includes(item.op)"
                                    style="width: 100px" size="small" v-model="item.data"
                                    :placeholder="'请输入关键字'"></a-input>

                                <a-select :allowClear="true" optionFilterProp="label" show-search
                                    @change="optionsChange"
                                    v-if="['select'].includes(item.type) && !['nu', 'nn'].includes(item.op)"
                                    v-model="item.data" size="small" placeholder="请选择" style=" width: 100px">
                                    <a-select-option v-for="(oitem, oindex) in item.options" :value="oitem.value"
                                        :key="oindex" :label="oitem.title">
                                        {{ oitem.title }}</a-select-option>
                                </a-select>

                                <a-select :maxTagCount="0" style=" width: 100px"
                                    v-if="['correlationRecord'].includes(item.type) && !['nu', 'nn'].includes(item.op)"
                                    @change="optionsChange" optionFilterProp="label"
                                    @focus="selectCorrelationRecordFocus(item)" :mode="'multiple'"
                                    :placeholder="item.placeholder" allow-clear v-model="item.value" size="small">
                                    <div slot="notFoundContent" style="text-align: center;">
                                        <a-empty :image="simpleImage" />
                                    </div>
                                    <a-select-option v-for="(sitem, sindex) in item.options" :key="sindex"
                                        :value="sitem.key" :label="sitem.value">
                                        {{ sitem.value }}
                                    </a-select-option>
                                </a-select>
                                <a-popconfirm ok-text="确认" cancel-text="取消" @confirm="function () {
                                    filters.rules.splice(index, 1);
                                    $forceUpdate()
                                }">
                                    <template slot="title">
                                        删除规则
                                    </template>
                                    <a-button icon="delete" type="danger" size="small"></a-button>
                                </a-popconfirm>

                            </a-space>
                        </a-row>
                    </template>
                    <template v-for="(item, index) in filters.groups">
                        <a-row :key="'group' + index" class="child" v-if="item.rules.length > 0">
                            <eip-search-pro-item :filters="item" :columns="columns"></eip-search-pro-item>
                        </a-row>
                    </template>
                </a-row>
            </div>
        </a-card>
    </div>
</template>
<script>
import {
    queryTable
} from "@/services/system/agile/run/list";
import { Empty } from 'ant-design-vue';
export default {
    name: 'eip-search-pro-item',
    components: {
    },
    props: {
        filters: {
            type: Object,
            default: function () {
                return {
                    groupOp: 'AND',
                    rules: [],
                    groups: []
                }
            }
        },
        columns: {
            type: Array
        }
    },
    beforeCreate() {
        this.simpleImage = Empty.PRESENTED_IMAGE_SIMPLE;
    },
    data() {
        return {
            type: {
                input: 'input',
                select: 'select',
                datetime: 'datetime',
                correlationRecord: 'correlationRecord'
            }
        }
    },

    methods: {
        /**
         * 新增
         */
        add() {
            this.filters.rules.push({ field: undefined, op: 'cn', data: '', ops: this.opControls.input, type: this.type.input, options: [] })
            this.$forceUpdate()
        },

        /**
         * 新增组
         */
        addGroup() {
            this.filters.groups.push({ groupOp: 'AND', rules: [{ field: undefined, op: 'cn', data: undefined, ops: this.opControls.input, type: this.type.input, options: [] }], groups: [] })
            this.$forceUpdate()
        },

        /**
         * 改变
         * @param {*} row 
         * @param {*} event 
         */
        opControlsChange(row, event) {
            this.$forceUpdate()
        },

        /**
         * 
         */
        optionsChange() {
            this.$forceUpdate()
        },

        /**
         * 参数改变
         * @param {*} row 
         * @param {*} event 
         */
        paramsChange(row, event) {
            row.ops = this.opControls.input;
            row.type = this.type.input;
            row.data = undefined;
            var control = this.$utils.find(this.columns, f => f.field == row.field);
            if (control) {
                if (control.format) {
                    switch (control.format) {
                        case "file":
                            break;
                        case "CorrelationRecord":
                            row.type = this.type.correlationRecord;
                            row.config = control.options;
                            row.ops = this.opControls.select;
                            row.op = row.ops[0].value;
                            break;
                    }
                } else {
                    if (control.style.length > 0) {
                        var options = [];
                        control.style.forEach(item => {
                            options.push({
                                value: item.value,
                                title: item.content
                            })
                        })
                        row.type = this.type.select;
                        row.ops = this.opControls.select;
                        row.op = row.ops[0].value;
                        row.options = options;
                    }
                }
                //判断是否具有样式
            }
            this.$forceUpdate()
        },

        /**
         * 关联记录下拉
         * @param {*} item 
         */
        selectCorrelationRecordFocus(item) {
            let that = this;
            var columns = [];
            var selectColumns = item.config.columns.filter(f => f.isShow).map(m => m.name);
            selectColumns.forEach(item => {
                columns.push({
                    field: item
                })
            })
            columns.push({
                field: "RelationId"
            })
            queryTable({
                table: item.config.dynamicConfig.table,
                columns: JSON.stringify(columns),
                sidx: item.config.columns.filter(f => f.sord).map(m => m.name).join(','),
                sord: item.config.columns.filter(f => f.sord).map(m => m.sordType).join(','),
            }).then(result => {
                if (result.code === that.eipResultCode.success) {
                    let dynamicData = [];
                    result.data.data.forEach((res) => {
                        dynamicData.push({
                            value: res[selectColumns[0]],
                            key: res["RelationId"],
                        });
                    });
                    item.options = dynamicData
                }
                that.$forceUpdate()
            });
        },
    }
}
</script>
<style scoped>
.box {
    width: 100%;
}

/* 只需要左边边框线 */
.child {
    width: 100%;
    position: relative;
    border: 1px solid #d9d9d9;
    border-style: none none none solid;
    padding: 2px 0;
    padding-left: 12px;
}

/* 设置一个伪元素宽2px 高50% 用于遮挡多余的左边框线 */
.child::before {
    display: block;
    content: '';
    position: absolute;
    background-color: white;
    width: 1px;
    height: 50%;
}

/* 设置第一个子元素的伪类定位 */
.box .child:first-child::before {
    left: -1px;
    top: 0;
}

/* 设置第二个子元素的伪类定位 */
.box .child:last-child::before {
    left: -1px;
    bottom: 0;
}

/* 设置子元素的横线，定位在高度的50% */
.box .child::after {
    top: 50%;
    left: 0;
    position: absolute;
    content: '';
    display: block;
    width: 10px;
    height: 1px;
    border: 1px solid #d9d9d9;
    border-style: solid none none none;
}
</style>