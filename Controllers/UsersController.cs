using Fuel_App.Models.Response;
using Fuel_App.Models.User;
using Fuel_App.Services.Shed_Owner_Service;
using Fuel_App.Services.User_Service;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Globalization;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fuel_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IShedOwnerService shedOwnersService;

        public UsersController(IUserService userService, IShedOwnerService shedOwnersService)
        {
            this.userService = userService;
            this.shedOwnersService = shedOwnersService;
        }
        // GET: api/<UsersController>
        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            return userService.Get();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public ActionResult<User> Get(string id)
        {
            var user = userService.Get(id);
            if (user == null)
            {
                return NotFound($"User with Id = {id} not found");
            }
            return user;
        }

        // GET api/<UsersController>/retrieveWaitingTime/shed_name
        [HttpGet("retrieveWaitingTime/{shed_name}")]
        public ActionResult<User> RetrieveWaitingTime(string shed_name)
        {
            Response final_response = new Models.Response.Response();
            // --------Get Users by Shed Name and get the arrival time of first person in the queue-----------------
            DateTime arrivalTime = new DateTime();
            List<User> the_users = userService.GetUsersByFuelShed(shed_name);
            foreach (User user_item in the_users)
            {
                if (user_item.DepartTime == "null" && user_item.ExitTimeBeforePumping == "null")
                {
                    arrivalTime = DateTime.Parse(user_item.ArrivalTime);
                    break;
                }
            }
            //-------------------------------------------------------------------------------------------------------

            // ---------Calculating the waiting time of the users based on the arrival time of the first person---------- 
            // Retriew the System Time
            DateTime theSystemDateTime = DateTime.Now;
            TimeSpan ts = theSystemDateTime - arrivalTime;
            // Calculating the total waiting time in minutes
            var total_minutes = ts.TotalMinutes;
            var final_result = "";
            // Removing the number of minutes calculated for many days (To get correct number of days,hours and minutes)
            if (total_minutes >= 1440 && total_minutes < 2880)
            {
                if (total_minutes == 1440)
                {
                    final_result = $"1 day";
                    final_response.response = final_result;
                    return Ok(final_response);
                }
                total_minutes = total_minutes - 1440;
            }
            if (total_minutes >= 2880 && total_minutes < 4320)
            {
                if (total_minutes == 2880)
                {
                    final_result = $"2 days";
                    final_response.response = final_result;
                    return Ok(final_response);
                }
                total_minutes = total_minutes - 2880;
            }
            if (total_minutes >= 4320 && total_minutes < 5760)
            {
                if (total_minutes == 4320)
                {
                    final_result = $"3 days";
                    final_response.response = final_result;
                    return Ok(final_response);
                }
                total_minutes = total_minutes - 4320;
            }
            if (total_minutes >= 5760 && total_minutes < 7200)
            {
                if (total_minutes == 5760)
                {
                    final_result = $"4 days";
                    final_response.response = final_result;
                    return Ok(final_response);
                }
                total_minutes = total_minutes - 5760;
            }
            if (total_minutes >= 7200 && total_minutes < 8640)
            {
                if (total_minutes == 7200)
                {
                    final_result = $"5 days";
                    final_response.response = final_result;
                    return Ok(final_response);
                }
                total_minutes = total_minutes - 7200;
            }
            if (total_minutes >= 8640)
            {
                final_result = $"Fuel not avaliable since {arrivalTime}";
                final_response.response = final_result;
                return Ok(final_response);
            }
            // Retriew total number of waiting days in the queue
            var number_of_days = Math.Round(ts.TotalDays, 0);
            int h = 0;
            int r = 0;
            // Calculating the final result respectively -----------------------------------------------------------
            if (total_minutes >= 60)
            {
                h = (int)total_minutes / 60;
                r = (int)total_minutes % 60;
                if (r != 0)
                {
                    if (h <= 1)
                    {
                        if (number_of_days == 0 || number_of_days < 1)
                        {
                            final_result = $"{h} hour and {r} minutes";
                        }
                        else
                        {
                            if (number_of_days == 1)
                            {

                                final_result = $"{number_of_days} day {h} hour and {r} minutes";

                            }
                            else
                            {

                                final_result = $"{number_of_days} days {h} hour and {r} minutes";
                            }

                        }
                    }
                    else
                    {
                        if (number_of_days == 0 || number_of_days < 1)
                        {
                            final_result = $"{h} hours and {r} minutes";
                        }
                        else
                        {
                            if (number_of_days == 1)
                            {

                                final_result = $"{number_of_days} day {h} hours and {r} minutes";
                            }
                            else
                            {

                                final_result = $"{number_of_days} days {h} hours and {r} minutes";
                            }
                        }

                    }
                }
                else
                {
                    if (h <= 1)
                    {
                        if (number_of_days == 0 || number_of_days < 1)
                        {
                            final_result = $"{h} hour";
                        }
                        else
                        {
                            if (number_of_days == 1)
                            {
                                final_result = $"{number_of_days} day and {h} hour";
                            }
                            else
                            {
                                final_result = $"{number_of_days} days and {h} hour";
                            }

                        }
                    }
                    else
                    {
                        if (number_of_days == 0 || number_of_days < 1)
                        {
                            final_result = $"{h} hours";
                        }
                        else
                        {
                            if (number_of_days == 1)
                            {
                                final_result = $"{number_of_days} day and {h} hours";
                            }
                            else
                            {
                                final_result = $"{number_of_days} days and {h} hours";
                            }
                        }
                    }

                }
            }
            else
            {
                if (number_of_days == 0 || number_of_days < 1)
                {
                    final_result = $"{Math.Round(total_minutes)} minutes";
                }
                else
                {
                    if (number_of_days == 1)
                    {
                        final_result = $"{number_of_days} day and {Math.Round(total_minutes,0)} minutes";
                    }
                    else
                    {
                        final_result = $"{number_of_days} days and {Math.Round(total_minutes,0)} minutes";
                    }
                }

            }
            if(number_of_days == 1 && h > 12)
            {
                final_result = $"0 days {h} hours and {r} minutes";
            }
            if (number_of_days == 2 && h > 12)
            {
                final_result = $"1 day {h} hours and {r} minutes";
            }
            if (number_of_days == 3 && h > 12)
            {
                final_result = $"2 days {h} hours and {r} minutes";
            }
            if (number_of_days == 4 && h > 12)
            {
                final_result = $"3 days {h} hours and {r} minutes";
            }
            if (number_of_days == 5 && h > 12)
            {
                final_result = $"4 days {h} hours and {r} minutes";
            }
            if (number_of_days == 6 && h > 12)
            {
                final_result = $"5 days {h} hours and {r} minutes";
            }
            // ---------------------------------------------------------------------------------------------------------
            //----------------------------------------------------------------------------------------------------------
            final_response.response = final_result;
            return Ok(final_response);
        }

        // POST api/<UsersController>
        [HttpPost]
        public ActionResult<User> Post([FromBody] User user)
        {
            userService.AddArrivalTime(user);
            //return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
            // ------------------------- Updating paths in the ShedOwners Service ------------------------
            var existingShed = shedOwnersService.RetriewShedDetails(user.PetrolShedName);
            if (existingShed == null)
            {
                return NotFound($"No Registerd Shed called \"{user.PetrolShedName}\" was found");
            }
            string returedValue = shedOwnersService.updateQueueLength(user.PetrolShedName,1);
            // -------------------------------------------------------------------------------------------
            Response final_response = new Models.Response.Response();
            final_response.response = $"User : {user.Id} joined to the fuel queue !";
            return Ok(final_response);
        }

        // PUT api/<UsersController>/5
        [HttpPut("existingWithoutPumping/{id}")]
        public ActionResult Put(string id, [FromBody] User user)
        {
            Response final_response = new Models.Response.Response();
            var existingUser = userService.Get(id);
            if(existingUser == null)
            {
                final_response.response = $"User with Id = {id} not found";
                return NotFound(final_response);
            }
            userService.ExitBeforePump(id, user);
            // ------------------------- Updating paths in the ShedOwners Service ------------------------
            var existingShed = shedOwnersService.RetriewShedDetails(user.PetrolShedName);
            if (existingShed == null)
            {
                final_response.response = $"No Registerd Shed called \"{user.PetrolShedName}\"";
                return NotFound(final_response);
            }
            string returedValue = shedOwnersService.updateQueueLength(user.PetrolShedName, -1);
            // -------------------------------------------------------------------------------------------
            final_response.response = $"User with Id : {id} exited before fuel pump";
            return Ok(final_response);
        }

        // PUT api/<UsersController>/pumpingDone/5
        [HttpPut("pumpingDone/{id}")]
        public ActionResult Put2(string id, [FromBody] User user)
        {
            Response final_response = new Models.Response.Response();
            var existingUser = userService.Get(id);
            if (existingUser == null)
            {
                final_response.response = $"User with Id = {id} not found";
                return NotFound(final_response);
            }
            userService.ExitAfterPump(id, user);
            // ------------------------- Updating paths in the ShedOwners Service ------------------------
            var existingShed = shedOwnersService.RetriewShedDetails(user.PetrolShedName);
            if (existingShed == null)
            {
                final_response.response = $"No Registerd Shed called \"{user.PetrolShedName}\"";
                return NotFound(final_response);
            }
            string returedValue = shedOwnersService.updateQueueLength(user.PetrolShedName, -1);
            // -------------------------------------------------------------------------------------------
            final_response.response = $"User with Id : {id} exited After the fuel pumping";
            return Ok(final_response);
        }

        // GET api/<UsersController>/5
        [HttpGet("queueLength/{shed_name}/{vehicle_type}")]
        public ActionResult<User> GetQueueLength(string shed_name,string vehicle_type)
        {
            Response final_response = new Models.Response.Response();
            var user = userService.GetQueueLength(shed_name, vehicle_type);
            if (user == 0)
            {
                final_response.response = $"No Avaliable queue for the given Shed: {shed_name}";
                return NotFound(final_response);
            }
            //final_response.response = $"There are {user} {vehicle_type}/s in the queue of Shed: \"{shed_name}\"";
            final_response.response = $"{user}";
            return Ok(final_response);
        }

        // GET api/<UsersController>/queueLength/shed_name
        [HttpGet("queueLength/{shed_name}")]
        public ActionResult<User> GetQueueTotalLength(string shed_name)
        {
            Response final_response = new Models.Response.Response();
            var user = userService.GetTotalQueueLength(shed_name);
            if (user == 0)
            {
                final_response.response = $"No Avaliable queue for the given Shed: \"{shed_name}\"";
                return NotFound(final_response);
            }
            final_response.response = $"{user}";
            return Ok(final_response);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
