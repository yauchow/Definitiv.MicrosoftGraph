<script lang="ts">

	import appsettings from "$lib/appsettings";

</script>
<style lang="scss" global>
    @use '../styles/global.scss'
</style>

<div class="container-fluid">
    <slot></slot>
</div>
{appsettings.apiUrl}