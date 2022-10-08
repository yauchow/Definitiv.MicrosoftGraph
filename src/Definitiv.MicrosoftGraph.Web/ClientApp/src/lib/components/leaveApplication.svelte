<script lang="ts">
	import deleteLeaveApplication from "$lib/deleteLeaveApplication";
	import type { LeaveApplication } from "$lib/getLeaveApplications";
	import updateLeaveApplication from "$lib/updateLeaveApplication";
	import { createEventDispatcher } from "svelte";
	import LeaveApplicationModal from "./leaveApplicationModal.svelte";

    export let leaveApplication: LeaveApplication;

    const dispatcher = createEventDispatcher();
    let showUpdateForm = false;

    const showUpdateLeaveApplicationClicked = () => {
        showUpdateForm = true;
    }

    const onDeleteLeaveApplicationClicked = async () => {
        await deleteLeaveApplication(leaveApplication.employeeId, leaveApplication.id);

        dispatcher("deleted", leaveApplication);
    }

    const onSubmited = async () => {

        await updateLeaveApplication(leaveApplication);

        showUpdateForm = false;
    }
</script>

{leaveApplication.id} - from: { leaveApplication.from.toLocaleString() } to: {leaveApplication.to.toLocaleString()}

<button class="btn btn-secondary" on:click={showUpdateLeaveApplicationClicked}>Update</button>
<button class="btn btn-danger" on:click={onDeleteLeaveApplicationClicked}>Delete</button>

<LeaveApplicationModal
    title="Update"
    bind:show={showUpdateForm}
    bind:leaveApplication={leaveApplication}
    on:submit={onSubmited} />