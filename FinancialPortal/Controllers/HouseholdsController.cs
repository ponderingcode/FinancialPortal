using FinancialPortal.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using FinancialPortal.Helpers;

namespace FinancialPortal.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Households
        public async Task<ActionResult> Index()
        {
            bool showCreateNew = (0 == db.Households.ToList().Count);
            ViewBag.showCreateNew = showCreateNew;
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
        //[Authorize(Roles = ApplicationRoleName.ADMINISTRATOR)]
        public ActionResult Create()
        {
            Household household = new Household();
            ApplicationUser creator = UserRolesHelper.GetUserById(User.Identity.GetUserId());
            household.HeadOfHousehold = creator;
            household.Members.Add(creator);
            return View(household);
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,HeadOfHousehold")] Household household)
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
        [Authorize(Roles = HouseholdRoleName.HEAD_OF_HOUSEHOLD)]
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
            //householdViewModel.HeadOfHousehold = household.HeadOfHousehold.FullName;
            householdViewModel.Members = new MultiSelectList(db.Users, Common.ID, Common.USER_NAME);
            householdViewModel.SelectedMembers = GetAllMemberUserIdsForHousehold(household.Id).ToList();
            return View(householdViewModel);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,HeadOfHousehold")] Household household)
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
        [Authorize(Roles = HouseholdRoleName.HEAD_OF_HOUSEHOLD)]
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

        public ActionResult InvitationAcceptance(int id, string inviteeEmailAddress)
        {
            ApplicationUser existingAccount = db.Users.Find(inviteeEmailAddress);
            InvitationAcceptanceViewModel invitationAcceptanceViewModel = new InvitationAcceptanceViewModel
            {
                InviteeEmailAddress = inviteeEmailAddress,
                HasAccount = !(null == existingAccount)
            };
            //ViewBag.InviteeEmailAddress = inviteeEmailAddress;
            //ViewBag.HasAccount = !(null == existingAccount);
            return View(invitationAcceptanceViewModel);
        }


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public void SendInvitation(string inviteeEmailAddress)
        //{
        //    string invitationUrl = Url.Action(ActionName.INVITATION_ACCEPTED, ControllerName.HOUSEHOLDS, new { id = householdId }, protocol: Request.Url.Scheme);
        //    IdentityMessage notificationMessage = new IdentityMessage
        //    {
        //        Destination = inviteeEmailAddress,
        //        Subject = "You're invited to join a Household Budget",
        //        Body = "<a href=" + invitationUrl + ">Take a look</a>"
        //    };
        //    EmailService emailService = new EmailService();
        //    emailService.SendAsync(notificationMessage);
        //}

        // GET: Households/SendInvitation/5
        public ActionResult Invite(int id)
        {
            InvitationViewModel invitationViewModel = new InvitationViewModel { Id = id };
            return View(invitationViewModel);
        }

        // POST: Households/SendInvitation/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invite(InvitationViewModel invitationViewModel)
        {
            if (ModelState.IsValid)
            {
                SendInvitationEmail(invitationViewModel);
            }
            return RedirectToAction(ActionName.EDIT, new { id = invitationViewModel.Id });
        }

        public void SendInvitationEmail(InvitationViewModel invitationViewModel)
        {
            string invitationUrl = Url.Action(ActionName.INVITATION_ACCEPTANCE, ControllerName.HOUSEHOLDS, new { id = invitationViewModel.Id, inviteeEmailAddress = invitationViewModel.InviteeEmailAddress }, protocol: Request.Url.Scheme);
            IdentityMessage notificationMessage = new IdentityMessage
            {
                Destination = invitationViewModel.InviteeEmailAddress,
                Subject = "You've been invited to join a household budget",
                Body = "<a href=" + invitationUrl + ">Click here if you wish to accept the invitation</a>"
            };
            EmailService emailService = new EmailService();
            emailService.SendAsync(notificationMessage);
        }

        public ICollection<string> GetAllMemberUserIdsForHousehold(int householdId)
        {
            return db.Households.Find(householdId).Members.Select(u => u.Id).ToList();
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
