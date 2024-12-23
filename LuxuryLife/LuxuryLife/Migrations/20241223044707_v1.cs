using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LuxuryLife.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admin",
                columns: table => new
                {
                    AdminId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Admin__719FE488CCBBC051", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Dob = table.Column<DateOnly>(type: "date", nullable: true),
                    Demographics = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Preferences = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SearchHistory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Createdate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__A4AE64D8CED30739", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    NewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Createdate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__News__7CC3777EEB9A1D67", x => x.NewId);
                });

            migrationBuilder.CreateTable(
                name: "Provider",
                columns: table => new
                {
                    ProviderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: true),
                    Createdate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Provider__B54C687D284498A8", x => x.ProviderId);
                });

            migrationBuilder.CreateTable(
                name: "Tour",
                columns: table => new
                {
                    TourId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    HomestayId = table.Column<int>(type: "int", nullable: true),
                    PricePerson = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Createdate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    ProviderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tour__604CEA3002475256", x => x.TourId);
                    table.ForeignKey(
                        name: "FK__Tour__ProviderId__4316F928",
                        column: x => x.ProviderId,
                        principalTable: "Provider",
                        principalColumn: "ProviderId");
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                columns: table => new
                {
                    BookingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    TourId = table.Column<int>(type: "int", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    CheckInDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CheckOutDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Booking__73951AED39332FF9", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK__Booking__Custome__5812160E",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK__Booking__TourId__59063A47",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId");
                });

            migrationBuilder.CreateTable(
                name: "Favourite",
                columns: table => new
                {
                    FavoriteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    TourId = table.Column<int>(type: "int", nullable: true),
                    Sumprice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Favourit__CE74FAD5B2B8E927", x => x.FavoriteId);
                    table.ForeignKey(
                        name: "FK__Favourite__Custo__4E88ABD4",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK__Favourite__TourI__4F7CD00D",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId");
                });

            migrationBuilder.CreateTable(
                name: "Homestay",
                columns: table => new
                {
                    HomestayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProviderId = table.Column<int>(type: "int", nullable: true),
                    TourId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Homestay__EDCB5CBABC26500E", x => x.HomestayId);
                    table.ForeignKey(
                        name: "FK_Homestay_Tour_TourId",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId");
                });

            migrationBuilder.CreateTable(
                name: "Listimagestour",
                columns: table => new
                {
                    ListimagestourId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TourId = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ImageDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Createdate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Listimag__C13099DB06DC99D8", x => x.ListimagestourId);
                    table.ForeignKey(
                        name: "FK__Listimage__TourI__46E78A0C",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId");
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: true),
                    TourId = table.Column<int>(type: "int", nullable: true),
                    Rating = table.Column<decimal>(type: "decimal(3,2)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Createdate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Review__74BC79CE402D27CA", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK__Review__Customer__534D60F1",
                        column: x => x.CustomerId,
                        principalTable: "Customer",
                        principalColumn: "CustomerId");
                    table.ForeignKey(
                        name: "FK__Review__TourId__5441852A",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TourId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service__C51BB00AB64FBE5D", x => x.ServiceId);
                    table.ForeignKey(
                        name: "FK__Service__TourId__4BAC3F29",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "TourId");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__Admin__A9D10534B027E995",
                table: "Admin",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Booking_CustomerId",
                table: "Booking",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_TourId",
                table: "Booking",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__A9D105343E5F5B4C",
                table: "Customer",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favourite_CustomerId",
                table: "Favourite",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Favourite_TourId",
                table: "Favourite",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Homestay_TourId",
                table: "Homestay",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Listimagestour_TourId",
                table: "Listimagestour",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "UQ__Provider__A9D10534A5ED99BC",
                table: "Provider",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_CustomerId",
                table: "Review",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_TourId",
                table: "Review",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Service_TourId",
                table: "Service",
                column: "TourId");

            migrationBuilder.CreateIndex(
                name: "IX_Tour_ProviderId",
                table: "Tour",
                column: "ProviderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admin");

            migrationBuilder.DropTable(
                name: "Booking");

            migrationBuilder.DropTable(
                name: "Favourite");

            migrationBuilder.DropTable(
                name: "Homestay");

            migrationBuilder.DropTable(
                name: "Listimagestour");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Customer");

            migrationBuilder.DropTable(
                name: "Tour");

            migrationBuilder.DropTable(
                name: "Provider");
        }
    }
}
