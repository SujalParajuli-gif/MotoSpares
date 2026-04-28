using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MotoSpares.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NormalizedSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MS_APPOINTMENT",
                columns: table => new
                {
                    Appointment_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Appointment_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Appointment_Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Service_Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Appointment_Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_APPOINTMENT", x => x.Appointment_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_NOTIFICATION",
                columns: table => new
                {
                    Notification_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Notification_Type = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Notification_Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Is_Read = table.Column<bool>(type: "boolean", nullable: false),
                    Notification_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_NOTIFICATION", x => x.Notification_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_PART",
                columns: table => new
                {
                    Part_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Part_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Part_Number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Unit_Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Stock_Quantity = table.Column<int>(type: "integer", nullable: false),
                    Reorder_Level = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_PART", x => x.Part_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_PART_REQUEST",
                columns: table => new
                {
                    Request_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Requested_Part_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Request_Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Request_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Request_Status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_PART_REQUEST", x => x.Request_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_REVIEW",
                columns: table => new
                {
                    Review_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Review_Text = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Review_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_REVIEW", x => x.Review_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_ROLE",
                columns: table => new
                {
                    Role_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Role_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Normalized_Role_Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Concurrency_Stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_ROLE", x => x.Role_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Full_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Role = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Created_At = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    User_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Normalized_User_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Normalized_Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email_Confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    Password_Hash = table.Column<string>(type: "text", nullable: true),
                    Security_Stamp = table.Column<string>(type: "text", nullable: true),
                    Concurrency_Stamp = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Phone_Confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    Two_Factor_Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Lockout_End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    Lockout_Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Access_Failed_Count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER", x => x.User_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_VEHICLE",
                columns: table => new
                {
                    Vehicle_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Vehicle_Number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Make = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_VEHICLE", x => x.Vehicle_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_VENDOR",
                columns: table => new
                {
                    Vendor_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Vendor_Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Vendor_Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Vendor_Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Vendor_Address = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_VENDOR", x => x.Vendor_ID);
                });

            migrationBuilder.CreateTable(
                name: "MS_PURCHASE_ITEM",
                columns: table => new
                {
                    Purchase_Item_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Purchase_Quantity = table.Column<int>(type: "integer", nullable: false),
                    Purchase_Unit_Cost = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Part_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_PURCHASE_ITEM", x => x.Purchase_Item_ID);
                    table.ForeignKey(
                        name: "FK_MS_PURCHASE_ITEM_MS_PART_Part_ID",
                        column: x => x.Part_ID,
                        principalTable: "MS_PART",
                        principalColumn: "Part_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MS_SALE_ITEM",
                columns: table => new
                {
                    Sale_Item_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sale_Quantity = table.Column<int>(type: "integer", nullable: false),
                    Sale_Unit_Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Part_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_SALE_ITEM", x => x.Sale_Item_ID);
                    table.ForeignKey(
                        name: "FK_MS_SALE_ITEM_MS_PART_Part_ID",
                        column: x => x.Part_ID,
                        principalTable: "MS_PART",
                        principalColumn: "Part_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MS_ROLE_CLAIM",
                columns: table => new
                {
                    Role_Claim_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Role_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Claim_Type = table.Column<string>(type: "text", nullable: true),
                    Claim_Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_ROLE_CLAIM", x => x.Role_Claim_ID);
                    table.ForeignKey(
                        name: "FK_MS_ROLE_CLAIM_MS_ROLE_Role_ID",
                        column: x => x.Role_ID,
                        principalTable: "MS_ROLE",
                        principalColumn: "Role_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_SALE_INVOICE",
                columns: table => new
                {
                    Sale_Invoice_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Sale_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Subtotal = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Discount_Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Total_Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Payment_Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Credit_Due_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Staff_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_SALE_INVOICE", x => x.Sale_Invoice_ID);
                    table.ForeignKey(
                        name: "FK_MS_SALE_INVOICE_MS_USER_Staff_ID",
                        column: x => x.Staff_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_APPOINTMENT",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Appointment_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_APPOINTMENT", x => new { x.User_ID, x.Appointment_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_APPOINTMENT_MS_APPOINTMENT_Appointment_ID",
                        column: x => x.Appointment_ID,
                        principalTable: "MS_APPOINTMENT",
                        principalColumn: "Appointment_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_APPOINTMENT_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_CLAIM",
                columns: table => new
                {
                    User_Claim_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Claim_Type = table.Column<string>(type: "text", nullable: true),
                    Claim_Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_CLAIM", x => x.User_Claim_ID);
                    table.ForeignKey(
                        name: "FK_MS_USER_CLAIM_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_LOGIN",
                columns: table => new
                {
                    Login_Provider = table.Column<string>(type: "text", nullable: false),
                    Provider_Key = table.Column<string>(type: "text", nullable: false),
                    Provider_Display_Name = table.Column<string>(type: "text", nullable: true),
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_LOGIN", x => new { x.Login_Provider, x.Provider_Key });
                    table.ForeignKey(
                        name: "FK_MS_USER_LOGIN_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_NOTIFICATION",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Notification_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_NOTIFICATION", x => new { x.User_ID, x.Notification_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_NOTIFICATION_MS_NOTIFICATION_Notification_ID",
                        column: x => x.Notification_ID,
                        principalTable: "MS_NOTIFICATION",
                        principalColumn: "Notification_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_NOTIFICATION_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_PART",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Part_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_PART", x => new { x.User_ID, x.Part_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_PART_MS_PART_Part_ID",
                        column: x => x.Part_ID,
                        principalTable: "MS_PART",
                        principalColumn: "Part_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_PART_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_PART_REQUEST",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Request_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_PART_REQUEST", x => new { x.User_ID, x.Request_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_PART_REQUEST_MS_PART_REQUEST_Request_ID",
                        column: x => x.Request_ID,
                        principalTable: "MS_PART_REQUEST",
                        principalColumn: "Request_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_PART_REQUEST_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_REVIEW",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Review_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_REVIEW", x => new { x.User_ID, x.Review_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_REVIEW_MS_REVIEW_Review_ID",
                        column: x => x.Review_ID,
                        principalTable: "MS_REVIEW",
                        principalColumn: "Review_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_REVIEW_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_ROLE",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Role_ID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_ROLE", x => new { x.User_ID, x.Role_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_ROLE_MS_ROLE_Role_ID",
                        column: x => x.Role_ID,
                        principalTable: "MS_ROLE",
                        principalColumn: "Role_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_ROLE_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_TOKEN",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Login_Provider = table.Column<string>(type: "text", nullable: false),
                    Token_Name = table.Column<string>(type: "text", nullable: false),
                    Token_Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_TOKEN", x => new { x.User_ID, x.Login_Provider, x.Token_Name });
                    table.ForeignKey(
                        name: "FK_MS_USER_TOKEN_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_VEHICLE",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Vehicle_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_VEHICLE", x => new { x.User_ID, x.Vehicle_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_VEHICLE_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_VEHICLE_MS_VEHICLE_Vehicle_ID",
                        column: x => x.Vehicle_ID,
                        principalTable: "MS_VEHICLE",
                        principalColumn: "Vehicle_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_PURCHASE_INVOICE",
                columns: table => new
                {
                    Purchase_Invoice_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Purchase_Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Purchase_Total = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Vendor_ID = table.Column<int>(type: "integer", nullable: false),
                    Created_By = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_PURCHASE_INVOICE", x => x.Purchase_Invoice_ID);
                    table.ForeignKey(
                        name: "FK_MS_PURCHASE_INVOICE_MS_USER_Created_By",
                        column: x => x.Created_By,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MS_PURCHASE_INVOICE_MS_VENDOR_Vendor_ID",
                        column: x => x.Vendor_ID,
                        principalTable: "MS_VENDOR",
                        principalColumn: "Vendor_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_VENDOR",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Vendor_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_VENDOR", x => new { x.User_ID, x.Vendor_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_VENDOR_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_VENDOR_MS_VENDOR_Vendor_ID",
                        column: x => x.Vendor_ID,
                        principalTable: "MS_VENDOR",
                        principalColumn: "Vendor_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_SALE_INVOICE_ITEM",
                columns: table => new
                {
                    Sale_Invoice_ID = table.Column<int>(type: "integer", nullable: false),
                    Sale_Item_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_SALE_INVOICE_ITEM", x => new { x.Sale_Invoice_ID, x.Sale_Item_ID });
                    table.ForeignKey(
                        name: "FK_MS_SALE_INVOICE_ITEM_MS_SALE_INVOICE_Sale_Invoice_ID",
                        column: x => x.Sale_Invoice_ID,
                        principalTable: "MS_SALE_INVOICE",
                        principalColumn: "Sale_Invoice_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_SALE_INVOICE_ITEM_MS_SALE_ITEM_Sale_Item_ID",
                        column: x => x.Sale_Item_ID,
                        principalTable: "MS_SALE_ITEM",
                        principalColumn: "Sale_Item_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_SALE_INVOICE",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Sale_Invoice_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_SALE_INVOICE", x => new { x.User_ID, x.Sale_Invoice_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_SALE_INVOICE_MS_SALE_INVOICE_Sale_Invoice_ID",
                        column: x => x.Sale_Invoice_ID,
                        principalTable: "MS_SALE_INVOICE",
                        principalColumn: "Sale_Invoice_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_SALE_INVOICE_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_PURCHASE_INVOICE_ITEM",
                columns: table => new
                {
                    Purchase_Invoice_ID = table.Column<int>(type: "integer", nullable: false),
                    Purchase_Item_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_PURCHASE_INVOICE_ITEM", x => new { x.Purchase_Invoice_ID, x.Purchase_Item_ID });
                    table.ForeignKey(
                        name: "FK_MS_PURCHASE_INVOICE_ITEM_MS_PURCHASE_INVOICE_Purchase_Invoi~",
                        column: x => x.Purchase_Invoice_ID,
                        principalTable: "MS_PURCHASE_INVOICE",
                        principalColumn: "Purchase_Invoice_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_PURCHASE_INVOICE_ITEM_MS_PURCHASE_ITEM_Purchase_Item_ID",
                        column: x => x.Purchase_Item_ID,
                        principalTable: "MS_PURCHASE_ITEM",
                        principalColumn: "Purchase_Item_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MS_USER_PURCHASE_INVOICE",
                columns: table => new
                {
                    User_ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Purchase_Invoice_ID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MS_USER_PURCHASE_INVOICE", x => new { x.User_ID, x.Purchase_Invoice_ID });
                    table.ForeignKey(
                        name: "FK_MS_USER_PURCHASE_INVOICE_MS_PURCHASE_INVOICE_Purchase_Invoi~",
                        column: x => x.Purchase_Invoice_ID,
                        principalTable: "MS_PURCHASE_INVOICE",
                        principalColumn: "Purchase_Invoice_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MS_USER_PURCHASE_INVOICE_MS_USER_User_ID",
                        column: x => x.User_ID,
                        principalTable: "MS_USER",
                        principalColumn: "User_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MS_PART_Part_Number",
                table: "MS_PART",
                column: "Part_Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MS_PURCHASE_INVOICE_Created_By",
                table: "MS_PURCHASE_INVOICE",
                column: "Created_By");

            migrationBuilder.CreateIndex(
                name: "IX_MS_PURCHASE_INVOICE_Vendor_ID",
                table: "MS_PURCHASE_INVOICE",
                column: "Vendor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_PURCHASE_INVOICE_ITEM_Purchase_Item_ID",
                table: "MS_PURCHASE_INVOICE_ITEM",
                column: "Purchase_Item_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_PURCHASE_ITEM_Part_ID",
                table: "MS_PURCHASE_ITEM",
                column: "Part_ID");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "MS_ROLE",
                column: "Normalized_Role_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MS_ROLE_CLAIM_Role_ID",
                table: "MS_ROLE_CLAIM",
                column: "Role_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_SALE_INVOICE_Staff_ID",
                table: "MS_SALE_INVOICE",
                column: "Staff_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_SALE_INVOICE_ITEM_Sale_Item_ID",
                table: "MS_SALE_INVOICE_ITEM",
                column: "Sale_Item_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_SALE_ITEM_Part_ID",
                table: "MS_SALE_ITEM",
                column: "Part_ID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "MS_USER",
                column: "Normalized_Email");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_Email",
                table: "MS_USER",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "MS_USER",
                column: "Normalized_User_Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_APPOINTMENT_Appointment_ID",
                table: "MS_USER_APPOINTMENT",
                column: "Appointment_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_CLAIM_User_ID",
                table: "MS_USER_CLAIM",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_LOGIN_User_ID",
                table: "MS_USER_LOGIN",
                column: "User_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_NOTIFICATION_Notification_ID",
                table: "MS_USER_NOTIFICATION",
                column: "Notification_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_PART_Part_ID",
                table: "MS_USER_PART",
                column: "Part_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_PART_REQUEST_Request_ID",
                table: "MS_USER_PART_REQUEST",
                column: "Request_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_PURCHASE_INVOICE_Purchase_Invoice_ID",
                table: "MS_USER_PURCHASE_INVOICE",
                column: "Purchase_Invoice_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_REVIEW_Review_ID",
                table: "MS_USER_REVIEW",
                column: "Review_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_ROLE_Role_ID",
                table: "MS_USER_ROLE",
                column: "Role_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_SALE_INVOICE_Sale_Invoice_ID",
                table: "MS_USER_SALE_INVOICE",
                column: "Sale_Invoice_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_VEHICLE_Vehicle_ID",
                table: "MS_USER_VEHICLE",
                column: "Vehicle_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_USER_VENDOR_Vendor_ID",
                table: "MS_USER_VENDOR",
                column: "Vendor_ID");

            migrationBuilder.CreateIndex(
                name: "IX_MS_VEHICLE_Vehicle_Number",
                table: "MS_VEHICLE",
                column: "Vehicle_Number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MS_PURCHASE_INVOICE_ITEM");

            migrationBuilder.DropTable(
                name: "MS_ROLE_CLAIM");

            migrationBuilder.DropTable(
                name: "MS_SALE_INVOICE_ITEM");

            migrationBuilder.DropTable(
                name: "MS_USER_APPOINTMENT");

            migrationBuilder.DropTable(
                name: "MS_USER_CLAIM");

            migrationBuilder.DropTable(
                name: "MS_USER_LOGIN");

            migrationBuilder.DropTable(
                name: "MS_USER_NOTIFICATION");

            migrationBuilder.DropTable(
                name: "MS_USER_PART");

            migrationBuilder.DropTable(
                name: "MS_USER_PART_REQUEST");

            migrationBuilder.DropTable(
                name: "MS_USER_PURCHASE_INVOICE");

            migrationBuilder.DropTable(
                name: "MS_USER_REVIEW");

            migrationBuilder.DropTable(
                name: "MS_USER_ROLE");

            migrationBuilder.DropTable(
                name: "MS_USER_SALE_INVOICE");

            migrationBuilder.DropTable(
                name: "MS_USER_TOKEN");

            migrationBuilder.DropTable(
                name: "MS_USER_VEHICLE");

            migrationBuilder.DropTable(
                name: "MS_USER_VENDOR");

            migrationBuilder.DropTable(
                name: "MS_PURCHASE_ITEM");

            migrationBuilder.DropTable(
                name: "MS_SALE_ITEM");

            migrationBuilder.DropTable(
                name: "MS_APPOINTMENT");

            migrationBuilder.DropTable(
                name: "MS_NOTIFICATION");

            migrationBuilder.DropTable(
                name: "MS_PART_REQUEST");

            migrationBuilder.DropTable(
                name: "MS_PURCHASE_INVOICE");

            migrationBuilder.DropTable(
                name: "MS_REVIEW");

            migrationBuilder.DropTable(
                name: "MS_ROLE");

            migrationBuilder.DropTable(
                name: "MS_SALE_INVOICE");

            migrationBuilder.DropTable(
                name: "MS_VEHICLE");

            migrationBuilder.DropTable(
                name: "MS_PART");

            migrationBuilder.DropTable(
                name: "MS_VENDOR");

            migrationBuilder.DropTable(
                name: "MS_USER");
        }
    }
}
