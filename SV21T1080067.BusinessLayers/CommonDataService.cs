using SV21T1080067.DataLayers;
using SV21T1080067.DomainModels;

namespace SV21T1080067.BusinessLayers
{
    public static class CommonDataService
    {
        private static readonly ICommonDAL<Province> provinceDB;
        private static readonly ICommonDAL<Customer> customerDB;
        private static readonly ICommonDAL<Supplier> supplierDB;
        private static readonly ICommonDAL<Shipper> shipperDB;
        private static readonly ICommonDAL<Employee> employeeDB;
        private static readonly ICommonDAL<Category> categoryDB;

        public static ICommonDAL<Province> ProvinceDB => provinceDB;

        /// <summary>
        /// Ctor
        /// </summary>
        static CommonDataService()
            {
            provinceDB = new DataLayers.SQLServer.ProvinceDAL(Configuration.ConnectionString);
            customerDB = new DataLayers.SQLServer.CustomerDAL(Configuration.ConnectionString);
            supplierDB = new DataLayers.SQLServer.SupplierDAL(Configuration.ConnectionString);
            shipperDB = new DataLayers.SQLServer.ShipperDAL(Configuration.ConnectionString);
            employeeDB = new DataLayers.SQLServer.EmployeeDAL(Configuration.ConnectionString);
            categoryDB = new DataLayers.SQLServer.CategoryDAL(Configuration.ConnectionString);
            }
        /// <summary>
        /// Lấy danh sách toàn bộ tỉnh thành
        /// </summary>
        /// <returns></returns>
        public static List<Province> ListOfProvinces()
        {
            return provinceDB.List().ToList();
        }
        


        /// <summary>
        /// Danh sách khách hàng (tìm kiếm, phân trang)
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = customerDB.Count(searchValue);
            return customerDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Danh sách khách hàng (tìm kiếm, không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Customer> ListOfCustomers(string searchValue = "")
        {
            return customerDB.List(1,0, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 khách hàng dựa vào mã khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Customer? GetCustomer(int id)
        {
            if(id<=0) return null;
            return customerDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1 khách hàng mới. Hàm trả về id của khách hàng được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCustomer(Customer data)
        {
            return customerDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin khách hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCustomer(Customer data)
        {
            return customerDB.Update(data);
        }
        /// <summary>
        /// Xóa 1 khách hàng dựa vào mã khách hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCustomer(int id) {
            return customerDB.Delete(id); 
        }
        /// <summary>
        /// Kiểm tra khách hàng có mã là id hiện có dữ liệu liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCustomer(int id)
        {
            return customerDB.InUsed(id);
        }




        /// <summary>
        /// Danh sách loại hàng (tìm kiếm, phân trang)
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = categoryDB.Count(searchValue);
            return categoryDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Danh sách loại hàng (tìm kiếm, không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Category> ListOfCategories(string searchValue = "")
        {
            return categoryDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 loại hàng dựa vào mã loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Category? GetCategory(int id)
        {
            if (id <= 0) return null;
            return categoryDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1 loại hàng mới. Hàm trả về id của loại hàng được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddCategory(Category data)
        {
            return categoryDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin loại hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateCategory(Category data)
        {
            return categoryDB.Update(data);
        }
        /// <summary>
        /// Xóa 1 loại hàng dựa vào mã loại hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteCategory(int id)
        {
            return categoryDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra loại hàng có mã là id hiện có dữ liệu liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedCategory(int id)
        {
            return categoryDB.InUsed(id);
        }





        /// <summary>
        /// Danh sách nhà cung cấp (tìm kiếm, phân trang)
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = supplierDB.Count(searchValue);
            return supplierDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Danh sách nhà cung cấp (tìm kiếm, không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Supplier> ListOfSuppliers(string searchValue = "")
        {
            return supplierDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 nhà cung cấp dựa vào mã nhà cung cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Supplier? GetSupplier(int id)
        {
            if (id <= 0) return null;
            return supplierDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1 nhà cung cấp mới. Hàm trả về id của nhà cung cấp được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddSupplier(Supplier data)
        {
            return supplierDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin nhà cung cấp
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateSupplier(Supplier data)
        {
            return supplierDB.Update(data);
        }
        /// <summary>
        /// Xóa 1 nhà cung cấp dựa vào mã nhà cung cấp
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteSupplier(int id)
        {
            return supplierDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra nhà cung cấp có mã là id hiện có dữ liệu liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedSupplier(int id)
        {
            return supplierDB.InUsed(id);
        }





        /// <summary>
        /// Danh sách người giao hàng (tìm kiếm, phân trang)
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = shipperDB.Count(searchValue);
            return shipperDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Danh sách người giao hàng (tìm kiếm, không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Shipper> ListOfShippers(string searchValue = "")
        {
            return shipperDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 người giao hàng dựa vào mã người giao hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Shipper? GetShipper(int id)
        {
            if (id <= 0) return null;
            return shipperDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1  người giao hàng mới. Hàm trả về id của người giao hàng được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddShipper(Shipper data)
        {
            return shipperDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin người giao hàng
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateShipper(Shipper data)
        {
            return shipperDB.Update(data);
        }
        /// <summary>
        /// Xóa 1 người giao hàng dựa vào mã người giao hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteShipper(int id)
        {
            return shipperDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra người giao hàng có mã là id hiện có dữ liệu liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedShipper(int id)
        {
            return shipperDB.InUsed(id);
        }





        /// <summary>
        /// Danh sách nhân viên (tìm kiếm, phân trang)
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(out int rowCount, int page = 1, int pageSize = 0, string searchValue = "")
        {
            rowCount = employeeDB.Count(searchValue);
            return employeeDB.List(page, pageSize, searchValue).ToList();
        }
        /// <summary>
        /// Danh sách nhân viên (tìm kiếm, không phân trang)
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public static List<Employee> ListOfEmployees(string searchValue = "")
        {
            return employeeDB.List(1, 0, searchValue).ToList();
        }
        /// <summary>
        /// Lấy thông tin của 1 nhân viên dựa vào mã nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Employee? GetEmployee(int id)
        {
            if (id <= 0) return null;
            return employeeDB.Get(id);
        }
        /// <summary>
        /// Bổ sung 1  nhân viên mới. Hàm trả về id của nhân viên được bổ sung
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int AddEmployee(Employee data)
        {
            return employeeDB.Add(data);
        }
        /// <summary>
        /// Cập nhật thông tin nhân viên
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool UpdateEmployee(Employee data)
        {
            return employeeDB.Update(data);
        }
        /// <summary>
        /// Xóa 1  nhân viên dựa vào mã nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool DeleteEmployee(int id)
        {
            return employeeDB.Delete(id);
        }
        /// <summary>
        /// Kiểm tra  nhân viên có mã là id hiện có dữ liệu liên quan hay không.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool IsUsedEmployee(int id)
        {
            return employeeDB.InUsed(id);
        }


    }
}
