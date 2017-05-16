using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Abaker86.CLAX.Tasks.Models;
using Microsoft.AspNet.Identity;

namespace Abaker86.CLAX.Tasks.Controllers
{
    [Authorize]
    public class TaskListsController : Controller
    {
        private ListDbContext db = new ListDbContext();
        private string _CurrentUser = System.Web.HttpContext.Current.User.Identity.GetUserId();

        // GET: TaskLists
        public ActionResult Index()
        {
            TaskListViewModel model = db.TaskListViewModels.Where(m => m.USER_ID == _CurrentUser).FirstOrDefault();
            if (model == null)
            {
                model = new TaskListViewModel() { USER_ID = _CurrentUser };
            }
            else
            {
                model.TASK_LISTS = db.TaskLists.Where(m => m.TASK_LIST_MAPPING_ID == model.TASK_LIST_MAPPING_ID).ToList();
            }

            return View(model);
        }

        // GET: TaskLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskListViewModel taskListViewModel = db.TaskListViewModels.Find(id);
            if (taskListViewModel == null)
            {
                return HttpNotFound();
            }
            return View(taskListViewModel);
        }

        // GET: TaskLists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TaskLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TASK_LIST_MAPPING_ID, USER_ID, TASK_LISTS")] TaskListViewModel taskListViewModel)
        {
            if (ModelState.IsValid)
            {

                TaskListViewModel tlvm = db.TaskListViewModels.Where(m => m.TASK_LIST_MAPPING_ID == taskListViewModel.TASK_LIST_MAPPING_ID).SingleOrDefault();
                if (tlvm == null) {
                    db.TaskListViewModels.Add(taskListViewModel);
                    db.SaveChanges();
                }

                db.TaskLists.Add(new TaskList() { TASK_LIST_MAPPING_ID = taskListViewModel.TASK_LIST_MAPPING_ID, LIST_NAME = "New List"});
                
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(taskListViewModel);
        }


        // POST: TaskLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateListItem([Bind(Include = "TASK_LIST_MAPPING_ID, TASK_LIST_ID, LIST_NAME, TASK_ITEMS")] TaskList taskList)
        {
            if (ModelState.IsValid)
            {
                
                db.TaskItems.Add(new TaskItem() { TASK_LIST_ID = taskList.TASK_LIST_ID,  ITEM_DESC = "NewList Item" });

                db.SaveChanges();
                string path = "Edit/" + taskList.TASK_LIST_ID;
                return RedirectToAction(path);
            }

            return View(taskList);
        }




        // GET: TaskLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskList taskList = db.TaskLists.Find(id);
            if (taskList == null)
            {
                return HttpNotFound();
            }
            else
            {
                taskList.TASK_ITEMS = db.TaskItems.Where(m => m.TASK_LIST_ID == taskList.TASK_LIST_ID).ToList();
            }
            return View(taskList);
        }

        // POST: TaskLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TASK_LIST_ID, TASK_LIST_MAPPING_ID, LIST_NAME, TASK_ITEMS")] TaskList taskList)
        {
            if (ModelState.IsValid)
            {
                if (taskList.TASK_ITEMS != null)
                {
                    foreach (TaskItem task in taskList.TASK_ITEMS)
                    {
                        db.Entry(task).State = EntityState.Modified;
                    }
                }

                db.Entry(taskList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit/"+taskList.TASK_LIST_ID);
            }
            return View(taskList);
        }

        //// GET: TaskLists/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TaskListViewModel taskListViewModel = db.TaskListViewModels.Find(id);
        //    if (taskListViewModel == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(taskListViewModel);
        //}

        // POST: TaskLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaskList taskList = db.TaskLists.Find(id);
            db.TaskLists.Remove(taskList);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteListItem(int id)
        {
            string path = string.Empty;
            TaskItem taskItem = db.TaskItems.Find(id);
            path = "Edit/" + taskItem.TASK_LIST_ID;
            db.TaskItems.Remove(taskItem);
            db.SaveChanges();

            return RedirectToAction(path);
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
