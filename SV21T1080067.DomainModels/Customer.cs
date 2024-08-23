namespace SV21T1080067.DomainModels
{
    /// <summary>
    /// Lop bieu dien thong tin ve khach hang
    /// </summary>
    public class Customer
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsLocked { get; set; } = false;

    }
}
