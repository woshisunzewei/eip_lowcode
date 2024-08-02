<template>
    <div class="getSingleData">
        <a-form-model :layout="form.layout">
            <a-form-model-item label="选择工作表">
                <a-select v-model="drawerData.table" :filter-option="filterOption" show-search
                    placeholder="请选择一个工作表，开始配置流程" style="width: 100%" @change="tableChange">
                    <a-select-option v-for="(item, index) in tables" :key="index" :value="item.configId">{{ item.name
                        }}</a-select-option>
                </a-select>
            </a-form-model-item>
            <a-form-model-item label="触发方式">
                <a-radio-group v-model="drawerData.type">
                    <a-radio :value="1">
                        当新增或更新记录时
                    </a-radio>
                    <a-radio :value="2">
                        仅新增记录时
                    </a-radio>
                    <a-radio :value="3">
                        仅更新记录时
                    </a-radio>
                    <a-radio :value="4">
                        删除记录时
                        <br>
                    </a-radio>
                </a-radio-group>
                <a-select v-model="drawerData.column" mode="multiple" :filter-option="filterOption"
                    v-if="drawerData.type === 1 || drawerData.type === 3" show-search
                    placeholder="当以上指定的其中一个字段更新时将触发流程，如未指定则表示任何字段更新时都会触发" style="width: 100%">
                    <a-select-option v-for="(item, index) in columns" :key="index" :value="item.model">{{ item.label
                        }}</a-select-option>
                </a-select>
            </a-form-model-item>
            <a-form-model-item label="筛选条件">
                <div class="getMethod">
                    <conditionFilter :config.sync="drawerData.conditionFilterData" :options="drawerData.options" />
                </div>
            </a-form-model-item>
        </a-form-model>
    </div>
</template>

<script>
import conditionFilter from './conditionFilter'
import { newGuid } from "@/utils/util";
import {
    table
} from "@/services/system/agile/automation/designer";
export default {
    name: "formChange",
    components: {
        conditionFilter
    },
    props: {
        drawerData: {
            required: true,
            type: Object,
            default: () => {
                return {
                    conditionFilterData: [],
                    options: []
                }
            }
        }
    },
    computed: {

    },
    data() {
        return {
            form: {
                layout: 'horizontal'
            },
            tables: [],//所有工作表
            columns: []//字段
        }
    },

    mounted() {
        this.initTable();

    },
    methods: {
        /**
     *
     * @param {搜索} input
     * @param {*} option
     */
        filterOption(input, option) {
            return (
                option.componentOptions.children[0].text
                    .toLowerCase()
                    .indexOf(input.toLowerCase()) >= 0
            );
        },

        /**
         * 工作表切换
         */
        tableChange(e) {
            let data = this.tables.filter(f => f.configId == e);
            if (data.length > 0 && data[0].columnJson) {
                this.columns = JSON.parse(data[0].columnJson)
            }
            this.drawerData.column = [];
            this.drawerData.options = JSON.parse(data[0].columnJson);
            this.drawerData.conditionFilterData = [{ ops: [], column: undefined, op: undefined, children: [], value: '' }]
        },

        /**
         * 初始化所有的工作表
         */
        initTable() {
            let that = this;
            table().then(function (result) {
                let table = result.data;
                that.tables = table;

                if (that.drawerData.conditionFilterData == undefined) {
                    that.drawerData.conditionFilterData = [{ ops: [], column: undefined, op: undefined, children: [], value: '' }]
                    that.drawerData.options = []
                }
                if (that.drawerData.table) {
                    let data = table.filter(f => f.configId == that.drawerData.table);
                    if (data.length > 0 && data[0].columnJson) {
                        that.columns = JSON.parse(data[0].columnJson)
                        that.drawerData.options = JSON.parse(data[0].columnJson);
                    }
                }
            });
        }
    }
}
</script>

<style scoped></style>