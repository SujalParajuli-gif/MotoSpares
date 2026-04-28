using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MotoSpares.Domain.Entities;

namespace MotoSpares.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    public DbSet<Part> Parts => Set<Part>();
    public DbSet<PurchaseInvoice> PurchaseInvoices => Set<PurchaseInvoice>();
    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();
    public DbSet<SaleInvoice> SaleInvoices => Set<SaleInvoice>();
    public DbSet<SaleItem> SaleItems => Set<SaleItem>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<PartRequest> PartRequests => Set<PartRequest>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<UserVehicle> UserVehicles => Set<UserVehicle>();
    public DbSet<UserVendor> UserVendors => Set<UserVendor>();
    public DbSet<UserPart> UserParts => Set<UserPart>();
    public DbSet<UserPurchaseInvoice> UserPurchaseInvoices => Set<UserPurchaseInvoice>();
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems => Set<PurchaseInvoiceItem>();
    public DbSet<UserSaleInvoice> UserSaleInvoices => Set<UserSaleInvoice>();
    public DbSet<SaleInvoiceItem> SaleInvoiceItems => Set<SaleInvoiceItem>();
    public DbSet<UserAppointment> UserAppointments => Set<UserAppointment>();
    public DbSet<UserPartRequest> UserPartRequests => Set<UserPartRequest>();
    public DbSet<UserReview> UserReviews => Set<UserReview>();
    public DbSet<UserNotification> UserNotifications => Set<UserNotification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureIdentityTables(modelBuilder);
        ConfigureApplicationUser(modelBuilder.Entity<ApplicationUser>());
        ConfigureVehicle(modelBuilder.Entity<Vehicle>());
        ConfigureVendor(modelBuilder.Entity<Vendor>());
        ConfigurePart(modelBuilder.Entity<Part>());
        ConfigurePurchaseInvoice(modelBuilder.Entity<PurchaseInvoice>());
        ConfigurePurchaseItem(modelBuilder.Entity<PurchaseItem>());
        ConfigureSaleInvoice(modelBuilder.Entity<SaleInvoice>());
        ConfigureSaleItem(modelBuilder.Entity<SaleItem>());
        ConfigureAppointment(modelBuilder.Entity<Appointment>());
        ConfigurePartRequest(modelBuilder.Entity<PartRequest>());
        ConfigureReview(modelBuilder.Entity<Review>());
        ConfigureNotification(modelBuilder.Entity<Notification>());
        ConfigureUserVehicle(modelBuilder.Entity<UserVehicle>());
        ConfigureUserVendor(modelBuilder.Entity<UserVendor>());
        ConfigureUserPart(modelBuilder.Entity<UserPart>());
        ConfigureUserPurchaseInvoice(modelBuilder.Entity<UserPurchaseInvoice>());
        ConfigurePurchaseInvoiceItem(modelBuilder.Entity<PurchaseInvoiceItem>());
        ConfigureUserSaleInvoice(modelBuilder.Entity<UserSaleInvoice>());
        ConfigureSaleInvoiceItem(modelBuilder.Entity<SaleInvoiceItem>());
        ConfigureUserAppointment(modelBuilder.Entity<UserAppointment>());
        ConfigureUserPartRequest(modelBuilder.Entity<UserPartRequest>());
        ConfigureUserReview(modelBuilder.Entity<UserReview>());
        ConfigureUserNotification(modelBuilder.Entity<UserNotification>());
    }

    private static void ConfigureIdentityTables(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole<Guid>>(entity =>
        {
            entity.ToTable("MS_ROLE");
            entity.Property(role => role.Id).HasColumnName("Role_ID");
            entity.Property(role => role.Name).HasColumnName("Role_Name").HasMaxLength(50);
            entity.Property(role => role.NormalizedName).HasColumnName("Normalized_Role_Name").HasMaxLength(50);
            entity.Property(role => role.ConcurrencyStamp).HasColumnName("Concurrency_Stamp");
        });

        modelBuilder.Entity<IdentityUserRole<Guid>>(entity =>
        {
            entity.ToTable("MS_USER_ROLE");
            entity.Property(userRole => userRole.UserId).HasColumnName("User_ID");
            entity.Property(userRole => userRole.RoleId).HasColumnName("Role_ID");
        });

        modelBuilder.Entity<IdentityUserClaim<Guid>>(entity =>
        {
            entity.ToTable("MS_USER_CLAIM");
            entity.Property(userClaim => userClaim.Id).HasColumnName("User_Claim_ID");
            entity.Property(userClaim => userClaim.UserId).HasColumnName("User_ID");
            entity.Property(userClaim => userClaim.ClaimType).HasColumnName("Claim_Type");
            entity.Property(userClaim => userClaim.ClaimValue).HasColumnName("Claim_Value");
        });

        modelBuilder.Entity<IdentityUserLogin<Guid>>(entity =>
        {
            entity.ToTable("MS_USER_LOGIN");
            entity.Property(userLogin => userLogin.LoginProvider).HasColumnName("Login_Provider");
            entity.Property(userLogin => userLogin.ProviderKey).HasColumnName("Provider_Key");
            entity.Property(userLogin => userLogin.ProviderDisplayName).HasColumnName("Provider_Display_Name");
            entity.Property(userLogin => userLogin.UserId).HasColumnName("User_ID");
        });

        modelBuilder.Entity<IdentityRoleClaim<Guid>>(entity =>
        {
            entity.ToTable("MS_ROLE_CLAIM");
            entity.Property(roleClaim => roleClaim.Id).HasColumnName("Role_Claim_ID");
            entity.Property(roleClaim => roleClaim.RoleId).HasColumnName("Role_ID");
            entity.Property(roleClaim => roleClaim.ClaimType).HasColumnName("Claim_Type");
            entity.Property(roleClaim => roleClaim.ClaimValue).HasColumnName("Claim_Value");
        });

        modelBuilder.Entity<IdentityUserToken<Guid>>(entity =>
        {
            entity.ToTable("MS_USER_TOKEN");
            entity.Property(userToken => userToken.UserId).HasColumnName("User_ID");
            entity.Property(userToken => userToken.LoginProvider).HasColumnName("Login_Provider");
            entity.Property(userToken => userToken.Name).HasColumnName("Token_Name");
            entity.Property(userToken => userToken.Value).HasColumnName("Token_Value");
        });
    }

    private static void ConfigureApplicationUser(EntityTypeBuilder<ApplicationUser> entity)
    {
        entity.ToTable("MS_USER");

        entity.Property(user => user.Id).HasColumnName("User_ID");
        entity.Property(user => user.FullName).HasColumnName("Full_Name").HasMaxLength(100).IsRequired();
        entity.Property(user => user.Email).HasColumnName("Email").HasMaxLength(100).IsRequired();
        entity.Property(user => user.PasswordHash).HasColumnName("Password_Hash");
        entity.Property(user => user.PhoneNumber).HasColumnName("Phone").HasMaxLength(20);
        entity.Property(user => user.Address).HasColumnName("Address").HasMaxLength(200);
        entity.Property(user => user.Role).HasColumnName("Role").HasMaxLength(20).IsRequired();
        entity.Property(user => user.CreatedAt).HasColumnName("Created_At").IsRequired();
        entity.Property(user => user.UserName).HasColumnName("User_Name").HasMaxLength(100);
        entity.Property(user => user.NormalizedUserName).HasColumnName("Normalized_User_Name").HasMaxLength(100);
        entity.Property(user => user.NormalizedEmail).HasColumnName("Normalized_Email").HasMaxLength(100);
        entity.Property(user => user.EmailConfirmed).HasColumnName("Email_Confirmed");
        entity.Property(user => user.PhoneNumberConfirmed).HasColumnName("Phone_Confirmed");
        entity.Property(user => user.SecurityStamp).HasColumnName("Security_Stamp");
        entity.Property(user => user.ConcurrencyStamp).HasColumnName("Concurrency_Stamp");
        entity.Property(user => user.TwoFactorEnabled).HasColumnName("Two_Factor_Enabled");
        entity.Property(user => user.LockoutEnd).HasColumnName("Lockout_End");
        entity.Property(user => user.LockoutEnabled).HasColumnName("Lockout_Enabled");
        entity.Property(user => user.AccessFailedCount).HasColumnName("Access_Failed_Count");

        entity.HasIndex(user => user.Email).IsUnique();
    }

    private static void ConfigureVehicle(EntityTypeBuilder<Vehicle> entity)
    {
        entity.ToTable("MS_VEHICLE");

        entity.HasKey(vehicle => vehicle.VehicleId);
        entity.Property(vehicle => vehicle.VehicleId).HasColumnName("Vehicle_ID");
        entity.Property(vehicle => vehicle.VehicleNumber).HasColumnName("Vehicle_Number").HasMaxLength(50).IsRequired();
        entity.Property(vehicle => vehicle.Make).HasColumnName("Make").HasMaxLength(100).IsRequired();
        entity.Property(vehicle => vehicle.Model).HasColumnName("Model").HasMaxLength(100).IsRequired();
        entity.Property(vehicle => vehicle.Year).HasColumnName("Year").IsRequired();

        entity.HasIndex(vehicle => vehicle.VehicleNumber).IsUnique();
    }

    private static void ConfigureVendor(EntityTypeBuilder<Vendor> entity)
    {
        entity.ToTable("MS_VENDOR");

        entity.HasKey(vendor => vendor.VendorId);
        entity.Property(vendor => vendor.VendorId).HasColumnName("Vendor_ID");
        entity.Property(vendor => vendor.VendorName).HasColumnName("Vendor_Name").HasMaxLength(100).IsRequired();
        entity.Property(vendor => vendor.VendorEmail).HasColumnName("Vendor_Email").HasMaxLength(100);
        entity.Property(vendor => vendor.VendorPhone).HasColumnName("Vendor_Phone").HasMaxLength(20);
        entity.Property(vendor => vendor.VendorAddress).HasColumnName("Vendor_Address").HasMaxLength(200);
    }

    private static void ConfigurePart(EntityTypeBuilder<Part> entity)
    {
        entity.ToTable("MS_PART");

        entity.HasKey(part => part.PartId);
        entity.Property(part => part.PartId).HasColumnName("Part_ID");
        entity.Property(part => part.PartName).HasColumnName("Part_Name").HasMaxLength(100).IsRequired();
        entity.Property(part => part.PartNumber).HasColumnName("Part_Number").HasMaxLength(50).IsRequired();
        entity.Property(part => part.Description).HasColumnName("Description").HasMaxLength(500);
        entity.Property(part => part.Category).HasColumnName("Category").HasMaxLength(100);
        entity.Property(part => part.UnitPrice).HasColumnName("Unit_Price").HasPrecision(18, 2).IsRequired();
        entity.Property(part => part.StockQuantity).HasColumnName("Stock_Quantity").IsRequired();
        entity.Property(part => part.ReorderLevel).HasColumnName("Reorder_Level").IsRequired();

        entity.HasIndex(part => part.PartNumber).IsUnique();
    }

    private static void ConfigurePurchaseInvoice(EntityTypeBuilder<PurchaseInvoice> entity)
    {
        entity.ToTable("MS_PURCHASE_INVOICE");

        entity.HasKey(invoice => invoice.PurchaseInvoiceId);
        entity.Property(invoice => invoice.PurchaseInvoiceId).HasColumnName("Purchase_Invoice_ID");
        entity.Property(invoice => invoice.PurchaseDate).HasColumnName("Purchase_Date").IsRequired();
        entity.Property(invoice => invoice.PurchaseTotal).HasColumnName("Purchase_Total").HasPrecision(18, 2).IsRequired();
        entity.Property(invoice => invoice.VendorId).HasColumnName("Vendor_ID").IsRequired();
        entity.Property(invoice => invoice.CreatedBy).HasColumnName("Created_By").IsRequired();

        entity.HasOne(invoice => invoice.Vendor)
            .WithMany(vendor => vendor.PurchaseInvoices)
            .HasForeignKey(invoice => invoice.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        entity.HasOne(invoice => invoice.CreatedByUser)
            .WithMany(user => user.CreatedPurchaseInvoices)
            .HasForeignKey(invoice => invoice.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigurePurchaseItem(EntityTypeBuilder<PurchaseItem> entity)
    {
        entity.ToTable("MS_PURCHASE_ITEM");

        entity.HasKey(item => item.PurchaseItemId);
        entity.Property(item => item.PurchaseItemId).HasColumnName("Purchase_Item_ID");
        entity.Property(item => item.PurchaseQuantity).HasColumnName("Purchase_Quantity").IsRequired();
        entity.Property(item => item.PurchaseUnitCost).HasColumnName("Purchase_Unit_Cost").HasPrecision(18, 2).IsRequired();
        entity.Property(item => item.PartId).HasColumnName("Part_ID").IsRequired();

        entity.HasOne(item => item.Part)
            .WithMany(part => part.PurchaseItems)
            .HasForeignKey(item => item.PartId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureSaleInvoice(EntityTypeBuilder<SaleInvoice> entity)
    {
        entity.ToTable("MS_SALE_INVOICE");

        entity.HasKey(invoice => invoice.SaleInvoiceId);
        entity.Property(invoice => invoice.SaleInvoiceId).HasColumnName("Sale_Invoice_ID");
        entity.Property(invoice => invoice.SaleDate).HasColumnName("Sale_Date").IsRequired();
        entity.Property(invoice => invoice.Subtotal).HasColumnName("Subtotal").HasPrecision(18, 2).IsRequired();
        entity.Property(invoice => invoice.DiscountAmount).HasColumnName("Discount_Amount").HasPrecision(18, 2).IsRequired();
        entity.Property(invoice => invoice.TotalAmount).HasColumnName("Total_Amount").HasPrecision(18, 2).IsRequired();
        entity.Property(invoice => invoice.PaymentStatus).HasColumnName("Payment_Status").HasConversion<string>().HasMaxLength(20).IsRequired();
        entity.Property(invoice => invoice.CreditDueDate).HasColumnName("Credit_Due_Date");
        entity.Property(invoice => invoice.StaffId).HasColumnName("Staff_ID").IsRequired();

        entity.HasOne(invoice => invoice.StaffUser)
            .WithMany(user => user.StaffSaleInvoices)
            .HasForeignKey(invoice => invoice.StaffId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureSaleItem(EntityTypeBuilder<SaleItem> entity)
    {
        entity.ToTable("MS_SALE_ITEM");

        entity.HasKey(item => item.SaleItemId);
        entity.Property(item => item.SaleItemId).HasColumnName("Sale_Item_ID");
        entity.Property(item => item.SaleQuantity).HasColumnName("Sale_Quantity").IsRequired();
        entity.Property(item => item.SaleUnitPrice).HasColumnName("Sale_Unit_Price").HasPrecision(18, 2).IsRequired();
        entity.Property(item => item.PartId).HasColumnName("Part_ID").IsRequired();

        entity.HasOne(item => item.Part)
            .WithMany(part => part.SaleItems)
            .HasForeignKey(item => item.PartId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureAppointment(EntityTypeBuilder<Appointment> entity)
    {
        entity.ToTable("MS_APPOINTMENT");

        entity.HasKey(appointment => appointment.AppointmentId);
        entity.Property(appointment => appointment.AppointmentId).HasColumnName("Appointment_ID");
        entity.Property(appointment => appointment.AppointmentDate).HasColumnName("Appointment_Date").IsRequired();
        entity.Property(appointment => appointment.AppointmentStatus).HasColumnName("Appointment_Status").HasConversion<string>().HasMaxLength(30).IsRequired();
        entity.Property(appointment => appointment.ServiceType).HasColumnName("Service_Type").HasMaxLength(100).IsRequired();
        entity.Property(appointment => appointment.AppointmentNotes).HasColumnName("Appointment_Notes").HasMaxLength(500);
    }

    private static void ConfigurePartRequest(EntityTypeBuilder<PartRequest> entity)
    {
        entity.ToTable("MS_PART_REQUEST");

        entity.HasKey(request => request.RequestId);
        entity.Property(request => request.RequestId).HasColumnName("Request_ID");
        entity.Property(request => request.RequestedPartName).HasColumnName("Requested_Part_Name").HasMaxLength(100).IsRequired();
        entity.Property(request => request.RequestDescription).HasColumnName("Request_Description").HasMaxLength(500);
        entity.Property(request => request.RequestDate).HasColumnName("Request_Date").IsRequired();
        entity.Property(request => request.RequestStatus).HasColumnName("Request_Status").HasConversion<string>().HasMaxLength(30).IsRequired();
    }

    private static void ConfigureReview(EntityTypeBuilder<Review> entity)
    {
        entity.ToTable("MS_REVIEW");

        entity.HasKey(review => review.ReviewId);
        entity.Property(review => review.ReviewId).HasColumnName("Review_ID");
        entity.Property(review => review.Rating).HasColumnName("Rating").IsRequired();
        entity.Property(review => review.ReviewText).HasColumnName("Review_Text").HasMaxLength(1000);
        entity.Property(review => review.ReviewDate).HasColumnName("Review_Date").IsRequired();
    }

    private static void ConfigureNotification(EntityTypeBuilder<Notification> entity)
    {
        entity.ToTable("MS_NOTIFICATION");

        entity.HasKey(notification => notification.NotificationId);
        entity.Property(notification => notification.NotificationId).HasColumnName("Notification_ID");
        entity.Property(notification => notification.NotificationType).HasColumnName("Notification_Type").HasConversion<string>().HasMaxLength(30).IsRequired();
        entity.Property(notification => notification.NotificationMessage).HasColumnName("Notification_Message").HasMaxLength(500).IsRequired();
        entity.Property(notification => notification.IsRead).HasColumnName("Is_Read").IsRequired();
        entity.Property(notification => notification.NotificationDate).HasColumnName("Notification_Date").IsRequired();
    }

    private static void ConfigureUserVehicle(EntityTypeBuilder<UserVehicle> entity)
    {
        entity.ToTable("MS_USER_VEHICLE");

        entity.HasKey(link => new { link.UserId, link.VehicleId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.VehicleId).HasColumnName("Vehicle_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserVehicles)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.Vehicle)
            .WithMany(vehicle => vehicle.UserVehicles)
            .HasForeignKey(link => link.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserVendor(EntityTypeBuilder<UserVendor> entity)
    {
        entity.ToTable("MS_USER_VENDOR");

        entity.HasKey(link => new { link.UserId, link.VendorId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.VendorId).HasColumnName("Vendor_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserVendors)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.Vendor)
            .WithMany(vendor => vendor.UserVendors)
            .HasForeignKey(link => link.VendorId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserPart(EntityTypeBuilder<UserPart> entity)
    {
        entity.ToTable("MS_USER_PART");

        entity.HasKey(link => new { link.UserId, link.PartId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.PartId).HasColumnName("Part_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserParts)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.Part)
            .WithMany(part => part.UserParts)
            .HasForeignKey(link => link.PartId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserPurchaseInvoice(EntityTypeBuilder<UserPurchaseInvoice> entity)
    {
        entity.ToTable("MS_USER_PURCHASE_INVOICE");

        entity.HasKey(link => new { link.UserId, link.PurchaseInvoiceId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.PurchaseInvoiceId).HasColumnName("Purchase_Invoice_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserPurchaseInvoices)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.PurchaseInvoice)
            .WithMany(invoice => invoice.UserPurchaseInvoices)
            .HasForeignKey(link => link.PurchaseInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigurePurchaseInvoiceItem(EntityTypeBuilder<PurchaseInvoiceItem> entity)
    {
        entity.ToTable("MS_PURCHASE_INVOICE_ITEM");

        entity.HasKey(link => new { link.PurchaseInvoiceId, link.PurchaseItemId });
        entity.Property(link => link.PurchaseInvoiceId).HasColumnName("Purchase_Invoice_ID");
        entity.Property(link => link.PurchaseItemId).HasColumnName("Purchase_Item_ID");

        entity.HasOne(link => link.PurchaseInvoice)
            .WithMany(invoice => invoice.PurchaseInvoiceItems)
            .HasForeignKey(link => link.PurchaseInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.PurchaseItem)
            .WithMany(item => item.PurchaseInvoiceItems)
            .HasForeignKey(link => link.PurchaseItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserSaleInvoice(EntityTypeBuilder<UserSaleInvoice> entity)
    {
        entity.ToTable("MS_USER_SALE_INVOICE");

        entity.HasKey(link => new { link.UserId, link.SaleInvoiceId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.SaleInvoiceId).HasColumnName("Sale_Invoice_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserSaleInvoices)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.SaleInvoice)
            .WithMany(invoice => invoice.UserSaleInvoices)
            .HasForeignKey(link => link.SaleInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureSaleInvoiceItem(EntityTypeBuilder<SaleInvoiceItem> entity)
    {
        entity.ToTable("MS_SALE_INVOICE_ITEM");

        entity.HasKey(link => new { link.SaleInvoiceId, link.SaleItemId });
        entity.Property(link => link.SaleInvoiceId).HasColumnName("Sale_Invoice_ID");
        entity.Property(link => link.SaleItemId).HasColumnName("Sale_Item_ID");

        entity.HasOne(link => link.SaleInvoice)
            .WithMany(invoice => invoice.SaleInvoiceItems)
            .HasForeignKey(link => link.SaleInvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.SaleItem)
            .WithMany(item => item.SaleInvoiceItems)
            .HasForeignKey(link => link.SaleItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserAppointment(EntityTypeBuilder<UserAppointment> entity)
    {
        entity.ToTable("MS_USER_APPOINTMENT");

        entity.HasKey(link => new { link.UserId, link.AppointmentId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.AppointmentId).HasColumnName("Appointment_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserAppointments)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.Appointment)
            .WithMany(appointment => appointment.UserAppointments)
            .HasForeignKey(link => link.AppointmentId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserPartRequest(EntityTypeBuilder<UserPartRequest> entity)
    {
        entity.ToTable("MS_USER_PART_REQUEST");

        entity.HasKey(link => new { link.UserId, link.RequestId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.RequestId).HasColumnName("Request_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserPartRequests)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.PartRequest)
            .WithMany(request => request.UserPartRequests)
            .HasForeignKey(link => link.RequestId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserReview(EntityTypeBuilder<UserReview> entity)
    {
        entity.ToTable("MS_USER_REVIEW");

        entity.HasKey(link => new { link.UserId, link.ReviewId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.ReviewId).HasColumnName("Review_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserReviews)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.Review)
            .WithMany(review => review.UserReviews)
            .HasForeignKey(link => link.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private static void ConfigureUserNotification(EntityTypeBuilder<UserNotification> entity)
    {
        entity.ToTable("MS_USER_NOTIFICATION");

        entity.HasKey(link => new { link.UserId, link.NotificationId });
        entity.Property(link => link.UserId).HasColumnName("User_ID");
        entity.Property(link => link.NotificationId).HasColumnName("Notification_ID");

        entity.HasOne(link => link.User)
            .WithMany(user => user.UserNotifications)
            .HasForeignKey(link => link.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(link => link.Notification)
            .WithMany(notification => notification.UserNotifications)
            .HasForeignKey(link => link.NotificationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
