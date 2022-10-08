import appsettings from "./appsettings"
import type { LeaveType } from "./getLeaveApplications";

const addLeaveApplication = async (employeeId: string, leaveType: LeaveType, from: Date, to: Date) : Promise<void> => {
    await fetch(
        `${appsettings.apiUrl}/api/employees/${employeeId}/leave-applications`,
        { 
            method: "POST",
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify({
                leaveType: Number(leaveType),
                from,
                to
            })
        });
} 

export default addLeaveApplication;