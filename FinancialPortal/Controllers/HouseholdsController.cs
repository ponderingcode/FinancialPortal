﻿using FinancialPortal.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FinancialPortal.Controllers
{
    [Authorize]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public async Task<ActionResult> Index()
        {
            return View(await db.Households.ToListAsync());
        }

        // GET: Households/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = await db.Households.FindAsync(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        [Authorize(Roles = ApplicationRole.ADMINISTRATOR)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Households.Add(household);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        [Authorize(Roles = ApplicationRole.ADMINISTRATOR + ", " + HouseholdRole.HEAD_OF_HOUSEHOLD)]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = await db.Households.FindAsync(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            HouseholdViewModel householdViewModel = new HouseholdViewModel { Id = household.Id, Name = household.Name };
            householdViewModel.Members = new MultiSelectList(db.Users, Common.ID, Common.USER_NAME);
            householdViewModel.SelectedMembers = GetAllMemberUserIdsForHousehold(household.Id).ToList();
            return View(householdViewModel);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        // GET: Households/Delete/5
        [Authorize(Roles = ApplicationRole.ADMINISTRATOR)]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = await db.Households.FindAsync(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Household household = await db.Households.FindAsync(id);
            db.Households.Remove(household);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ICollection<string> GetAllMemberUserIdsForHousehold(int householdId)
        {
            return db.Households.Find(householdId).Members.Select(u => u.Id).ToList();
        }

        public ICollection<string> GetAllInviteeUserIdsForHousehold(int householdId)
        {
            return db.Households.Find(householdId).Invitees.Select(u => u.Id).ToList();
        }
    }
}
