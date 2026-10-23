using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShoesShop.Models;

public partial class ShoesShopContext : DbContext
{
    public ShoesShopContext()
    {
    }

    public ShoesShopContext(DbContextOptions<ShoesShopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductItem> ProductItems { get; set; }

    public virtual DbSet<ProductsOrder> ProductsOrders { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Size> Sizes { get; set; }

    public virtual DbSet<Subcategory> Subcategories { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=ShoesShop;Username=postgres;Password=1111");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.CategoryName, "categories_category_name_key").IsUnique();

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.CategoryName).HasColumnName("category_name");
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.IdManufacturers).HasName("manufacturers_pkey");

            entity.ToTable("manufacturers");

            entity.HasIndex(e => e.ManufacturerName, "manufacturers_manufacturer_name_key").IsUnique();

            entity.Property(e => e.IdManufacturers).HasColumnName("id_manufacturers");
            entity.Property(e => e.ManufacturerName).HasColumnName("manufacturer_name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrders).HasName("orders_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.IdOrders).HasColumnName("id_orders");
            entity.Property(e => e.DateOfOrder)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("date_of_order");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(12, 2)
                .HasColumnName("total_amount");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("fk_orders_user");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProducts).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.IdProducts).HasColumnName("id_products");
            entity.Property(e => e.Composition).HasColumnName("composition");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdManufacturer).HasColumnName("id_manufacturer");
            entity.Property(e => e.IdSubcategory).HasColumnName("id_subcategory");
            entity.Property(e => e.PhotoUrl).HasColumnName("photo_url");
            entity.Property(e => e.ProductName).HasColumnName("product_name");
            entity.Property(e => e.SizeRange).HasColumnName("size_range");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("fk_products_category");

            entity.HasOne(d => d.IdManufacturerNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdManufacturer)
                .HasConstraintName("fk_products_manufacturer");

            entity.HasOne(d => d.IdSubcategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdSubcategory)
                .HasConstraintName("fk_products_subcategory");
        });

        modelBuilder.Entity<ProductItem>(entity =>
        {
            entity.HasKey(e => e.IdProductItems).HasName("product_items_pkey");

            entity.ToTable("product_items");

            entity.HasIndex(e => new { e.IdProduct, e.IdSize }, "product_items_id_product_id_size_key").IsUnique();

            entity.Property(e => e.IdProductItems).HasColumnName("id_product_items");
            entity.Property(e => e.CountInStock).HasColumnName("count_in_stock");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdSize).HasColumnName("id_size");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasColumnName("price");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("fk_items_product");

            entity.HasOne(d => d.IdSizeNavigation).WithMany(p => p.ProductItems)
                .HasForeignKey(d => d.IdSize)
                .HasConstraintName("fk_items_size");
        });

        modelBuilder.Entity<ProductsOrder>(entity =>
        {
            entity.HasKey(e => e.IdProductsOrders).HasName("products_orders_pkey");

            entity.ToTable("products_orders");

            entity.HasIndex(e => new { e.IdOrder, e.IdProductItem }, "products_orders_id_order_id_product_item_key").IsUnique();

            entity.Property(e => e.IdProductsOrders).HasColumnName("id_products_orders");
            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdProductItem).HasColumnName("id_product_item");
            entity.Property(e => e.Quantity).HasColumnName("quantity");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.ProductsOrders)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("fk_po_order");

            entity.HasOne(d => d.IdProductItemNavigation).WithMany(p => p.ProductsOrders)
                .HasForeignKey(d => d.IdProductItem)
                .HasConstraintName("fk_po_item");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRoles).HasName("roles_pkey");

            entity.ToTable("roles");

            entity.HasIndex(e => e.RoleName, "roles_role_name_key").IsUnique();

            entity.Property(e => e.IdRoles).HasColumnName("id_roles");
            entity.Property(e => e.RoleName).HasColumnName("role_name");
        });

        modelBuilder.Entity<Size>(entity =>
        {
            entity.HasKey(e => e.IdSizes).HasName("sizes_pkey");

            entity.ToTable("sizes");

            entity.HasIndex(e => e.Size1, "sizes_size_key").IsUnique();

            entity.Property(e => e.IdSizes).HasColumnName("id_sizes");
            entity.Property(e => e.Size1)
                .HasPrecision(4, 1)
                .HasColumnName("size");
        });

        modelBuilder.Entity<Subcategory>(entity =>
        {
            entity.HasKey(e => e.IdSubcategories).HasName("subcategories_pkey");

            entity.ToTable("subcategories");

            entity.HasIndex(e => e.SubcategoryName, "subcategories_subcategory_name_key").IsUnique();

            entity.Property(e => e.IdSubcategories).HasColumnName("id_subcategories");
            entity.Property(e => e.SubcategoryName).HasColumnName("subcategory_name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsers).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "users_login_key").IsUnique();

            entity.Property(e => e.IdUsers).HasColumnName("id_users");
            entity.Property(e => e.FirstName).HasColumnName("first_name");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.LastName).HasColumnName("last_name");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.MiddleName).HasColumnName("middle_name");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("fk_users_role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
