using GameModule;
using Microsoft.AspNetCore.Mvc;
using ProjectNomad.Shared.DTOs.ServerToWasm;
using ProjectNomad.Shared.DTOs.WasmToServer;

namespace ProjectNomad.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobController : ControllerBase
    {
        private readonly IJobSection _jobSection;
        public JobController(IJobSection jobSection)
        {
            _jobSection = jobSection;
        }

        // POST api/job
        [HttpPost]
        public async Task<ActionResult<JobDto>> Add([FromBody] JobAdd job)
        {
            if (job == null)
                return BadRequest("JobAdd model is null");

            if (!ModelState.IsValid)
                return BadRequest("Invalid model object");
            try
            {
                var jobDto = await _jobSection.AddJob(job.HumanId, job.Type, job.PriorityPercentage);
                return Ok(jobDto);
            }
            catch (Exception ex)
            {
                //todo log ex
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/job
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] JobUpdate job)
        {
            if (job == null)
                return BadRequest("JobAdd model is null");

            if (!ModelState.IsValid)
                return BadRequest("Invalid model object");

            try
            {
                await _jobSection.UpdateJob(job.Id, job.PriorityPercentage);
                return Ok();
            }
            catch (Exception ex)
            {
                //todo log ex
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/job/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _jobSection.DeleteJob(id);
            return Ok();
        }        
    }
}
