using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MegwayParcel.Common.Migrations
{
    /// <inheritdoc />
    public partial class _Mig_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Access = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    BlogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Prefix = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortDetail = table.Column<string>(type: "varchar(1000)", unicode: false, maxLength: 1000, nullable: true),
                    AuthorName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    FullDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "CompanySetting",
                columns: table => new
                {
                    CompanySettingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    InsuranceCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CollectionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingPercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SurchargePercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemoteSurchargePercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VATChargePercent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SettingName = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySetting", x => x.CompanySettingId);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    ContactUsId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ContactU__E10B7AC8CD6FB5BD", x => x.ContactUsId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsEU = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "GuestUser",
                columns: table => new
                {
                    GuestUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    EmailCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    EmailExpiry = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestUser", x => x.GuestUserId);
                });

            migrationBuilder.CreateTable(
                name: "LandMarkCountries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandMarkCountries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    MenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ControllerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ActionName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.MenuId);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CollectionDate = table.Column<DateTime>(type: "date", nullable: true),
                    CollectionTime = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    RequestQuote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectQuote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAddCart = table.Column<bool>(type: "bit", nullable: true),
                    IsRemoveFromBasket = table.Column<bool>(type: "bit", nullable: true),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    CollectionOptionId = table.Column<int>(type: "int", nullable: true),
                    OrderResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsuranceValue = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InsuranceCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CollectionCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ShippingCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Surcharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemoteSurcharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VATCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ErrorResponse = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    RefrenceNo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IsPaymentReceived = table.Column<bool>(type: "bit", nullable: false),
                    PaymentRefrenceNo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    OrderStatusId = table.Column<int>(type: "int", nullable: false),
                    IsSameCountry = table.Column<bool>(type: "bit", nullable: false),
                    PaymentResponse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SelectedCompanyLogo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SignaturePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsCollectionEmailSent = table.Column<bool>(type: "bit", nullable: true),
                    CollectionEmailError = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IsDeliveryEmailSent = table.Column<bool>(type: "bit", nullable: true),
                    DeliveryEmailError = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IsInvoiceEmailSent = table.Column<bool>(type: "bit", nullable: true),
                    InvoiceEmailError = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    AirWaybillReference = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    ResidentialSurcharge = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    OrderEmail = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    ServiceCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    PageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Prefix = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Detail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.PageId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "ServicesName",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service_Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesName", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteMap",
                columns: table => new
                {
                    SiteMapId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    Url = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true),
                    SortOrder = table.Column<int>(type: "int", nullable: true),
                    Detail = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteMap", x => x.SiteMapId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AcountName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Title = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ForeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SurName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AddTitle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    AddForeName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddSurName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddLine1 = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    AddLine2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Postcode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsResidentialAddress = table.Column<bool>(type: "bit", nullable: false),
                    IsVatRegistered = table.Column<bool>(type: "bit", nullable: false),
                    MyVatNumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    EORINumber = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsShippingOrder = table.Column<bool>(type: "bit", nullable: false),
                    IsNewsOffers = table.Column<bool>(type: "bit", nullable: false),
                    CompanySettingId = table.Column<int>(type: "int", nullable: true),
                    EmailCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EmailExpiry = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsEmailConfirmed = table.Column<bool>(type: "bit", nullable: true),
                    IsWithoutPayment = table.Column<bool>(type: "bit", nullable: false),
                    DefForename = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DefSurname = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DefEmailAddress = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DefTelephoneNumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DefMobileNumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DefCompanyName = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    DefAddressLineOne = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    DefAddressLineTwo = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    DefCity = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DefCounty = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DefPostcode = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    DefTitle = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IossNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.CustomerId);
                    table.ForeignKey(
                        name: "FK_Customer_CompanySetting",
                        column: x => x.CompanySettingId,
                        principalTable: "CompanySetting",
                        principalColumn: "CompanySettingId");
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    AddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: true),
                    Forename = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Surname = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    EmailAddress = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TelephoneNumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    MobileNumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CompanyName = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    AddressLineOne = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    AddressLineTwo = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    County = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Postcode = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IsAddressResidential = table.Column<bool>(type: "bit", nullable: false),
                    EORINumber = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.AddressId);
                    table.ForeignKey(
                        name: "FK_Address_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "ConsignmentSummary",
                columns: table => new
                {
                    ConsignmentSummaryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    IsCustomsInvoice = table.Column<bool>(type: "bit", nullable: false),
                    ContentSummary = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ReasonForShipment = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TotalGoodsValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShippingCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalValueForCustoms = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DocumentUrl = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DocumentCategory = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsignmentSummary", x => x.ConsignmentSummaryId);
                    table.ForeignKey(
                        name: "FK_ConsignmentSummary_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "CustomerInvoice",
                columns: table => new
                {
                    CustomerInvoiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    CommodityCode = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    ProductDescription = table.Column<string>(type: "varchar(250)", unicode: false, maxLength: 250, nullable: true),
                    ValueForCustoms = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalGoodsValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShippingCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalValueForCustoms = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsAgree = table.Column<bool>(type: "bit", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    NoOfItems = table.Column<int>(type: "int", nullable: true),
                    PerItemValue = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PerItemWeight = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInvoice", x => x.CustomerInvoiceId);
                    table.ForeignKey(
                        name: "FK_CustomerInvoice_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "DeliveryDetail",
                columns: table => new
                {
                    DeliveryDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    IsResidentialAddress = table.Column<bool>(type: "bit", nullable: false),
                    IsSignatureRequired = table.Column<bool>(type: "bit", nullable: false),
                    ReceiverTaxId = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryDetail", x => x.DeliveryDetailId);
                    table.ForeignKey(
                        name: "FK_DeliveryDetail_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "ExporterDetail",
                columns: table => new
                {
                    ExporterDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    IsCommercialShipmet = table.Column<bool>(type: "bit", nullable: false),
                    IsExporterAddressAsCollectionAddress = table.Column<bool>(type: "bit", nullable: false),
                    ExporterEORINo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ExporterVATNo = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    IossNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExporterDetail", x => x.ExporterDetailId);
                    table.ForeignKey(
                        name: "FK_ExporterDetail_Order",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "RoleMenu",
                columns: table => new
                {
                    RoleMenuId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleMenu", x => x.RoleMenuId);
                    table.ForeignKey(
                        name: "FK_RollMenu_Menu",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "MenuId");
                    table.ForeignKey(
                        name: "FK_RollMenu_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Role",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "RoleId");
                });

            migrationBuilder.CreateTable(
                name: "LandMarkPrices",
                columns: table => new
                {
                    PriceId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    LandMarkCountriesCountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LandMarkPrices", x => x.PriceId);
                    table.ForeignKey(
                        name: "FK_LandMarkPrices_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LandMarkPrices_LandMarkCountries_LandMarkCountriesCountryId",
                        column: x => x.LandMarkCountriesCountryId,
                        principalTable: "LandMarkCountries",
                        principalColumn: "CountryId");
                    table.ForeignKey(
                        name: "FK_LandMarkPrices_ServicesName_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "ServicesName",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_OrderId",
                table: "Address",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ConsignmentSummary_OrderId",
                table: "ConsignmentSummary",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CompanySettingId",
                table: "Customer",
                column: "CompanySettingId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInvoice_OrderId",
                table: "CustomerInvoice",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryDetail_OrderId",
                table: "DeliveryDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExporterDetail_OrderId",
                table: "ExporterDetail",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_LandMarkPrices_CountryId",
                table: "LandMarkPrices",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LandMarkPrices_LandMarkCountriesCountryId",
                table: "LandMarkPrices",
                column: "LandMarkCountriesCountryId");

            migrationBuilder.CreateIndex(
                name: "IX_LandMarkPrices_ServiceId",
                table: "LandMarkPrices",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenu_MenuId",
                table: "RoleMenu",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenu_RoleId",
                table: "RoleMenu",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "ConsignmentSummary");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "CustomerInvoice");

            migrationBuilder.DropTable(
                name: "DeliveryDetail");

            migrationBuilder.DropTable(
                name: "ExporterDetail");

            migrationBuilder.DropTable(
                name: "GuestUser");

            migrationBuilder.DropTable(
                name: "LandMarkPrices");

            migrationBuilder.DropTable(
                name: "Page");

            migrationBuilder.DropTable(
                name: "RoleMenu");

            migrationBuilder.DropTable(
                name: "SiteMap");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CompanySetting");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "LandMarkCountries");

            migrationBuilder.DropTable(
                name: "ServicesName");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
