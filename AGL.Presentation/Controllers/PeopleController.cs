using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AGL.BusinessLogic;
using AGL.Dto;
using AGL.Library;
using AGL.Presentation.Models;
using Microsoft.AspNetCore.Mvc;

namespace AGL.Presentation.Controllers
{
    public class PeopleController : Controller
    {
        private IPeopleBusinessLogic _peopleBusinessLogic { get; }

        public PeopleController(IPeopleBusinessLogic peopleBusinessLogic)
        {
            _peopleBusinessLogic = peopleBusinessLogic;
        }

        /// <summary>
        /// Gets a list of people from the business layer in the correct view structure.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            try
            {
                var peopleResponse = await _peopleBusinessLogic.GetPeople();
                if (peopleResponse.ResponseStatus == ResponseStatusEnum.Failure)
                {
                    peopleResponse.Errors.ForEach(error =>
                    {
                        ModelState.AddModelError("", error);
                    });

                    return View(new List<Gender>());
                }
                else
                {
                    var viewModel = peopleResponse.Data.Select(gender => new Gender
                    {
                        GenderType = gender.Gender,
                        Cats = gender.Cats.Select(cat => new Cat
                        {
                            Name = cat.Name
                        }).ToList()
                    }).ToList();

                    return View(viewModel);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return View(new List<Gender>());
            }
        }

        /// <summary>
        /// Returns contact details from the razor view
        /// </summary>
        public IActionResult Contact()
        {
             return View();
        }
        
        public IActionResult Error()
        {
            return View(new Error { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
