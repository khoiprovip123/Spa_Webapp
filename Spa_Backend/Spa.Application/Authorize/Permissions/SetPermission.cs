using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spa.Application.Authorize.Permissions
{
    public enum SetPermission
    {
        //Appointment
        GetAllApointment=1,
        GetAllByBranch=2,
        GetAllByStatus=3,
        CreateAppointment=4,
        GetAppointmentById=5,
        UpdateStatus=7,
        AssignTechnicalStaff=8,
        UpdateAppointmentWithoutService=9,
        UpdateAppointmentWithService=10,
        UpdateAppointment=11,
        DeleteAppointmentById=12,
        UpdateDiscount=13,

        //Authentication
        CreateUser=14,
        CreateUserForEmployee=15,

        //Bill
        CreateBill=16,
        GetAllBillAsync=17,
        GetBillByIdAsync=18,
        UpdateBill=19,

        //Branch
        GetAllBranches=20,
        GetBranchByID=21,
        GetBranchNameByID=22,
        CreateBranch=23,
        UpdateBranch=24,
        DeleteBranch=25,

        //Customer
        GetAllCustomer=26,
        GetAllByPage=27,
        GetCusomerById=28,
        CreateCustomer=29,
        UpdateCustomer=30,
        DeactivateCustomer=31,
        SearchCustomers=32,
        UploadImage=33,
        UploadImages=34,
        GetHistoryCustomerById=35,

        //CustomerType
        GetAllCustomerTypes=36,
        GetCustomerTypeById=37,
        CreateCustomerType=38,
        UpdateCustomerType=39,
        DeleteCustomerType=40,

        //Job/JobType
        GetAllJobs=41,
        GetJobTypeByID=42,
        CreateJobType=43,
        UpdateJob=44,
        DeleteJob=45,

        //Payment
        GetPaymentByDay=46,
        GetPaymenOfCustomer=47,
        AddPayment=48,
        GetPaymentByBranch=49,
        ExportExelPayment=50,

        //Permission
        GetAllPermissions=51,
        GetPermissionsByJobType=52,
        GetPermissionsByName=53,
        GetPermissionNameByJobType=54,
        GetRolePermissionByID=55,
        CreateRolePermission=56,
        CreatePermission=57,
        DeleteRolePermission=58,
        DeletePermission=59,
        UpdatePermission=60,

        //Service
        GetAllService=61,
        GetServiceById=62,
        CreateService=63,
        UpdateService=64,
        DeleteService=65,

        //User
        OnlyUser = 66,
        ViewAllUser = 67,
        UserPage = 68,
        GetAllEmployee = 69,
        EmployeeByBranchAndJob = 70,
        GetAllAdmin=71,
        GetUserByEmail=72,
        GetUserByAdmin=73,
        GetUserByEmployee=74,
        GetUserBoolByEmail=75,
        UpdateUser=76,
        DeleteUser=77,

        //Report
        GetRevenueReportByBranch=78,
        GetRevenueReportByDay=79,

        //Bill
        GetBillHistory=80,
        GetBillByCustomer=81,

        //Appointment
        InfoToCreateBill=82,
        GetAppointmentByDay=83,
    }
}
