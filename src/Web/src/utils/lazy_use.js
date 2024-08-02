import Vue from 'vue'

import {
    AutoComplete,
    Button,
    Drawer,
    Empty,
    Menu,
    Icon,
    Tabs,
    Select,
    Radio,
    InputNumber,
    Input,
    Spin,
    DatePicker,
    Modal,
    Pagination,
    Tag,
    Divider,
    ConfigProvider,
    Form,
    FormModel,
    Breadcrumb,
    Checkbox,
    Tooltip,
    Card,
    Row,
    Col,
    Switch,
    notification,
    message,
    Upload,
    Tree,
    Progress,
    Dropdown,
    Descriptions,
    Popconfirm,
    Steps,
    Space,
    Result,
    Layout,
    TreeSelect,
    Collapse,
    Popover,
    Alert,
    Badge,
    Avatar,
    Timeline,
    Slider,
    TimePicker,
    Skeleton,
    List,
    Cascader,
    Rate
} from 'ant-design-vue'
Vue.use(AutoComplete)
Vue.use(Drawer)
Vue.use(Empty)
Vue.use(Button)
Vue.use(Menu)
Vue.use(Icon)
Vue.use(Tabs)
Vue.use(Select)
Vue.use(Radio)
Vue.use(InputNumber)
Vue.use(Input)
Vue.use(Spin)
Vue.use(DatePicker)
Vue.use(Modal)
Vue.use(Pagination)
Vue.use(Tag)
Vue.use(Divider)
Vue.use(ConfigProvider)
Vue.use(Form)
Vue.use(FormModel)
Vue.use(Breadcrumb)
Vue.use(Checkbox)
Vue.use(Tooltip)
Vue.use(Card)
Vue.use(Row)
Vue.use(Col)
Vue.use(Switch)
Vue.use(Upload)
Vue.use(Tree)
Vue.use(Progress)
Vue.use(Dropdown)
Vue.use(Descriptions)
Vue.use(Popconfirm)
Vue.use(Steps)
Vue.use(Space)
Vue.use(Result)
Vue.use(Layout)
Vue.use(TreeSelect)
Vue.use(Collapse)
Vue.use(Popover)
Vue.use(Alert)
Vue.use(Badge)
Vue.use(Avatar)
Vue.use(Timeline)
Vue.use(Slider)
Vue.use(TimePicker)
Vue.use(Skeleton)
Vue.use(List)
Vue.use(Cascader)
Vue.use(Rate)
Vue.prototype.$confirm = Modal.confirm
Vue.prototype.$message = message
Vue.prototype.$notification = notification
Vue.prototype.$info = Modal.info
Vue.prototype.$success = Modal.success
Vue.prototype.$error = Modal.error
Vue.prototype.$warning = Modal.warning