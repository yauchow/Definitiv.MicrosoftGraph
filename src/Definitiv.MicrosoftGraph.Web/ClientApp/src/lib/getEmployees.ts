import appsettings from "./appsettings";


export interface Employee {
    id: string,
    emailAddress: string
}

const getEmployees = async (): Promise<Employee[]> => {
    var response = await fetch(`${appsettings.apiUrl}/api/employees`);

    return await response.json();
}

export default getEmployees;