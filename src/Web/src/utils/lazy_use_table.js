import Vue from 'vue'
import XEUtils from 'xe-utils'
import VXETablePluginAntd from 'vxe-table-plugin-antd'
import {
    // 核心
    VXETable,
    // 功能模块
    // Icon,
    Filter,
    Menu,
    Edit,
    Export,
    Keyboard,
    Validator,
    // 可选组件
    Column,
    Colgroup,
    // Grid,
    Toolbar,
    // Pager,
    Checkbox,
    // CheckboxGroup,
    // Radio,
    // RadioGroup,
    // RadioButton,
    Input,
    // Textarea,
    Button,
    Modal,
    Tooltip,
    // Form,
    // FormItem,
    // FormGather,
    Select,
    // Optgroup,
    // Option,
    // Switch,
    // List,
    // Pulldown,
    // 表格
    Table
} from 'vxe-table'
import zhCN from 'vxe-table/lib/locale/lang/zh-CN'

// 表格功能
Vue
// .use(Icon)
    .use(Filter)
    .use(Edit)
    .use(Menu)
    .use(Export)
    .use(Keyboard)
    .use(Validator)

// 可选组件
.use(Column)
    .use(Colgroup)
    // .use(Grid)
    .use(Toolbar)
    // .use(Pager)
    .use(Checkbox)
    // .use(CheckboxGroup)
    // .use(Radio)
    // .use(RadioGroup)
    // .use(RadioButton)
    .use(Input)
    // .use(Textarea)
    .use(Button)
    .use(Modal)
    .use(Tooltip)
    // .use(Form)
    // .use(FormItem)
    // .use(FormGather)
    .use(Select)
    // .use(Optgroup)
    // .use(Option)
    // .use(Switch)
    // .use(List)
    // .use(Pulldown)

// 安装表格
.use(Table)

// 给 vue 实例挂载内部对象，例如：
Vue.prototype.$XModal = VXETable.modal
Vue.prototype.$XPrint = VXETable.print
Vue.prototype.$XSaveFile = VXETable.saveFile
Vue.prototype.$XReadFile = VXETable.readFile

VXETable.setup({
    validFullData: 'obsolete', // 将默认为校验全量数据
    i18n: (key, args) => XEUtils.toFormatString(XEUtils.get(zhCN, key), args),
    size: 'small', // 全局尺寸
    zIndex: 999, // 全局 zIndex 起始值，如果项目的的 z-index 样式值过大时就需要跟随设置更大，避免被遮挡
    version: 1.0, // 版本号，对于某些带数据缓存的功能有用到，上升版本号可以用于重置数据
    table: {
        showHeader: true,
        //   keepSource: false,
        //   animat: true,
        //   cloak: false,
        //   delayHover: 250,
        //   showOverflow: null,
        //   showHeaderOverflow: null,
        //   showFooterOverflow: null,
        //   size: null,
        resizable: true,
        //   autoResize: false,
        //   stripe: false,
        border: true,
        columnConfig: {
            // isCurrent: true, isHover: true
        },
        rowConfig: {
            isCurrent: true,
            isHover: true
        },
        //   round: false,
        //   emptyText: '暂无数据',
        radioConfig: {
            trigger: 'row'
        },
        checkboxConfig: {
            trigger: 'row'
        },

        //   sortConfig: {
        //     remote: false,
        //     trigger: 'default',
        //     orders: ['asc', 'desc', null],
        //     sortMethod: null
        //   },
        //   filterConfig: {
        //     remote: false,
        //     filterMethod: null
        //   },
        //   expandConfig: {
        //     trigger: 'default',
        //     showIcon: true
        //   },
        //   treeConfig: {
        //     children: 'children',
        //     hasChild: 'hasChild',
        //     indent: 20,
        //     showIcon: true
        //   },
        printConfig: {},
        tooltipConfig: {
            showAll: true,
            enterable: true,
            contentMethod: ({ type, column, row, items, _columnIndex }) => {
                const { field, title } = column
                if (title == '操作') {
                    return ''
                }
                return null
            }
        },
        //   menuConfig: {
        //     visibleMethod () {}
        //   },
        //   rowId: '_XID', // 行数据的唯一记录ID字段名
        //   editConfig: {
        //     mode: 'cell',
        //     showAsterisk: true
        //   },
        importConfig: {
            modes: ['insert', 'covering']
        },
        exportConfig: {
            modes: ['current', 'selected']
        },
        customConfig: {
            storage: true
        },
        //   scrollX: {
        //     gt: 60
        //   },
        //   scrollY: {
        //     gt: 100
        //   }
        // },
        // grid: {
        //   size: null,
        //   zoomConfig: {
        //     escRestore: true
        //   },
        //   pagerConfig: {
        //     perfect: false
        //   },
        //   toolbarConfig: {
        //     perfect: false
        //   },
        //   proxyConfig: {
        //     autoLoad: true,
        //     message: true,
        //     props: {
        //       list: null, // 用于列表，读取响应数据
        //       result: 'result', // 用于分页，读取响应数据
        //       total: 'page.total' // 用于分页，读取总条数
        //     }
        //     beforeItem: null,
        //     beforeColumn: null,
        //     beforeQuery: null,
        //     afterQuery: null,
        //     beforeDelete: null,
        //     afterDelete: null,
        //     beforeSave: null,
        //     afterSave: null
        //   }
        // },
        // pager: {
        //   size: null,
        //   autoHidden: false,
        //   perfect: true,
        //   pageSize: 10,
        //   pagerCount: 7,
        //   pageSizes: [10, 15, 20, 50, 100],
        //   layouts: ['PrevJump', 'PrevPage', 'Jump', 'PageCount', 'NextPage', 'NextJump', 'Sizes', 'Total']
        // },
        // form: {
        //   preventSubmit: false
        //   size: null,
        //   colon: false,
        //   validConfig: {
        //     autoPos: true
        //   },
        //   tooltipConfig: {
        //     enterable: true
        //   },
        //   titleAsterisk: true
        // },
        // input: {
        //   size: null,
        //   transfer: false
        //   parseFormat: 'yyyy-MM-dd HH:mm:ss.SSS',
        //   labelFormat: '',
        //   valueFormat: '',
        //   startDay: 1,
        //   digits: 2,
        //   controls: true
        // },
        // textarea: {
        //   size: null
        //   autosize: {
        //     minRows: 1,
        //     maxRows: 10
        //   }
        // },
        // select: {
        //   size: null,
        //   transfer: false,
        //   optionConfig: {
        //     keyField: '_X_OPTION_KEY' // 选项数据的唯一记录ID字段名
        //   },
        //   multiCharOverflow: 8
        // },
        // toolbar: {
        //   size: null,
        //   import: {
        //     mode: 'covering'
        //   },
        //   export: {
        //     types: ['csv', 'html', 'xml', 'txt']
        //   },
        //   custom: {
        //     isFooter: true
        //   },
        //   buttons: [],
        //   tools: []
        // },
        // button: {
        //   size: null,
        //   transfer: false
        // },
        // radio: {
        //   size: null
        // },
        // checkbox: {
        //   size: null
        // },
        // switch: {
        //   size: null
        // },
        // modal: {
        //   // size: null,
        //   minWidth: 340,
        //   minHeight: 200,
        //   lockView: true,
        //   mask: true,
        //   duration: 3000,
        //   marginSize: 0,
        //   dblclickZoom: true,
        //   showTitleOverflow: true
        //   storage: false
        // },
        // list: {
        //   scrollY: {
        //     gt: 100
        //   }
    },
    icon: {
        // loading
        LOADING: 'vxe-icon-spinner roll vxe-loading--default-icon',

        // table
        TABLE_SORT_ASC: 'vxe-icon-caret-up',
        TABLE_SORT_DESC: 'vxe-icon-caret-down',
        TABLE_FILTER_NONE: 'vxe-icon-funnel',
        TABLE_FILTER_MATCH: 'vxe-icon-funnel',
        TABLE_EDIT: 'vxe-icon-edit',
        TABLE_HELP: 'vxe-icon-question-circle-fill',
        TABLE_TREE_LOADED: 'vxe-icon-spinner roll',
        TABLE_TREE_OPEN: 'vxe-icon-caret-right rotate90',
        TABLE_TREE_CLOSE: 'vxe-icon-caret-right',
        TABLE_EXPAND_LOADED: 'vxe-icon-spinner roll',
        TABLE_EXPAND_OPEN: 'vxe-icon-arrow-right rotate90',
        TABLE_EXPAND_CLOSE: 'vxe-icon-arrow-right',
        TABLE_CHECKBOX_CHECKED: 'vxe-icon-checkbox-checked',
        TABLE_CHECKBOX_UNCHECKED: 'vxe-icon-checkbox-unchecked',
        TABLE_CHECKBOX_INDETERMINATE: 'vxe-icon-checkbox-indeterminate',
        TABLE_RADIO_CHECKED: 'vxe-icon-radio-checked',
        TABLE_RADIO_UNCHECKED: 'vxe-icon-radio-unchecked',

        // button
        BUTTON_DROPDOWN: 'vxe-icon-arrow-down',
        BUTTON_LOADING: 'vxe-icon-spinner roll',

        // select
        SELECT_LOADED: 'vxe-icon-spinner roll',
        SELECT_OPEN: 'vxe-icon-caret-down rotate180',
        SELECT_CLOSE: 'vxe-icon-caret-down',

        // pager
        PAGER_JUMP_PREV: 'vxe-icon-arrow-double-left',
        PAGER_JUMP_NEXT: 'vxe-icon-arrow-double-right',
        PAGER_PREV_PAGE: 'vxe-icon-arrow-left',
        PAGER_NEXT_PAGE: 'vxe-icon-arrow-right',
        PAGER_JUMP_MORE: 'vxe-icon-ellipsis-h',

        // input
        INPUT_CLEAR: 'vxe-icon-error-circle-fill',
        INPUT_PWD: 'vxe-icon-eye-fill',
        INPUT_SHOW_PWD: 'vxe-icon-eye-fill-close',
        INPUT_PREV_NUM: 'vxe-icon-caret-up',
        INPUT_NEXT_NUM: 'vxe-icon-caret-down',
        INPUT_DATE: 'vxe-icon-calendar',
        INPUT_SEARCH: 'vxe-icon-search',

        // modal
        MODAL_ZOOM_IN: 'vxe-icon-square',
        MODAL_ZOOM_OUT: 'vxe-icon-maximize',
        MODAL_CLOSE: 'vxe-icon-close',
        MODAL_INFO: 'vxe-icon-info-circle-fill',
        MODAL_SUCCESS: 'vxe-icon-success-circle-fill',
        MODAL_WARNING: 'vxe-icon-warnion-circle-fill',
        MODAL_ERROR: 'vxe-icon-error-circle-fill',
        MODAL_QUESTION: 'vxe-icon-question-circle-fill',
        MODAL_LOADING: 'vxe-icon-spinner roll',

        // toolbar
        TOOLBAR_TOOLS_REFRESH: 'vxe-icon-repeat',
        TOOLBAR_TOOLS_REFRESH_LOADING: 'vxe-icon-repeat roll',
        TOOLBAR_TOOLS_IMPORT: 'vxe-icon-upload',
        TOOLBAR_TOOLS_EXPORT: 'vxe-icon-download',
        TOOLBAR_TOOLS_PRINT: 'vxe-icon-print',
        TOOLBAR_TOOLS_FULLSCREEN: 'vxe-icon-fullscreen',
        TOOLBAR_TOOLS_MINIMIZE: 'vxe-icon-minimize',
        TOOLBAR_TOOLS_CUSTOM: 'vxe-icon-custom-column',

        // form
        FORM_PREFIX: 'vxe-icon-question-circle-fill',
        FORM_SUFFIX: 'vxe-icon-question-circle-fill',
        FORM_FOLDING: 'vxe-icon-arrow-up rotate180',
        FORM_UNFOLDING: 'vxe-icon-arrow-up'
    }
})

// 自定义全局的格式化处理函数
VXETable.formats.mixin({
    // 格式化性别
    formatSex({ cellValue }) {
        return cellValue ? (cellValue === '1' ? '男' : '女') : ''
    },
    // 格式化下拉选项
    formatSelect({ cellValue }, list) {
        const item = list.find(item => item.value === cellValue)
        return item ? item.label : ''
    },
    // 格式化日期，默认 yyyy-MM-dd HH:mm:ss
    formatDate({ cellValue }, format) {
        return XEUtils.toDateString(cellValue, format || 'yyyy-MM-dd HH:mm:ss')
    },
    // 四舍五入金额，每隔3位逗号分隔，默认2位数
    formatAmount({ cellValue }, digits = 2) {
        return XEUtils.commafy(XEUtils.toNumber(cellValue), { digits })
    },
    // 格式化银行卡，默认每4位空格隔开
    formatBankcard({ cellValue }) {
        return XEUtils.commafy(XEUtils.toValueString(cellValue), { spaceNumber: 4, separator: ' ' })
    },
    // 四舍五入,默认两位数
    formatFixedNumber({ cellValue }, digits = 2) {
        return XEUtils.toFixed(XEUtils.round(cellValue, digits), digits)
    },
    // 向下舍入,默认两位数
    formatCutNumber({ cellValue }, digits = 2) {
        return XEUtils.toFixed(XEUtils.floor(cellValue, digits), digits)
    }
})
import 'vxe-table-plugin-antd/dist/style.css'
VXETable.use(VXETablePluginAntd)

//通用工具
Vue.prototype.$utils = XEUtils;