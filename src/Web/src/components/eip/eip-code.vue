<template>
    <div ref="main" :style="{ width: width, height, height }"></div>
</template>

<script>
//https://microsoft.github.io/monaco-editor/
//设置语言包
import { setLocaleData } from "monaco-editor-nls";
import zh_CN from "monaco-editor-nls/locale/zh-hans";
setLocaleData(zh_CN)

export default {
    name: 'eip-code',
    props: {
        readOnly: {
            type: Boolean,
            default: false,
        },
        value: {
            type: String,
            default: "",
        },
        width: {
            type: String,
            default: "100%",
        },
        height: {
            type: String,
            default: "100%",
        },
        language: {
            type: String,
            default: "typescript",
        },
    },
    data() {
        return {
            monacoEditor: null
        };
    },

    mounted() {
        this.init();
    },
    methods: {
        setEditorBehavior() {
            let th = this
            const monaco = require("monaco-editor/esm/vs/editor/editor.api");
            monaco.languages.registerCompletionItemProvider('javascript', {
                provideCompletionItems(model, pos) {
                    return {
                        suggestions: [
                            {
                                label: "row-col",
                                insertText: `[{"cols":[{"span":6},{"span":6}]}]`,
                                kind: monaco.languages.CompletionItemKind["Snippet"],
                                detail: "插入行列",
                            }
                            // ,
                            // {
                            //     label: "for-full",
                            //     insertText: beautify_js(`for (let i = 0; i < len; i++) { }`, beautify_options),
                            //     kind: monaco.languages.CompletionItemKind["Snippet"],
                            //     detail: "完整for循环",
                            // },
                            // {
                            //     label: "click-options",
                            //     insertText: beautify_js(`click:{func:"",addr: "",options: {},callBack:function (options, context) {}}`, beautify_options),
                            //     kind: monaco.languages.CompletionItemKind["Snippet"],
                            //     detail: "click默认传参",
                            // },
                            // {
                            //     label: "element-text",
                            //     insertText: beautify_js(`elements: [{type: "string",className: "",descrip: ""}]`, beautify_options),
                            //     kind: monaco.languages.CompletionItemKind["Snippet"],
                            //     detail: "元素默认文本",
                            // },
                            // {
                            //     label: "element-input",
                            //     insertText: beautify_js(`elements: [{type: "input",className: "",classPrintName: "",placeholder: "",code: "",model: true,name: "_",_: allDatas._}]`, beautify_options),
                            //     kind: monaco.languages.CompletionItemKind["Snippet"],
                            //     detail: "元素默认输入",
                            // }
                        ],
                        dispose() {
                            const line = pos.lineNumber
                            const column = pos.column
                            if (model.getValueInRange(new monaco.Range(line, column - 1, line, column)) !== "/") {
                                return
                            }
                            th.editorBox.executeEdits("", [//这里是为了删除快捷提示输入的斜杠
                                {
                                    range: new monaco.Range(line, column - 1, line, column),
                                    text: null
                                }
                            ])
                        }
                    }
                },
                triggerCharacters: ['/']//输入斜杠或者label里面的首字母都会进行提示
            })
        },

        init() {
            const monaco = require("monaco-editor/esm/vs/editor/editor.api");
            // 使用 - 创建 monacoEditor 对象
            this.monacoEditor = monaco.editor.create(this.$refs.main, {
                theme: "vs-dark", // 主题
                value: this.value, // 默认显示的值
                language: this.language,
                folding: true, // 是否折叠
                foldingHighlight: true, // 折叠等高线
                overviewRulerBorder: false, // 不要滚动条的边框
                foldingStrategy: "indentation", // 折叠方式  auto | indentation
                showFoldingControls: "always", // 是否一直显示折叠 always | mouseover
                disableLayerHinting: true, // 等宽优化
                emptySelectionClipboard: false, // 空选择剪切板
                selectionClipboard: false, // 选择剪切板
                automaticLayout: true, // 自动布局
                // minimap: { // 关闭小地图
                //     enabled: false,
                // },
                codeLens: false, // 代码镜头
                scrollBeyondLastLine: false, // 滚动完最后一行后再滚动一屏幕
                colorDecorators: true, // 颜色装饰器
                accessibilitySupport: "off", // 辅助功能支持  "auto" | "off" | "on"
                lineNumbers: "on", // 行号 取值： "on" | "off" | "relative" | "interval" | function
                lineNumbersMinChars: 5, // 行号最小字符   number
                enableSplitViewResizing: false,
                readOnly: this.readOnly, //是否只读  取值 true | false
                wordWrap: "on", // 自动换行
            });

            //事件
            this.monacoEditor.onDidChangeModelContent(() => {
                let value = this.monacoEditor.getValue() // 获取编辑器中的语句
                this.$emit("update:value", value);
            })
        },
    },
};
</script>

<style scoped>
/deep/ .margin {
    margin: 0;
}
</style>