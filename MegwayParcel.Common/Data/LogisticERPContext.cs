using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable
namespace MegwayParcel.Common.Data
{
    public partial class LogisticERPContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
       
        protected readonly IConfiguration Configuration;

       

        public LogisticERPContext(DbContextOptions<LogisticERPContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Blog> Blogs { get; set; }
        public virtual DbSet<CompanySetting> CompanySettings { get; set; }
        public virtual DbSet<ConsignmentSummary> ConsignmentSummaries { get; set; }
        public virtual DbSet<ContactU> ContactUs { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerInvoice> CustomerInvoices { get; set; }
        public virtual DbSet<DeliveryDetail> DeliveryDetails { get; set; }
        public virtual DbSet<ExporterDetail> ExporterDetails { get; set; }
        public virtual DbSet<GuestUser> GuestUsers { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Page> Pages { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RoleMenu> RoleMenus { get; set; }
        public virtual DbSet<SiteMap> SiteMaps { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<ServicesName> ServicesName { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<LandMarkPrices> LandMarkPrices { get; set; }

        public DbSet<LandMarkCountries> LandMarkCountries { get; set; }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //var connectionString = Configuration.GetConnectionString("DefaultConnection");

            //var optionsBuilder1 = new DbContextOptionsBuilder<LogisticERPContext>();
            //optionsBuilder1.UseSqlServer(connectionString);
            //optionsBuilder1.EnableSensitiveDataLogging();

            if (!optionsBuilder.IsConfigured)
            {
                // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                //optionsBuilder.UseSqlServer("Server=204.12.227.112;Database=Megway_Parcels;Integrated Security=True;Integrated Security=False;Trusted_Connection=False;User ID=megwayuser; Password=root123;MultipleActiveResultSets=True;Connection Timeout=2000");
                //optionsBuilder.UseSqlServer("Server=ZQ\\SQLEXPRESS;Database=Megway_Parcels2;User ID=megwayuser; Password=root123;Integrated Security=False;Trusted_Connection=False");
                //optionsBuilder.UseSqlServer("Server=.;Database=MegwayLoc;User ID=megwayuser; Password=root123;Integrated Security=False;Trusted_Connection=False");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");

                entity.Property(e => e.AddressLineOne)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.AddressLineTwo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.County)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Eorinumber)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("EORINumber");

                entity.Property(e => e.Forename)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Postcode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.TelephoneNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Address_Order");
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.ToTable("Blog");

                entity.Property(e => e.AuthorName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ShortDetail)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanySetting>(entity =>
            {
                entity.ToTable("CompanySetting");

                entity.Property(e => e.CollectionAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InsuranceCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RemoteSurchargePercent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SettingName)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ShippingPercent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SurchargePercent).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.VatchargePercent)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("VATChargePercent");
            });

            modelBuilder.Entity<ConsignmentSummary>(entity =>
            {
                entity.ToTable("ConsignmentSummary");

                entity.Property(e => e.ContentSummary)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentCategory)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DocumentUrl).HasMaxLength(250);

                entity.Property(e => e.ReasonForShipment)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalGoodsValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalValueForCustoms).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ConsignmentSummaries)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConsignmentSummary_Order");
            });

            modelBuilder.Entity<ContactU>(entity =>
            {
                entity.HasKey(e => e.ContactUsId)
                    .HasName("PK__ContactU__E10B7AC8CD6FB5BD");

                entity.Property(e => e.ContactUsId).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.AcountName).HasMaxLength(250);

                entity.Property(e => e.AddForeName).HasMaxLength(250);

                entity.Property(e => e.AddLine1).HasMaxLength(250);

                entity.Property(e => e.AddLine2).HasMaxLength(500);

                entity.Property(e => e.AddSurName).HasMaxLength(250);

                entity.Property(e => e.AddTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.BusinessType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.ConfirmPassword)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DefAddressLineOne)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DefAddressLineTwo)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DefCity)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DefCompanyName)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.DefCounty)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DefEmailAddress)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DefForename)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DefMobileNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DefPostcode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DefSurname)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DefTelephoneNumber)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.DefTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EmailCode).HasMaxLength(50);

                entity.Property(e => e.EmailExpiry).HasColumnType("datetime");

                entity.Property(e => e.Eorinumber)
                    .HasMaxLength(500)
                    .HasColumnName("EORINumber");

                entity.Property(e => e.ForeName).HasMaxLength(250);

                entity.Property(e => e.IossNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobileNo).HasMaxLength(50);

                entity.Property(e => e.MyVatNumber).HasMaxLength(500);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Postcode).HasMaxLength(50);

                entity.Property(e => e.State).HasMaxLength(50);

                entity.Property(e => e.SurName).HasMaxLength(250);

                entity.Property(e => e.TelephoneNumber).HasMaxLength(50);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.CompanySetting)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.CompanySettingId)
                    .HasConstraintName("FK_Customer_CompanySetting");
            });

            modelBuilder.Entity<CustomerInvoice>(entity =>
            {
                entity.ToTable("CustomerInvoice");

                entity.Property(e => e.CommodityCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PerItemValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PerItemWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductDescription)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ShippingCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalGoodsValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalValueForCustoms).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ValueForCustoms).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.CustomerInvoices)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_CustomerInvoice_Order");
            });

            modelBuilder.Entity<DeliveryDetail>(entity =>
            {
                entity.ToTable("DeliveryDetail");

                entity.Property(e => e.ReceiverTaxId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.DeliveryDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_DeliveryDetail_Order");
            });

            modelBuilder.Entity<ExporterDetail>(entity =>
            {
                entity.ToTable("ExporterDetail");

                entity.Property(e => e.ExporterEorino)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ExporterEORINo");

                entity.Property(e => e.ExporterVatno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("ExporterVATNo");

                entity.Property(e => e.IossNumber)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.ExporterDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK_ExporterDetail_Order");
            });

            modelBuilder.Entity<GuestUser>(entity =>
            {
                entity.ToTable("GuestUser");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailExpiry).HasColumnType("datetime");
            });

            modelBuilder.Entity<Menu>(entity =>
            {
                entity.ToTable("Menu");

                entity.Property(e => e.ActionName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ControllerName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(e => e.AirWaybillReference)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CollectionCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CollectionDate).HasColumnType("date");

                entity.Property(e => e.CollectionEmailError).IsUnicode(false);

                entity.Property(e => e.CollectionTime)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeliveryEmailError).IsUnicode(false);

                entity.Property(e => e.ErrorResponse).IsUnicode(false);

                entity.Property(e => e.InsuranceCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InsuranceValue).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceEmailError).IsUnicode(false);

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.OrderEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentRefrenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RefrenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.RemoteSurcharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ResidentialSurcharge).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SelectedCompanyLogo).HasMaxLength(50);

                entity.Property(e => e.ShippingCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.SignaturePrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Surcharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TotalCharges).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Vatcharges)
                    .HasColumnType("decimal(18, 2)")
                    .HasColumnName("VATCharges");
            });

            modelBuilder.Entity<Page>(entity =>
            {
                entity.ToTable("Page");

                entity.Property(e => e.Prefix)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.ToTable("RoleMenu");

                entity.HasOne(d => d.Menu)
                    .WithMany(p => p.RoleMenus)
                    .HasForeignKey(d => d.MenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RollMenu_Menu");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleMenus)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RollMenu_Role");
            });

            modelBuilder.Entity<SiteMap>(entity =>
            {
                entity.ToTable("SiteMap");

                entity.Property(e => e.Detail).IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Url)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhoneNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Role");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
