using FinancialPortal.Models;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using FinancialPortal.Helpers;
using FinancialPortal.Identity;

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

        public ActionResult JoinHousehold(int id)
        {
            AddHouseholdMember(User.Identity.GetUserId(), id);
            return RedirectToAction(ActionName.INDEX, ControllerName.HOUSEHOLDS);

        }

        public ActionResult LeaveHousehold(int id)
        {
            RemoveHouseholdMember(User.Identity.GetUserId(), id);
            return RedirectToAction(ActionName.INDEX, ControllerName.HOME);
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
            household.HeadOfHousehold = UserRolesHelper.GetUserById(User.Identity.GetUserId()); ;
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
                AddHouseholdMember(User.Identity.GetUserId(), household.Id, true);
                ApplicationHelper.AssociateAllBankAccountsWithHousehold();
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
            householdViewModel.Members = new MultiSelectList(db.Users, Common.ID, Common.USER_NAME);
            householdViewModel.SelectedMembers = GetAllMemberIdsForHousehold(household.Id);
            householdViewModel.IncomeAnnual = db.Households.Find(householdViewModel.Id).IncomeAnnual;
            return View(householdViewModel);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HouseholdViewModel householdViewModel)
        {
            if (ModelState.IsValid)
            {
                Household householdData = db.Households.Find(householdViewModel.Id);
                householdData.Name = householdViewModel.Name;
                householdData.IncomeAnnual = householdViewModel.IncomeAnnual;
                var membersOfHousehold = GetAllMemberIdsForHousehold(householdData.Id);

                if (null == householdViewModel.SelectedMembers)
                {
                    RemoveAllMembersFromHousehold(householdData.Id);
                }
                else
                {
                    foreach (var userId in membersOfHousehold)
                    {
                        if (!householdViewModel.SelectedMembers.Contains(userId))
                        {
                            RemoveHouseholdMember(userId, householdData.Id);
                        }
                    }
                    foreach (var userId in householdViewModel.SelectedMembers)
                    {
                        if (!IsMemberOfHousehold(userId, householdData.Id))
                        {
                            InviteUserToJoinHousehold(userId, householdData.Id);
                        }
                    }
                }
                db.SaveChanges();
                return RedirectToAction(ActionName.INDEX);
            }
            return View(householdViewModel);
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
            RemoveAllMembersFromHousehold(id);
            db.Households.Remove(household);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ActionResult InvitationAcceptance(int householdId, string userId, string inviteeEmailAddress)
        {
            //ApplicationUser user = db.Users.FirstOrDefault(u => u.Email == inviteeEmailAddress);
            InvitationAcceptanceViewModel invitationAcceptanceViewModel = new InvitationAcceptanceViewModel
            {
                InviteeEmailAddress = inviteeEmailAddress,
                //HasAccount = !(null == user)
                HasAccount = true
            };
            AddHouseholdMember(userId, householdId);
            return View(invitationAcceptanceViewModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //
        // Helper functions
        //

        public bool RemoveAllMembersFromHousehold(int householdId)
        {
            db.Households.Find(householdId).Members.Clear();
            db.SaveChanges();
            return 0 == db.Households.Find(householdId).Members.Count;
        }

        public bool IsMemberOfHousehold(string userId, int householdId)
        {
            List<ApplicationUser> householdMembers = db.Households.Find(householdId).Members.ToList();
            foreach (ApplicationUser user in householdMembers)
            {
                if (user.Id == userId)
                {
                    return true;
                }
            }
            return false;
        }

        public void InviteUserToJoinHousehold(string userId, int householdId)
        {
            string senderEmailAddress = UserRolesHelper.GetUserById(User.Identity.GetUserId()).Email;
            string inviteeEmailAddress = UserRolesHelper.GetUserById(userId).Email;
            string invitationUrl = Url.Action(ActionName.INVITATION_ACCEPTANCE, ControllerName.HOUSEHOLDS, new { householdId = householdId, userId = userId, inviteeEmailAddress = inviteeEmailAddress }, protocol: Request.Url.Scheme);
            UserSpecificIdentityMessage notificationMessage = new UserSpecificIdentityMessage
            {
                Origin = senderEmailAddress,
                Destination = inviteeEmailAddress,
                Subject = "You've been invited to join a household budget",
                Body = "<a href=" + invitationUrl + ">Click here if you wish to accept the invitation</a>"
            };
            EmailService emailService = new EmailService();
            emailService.SendAsSpecificUserAsync(notificationMessage);
        }

        public bool AddHouseholdMember(string userId, int householdId, bool headOfHousehold = false)
        {
            if (!IsMemberOfHousehold(userId, householdId))
            {
                UserRolesHelper.RemoveUserFromRole(userId, HouseholdRoleName.NONE);
                UserRolesHelper.AddUserToRole(userId, HouseholdRoleName.MEMBER);
                if (headOfHousehold)
                {
                    UserRolesHelper.AddUserToRole(userId, HouseholdRoleName.HEAD_OF_HOUSEHOLD);
                }
                db.Households.Find(householdId).Members.Add(db.Users.Find(userId));
                db.SaveChanges();
            }
            return IsMemberOfHousehold(userId, householdId);
        }

        public bool RemoveHouseholdMember(string userId, int householdId)
        {
            if (IsMemberOfHousehold(userId, householdId))
            {
                UserRolesHelper.RemoveUserFromRole(userId, HouseholdRoleName.MEMBER);
                UserRolesHelper.AddUserToRole(userId, HouseholdRoleName.NONE);
                db.Households.Find(householdId).Members.Remove(db.Users.Find(userId));
                db.SaveChanges();
            }
            return !IsMemberOfHousehold(userId, householdId);
        }

        public List<ApplicationUser> GetAllMembersOfHousehold(int householdId)
        {
            return db.Households.Find(householdId).Members.ToList();
        }

        public List<string> GetAllMemberIdsForHousehold(int householdId)
        {
            return db.Households.Find(householdId).Members.Select(u => u.Id).ToList();
        }
    }
}
