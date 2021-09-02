using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;

namespace HotelListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HotelController> _logger;
        private readonly IMapper _mapper;

        public HotelController(IUnitOfWork unitOfWork, ILogger<HotelController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotels([FromQuery] RequestParams requestParams)
        {
            var hotels = await _unitOfWork.Hotels.GetAll(requestParams);
            var results = _mapper.Map<IList<HotelDTO>>(hotels);
            return Ok(results);
        }

        [Authorize]
        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        { 
            var country = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string>{"Hotel"});
            var result = _mapper.Map<HotelDTO>(country);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateHotel([FromBody] CreateHotelDTO hotelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }
            var hotel = _mapper.Map<Hotel>(hotelDto);
            await _unitOfWork.Hotels.Insert(hotel);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetHotel", new {id = hotel.Id});
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] CreateHotelDTO hotelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(UpdateHotel)}");
                return BadRequest(ModelState);
            }
            var hotel = _mapper.Map<Hotel>(hotelDto);
            _unitOfWork.Hotels.Update(hotel);
            await _unitOfWork.Save();

            return CreatedAtRoute("GetHotel", new { id = hotel.Id });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete attempt in {nameof(DeleteHotel)}");
                return BadRequest();
            }

            if (_unitOfWork.Hotels.Get(x => x.Id == id) == null)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(DeleteHotel)}");
                return BadRequest($"The hotel with {id} was not found");
            }
            await _unitOfWork.Hotels.Delete(id);
            await _unitOfWork.Save();

            return NoContent();
        }

    }
}
