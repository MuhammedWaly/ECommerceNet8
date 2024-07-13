using ECommerceNet8.DTOs.AddressDtos.Request;
using ECommerceNet8.DTOs.AddressDtos.Response;
using ECommerceNet8.Infrastructure.Data.OrderModels;
using ECommerceNet8.Core.Reposiatories.AddressRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ECommerceNet8.Infrastructure.Constants;
using System.Security.Claims;

namespace ECommerceNet8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpGet]
        [Route("GetAllAddress")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<IEnumerable<Address>>> GetAllAddresses()
        {
            var addresses = await _addressRepository.GetAllAddresses();
            return Ok(addresses);
        }

        [HttpGet]
        [Route("GetAddressById/{addressId}")]
        [ActionName("GetAddressById")]
        public async Task<ActionResult<Address>> GetAddressById(
            [FromRoute] int addressId)
        {
            var address = await _addressRepository.GetAddressById(addressId);
            if (address == null)
            {
                return BadRequest("No Address Found With Given Id");
            }

            return Ok(address);
        }

        [HttpGet]
        [Route("GetAddressesByUserId/")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddressesByUserId()

        {
            string userId = HttpContext.User.FindFirstValue("uid");
            var addresses = await _addressRepository.GetAddressesByUserId(userId);
            return Ok(addresses);
        }

        [HttpPost]
        [Route("AddAddress")]
        [Authorize]
        public async Task<IActionResult> AddAddress([FromBody] Request_AddressInfo addressInfo)
        {
            string userId = HttpContext.User.FindFirstValue("uid");
            var address = await _addressRepository.AddAddress(userId, addressInfo);

            return CreatedAtAction(nameof(GetAddressById),
                new { addressId = address.Id }, address);
        }

        [HttpPut]
        [Route("UpdateAddress/{addressId}")]
        [Authorize]
        public async Task<ActionResult<Response_AddressInfo>> UpdateAddress(
            [FromRoute] int addressId, [FromBody] Request_AddressInfo addressInfo)
        {
            var addressResponse = await _addressRepository.UpdateAddress(addressId, addressInfo);
            if (addressResponse.isSuccess == false)
            {
                return BadRequest(addressResponse);
            }

            return Ok(addressResponse);
        }

        [HttpDelete]
        [Route("DeleteAddress/{addressId}")]
        [Authorize]
        public async Task<ActionResult<Response_AddressInfo>> DeleteAddress(
            [FromRoute] int addressId)
        {
            var addressResponse = await _addressRepository.DeleteAddress(addressId);
            if (addressResponse.isSuccess == false)
            {
                return BadRequest(addressResponse);
            }
            return Ok(addressResponse);
        }

    }
}
