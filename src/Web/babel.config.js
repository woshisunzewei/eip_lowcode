const IS_PROD = ['production', 'prod'].includes(process.env.NODE_ENV)

const plugins = []
if (IS_PROD) {
    plugins.push('transform-remove-console')
}
//按需加载
plugins.push(["import", {
    "libraryName": "ant-design-vue",
    "libraryDirectory": "es",
    "style": true // 样式是否也按需加载
}], ["import", {
    "libraryName": "vxe-table",
    "style": true // 样式是否也按需加载
}, 'vxe-table'])
module.exports = {
    presets: [
        '@vue/cli-plugin-babel/preset'
    ],
    plugins
}