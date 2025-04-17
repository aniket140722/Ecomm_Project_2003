using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm_Project_2003.DataAccess.Migrations
{
    public partial class AddSPCoverTypeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CREATE
            migrationBuilder.Sql(@"CREATE PROCEDURE CreateCoverType
	                              @name varchar(50)
                                  AS
	                              insert CoverTypes values(@name)");
            // UPDATE
            migrationBuilder.Sql(@"CREATE PROCEDURE UpdateCoverType
                                  @id int,  
	                              @name varchar(50)
                                  AS
	                              update CoverTypes set name=@name where id=@id");
            // DELETE
            migrationBuilder.Sql(@"CREATE PROCEDURE DeleteCoverType
                                  @id int
                                  AS
	                              delete from CoverTypes where id=@id");
            // GET
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCoverTypes
                                  AS
	                              select *  from CoverTypes");
            // FIND
            migrationBuilder.Sql(@"CREATE PROCEDURE GetCoverType
                                   @id int
                                  AS
	                              select *  from CoverTypes where id=@id");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
