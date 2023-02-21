using GIDataAccess.Models;
using GRDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PlanGIDataAccess.Models;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class PlanGIDbContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public virtual DbSet<im_PlanGoodsIssue> im_PlanGoodsIssue { get; set; }
        public virtual DbSet<im_PlanGoodsIssueItem> im_PlanGoodsIssueItem { get; set; }
        public virtual DbSet<View_PlanGIWithWave> View_PlanGIWithWave { get; set; }
        public virtual DbSet<View_PlanGI> View_PlanGI { get; set; }

        public virtual DbSet<GetValueByColumn> GetValueByColumn { get; set; }
        public virtual DbSet<View_PlanGiProcessStatus> View_PlanGiProcessStatus { get; set; }

        public virtual DbSet<im_Signatory_log> im_Signatory_log { get; set; }

        public DbSet<im_DocumentFile> im_DocumentFile { get; set; }

        public virtual DbSet<im_Pack> im_Pack { get; set; }
        public virtual DbSet<im_PackItem> im_PackItem { get; set; }
        public virtual DbSet<View_PrintOutPlanGI> View_PrintOutPlanGI { get; set; }
        public virtual DbSet<View_RPT_Delivery_Note> View_RPT_Delivery_Note { get; set; }
        public virtual DbSet<VIEW_PlanGoodsIssue> VIEW_PlanGoodsIssue { get; set; }
        public virtual DbSet<View_RPT_Product_Return> View_RPT_Product_Return { get; set; }
        public virtual DbSet<im_GoodsIssue> im_GoodsIssue { get; set; }
        public virtual DbSet<CheckStatusWave> CheckStatusWave { get; set; }
        public virtual DbSet<CheckWaveDip> CheckWaveDip { get; set; }
        public virtual DbSet<CheckWaveDipbyWave> CheckWaveDipbyWave { get; set; }
        public virtual DbSet<View_CheckWave_Round> View_CheckWave_Round { get; set; }
        public virtual DbSet<View_RPT_DeliveryNote_emergency> View_RPT_DeliveryNote_emergency { get; set; }
        public virtual DbSet<View_im_PlanGoodsIssue_status_SAP> View_im_PlanGoodsIssue_status_SAP { get; set; }
        public virtual DbSet<im_TruckLoad> im_TruckLoad { get; set; }
        public virtual DbSet<im_TruckLoadItem> im_TruckLoadItem { get; set; }

        public DbSet<log_api_request> log_api_request { get; set; }
        public DbSet<log_api_reponse> log_api_reponse { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false);

                var configuration = builder.Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection").ToString();

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }

    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
        public int Rating { get; set; }
        public List<Post> Posts { get; set; }
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
