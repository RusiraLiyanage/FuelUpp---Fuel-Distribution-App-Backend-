using Fuel_App.Models.Response;
using Fuel_App.Models.Shed_Owner_Model;
using Fuel_App.Models.User;
using Fuel_App.Services.Shed_Owner_Service;
using Fuel_App.Services.User_Service;
using Microsoft.AspNetCore.Mvc;
using System;
using static System.Collections.Specialized.BitVector32;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fuel_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShedOwnersController : ControllerBase
    {
        private readonly IShedOwnerService shedOwnersService;

        public ShedOwnersController(IShedOwnerService shedOwnerService)
        {
            this.shedOwnersService = shedOwnerService;
        }
        // GET: api/<ShedOwnersController>
        [HttpGet]
        public ActionResult<List<ShedOwner>> Get()
        {
            return shedOwnersService.Get();
        }

        // GET api/<ShedOwnersController>/5
        [HttpGet("{shed_name}")]
        public ActionResult<ShedOwner> Get(string shed_name)
        {
            Response final_response = new Models.Response.Response();
            var shed_owner = shedOwnersService.RetriewShedDetails(shed_name);
            if (shed_owner == null)
            {
                final_response.response = $"Fuel Shed \"{shed_name}\" wasn't found";
                return NotFound(final_response);
            }
            return shed_owner;
        }

        // POST api/<ShedOwnersController>
        [HttpPost]
        public ActionResult<ShedOwner> Post([FromBody] ShedOwner shed_owner)
        {
            Response final_response = new Models.Response.Response();
            // updating QueueLength to "0" since the Shed Owner is just registering the shed (Bypassing the request values)
            shed_owner.QueueLength = 0;
            shedOwnersService.RegisterPetrolShed(shed_owner);
            //return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
            final_response.response = $"Fuel Shed: \"{shed_owner.ShedName}\" was registerd successfully";
            //final_response.response = final_response.response.Replace('\',);
            return Ok(final_response);
        }

        // PUT api/<ShedOwnersController>/updateFuelData
        [HttpPut("updateFuelData/{shed_name}")]
        public ActionResult Put(string shed_name, [FromBody] ShedOwner shed_owner)
        {
            Response final_response = new Models.Response.Response();
            //return Ok($"{shed_name}");
            var existingShed = shedOwnersService.RetriewShedDetails(shed_name);
            if (existingShed == null)
            {
                final_response.response = $"The Shed: \"{shed_name}\" wasn't found";
                return NotFound(final_response);
            }
            shedOwnersService.UpdateFuelData(shed_name, shed_owner);
            final_response.response = $"The Fuel Data for Shed: {shed_name} was updated successfully";
            return Ok(final_response);
        }

        // PUT api/<ShedOwnersController>/updateFuelQueueLength
        [HttpPut("updateFuelQueueLength/{shed_name}/{queue_action}")]
        public ActionResult UpdateQueueLength(string shed_name, int queue_action)
        {
            Response final_response = new Models.Response.Response();
            var existingShed = shedOwnersService.RetriewShedDetails(shed_name);
            if (existingShed == null)
            {
                final_response.response = $"No Registerd Shed called \"{shed_name}\"";
                return NotFound(final_response);
            }
            string returedValue = shedOwnersService.updateQueueLength(shed_name, queue_action);
            final_response.response = returedValue;
            return Ok(final_response);
            //return Ok($"The Fuel Data for Shed: {shed_name} was updated successfully");
        }


        // DELETE api/<ShedOwnersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
