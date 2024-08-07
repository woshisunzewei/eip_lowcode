<template>
    <div>
        <a-form-item label="编码规则">
            <eip-editor ref="editor" v-model="value.rule" :height="tinymce.height" :toolbar="tinymce.toolbar"
                :plugins="tinymce.plugins" :menubar="tinymce.menubar"></eip-editor>
            <a-space>
                <a-tag type="primary" @click="number.visible = true">
                    编号
                </a-tag> <a-tag type="primary" @click="datetime.visible = true">
                    时间
                </a-tag>
                <a-tag type="primary" @click="condition.visible = true">
                    引用字段
                </a-tag>
            </a-space>
        </a-form-item>
        <a-form-item>
            <span slot="label">
                选项
            </span>

            <a-checkbox v-model="value.loadDisplay">加载显示编码
                <a-tooltip title="启用后页面加载就会调用规则生成编码，为防止重复,每次打开都会增加一个">
                    <a-icon type="question-circle-o" />
                </a-tooltip>
            </a-checkbox>

            <a-checkbox v-model="value.userOccupation">用户占用
                <a-tooltip title="启用后打开会显示用户上一次生成的编码，直到当前用户处理">
                    <a-icon type="question-circle-o" />
                </a-tooltip>
            </a-checkbox>
        </a-form-item>
        <a-modal v-drag :zIndex="99999" width="370px" :visible="condition.visible" :bodyStyle="{ padding: '1px' }"
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

        <a-modal v-drag :zIndex="1001" width="670px" :visible="number.visible" :bodyStyle="{ padding: '1px' }"
            :destroyOnClose="true" :maskClosable="false" title="编号设置" @cancel="number.visible = false" @ok="numberOk">
            <a-form-model :label-col="labelCol">
                <a-form-model-item :wrapper-col="wrapperCol" label="位数">
                    <a-input-number v-model="value.length" style="width:100%" :min="2"
                        placeholder="请选择"></a-input-number>
                </a-form-model-item>
                <a-form-model-item :wrapper-col="wrapperCol" label="递增" help="勾选时，超出位数继续递增； 取消勾选时，超出位数后从0开始编号">
                    <a-checkbox v-model="value.incremental">编号超出位数后继续递增
                    </a-checkbox>
                </a-form-model-item>
                <a-form-model-item :wrapper-col="wrapperCol" label="开始值">
                    <a-input-number v-model="value.beginNumber" style="width:100%" :min="1"
                        placeholder="修改后将使用新的初始值重新编号"></a-input-number>
                </a-form-model-item>
                <a-form-model-item :wrapper-col="wrapperCol" label="周期重置">
                    <a-select v-model="value.repeat" allow-clear style="width: 100%" placeholder="请选择">
                        <a-select-option value="">不重复</a-select-option>
                        <a-select-option :value="'day'">每天重置</a-select-option>
                        <a-select-option :value="'month'">每月重置</a-select-option>
                        <a-select-option :value="'year'">每年重置</a-select-option>
                    </a-select>
                </a-form-model-item>
            </a-form-model>
        </a-modal>

        <a-modal v-drag :zIndex="1001" width="670px" :visible="datetime.visible" :bodyStyle="{ padding: '1px' }"
            :destroyOnClose="true" :maskClosable="false" title="日期格式" @cancel="datetime.visible = false"
            @ok="datetimeOk">
            <a-form-model :label-col="labelCol">
                <a-form-model-item :wrapper-col="wrapperCol" label="日期格式" help="请输入时间格式化形式：如yyyyMMddHHmmss">
                    <a-input v-model="value.format" placeholder="yyyyMMDD HH:mm:ss"></a-input>
                </a-form-model-item>
            </a-form-model>
        </a-modal>
    </div>
</template>

<script>

/*
 * author 孙泽伟
 * date 2022-03-28
 * description 编号
 */
export default {
    name: "KSerialNo",
    components: {},
    data() {
        return {
            labelCol: { span: 4 },
            wrapperCol: { span: 20 },
            tinymce: {
                plugins: "",
                toolbar: "",
                height: 100,
                menubar: "",
            },

            condition: {
                title: "选择字段",
                visible: false,

                tree: {
                    type: "",
                    style: {
                        overflow: "auto",
                        height: "400px",
                    },
                    data: [],
                    spinning: false,
                },
            },

            number: {
                visible: false,
            },
            datetime: {
                visible: false,
            }
        };
    },
    props: {
        value: {
            type: Object,
            required: true,
        }
    },
    created() {
        this.conditionInit();
    },
    methods: {
        /**
         * 初始化条件
         */
        conditionInit() {
            let that = this;

            that.condition.tree.spinning = true;
            let fieldSchema = this.eipAgileDesigner.kfd.getFieldSchema();
            fieldSchema
                .filter((f) => !["batch", "text", "divider", "dataShow"].includes(f.type))
                .forEach((item) => {
                    that.condition.tree.data.push({
                        key: item.model,
                        isLeaf: true,
                        title: item.label != "" ? item.label : item.model,
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
            that.condition.tree.data.push(this.eipSystemFiled)
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
            if (event.node.getNodeChildren().length == 0) {
                let arrayTrees = this.$utils.toTreeArray(this.condition.tree.data)
                var data = this.$utils.find(arrayTrees, f => f.key == keys[0]);
                if (data) {
                    let html = "<span id='" + encodeURIComponent(JSON.stringify({
                        type: 'column',
                        column: keys[0]
                    })) + "' class='non-editable'>" + data.title + "</span>"
                    this.$refs.editor.insertIndex(html, 0);
                }
                this.conditionCancel();
            }
        },

        /**
         * 编号成功
         */
        numberOk() {
            let that = this;
            //替换已有编号
            const tempDiv = document.createElement('div');
            // 设置div的内容为HTML字符串
            tempDiv.innerHTML = this.value.rule;
            // 查询所有的span标签
            const spans = tempDiv.querySelectorAll('span');
            let html = "<span id='" + encodeURIComponent(JSON.stringify({
                type: 'number',//自然数
                length: this.value.length,
                incremental: this.value.incremental,
                beginNumber: this.value.beginNumber,
                repeat: this.value.repeat,
            })) + "' class='non-editable'>编号</span>"
            var have = false;
            spans.forEach(span => {
                if (span.innerHTML == '编号') {
                    that.value.rule = that.value.rule.replaceAll(span.outerHTML, html)
                    have = true;
                }
            })
            if (!have) {
                this.$refs.editor.insertIndex(html, 0);
            }
            this.number.visible = false;
        },

        /**
         * 时间成功
         */
        datetimeOk() {
            let that = this;
            //替换已有编号
            const tempDiv = document.createElement('div');
            // 设置div的内容为HTML字符串
            tempDiv.innerHTML = this.value.rule;
            // 查询所有的span标签
            const spans = tempDiv.querySelectorAll('span');
            let html = "<span id='" + encodeURIComponent(JSON.stringify({
                type: 'datetime',
                format: this.value.format
            })) + "' class='non-editable'>时间</span>"
            var have = false;
            spans.forEach(span => {
                if (span.innerHTML == '时间') {
                    that.value.rule.replaceAll(span.outerHTML, html)
                    have = true;
                }
            })
            if (!have) {
                this.$refs.editor.insertIndex(html, 0);
            }
            this.datetime.visible = false;
        }
    },
};
</script>

<style lang="less" scoped></style>