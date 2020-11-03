using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RPG.Web.Extensions;
using RPG.Web.Models;
using RPG.Web.ViewModels;

namespace RPG.Web.Controllers
{
    public class GameController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWorkRepository _workRepository;
        private readonly ITavernRepository _tavernRepository;
        private readonly IMailRepository _mailRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IInventoryRepository _inventoryRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly ITripRepository _tripRepository;

        public GameController(UserManager<ApplicationUser> userManager, 
            IWorkRepository workRepository, ITavernRepository tavernRepository, 
            IMailRepository mailRepository, IItemRepository itemRepository,
            IInventoryRepository inventoryRepository, ILocationRepository locationRepository,
            ITripRepository tripRepository)
        {
            _userManager = userManager;
            _workRepository = workRepository;
            _tavernRepository = tavernRepository;
            _mailRepository = mailRepository;
            _itemRepository = itemRepository;
            _inventoryRepository = inventoryRepository;
            _locationRepository = locationRepository;
            _tripRepository = tripRepository;
        }

        [HttpGet]
        [Route("[controller]/Rank")]
        [Route("[controller]/UsersRank")]
        public ViewResult UsersRank(string sortBy, string orderBy)
        {
            var model = new RankViewModel()
            {
                SortBy = sortBy ?? "AllExperience",
                OrderBy = orderBy ?? (sortBy == "UserName" ? "ASC" : "DESC")
            };

            List<string> sort = new List<string>
            {
                "UserName", "Level", "AllExperience", "Strength", "Dexterity", "Stamina", "Intelligence"
            };

            List<string> order = new List<string>
            {
                "ASC", "DESC"
            };

            if(!sort.Any(x => x.Contains(model.SortBy)))
            {
                model.SortBy = "AllExperience";
            }

            if(!order.Any(x => x.Contains(model.OrderBy)))
            {
                model.OrderBy = model.SortBy == "UserName" ? "ASC" : "DESC";
            }

            model.Users = _userManager.Users.OrderBy($"{model.SortBy} {model.OrderBy}");
            
            return View(model);
        }

        [HttpGet]
        public async Task<ViewResult> Area(string sortBy, string orderBy)
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new RankViewModel()
            {
                SortBy = sortBy ?? "AllExperience",
                OrderBy = orderBy ?? (sortBy == "UserName" ? "ASC" : "DESC")
            };

            List<string> sort = new List<string>
            {
                "UserName", "Level", "AllExperience", "Strength", "Dexterity", "Stamina", "Intelligence"
            };

            List<string> order = new List<string>
            {
                "ASC", "DESC"
            };

            if(!sort.Any(x => x.Contains(model.SortBy)))
            {
                model.SortBy = "AllExperience";
            }

            if(!order.Any(x => x.Contains(model.OrderBy)))
            {
                model.OrderBy = model.SortBy == "UserName" ? "ASC" : "DESC";
            }

            model.Users = _userManager.Users.OrderBy($"{model.SortBy} {model.OrderBy}")
                                            .Where(x => x.PositionX == user.PositionX)
                                            .Where(x => x.PositionY == user.PositionY);
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpgradeSkills(string id)
        {
            if(id == "Strength" || id == "Dexterity" || id == "Stamina" || id == "Intelligence")
            {
                var user = await _userManager.GetUserAsync(User);

                if(user.SkillPoints > 0)
                {
                    user.SkillPoints--;
                    switch(id)
                    {
                        case "Strength":
                            user.Strength++;
                            break;
                        case "Dexterity":
                            user.Dexterity++;
                            break;
                        case "Stamina":
                            user.Stamina++;
                            user.MaxHealthPoints+=10;
                            user.MaxActionPoints++;
                            break;
                        case "Intelligence":
                            user.Intelligence++;
                            break;
                    }
                    await _userManager.UpdateAsync(user);
                }
            }

            var referer = Request.Headers["Referer"];
            
            if(string.IsNullOrEmpty(referer))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return Redirect(referer);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Medic()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new MedicViewModel();

            if(user.HealthPoints == user.MaxHealthPoints){
                model.FullHealth = true;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Medic(MedicViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if(user.Status != UserStatus.Stays)
                {
                    ModelState.AddModelError(string.Empty, "Aktualnie nie możesz się uleczyć.");
                }
                else if(model.Points < 1)
                {
                    ModelState.AddModelError(string.Empty, "Podano nieprawidłową wartość.");
                }
                else if(model.Points > user.MaxHealthPoints - user.HealthPoints)
                {
                    ModelState.AddModelError(string.Empty, "Podano za dużą wartość.");
                }
                else if(model.Points * 10 > user.Gold)
                {
                    ModelState.AddModelError(string.Empty, "Nie masz tyle złota.");
                }
                else 
                {
                    user.Gold-=(int)model.Points*10;
                    user.HealthPoints+=(int)model.Points;
                    await _userManager.UpdateAsync(user);
                }

                if(user.HealthPoints == user.MaxHealthPoints){
                    model.FullHealth = true;
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Work()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new WorkViewModel();

            if(user.Status == UserStatus.Work)
            {
                var work = _workRepository.GetWork(user.Id);

                model.Work = work;
                model.WorkProgress = true;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Work(WorkViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if(user.Status != UserStatus.Stays)
                {
                    ModelState.AddModelError(string.Empty, "Aktualnie nie możesz pracować.");
                }
                else if(user.ActionPoints < model.Work.Hours)
                {
                    ModelState.AddModelError(string.Empty, "Nie masz siły na taką pracę!");
                }
                else
                {
                    var work = new Work() 
                    {
                        UserId = user.Id,
                        Start = DateTime.UtcNow,
                        End = DateTime.UtcNow.AddHours(model.Work.Hours),
                        Hours = model.Work.Hours
                    };

                    user.Status = UserStatus.Work;
                    await _userManager.UpdateAsync(user);

                    _workRepository.Add(work);

                    model.Work = work;
                    model.WorkProgress = true;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StopWork()
        {
            var user = await _userManager.GetUserAsync(User);
            user.Status = UserStatus.Stays;
            await _userManager.UpdateAsync(user);
            _workRepository.Delete(user.Id);

            return RedirectToAction("Work");
        }

        [HttpGet]
        public async Task<IActionResult> Tavern()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new TavernViewModel()
            {
                TimeToMaxPoints = (float)(user.MaxActionPoints - user.ActionPoints) / 2
            };

            if(user.Status == UserStatus.Tavern)
            {
                var tavern = _tavernRepository.GetTavern(user.Id);

                model.Tavern = tavern;
                model.TavernProgress = true;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Tavern(TavernViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                model.TimeToMaxPoints = (float)(user.MaxActionPoints - user.ActionPoints) / 2;

                if(user.Status != UserStatus.Stays)
                {
                    ModelState.AddModelError(string.Empty, "Aktualnie nie możesz wybrać się do tawerny.");
                } 
                else if(user.ActionPoints >= user.MaxActionPoints)
                {
                    ModelState.AddModelError(string.Empty, "Twoja postać jest w pełni sił.");
                }
                else 
                {
                    var hours = model.Tavern.Hours ?? model.TimeToMaxPoints;

                    var tavern = new Tavern()
                    {
                        UserId = user.Id,
                        Start = DateTime.UtcNow,
                        End = DateTime.UtcNow.AddHours(hours),
                        Hours = hours
                    };

                    user.Status = UserStatus.Tavern;
                    await _userManager.UpdateAsync(user);

                    _tavernRepository.Add(tavern);

                    model.Tavern = tavern;
                    model.TavernProgress = true;
                }
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> StopTavern()
        {
            var user = await _userManager.GetUserAsync(User);
            user.Status = UserStatus.Stays;
            await _userManager.UpdateAsync(user);
            _tavernRepository.Delete(user.Id);

            return RedirectToAction("Tavern");
        }

        [HttpGet]
        public async Task<IActionResult> Stats(string id)
        {
            var user = id == null ? await _userManager.GetUserAsync(User) : await _userManager.FindByIdAsync(id);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Mail()
        {
            var user = await _userManager.GetUserAsync(User);
            var receivedMails = _mailRepository.GetMails(user.Id, MailType.Received).OrderByDescending(x => x.Id);

            return View(receivedMails);
        }

        [HttpGet]
        public async Task<IActionResult> SentMail()
        {
            var user = await _userManager.GetUserAsync(User);
            var sentMails = _mailRepository.GetMails(user.Id, MailType.Sent).OrderByDescending(x => x.Id);

            return View(sentMails);
        }

        [HttpGet]
        public async Task<IActionResult> NewMail(string id)
        {
            var toUser = await _userManager.FindByIdAsync(id);
            var model = new NewMailViewModel();

            if(toUser != null)
            {
                model.UserName = toUser.UserName;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewMail(NewMailViewModel model)
        {
            if(ModelState.IsValid){
                var toUser = await _userManager.FindByNameAsync(model.UserName);

                if(toUser == null)
                {
                    ModelState.AddModelError(string.Empty, "Nie ma takiego gracza.");
                }
                else
                {
                    var user = await _userManager.GetUserAsync(User);
                    var receivedMail = new Mail()
                    {
                        FromId = user.Id,
                        ToId = toUser.Id,
                        Type = MailType.Received,
                        Title = model.Title,
                        Content = model.Content,
                        Status = MailStatus.New
                    };
                    var sentMail = new Mail()
                    {
                        FromId = user.Id,
                        ToId = toUser.Id,
                        Type = MailType.Sent,
                        Title = model.Title,
                        Content = model.Content,
                        Status = MailStatus.New
                    };

                    _mailRepository.Add(receivedMail);
                    _mailRepository.Add(sentMail);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteMail(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var mails = _mailRepository.GetMails(user.Id, MailType.Received);
            var mail = mails.FirstOrDefault(x => x.Id == id);
            
            if(mail == null)
            {
                ViewBag.ErrorMessage = $"Nie można znaleźć odebranego maila o Id: '{id}'.";
                return View("NotFound");
            }

            _mailRepository.Delete(mail.Id);

            return RedirectToAction("Mail");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteSentMail(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var mails = _mailRepository.GetMails(user.Id, MailType.Sent);
            var mail = mails.FirstOrDefault(x => x.Id == id);
            
            if(mail == null)
            {
                ViewBag.ErrorMessage = $"Nie można znaleźć wysłanego maila o Id: '{id}'.";
                return View("NotFound");
            }

            _mailRepository.Delete(mail.Id);

            return RedirectToAction("SentMail");
        }

        [HttpGet]
        [Route("[controller]/Shop")]
        [Route("[controller]/WeaponShop")]
        public async Task<IActionResult> WeaponShop()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new ShopViewModel()
            {
                Items = _itemRepository.GetItems(ItemType.Weapon).ToList(),
                Inventory = _inventoryRepository.GetInventory(user.Id).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ArmorShop()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new ShopViewModel()
            {
                Items = _itemRepository.GetItems(ItemType.Armor).ToList(),
                Inventory = _inventoryRepository.GetInventory(user.Id).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ShopBuy(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var item = _itemRepository.GetItem(id);
            if(item == null)
            {
                ViewBag.ErrorMessage = $"Nie można znaleźć przedmiotu o Id: '{id}'.";
                return View("NotFound");
            }

            var inventory = _inventoryRepository.GetInventory(user.Id);

            if(inventory.Count() > 13)
            {
                ModelState.AddModelError(string.Empty, "Nie pomieścisz już więcej w plecaku.");
            }
            else if(item.Cost > user.Gold)
            {
                ModelState.AddModelError(string.Empty, "Nie masz wystarczającej ilości złota.");
            }
            else
            {
                user.Gold -= item.Cost;
                await _userManager.UpdateAsync(user);
                
                _inventoryRepository.Add(new Inventory()
                {
                    ItemId = item.Id,
                    UserId = user.Id
                });
            }

            return RedirectToAction("WeaponShop");
        }

        [HttpGet]
        public async Task<IActionResult> ShopSell(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var inventory = _inventoryRepository.GetInventory(user.Id).FirstOrDefault(x => x.Id == id);

            if(inventory == null)
            {
                ModelState.AddModelError(string.Empty, "Brak przedmiotu.");
            }
            else if(inventory.Used == true)
            {
                ModelState.AddModelError(string.Empty, "Przedmiot używany.");
            }
            else
            {
                var item = _itemRepository.GetItem(inventory.ItemId);
                user.Gold += item.Cost;
                await _userManager.UpdateAsync(user);

                _inventoryRepository.Delete(inventory.Id);
            }

            return RedirectToAction("WeaponShop");
        }

        [HttpGet]
        public async Task<IActionResult> Inventory(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _inventoryRepository.GetInventory(user.Id);

            if(id != null)
            {
                var inventory = model.FirstOrDefault(x => x.Id == id);

                if(inventory.Used)
                {
                    inventory.Used = false;
                }
                else if(inventory.Stamina < 1)
                {
                    ModelState.AddModelError(string.Empty, "Ten przedmiot jest zniszczony.");
                }
                else
                {
                    var isWearing = false;
                    foreach(var used in model.Where(x => x.Used == true).ToList())
                    {
                        if(_itemRepository.GetItem(used.ItemId).Type == _itemRepository.GetItem(inventory.ItemId).Type)
                        {
                            isWearing = true;
                            break;
                        }
                    }

                    if(isWearing == true)
                    {
                        ModelState.AddModelError(string.Empty, "Postać już ma założony jakiś przedmiot.");
                    }
                    else
                    {
                        inventory.Used = true;
                    }
                }

                _inventoryRepository.Update(inventory);
                
                return RedirectToAction("Inventory", new { id = "" });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Blacksmith(int? id)
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _inventoryRepository.GetInventory(user.Id).Where(x => x.Used == false);

            if(id != null)
            {
                var inventory = model.FirstOrDefault(x => x.Id == id);
                user.Gold -= 100 - inventory.Stamina;
                inventory.Stamina = 100;

                _inventoryRepository.Update(inventory);
                await _userManager.UpdateAsync(user);

                return RedirectToAction("Blacksmith", new { id = "" });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Map(int? x, int? y)
        {
            var user = await _userManager.GetUserAsync(User);

            if(user.Status == UserStatus.Trip)
            {
                return RedirectToAction("Trip");
            }

            var model = new MapViewModel()
            {
                Locations = _locationRepository.GetLocations(),
                X = x ?? 4,
                Y = y ?? 1
            };

            return View(model);
        }

        [HttpGet]
        public ViewResult Location(int id)
        {
            var location = _locationRepository.GetLocations().SingleOrDefault(x => x.Id == id);

            if(location == null)
            {
                ViewBag.ErrorMessage = $"Nie można znaleźć lokacji o Id: '{id}'.";
                return View("NotFound");
            }

            return View(location);
        }

        [HttpGet]
        public async Task<IActionResult> Trip(int? id)
        {
            var user = await _userManager.GetUserAsync(User);

            if(id == null)
            {
                if(user.Status != UserStatus.Trip) 
                    return RedirectToAction("Map");
                else
                {
                    var trip = _tripRepository.GetTrip(user.Id);

                    return View(trip);
                }
            }
            else
            {
                if(user.Status == UserStatus.Trip)
                    return RedirectToAction("Trip", new { id = "" });
                else if(user.Status != UserStatus.Stays)
                    return RedirectToAction("Map");

                var location = _locationRepository.GetLocations().SingleOrDefault(x => x.Id == id);

                if(location == null)
                {
                    ViewBag.ErrorMessage = $"Nie można znaleźć lokacji o Id: '{id}'.";
                    return View("NotFound");
                }

                var distance = Math.Round(Math.Sqrt(Math.Pow((user.PositionX - location.X), 2) + Math.Pow((user.PositionY - location.Y), 2)) * 100, 1);

                var trip = new Trip()
                {
                    UserId = user.Id,
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow + TimeSpan.FromSeconds((distance / 100) * 300),
                    X = location.X,
                    Y = location.Y,
                    Distance = (int)distance
                };

                user.Status = UserStatus.Trip;
                await _userManager.UpdateAsync(user);

                _tripRepository.Add(trip);

                return RedirectToAction("Trip");
            }
        }


        [HttpPost]
        public async Task<IActionResult> StopTrip()
        {
            var user = await _userManager.GetUserAsync(User);
            user.Status = UserStatus.Stays;
            await _userManager.UpdateAsync(user);
            _tripRepository.Delete(user.Id);

            return RedirectToAction("Map");
        }


        [HttpGet]
        public async Task<IActionResult> Arena(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var model = new ArenaViewModel();
            if(user != null)
            {
                model.UserName = user.UserName;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Arena(ArenaViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                if(user.Status != UserStatus.Stays)
                {
                    ModelState.AddModelError(string.Empty, "Aktualnie nie możesz walczyć na arenie.");
                } 
                else if(model.UserName == user.UserName)
                {
                    ModelState.AddModelError(string.Empty, "Nie możesz walczyć ze sobą.");
                }
                else if(user.ActionPoints < 1)
                {
                    ModelState.AddModelError(string.Empty, "Nie masz siły na walkę na arenie.");
                }
                else
                {
                    var enemy = await _userManager.FindByNameAsync(model.UserName);
                    
                    if(enemy == null)
                    {
                        ModelState.AddModelError(string.Empty, "Podany gracz nie istnieje.");
                    } 
                    else if(user.PositionX != enemy.PositionX || user.PositionY != enemy.PositionY)
                    {
                        ModelState.AddModelError(string.Empty, "Tego gracza nie ma w tym miejscu.");
                    }
                    else if(enemy.Status != UserStatus.Stays)
                    {
                        ModelState.AddModelError(string.Empty, "Podany gracz jest teraz czymś zajęty.");
                    }
                    else if(user.HealthPoints < user.MaxHealthPoints * 0.25)
                    {
                        ModelState.AddModelError(string.Empty, "Jesteś rany. Ulecz się zanim podejmiesz walkę.");
                    }
                    else if(enemy.HealthPoints < enemy.MaxHealthPoints * 0.25)
                    {
                        ModelState.AddModelError(string.Empty, "Przeciwnik jest ranny. Nie możesz go zaatakować.");
                    }
                    else
                    {
                        var arenaResultViewModel = new ArenaResultViewModel()
                        {
                            User = user,
                            Enemy = enemy
                        };

                        int userHealth = user.HealthPoints;
                        int enemyHealth = enemy.HealthPoints;

                        int i = 0;
                        int userScore = 0;
                        int enemyScore = 0;
                        Random rand = new Random();

                        while(user.HealthPoints > 0 && enemy.HealthPoints > 0)
                        {
                            i++;
                            var roundInfo = string.Empty;

                            if(enemy.GetDodge() > rand.NextDouble() * 100)
                            {
                                roundInfo += "<div class=\"alert alert-success\" role=\"alert\">Spudłowałeś.</div>";
                            }
                            else
                            {
                                ItemWeapon weapon = null;
                                foreach(var used in _inventoryRepository.GetInventory(user.Id).Where(x => x.Used == true).ToList())
                                {
                                    var item = _itemRepository.GetItem(used.ItemId);
                                    if(item.Type == ItemType.Weapon)
                                    {
                                        weapon = (ItemWeapon)item.ItemType;
                                        break;
                                    }
                                }

                                var userAtt = rand.Next(user.GetDamage() - 1, user.GetDamage() + 2);
                                if(weapon != null)
                                {
                                    userAtt = rand.Next(user.GetDamage() + weapon.MinDamage, user.GetDamage() + weapon.MaxDamage);
                                }

                                if(user.GetCritical() >= rand.NextDouble() * 100)
                                {
                                    roundInfo += "<div class=\"alert alert-success\" role=\"alert\">Uderzenie krytyczne.</div>";
                                    userAtt *= 2;
                                }

                                ItemArmor armor = null;
                                foreach(var used in _inventoryRepository.GetInventory(enemy.Id).Where(x => x.Used == true).ToList())
                                {
                                    var item = _itemRepository.GetItem(used.ItemId);
                                    if(item.Type == ItemType.Armor)
                                    {
                                        armor = (ItemArmor)item.ItemType;
                                        break;
                                    }
                                }

                                if(armor != null)
                                {
                                    userAtt -= (int)(userAtt * (rand.Next(0, armor.Resist + 1) / 100f));
                                }   

                                enemy.HealthPoints -= userAtt;
                                userScore += userAtt;
                                roundInfo += $"<div class=\"alert alert-success\" role=\"alert\">Zadałeś {userAtt} obrażeń.</div>";

                                if(enemy.HealthPoints < 1) goto RoundEnd;
                            }

                            if(user.GetDodge() > rand.NextDouble() * 100)
                            {
                                roundInfo += "<div class=\"alert alert-danger\" role=\"alert\">Wróg spudłował.</div>";
                            }
                            else
                            {
                                ItemWeapon weapon = null;
                                foreach(var used in _inventoryRepository.GetInventory(enemy.Id).Where(x => x.Used == true).ToList())
                                {
                                    var item = _itemRepository.GetItem(used.ItemId);
                                    if(item.Type == ItemType.Weapon)
                                    {
                                        weapon = (ItemWeapon)item.ItemType;
                                        break;
                                    }
                                }

                                var enemyAtt = rand.Next(enemy.GetDamage() - 1, enemy.GetDamage() + 2);
                                if(weapon != null)
                                {
                                    enemyAtt = rand.Next(enemy.GetDamage() + weapon.MinDamage, enemy.GetDamage() + weapon.MaxDamage);
                                }

                                if(enemy.GetCritical() >= rand.NextDouble() * 100)
                                {
                                    roundInfo += "<div class=\"alert alert-danger\" role=\"alert\">Uderzenie krytyczne.</div>";
                                    enemyAtt *= 2;
                                }

                                ItemArmor armor = null;
                                foreach(var used in _inventoryRepository.GetInventory(user.Id).Where(x => x.Used == true).ToList())
                                {
                                    var item = _itemRepository.GetItem(used.ItemId);
                                    if(item.Type == ItemType.Armor)
                                    {
                                        armor = (ItemArmor)item.ItemType;
                                        break;
                                    }
                                }

                                if(armor != null)
                                {
                                    enemyAtt -= (int)(enemyAtt * (rand.Next(0, armor.Resist + 1) / 100f));
                                }

                                user.HealthPoints -= enemyAtt;
                                enemyScore += enemyAtt;
                                roundInfo += $"<div class=\"alert alert-danger\" role=\"alert\">Wróg zadał Ci {enemyAtt} obrażeń.</div>";
                            }

                            goto RoundEnd;

                            RoundEnd:
                                arenaResultViewModel.FightInfo += $"<div class=\"card mb-3\"><div class=\"card-header\">Runda: {i}</div><div class=\"card-body\">{roundInfo}</div></div>";
                        }

                        var userReport = new ArenaReportViewModel()
                        {
                            Type =  ReportType.Defend,
                            User = enemy,
                            Rounds = i,
                            UserScore = userScore,
                            EnemyScore = enemyScore,
                        };

                        var enemyReport = new ArenaReportViewModel()
                        {
                            Type =  ReportType.Attack,
                            User = user,
                            Rounds = i,
                            UserScore = userScore,
                            EnemyScore = enemyScore,
                        };

                        var winExperience = userScore > enemyScore ? user.Level * 5 : enemy.Level * 5;
                        var loseExperience = 5;

                        if(userScore > enemyScore)
                        {
                            arenaResultViewModel.ExperiencePoints = winExperience;
                            userReport.ExperiencePoints = winExperience;
                            user.ExperiencePoints += winExperience;
                            user.AllExperience += winExperience;

                            enemyReport.ExperiencePoints = loseExperience;
                            enemy.ExperiencePoints += loseExperience;
                            enemy.AllExperience += loseExperience;
                            enemy.HealthPoints = (int)Math.Floor(enemy.MaxHealthPoints * 0.1);
                        }
                        else
                        {
                            arenaResultViewModel.ExperiencePoints = loseExperience;
                            userReport.ExperiencePoints = loseExperience;
                            user.ExperiencePoints += loseExperience;
                            user.AllExperience += loseExperience;
                            user.HealthPoints = (int)Math.Floor(user.MaxHealthPoints * 0.1);

                            enemyReport.ExperiencePoints = winExperience;
                            enemy.ExperiencePoints += winExperience;
                            enemy.AllExperience += winExperience;
                        }

                        userReport.HealthPoints = enemyHealth;
                        enemyReport.HealthPoints = userHealth;

                        var userReportHtml = await this.RenderViewAsync("ArenaReport", userReport);
                        var enemyReportHtml = await this.RenderViewAsync("ArenaReport", enemyReport);

                        var userMail = new Mail()
                        {
                            FromId = "system",
                            ToId = user.Id,
                            Type = MailType.Received,
                            Title = (userScore > enemyScore ? "[WYGRANA]" : "[PRZEGRANA]") + " Raport z walki",
                            Content = userReportHtml,
                            Status = MailStatus.New
                        };

                        var enemyMail = new Mail()
                        {
                            FromId = "system",
                            ToId = enemy.Id,
                            Type = MailType.Received,
                            Title = (userScore < enemyScore ? "[WYGRANA]" : "[PRZEGRANA]") + " Raport z walki",
                            Content = enemyReportHtml,
                            Status = MailStatus.New
                        };

                        var userInventory = _inventoryRepository.GetInventory(user.Id).Where(x => x.Used == true);
                        var enemyInventory = _inventoryRepository.GetInventory(enemy.Id).Where(x => x.Used == true);

                        foreach(var inventory in userInventory.ToList())
                        {
                            inventory.Stamina--;
                            if(inventory.Stamina < 1) 
                                inventory.Used = false;

                            _inventoryRepository.Update(inventory);
                        }

                        foreach(var inventory in enemyInventory.ToList())
                        {
                            inventory.Stamina--;
                            if(inventory.Stamina < 1) 
                                inventory.Used = false;

                            _inventoryRepository.Update(inventory);
                        }

                        _mailRepository.Add(userMail);
                        _mailRepository.Add(enemyMail);

                        await _userManager.UpdateAsync(user);
                        await _userManager.UpdateAsync(enemy);

                        arenaResultViewModel.UserScore = userScore;
                        arenaResultViewModel.EnemyScore = enemyScore;

                        return View("ArenaResult", arenaResultViewModel);
                    }
                }
            }

            return View();
        }       
    }
}