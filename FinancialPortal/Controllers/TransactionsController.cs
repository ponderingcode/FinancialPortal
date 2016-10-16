using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancialPortal.Models;
using Microsoft.AspNet.Identity;

namespace FinancialPortal.Controllers
{
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        public ActionResult Index()
        {
            return View(db.Transactions.ToList());
        }

        // GET: Transactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        public ActionResult Create()
        {
            TransactionViewModel viewModel = new TransactionViewModel
            {
                BankAccounts = new SelectList(db.BankAccounts, Common.ID, Common.NAME),
                Categories = new SelectList(db.Categories, Common.ID, Common.NAME)
            };

            return View(viewModel);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TransactionViewModel viewModel)
        {
            string userId = User.Identity.GetUserId();
            BankAccount bankAccount = db.BankAccounts.Find(viewModel.SelectedBankAccount);
            Category category = db.Categories.Find(viewModel.SelectedCategory);
            Transaction transaction = new Transaction
            {
                Id = viewModel.Id,
                Amount = viewModel.Amount,
                BankAccountId = bankAccount.Id,
                BankAccount = bankAccount,
                CategoryId = category.Id,
                Category = category,
                Date = viewModel.Date,
                Description = viewModel.Description,
                EnteredById = userId,
                EnteredBy = db.Users.Find(userId),
                Reconciled = viewModel.Reconciled,
                ReconciledAmount = viewModel.ReconciledAmount,
                Type = viewModel.Type
            };


            if (TransactionTypeName.RECEIVED == transaction.Type)
            {
                bankAccount.Balance += transaction.Amount;
            }
            else
            {
                bankAccount.Balance -= transaction.Amount;
            }

            if (0 > bankAccount.Balance)
            {
                transaction.Misc = "OVERDRAFT";
            }

            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: Transactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BankAccountId,CategoryId,EnteredById,Date,Description,Type,Amount,ReconciledAmount,Reconciled")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
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
    }
}
