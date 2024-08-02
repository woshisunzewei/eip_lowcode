<template>
    <div>
        <a-empty :image="simpleImage" :description="description" />
    </div>
</template>
  
<script>
import { Empty } from 'ant-design-vue';
export default {
    name: "eip-empty",
    data() {
        return {

        };
    },
    beforeCreate() {
        this.simpleImage = Empty.PRESENTED_IMAGE_SIMPLE;
    },
    props: {
        description: {
            type: String
        }
    }
};
</script>