import appsettings from "./appsettings";

export enum LeaveType {
    AnnualLeave = 0,
    SickLeave = 1
}

export interface LeaveApplication {
    id: string,
    leaveType: LeaveType,
    employeeId: string,
    from: Date,
    to: Date,
    outlookEventId?: string
}

const getLeaveApplications = async (): Promise<LeaveApplication[]> => {
    const response = await fetch(`${appsettings.apiUrl}/api/leave-applications`);

    let leaveApplications = await response.json();

    leaveApplications = leaveApplications.map((leaveApplication: LeaveApplication) => (
        { 
            ... leaveApplication, 
            from: new Date(leaveApplication.from), 
            to: new Date(leaveApplication.to)
        }));

    return leaveApplications;
}

export default getLeaveApplications;