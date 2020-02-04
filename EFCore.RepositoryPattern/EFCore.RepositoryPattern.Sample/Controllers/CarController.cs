using EFCore.RepositoryPattern.Generics.Exceptions;
using EFCore.RepositoryPattern.Sample.Data.Entities;
using EFCore.RepositoryPattern.Sample.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.RepositoryPattern.Sample.Controllers
{
    [ApiController]
    [Produces("application/json")]
    public sealed class CarController : ControllerBase
    {
        private readonly ICarService carService;

        public CarController(ICarService carService)
        {
            this.carService = carService;
        }

        [HttpGet]
        [Route("api/v1/cars")]
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {
                var result = await carService.GetAllAsync();

                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/v1/cars/{id:guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            try
            {
                var result = await carService.GetByIdAsync(id);

                return Ok(result);
            }
            catch (EntityNotFoundException<Car>)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("api/v1/cars/ids")]
        public async Task<IActionResult> GetByIdsAsync([FromQuery] ICollection<Guid> ids)
        {
            try
            {
                var result = await carService.GetByIdsAsync(ids);

                return Ok(result);
            }
            catch (EntityNotFoundException<Car>)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/v1/cars")]
        public async Task<IActionResult> CreateAsync([FromBody] Car car)
        {
            try
            {
                await carService.CreateAsync(car);

                return Created($"api/v1/cars/{ car.Id }", car);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("api/v1/cars/import")]
        public async Task<IActionResult> CreateBulkAsync([FromBody] ICollection<Car> cars)
        {
            try
            {
                await carService.CreateBulkAsync(cars);

                return Created("api/v1/cars", cars);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/v1/cars/{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromBody] Car car, [FromRoute] Guid id)
        {
            try
            {
                await carService.UpdateAsync(car, id);

                return Ok(car);
            }
            catch (EntityNotFoundException<Car>)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPut]
        [Route("api/v1/cars")]
        public async Task<IActionResult> UpdateBulkAsync([FromBody] ICollection<Car> cars)
        {
            try
            {
                await carService.UpdateBulkAsync(cars);

                return Ok(cars);
            }
            catch (EntityNotFoundException<Car>)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/v1/cars/{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            try
            {
                await carService.DeleteAsync(id);

                return NoContent();
            }
            catch (EntityNotFoundException<Car>)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [Route("api/v1/cars")]
        public async Task<IActionResult> DeleteBulkAsync([FromQuery] ICollection<Guid> ids)
        {
            try
            {
                await carService.DeleteBulkAsync(ids);

                return NoContent();
            }
            catch (EntityNotFoundException<Car>)
            {
                return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
