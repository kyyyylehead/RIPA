﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RIPASTOP.Models;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace RIPASTOP.Controllers
{
    public class StopsSubmissionController : Controller
    {
        private RIPASTOPContext db = new RIPASTOPContext();
        private Entities entitiesdb = new Entities();
        private DateTime startDate;

        public partial class UserAuth
        {
            public bool authorized { get; set; }
            public bool authorizedAdmin { get; set; }
        }
        public static bool UserBelongsToGroup(string group, string username)
        {
            PrincipalContext pc = new PrincipalContext(ContextType.Domain, ConfigurationManager.AppSettings["domain"]);
            GroupPrincipal gp = GroupPrincipal.FindByIdentity(pc, group);
            UserPrincipal up = UserPrincipal.FindByIdentity(pc, username);
            if (gp == null)
            {
                return false;
            }
            else
            {
                return up.IsMemberOf(gp);
            }
        }

        public static UserAuth AuthorizeUser(string username)
        {
            UserAuth user = new UserAuth();
            string domain = string.Format(@"{0}\", ConfigurationManager.AppSettings["domain"]);
            if (username.ToUpper().IndexOf(domain) > -1)
            {
                username = username.ToUpper().Replace(domain, "");
            }
            user.authorizedAdmin = UserBelongsToGroup(ConfigurationManager.AppSettings["authorizedAdmin"], username);

            return user;
        }

        // GET: Admin
        public ActionResult Index(int? sid)
        {
            
            UserAuth user = new UserAuth();

            user = AuthorizeUser(User.Identity.Name.ToString());

            if (!user.authorizedAdmin)
            {
                return RedirectToAction("Unauthorized", "Home");
            }

            UserProfile_Conf uid = db.UserProfile_Conf.SingleOrDefault(x => x.NTUserName == User.Identity.Name.ToString());
            ViewBag.UserProfileID = uid.UserProfileID;
            ViewBag.admin = user.authorizedAdmin;
            // web.config debug setting
            ViewBag.debug = HttpContext.IsDebuggingEnabled;

            // Check if there are "Pending Fixes" before allowing them to submit again
            if (sid != 0)
            {
                Submissions submitedRec = entitiesdb.Submissions.Find(sid);
                //Submissions submitedRec = entitiesdb.Submissions
                //                            .Where(x => x.Status == "Pending Fixes")
                //                            .OrderByDescending(x => x.ID)
                //                            .Select(x => x).FirstOrDefault();
                if (submitedRec != null)
                {
                    return RedirectToAction("SubmissionStatusGet", "StopsSubmission", new { sid = submitedRec.ID, endDate = submitedRec.EndDate });
                }
            }
           
            Submissions dateRec = entitiesdb.Submissions
                            .OrderByDescending(x => x.EndDate)
                            .Select(x => x).FirstOrDefault();
            if (dateRec != null)
            {
                startDate = Convert.ToDateTime(dateRec.EndDate).AddDays(1);
            }
            else
            {
                startDate = Convert.ToDateTime("2018-07-01");
                //startDate = Convert.ToDateTime("2018-07-16");
            }
            //List<Stop> Stops = db.Stop
            //        .Where(x => x.Status.Trim() != "success").ToList();
            //ExtractJNode eJson;
            //foreach (Stop st in Stops)
            //{
            //    //extract date from JsonStop
            //    JObject JsonStopO;
            //    JsonStopO = JObject.Parse(st.JsonStop);
            //    eJson = new ExtractJNode("date", JsonStopO);
            //    startDate = Convert.ToDateTime(eJson.traverseNode());

            //    // Only stops after July 1st 2018 should be submitted
            //    //if (startDate >= Convert.ToDateTime("2018-07-01"))
            //    if (startDate >= Convert.ToDateTime("2018-04-17"))
            //    {
            //        break;
            //    }
            //}
            Submissions submission = new Submissions();
            if (startDate.ToString() == "1/1/0001 12:00:00 AM")
            {
                ModelState.AddModelError(string.Empty, "There are no records to submit");
            }
            else
            {
                submission.StartDate = startDate;
                ViewBag.StartDate = startDate.ToString();
                if (TempData["CustomError"] != null)
                {
                    ModelState.AddModelError(string.Empty, TempData["CustomError"].ToString());
                }
            }
            submission.subList = entitiesdb.Submissions
                                    .OrderByDescending(x => x.StartDate)
                                    .OrderByDescending(y => y.DateSubmitted).ToList();

            return View(submission);
        }


        public ActionResult SubmissionStatusGet(int? sid, DateTime? endDate)
        {
            UserAuth user = new UserAuth();

            user = AuthorizeUser(User.Identity.Name.ToString());

            if (!user.authorizedAdmin)
            {
                return RedirectToAction("Unauthorized", "Home");
            }

            UserProfile_Conf uid = db.UserProfile_Conf.SingleOrDefault(x => x.NTUserName == User.Identity.Name.ToString());
            ViewBag.UserProfileID = uid.UserProfileID;
            ViewBag.admin = user.authorizedAdmin;

            // web.config debug setting
            ViewBag.debug = HttpContext.IsDebuggingEnabled;

            Submissions submission = entitiesdb.Submissions.Find(sid);
            if (endDate == null)
            {
                endDate = submission.EndDate;
            }
            ViewBag.submissionEndDate = endDate;

            if (endDate < submission.StartDate || endDate > DateTime.Now)
            {
                TempData["CustomError"] = "Invalid Date Range";
                return RedirectToAction("Index", "StopsSubmission", sid);
            }
            else
            {
                    submission.statusMsgs = entitiesdb.StatusMessage_JSON_vw
                            .Where(x => x.submissionID == sid && x.StopStatus != "success")
                            .ToList();
                    submission.subList = entitiesdb.Submissions
                                    .Where(x => x.StartDate == submission.StartDate &&
                                                x.EndDate == submission.EndDate).ToList();
            }
            bool fixedFlag = false;
            List<Stop> Stops = db.Stop.Where(x => x.SubmissionsID == submission.ID).ToList();
            foreach (Stop st in Stops)
            {
                JObject submissionO = JObject.Parse(st.JsonSubmissions);
                JObject lastSubmission = (JObject)submissionO["SubmissionInfo"].Last();
                int submissionID = (int)lastSubmission["submissionID"];
                bool edited = (bool)lastSubmission["edited"];
                if (submissionID == submission.ID && edited == true)
                {
                    fixedFlag = true;
                    break;
                }
            }
            //int stopsCount = entitiesdb.StopOfficerIDDateTime_JSON_vw.ToList()
            //    .Where(x => submission.StartDate <= Convert.ToDateTime(x.stopDate) && Convert.ToDateTime(x.stopDate) <= endDate).Count();

            int stopsCount = entitiesdb.StopOfficerIDDateTime_JSON_vw.ToList()
                .Join(db.Stop,
                j => j.ID,
                s => s.ID,
                (j, s) => new { StopOfficerIDDateTime_JSON_vw = j, Stop = s })
                .Where(x => submission.StartDate <= Convert.ToDateTime(x.StopOfficerIDDateTime_JSON_vw.stopDate) && Convert.ToDateTime(x.StopOfficerIDDateTime_JSON_vw.stopDate) <= endDate).Count();

            ViewBag.fixedFlag = fixedFlag;
            ViewBag.totalStops = stopsCount;

            return View(submission);
        }

        // POST: StopsSubmission/SubmissionStats
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmissionStats(Submissions submission, int? sid, DateTime startDate, DateTime? endDate)
        {
            UserAuth user = new UserAuth();

            user = AuthorizeUser(User.Identity.Name.ToString());

            if (!user.authorizedAdmin)
            {
                return RedirectToAction("Unauthorized", "Home");
            }

            if (ModelState.IsValid)
            {
                UserProfile_Conf uid = db.UserProfile_Conf.SingleOrDefault(x => x.NTUserName == User.Identity.Name.ToString());
                ViewBag.UserProfileID = uid.UserProfileID;
                ViewBag.admin = user.authorizedAdmin;

                // web.config debug setting
                ViewBag.debug = HttpContext.IsDebuggingEnabled;
                ViewBag.submissionEndDate = endDate;
                DOJSubmitController dOJSubmit = new DOJSubmitController();

                //Make sure the connection to DOJ url is available
                DOJSubmitController.connectionStatus connectStat = dOJSubmit.HTTP_Connection();
                if (!connectStat.connected)
                {
                    TempData["CustomError"] = connectStat.error;
                    return RedirectToAction("Index", "StopsSubmission", sid);
                }

                // If submission record in this date range is In Progress do not allow another submission
                Submissions submissionInProgress = entitiesdb.Submissions
                                .Where(x => x.StartDate == startDate && x.EndDate == endDate && x.Status == "In Progress")
                                .FirstOrDefault();

                if (submissionInProgress != null)
                {
                    TempData["CustomError"] = "A submission in this date range is already 'In Progress'";
                    return RedirectToAction("Index", "StopsSubmission", sid);
                }

                if (sid != 0)
                {
                    Submissions submissionOld = entitiesdb.Submissions.Find(sid);
                    submission = submissionOld;
                }
                submission.StartDate = startDate;
                if (endDate < submission.StartDate || endDate > DateTime.Now)
                {
                    TempData["CustomError"] = "Invalid Date Range";
                    return RedirectToAction("Index", "StopsSubmission", sid);
                }
                else
                {
                    //int stopsCount = entitiesdb.StopOfficerIDDateTime_JSON_vw.ToList()
                    //        .Where(x => submission.StartDate <= Convert.ToDateTime(x.stopDate) && Convert.ToDateTime(x.stopDate) < submission.EndDate).Count();
                    //ViewBag.totalToBeSubmitted = stopsCount;
                    bool fixedFlag = false;
                    List<Stop> Stops = db.Stop.Where(x => x.SubmissionsID == submission.ID).ToList();
                    // Check if any records have been edited
                    foreach (Stop st in Stops)
                    {
                        JObject submissionO = JObject.Parse(st.JsonSubmissions);
                        JObject lastSubmission = (JObject)submissionO["SubmissionInfo"].Last();
                        int submissionID = (int)lastSubmission["submissionID"];
                        bool edited = (bool)lastSubmission["edited"];
                        if (submissionID == submission.ID && edited == true)
                        {
                            fixedFlag = true;
                            break;
                        }
                    }
                    ViewBag.fixedFlag = fixedFlag;

                    // Change the status of the current submission record, with edited Stops, to "resumbit", 
                    // and create a new submission record to Resubmit all the fixed records again
                    if (Stops.Count != 0)
                    {
                        submission.Status = "Resubmit";
                        if (ModelState.IsValid)
                        {
                            entitiesdb.Entry(submission).State = EntityState.Modified;
                            await entitiesdb.SaveChangesAsync();
                        }
                        Submissions newSubmission = new Submissions();
                        newSubmission.StartDate = submission.StartDate;
                        submission = newSubmission;
                    }
                    submission.Status = "In Progress";
                    submission.DateSubmitted = DateTime.Now;
                    submission.EndDate = endDate;

                    var state = await dOJSubmit.GetStops(submission);
                    entitiesdb.Entry(submission).State = state;

                    if (submission.TotalProcessed == submission.TotalSuccess)
                    {
                        submission.Status = "Finished";
                    }
                    else
                    {
                        submission.Status = "Pending Fixes";
                        submission.statusMsgs = entitiesdb.StatusMessage_JSON_vw
                                .Where(x => x.submissionID == submission.ID && x.StopStatus != "success")
                                .ToList();
                        submission.subList = entitiesdb.Submissions.ToList();

                    }

                    if (ModelState.IsValid)
                    {
                        state = entitiesdb.Entry(submission).State;
                        await entitiesdb.SaveChangesAsync();

                        ViewBag.submissionID = submission.ID;
                    }

                }
            }
            else
            {
                TempData["CustomError"] = "End Date is required.";
            }


            return RedirectToAction("Index", "StopsSubmission", submission.ID);
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