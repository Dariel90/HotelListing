using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.Data;
using HotelListing.DTOs;
using HotelListing.IRepository;
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
        public async Task<IActionResult> GetHotels()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAll();
                var results = _mapper.Map<IList<HotelDTO>>(countries);
                return Ok(results);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something Went Wrong in the {nameof(GetHotels)}");
                return StatusCode(500, "Internal Sever Error. Please Try Again Later");
            }
        }

        [HttpGet("{id:int}", Name = "GetHotel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetHotel(int id)
        { 
            try
            {
                var country = await _unitOfWork.Hotels.Get(q => q.Id == id, new List<string>{"Country"});
                var result = _mapper.Map<HotelDTO>(country);
                return Ok(result);
            }
            catch (Exception)
            {
                _logger.LogError($"Something Went Wrong in the {nameof(GetHotel)}");
                return StatusCode(500, "Internal Sever Error. Please Try Again Later");
            }
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

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);
                await _unitOfWork.Hotels.Insert(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetHotel", new {id = hotel.Id});
            }
            catch (Exception e)
            {
                _logger.LogError($"Something Went Wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal Sever Error. Please Try Again Later");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] CreateHotelDTO hotelDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateHotel)}");
                return BadRequest(ModelState);
            }

            try
            {
                var hotel = _mapper.Map<Hotel>(hotelDto);
                _unitOfWork.Hotels.Update(hotel);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetHotel", new { id = hotel.Id });
            }
            catch (Exception e)
            {
                _logger.LogError($"Something Went Wrong in the {nameof(CreateHotel)}");
                return StatusCode(500, "Internal Sever Error. Please Try Again Later");
            }
        }

    }
}
