using RescuePetHomework02.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;

namespace RescuePetHomework02.Controllers
{
    public class HomeController : Controller
    {
        private RescuePetHW02Entities1 db = new RescuePetHW02Entities1();
        public ActionResult Index()
        {
            using (db)
            {
                var adoptions = db.Adoptions
                    .Include(a => a.User)
                    .Include(a => a.Pet)
                    .ToList();

                return View(adoptions);
            }
        }

        public ActionResult Pets(int? typeId, int? breedId, int? locationId)
        {
            using (db)
            {
                var loadpet = db.Pets
                    .Include(p => p.User)
                    .Include(p => p.Location)
                    .Include(p => p.Gender)
                    .Include(p => p.Status)
                    .Include(p => p.Type)
                    .Include(p => p.Breed)
                    .Where(p => p.PetID >= 1);

                if (typeId.HasValue)
                {
                    loadpet = loadpet.Where(p => p.Type.TypeID == typeId.Value);
                }

                if (breedId.HasValue)
                {
                    loadpet = loadpet.Where(p => p.Breed.BreedID == breedId.Value);
                }

                if (locationId.HasValue)
                {
                    loadpet = loadpet.Where(p => p.Location.LocationID == locationId.Value);
                }

                
                var pets = loadpet.Select(p => new PetViewModel
                {
                    PetID = p.PetID,
                    Name = p.Name,
                    Image = p.Image,
                    StatusName = p.Status.Name,
                    TypeName = p.Type.Name,
                    BreedName = p.Breed.Name,
                    LocationName = p.Location.Name,
                    UserName = p.User.Name,
                    GenderName = p.Gender.Name,
                    Age = p.Age ?? 0,
                    Weight = (double)(p.Weight ?? 0),
                    Story = p.Story
                }).ToList();

                return View(pets);
            }
        }

        public JsonResult FilterBreedsByType(int? typeId)
        {
            using (db)
            {
                var breeds = db.Breeds
                    .Where(b => !typeId.HasValue || b.TypeID == typeId.Value)
                    .Select(b => new { b.BreedID, b.Name })
                    .ToList();

                return Json(breeds, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Adopt(int? petId)
        {
            var selectedPet = GetPetById(petId);

            if (selectedPet == null)
            {
                return HttpNotFound();
            }

            PopulateDropdowns();

            return View(selectedPet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateAdoptionStatus(int petId, int selectedUserId)
        {
            try
            {
                var selectedPet = GetPetById(petId);

                if (selectedPet == null)
                {
                    return HttpNotFound();
                }

                selectedPet.StatusID = 2;
                CreateAdoption(selectedUserId, selectedPet.PetID);

                TempData["Message"] = "Status changed successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while changing the status: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        private Pet GetPetById(int? petId)
        {
            return db.Pets.Find(petId);
        }

        private void PopulateDropdowns()
        {
            ViewBag.FullNameDropdown = GetFullNameDropdown();
            ViewBag.PhoneDropdown = GetPhoneDropdown();
            ViewBag.UsersID = GetUsersDropdown();
        }

        private SelectList GetFullNameDropdown()
        {
            return new SelectList(db.Users, "UserID", "Name");
        }

        private SelectList GetPhoneDropdown()
        {
            return new SelectList(db.Users, "UserID", "PhoneNumber");
        }

        private SelectList GetUsersDropdown()
        {
            return new SelectList(db.Users, "UserID", "UserID");
        }

        private void CreateAdoption(int selectedUserId, int petId)
        {
            var adoption = new Adoption
            {
                UserID = selectedUserId,
                PetID = petId,
            };

            db.Adoptions.Add(adoption);
            db.SaveChanges();
        }

        public ActionResult Donation()
        {
            var donationViewModel = new DonationViewModel
            {
                Users = new SelectList(db.Users, "Name", "Name"),
                PhoneList = new SelectList(db.Users.Select(u => u.PhoneNumber).ToList()),
                TotalRaised = (decimal)db.Donations.Sum(d => d.Amount),
            };
            donationViewModel.Percentage = (donationViewModel.TotalRaised * 100) / 750000;
            donationViewModel.Status = Math.Round(donationViewModel.Percentage).ToString() + "%";

            return View(donationViewModel);
        }

        [HttpPost]
        public ActionResult Donation(DonationViewModel donationViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var selectedUser = db.Users.FirstOrDefault(u => u.Name == donationViewModel.SelectedUserName);

                    if (selectedUser != null)
                    {
                        var donation = new Donation
                        {
                            UserID = selectedUser.UserID,
                            Amount = donationViewModel.Amount
                        };

                        db.Donations.Add(donation);
                        db.SaveChanges();

                        donationViewModel.TotalRaised = (decimal)db.Donations.Sum(d => d.Amount);

                        donationViewModel.Percentage = (donationViewModel.TotalRaised * 100) / 750000;
                        donationViewModel.Status = Math.Round(donationViewModel.Percentage).ToString() + "%";

                        return RedirectToAction("Donation", donationViewModel);
                    }
                    else
                    {
                        ModelState.AddModelError("SelectedUserName", "Invalid user selected.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while processing the donation. Please try again.");
                }
            }

            donationViewModel.Users = new SelectList(db.Users, "Name", "Name");
            donationViewModel.PhoneList = new SelectList(db.Users.Select(u => u.PhoneNumber).ToList());

            return View(donationViewModel);
        }

        public ActionResult Post()
        {
            ViewBag.PhoneNumbers = new SelectList(db.Users.Select(u => u.PhoneNumber).Distinct());
            ViewBag.PetTypes = new SelectList(db.Types, "TypeID", "Name");
            ViewBag.Locations = new SelectList(db.Locations, "LocationID", "Name");
            ViewBag.Genders = new SelectList(db.Genders, "GenderID", "Name");
            ViewBag.Breeds = new SelectList(db.Breeds, "BreedID", "Name");
            ViewBag.Users = new SelectList(db.Users, "UserID", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Post(PostAPetViewModel newPet)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var pet = new Pet
                    {
                        Name = newPet.PetName,
                        Story = newPet.PetStory,
                        GenderID = newPet.Gender,
                        TypeID = newPet.PetType,
                        UserID = newPet.UserId,
                        BreedID = newPet.Breed,
                        LocationID = newPet.Location,
                        Age = newPet.Age,
                        Weight = newPet.Weight,
                        Image = newPet.PetImage.FileName,
                        StatusID = 1
                    };

                    db.Pets.Add(pet);
                    db.SaveChanges();

                    return RedirectToAction("Pets");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the pet. Please try again.");
                }
            }

            ViewBag.PhoneNumbers = new SelectList(db.Users.Select(u => u.PhoneNumber).Distinct());
            ViewBag.PetTypes = new SelectList(db.Types, "TypeID", "Name");
            ViewBag.Locations = new SelectList(db.Locations, "LocationID", "Name");
            ViewBag.Genders = new SelectList(db.Genders, "GenderID", "Name");
            ViewBag.Breeds = new SelectList(db.Breeds, "BreedID", "Name");
            ViewBag.Users = new SelectList(db.Users, "UserID", "Name");

            return View(newPet);
        }
    }
}