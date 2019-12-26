namespace Loader.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Loader.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "Loader.Models.ApplicationDbContext";
        }

        protected override void Seed(Loader.Models.ApplicationDbContext context)
        {

            context.DataType.AddOrUpdate(
                     new Models.Datatype { DTId = 1, DType = "Numeric" },
                     new Models.Datatype { DTId = 2, DType = "Date" },
                     new Models.Datatype { DTId = 3, DType = "Bit" },
                     new Models.Datatype { DTId = 4, DType = "String" },
                     new Models.Datatype { DTId = 5, DType = "Script" }
                 );

            context.Param.AddOrUpdate(
                    new Models.Param { ParentId = 0, IsGroup = true, PId = 1, PName = "System Parameters" },
                    new Models.Param { ParentId = 0, IsGroup = true, PId = 2, PName = "User Parameters" },
                    new Models.Param { PId = 3, PName = "Company Info", ParentId = 1, IsGroup = true },
                    new Models.Param { PId = 4, PName = "Company Name", ParentId = 3, IsGroup = false },
                    new Models.Param { PId = 5, PName = "Company Address", ParentId = 3, IsGroup = false },
                    new Models.Param { PId = 6, PName = "Phone Number", ParentId = 3, IsGroup = false },
                    new Models.Param { PId = 7, PName = "Email", ParentId = 3, IsGroup = false },
                    new Models.Param { PId = 9, PName = "Logo", ParentId = 3, IsGroup = false },
                    new Models.Param { PId = 11, PName = "Designation", ParentId = 1, IsGroup = false },
                    new Models.Param { PId = 14, PName = "Designation in Heirarchy", ParentId = 11, IsGroup = false },
                    new Models.Param { PId = 15, PName = "Allow Employee in Group", ParentId = 11, IsGroup = false },
                    new Models.Param { PId = 16, PName = "Department in Heirarchy", ParentId = 13, IsGroup = false },
                     new Models.Param { PId = 17, PName = "Authorization", ParentId = 1, IsGroup = true },
                     new Models.Param { PId = 18, PName = "Menu Template with Designation", ParentId = 17, IsGroup = false },
                     new Models.Param { PId = 19, PName = "Employee", ParentId = 1, IsGroup = true },
                     new Models.Param { PId = 20, PName = "With Department", ParentId = 19, IsGroup = false },
                     new Models.Param { PId = 21, PName = "With Designation", ParentId = 19, IsGroup = false },
                     new Models.Param { PId = 22, PName = "With Payroll Parameters", ParentId = 19, IsGroup = false },

                     new Models.Param { PId = 23, PName = "User with Employee", ParentId = 17, IsGroup = false }
                     

                );
            context.ParamValue.AddOrUpdate(
                new Models.ParamValue {PId=4,DTId=4,PDescription="Company Name" ,PValue=""},
                new Models.ParamValue { PId = 5, DTId = 4, PDescription = "Company Address" },
                new Models.ParamValue { PId = 6, DTId = 1, PDescription = "Phone Number" },
                new Models.ParamValue { PId = 7, DTId = 4, PDescription = "Email" },
                new Models.ParamValue { PId = 9, DTId = 4, PDescription = "Logo Path" },
                new Models.ParamValue { PId = 14, DTId = 3, PDescription = "Designation in Heirarchy",PValue="true" },
                new Models.ParamValue { PId = 15, DTId = 3, PDescription = "Allow Employee in Group", PValue = "true" },
                new Models.ParamValue { PId = 16, DTId = 3, PDescription = "Department in Heirarchy", PValue = "true" },
                new Models.ParamValue { PId = 18, DTId = 3, PDescription = "Menu Template with Designation", PValue = "true" },
                new Models.ParamValue { PId = 20, DTId = 3, PDescription = "With Department", PValue = "true" },
                new Models.ParamValue { PId = 21, DTId = 3, PDescription = "With Designation", PValue = "true" },
                new Models.ParamValue { PId = 22, DTId = 3, PDescription = "With Date Of Join,Gender,Maritial Status,Nationality,Religion,BloodGroup", PValue = "true" },
                new Models.ParamValue { PId = 23, DTId = 3, PDescription = "User With Employee", PValue = "true" }

           );


            context.BloodGroup.AddOrUpdate(
                new Models.BloodGroup { BGId = 1, BGName = "A+" },
                new Models.BloodGroup { BGId = 2, BGName = "A-" },
                new Models.BloodGroup { BGId = 3, BGName = "B+" },
                new Models.BloodGroup { BGId = 4, BGName = "B-" },
                new Models.BloodGroup { BGId = 5, BGName = "AB+" },
                new Models.BloodGroup { BGId = 6, BGName = "AB-" },
                new Models.BloodGroup { BGId = 7, BGName = "O+" },
                new Models.BloodGroup { BGId = 8, BGName = "O-" }

                );
            context.Religion.AddOrUpdate(
                new Models.Religion { RId = 1, ReligionName = "Hindu" },
                new Models.Religion { RId = 2, ReligionName = "Muslim" },
                new Models.Religion { RId = 3, ReligionName = "Christian" },
                new Models.Religion { RId = 4, ReligionName = "Buddhist" }
                );
            context.Nationality.AddOrUpdate(
                new Models.Nationality { NId = 1, NName = "Nepali" },
                new Models.Nationality { NId = 2, NName = "Indian" }

                );
            context.MaritialStatus.AddOrUpdate(
                new Models.MaritialStatus { MSId = 1, MSName = "Unmarried" },
                new Models.MaritialStatus { MSId = 2, MSName = "Married" },
                new Models.MaritialStatus { MSId = 3, MSName = "Divorced" }

                );
            context.Menu.AddOrUpdate(
                new Models.Menu { MenuId = 1, PMenuId = 0, MenuCaption = "Parameter", IsGroup = true, IsEnable = true, IsContextMenu = true, Order = 1 },
                new Models.Menu { MenuId = 2, PMenuId = 1, MenuCaption = "Parameter Setup", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 2,Controler="Parameter",Acton="Index" },
                new Models.Menu { MenuId = 3, PMenuId = 1, MenuCaption = "System Customize", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 3,Controler="Parameter",Acton="SystemCustomize" },
                new Models.Menu { MenuId = 4, PMenuId = 1, MenuCaption = "User Customize", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 3, Controler = "Parameter", Acton = "UserCustomize" },
                new Models.Menu { MenuId = 5, PMenuId = 0, MenuCaption = "Department", IsGroup = true, IsEnable = true, IsContextMenu = true, Order = 3 },
                new Models.Menu { MenuId = 6, PMenuId = 5, MenuCaption = "Department Setup", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 3, Controler = "Department", Acton = "Index" },
                new Models.Menu { MenuId = 7, PMenuId = 0, MenuCaption = "Designation", IsGroup = true, IsEnable = true, IsContextMenu = true, Order = 3 },
                new Models.Menu { MenuId = 8, PMenuId = 7, MenuCaption = "Designation Setup", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 3, Controler = "Designation", Acton = "Index" },
                new Models.Menu { MenuId = 9, PMenuId = 0, MenuCaption = "Employee", IsGroup = true, IsEnable = true, IsContextMenu = true, Order = 3 },
                new Models.Menu { MenuId = 10, PMenuId = 9, MenuCaption = "Employee Setup", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 3, Controler = "Employee", Acton = "Index" },
                new Models.Menu { MenuId = 11, PMenuId = 0, MenuCaption = "Users", IsGroup = true, IsEnable = true, IsContextMenu = true, Order = 3 },
                new Models.Menu { MenuId = 12, PMenuId = 11, MenuCaption = "Users Setup", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 3, Controler = "Users", Acton = "Index" },
               new Models.Menu { MenuId = 13, PMenuId = 0, MenuCaption = "Menu", IsGroup = true, IsEnable = true, IsContextMenu = true, Order = 3 },
                new Models.Menu { MenuId = 14, PMenuId = 13, MenuCaption = "Menu Setup", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 3, Controler = "Menu", Acton = "Index" },
                new Models.Menu { MenuId = 15, PMenuId = 13, MenuCaption = "MenuTemplate Setup", IsGroup = false, IsEnable = true, IsContextMenu = true, Order = 3, Controler = "MenuTemplate", Acton = "Index" }
                );
           
            context.Gender.AddOrUpdate(
                new Models.Gender { GenderId = 1, GenderName = "Male" },
                new Models.Gender { GenderId = 2, GenderName = "Female" },
                new Models.Gender { GenderId = 3, GenderName = "Others" }

                );
            context.Status.AddOrUpdate(
                new Models.Status { StatusId = 1, StatusName = "Active" },
                new Models.Status { StatusId = 2, StatusName = "Inactive" },
                new Models.Status { StatusId = 3, StatusName = "Resigned" }
                );
            context.Designation.AddOrUpdate(
                new Models.Designation { DGId=1,DGName="Super User",PDGId=0,PostedBy=1,PostedOn=DateTime.Now}


                );
            //context.Department.AddOrUpdate(
            //    new Models.Department { DeptId=1,DeptName="Admin",PDeptId=0,PostedBy=1,PostedOn=DateTime.Now}


            //    );
            context.Roles.AddOrUpdate(
               new Models.Role { DGId = 1, MTId = 1, Name = "Super Admin",Id=1 }
               );
            context.MenuTemplate.AddOrUpdate(
                new Models.MenuTemplate { MTId = 1, MTName = "Super Admin", DesignationId = 1, PostedBy = 1, PostedOn = DateTime.Now }

                );
            context.MenuVsTemplate.AddOrUpdate(
                new Models.MenuVsTemplate { MenuId=1,TemplateId=1},
                new Models.MenuVsTemplate { MenuId = 2, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 3, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 4, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 5, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 6, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 7, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 8, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 9, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 10, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 11, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 12, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 13, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 14, TemplateId = 1 },
                new Models.MenuVsTemplate { MenuId = 15, TemplateId = 1 }

                );
       

           
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
