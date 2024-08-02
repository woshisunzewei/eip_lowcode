<template>
    <div>
        <iframe frameborder="no" :src="src" :height="height" width="100%"></iframe>
    </div>
</template>
  
<script>
export default {
    data() {
        return {
            height: this.eipHeaderHeight() - 14 + "px",
            src: ''
        };
    },
    created() {
    },
    computed: {

    },
    mounted() {
        const route = this.$route;
        this.src = route.meta.params.url;
    },
    methods: {

    },
};
</script>