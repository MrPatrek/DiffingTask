using DiffingTask.ActionFilters;
using DiffingTask.Presentation.ModelBinders;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using Shared.DataTransferObjects;
using Shared.RequestFeatures;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace DiffingTask.Presentation.Controllers
{
    [Route("api/encodedbinaries")]
    [ApiController]
    public class EncodedBinaryController : ControllerBase
    {
        private readonly IServiceManager _service;

        [StringSyntax("Route")]
        private const string encodedBinaryByKeyIdRoute = "{keyId:int:min(1)}";

        [StringSyntax("Route")]
        private const string bySideRoutePart = "/{side:alpha}";

        private const string encodedBinaryByKeyIdAndSideRoute = encodedBinaryByKeyIdRoute + bySideRoutePart;

        public EncodedBinaryController(IServiceManager service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetEncodedBinaries([FromQuery] EncodedBinaryParameters encodedBinaryParameters)
        {
            var pagedResult = await _service.EncodedBinaryService.GetEncodedBinariesAsync(encodedBinaryParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));

            return Ok(pagedResult.encodedBinaries);
        }

        [HttpGet(encodedBinaryByKeyIdAndSideRoute, Name = "EncodedBinaryByKeyIdAndSide")]
        public async Task<IActionResult> GetEncodedBinary(uint keyId, string side)
        {
            var encodedBinary = await _service.EncodedBinaryService.GetEncodedBinaryAsync(keyId, side, trackChanges: false);

            return Ok(encodedBinary);
        }

        [HttpGet(encodedBinaryByKeyIdRoute)]
        public async Task<IActionResult> GetEncodedBinariesForKeyId(uint keyId)
        {
            var encodedBinaries = await _service.EncodedBinaryService.GetByKeyIdsAsync([keyId], trackChanges: false);

            return Ok(encodedBinaries);
        }

        [HttpGet("collection/({keyIds})", Name = "EncodedBinaryCollection")]
        public async Task<IActionResult> GetEncodedBinaryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<uint> keyIds)
        {
            var encodedBinaries = await _service.EncodedBinaryService.GetByKeyIdsAsync(keyIds, trackChanges: false);

            return Ok(encodedBinaries);
        }

        [HttpGet(encodedBinaryByKeyIdRoute + "/diff")]
        public async Task<IActionResult> GetDiff(uint keyId)
        {
            var diff = await _service.EncodedBinaryService.GetDiffAsync(keyId, trackChanges: false);

            return Ok(diff);
        }

        [HttpPost(encodedBinaryByKeyIdAndSideRoute)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEncodedBinary(uint keyId, string side, [FromBody] EncodedBinaryForCreationDto encodedBinary)
        {
            var createdEncodedBinary = await _service.EncodedBinaryService.CreateEncodedBinaryAsync(keyId, side, encodedBinary, trackChanges: false);

            return CreatedAtRoute("EncodedBinaryByKeyIdAndSide",
                new { keyId = createdEncodedBinary.KeyId, side = createdEncodedBinary.Side }, createdEncodedBinary);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEncodedBinaryCollection([FromBody] IEnumerable<EncodedBinaryForCreationCollectionDto> encodedBinaryCollection)
        {
            var result = await _service.EncodedBinaryService.CreateEncodedBinaryCollectionAsync(encodedBinaryCollection, trackChanges: false);

            return CreatedAtRoute("EncodedBinaryCollection", new { result.keyIds }, result.encodedBinaries);
        }

        [HttpDelete(encodedBinaryByKeyIdAndSideRoute)]
        public async Task<IActionResult> DeleteEncodedBinary(uint keyId, string side)
        {
            await _service.EncodedBinaryService.DeleteEncodedBinaryAsync(keyId, side, trackChanges: false);

            return NoContent();
        }

        [HttpPut(encodedBinaryByKeyIdAndSideRoute)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateEncodedBinary(uint keyId, string side, [FromBody] EncodedBinaryForUpdateDto encodedBinary)
        {
            await _service.EncodedBinaryService.UpdateEncodedBinaryAsync(keyId, side, encodedBinary, trackChanges: true);

            return NoContent();
        }

        [HttpPatch(encodedBinaryByKeyIdAndSideRoute)]
        public async Task<IActionResult> PartiallyUpdateEncodedBinary(uint keyId, string side,
            [FromBody] JsonPatchDocument<EncodedBinaryForUpdateDto> patchDoc)
        {
            if (patchDoc is null)
                return BadRequest("patchDoc object sent from client is null.");

            var result = await _service.EncodedBinaryService.GetEncodedBinaryForPatchAsync(keyId, side, trackChanges: true);

            patchDoc.ApplyTo(result.encodedBinaryToPatch, ModelState);

            TryValidateModel(result.encodedBinaryToPatch);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _service.EncodedBinaryService.SaveChangesForPatchAsync(result.encodedBinaryToPatch, result.encodedBinaryEntity);

            return NoContent();
        }

    }
}
