<script lang="ts">
	import addLeaveApplication from "$lib/addLeaveApplication";
	import type { Employee } from "$lib/getEmployees";
	import { LeaveType, type LeaveApplication } from "$lib/getLeaveApplications";
	import { createEventDispatcher } from "svelte";
	import LeaveApplicationModal from "./leaveApplicationModal.svelte";

    export let employee: Employee;
    export let show: boolean;

    const dispatcher = createEventDispatcher();

    let leaveApplication: LeaveApplication = { id: "", employeeId: "", leaveType: LeaveType.AnnualLeave, from: new Date(), to: new Date()};

    const onSubmit = async () => {
        await addLeaveApplication(employee.id, leaveApplication.leaveType, leaveApplication.from, leaveApplication.to);

        dispatcher("submitted");
    }
</script>

<LeaveApplicationModal
    title={`Add leave for ${employee.emailAddress}`}
    bind:leaveApplication={leaveApplication} 
    bind:show={show}
    on:submit={onSubmit}
    />

