<template>
    <div>
        <a-card size="small">
            <a-steps :current="current" @change="onChange">
                <a-step v-for="item in steps" :key="item.title" :title="item.title" />
            </a-steps>
            <div class="steps-content">
                <a-card size="small" v-if="current == 0" :bordered="false">
                    <div class="padding-bottom-xs">
                        <a-select style="width:200px" @change="change" allow-clear v-model="dataBaseId"
                            placeholder="请选择系统">
                            <a-select-option v-for="(item, index) in dataBase" :value="item.dataBaseId" :key="index">
                                {{ item.name }} </a-select-option>
                        </a-select>
                    </div>
                    <vxe-table ref="table" :loading="table.loading" :column-config="{ isCurrent: true, isHover: true }"
                        :row-config="{ isCurrent: true, isHover: true }" :height="table.height" :export-config="{}"
                        :print-config="{}" :data="table.data">
                        <template #loading>
                            <a-spin></a-spin>
                        </template>
                        <template #empty>
                            <eip-empty />
                        </template>
                        <vxe-column type="checkbox" width="40" align="center" fixed="left">
                            <template #header="{ checked, indeterminate }">
                                <span @click.stop="$refs.table.toggleAllCheckboxRow()">
                                    <a-checkbox :indeterminate="indeterminate" :checked="checked">
                                    </a-checkbox>
                                </span>
                            </template>
                            <template #checkbox="{ row, checked, indeterminate }">
                                <span @click.stop="$refs.table.toggleCheckboxRow(row)">
                                    <a-checkbox :indeterminate="indeterminate" :checked="checked">
                                    </a-checkbox>
                                </span>
                            </template>
                        </vxe-column>
                        <vxe-column type="seq" title="序号" width="60"></vxe-column>
                        <vxe-column field="name" title="表名称" width="400" showOverflow="ellipsis"></vxe-column>
                        <vxe-column field="description" title="描述" showOverflow="ellipsis"></vxe-column>
                        <vxe-column field="isFromAgile" title="敏捷开发" width="100" align="center">
                            <template #default="{ row }">
                                <a-tag v-if="row.isFromAgile" color="red"> 敏 </a-tag>
                            </template>
                        </vxe-column>
                    </vxe-table>
                </a-card>
                <a-card size="small" v-if="current == 1" :bordered="false">
                    <a-tabs v-if="table.chosen.length > 0" type="card" :default-active-key="table.chosen[0].Table">
                        <a-tab-pane :tab="item.Table + (item.Description ? ('【' + item.Description + '】') : '')"
                            v-for="(item, index) in table.chosen" :key="item.Table">
                            <a-tabs @change="createCodeChange" v-if="table.createCode.length > 0">
                                <a-tab-pane key="1" tab="实体">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].entities" language="csharp"></eip-code>
                                </a-tab-pane>
                                <a-tab-pane key="2" tab="查询Dto">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].findDto" language="csharp"></eip-code>
                                </a-tab-pane>
                                <a-tab-pane key="3" tab="控制器">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].controller" language="csharp"></eip-code>
                                </a-tab-pane>
                                <a-tab-pane key="4" tab="Logic接口">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].logic" language="csharp"></eip-code>
                                </a-tab-pane>
                                <a-tab-pane key="5" tab="Logic接口实现">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].logicImpl" language="csharp"></eip-code>
                                </a-tab-pane>
                                <a-tab-pane key="6" tab="Repository接口">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].repository" language="csharp"></eip-code>
                                </a-tab-pane>
                                <a-tab-pane key="7" tab="Repository接口实现">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].repositoryImpl" language="csharp"></eip-code>
                                </a-tab-pane>
                                <a-tab-pane key="8" tab="IDbContext">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].iDbContext" language="csharp"></eip-code>
                                </a-tab-pane>
                                <a-tab-pane key="9" tab="SqlDbContext">
                                    <eip-code :readOnly="true" :height="code.height"
                                        :value="table.createCode[index].sqlDbContext" language="csharp"></eip-code>
                                </a-tab-pane>
                                <div slot="tabBarExtraContent">
                                    <a-button type="primary" @click="downloadFile(index)"><a-icon
                                            type="file-text" />下载当前文件</a-button>
                                </div>
                            </a-tabs>
                        </a-tab-pane>
                        <div slot="tabBarExtraContent">
                            <a-space>
                                <a-button type="primary" @click="downloadFileAll()"><a-icon
                                        type="file-zip" />下载所有文件</a-button>

                                <a-button v-if="current == steps.length - 1" type="primary" @click="createFile">
                                    <a-icon type="folder" /> 生成文件到系统目录
                                </a-button>
                            </a-space>
                        </div>
                    </a-tabs>
                </a-card>
            </div>
            <div class="steps-action">
                <a-space>
                    <a-button v-if="current > 0" style="margin-left: 8px" @click="prev">
                        <a-icon type="step-backward"></a-icon> 上一步
                    </a-button>
                    <a-button v-if="current < steps.length - 1" type="primary" @click="next">
                        <a-icon type="step-forward"></a-icon> 下一步
                    </a-button>
                </a-space>
            </div>
        </a-card>
    </div>
</template>
<script>
import {
    table,
    file,
    code
} from "@/services/system/agile/codegeneration/index";
import { query } from "@/services/system/database/list";
import JSZip from "jszip";
export default {
    components: {
    },
    data() {
        return {
            current: 0,
            steps: [
                {
                    title: '生成表选择',
                },
                {
                    title: '查看代码',
                }
            ],

            table: {
                height: window.innerHeight - 286,
                data: [],
                loading: true,
                chosen: [],//选择表
                createCode: []
            },
            code: {
                key: "1",
                height: window.innerHeight - 370 + "px",
            },

            dataBaseId: '',
            dataBase: []
        };
    },
    created() {
        this.initDataBase();

    },
    methods: {
        change() {
            this.tableInit();
        },
        /**
  * 初始化数据库
  */
        initDataBase() {
            let that = this;
            query().then(function (result) {
                if (result.code == that.eipResultCode.success) {
                    that.dataBase = result.data;
                    that.dataBaseId = that.dataBase[0].dataBaseId;
                }
                that.tableInit();
            });
        },
        /**
         * 
         * @param {*} current 
         */
        onChange(current) {
            var changeResult = this.changeEvent();
            if (changeResult) {
                this.current = current;
            }
        },
        /**
         * 
         */
        createCodeChange(e) {
            this.code.key = e;
        },
        /**
         * 下载文件
         */
        downloadFile(index) {
            var content = "", filename = "";
            var table = this.table.chosen[index].Table,
                tableNameSplit = table.split("_");
            var replaceTableName = "";
            tableNameSplit.forEach(f => {
                replaceTableName += this.ucfirst(f);
            })
            if (this.code.key == "1") {
                content = this.table.createCode[index].entities;
                filename = replaceTableName;
            }
            if (this.code.key == "2") {
                content = this.table.createCode[index].findDto;
                filename = replaceTableName + "FindDto";
            }
            if (this.code.key == "3") {
                content = this.table.createCode[index].controller;
                filename = replaceTableName + "Controller";
            }
            if (this.code.key == "4") {
                content = this.table.createCode[index].logic;
                filename = "I" + replaceTableName + "Logic";
            }
            if (this.code.key == "5") {
                content = this.table.createCode[index].logicImpl;
                filename = replaceTableName + "Logic";
            }
            if (this.code.key == "6") {
                content = this.table.createCode[index].repository;
                filename = "I" + replaceTableName + "Repository";
            }
            if (this.code.key == "7") {
                content = this.table.createCode[index].repositoryImpl;
                filename = replaceTableName + "Repository";
            }
            if (this.code.key == "8") {
                content = this.table.createCode[index].iDbContext;
                filename = "IDbContext";
            }
            if (this.code.key == "9") {
                content = this.table.createCode[index].sqlDbContext;
                filename = "SqlDbContext";
            }
            this.$XSaveFile({
                filename: filename,
                type: 'cs',
                content: content
            })
        },

        /**
        * 下载文件
        */
        downloadFileAll() {
            var zip = new JSZip();
            this.table.chosen.forEach((item, index) => {
                var content = "", filename = "";
                var table = this.table.chosen[index].Table,
                    tableNameSplit = table.split("_");
                var replaceTableName = "";
                tableNameSplit.forEach(f => {
                    replaceTableName += this.ucfirst(f);
                })
                content = this.table.createCode[index].entities;
                filename = replaceTableName + ".cs";
                zip.file(filename, content);

                content = this.table.createCode[index].findDto;
                filename = replaceTableName + "FindDto.cs";
                zip.file(filename, content);

                content = this.table.createCode[index].controller;
                filename = replaceTableName + "Controller.cs";
                zip.file(filename, content);

                content = this.table.createCode[index].logic;
                filename = "I" + replaceTableName + "Logic.cs";
                zip.file(filename, content);

                content = this.table.createCode[index].logicImpl;
                filename = replaceTableName + "Logic.cs";
                zip.file(filename, content);

                content = this.table.createCode[index].repository;
                filename = "I" + replaceTableName + "Repository.cs";
                zip.file(filename, content);

                content = this.table.createCode[index].repositoryImpl;
                filename = replaceTableName + "Repository.cs";
                zip.file(filename, content);

                content = this.table.createCode[index].iDbContext;
                filename = replaceTableName + "_IDbContext.cs";
                zip.file(filename, content);

                content = this.table.createCode[index].sqlDbContext;
                filename = replaceTableName + "_SqlDbContext.cs";
                zip.file(filename, content);
            })
            let that = this;
            zip.generateAsync({ type: "blob" })
                .then(function (content) {
                    that.$XSaveFile({ filename: '代码生成文件', type: 'zip', content: content })
                });
        },
        /**
         * 初始化所有表
         */
        tableInit() {
            let that = this;
            that.table.loading = true;
            table({
                id: this.dataBaseId
            }).then((result) => {
                that.table.data = result.data;
                that.table.loading = false;
            });
        },
        /**
         * 首字母大写
         * @param {*} str 
         */
        ucfirst(str) {
            return str.slice(0, 1).toUpperCase() + str.slice(1);
        },

        /**
         * 下一步
         */
        next() {
            var changeResult = this.changeEvent();
            if (changeResult) {
                this.current++;
            }
        },

        /**
         * 
         */
        changeEvent() {
            //判断当前节点
            let that = this;
            var changeResult = true;
            //生成代码
            if (this.current == 0) {
                that.table.chosen = [];
                //判断是否选择了表
                var rows = that.$refs.table.getCheckboxRecords();
                if (rows.length == 0) {
                    that.$message.warning("请选择需要生成表");
                    changeResult = false;
                } else {
                    that.table.createCode = [];
                    rows.forEach(item => {
                        that.table.chosen.push({
                            Table: item.name,
                            Description: item.description,
                            Id: that.dataBaseId
                        });
                    })
                    code(that.table.chosen).then(result => {
                        that.table.createCode = result;
                        changeResult = true;
                    })
                }
            }
            return changeResult;
        },

        /**
         * 生成文件
         */
        createFile() {
            let that = this;
            that.$loading.show({ text: '文件生成中' });
            file(that.table.createCode).then(result => {
                if (result.code === that.eipResultCode.success) {
                    that.$message.success(result.msg);
                } else {
                    that.$message.error(result.msg);
                }
                that.loading = false;
                that.$loading.hide();
            })
        },
        prev() {
            this.current--;
        },
    },
};
</script>
<style scoped lang="less">
.steps-content {
    margin-top: 16px;
    border: 1px dashed #e9e9e9;
    border-radius: 6px;
    background-color: #fafafa;
    min-height: 200px;
}

.steps-action {
    margin-top: 24px;
    float: right;
}

/deep/ .CodeMirror {
    height: 600px !important;
}
</style>