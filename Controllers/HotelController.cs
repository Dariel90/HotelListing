using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HotelListing.DTOs;
using HotelListing.IRepository;
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
        [ProducesErrorResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(StatusCodes.Status500InternalServerError)]
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

        [HttpGet("{id:int}")]
        [ProducesErrorResponseType(StatusCodes.Status200OK)]
        [ProducesErrorResponseType(StatusCodes.Status500InternalServerError)]
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
    }
}
