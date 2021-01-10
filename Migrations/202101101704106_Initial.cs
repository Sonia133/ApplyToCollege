namespace College.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deans",
                c => new
                    {
                        DeanId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.DeanId)
                .ForeignKey("dbo.Faculties", t => t.DeanId)
                .Index(t => t.DeanId);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        FacultyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        City = c.String(nullable: false),
                        Places = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.FacultyId);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        exam_id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false, maxLength: 20),
                        Date = c.DateTime(nullable: false),
                        Type = c.String(nullable: false),
                        Faculty_FacultyId = c.Int(),
                    })
                .PrimaryKey(t => t.exam_id)
                .ForeignKey("dbo.Faculties", t => t.Faculty_FacultyId)
                .Index(t => t.Faculty_FacultyId);
            
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        student_id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Cnp = c.Double(nullable: false),
                        Frequency = c.String(nullable: false),
                        Email = c.String(),
                        Sat = c.Double(nullable: false),
                        Badge = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.student_id);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        teacher_id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Subject = c.String(nullable: false, maxLength: 20),
                        Email = c.String(),
                        Faculty_FacultyId = c.Int(),
                    })
                .PrimaryKey(t => t.teacher_id)
                .ForeignKey("dbo.Faculties", t => t.Faculty_FacultyId)
                .Index(t => t.Faculty_FacultyId);
            
           
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Deans", "DeanId", "dbo.Faculties");
            DropForeignKey("dbo.Teachers", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.StudentFaculties", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.StudentFaculties", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.Exams", "Faculty_FacultyId", "dbo.Faculties");
            DropIndex("dbo.StudentFaculties", new[] { "Faculty_FacultyId" });
            DropIndex("dbo.StudentFaculties", new[] { "Student_StudentId" });
            DropIndex("dbo.Teachers", new[] { "Faculty_FacultyId" });
            DropIndex("dbo.Exams", new[] { "Faculty_FacultyId" });
            DropIndex("dbo.Deans", new[] { "DeanId" });
            DropTable("dbo.StudentFaculties");
            DropTable("dbo.Teachers");
            DropTable("dbo.Students");
            DropTable("dbo.Exams");
            DropTable("dbo.Faculties");
            DropTable("dbo.Deans");
        }
    }
}
