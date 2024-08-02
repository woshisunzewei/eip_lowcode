<template>
    <a-drawer width="80%" :bodyStyle="{ padding: '1px' }" :destroyOnClose="true" :visible="visible" title="组织架构地图模式查看"
        @close="cancel">
        <a-spin :spinning="spinning">
            <div :style="'height:' + height + 'px'">
                <el-amap v-if="!spinning" ref="amap" vid="amap" :rotateEnable="true" :mapStyle="mapConfig.mapStyle"
                    :zoom="mapConfig.zoom" :plugin="mapConfig.plugin" :center="mapConfig.center"
                    :events="mapConfig.events">
                    <el-amap-marker v-for="(marker, index) in markers" :position="marker.position"
                        :offset="marker.offset" :title="marker.title" :label="marker.label" :icon="marker.icon"
                        :key="index" :event="marker.event" :events="marker.events"></el-amap-marker>
                </el-amap>
            </div>
        </a-spin>
        <div class="stylePosition">
            <a-select style="width:120px" v-model="mapConfig.mapStyle">
                <a-select-option value="dark">深色样式</a-select-option>
                <a-select-option value="normal">默认样式</a-select-option>
                <a-select-option value="light">浅色样式</a-select-option>
                <a-select-option value="fresh">清新风格</a-select-option>
            </a-select>
        </div>
    </a-drawer>
</template>

<script>
import {
    query,
} from "@/services/system/organization/list";
export default {
    name: "maplist",
    data() {
        const that = this;
        return {
            height: window.innerHeight - 58,
            spinning: false,
            markers: [],
            mapConfig: {
                mapStyle: "dark", //设置地图显示样式，目前支持normal（默认样式）、dark（深色样式）、light（浅色样式）、fresh(osm清新风格样式)四种
                events: {
                    init(o) {
                        that.map = o;
                    },
                    complete: (o) => {
                        that.$refs.amap.$$getInstance().setFitView();
                    },
                },
                center: [121.59996, 31.197646],
                zoom: 18,
                plugin: [
                    {
                        enableHighAccuracy: true, //是否使用高精度定位，默认:true
                        timeout: 100, //超过10秒后停止定位，默认：无穷大
                        maximumAge: 0, //定位结果缓存0毫秒，默认：0
                        convert: true, //自动偏移坐标，偏移后的坐标为高德坐标，默认：true
                        showButton: true, //显示定位按钮，默认：true
                        buttonPosition: "RB", //定位按钮停靠位置，默认：'LB'，左下角
                        showMarker: true, //定位成功后在定位到的位置显示点标记，默认：true
                        showCircle: true, //定位成功后用圆圈表示定位精度范围，默认：true
                        panToLocation: true, //定位成功后将定位到的位置作为地图中心点，默认：true
                        zoomToAccuracy: true, //定位成功后调整地图视野范围使定位位置及精度范围视野内可见，默认：f
                        extensions: "all",
                        pName: "Geolocation",
                        events: {
                            init(o) { },
                        },
                    },
                    {
                        //地图工具条
                        pName: "ToolBar",
                        init(o) { },
                    },
                    {
                        //左下角缩略图插件 比例尺
                        pName: "Scale",
                        init(o) { },
                    },
                ],
            },

            map: null,
        };
    },
    mounted() {
        this.initMapMarkers();
    },
    props: {
        visible: {
            type: Boolean,
            default: false,
        },
        lng: {
            type: String,
        },
        lat: {
            type: String,
        },
        title: {
            type: String,
        },
    },

    methods: {
        /**
         *
         */
        initMapMarkers() {
            var that = this;
            that.spinning = true;
            query({
                current: 1,
                size: 500,
            }).then((result) => {
                if (result.code == that.eipResultCode.success) {
                    result.data.forEach(item => {
                        if (item.latitude) {
                            that.markers.push({
                                position: [parseFloat(item.longitude), parseFloat(item.latitude)],
                                icon: new AMap.Icon({
                                    image: "/images/mapWarn.gif",
                                    size: new AMap.Size(128, 128), //图标大小
                                    imageSize: new AMap.Size(100, 100),
                                }),
                                offset: [-20, -50], //设置偏移量是因为把坐标点换成自己的图片后会有偏移
                                label: {
                                    offset: [20, 2],
                                    content: "<div class='info'>" + item.name + "</div>", //设置文本标注内容e, //设置文本标注内容
                                    direction: "rigth", //设置文本标注方位
                                },
                                events: {
                                    click: (e) => { },
                                },
                            });

                        }
                    })
                }
                that.spinning = false;
            });
        },


        /**
         * 取消
         */
        cancel() {
            this.$emit("update:visible", false);
        },
    },
};
</script>

<style>
.amap-marker-label {
    border: 0;
    background-color: transparent;
}

.info {
    padding: 4px;
    margin-bottom: 1rem;
    border-radius: 0.25rem;
    top: 1rem;
    background-color: white;
    width: auto;
    border-width: 0;
    right: 1rem;
    box-shadow: 0 2px 6px 0 rgb(114 124 245 / 50%);
}

.stylePosition {
    position: absolute;
    top: 70px;
    left: 80px;
    color: #fff
}
</style>