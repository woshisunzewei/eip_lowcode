<template>
    <div>
        <div @click="onChosen" v-if="data.address == ''">
            <a-input :size="record.options.size" :placeholder="record.options.placeholder" disabled>
                <a-icon slot="addonAfter" type="environment" :style="{ 'color': 'rgba(0, 0, 0, 0.25)' }" />
            </a-input>
        </div>
        <div v-else class="location">
            <div v-if="!disabled" class="deleteIcon Absolute" @click="closeChosen">
                <span class="eLpXCR"><a-icon type="close"></a-icon></span>
            </div>
            <div class="flexRow">
                <div @click="onChosenToMap" class="flexr">
                    <div class="title">位置</div>
                    <div class="address">{{ data.address }}</div>
                </div>
                <a-tooltip v-if="!disabled" title="点击重新定位">
                    <div @click="onChosen" class="locationIcon">
                        <a-icon type="environment"></a-icon>
                    </div>
                </a-tooltip>
            </div>
        </div>
        <a-drawer v-if="drawer.visible && record.options.dialog.type == 'drawer'" :destroyOnClose="true" :footer="null"
            :bodyStyle="{ padding: '1px' }" :width="record.options.dialog.width + record.options.dialog.widthUnit"
            :visible="drawer.visible" :title="drawer.title + (map.address ? '-' + map.address : '')"
            :zIndex="record.options.dialog.zIndex" @close="drawer.visible = false">
            <div class="eip-drawer-body" style="height: calc(100vh - 110px);">
                <el-amap-search-box class="search-box" :search-option="searchOption" :on-search-result="querySearch">
                </el-amap-search-box>
                <div id="amap-container" style="width:100%;height:500px;margin-top:12px">
                    <el-amap vid="amap" style="height: calc(100vh - 166px);" ref="amap" :center="center" :zoom="zoom"
                        :events="events" :plugin="plugin">
                        <el-amap-marker :position="center"></el-amap-marker>
                    </el-amap>
                </div>
            </div>

            <div class="eip-drawer-toolbar">
                <div class="flex justify-between align-center">
                    <div></div>
                    <a-space>
                        <a-button @click="drawer.visible = false"><a-icon type="close" />关闭</a-button>
                        <a-button key="submit" type="primary" @click="confirm"><a-icon type="save" />确定</a-button>
                    </a-space>
                </div>
            </div>
        </a-drawer>

        <a-modal v-if="drawer.visible && record.options.dialog.type == 'modal'"
            :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }"
            :width="record.options.dialog.width + record.options.dialog.widthUnit" :visible="drawer.visible"
            :title="record.options.placeholder" :centered="record.options.dialog.centered"
            :maskClosable="record.options.dialog.closable" :zIndex="record.options.dialog.zIndex"
            @cancel="drawer.visible = false">
            <a-card class="margin-bottom-xs eip-admin-card-small">
                <el-amap-search-box class="search-box" :search-option="searchOption" :on-search-result="querySearch">
                </el-amap-search-box>
                <div id="amap-container" style="width:100%;height:500px;margin-top:12px">
                    <el-amap vid="amap" style="height: 500px;" ref="amap" :center="center" :zoom="zoom" :events="events"
                        :plugin="plugin">
                        <el-amap-marker :position="center"></el-amap-marker>
                    </el-amap>
                </div>
            </a-card>

            <template slot="footer">
                <a-space>
                    <a-button key="back" @click="drawer.visible = false"> 取消 </a-button>
                    <a-button key="submit" @click="confirm" type="primary">
                        确定
                    </a-button></a-space>
            </template>
        </a-modal>
    </div>
</template>

<script>
export default {
    name: "AMap",
    components: {},
    data() {
        let that = this;
        return {
            drawer: {
                bodyStyle: {
                    padding: "2px",
                },
                visible: false,
                title: "",
                width: 800
            },

            data: {
                lng: '',
                lat: '',
                address: ''
            },
            map: {
                lng: '',
                lat: '',
                address: ''
            },
            zoom: 13,
            center: [106.5572085, 29.6154994],
            searchOption: {
                city: "全国",
                citylimit: false,
            },
            plugin: [
                {
                    timeout: 1000, //超过10秒后停止定位
                    panToLocation: true, //定位成功后将定位到的位置作为地图中心点
                    zoomToAccuracy: true, //定位成功后调整地图视野范围使定位位置及精度范围视野内可见
                    pName: 'Geolocation',
                    events: {
                        init(o) {
                            if (that.data.address == '') {
                                // o 是高德地图定位插件实例
                                o.getCurrentPosition((status, result) => {
                                    if (result && result.position) {
                                        that.setAddress(result.position.lng, result.position.lat)
                                    } else {
                                        that.setAddress(that.center[0], that.center[1])
                                    }
                                });
                            }
                        }
                    }
                }
            ],
            events: {
                click(e) {
                    that.setAddress(e.lnglat.lng, e.lnglat.lat)
                }
            },
            search: {
                key: '',
                status: false,
                error: '请输入关键字查询'
            }
        }
    },
    props: ["value", "record", "disabled"],
    watch: {
        value(value) {
            if (value) {
                this.data = JSON.parse(value)
            }
        }
    },

    methods: {

        setAddress(lng, lat) {
            let that = this;
            that.center = [lng, lat];
            that.map.lng = lng;
            that.map.lat = lat;
            var geocoder = new AMap.Geocoder({
                radius: 1000,
                extensions: "all"
            });
            geocoder.getAddress(that.center, function (status, result) {
                if (status === 'complete' && result.info === 'OK') {
                    if (result && result.regeocode) {
                        that.map.address = result.regeocode.formattedAddress;
                    }
                } else {
                    that.$message.error(result)
                }
            });
        },

        /**
         * 点击搜索后触发的事件
         */
        querySearch(pois) {
            this.setAddress(pois[0].lng, pois[0].lat)
        },

        /**
         * 选择
         */
        onChosen() {
            if (!this.disabled) {
                this.drawer.visible = true;
                this.drawer.title = this.record.options.placeholder;
                this.drawer.width = this.record.options.dialog.width;
                if (this.data.address != '') {
                    this.center = [this.data.lng, this.data.lat];
                    this.map = this.$utils.clone(this.data, true);
                }
            }

        },

        /**
         * 地图上显示
         */
        onChosenToMap() {
            window.open("https://ditu.amap.com/regeo?lng=" + this.data.lng + "&lat=" + this.data.lat + "&src=uriapi&innersrc=uriapi")
        },

        /***
         * 确认
         */
        confirm: function () {
            this.drawer.visible = false;

            this.data.lat = this.map.lat;
            this.data.lng = this.map.lng;
            this.data.address = this.map.address;
            this.$emit("change", JSON.stringify(this.data));
        },

        /**
         * 关闭
         */
        closeChosen: function () {
            this.data = {
                lng: '',
                lat: '',
                address: ''
            }
            this.$emit("change", '');
        },


    }
}
</script>

<style lang="less" scoped>
.location {
    box-shadow: rgba(0, 0, 0, 0.12) 0px 1px 4px, rgba(0, 0, 0, 0.12) 0px 0px 2px;
    border-radius: 4px;
    position: relative;
}

.flexRow {
    display: flex;
    min-width: 0;
}

.flexr {
    flex: 1;
    line-height: 18px;
}

.location .title {
    padding: 2px 7px 2px;
    font-size: 13px;
    color: rgb(51, 51, 51);
    font-weight: 600;
}

.location .address {
    color: rgb(51, 51, 51);
    font-size: 12px;
    padding: 0px 2px 2px 7px;
}

.Absolute {
    position: absolute !important;
}

.location .deleteIcon {
    top: -12px;
    right: -12px;
}

.location .locationIcon {
    margin-left: 10px;
    width: 44px;
    display: flex;
    -webkit-box-align: center;
    align-items: center;
    -webkit-box-pack: center;
    justify-content: center;
}

.eLpXCR {
    cursor: pointer;
    width: 22px;
    height: 22px;
    font-size: 8px;
    border-radius: 22px;
    background: rgb(255, 255, 255);
    color: rgb(117, 117, 117);
    box-shadow: rgba(0, 0, 0, 0.16) 0px 1px 2px 1px;
    display: flex;
    -webkit-box-align: center;
    align-items: center;
    -webkit-box-pack: center;
    justify-content: center;
    border: 1px solid rgb(245, 245, 245);
}

.serachinput {
    width: 300px;
    box-sizing: border-box;
    padding: 9px;
    border: 1px solid #dddee1;
    line-height: 20px;
    font-size: 16px;
    height: 38px;
    color: #333;
    position: relative;
    border-radius: 4px;
    -webkit-box-shadow: #666 0px 0px 10px;
    -moz-box-shadow: #666 0px 0px 10px;
    box-shadow: #666 0px 0px 10px;
}

::v-deep .el-dialog__header {
    border-bottom: 0 !important;
}

.search-box {
    width: 100%;
    height: 40px;
}

::v-deep .el-vue-search-box-container .search-box-wrapper .search-btn {
    background-color: #409eff;
    border-color: #409eff;
    color: #fff;
    width: 70px;
}

::v-deep .el-vue-search-box-container .search-box-wrapper input {
    height: 40px;
    line-height: 40px;
    border: 1px solid #dcdfe6;
}
</style>