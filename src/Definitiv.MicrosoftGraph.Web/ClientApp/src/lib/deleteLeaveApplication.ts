import appsettings from "./appsettings";

const deleteLeaveApplication = async (employeeId: string, leaveApplicationid: string): Promise<void> => {
    await fetch(
        `${appsettings.apiUrl}/api/employees/${employeeId}/leave-applications/${leaveApplicationid}`,
        {method: 'DELETE'}
    );
}

export default deleteLeaveApplication;