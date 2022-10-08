import appsettings from "./appsettings";
import type { LeaveApplication } from "./getLeaveApplications";

const updateLeaveApplication = async (leaveApplication: LeaveApplication): Promise<void> => {
    await fetch(
        `${appsettings.apiUrl}/api/employees/${leaveApplication.employeeId}/leave-applications/${leaveApplication.id}`,
        { 
            method: "PUT",
            headers: {
                'content-type': 'application/json'
            },
            body: JSON.stringify(
                {
                    ... leaveApplication,
                    leaveType: Number(leaveApplication.leaveType)
                })
        });
}

export default updateLeaveApplication;