using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HerbariumMVC.Models;
using System.Globalization;

namespace HerbariumMVC.Controllers
{
    public class HomeController : Controller
    {
        HerbariumDataAccess da = new HerbariumDataAccess();

        //*******************************************
        // GET: /Home/
        public ActionResult Index()
        {
            return View();
        }
        //*******************************************

        //*******************************************
        // GET: /Home/Edit/5

        public ActionResult Edit(String id)
        {
            // admin protected
            var admin = Session["admin"];
            if (admin == null || ((bool)admin) == false)
            {
                return RedirectToAction("Index");
            }

            HerbariumDataAccess da = new HerbariumDataAccess();
            User user = da.getUser(id);
            return View(user);
        }
        //*******************************************

        //*******************************************
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(User user, bool resetPassword)
        {
            // admin protected
            var admin = Session["admin"];
            if (admin == null || ((bool)admin) == false)
            {
                return RedirectToAction("Index");
            }

            if (resetPassword)
            {
                da.resetPassword(user);
            }
            try
            {
                HerbariumDataAccess da = new HerbariumDataAccess();
                da.saveUser(user);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // returns the view NewUser
        public ActionResult NewUser()
        {
            // admin protected
            var admin = Session["admin"];
            if (admin == null || ((bool)admin) == false)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        // when you fill out the form on NewUser this catches and creates a new user
        // adds a new user then redirects to the ViewUsers page
        [HttpPost]
        public ActionResult NewUser(User user)
        {
            // admin protected
            var admin = Session["admin"];
            if (admin == null || ((bool)admin) == false)
            {
                return RedirectToAction("Index");
            }

            // TODO: validate user, make sure this user doesn't already exist, etc.
            da.newUser(user);
            return RedirectToAction("ViewUsers");
        }

        //*******************************************

        //
        // GET: /Home/Details/5
        public ActionResult Details(int id)
        {
            // admin protected
            var admin = Session["admin"];
            if (admin == null || ((bool)admin) == false)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            // admin protected
            var admin = Session["admin"];
            if (admin == null || ((bool)admin) == false)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Header()
        {
            return PartialView();
        }



        // partial to display the login
        // the partial view will use session data to check if the user is logged in or not
        public ActionResult Login()
        {
            return PartialView();
        }






        // LoginForm is what handles the submit when someone fill out the login form
        [HttpPost]
        public ActionResult LoginForm(string username, string password)
        {

            // if they didn't enter any info redirect to home without logging in.
            if (username == null
                || username == ""
                || password == null
                || password == "")
            {
                return RedirectToAction("Index");
            }

            // query database to see if userName exists
            User signInUser = da.getUser(username);



            // if password is default the user still needs to set their password
            if (signInUser.password == "herbarium")
            {
                ViewBag.username = username;
                return View("SetPassword");
            }

            // if we find a user in the database with that userName
            // and password matches that userName
            if (signInUser != null && signInUser.password == da.CreateMD5Hash(password))
            {
                // start their session by logging them in
                // this allows the adding and editing specimen data
                Session["loggedIn"] = true;

                // If signInUser.admin == "True" 
                // there will be an "Admin" panel available
                if (signInUser.admin)
                {
                    Session["admin"] = signInUser.admin;
                }
                
                Session["username"] = signInUser.userName;


            }
            return RedirectToAction("Index");

        }


        // you get sent here if your password is "herbarium" in the database
        public ActionResult SetPassword()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SetPassword(SetPassword userToSet, string username)
        {
            List<string> errors = new List<string>();

            if (!userToSet.password1.Equals(userToSet.password2))
            {
                errors.Add("Entered passwords do not match");
            }

            if (userToSet.password1.Length < 6)
            {
                errors.Add("Password must be at least 6 characters long");
            }

            if (errors.Count() == 0) // no errors, set new password
            {
                bool result = da.setPassword(userToSet, username);
                ViewBag.result = result;
                ViewBag.username = username;
                return View("PasswordResetResult");
            }
            else // there were errors
            {
                ViewBag.errors = errors;
                return View("SetPassword");
            }

        }


        [HttpGet]
        public ActionResult EditASpecimen(string SpecimenGUID, string LocationGUID, string AIKey)
        {
            // login protected
            var loggedIn = Session["loggedIn"];
            if (loggedIn == null || ((bool)loggedIn) == false)
            {
                return RedirectToAction("Index");
            }

            string specimenGUID = SpecimenGUID;
            string locationGUID =  LocationGUID;
            int aikey = Int32.Parse(AIKey);

            List<SearchResultViewModel> results = da.getSingleSpecimen(specimenGUID, locationGUID, aikey);
            return View(results.First());
        }

        [HttpPost]
        public ActionResult EditASpecimen(SearchResultViewModel entry)
        {
            string imageNameWithoutPath = entry.ImageName;
            // login protected
            var loggedIn = Session["loggedIn"];
            if (loggedIn == null || ((bool)loggedIn) == false)
            {
                return RedirectToAction("Index");
            }

            SearchResultViewModel entryToPass = entry;

            entryToPass.ImageName = Request.MapPath("~/Images/" + entry.ImageName);

            // returns true if successfully updated
            if (da.updateSpecimen(entryToPass, imageNameWithoutPath))
            {
                return View ("EditASpecimenSuccess");
            }
            else
            {
                return View("EditASpecimemFailed");
            }
        }




        public ActionResult Search(String search, bool multipleSearch)
        {
            ViewBag.search = search; // for testing purposes

            Session["search"] = search;

            // array to store each individual word in the search string
            string[] searches = { };
            // copy for manipulation, also forces lower case
            string searchString = search.ToLower();

            // remove a few weird symbols
            searchString = searchString.Replace(",", " ");
            searchString = searchString.Replace(".", " ");
            searchString = searchString.Replace("!", " ");
            searchString = searchString.Replace("?", " ");
            searchString = searchString.Replace("@", " ");
            searchString = searchString.Replace("&", " ");
            searchString = searchString.Replace("{", " ");
            searchString = searchString.Replace("}", " ");
            searchString = searchString.Trim();

            Session["search"] = searchString;

            if (searchString != null && !searchString.Equals(""))
            {
                // split the search terms into an array of individual words
                searches = searchString.Split(' ');

                // remove white space from each word just to be safe
                for (int i=0; i<searches.Length; i++)
                {
                    searches[i] = searches[i].Trim();
                }

                // now that the words have been steralized lets do the search for real

                // get all specimens to search through
                List<SearchResultViewModel> specimens = da.getAllSpecimensWithAcquisitionAndLocationData(multipleSearch);


                // do this for each word
                foreach (string word in searches)
                {
                    // check each row in the table to see if it has this word
                    // if the word is nowhere to be seen in a specimen's data
                    // delete the specimen from the results
                    // word should be lower case so lets make sure we check against only lower case
                    // should have been entered into database lower, but lets just be sure
                    // just comment out the fields you don't want to search on (will reduce rows returned)
                    for (int i = specimens.Count - 1; i >= 0; i--)
                    {
                        if ( 
                              !(
                                    specimens[i].Family           .Contains(word) || word.Equals(specimens[i].Family.ToLower()           )
                                 || specimens[i].Genus            .Contains(word) || word.Equals(specimens[i].Genus.ToLower()            )
                                 || specimens[i].Species          .Contains(word) || word.Equals(specimens[i].Species.ToLower()          )
                               //|| specimens[i].SpecimenGUID     .Contains(word) || word.Equals(specimens[i].SpecimenGUID     .ToLower())
                                 || specimens[i].SubSpecies       .Contains(word) || word.Equals(specimens[i].SubSpecies       .ToLower())
                                 || specimens[i].Author           .Contains(word) || word.Equals(specimens[i].Author           .ToLower())
                                 || specimens[i].SubAuthor        .Contains(word) || word.Equals(specimens[i].SubAuthor        .ToLower())
                                 || specimens[i].Community        .Contains(word) || word.Equals(specimens[i].Community        .ToLower())
                                 || specimens[i].SoilType         .Contains(word) || word.Equals(specimens[i].SoilType         .ToLower())
                                 || specimens[i].SoilColor        .Contains(word) || word.Equals(specimens[i].SoilColor        .ToLower())
                                 || specimens[i].SoilMoisture     .Contains(word) || word.Equals(specimens[i].SoilMoisture     .ToLower())
                               //|| specimens[i].SIKey.ToString() ==         word || word.Equals(specimens[i].SIKey.ToString()           )
                                 || specimens[i].AcquisitionNo.ToString() == word || word.Equals(specimens[i].AcquisitionNo.ToString()   )
                                 || specimens[i].EthnobotanicalUse.Contains(word) || word.Equals(specimens[i].EthnobotanicalUse.ToLower())
                                 || specimens[i].CollectedDate.ToString() == word || word.Equals(specimens[i].CollectedDate.ToString()   )
                                 || specimens[i].CollectorNo      .Contains(word) || word.Equals(specimens[i].CollectorNo      .ToLower())
                                 || specimens[i].Notes            .Contains(word) || word.Equals(specimens[i].Notes            .ToLower())
                                 || specimens[i].CollectorName    .Contains(word) || word.Equals(specimens[i].CollectorName    .ToLower())
                               //|| specimens[i].Deleted.ToString() == word       || word.Equals(specimens[i].Deleted.ToString()         )
                               //|| specimens[i].AIKey.ToString() == word         || word.Equals(specimens[i].AIKey.ToString()           )
                                 || specimens[i].UserName         .Contains(word) || word.Equals(specimens[i].UserName         .ToLower())
                               //|| specimens[i].LocationGUID     .Contains(word) || word.Equals(specimens[i].LocationGUID     .ToLower())
                                 || specimens[i].Lat.ToString() == word           || word.Equals(specimens[i].Lat.ToString()             )
                                 || specimens[i].Lng.ToString() == word           || word.Equals(specimens[i].Lng.ToString()             )
                                 || specimens[i].Elevation.ToString() == word     || word.Equals(specimens[i].Elevation.ToString()       )
                                 || specimens[i].Country          .Contains(word) || word.Equals(specimens[i].Country          .ToLower())
                                 || specimens[i].County           .Contains(word) || word.Equals(specimens[i].County           .ToLower())
                                 || specimens[i].State            .Contains(word) || word.Equals(specimens[i].State            .ToLower())
                               //|| specimens[i].LocationDesc     .Contains(word) || word.Equals(specimens[i].LocationDesc     .ToLower())
                                 || specimens[i].LIKey.ToString() == word         || word.Equals(specimens[i].LIKey.ToString()           )
                                 || specimens[i].Township         .Contains(word) || word.Equals(specimens[i].Township         .ToLower())
                               //|| specimens[i].Sect             .Contains(word) || word.Equals(specimens[i].Sect             .ToLower())
                               //|| specimens[i].Range            .Contains(word) || word.Equals(specimens[i].Range            .ToLower())
                               ) 
                            )
                        {
                            specimens.RemoveAt(i);
                        }
                    }
                }

                // Creates a TextInfo based on the "en-US" culture.
                // this helps the TextInfo convert strings to proper capitalization
                TextInfo myTI = new CultureInfo("en-US", false).TextInfo;

                foreach (var item in specimens)
                {
                    item.Family             = myTI.ToTitleCase(item.Family);
                    item.Genus              = myTI.ToTitleCase(item.Genus);
                    item.Species            = myTI.ToTitleCase(item.Species);
                    item.SubSpecies         = myTI.ToTitleCase(item.SubSpecies);
                    item.Author             = myTI.ToTitleCase(item.Author);
                    item.SubAuthor          = myTI.ToTitleCase(item.SubAuthor);
                    item.Community          = myTI.ToTitleCase(item.Community);
                    item.SoilType           = myTI.ToTitleCase(item.SoilType);
                    item.SoilColor          = myTI.ToTitleCase(item.SoilColor);
                    item.SoilMoisture       = myTI.ToTitleCase(item.SoilMoisture);
                    item.CollectorName      = myTI.ToTitleCase(item.CollectorName);
                    item.Country            = myTI.ToTitleCase(item.Country);
                    item.County             = myTI.ToTitleCase(item.County);
                    item.State              = myTI.ToTitleCase(item.State);
                    item.Township           = myTI.ToTitleCase(item.Township);
                }

                ViewBag.resultCount = specimens.Count;

                return View(specimens);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult ViewImage(string ImageName)
        {
            ViewBag.ImageName = ImageName;
            return View();
        }

        public ActionResult DeleteASpecimen(string LocationGUID, string ImageName)
        {
            string ImageNameWithFullPath = " ";

            if (ImageName != " " && !String.IsNullOrEmpty(ImageName))
            {
                // tried to do this step inside the da.deleteASpecimen() function
                // but Request.MapPath() is a Controller method and da is not a controller
                // just easier to do the string manipulation here since this is a controller
                ImageNameWithFullPath = Request.MapPath("~/Images/" + ImageName);
            }
            da.deleteASpecimen(LocationGUID, ImageNameWithFullPath);

            ViewBag.search = Session["search"];
            return View("DeleteASpecimenSuccess");
        }

        public ActionResult ViewYourOwnEntries()
        {
            // login protected
            var loggedIn = Session["loggedIn"];
            if (loggedIn == null || ((bool)loggedIn) == false)
            {
                return RedirectToAction("Index");
            }

            List<SearchResultViewModel> specimens = da.getAllSpecimensWithAcquisitionAndLocationData(false);
            for (int i = specimens.Count - 1; i >= 0; i--)
            {
                if (specimens[i].UserName != (string) Session["username"])
                {
                    specimens.RemoveAt(i);
                }
            }
            
            return View(specimens);
        }



        public ActionResult ViewUsers()
        {
            // admin protected
            var admin = Session["admin"];
            if (admin == null || ((bool)admin) == false)
            {
                return RedirectToAction("Index");
            }

            List<User> users = da.getAllUsers();

            return View(users);
        }

        public ActionResult NewDatabaseEntry()
        {
            // login protected
            var loggedIn = Session["loggedIn"];
            if (loggedIn == null || ((bool) loggedIn) == false)
            {
                return RedirectToAction("Index");
            }

            ViewBag.repeat = false;

            return View();
        }

        [HttpPost]
        public ActionResult NewDatabaseEntry(NewDatabaseSpecimenEntry entry, string Save, string Username)
        {
            var loggedIn = Session["loggedIn"];
            if (loggedIn == null || ((bool)loggedIn) == false)
            {
                return RedirectToAction("Index");
            }

            if (Save.Equals("Save and Print Labels"))
            {
                da.newSpecimen(entry, Username);

                ViewBag.entry = entry;
                return RedirectToAction("PrintLabels", entry);
            }

            else if (Save.Equals("Just Print Labels"))
            {
                ViewBag.entry = entry;
                return RedirectToAction("PrintLabels", entry);
            }

            else if (Save.Equals("Save with No Printing"))
            {
                da.newSpecimen(entry, Username);
            }

            else if (Save.Equals("Cancel"))
            {
                return RedirectToAction("NewDatabaseEntry");
            }

            ViewBag.repeat = true;

            return View("NewDatabaseEntry", entry);
        }

        public ActionResult PrintLabels(NewDatabaseSpecimenEntry entry)
        {
            return View(entry);
        }

        public ActionResult Logout()
        {
            Session["admin"] = false;
            Session["loggedIn"] = false;
            Session["username"] = "";
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index");
        }
    }
}
