namespace Project.Models 
{
    using Project.Models.DataMapper;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Group4_DB : DbContext
    {
        
        public Group4_DB()
            : base("name=Group4_DB")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Group4_DB, Migrations.Configuration>("Group4_DB"));
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<GroupRoles> GroupRoles { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }
        public virtual DbSet<Attributes> Attributes { get; set; }
        public virtual DbSet<ProductAttribute> ProductAttributes { get; set; }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Slider> Sliders { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<ProductSize> ProductSizes { get; set; }
        public virtual DbSet<Color> Colors { get; set; }
        public virtual DbSet<ProductColor> ProductColors { get; set; }
        
        public System.Data.Entity.DbSet<Project.Models.DataMapper.AttributeType> AttributeTypes { get; set; }
        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}