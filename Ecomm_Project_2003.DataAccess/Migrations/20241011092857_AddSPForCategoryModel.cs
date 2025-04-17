using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecomm_Project_2003.DataAccess.Migrations
{
    public partial class AddSPForCategoryModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE PROCEDURE CreateCategory
	                               @name varchar(50)
                                   AS
	                              insert Category values (@name)");
            migrationBuilder.Sql(@"CREATE PROCEDURE UpdateCategory
                                  @id int,
	                               @name varchar(50)
                                   AS
	                              Update Category set name=@name where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE DeleteCategory
                                  @id int
                                   AS
	                              Delete from Categories  where id=@id");
            migrationBuilder.Sql(@"CREATE PROCEDURE GETCategories
                                   AS
	                              select * from Categories");
            migrationBuilder.Sql(@"CREATE PROCEDURE GETCategory
                                   @id int
                                   AS
	                              select * from Categories where id=@id");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
