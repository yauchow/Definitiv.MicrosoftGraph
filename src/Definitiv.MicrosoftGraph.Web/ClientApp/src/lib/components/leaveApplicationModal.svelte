<script lang="ts">
	import { LeaveType, type LeaveApplication } from "$lib/getLeaveApplications";
	import { createEventDispatcher } from "svelte";

    const dispatcher = createEventDispatcher();

    const options = [
        { id: LeaveType.AnnualLeave, text: "Annual Leave" },
        { id: LeaveType.SickLeave, text: "Sick Leave" },
    ]

    export let leaveApplication: LeaveApplication;
    export let show: boolean;
    export let title: string;

    const onSubmit = () => dispatcher("submit", leaveApplication);

    const onchange = (e: Event) => {}

</script>

<style lang="scss">
    .show {
        display: block;
    }

    .opacue {
        opacity: 0.5;
    }
</style>

<div id="add-leave-application" 
     class="modal"
     class:show={show} 
     tabindex="-1">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">{title}</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close" on:click={() => show = false}></button>
            </div>
            <div class="modal-body">
                <form class="container-fluid"
                    on:submit|preventDefault={onSubmit}>
                    <div class="row">
                        <div class="form-group col-12">
                            <label for="from">Leave Type</label>
                            <select id="from" class="form-control" type="date" bind:value={leaveApplication.leaveType}>
                                {#each options as option}
                                    <option value={option.id}>{option.text}</option>
                                {/each}
                            </select>
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-group col-12">
                            <label for="from">from</label>
                            <input id="from" class="form-control" type="date" 
                                   value={leaveApplication.from.toISOString().substr(0, 10)} 
                                   on:change={(e) => leaveApplication.from = new Date(e.target?.value)} />
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-group col-12">
                            <label for="to">to</label>
                            <input id="to" class="form-control" type="date" 
                                   value={leaveApplication.to.toISOString().substr(0, 10)} 
                                   on:change={(e) => leaveApplication.to = new Date(e.target?.value)} />
                        </div>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-primary" on:click={onSubmit}>Ok</button>
            </div>
        </div>
    </div>
</div>

{#if show}
<div class="modal-backdrop opacue"></div>
{/if}