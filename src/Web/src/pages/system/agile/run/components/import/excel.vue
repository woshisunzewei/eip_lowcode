<template>
    <a-drawer :destroyOnClose="true" :maskClosable="false" width="100%" :visible="visible" :closable="false"
        :footer="null" @close="cancel" :bodyStyle="bodyStyle">
        <a-spin :spinning="spinning">
            <div id="x-spreadsheet-excel"></div>
        </a-spin>
        <div style="position: absolute;top:2px;right: 10px;">
            <a-space>
                <a-button class="margin-right" type="dashed" @click="okMsg">
                    <a-icon type="save"></a-icon>确定导入</a-button>
                <a-tooltip title="关闭">
                    <a @click="cancel" style="color: #ff4d4f">
                        <a-icon type="close" style="font-size: 14px;" />
                        <span>关闭</span>
                    </a>
                </a-tooltip>
            </a-space>
        </div>
        <run-list-import-excel-comfirm ref="importData" v-if="importData.visible"
            @importBusinessData="importBusinessData" :visible.sync="importData.visible"
            :title="importData.title"></run-list-import-excel-comfirm>
    </a-drawer>
</template>

<script>
import runListImportExcelComfirm from "@/pages/system/agile/run/components/list/importExcel";
//https://www.jianshu.com/p/20b43ba0fe1f
//https://github.com/myliang/x-spreadsheet
//https://blog.csdn.net/lee576/article/details/128172427
import { mapGetters } from "vuex";
import Spreadsheet from "x-data-spreadsheet";
import 'x-data-spreadsheet/dist/locale/zh-cn'
Spreadsheet.locale('zh-cn', (window.x_spreadsheet).$messages['zh-cn'])

import {
    importDataTable
} from "@/services/system/agile/run/list";
export default {
    components: {
        runListImportExcelComfirm
    },
    name: "import-excel",
    computed: {
        ...mapGetters("account", ["user", "systemAgile"]),
    },
    data() {
        return {

            //导入数据
            importData: {
                visible: false,
                title: null,
            },
            //表格配置信息
            filterName: '',
            spinning: true,
            bodyStyle: {
                padding: "0px",
            },

            spreadsheet: null
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
        model: {
            type: String,
        },
        columns: Array
    },

    mounted() {
        let that = this;
        setTimeout(function () { that.init(); }, 500)
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
        init() {
            var header = {};
            this.columns.forEach((item, index) => {
                header[index] = ({
                    'text': item.title, editable: false
                })
            })
            const data = {
                name: this.title,
                rows: {
                    0: {
                        cells: header
                    }
                }
            }
            this.spreadsheet = new Spreadsheet("#x-spreadsheet-excel", {
                mode: 'edit', // edit | read
                showToolbar: true,// 顶部工具栏
                showGrid: true,
                showContextmenu: true, // 切换右键菜单显示状态
                showBottomBar: true,  // 底部工具栏
                view: {
                    height: () => document.documentElement.clientHeight,
                    width: () => document.documentElement.clientWidth,
                },
                row: {
                    len: 10000,
                    height: 25,
                },
                col: {
                    len: 25,
                    width: 100,
                    indexWidth: 60,
                    minWidth: 60,
                },
                style: {
                    bgcolor: '#ffffff',
                    align: 'left',
                    valign: 'middle',
                    textwrap: false,
                    strike: false,
                    underline: false,
                    color: '#0a0a0a',
                    font: {
                        name: 'Helvetica',
                        size: 10,
                        bold: false,
                        italic: false,
                    },
                },
            })
                .loadData(data) // load data
                .change(data => {
                    // save data to db
                    console.log(data)
                });
            // 使用插件API添加自定义按钮
            // spreadsheet.addToolbarItem({
            //     title: '自定义按钮',
            //     click: function () {
            //         alert('按钮被点击！');
            //         // 在这里添加你的自定义逻辑
            //     }
            // });
            // 禁止右键，右键有设置单元格可编辑菜单，防止修改表头
            //spreadsheet.sheet.contextMenu.isHide = true
            // 设置冻结
            this.spreadsheet.sheet.data.setFreeze(1, 0)

            // 编辑单元格触发
            // s.on('cell-selected', (cell, ri, ci) => {
            // console.log('cell:', cell, ', ri:', ri, ', ci:', ci);
            // }).on('cell-edited', (text, ri, ci) => {
            //     console.log('text:', text, ', ri: ', ri, ', ci:', ci);
            // });

            this.spreadsheet.validate()

            this.spinning = false;
        },
        okMsg() {
            this.importData.title = this.title + "导入提示";
            this.importData.visible = true;
        },

        /**
         * 
         * @param {]} type 
         */
        importBusinessData(type) {
            var exportExcelData = this.spreadsheet.getData()
            var data = [];
            let that = this;
            for (let i in exportExcelData[0].rows) {
                var dataObj = {};
                if (i != "0" && i != "len") {
                    const element = exportExcelData[0].rows[i];
                    for (let ci in element.cells) {
                        const cell = element.cells[ci];
                        dataObj[that.columns[ci].title] = cell.text;
                    }
                    data.push(dataObj)
                }
            }
            const formData = new FormData();
            that.$message.loading({
                content: "导入中...",
                duration: 0,
            });
            this.importData.visible = false;
            formData.append("table", JSON.stringify(data));
            formData.append("cols", JSON.stringify(this.columns));
            formData.append("configId", this.configId);
            formData.append("type", type);
            importDataTable(formData)
                .then((result) => {
                    that.$message.destroy();
                    if (result.code == that.eipResultCode.success) {
                        that.$message.success(result.msg);
                        that.cancel();
                        that.$emit("reload");
                    } else {
                        var msg = [];
                        try {
                            var datas = JSON.parse(result.msg);
                            datas.forEach((item) => {
                                var m = "";
                                m += "第" + item.RowIndex + "行，";
                                for (let key in item.FieldErrors) {
                                    m += item.FieldErrors[key] + ",";
                                }
                                msg.push(m);
                            });
                            that.upload.msg = msg.join("<br/>");
                            that.upload.visible = true;
                        } catch (error) {
                            result.data.forEach((item) => {
                                msg.push(item);
                            });
                            that.upload.msg = msg.join("<br/>");
                            that.upload.visible = true;
                        }
                    }
                })
                .catch((e) => {
                    that.$message.destroy();
                    that.$message.error("上传出错");
                });
        }
    },
};
</script>

<style lang="less"></style>