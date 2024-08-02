<template>
    <a-modal width="1170px" v-drag centered :destroyOnClose="true" :maskClosable="false" :visible="visible"
        :zIndex="1001" :bodyStyle="{ padding: '1px', 'background-color': '#f0f2f5' }" title="位置选择" @cancel="cancel">
        <span>
            <el-amap-search-box class="search-box" :search-option="searchOption" :on-search-result="querySearch">
            </el-amap-search-box>
            <div id="amap-container" style="width:100%;height:500px;margin-top:12px">
                <el-amap class="amap-box" :vid="'amap-vue'" :center="center" :plugin="plugin" :events="events"
                    :zoom="zoom">
                    <el-amap-marker :position="center"></el-amap-marker>
                </el-amap>
            </div>
        </span>
        <span slot="footer" class="dialog-footer">
            <a-button @click="cancel">取 消</a-button>
            <a-button type="primary" @click="confirm">确 定</a-button>
        </span>
    </a-modal>
</template>

<script>
import Vue from 'vue'
import VueAMap from 'vue-amap';
Vue.use(VueAMap);

export default {
    name: "AMap",
    components: {},
    data() {
        return {
            markersArray: [this.lng ? this.lng : 104.066301, this.lat ? this.lat : 30.572961],
            zoom: 15,
            lnglatInfo: {
                lng: '',
                lat: '',
                addr: ''
            }
            , searchOption: {
                city: "全国",
                citylimit: false,
            }
            , center: [this.lng ? this.lng : 104.066301, this.lat ? this.lat : 30.572961]
            , plugin: [
                {
                    pName: 'Scale',
                    events: {
                        init(instance) {
                        }
                    }
                },
                {
                    pName: 'ToolBar',
                    position: 'RT',
                    events: {
                        init(instance) {
                        }
                    }
                }
            ]
            , events: {
                init(o) {
                },
                zoomchange: (e) => {
                },
                zoomend: (e) => {

                },
                click: e => {
                    //地图中的点击事件
                    this.center = [e.lnglat.lng, e.lnglat.lat];
                    this.markersArray = [];
                    this.markersArray.push(this.center)
                    this.lnglatInfo.lat = e.lnglat.lat;
                    this.lnglatInfo.lng = e.lnglat.lng;
                }
            },
        }
    },
    props: {
        lng: {
            type: Number,
        },
        lat: {
            type: Number,
        },
        title: {
            type: String,
        },
        visible: Boolean
    },
    methods: {

        //点击搜索后触发的事件
        querySearch(pois) {
            this.input = document.querySelector('.search-box-wrapper input').value
            this.center = [pois[0].lng, pois[0].lat]        //选择了第一个值
            this.markersArray = [];    //标记点先清空  
            this.markersArray.push([pois[0].lng, pois[0].lat])   //把搜索的位置的第一个值存入标记中并显示标记点
            this.lnglatInfo.lat = pois[0].lat;
            this.lnglatInfo.lng = pois[0].lng;
        },
        /***
         * 确认，并返回选择的经纬度
         */
        confirm: function () {
            this.$emit('map-confirm', this.lnglatInfo)
        },
        /***
         * 取消
         */
        cancel: function () {
            this.$emit('cancel')
        }
    }
}
</script>

<style lang="less" scoped>
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