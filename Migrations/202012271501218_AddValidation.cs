namespace College.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValidation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "Faculty_FacultyId", "dbo.Faculties");
            DropIndex("dbo.Students", new[] { "Faculty_FacultyId" });
            AlterColumn("dbo.Deans", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Faculties", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Faculties", "Description", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Exams", "Subject", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Exams", "Type", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Frequency", c => c.String(nullable: false));
            AlterColumn("dbo.Students", "Faculty_FacultyId", c => c.Int());
            AlterColumn("dbo.Teachers", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Teachers", "Subject", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.Students", "Faculty_FacultyId");
            AddForeignKey("dbo.Students", "Faculty_FacultyId", "dbo.Faculties", "faculty_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "Faculty_FacultyId", "dbo.Faculties");
            DropIndex("dbo.Students", new[] { "Faculty_FacultyId" });
            AlterColumn("dbo.Teachers", "Subject", c => c.String());
            AlterColumn("dbo.Teachers", "Name", c => c.String());
            AlterColumn("dbo.Students", "Faculty_FacultyId", c => c.Int(nullable: false));
            AlterColumn("dbo.Students", "Frequency", c => c.String());
            AlterColumn("dbo.Students", "Name", c => c.String());
            AlterColumn("dbo.Exams", "Type", c => c.String());
            AlterColumn("dbo.Exams", "Subject", c => c.String(nullable: false));
            AlterColumn("dbo.Faculties", "Description", c => c.String());
            AlterColumn("dbo.Faculties", "City", c => c.String());
            AlterColumn("dbo.Deans", "Email", c => c.String());
            CreateIndex("dbo.Students", "Faculty_FacultyId");
            AddForeignKey("dbo.Students", "Faculty_FacultyId", "dbo.Faculties", "faculty_id", cascadeDelete: true);
        }
    }
}
