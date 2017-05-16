using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Abaker86.CLAX.Tasks.Models
{
    [Table(name:"TASK_LIST_MAPPING")]
    public class TaskListViewModel
    {
        [Key]
        public int TASK_LIST_MAPPING_ID { get; set; }
        public List<TaskList> TASK_LISTS { get; set; }
        public string USER_ID { get; set; }
    }

    [Table(name:"TASK_LIST")]
    public class TaskList
    {
        [Key]
        public int TASK_LIST_ID { get; set; }
        public int TASK_LIST_MAPPING_ID { get; set; }
        public string LIST_NAME { get; set; }
        public List<TaskItem> TASK_ITEMS { get; set; }
    }

    [Table(name: "TASK_ITEM")]
    public class TaskItem
    {
        [Key]
        public int TASK_ITEM_ID { get; set; }
        public int TASK_LIST_ID { get; set; }
        public string ITEM_DESC { get; set; }
        public bool IS_COMPLETE { get; set; }
    }

    public class ListDbContext : IdentityDbContext<ApplicationUser>
    {
        public ListDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ListDbContext Create()
        {
            return new ListDbContext();
        }

        public DbSet<Abaker86.CLAX.Tasks.Models.TaskListViewModel> TaskListViewModels { get; set; }
        public DbSet<Abaker86.CLAX.Tasks.Models.TaskList> TaskLists { get; set; }
        public DbSet<Abaker86.CLAX.Tasks.Models.TaskItem> TaskItems { get; set; }
    }

}