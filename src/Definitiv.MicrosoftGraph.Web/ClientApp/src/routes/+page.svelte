<script lang="ts">

    import appsettings from "$lib/appsettings";
	import type { Employee } from "$lib/getEmployees";
	import getEmployees from "$lib/getEmployees";
	import { onMount } from "svelte";
    import AddLeaveApplication from "$lib/components/addLeaveApplication.svelte";
    import LeaveApplicationEdit from "$lib/components/leaveApplication.svelte";
	import type { LeaveApplication } from "$lib/getLeaveApplications";
	import getLeaveApplications from "$lib/getLeaveApplications";

    let employees: Employee[] = [];
    let leaveApplications: LeaveApplication[] = [];
    let showAddLeaveApplication = false;
    let selectedEmployee: Employee;

    onMount(async () => { 
        employees = await getEmployees(); 
        leaveApplications = await getLeaveApplications();

    });

    const addLeaveApplication = (employee: Employee) => {
        selectedEmployee = employee;

        showAddLeaveApplication = true;
    }

    const onLeaveApplicationsUpdated = async () => {
        showAddLeaveApplication = false;
        leaveApplications = await getLeaveApplications();
    }

</script>

<h1>Welcome to SvelteKit</h1>
<p>Visit <a href="https://kit.svelte.dev">kit.svelte.dev</a> to read the documentation</p>

<h2>Employees</h2>

<div class="row">
    <div class="col-12">
        <ul class="list-group">
            {#each employees as employee}
                <li class="list-group-item">
                    {employee.emailAddress} 
                    <button class="btn btn-primary" 
                            on:click={() => addLeaveApplication(employee)}>
                        Add Leave Application
                    </button>
                </li>
            {/each }
        </ul>
    </div>
</div>

{#if selectedEmployee}
<AddLeaveApplication employee={selectedEmployee} 
                     bind:show={showAddLeaveApplication} 
                     on:submitted={onLeaveApplicationsUpdated} />
{/if}

<h2>Leave Applications</h2>

<div class="row">
    <div class="col-12">
        <ul class="list-group">
        {#each leaveApplications as leaveApplication}
            <li class="list-group-item">
                <LeaveApplicationEdit {leaveApplication} on:deleted={onLeaveApplicationsUpdated} />
            </li>
        {/each}
        </ul>
    </div>
</div>


