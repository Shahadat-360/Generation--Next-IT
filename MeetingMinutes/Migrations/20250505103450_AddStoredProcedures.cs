using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingMinutes.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop procedures if they exist
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Meeting_Minutes_Master_Save_SP') DROP PROCEDURE Meeting_Minutes_Master_Save_SP");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Meeting_Minutes_Details_Save_SP') DROP PROCEDURE Meeting_Minutes_Details_Save_SP");

            // Create Master procedure
            migrationBuilder.Sql(@"
CREATE PROCEDURE Meeting_Minutes_Master_Save_SP
    @Id INT OUTPUT,
    @CustomerType NVARCHAR(20),
    @CustomerId INT,
    @CustomerName NVARCHAR(200),
    @Date DATE,
    @Time TIME,
    @MeetingPlace NVARCHAR(200),
    @AttendsFromClientSide NVARCHAR(500),
    @AttendsFromHostSide NVARCHAR(500),
    @MeetingAgenda NVARCHAR(MAX),
    @MeetingDiscussion NVARCHAR(MAX),
    @MeetingDecision NVARCHAR(MAX)
AS
BEGIN
    IF @Id = 0
    BEGIN
        -- Insert new record
        INSERT INTO Meeting_Minutes_Master_Tbl (
            CustomerType, CustomerId, CustomerName, Date, Time, MeetingPlace,
            AttendsFromClientSide, AttendsFromHostSide, MeetingAgenda, MeetingDiscussion, MeetingDecision
        )
        VALUES (
            @CustomerType, @CustomerId, @CustomerName, @Date, @Time, @MeetingPlace,
            @AttendsFromClientSide, @AttendsFromHostSide, @MeetingAgenda, @MeetingDiscussion, @MeetingDecision
        );
        
        SET @Id = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        -- Update existing record
        UPDATE Meeting_Minutes_Master_Tbl
        SET CustomerType = @CustomerType,
            CustomerId = @CustomerId,
            CustomerName = @CustomerName,
            Date = @Date,
            Time = @Time,
            MeetingPlace = @MeetingPlace,
            AttendsFromClientSide = @AttendsFromClientSide,
            AttendsFromHostSide = @AttendsFromHostSide,
            MeetingAgenda = @MeetingAgenda,
            MeetingDiscussion = @MeetingDiscussion,
            MeetingDecision = @MeetingDecision
        WHERE Id = @Id;
    END
END");

            // Create Detail procedure
            migrationBuilder.Sql(@"
CREATE PROCEDURE Meeting_Minutes_Details_Save_SP
    @Id INT OUTPUT,
    @MeetingMinutesMasterId INT,
    @ProductServiceId INT,
    @ProductServiceName NVARCHAR(200),
    @Quantity DECIMAL(18, 2),
    @Unit NVARCHAR(50)
AS
BEGIN
    IF @Id = 0
    BEGIN
        -- Insert new record
        INSERT INTO Meeting_Minutes_Details_Tbl (
            MeetingMinutesMasterId, ProductServiceId, ProductServiceName, Quantity, Unit
        )
        VALUES (
            @MeetingMinutesMasterId, @ProductServiceId, @ProductServiceName, @Quantity, @Unit
        );
        
        SET @Id = SCOPE_IDENTITY();
    END
    ELSE
    BEGIN
        -- Update existing record
        UPDATE Meeting_Minutes_Details_Tbl
        SET MeetingMinutesMasterId = @MeetingMinutesMasterId,
            ProductServiceId = @ProductServiceId,
            ProductServiceName = @ProductServiceName,
            Quantity = @Quantity,
            Unit = @Unit
        WHERE Id = @Id;
    END
END");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop stored procedures when migration is rolled back
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Meeting_Minutes_Master_Save_SP') DROP PROCEDURE Meeting_Minutes_Master_Save_SP");
            migrationBuilder.Sql("IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'Meeting_Minutes_Details_Save_SP') DROP PROCEDURE Meeting_Minutes_Details_Save_SP");
        }
    }
}
