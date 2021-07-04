using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using Hahn.ApplicatonProcess.February2021.Domain.Models.API;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class SharedValuesController : ControllerBase
    {
        private readonly ISharedValuesService _sharedValuesService;
        private readonly ILogger<SharedValuesController> _logger;

        public SharedValuesController(ISharedValuesService sharedValuesService, ILogger<SharedValuesController> logger)
        {
            _sharedValuesService = sharedValuesService;
            _logger = logger;
        }

        /// <summary>
        /// Returns a list of countries
        /// </summary>
        [HttpGet(nameof(GetCountries))]
        [ProducesResponseType(typeof(IList<CountryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCountriesResponse>> GetCountries()
        {
            var result = new GetCountriesResponse()
            {
                Countries = await _sharedValuesService.GetAllCountriesAsync()
            };
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of departments
        /// </summary>
        [HttpGet(nameof(GetDepartments))]
        [ProducesResponseType(typeof(IList<DepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetDepartmentsResponse>> GetDepartments()
        {
            var result = new GetDepartmentsResponse()
            {
                Departments = await _sharedValuesService.GetAllDepartmentsAsync()
            };
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a list of top level domains
        /// </summary>
        /// <response code="200">The list of top level domains</response>
        /// <response code="500">Error occured retrieving the list of top level domains, returns message in machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807</response>
        [HttpGet(nameof(GetTopLevelDomains))]
        [ProducesResponseType(typeof(GetTopLevelDomainsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetTopLevelDomainsResponse>> GetTopLevelDomains()
        {
            var result = new GetTopLevelDomainsResponse
            {
                TopLevelDomains = await _sharedValuesService.GetAllTopLevelDomainsAsync()
            };
            return Ok(result);
        }
    }
}
