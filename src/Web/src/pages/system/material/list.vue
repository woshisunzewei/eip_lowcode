<template>
    <div>
        <a-card :hoverable="true">
            <div class="flex justify-between align-center ">
                <div>
                    <a-space>
                        <a-upload :multiple="true" :customRequest="uploadFile" :showUploadList="false"
                            :before-upload='beforeUpload'>
                            <a-button type="primary"><a-icon type="file-add"></a-icon>上传文件</a-button>
                        </a-upload>
                        <a-button @click="folderAdd" type="dashed"><a-icon type="folder-add"></a-icon>新建文件夹</a-button>
                        <a-button type="danger" @click="materialDelCheck"><a-icon type="delete"></a-icon>删除</a-button>
                    </a-space>
                </div>
                <div>
                    <a-space>
                        <a-input-search @search="initData" v-model="search.name" allow-clear :loading="spinning"
                            placeholder="搜索您的文件" style="width: 200px" />
                        <!-- <a-icon style="font-size: 30px;" type="menu"></a-icon> -->
                    </a-space>
                </div>
            </div>

            <div class="flex justify-between align-center margin-top">
                <div>
                    <a-breadcrumb>
                        <a-breadcrumb-item v-for="(item, index) in breadcrumb" :key="index"><a
                                @click="breadcrumbClick(item)" v-if="index != breadcrumb.length" href="javascript:;">{{
                                item.name }}</a>
                            <label v-else>{{ item.name }}</label></a-breadcrumb-item>
                    </a-breadcrumb>
                </div>
                <div>
                    <div> 已全部加载，共 {{ this.data.length }} 个 </div>
                </div>
            </div>

            <a-spin :spinning="spinning">
                <div class="demo-file-list-group margin-top">
                    <div class="eip-file-list-group">
                        <div class="eip-file-list">
                            <div class="eip-file-list-header">
                                <div class="eip-file-list-check-group">
                                    <a-checkbox :indeterminate="check.indeterminate" :checked="check.all"
                                        @change="checkAllChange">
                                        {{ check.title }}
                                    </a-checkbox>
                                </div>
                            </div>
                            <div class="eip-file-list-body  beauty-scroll"
                                :style="{ height: height, overflow: 'auto' }">
                                <div v-if="data.length > 0" @click="itemClick(item)" class="eip-file-list-item"
                                    :class="item.checked ? 'checked' : ''" v-for="(item, index) in data" :key="index">
                                    <a-dropdown :trigger="['contextmenu']">
                                        <div>
                                            <a-tooltip>
                                                <template slot="title">
                                                    <div>名称： {{ item.name }}.{{ item.suffix }}</div>
                                                    <div>创建时间： {{ item.createTime }}</div>
                                                    <div>创建人： {{ item.createUserName }}</div>
                                                </template>
                                                <div class="eip-file-list-item-body ant-dropdown-trigger">
                                                    <div v-viewer class="eip-file-list-item-icon">
                                                        <viewer>
                                                            <img style="max-width: 84px;max-height: 84px;" :class="['png',
                                                                'jpeg',
                                                                'jpg',
                                                                'gif'
                                                            ].includes(item.suffix) ? '' : 'eip-file-list-item-icon-image'" v-real-img="item.path" />
                                                        </viewer>
                                                    </div>
                                                    <div class="eip-file-list-item-title">{{ item.name }}
                                                    </div>
                                                </div>
                                            </a-tooltip>
                                            <div @click.stop="checkClick(item)" class="eip-file-list-item-check"><i
                                                    class="eip-file-icon-check checked"></i>
                                            </div>
                                        </div>
                                        <a-menu slot="overlay">
                                            <!-- <a-menu-item key="open">
                                                <a-icon type="eye"></a-icon> 打开
                                            </a-menu-item>
                                            <a-menu-divider /> -->
                                            <!-- <a-menu-item @click="download(item)" key="download" v-if="item.type != 0">
                                                <a-icon type="cloud-download"></a-icon> 下载
                                            </a-menu-item> -->
                                            <a-menu-item key="edit" @click="folderRename(item)">
                                                <a-icon type="edit"></a-icon> 重命名
                                            </a-menu-item>
                                            <!-- <a-menu-item key="swap">
                                                <a-icon type="swap"></a-icon> 移动到
                                            </a-menu-item> -->
                                            <a-menu-divider />
                                            <a-menu-item @click="materialDel(item)" key="delete"
                                                style="color: #ff4d4f;">
                                                <a-icon type="delete"></a-icon>删除
                                            </a-menu-item>
                                        </a-menu>
                                    </a-dropdown>
                                </div>
                                <div v-if="data.length == 0">
                                    <eip-empty />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </a-spin>
        </a-card>
        <a-modal :visible="folder.visible" :title="folder.title" @cancel="folder.visible = false" v-drag>
            <a-form-model :model="folder" :rules="folder.rules" ref="form" :label-col="config.labelCol"
                :wrapper-col="config.wrapperCol">
                <a-form-model-item label="名称" prop="name">
                    <a-input v-model="folder.name" placeholder="请输入名称"></a-input>
                </a-form-model-item>
            </a-form-model>
            <template slot="footer">
                <a-space>
                    <a-button key="back" @click="folder.visible = false"><a-icon type="close" />取消</a-button>
                    <a-button key="submit" @click="folderSave" type="primary" :loading="folder.loading"><a-icon
                            type="save" />提交</a-button>
                </a-space>
            </template>
        </a-modal>
    </div>
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
import {
    query,
    del,
    save,
    upload
} from "@/services/system/material/list";
import { newGuid } from "@/utils/util";
import { deleteConfirm } from "@/utils/util";
export default {
    components: {},
    data() {
        return {
            config: {
                labelCol: { span: 4 },
                wrapperCol: { span: 20 },
            },
            height: this.eipHeaderHeight() - 178 + "px",
            data: [],
            breadcrumb: [{
                id: null,
                name: '全部文件'
            }],

            check: {
                all: false,
                indeterminate: false,//半选中
                title: '全选'
            },

            search: {
                parentId: null,//当前上级
                sord: 'asc',
                sidx: 'type',
                name: ''
            },
            folder: {
                rules: {
                    name: [
                        {
                            required: true,
                            message: "请输入名称",
                            trigger: "blur",
                        },
                    ]
                },
                visible: false,
                title: '文件夹',
                name: '',
                materialId: newGuid(),
                loading: false,
                isRename: false
            },

            uploadOption: {
                fileNumber: 0,//上传文件个数
                successNumber: 0,//上传成功数量
            }
        };
    },
    created() {
        this.initData();
    },
    mounted() {
    },
    methods: {
        /**
         * 上传前确认选中数量
         * @param {*} file 
         * @param {*} fileList 
         */
        beforeUpload(file, fileList) {
            // fileList 只包含了当次上传的文件列表，不包含已上传的文件列表
            // 所以长度要加上已上传的文件列表的长度
            this.uploadOption.fileNumber = fileList.length;
        },
        /**
         * 初始化数据
         */
        initData() {
            let that = this;
            that.spinning = true;
            var beforeCheck = this.data.filter(f => f.checked);
            this.data = [];//之前是否具有已选中
            query(this.search).then((result) => {
                //判断类型
                result.data.forEach(element => {
                    if (element.type == 0) {
                        element.path = "/images/material/dir.png"
                    } else {
                        switch (element.suffix) {
                            case "txt":
                                element.path = "/images/material/txt.png"
                                break;
                            case "doc":
                            case "docx":
                                element.path = "/images/material/doc.png"
                                break;
                            case "xls":
                            case "xlsx":
                                element.path = "/images/material/xls.png"
                                break;
                            case "apk":
                                element.path = "/images/material/apk.png"
                                break;
                            case "bt":
                                element.path = "/images/material/bt.png"
                                break;
                            case "exe":
                                element.path = "/images/material/exe.png"
                                break;
                            case "flash":
                                element.path = "/images/material/flash.png"
                                break;
                            case "htm":
                            case "html":
                                element.path = "/images/material/htm.png"
                                break;
                            case "mp3":
                                element.path = "/images/material/mp3.png"
                                break;
                            case "mp4":
                                element.path = "/images/material/mp4.png"
                                break;
                            case "pdf":
                                element.path = "/images/material/pdf.png"
                                break;
                            case "ppt":
                            case "pptx":
                                element.path = "/images/material/ppt.png"
                                break;
                            case "psd":
                                element.path = "/images/material/psd.png"
                                break;
                            case "ttf":
                                element.path = "/images/material/ttf.png"
                                break;
                            case "zip":
                                element.path = "/images/material/zip.png"
                                break;
                            case "jpg":
                            case "png":
                            case "jpeg":
                            case "gif":
                                break;
                            default:
                                element.path = "/images/material/file.png"
                                break;
                        }
                    }
                    //判断是否需要选中
                    element.checked = beforeCheck.filter(f => f.materialId == element.materialId).length > 0;
                });
                that.data = result.data;
                var totalChecked = that.data.filter(f => f.checked).length;
                that.check.title = totalChecked != 0 ? '已选中' + totalChecked + "个文件/文件夹" : '全选';
                that.check.indeterminate = totalChecked == 0 ? false : (totalChecked != that.data.length);
                that.check.all = result.data.length == 0 ? false : (totalChecked == that.data.length);
                that.spinning = false;
            });
        },

        /**
         * 
         * @param {*} item 
         */
        checkClick(item) {
            item.checked = !item.checked;
            //统计总共选中
            var totalChecked = this.data.filter(f => f.checked).length;
            this.check.title = totalChecked != 0 ? '已选中' + totalChecked + "个文件/文件夹" : '全选';
            this.check.indeterminate = totalChecked == 0 ? false : (totalChecked != this.data.length);
            this.check.all = (totalChecked == this.data.length);
        },

        /**
         * 
         */
        checkAllChange(e) {
            var checkAll = e.target.checked;
            this.check.indeterminate = false;
            this.check.all = checkAll;
            this.check.title = checkAll ? '已选中' + this.data.length + "个文件/文件夹" : '全选';
            this.data.map(m => m.checked = checkAll)
        },

        /**
         * 点击
         */
        itemClick(item) {
            //如果点击是文件夹则打开下级
            if (item.type == 0) {
                this.search.parentId = item.materialId;
                //生成面包屑导航
                this.breadcrumb.push({
                    id: item.materialId,
                    name: item.name
                })
                this.check = {
                    all: false,
                    indeterminate: false,//半选中
                    title: '全选'
                }
                this.initData();
            } else {
                //判断类别,若是图片就直接打开
                switch (item.suffix) {
                    case 'jpg':
                    case 'png':
                    case 'jpeg':
                    case 'gif':
                        break;
                    default:
                        this.$message.error('暂不支持预览')
                        break;
                }
            }
        },

        /**
         * 导航点击
         * @param {*} item 
         */
        breadcrumbClick(item) {
            var breadcrumb = [];
            for (let i = 0; i < this.breadcrumb.length; i++) {
                var f = this.breadcrumb[i];
                if (f.id != item.id) {
                    breadcrumb.push(f)
                } else {
                    breadcrumb.push(f);
                    break;
                }
            }
            this.breadcrumb = breadcrumb;
            this.search.parentId = item.id;
            this.check = {
                all: false,
                indeterminate: false,//半选中
                title: '全选'
            }
            this.initData();
        },

        /**
         * 删除
         */
        materialDel(item) {
            let that = this;
            deleteConfirm(
                "素材【" + item.name + "】" + that.eipMsg.delete,
                function () {
                    that.$loading.show({ text: that.eipMsg.delloading });
                    del({ id: item.materialId }).then((result) => {
                        that.$loading.hide();
                        if (result.code == that.eipResultCode.success) {
                            that.check = {
                                all: false,
                                indeterminate: false,//半选中
                                title: '全选'
                            }
                            that.initData();
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
         * 删除选中素材
         */
        materialDelCheck() {
            let that = this;
            if (this.check.all == false && this.check.indeterminate == false) {
                this.$message.error('请选择要删除素材');
            } else {
                deleteConfirm(
                    "确定删除选中素材",
                    function () {
                        that.$loading.show({ text: that.eipMsg.delloading });
                        del({ id: that.data.filter(f => f.checked).map(m => m.materialId).join(',') }).then((result) => {
                            that.$loading.hide();
                            if (result.code == that.eipResultCode.success) {
                                that.initData();
                                that.$message.success(result.msg);
                            } else {
                                that.$message.error(result.msg);
                            }
                        });
                    },
                    that
                );
            }
        },

        /**
        * 上传
        */
        uploadFile(data) {
            let that = this;
            that.$message.loading({
                content: "上传中...",
                duration: 0,
            });
            const formData = new FormData();
            formData.append("Files", data.file);
            formData.append("materialId", newGuid());
            formData.append("name", data.file.name.substring(0, data.file.name.lastIndexOf(".")));
            formData.append("type", 1);
            formData.append("parentId", that.search.parentId);
            formData.append("suffix", data.file.name.match(/[^.]+$/)[0]);
            //上传
            save(formData).then(function (result) {
                that.$message.destroy();
                that.uploadOption.successNumber += 1;
                if (result.code === that.eipResultCode.success) {
                    if (that.uploadOption.successNumber == that.uploadOption.fileNumber) {
                        that.$message.success(result.msg);
                        that.initData();
                        that.uploadOption.successNumber = 0;
                        that.uploadOption.fileNumber = 0;
                    }
                } else {
                    that.$message.error(result.msg);
                }
            }).catch(() => {
                that.$message.destroy();
                that.$message.error("上传出错");
            });
        },

        /**
         * 下载
         * @param {} item 
         */
        download(item) {
            let a = document.createElement('a')
            a.href = item.path
            a.download = 'pic'
            a.click()
        },

        /**
         * 文件夹新增
         */
        folderAdd() {
            this.folder.visible = true;
            this.folder.isRename = false;
            this.folder.materialId = newGuid();
            this.folder.name = "";
        },

        /**
         * 重命名
         */
        folderRename(item) {
            this.folder.materialId = item.materialId;
            this.folder.name = item.name;
            this.folder.visible = true;
            this.folder.isRename = true;
        },
        /**
         * 文件夹
         */
        folderSave() {
            let that = this;
            this.$refs.form.validate((valid) => {
                if (valid) {
                    that.loading = true;
                    that.$loading.show({ text: that.eipMsg.saveLoading });
                    const formData = new FormData();
                    formData.append("materialId", that.folder.materialId);
                    formData.append("name", that.folder.name);
                    if (!that.folder.isRename) {
                        formData.append("type", 0);
                        formData.append("parentId", that.search.parentId);
                    }
                    save(formData).then(function (result) {
                        if (result.code === that.eipResultCode.success) {
                            that.$message.success(result.msg);
                            that.initData();
                            that.folder.visible = false;
                        } else {
                            that.$message.error(result.msg);
                        }
                        that.loading = false;
                        that.$loading.hide();
                    });
                } else {
                    return false;
                }
            });
        },

    },
};
</script>

<style lang="less" scoped>
.eip-file-list-group .eip-file-icon-check {
    width: 16px;
    height: 16px;
    border-radius: 2px;
    display: inline-block;
    border: 1px solid #d9d9d9;
    background: #fff;
    box-sizing: border-box;
    transition: all .3s;
    position: relative;
    cursor: pointer
}

.eip-file-list-group .eip-file-icon-check:hover {
    border-color: #1890ff
}

.eip-file-list-group .eip-file-icon-check:before {
    content: "";
    width: 5.71428571px;
    height: 9.14285714px;
    display: table;
    border: 2px solid #fff;
    border-top: 0;
    border-left: 0;
    transform: rotate(45deg) scale(0) translate(-50%, -50%);
    opacity: 0;
    transition: all .1s cubic-bezier(.71, -.46, .88, .6), opacity .1s;
    box-sizing: border-box;
    position: absolute;
    top: 50%;
    left: 21.5%
}

.eip-file-list-group .eip-file-icon-check.checked {
    background: #1890ff;
    border-color: #1890ff
}

.eip-file-list-group .eip-file-icon-check.checked:before {
    transform: rotate(45deg) scale(1) translate(-50%, -50%);
    opacity: 1;
    transition: all .2s cubic-bezier(.12, .4, .29, 1.46) .1s
}

.eip-file-list-group .eip-file-icon-check:after {
    content: "";
    width: 8px;
    height: 8px;
    display: table;
    transform: translate(-50%, -50%) scale(1);
    opacity: 0;
    transition: all .1s cubic-bezier(.71, -.46, .88, .6), opacity .1s;
    box-sizing: border-box;
    position: absolute;
    top: 50%;
    left: 50%
}

.eip-file-list-group .eip-file-icon-check.indeterminate:after {
    background: #1890ff;
    opacity: 1
}

.eip-file-list-group .eip-file-list-selector {
    display: none;
    position: absolute;
    box-sizing: border-box;
    background: rgba(0, 120, 215, .2);
    border: 1px solid #0078d7;
    z-index: 999
}

.eip-file-list {
    position: relative;
    box-sizing: border-box
}

.eip-file-list .eip-file-list-header {
    padding: 0 16px;
    border-bottom: 1px solid #f0f0f0;
    box-sizing: border-box;
    line-height: 36px;
    display: flex
}

.eip-file-list .eip-file-list-header .eip-file-list-check-group {
    display: inline-flex;
    align-items: center;
    cursor: pointer
}

.eip-file-list .eip-file-list-header .eip-file-list-check-group .eip-file-list-check {
    margin-right: 14px
}

.eip-file-list .eip-file-list-body {
    padding: 14px 14px 0 0
}

.eip-file-list .eip-file-list-item {
    width: 110px;
    padding: 6px 4px;
    margin: 0 0 14px 14px;
    display: inline-block;
    box-sizing: border-box;
    border: 1px solid transparent;
    transition: all .3s;
    border-radius: 6px;
    text-align: center;
    position: relative;
    cursor: pointer
}

.eip-file-list .eip-file-list-item .eip-file-list-item-icon {
    max-height: 84px;
    display: flex;
    align-items: center;
    justify-content: center;
    position: relative
}

.eip-file-list .eip-file-list-item .eip-file-list-item-icon>img {
    max-width: 84px;
    max-height: 84px;
    border-radius: 2px;
    display: inline-block;
    pointer-events: none
}

.eip-file-list .eip-file-list-item .eip-file-list-item-icon>.eip-file-list-item-icon-image {
    width: 56px;
    height: 56px;
    object-fit: cover
}

.eip-file-list .eip-file-list-item .eip-file-list-item-title {
    margin-top: 4px;
    overflow: hidden;
    white-space: nowrap;
    word-break: break-all;
    text-overflow: ellipsis;
    box-sizing: border-box;
    transition: color .3s
}

.eip-file-list .eip-file-list-item .eip-file-list-item-title:hover {
    color: #1890ff
}

.eip-file-list .eip-file-list-item .eip-file-list-item-check {
    border-radius: 50%;
    background: #fff;
    transition: all .3s;
    position: absolute;
    top: 6px;
    right: 6px;
    opacity: 0
}

.eip-file-list .eip-file-list-item .eip-file-list-item-check .eip-file-icon-check {
    width: 18px;
    height: 18px;
    border-radius: 50%;
    display: block;
    opacity: .4
}

.eip-file-list .eip-file-list-item:hover,
.eip-file-list .eip-file-list-item.active,
.eip-file-list .eip-file-list-item.checked {
    background: #e6f7ff
}

.eip-file-list .eip-file-list-item:hover .eip-file-list-item-check,
.eip-file-list .eip-file-list-item.active .eip-file-list-item-check,
.eip-file-list .eip-file-list-item.checked .eip-file-list-item-check {
    opacity: 1
}

.eip-file-list .eip-file-list-item.active,
.eip-file-list .eip-file-list-item.checked {
    border-color: #91d5ff
}

.eip-file-list .eip-file-list-item.active .eip-file-list-item-check .eip-file-icon-check,
.eip-file-list .eip-file-list-item.checked .eip-file-list-item-check .eip-file-icon-check {
    opacity: 1
}

.eip-file-list-table-item-sort {
    width: 8px;
    height: 12px;
    margin-left: 6px;
    position: relative;
    display: inline-block;
    flex-shrink: 0
}

.eip-file-list-table-item-sort:before,
.eip-file-list-table-item-sort:after {
    content: "";
    width: 0;
    height: 0;
    display: block;
    border: 4px solid hsla(0, 0%, 60%, .6);
    border-left-color: transparent;
    border-right-color: transparent
}

.eip-file-list-table-item-sort:before {
    border-top-color: transparent;
    margin: -4px 0 4px
}

.eip-file-list-table-item-sort:after {
    border-bottom-color: transparent
}

.eip-file-list-table-item-sort.eip-file-list-sort-asc:before {
    border-bottom-color: #1890ff
}

.eip-file-list-table-item-sort.eip-file-list-sort-desc:after {
    border-top-color: #1890ff
}

.eip-file-list-table {
    position: relative
}

.eip-file-list-table .eip-file-list-table-item {
    line-height: 44px;
    border-top: 1px solid #f0f0f0;
    transition: all .3s;
    position: relative
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-body {
    display: flex;
    align-items: center
}

.eip-file-list-table .eip-file-list-table-item:first-child {
    border-top-color: transparent;
    margin-top: -1px
}

.eip-file-list-table .eip-file-list-table-item:last-child {
    border-bottom: 1px solid #f0f0f0
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-check-group {
    width: 36px;
    padding-left: 16px;
    box-sizing: border-box;
    line-height: 0;
    flex-shrink: 0
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-name {
    display: flex;
    align-items: center;
    padding: 0 10px;
    box-sizing: border-box;
    position: relative;
    overflow: hidden;
    min-width: 110px;
    flex: 1
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-icon {
    flex: 1;
    display: flex;
    align-items: center;
    overflow: hidden
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-icon-image {
    width: 24px;
    height: 24px;
    flex-shrink: 0;
    cursor: pointer
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-icon-image:hover+.eip-file-list-table-item-title {
    color: #1890ff
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-title {
    padding-left: 10px;
    transition: color .3s;
    box-sizing: border-box;
    text-overflow: ellipsis;
    word-break: break-all;
    white-space: nowrap;
    overflow: hidden;
    cursor: pointer
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-title:hover {
    color: #1890ff
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-tool-group {
    flex-shrink: 0;
    box-sizing: border-box;
    align-items: center;
    display: none
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-tool-group .eip-file-list-item-tool {
    font-size: 17px;
    margin-left: 16px;
    color: #1890ff;
    cursor: pointer
}

.eip-file-list-table .eip-file-list-table-item:hover .eip-file-list-table-item-tool-group {
    display: flex
}

.eip-file-list-table .eip-file-list-table-item .eip-file-list-table-item-col {
    padding: 0 10px;
    box-sizing: border-box
}

.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item {
    line-height: 36px
}

.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-name>div {
    overflow: hidden;
    white-space: nowrap;
    word-break: break-all;
    text-overflow: ellipsis
}

.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-name,
.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-col {
    transition: color .3s;
    cursor: pointer
}

.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-name:hover,
.eip-file-list-table .eip-file-list-table-header .eip-file-list-table-item .eip-file-list-table-item-col:hover {
    color: #1890ff
}

.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item:hover,
.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item.active,
.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item.checked {
    background: #e6f7ff;
    border-color: #91d5ff
}

.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item:hover+.eip-file-list-table-item,
.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item.active+.eip-file-list-table-item,
.eip-file-list-table .eip-file-list-table-body .eip-file-list-table-item.checked+.eip-file-list-table-item {
    border-top-color: #91d5ff
}

.eip-file-item-context-menu {
    min-width: 100px
}

.demo-file-list-group {
    position: relative;
    overflow-x: auto;
}

.demo-file-list-group .demo-file-list-empty {
    position: absolute;
    top: 100px;
    left: 50%;
    transform: translate(-50%)
}
</style>