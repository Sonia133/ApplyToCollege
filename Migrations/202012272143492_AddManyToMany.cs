namespace College.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddManyToMany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "Faculty_FacultyId", "dbo.Faculties");
            DropIndex("dbo.Students", new[] { "Faculty_FacultyId" });
            CreateTable(
                "dbo.StudentFaculties",
                c => new
                    {
                        Student_StudentId = c.Int(nullable: false),
                        Faculty_FacultyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Student_StudentId, t.Faculty_FacultyId })
                .ForeignKey("dbo.Students", t => t.Student_StudentId, cascadeDelete: true)
                .ForeignKey("dbo.Faculties", t => t.Faculty_FacultyId, cascadeDelete: true)
                .Index(t => t.Student_StudentId)
                .Index(t => t.Faculty_FacultyId);
            
            DropColumn("dbo.Students", "Faculty_FacultyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Students", "Faculty_FacultyId", c => c.Int());
            DropForeignKey("dbo.StudentFaculties", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.StudentFaculties", "Student_StudentId", "dbo.Students");
            DropIndex("dbo.StudentFaculties", new[] { "Faculty_FacultyId" });
            DropIndex("dbo.StudentFaculties", new[] { "Student_StudentId" });
            DropTable("dbo.StudentFaculties");
            CreateIndex("dbo.Students", "Faculty_FacultyId");
            AddForeignKey("dbo.Students", "Faculty_FacultyId", "dbo.Faculties", "faculty_id");
        }
    }
}
