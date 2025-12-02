namespace FancySignup.ViewModels
{
    public class AdminUserViewModel
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string CountryName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int AdminUsers { get; set; }
        public int InactiveUsers { get; set; }

        public List<AdminUserViewModel> Users { get; set; } = new();
    }
}
