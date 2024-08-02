<template>
    <div class="getSingleData">
        <a-form-model layout="horizontal" :model="drawerData">
            <a-form-model-item label="选择工作表">
                <a-select v-model="drawerData.table" :filter-option="filterOption" show-search
                    placeholder="请选择一个工作表，开始配置流程" style="width: 100%" @change="tableChange">
                    <a-select-option v-for="(item, index) in tables" :key="index" :value="item.configId">{{ item.name
                        }}</a-select-option>
                </a-select>
                <a-radio-group v-model="drawerData.addType">
                    <a-radio :value="1">
                        新增一条记录
                    </a-radio>
                    <a-radio :value="2">
                        基于多条记录逐条新增记录
                    </a-radio>
                </a-radio-group>
            </a-form-model-item>
            <a-form-model-item label="选择数据源" v-if="drawerData.addType === 2">
                <a-select v-model="drawerData.addMultipleData" :filter-option="filterOption" show-search
                    placeholder="当以上指定的其中一个字段更新时将触发流程，如未指定则表示任何字段更新时都会触发" style="width: 100%">
                    <a-select-option v-for="(item, index) in configList.filter(f => f.type == 4)" :key="index"
                        :value="item.id">{{ item.title
                        }}</a-select-option>
                </a-select>
            </a-form-model-item>
            <a-form-model-item label="新增记录">
                <div class="info" v-if="!drawerData.table">
                    <a-alert :message="`必须先选择一个对象后，才能设置可执行的动作`" :closable="false" show-icon></a-alert>
                </div>
                <div v-else>
                    <div v-for="(item, index) in drawerData.addData" :key="index">
                        <span>{{ item.label }}</span>
                        <div class="flex justify-start ">
                            <eip-editor :ref="item.model" class="flex-sub" v-model="item.value" :height="tinymce.height"
                                :toolbar="tinymce.toolbar" :plugins="tinymce.plugins" :statusbar="tinymce.statusbar"
                                content_style="p{margin:5px} body{margin:0 0 0 4px} .mce-content-body:not([dir=rtl])[data-mce-placeholder]:not(.mce-visualblocks)::before{left:6px}"
                                :menubar="tinymce.menubar" :placeholder="item.placeholder"></eip-editor>
                            <a-popover placement="bottomLeft" v-model="item.show">
                                <div slot="content" style="width: 600px;">
                                    <a-input-search placeholder="搜索流程节点对象下的字段"
                                        style="width: 100%;padding-bottom: 4px;" />
                                    <a-collapse :expand-icon-position="expandIconPosition"
                                        style="max-height: 400px;overflow: auto;">
                                        <a-collapse-panel v-for="(citem, cindex) in configList" :key="cindex"
                                            :header="citem.title">

                                            <!-- 表单触发 -->
                                            <ul v-if="citem.type == 0" class="conditionFieldBox show">
                                                <li @click="columnClick(item, index, citem, itemc)"
                                                    v-for="itemc in citem.data.columns" :key="itemc.model"
                                                    class="flexRow ThemeHoverBGColor3">
                                                    <div class="ellipsis"><span class="field">[{{ itemc.type
                                                            }}]</span><span title="自动编号">{{ itemc.label }}</span></div>
                                                </li>
                                            </ul>

                                            <!-- 获取多条数据 -->
                                            <ul v-if="citem.type == 4" class="conditionFieldBox show">
                                                <li @click="columnClick(item, index, citem, itemc)"
                                                    v-for="itemc in citem.data.columns" :key="itemc.model"
                                                    class="flexRow ThemeHoverBGColor3">
                                                    <div class="ellipsis"><span class="field">[{{ itemc.type
                                                            }}]</span><span title="自动编号">{{ itemc.label }}</span></div>
                                                </li>
                                            </ul>


                                            <span slot="extra" class="Gray_9e">
                                                {{ citem.type == 0 ? '工作表“' + item.data.tableName + '”' : '' }}
                                            </span>
                                        </a-collapse-panel>
                                    </a-collapse>
                                </div>
                                <a-button type="primary" icon="diff"></a-button>
                            </a-popover>
                        </div>

                    </div>
                </div>

            </a-form-model-item>
        </a-form-model>
    </div>
</template>

<script>
export default {
    name: "addData",
    components: {
    },
    props: {
        tables: {
            required: true,
            type: Array,
            default: () => { return [] }
        },
        configList: {
            required: true,
            type: Array,
            default: () => { return [] }
        },
        drawerData: {
            required: true,
            type: Object
        }
    },
    data() {
        return {
            visible: false,
            expandIconPosition: 'right',
            tinymce: {
                plugins: "preview fullscreen code",
                toolbar: "",
                height: 32,
                statusbar: false,
                show: true,
                menubar: "",
            },
            columns: []
        }
    },

    mounted() {
        this.init();
    },
    methods: {
        /**
       * 选择
       * @param {*} item 
       * @param {*} itemc 
       */
        columnClick(item, index, citem, itemc) {
            let html = "<span id='" + encodeURIComponent(JSON.stringify({
                id: citem.id,
                model: itemc.model
            })) + "' class='non-editable'>" + itemc.label + "</span>"
            this.$refs[item.model][0].insertIndex(html, index);
            item.show = false;
        },
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
                var jsonData = JSON.parse(data[0].columnJson)
                jsonData.forEach(f => {
                    switch (f.type) {
                        case 'input':
                        case 'number':
                        case 'textarea':
                            f.placeholder = '请填写' + f.label
                            break;
                        case 'select':
                        case 'radio':
                        case 'checkbox':
                        case 'dataList':
                        case 'date':
                            f.placeholder = '请选择' + f.label
                            break;
                        default:
                            f.placeholder = "";
                            break;
                    }
                    f.value = undefined;
                    f.show = false;
                })
                this.columns = jsonData;
                this.drawerData.addData = jsonData;
            }
        },

        /**
         * 初始化
         */
        init() {
            let that = this;
            if (that.drawerData.table != undefined) {
                let data = that.tables.filter(f => f.configId == that.drawerData.table);
                if (data.length > 0 && data[0].columnJson) {
                    that.columns = JSON.parse(data[0].columnJson)
                }
            }
        }
    }
}
</script>

<style scoped>
.conditionFieldBox {
    background-color: #f5f5f5;
    display: none;
    height: 0;
    overflow: hidden;
    transition: all 20s ease-in-out
}

.conditionFieldBox.show {
    display: block;
    height: 100%
}

.conditionFieldBox li {
    align-items: center;
    cursor: pointer;
    height: 36px;
    padding: 0 16px 0 39px
}

.conditionFieldBox li:not(:hover) {
    background-color: transparent !important
}

.conditionFieldBox li .field {
    color: #8f8f8f;
    margin-right: 5px
}

.conditionFieldBox li:not(.conditionFieldNull):hover span {
    color: #fff
}

.ThemeHoverBGColor3:hover {
    background-color: #1e88e5 !important
}

.ellipsis {
    overflow: hidden;
    text-overflow: ellipsis;
    vertical-align: top;
    white-space: nowrap
}

.flexRow {
    display: flex;
    min-width: 0
}

/deep/ .info {
    padding: 16px 0;
    font-size: 12px;
    width: 100% !important
}

/deep/ .ant-collapse-content>.ant-collapse-content-box {
    padding: 0 !important;
}

/deep/ .ant-collapse-icon-position-right>.ant-collapse-item>.ant-collapse-header {
    padding: 6px 36px 6px 14px !important;
}

/deep/.ant-form-item-label>label {
    font-weight: bold;
}
</style>