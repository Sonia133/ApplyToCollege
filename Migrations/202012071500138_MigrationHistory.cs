namespace College.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationHistory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Deans",
                c => new
                    {
                        DeanId = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.DeanId)
                .ForeignKey("dbo.Faculties", t => t.DeanId)
                .Index(t => t.DeanId);
            
            CreateTable(
                "dbo.Faculties",
                c => new
                    {
                        faculty_id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        City = c.String(),
                        Places = c.Int(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.faculty_id);
            
            CreateTable(
                "dbo.Exams",
                c => new
                    {
                        exam_id = c.Int(nullable: false, identity: true),
                        Subject = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Type = c.String(),
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
                        Name = c.String(),
                        Cnp = c.Double(nullable: false),
                        Frequency = c.String(),
                        Email = c.String(),
                        Sat = c.Double(nullable: false),
                        Badge = c.Int(nullable: false),
                        Faculty_FacultyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.student_id)
                .ForeignKey("dbo.Faculties", t => t.Faculty_FacultyId, cascadeDelete: true)
                .Index(t => t.Faculty_FacultyId);
            
            CreateTable(
                "dbo.Teachers",
                c => new
                    {
                        teacher_id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Subject = c.String(),
                        Email = c.String(),
                        Faculty_FacultyId = c.Int(),
                    })
                .PrimaryKey(t => t.teacher_id)
                .ForeignKey("dbo.Faculties", t => t.Faculty_FacultyId)
                .Index(t => t.Faculty_FacultyId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Deans", "DeanId", "dbo.Faculties");
            DropForeignKey("dbo.Teachers", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.Students", "Faculty_FacultyId", "dbo.Faculties");
            DropForeignKey("dbo.Exams", "Faculty_FacultyId", "dbo.Faculties");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Teachers", new[] { "Faculty_FacultyId" });
            DropIndex("dbo.Students", new[] { "Faculty_FacultyId" });
            DropIndex("dbo.Exams", new[] { "Faculty_FacultyId" });
            DropIndex("dbo.Deans", new[] { "DeanId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Teachers");
            DropTable("dbo.Students");
            DropTable("dbo.Exams");
            DropTable("dbo.Faculties");
            DropTable("dbo.Deans");
        }
    }
}
