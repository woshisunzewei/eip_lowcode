<template>
    <a-modal v-drag :destroyOnClose="true" :maskClosable="false" :centered="modal.centered" :width="modal.width"
        :visible="visible" :title="'回收站(' + name + ')'" :footer="null" @cancel="cancel"
        :dialogStyle="{ top: modal.top }" :bodyStyle="modal.bodyStyle">
        <a-card class="eip-admin-card-small eip-admin-card-small-bottom-border" :bordered="false"
            v-if="table.search.option.config.length > 0">
            <eip-search :option="table.search.option" @search="search" @advanced="advanced"></eip-search>
        </a-card>

        <a-card class="eip-admin-card-small" :loading="card.loading" :bordered="false">
            <vxe-toolbar ref="runToolbar" custom print :refresh="{ query: initData }">
                <template v-slot:buttons>
                    <a-space>
                        <a-button type="primary" @click="delRecovery">
                            <a-icon type="retweet" /> 恢复选中
                        </a-button>

                        <a-button type="primary" @click="delPhysics">
                            <a-icon type="delete" /> 彻底删除
                        </a-button>

                        <a-button type="danger" @click="delPhysicsAll">
                            <a-icon type="delete" /> 立即清空
                        </a-button>
                    </a-space>
                </template>
                <template #tools>
                    <a-space>
                        <a-dropdown class="margin-right-sm">
                            <a-menu slot="overlay" @click="exportMenuClick">
                                <a-menu-item key="1"> <a-icon type="file-excel" />默认导出 </a-menu-item>
                                <a-menu-item key="2"> <a-icon type="file-excel" />导出选中</a-menu-item>
                                <a-menu-item key="3"> <a-icon type="file-excel" />高级导出 </a-menu-item>
                                <a-menu-item key="4"> <a-icon type="file-excel" />全量导出 </a-menu-item>
                            </a-menu>
                            <a-button shape="circle" icon="download" />
                        </a-dropdown>
                    </a-space>
                </template>
            </vxe-toolbar>

            <vxe-table v-if="table.loaded" :loading="table.loading" ref="recycleBin" :border="agile.config.style.border"
                :column-config="{
                    isCurrent: agile.config.style.columnIsCurrent,
                    isHover: true,
                }" :show-footer="table.columns.filter(f => f.total).length > 0"
                :row-config="{ isCurrent: agile.config.style.rowIsCurrent, isHover: true, height: agile.config.style.rowHeight }"
                :seq-config="{ startIndex: (table.seqConfig.startIndex - 1) * table.page.param.size }"
                :stripe="agile.config.style.stripe" :export-config="{
                    filename: table.title
                }" :height="table.height" :print-config="{}" :data="table.data" :footer-method="returnDataFooter"
                :expand-config="{ accordion: true, lazy: true, toggleMethod: toggleExpand, loadMethod: loadExpand }"
                :tooltip-config="tooltipConfig" @sort-change="tableSort">

                <template #loading>
                    <a-spin></a-spin>
                </template>

                <template #empty>
                    <eip-empty />
                </template>
                <vxe-column v-if="agile.config.style.select != 'null'" :type="agile.config.style.select" width="40"
                    align="center" fixed="left">

                    <template #header="{ checked, indeterminate }">
                        <span v-if="agile.config.style.select == 'checkbox'"
                            @click.stop="$refs.recycleBin.toggleAllCheckboxRow()">
                            <a-checkbox :indeterminate="indeterminate" :checked="checked">
                            </a-checkbox>
                        </span>
                    </template>

                    <template #checkbox="{ row, checked, indeterminate }">
                        <span @click.stop="$refs.recycleBin.toggleCheckboxRow(row)">
                            <a-checkbox :indeterminate="indeterminate" :checked="checked">
                            </a-checkbox>
                        </span>
                    </template>

                    <template #radio="{ row, checked }">
                        <span @click.stop="$refs.recycleBin.toggleCheckboxRow(row)">
                            <a-radio :checked="checked">
                            </a-radio>
                        </span>
                    </template>
                </vxe-column>
                <vxe-column type="seq" width="50" align="center">

                    <template #header="{}">
                        <span>序号</span>
                    </template>
                </vxe-column>

                <template v-for="(item, i) in table.columns">
                    <vxe-colgroup :key="i" align="center" v-if="item.header.columns.length > 0"
                        :title="item.header.title">
                        <vxe-column :key="headerIndex" v-for="(headerItem, headerIndex) in item.header.columns"
                            :field="headerItem.field" :title="headerItem.title" :width="headerItem.width"
                            :align="headerItem.align" :sortable="headerItem.sort" showOverflow="ellipsis"
                            :type="headerItem.subtable.length > 0 ? 'expand' : ''">
                            <template #header="{}">
                                <a-tooltip>
                                    <template slot="title">
                                        改变列宽后保存
                                    </template>
                                    <a-icon type="save"></a-icon>
                                </a-tooltip>
                            </template>

                            <template v-slot="{ row }">
                                <run-list-content :row="row" :item="headerItem"
                                    @batchDetail="batchDetail"></run-list-content>
                            </template>

                            <template #content="{ row }">
                                <run-list-subtable :data="row" :item="headerItem" :table="table"></run-list-subtable>
                            </template>
                        </vxe-column>
                    </vxe-colgroup>

                    <vxe-column :key="i" v-else :field="item.field" :title="item.title" :width="item.width"
                        :align="item.align" :sortable="item.sort" showOverflow="ellipsis"
                        :type="item.subtable.length > 0 ? 'expand' : ''">

                        <template #header>
                            <a-tooltip v-if="item.remark">
                                <template slot="title">
                                    {{ item.remark }}
                                </template>
                                {{ item.title }}
                                <a-icon type="question-circle" />
                            </a-tooltip>
                            <span v-else>{{ item.title }}</span>
                        </template>

                        <template v-slot="{ row }">
                            <run-list-content :row="row" :item="item" @batchDetail="batchDetail"></run-list-content>
                        </template>

                        <template #content="{ row }">
                            <run-list-subtable v-if="item.subtable.length > 0" :data="row" :item="item"
                                :table="table"></run-list-subtable>
                        </template>
                    </vxe-column>
                </template>
            </vxe-table>
            <a-pagination class="padding-top-sm float-right" v-model="table.page.param.current"
                v-if="agile.config.style.paging" show-size-changer show-quick-jumper
                :page-size-options="table.page.sizeOptions" :show-total="(total) => `共 ${total} 条`"
                :page-size="table.page.param.size" :total="table.page.total" @change="initData"
                @showSizeChange="sizeChange"></a-pagination>
        </a-card>
    </a-modal>
</template>

<script>
import Vue from 'vue'
import Viewer from 'v-viewer'
import 'viewerjs/dist/viewer.css'
Vue.use(Viewer)
Viewer.setDefaults({
    Options: {
        'inline': true, // 启用 inline 模式
        'button': true, // 显示右上角关闭按钮
        'navbar': true, // 显示缩略图导航
        'title': true, // 显示当前图片的标题
        'toolbar': true, // 显示工具栏
        'tooltip': true, // 显示缩放百分比
        'movable': true, // 图片是否可移动
        'zoomable': true, // 图片是否可缩放
        //'rotatable': true, // 图片是否可旋转
        //'scalable': true, // 图片是否可翻转
        'transition': true, // 使用 CSS3 过度
        //'fullscreen': true, // 播放时是否全屏
        'keyboard': true, // 是否支持键盘
        'url': 'data-source' // 设置大图片的 url
    }
})

import runListSubtable from "@/pages/system/agile/run/components/list/subtable";
import runListContent from "@/pages/system/agile/run/components/list/content";
import {
    query,
    queryFooter,
    queryConfig,

    eventApi,
    reportData,
    menuAgile,
    findAgileById,

    delPhysics,
    delPhysicsAll,
    delRecovery,
} from "@/services/system/agile/run/list";
import { mapMutations, mapGetters } from "vuex";
import { listPublic } from "@/services/system/agile/list/designer";
import { selectTableRow, deleteConfirm, operationConfirm } from "@/utils/util";

export default {
    data() {
        return {
            modal: {
                top: "100px",
                centered: false,
                width: "90%",
                formWidth: "850px",
                bodyStyle: {
                    padding: "4px",
                },
            },
            tooltipConfig: {
                showAll: true,
                enterable: true,
                contentMethod: ({ type, column, row, items, _columnIndex }) => {
                    if (type == 'header') {
                        return ''
                    }
                    const { field } = column
                    if (row) {
                        var columnFilter = this.table.columns.filter(f => f.field == field);
                        if (columnFilter != null && columnFilter.length > 0) {
                            var data = columnFilter[0];
                            switch (data.format) {
                                case "Map":
                                    if (row[field]) {
                                        return JSON.parse(row[field]).address;
                                    }
                                    break;
                                case "File":
                                case "Image":
                                    if (row[field]) {
                                        return '';
                                    }
                                    break;
                            }
                            if (data.style.length > 0) {
                                var style = data.style.filter(f => f.value == row[field.replace('_Txt', '')]);
                                return style.length > 0 ? style[0].content : '';
                            }
                        }
                        if (column.title == '操作') {
                            return ''
                        }

                        var value = row[field];
                        if (value) {
                            return value;
                        }
                    }
                    // 其余的单元格使用默认行为
                    return null
                }
            },

            //表格配置信息
            table: {
                page: {
                    param: {
                        current: 1,
                        size: this.eipPage.size,
                        sord: "desc",
                        sidx: "",
                        filters: "",
                        type: 0,
                        rote: {
                            menuId: this.menuId,
                        },
                        configId: this.configId,
                        menuId: this.menuId,
                        haveDataPermission: this.haveDataPermission, //是否具有数据权限
                        isDelete: 1,
                        isPaging: true,
                        cols: [],
                        reportName: '',
                        timeStamp: ''//时间戳:可通过设置该值达到不读取缓存数据效果
                    },

                    total: 0,
                    sizeOptions: this.eipPage.sizeOptions,
                },
                height: this.eipHeaderHeight() - 268 + "px",
                loading: false,
                loaded: false, //配置是否加载完成
                data: [],
                footerData: [],//表尾数据
                search: {
                    option: {
                        labelCol: 8,
                        wrapperCol: 16,
                        num: 6,
                        config: [],
                    },
                },
                columns: [],
                columnExpand: {},//点击展开字段信息
                columnLoadComplate: false,//是否加载配置完毕
                columnResizableChange: false,//是否重新改变了宽度

                subtableNames: {},//子表名称
                title: '',

                seqConfig: {
                    startIndex: 1
                }//排序号
            },

            //敏捷开发配置信息
            agile: {
                configId: null, //配置Id
                config: this.$utils.clone(this.eipTableConfig, true),
            },

            route: {
                name: this.name,
            },

            card: {
                loading: true,
            },

            //子表
            batch: {
                visible: false,
                title: null,
                configId: null, //编辑页面配置Id
                relationId: null,//关联Id
                model: null//子表名称
            },

            interval: null,

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
        menuId: {
            type: String,
        },

        name: {
            type: String,
        },

        haveDataPermission: {
            type: Boolean,
            default: false,
        },
    },
    beforeDestroy() {
        if (this.interval) {
            clearInterval(this.interval);
        }
    },
    watch: {
        //若有改变则赋予属性
        'edit.config.editConfigId'(val) {
            if (val) {
                this.setOptions();
            }
        }
    },
    components: {
        runListSubtable,
        runListContent,
    },

    created() {
        const route = this.$route;
        if (!this.menuId) {
            this.route.name = route.name;
            this.table.page.param.menuId = route.meta.menuId;
            this.table.page.param.configId = route.meta.params.id;
        }
        this.initConfig();
    },
    computed: {
        ...mapGetters("account", ["systemAgile"]),
    },
    methods: {
        /**
    * 调整,收起展开高度
    */
        advanced(advanced) {
            var height = 160;
            if (advanced) {
                //判断具有几行
                var number =
                    this.table.search.option.config.length / this.table.search.option.num;
                if (number % 1 === 0) {
                    number += 1;
                } else {
                    number = Math.ceil(number);
                }
                height = height + (number - 1) * 40;
            }
            this.table.height = this.eipHeaderHeight() - 268 - height;
        },
        /**
   * 恢复
   */
        delRecovery() {
            let that = this;
            selectTableRow(
                that.$refs.recycleBin,
                function (rows) {
                    //提示是否删除
                    operationConfirm(
                        "确认恢复选中数据",
                        function () {
                            that.$loading.show({ text: '正在恢复中...' });
                            let ids = that.$utils.map(rows, (item) => item.RelationId);
                            delRecovery({
                                configId: that.agile.configId,
                                id: ids.join(","),
                            }).then((result) => {
                                that.$loading.hide();
                                if (result.code == that.eipResultCode.success) {
                                    that.search();
                                    that.$emit("ok", result);
                                    that.$message.success(result.msg);
                                } else {
                                    that.$message.error(result.msg);
                                }
                            });
                        },
                        that
                    );
                },
                that,
                false
            );
        },
        /**
     * 删除
     */
        delPhysics() {
            let that = this;
            selectTableRow(
                that.$refs.recycleBin,
                function (rows) {
                    //提示是否删除
                    deleteConfirm(
                        that.eipMsg.delete,
                        function () {
                            that.$loading.show({ text: that.eipMsg.delloading });
                            let ids = that.$utils.map(rows, (item) => item.RelationId);
                            delPhysics({
                                configId: that.agile.configId,
                                id: ids.join(","),
                            }).then((result) => {
                                that.$loading.hide();
                                if (result.code == that.eipResultCode.success) {
                                    that.search();
                                    that.$emit("ok", result);
                                    that.$message.success(result.msg);
                                } else {
                                    that.$message.error(result.msg);
                                }
                            });
                        },
                        that
                    );
                },
                that,
                false
            );
        },
        /**
           * 删除所有
           */
        delPhysicsAll() {
            let that = this;
            deleteConfirm(
                that.eipMsg.delete,
                function () {
                    that.$loading.show({ text: that.eipMsg.delloading });
                    delPhysicsAll({
                        id: that.agile.configId,
                    }).then((result) => {
                        that.$loading.hide();
                        if (result.code == that.eipResultCode.success) {
                            that.search();
                            that.$emit("ok", result);
                            that.$message.success(result.msg);
                        } else {
                            that.$message.error(result.msg);
                        }
                    });
                },
                that
            );
        },

        /**
                * 取消
                */
        cancel() {
            this.$emit("update:visible", false);
        },

        /**
       * 列表排序改变
       */
        tableSort({ sortList }) {
            let that = this;
            //是否分页
            if (that.agile.config.style.paging) {
                that.table.page.param.current = 1;
                that.table.loading = true;
                that.table.page.param.isPaging = true;
                if (sortList.length > 0) {
                    that.table.page.param.sidx = sortList[0].property;
                    that.table.page.param.sord = sortList[0].order;
                } else {
                    var sortDefault = that.agile.config.columns.filter(f => f.isSortDefalut);
                    that.table.page.param.sidx = sortDefault.map(m => m.name).join(',');
                    that.table.page.param.sord = sortDefault.map(m => m.isSortAsc ? "asc" : "desc").join(',');
                }

                that.initData();
            }
        },

        ...mapMutations("account", ["setSystemAgile"]),

        /*
         * 赋予编辑页面属性
         */
        setOptions() {
            var systemAgileData = this.systemAgile.filter(f => f.configId == this.edit.config.editConfigId);
            if (systemAgileData && systemAgileData.length > 0) {
                let editConfig = systemAgileData[0];
                var publicJson = JSON.parse(editConfig.publicJson);
                this.edit.options = publicJson.config;
            }
        },

        /**
         * 列表初始化
         */
        search(filters) {
            this.table.page.param.current = 1;
            this.table.page.param.filters = filters;
            this.initData();
        },

        /**
         * 导出
         */
        reportBusinessData() {
            let that = this;
            that.table.page.param.cols = this.table.columns;
            that.table.page.param.reportName = that.route.name;
            that.$message.loading('导出中', 0)
            reportData(that.table.page.param).then((result) => {
                that.$message.destroy();
                that.$message.success("导出成功");
            });
        },

        /**
          * 初始化配置
          */
        async initConfig() {
            let that = this;
            that.agile.configId = this.table.page.param.configId;
            that.agile.config.columns = [];
            that.table.loading = true;
            var systemAgileData = that.systemAgile.filter(f => f.configId == that.agile.configId);
            if (!(systemAgileData && systemAgileData.length > 0)) {
                var menuAgileData = await menuAgile({ configId: that.agile.configId });
                if (menuAgileData.code === that.eipResultCode.success) {
                    systemAgileData = menuAgileData.data;
                } else {
                    that.$message.error('获取配置错误')
                }
            }
            if (systemAgileData[0].publicJson) {
                that.agile.config = JSON.parse(systemAgileData[0].publicJson);
                await that.initSearch();
                that.initColumns();
                that.initData();
                that.initConnect();
                that.table.title = (systemAgileData[0].remark ? systemAgileData[0].remark + '-' : '') + systemAgileData[0].name;
                that.card.loading = false;
            } else {
                that.$message.error('请配置视图')
            }
            that.table.loaded = true;
        },

        /**
         * 定时器绑定操作栏
         */
        initConnect() {
            let that = this;
            that.interval = setInterval(function () {
                if (!that.$utils.isUndefined(that.$refs.recycleBin)) {
                    that.$refs.recycleBin.connect(that.$refs.runToolbar)
                    clearInterval(that.interval);
                }
            }, 1000);
        },

        /**
         * 渲染列
         */
        initColumns() {
            let that = this;
            var headerFields = [];
            that.agile.config.columns.filter(f => f.isShow && f.header.field.length > 0).forEach((item) => {
                headerFields = headerFields.concat(item.header.field);
            });
            that.agile.config.columns.filter(f => f.isShow).forEach((item) => {
                var column = {
                    subtable: [],
                    header: {
                        title: null,
                        columns: []
                    }
                };
                if (item.header.field.length > 0) {
                    item.header.field.forEach(fitem => {
                        var columnFilter = that.agile.config.columns.filter(f => f.name == fitem);
                        if (columnFilter.length > 0) {
                            var columnObj = columnFilter[0]
                            var columnHeader = {
                                field: columnObj.name,
                                fieldReplaceTxt: columnObj.name.replace(/_Txt*$/, ''),
                                title: columnObj.description,
                                align: columnObj.align,
                                sort: columnObj.isSort,
                                width: columnObj.width,
                                style: columnObj.style,
                                total: columnObj.isTotal,
                                format: columnObj.format,
                                subtable: columnObj.subtable,
                                remark: columnObj.remark,
                                options: columnObj.options,
                                dialog: columnObj.dialog,
                                script: columnObj.script,
                                header: {
                                    title: null,
                                    columns: []
                                }
                            };
                            column.header.columns.push(columnHeader)
                        }
                    })
                    column.header.title = item.header.title;
                } else {
                    //判断字段是否在头部字段里面
                    if (headerFields.filter(f => f == item.name) == 0) {
                        column = {
                            field: item.name,
                            fieldReplaceTxt: item.name.replace(/_Txt*$/, ''),
                            title: item.description,
                            align: item.align,
                            sort: item.isSort,
                            width: item.width,
                            style: item.style,
                            total: item.isTotal,
                            format: item.format,
                            subtable: item.subtable,
                            remark: item.remark,
                            options: item.options,
                            dialog: item.dialog,
                            script: item.script,
                            header: {
                                title: null,
                                columns: []
                            }
                        };
                    }
                }
                that.table.columns.push(column);
            });


            //判断是否有删除
            if (that.table.columns.filter(f => f.field == "UpdateTime").length == 0) {
                that.table.columns.push({
                    field: "UpdateTime",
                    fieldReplaceTxt: "UpdateTime",
                    title: "删除时间",
                    align: "left",
                    sort: true,
                    width: 180,
                    style: [],
                    total: null,
                    format: null,
                    subtable: [],
                    remark: null,
                    options: null,
                    dialog: null,
                    script: null,
                    header: {
                        title: null,
                        columns: []
                    }
                });
            }

            if (that.table.columns.filter(f => f.field == "UpdateUserName").length == 0) {
                that.table.columns.push({
                    field: "UpdateUserName",
                    fieldReplaceTxt: "UpdateUserName",
                    title: "删除人",
                    align: "left",
                    sort: true,
                    width: 130,
                    style: [],
                    total: null,
                    format: null,
                    subtable: [],
                    remark: null,
                    options: null,
                    dialog: null,
                    script: null,
                    header: {
                        title: null,
                        columns: []
                    }
                });
            }
        },

        /**
         * 初始化查询
         */
        async initSearch() {
            let that = this;
            if (that.agile.config.search) {
                if (that.agile.config.search.num) {
                    that.table.search.option.num = that.agile.config.search.num;
                }
                if (that.agile.config.search.labelCol) {
                    that.table.search.option.labelCol = that.agile.config.search.labelCol;
                }
                if (that.agile.config.search.wrapperCol) {
                    that.table.search.option.wrapperCol =
                        that.agile.config.search.wrapperCol;
                }
                that.table.search.option.searchButton = that.agile.config.search.searchButton;
                let config = [];

                for (
                    let index = 0;
                    index < that.agile.config.search.columns.length;
                    index++
                ) {
                    const item = that.agile.config.search.columns[index];
                    switch (item.type) {
                        case "input":
                            config.push({
                                field: item.fields ? item.fields : item.field,
                                op: item.op,
                                placeholder: item.placeholder,
                                title: item.title,
                                value: "",
                                type: "text",
                            });
                            break;
                        case "user":
                            var options = item.options;
                            config.push({
                                field: item.fields ? item.fields : item.field,
                                op: item.op,
                                placeholder: item.placeholder,
                                title: item.title,
                                value: "",
                                type: "user",
                                options: {
                                    multiple: options.multiple, //单选
                                }
                            });
                            break;
                        case "organization":
                            var options = item.options;
                            config.push({
                                field: item.fields ? item.fields : item.field,
                                op: item.op,
                                placeholder: item.placeholder,
                                title: item.title,
                                value: "",
                                type: "organization",
                                options: {
                                    multiple: options.multiple, //单选
                                }
                            });
                            break;
                        case "number":
                            config.push({
                                field: item.fields ? item.fields : item.field,
                                op: item.op,
                                placeholder: item.placeholder,
                                title: item.title,
                                value: "",
                                type: "number",
                            });
                            break;
                        case "bool":
                            config.push({
                                field: item.fields ? item.fields : item.field,
                                op: item.op,
                                placeholder: item.placeholder,
                                title: item.title,
                                value: undefined,
                                type: "bool",
                                options: item.options
                            });
                            break;
                        case "select":
                            //得到具体配置
                            var options = item.options;
                            if (options) {
                                switch (options.source.type) {
                                    case "dynamic":
                                        config.push({
                                            field: item.field.replace(/_Txt*$/, ''),
                                            op: item.op,
                                            placeholder: item.placeholder,
                                            title: item.title,
                                            type: "select",
                                            options: {
                                                multiple: options.multiple, //单选
                                                source: {
                                                    type: "dynamic",
                                                    format: [],
                                                    dynamicConfig: options.source,
                                                },
                                            }
                                        });
                                        break;
                                    case "custom":
                                        config.push({
                                            field: item.field.replace(/_Txt*$/, ''),
                                            op: item.op,
                                            placeholder: item.placeholder,
                                            title: item.title,
                                            type: "select",
                                            options: {
                                                multiple: options.multiple, //单选
                                                source: {
                                                    type: "custom",
                                                    format: options.source.custom,
                                                },
                                            }
                                        });
                                        break;
                                }
                            }
                            break;
                        case "dictionary":
                            var options = item.options;
                            config.push({
                                field: item.field.replace(/_Txt*$/, ''),
                                op: item.op,
                                placeholder: item.placeholder,
                                title: item.title,
                                type: "dictionary",
                                options: {
                                    multiple: options.multiple, //单选
                                    source: {
                                        format: [],
                                        dictionaryId: options.source.dictionaryId,
                                        dictionaryLevel: options.source.dictionaryLevel,
                                    },
                                }
                            });
                            break;
                        case "correlationRecord":
                            //得到具体配置
                            var options = item.options;
                            if (options) {
                                config.push({
                                    field: item.field.replace(/_Txt*$/, ''),
                                    op: item.op,
                                    placeholder: item.placeholder,
                                    title: item.title,
                                    type: "correlationRecord",
                                    options: {
                                        multiple: options.multiple, //单选
                                        source: {
                                            table: options.source.table,//数据源类型
                                            key: options.source.key,
                                            value: options.source.value,//值
                                            sidx: options.source.sidx,//排序字段
                                            sord: options.source.sord,//排序方式
                                            format: []
                                        },
                                    }
                                });
                            }
                            break;
                        case "datetime":
                            switch (item.options.format) {
                                case "YYYY":
                                    item.options.mode = 'year';
                                    break;
                                case "YYYY-MM":
                                    item.options.mode = 'month';
                                    break;
                                default:
                                    item.options.mode = 'date';
                                    break;
                            }
                            config.push({
                                field: item.fields ? item.fields : item.field,
                                op: item.op,
                                open: false,
                                placeholder: item.placeholder,
                                title: item.title,
                                value: item.op == 'daterange' ? [] : '',
                                type: "datetime",
                                options: item.options
                            });
                    }
                }

                that.table.search.option.config = config;
            }
        },

        /**
         * 初始化列表数据
         */
        async initData() {
            let that = this;
            that.table.loading = true;
            that.table.page.param.isPaging = that.agile.config.style.paging;
            //默认排序字段
            if (!that.table.page.param.sidx) {
                var sortDefault = that.agile.config.columns.filter(f => f.isSortDefalut);
                that.table.page.param.sidx = sortDefault.map(m => m.name).join(',');
                that.table.page.param.sord = sortDefault.map(m => m.isSortAsc ? "asc" : "desc").join(',');
            }
            if (that.agile.config.read != null && !that.agile.config.read.cache) {
                that.table.page.param.timeStamp = this.$utils.timestamp();
            }

            const route = this.$route;
            //参数是否带查询（两种传参数：若url里面有filters则用，否则用meta里面的参数）
            var filtersQuery = route.query.filters
            if (!filtersQuery) {
                filtersQuery = route.meta && route.meta.params && route.meta.params.filters;
            } if (filtersQuery) {
                let filters = that.table.page.param.filters;
                let searchFilter = {};
                if (filters) {
                    searchFilter = JSON.parse(filters);
                } else {
                    searchFilter = { "groupOp": "AND", "rules": [], "groups": [] };
                }
                var filtersJson = JSON.parse(unescape(filtersQuery));
                searchFilter.rules.push(filtersJson);
                that.table.page.param.filters = JSON.stringify(searchFilter)
            }
            query(that.table.page.param).then(async function (result) {
                that.table.loading = false;
                if (result.code == that.eipResultCode.success) {
                    let queryData = result.data;
                    //得到表尾数据
                    that.table.footerData = await that.initDataFooter();
                    queryData.data.forEach(d => {
                        d.subtableColumns = [];
                        d.subtableDatas = {};
                    })
                    that.table.data = queryData.data;
                    if (that.agile.config.style.paging) {
                        that.table.page.total = queryData.count;
                    }

                    that.setStartIndex();
                } else {
                    that.$message.error(result.msg)
                }
            });
        },

        /**
         * 重置序列化
         */
        setStartIndex() {
            let page = this.$utils.clone(this.table.page.param, true);
            this.table.seqConfig.startIndex = page.current;
        },


        /**
            * 返回规则
            * @param {*} field
            * @param {*} op
            * @param {*} data
            */
        getRules(field, op, data) {
            return {
                field,
                op,
                data
            }
        },

        /**
         * 初始化表尾数据
         */
        async initDataFooter() {
            let that = this;
            var footer = [];
            var totalColumns = that.table.columns.filter(f => f.total);
            if (totalColumns.length > 0) {
                footer.push("")
                footer.push("合计")
                var result = await queryFooter(that.table.page.param)
                if (result.code == that.eipResultCode.success) {
                    that.table.columns.forEach(item => {
                        footer.push(item.total ? result.data[0][item.field] : "");
                    })
                }
            }
            return [footer]
        },

        /**
         *返回底部数据
         */
        returnDataFooter() {
            return this.table.footerData;
        },

        /**
         * 该方法在展开或关闭触发之前调用，可以通过返回值来决定是否允许继续执行
         */
        toggleExpand({ expanded, column, columnIndex, row, rowIndex }) {
            this.table.columnExpand = column;
            return true;
        },

        /**
         * 加载点开
         */
        async loadExpand({ row }) {
            let that = this;
            var columnsExpand = this.table.columns.filter(f => f.field == this.table.columnExpand.field);
            if (columnsExpand.length > 0) {
                return new Promise(resolve => {
                    var column = columnsExpand[0];
                    column.subtable.forEach(async item => {
                        var configId = item.id;
                        var columns = [];

                        var config = null;
                        var systemAgileData = that.systemAgile.filter(f => f.configId == configId);

                        if (systemAgileData && systemAgileData.length > 0) {
                            config = JSON.parse(systemAgileData[0].publicJson);
                        } else {
                            systemAgileData = await findAgileById(configId);
                            if (systemAgileData.code === that.eipResultCode.success) {
                                config = JSON.parse(systemAgileData.data.publicJson);
                            } else {
                                that.$message.error('获取配置错误')
                            }
                        }
                        that.table.subtableNames[configId] = item.name;
                        // 显示列
                        config.columns.filter(f => f.isShow).forEach((citem) => {
                            var column = {
                                field: citem.name,
                                fieldReplaceTxt: citem.name.replace(/_Txt*$/, ''),
                                title: citem.description,
                                align: citem.align,
                                sort: citem.isSort,
                                width: citem.width,
                                style: citem.style,
                                total: citem.isTotal,
                                format: citem.format,
                            };
                            columns.push(column);
                        });
                        row.subtableColumns.push(columns)

                        //关联条件
                        var where = item.condition;
                        if (where) {
                            var sqlparams = [];
                            var condition = where.split("{");
                            condition.forEach(sitem => {
                                if (sitem.includes("}")) {
                                    var value = row[sitem.split("}")[0]];
                                    sqlparams.push({
                                        key: "{" + sitem.split("}")[0] + "}",
                                        value: value
                                    })
                                }
                            });
                            sqlparams.forEach(pitem => {
                                where = where.replace(pitem.key, pitem.value);
                            });
                        }

                        //查询数据
                        query({
                            isPaging: false,
                            configId: configId,
                            where: where
                        }).then(async function (subtableResult) {
                            if (subtableResult.code == that.eipResultCode.success) {
                                row.subtableDatas[configId] = subtableResult.data.data
                            }

                            //得到属性个数
                            var count = 0;
                            for (var i in row.subtableDatas) {
                                count++
                            }

                            if (column.subtable.length == count) {
                                that.table.columnLoadComplate = true;
                                resolve()
                            }
                        });
                    })
                })
            }
        },

        /**
         *数量改变
         */
        sizeChange(current, pageSize) {
            this.table.page.param.current = 1;
            this.table.page.param.size = pageSize;
            this.reload();
        },

        /**
         *提示状态信息
         */
        operateStatus(result) {
            if (result.code === this.eipResultCode.success) {
                this.reload();
            }
        },

        /**
         * 重新加载
         */
        reload() {
            this.table.page.param.current = 1;
            this.initData();
        },


        /**
         * 悬浮显示
         */
        tooltip({ type, column, row, items, _columnIndex }) {
            const { property } = column;
            if (row) {
                return row[property];
            }
            return null;
        },
        /**
            * 导出
            * @param {*} e 
            */
        exportMenuClick(e) {
            switch (e.key) {
                case "1":
                    this.$refs.runTable.exportData({ type: 'csv' })
                    break;
                case "2":
                    this.$refs.runTable.exportData({
                        data: this.$refs.runTable.getCheckboxRecords()
                    })
                    break;
                case "3":
                    this.$refs.runTable.openExport()
                    break;
                case "4":
                    this.reportBusinessData();
                    break;
            }
            console.log('click', e);
        },
        /**
         * 子表点击详情
         */
        batchDetail(data) {
            var row = data.row;
            var item = data.item;
            this.batch.visible = true;
            this.batch.title = item.title + "（" + row[item.field] + "）";
            this.batch.configId = this.edit.config.editConfigId;
            this.batch.relationId = row["RelationId"];
            this.batch.model = item.field;
        },

    },
};
</script>