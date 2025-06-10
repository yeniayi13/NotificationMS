using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using NotificationMS.Common.Dtos.Request;
using NotificationMS.Application.Command;
using NotificationMS.Application.Queries;
using NotificationMS.Infrastructure.Exceptions;

namespace NotificationMS.Controller
{

    [ApiController]
    [Route("notification")]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly IMediator _mediator;

        public NotificationController(ILogger<NotificationController> logger, IMediator mediator)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(logger));
        }
        [HttpPost("Add-Notification")]
        public async Task<IActionResult> CreatedProduct([FromBody] CreateNotificationDto createNotificationDto)
        {
            try
            {
               
                var command = new CreateNotificationCommand(createNotificationDto);
                var notificatioId = await _mediator.Send(command);

                
                return Ok(notificatioId);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError("Error: {Message}", e.Message);
                return StatusCode(400, $"Error de argumento nulo: {e.Message}");
            }
           
            catch (InvalidOperationException e)
            {
                _logger.LogError("Error: {Message}", e.Message);
                return StatusCode(500, $"Operación inválida: {e.Message}");
            }
            catch (HttpRequestException e) when (e.Message.Contains("401"))
            {
                _logger.LogError("Error de autenticación: {Message}", e.Message);
                return StatusCode(401, "Acceso denegado. Verifica tus credenciales.");
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError("Acceso no autorizado: {Message}", e.Message);
                return StatusCode(401, "No tienes permisos para acceder a este recurso.");
            }
            catch (Exception e)
            {
                _logger.LogError("Error inesperado: {Message}", e.Message);
                return StatusCode(500, "Ocurrió un error inesperado al intentar crear el producto.");
            }
        }
        [HttpGet("Get-Notification/{userId}")]
        public async Task<IActionResult> GetAllByUserId([FromRoute] Guid userId)
        {
            try
            {
                var query = new GetAllByUserIdQuery(userId);
                var notifications = await _mediator.Send(query);
                return Ok(notifications);
            }
            catch (NotificationNotFoundException e)
            {
                _logger.LogError("Error: {Message}", e.Message);
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Error inesperado: {Message}", e.Message);
                return StatusCode(500, "Ocurrió un error inesperado al intentar obtener las notificaciones.");
            }
        }
    }

}
