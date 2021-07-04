using AutoMapper;
using Hahn.ApplicatonProcess.February2021.Data.Interfaces;
using Hahn.ApplicatonProcess.February2021.Domain.Exceptions;
using Hahn.ApplicatonProcess.February2021.Domain.Models.API;
using Hahn.ApplicatonProcess.February2021.Domain.Models.DTO;
using Hahn.ApplicatonProcess.February2021.Domain.Models.Entities;
using Hahn.ApplicatonProcess.February2021.Domain.Models.Validation;
using Hahn.ApplicatonProcess.February2021.Domain.Utils;
using Hahn.ApplicatonProcess.February2021.Domain.Validators;
using Hahn.ApplicatonProcess.February2021.Web.SwaggerExamples;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.February2021.Web.Controllers
{
    /// <summary>
    /// Endpoint to handle the class named Asset 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    public class AssetController : ControllerBase
    {

        private readonly IAssetService _assetService;
        private readonly IValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly ILogger<AssetController> _logger;

        public AssetController(IAssetService assetService, IValidationService validationService, IMapper mapper, ILogger<AssetController> logger)
        {
            _assetService = assetService;
            _validationService = validationService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns Asset by Id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GetAssetByPageResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByPageAsync(int page = 1, int itemsPerPage = 10)
        {
            try
            {
                if (page == 0) page = 1;
                if (itemsPerPage < 1) itemsPerPage = 10;
                page--;
                var TotalItems = await _assetService.CountAsync(null);
                 var totalPages = (long)Math.Ceiling(TotalItems / (decimal)itemsPerPage);
                var assets = _assetService.GetByPage(null, page, itemsPerPage);

                return Ok(new GetAssetByPageResponse
                {
                    TotalItems = TotalItems,
                    TotalPages = totalPages,
                    Assets = assets.ToList()
                });
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Requested asset {0} not found!", ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Returns Asset by Id
        /// </summary>
        /// <param name="id">Asset unique Id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            try
            {
                var result = await _assetService.GetAsync(id);
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Requested asset {0} not found!", ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Creates new Asset
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Asset
        ///     {
        ///        "AssetName": "Super computer",
        ///        "Department": "1",
        ///        "Country": "BRA",
        ///        "Email": "email@email.com",
        ///        "PurchaseDate": "2021-07-04T00:00:00-03:00",
        ///        "Broken": false
        ///     }
        ///
        /// </remarks>
        /// <response code="201">The asset created successfully</response>
        /// <response code="400">Validation failed</response>
        /// <response code="500">Error occured creating the asset, returns message in machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807</response>
        [HttpPost]
        [SwaggerRequestExample(typeof(ExpandoObject),typeof(AssetDtoExampleNew))]
        [ProducesResponseType(typeof(AssetDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(IList<ValidationMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PostAsync([FromBody] PostAssetRequest asset)
        {
            try
            {
                var result = _validationService.Validate<PostAssetRequest, AssetValidator>(asset);
                if (result.IsValid)
                {
                    var id = await _assetService.CreateAsync(_mapper.Map<AssetDto>(asset));
                    return Created($"{Url.ActionLink(action: "Get", controller: "Asset")}/{id}", await _assetService.GetAsync(id));
                }
                else
                {
                    _logger.LogWarning("Failed validation: {0}", result.Errors.FormatErrors());
                    return BadRequest(result.Errors.FormatErrors());
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Requested asset {0} not found!", ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Updates an Asset
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Asset
        ///     {
        ///        "id": 1,
        ///        "AssetName": "Super computer",
        ///        "Department": "1",
        ///        "Country": "BRA",
        ///        "Email": "email@email.com",
        ///        "PurchaseDate": "2021-07-04T00:00:00-03:00",
        ///        "Broken": false
        ///     }
        ///
        /// </remarks>
        /// <response code="204">The asset updated successfully</response>
        /// <response code="400">Validation failed</response>
        /// <response code="404">The asset not found, returns message in machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807</response>
        /// <response code="500">Error occured updating the asset, returns message in machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807</response>
        [HttpPut]
        [SwaggerRequestExample(typeof(ExpandoObject), typeof(AssetDtoExampleEdit))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(IList<ValidationMessage>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> PutAsync([FromBody] PutAssetRequest asset)
        {
            var result = _validationService.Validate<PutAssetRequest, AssetValidator>(asset);
            try
            {
                if (result.IsValid)
                {
                    if (await _assetService.UpdateAsync(_mapper.Map<AssetDto>(asset)))
                    {
                        return NoContent();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    _logger.LogWarning("Failed validation: {0}", result.Errors.FormatErrors());
                    return BadRequest(result.Errors.FormatErrors());
                }
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Requested asset {0} not found!", ex.Message);
                return NotFound();
            }
        }

        /// <summary>
        /// Deletes an Asset by Id
        /// </summary>
        /// <param name="id">Asset unique Id</param>
        /// <response code="204">The asset deleted successfully</response>
        /// <response code="400">Incorrect identifier format, returns message in machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807 </response>
        /// <response code="404">The asset not found, returns message in machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807</response>
        /// <response code="500">Error occured retrieving the asset, returns message in machine-readable format for specifying errors in HTTP API responses based on https://tools.ietf.org/html/rfc7807</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                await _assetService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning("Requested asset {0} not found!", ex.Message);
                return NotFound();
            }
        }
    }
}
