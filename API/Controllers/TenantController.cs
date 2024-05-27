using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stockify.Business;
using Stockify.Models;
using System;
using System.Security.Claims;

namespace Stockify.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TenantController : ControllerBase
    {
        private readonly ITenantService _tenantService;
        private readonly IUserService _userService;

        public TenantController(ITenantService tenantService, IUserService userService)
        {
            _tenantService = tenantService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var inventories = _tenantService.GetAll();
            return Ok(inventories);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var tenant = _tenantService.Get(id);
            if (tenant == null)
            {
                return NotFound();
            }
            return Ok(tenant);
        }

        [HttpGet("{tenantId}/users")]
        public IActionResult GetUserByTenantId(int tenantId)
        {
            var user = _userService.GetUserByTenantId(tenantId);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

       [HttpPost]
        public IActionResult Add([FromBody] TenantCreateDto TenantCreateDto)
        {
            if (TenantCreateDto == null)
            {
                return BadRequest();
            }
            var Tenant = new Tenant
            {
                Name = TenantCreateDto.Name,
                Password = TenantCreateDto.Password,
                Contact = TenantCreateDto.Contact,
                Role = "tenant"

            };
            _tenantService.Add(Tenant);
            return CreatedAtAction(nameof(Get), new { id = Tenant.Id }, Tenant);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Tenant tenant)
        {
            if (id != tenant.Id)
            {
                return BadRequest();
            }
            _tenantService.Update(tenant);
            return NoContent();
        }

        [HttpPut("user/{userId}/role")]
        [Authorize(Roles = Roles.Tenant)] // Solo los tenants pueden actualizar roles
        public IActionResult UpdateUserRole(int userId, [FromBody] UserPutRole userPutRole)
        {
            try
            {
                // Verificar si el usuario que realiza la solicitud es un Tenant
                if (!User.IsInRole(Roles.Tenant))
                {
                    return Unauthorized("Solo los tenants pueden actualizar roles de usuarios.");
                }

                // Obtener el usuario por su ID
                var user = _userService.Get(userId);
                if (user == null)
                {
                    return NotFound("Usuario no encontrado.");
                }

                // Actualizar el rol del usuario
                user.Role = userPutRole.Role;
                _userService.Update(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error interno: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _tenantService.Delete(id);
            return NoContent();
        }
    }
}
