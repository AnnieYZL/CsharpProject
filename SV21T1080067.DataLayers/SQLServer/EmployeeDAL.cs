using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using SV21T1080067.DomainModels;

namespace SV21T1080067.DataLayers.SQLServer
{
    public class EmployeeDAL : _BaseDAL, ICommonDAL<Employee>
    {
        public EmployeeDAL(string connectionString) : base(connectionString)
        {
        }

        public int Add(Employee data)
        {
            int id = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"insert into Employees(FullName, BirthDate, Address, Phone, Email, Photo, IsWorking) 
                            VALUES(@FullName, @BirthDate, @Address, @Phone, @Email, @Photo, @IsWorking); select @@IDENTITY";
                var parameters = new
                {
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    IsWorking = data.IsWorking
                };
                id = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return id;
        }

        public int Count(string searchValue = "")
        {
            int count = 0;
            using (var connection = OpenConnection())
            {
                var sql = @"select count(*)
	                        from Employees
	                        where (FullName like @searchValue)";
                var parameters = new { searchValue = $"%{searchValue}%" };
                count = connection.ExecuteScalar<int>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return count;
        }

        public bool Delete(int id)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"delete from Employees
	                        where EmployeeId = @EmployeeId";
                var parameters = new { EmployeeId = id };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }

        public Employee? Get(int id)
        {
            Employee? data = null;
            using (var connection = OpenConnection())
            {
                var sql = @"select * from Employees
	                        where EmployeeId = @EmployeeId";
                var parameters = new { EmployeeId = id };
                data = connection.QueryFirstOrDefault<Employee>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return data;
        }
        /// <summary>
        /// Chưa viết
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool InUsed(int id)
        {

            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"if exists(select * from Orders where EmployeeId = @EmployeeId ) select 1 else select 0";
                var parameters = new { EmployeeId = id };
                result = connection.ExecuteScalar<bool>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text);
                connection.Close();
            }
            return result;
        }

        public IList<Employee> List(int page = 1, int pageSize = 0, string searchValue = "")
        {
            List<Employee> data = new List<Employee>();
            using (var connection = OpenConnection())
            {
                var sql = @"select * from (
	            select * , row_number() over (order by FullName) as RowNumber
	            from Employees
	            where (FullName like @searchValue)

            ) as t
            where (@pageSize = 0) or (RowNumber between (@page - 1) * @pageSize+1 and @page * @pageSize)
            order by RowNumber;";
                var parameters = new
                {
                    page,
                    pageSize,
                    searchValue = $"%{searchValue}%"
                };
                data = connection.Query<Employee>(sql: sql, param: parameters, commandType: System.Data.CommandType.Text).ToList();
            }
            return data;
        }

        public bool Update(Employee data)
        {
            bool result = false;
            using (var connection = OpenConnection())
            {
                var sql = @"update Employees
                            set FullName = @FullName, BirthDate = @BirthDate, Address = @Address, Phone = @Phone, Email = @Email, Photo = @Photo, IsWorking = @IsWorking
                            where EmployeeId = @EmployeeId";
                var parameters = new
                {
                    EmployeeId = data.EmployeeID,
                    FullName = data.FullName ?? "",
                    BirthDate = data.BirthDate,
                    Address = data.Address ?? "",
                    Phone = data.Phone ?? "",
                    Email = data.Email ?? "",
                    Photo = data.Photo ?? "",
                    IsWorking = data.IsWorking
                };
                result = connection.Execute(sql: sql, param: parameters, commandType: System.Data.CommandType.Text) > 0;
                connection.Close();
            }
            return result;
        }
    }
}
