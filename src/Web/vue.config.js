let path = require('path')
const webpack = require('webpack')
const ThemeColorReplacer = require('webpack-theme-color-replacer')
const MonacoWebpackPlugin = require('monaco-editor-esm-webpack-plugin');

function resolve(dir) {
    return path.join(__dirname, dir)
}
const {
    getThemeColors,
    modifyVars
} = require('./src/utils/themeUtil')
const {
    resolveCss
} = require('./src/utils/theme-color-replacer-extend')
const CompressionWebpackPlugin = require('compression-webpack-plugin')

const productionGzipExtensions = ['js', 'css', 'less']
const isProd = process.env.NODE_ENV === 'production'
const assetsCDN = {
    // webpack build externals
    externals: {
        vue: 'Vue',
        'vue-router': 'VueRouter',
        vuex: 'Vuex',
        axios: 'axios',
        nprogress: 'NProgress',
        clipboard: 'ClipboardJS',
        'js-cookie': 'Cookies',
        'html2canvas': 'html2canvas',
        'vant': 'vant',
        'echarts': 'echarts'
    },
    css: [
        '/static/cdn/vant.min.css'
    ],
    js: [
        '/static/cdn/vue.min.js',
        '/static/cdn/vue-router.min.js',
        '/static/cdn/vuex.min.js',
        '/static/cdn/axios.min.js',
        '/static/cdn/nprogress.min.js',
        '/static/cdn/clipboard.min.js',
        '/static/cdn/js.cookie.min.js',
        '/static/cdn/html2canvas.min.js',
        '/static/cdn/vant.min.js',
    ]
}
module.exports = {
    runtimeCompiler: true,
    lintOnSave: false,
    devServer: {
        historyApiFallback: {
            index: '/index.html'
        },
        port: 8888,
        open: true
    },
    pluginOptions: {
        'style-resources-loader': {
            preProcessor: 'less',
            patterns: [path.resolve(__dirname, "./src/theme/theme.less")],
        }
    },
    configureWebpack: config => {
        config.entry.app = ["babel-polyfill", "whatwg-fetch", "./src/main.js"];
        config.performance = {
            hints: false
        }
        config.module.rules.push({
            test: /\.js/,
            enforce: 'pre',
            include: /node_modules[\\\/]monaco-editor[\\\/]esm/,
            use: MonacoWebpackPlugin.loader
        });

        config.plugins.push(
            new ThemeColorReplacer({
                fileName: 'css/theme-colors-[contenthash:8].css',
                matchColors: getThemeColors(),
                injectCss: true,
                resolveCss
            })
        )
        config.plugins.push(
                new MonacoWebpackPlugin({ languages: ['javascript', 'typescript', 'json', 'csharp'] })
            )
            // Ignore all locale files of moment.js
        config.plugins.push(new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/))
            // 生产环境下将资源压缩成gzip格式
        if (isProd) {
            const TerserPlugin = require("terser-webpack-plugin");
            config.plugins.push(
                    new TerserPlugin({
                        terserOptions: {
                            ecma: undefined,
                            warnings: false,
                            parse: {},
                            compress: {
                                drop_console: true,
                                drop_debugger: true,
                                pure_funcs: ['console.log'], // 移除console
                            },
                        },
                    })
                )
                // add `CompressionWebpack` plugin to webpack plugins
            config.plugins.push(new CompressionWebpackPlugin({
                algorithm: 'gzip',
                test: new RegExp('\\.(' + productionGzipExtensions.join('|') + ')$'),
                threshold: 10240,
                minRatio: 0.8
            }))

            // 开启分离js
            config.optimization = {
                runtimeChunk: 'single',
                splitChunks: {
                    chunks: 'all',
                    maxInitialRequests: Infinity,
                    minSize: 20000,
                    cacheGroups: {
                        monacoEditor: {
                            chunks: 'async',
                            name: 'monaco-editor',
                            priority: 22,
                            test: /[\\/]monaco-editor[\\/]/,
                            enforce: true,
                            reuseExistingChunk: true,
                        },
                        antd: {
                            chunks: 'all',
                            name: `ant-design-vue`,
                            test: /[\\/]ant-design-vue[\\/]/,
                            priority: 0,
                        },
                        antdesign: {
                            chunks: 'all',
                            name: `ant-design`,
                            test: /[\\/]@ant-design[\\/]/,
                            priority: 0,
                        },
                        vxeTable: {
                            chunks: 'all',
                            name: `vxe-table`,
                            test: /[\\/]vxe-table[\\/]/,
                            priority: 0,
                        },
                        vueOffice: {
                            chunks: 'all',
                            name: `vue-office`,
                            test: /[\\/]@vue-office[\\/]/,
                            priority: 0,
                        },
                        zrender: {
                            chunks: 'all',
                            name: `zrender`,
                            test: /[\\/]zrender[\\/]/,
                            priority: 0,
                        },
                        bwipjs: {
                            chunks: 'all',
                            name: `bwip-js`,
                            test: /[\\/]bwip-js[\\/]/,
                            priority: 0,
                        },
                        jquery: {
                            chunks: 'all',
                            name: `jquery`,
                            test: /[\\/]jquery[\\/]/,
                            priority: 0,
                        },
                        moment: {
                            chunks: 'all',
                            name: `moment`,
                            test: /[\\/]moment[\\/]/,
                            priority: 0,
                        },

                        vueqr: {
                            chunks: 'all',
                            name: `vue-qr`,
                            test: /[\\/]vue-qr[\\/]/,
                            priority: 0,
                        },

                        jszip: {
                            chunks: 'all',
                            name: `jszip`,
                            test: /[\\/]jszip[\\/]/,
                            priority: 0,
                        },

                        form: {
                            chunks: 'all',
                            name: `form`,
                            test: /[\\/]form[\\/]/,
                            priority: 0,
                        },

                        mobile: {
                            chunks: 'all',
                            name: `mobile`,
                            test: /[\\/]mobile[\\/]/,
                            priority: 0,
                        },

                        workflow: {
                            chunks: 'all',
                            name: `workflow`,
                            test: /[\\/]workflow[\\/]/,
                            priority: 0,
                        },

                        swiper: {
                            chunks: 'all',
                            name: `swiper`,
                            test: /[\\/]swiper[\\/]/,
                            priority: 0,
                        },

                        corejs: {
                            chunks: 'all',
                            name: `corejs`,
                            test: /[\\/]core-js[\\/]/,
                            priority: 0,
                        }
                    }
                }
            }
        }
        // if prod, add externals
        if (isProd) {
            config.externals = assetsCDN.externals
        }
    },
    chainWebpack: config => {
        // 通过将perfetch插件的移除或者修改，就真正实现了路由懒加载的方式
        config.plugins.delete('prefetch')
            // key,value自行定义，比如.set('@assets', resolve('src/assets'))
        config.resolve.alias.set('@ant-design/icons/lib/dist$', resolve("src/icon.js"));
        // 生产环境下关闭css压缩的 colormin 项，因为此项优化与主题色替换功能冲突
        if (isProd) {
            config.plugin('optimize-css')
                .tap(args => {
                    args[0].cssnanoOptions.preset[1].colormin = false
                    return args
                })

            config.plugin('webpack-bundle-analyzer')
                .use(require('webpack-bundle-analyzer').BundleAnalyzerPlugin)

        }
        // 生产环境下使用CDN
        if (isProd) {
            config.plugin('html')
                .tap(args => {
                    args[0].cdn = assetsCDN
                    return args
                })
        }
    },
    css: {
        loaderOptions: {
            less: {
                lessOptions: {
                    modifyVars: modifyVars(),
                    javascriptEnabled: true
                }
            }
        }
    },
    publicPath: process.env.VUE_APP_PUBLIC_PATH,
    outputDir: 'dist', //打包文件输出路径，即打包到哪里
    assetsDir: 'static', // 静态资源地址
    productionSourceMap: false // 生产环境是否生成 sourceMap 文件
}