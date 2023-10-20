using crudSignalR.Core.Application;
using crudSignalR.Core.Application.Dtos.EmailService;
using crudSignalR.Core.Application.Dtos.Student;
using crudSignalR.Core.Application.Features.Student.Commads.Create;
using crudSignalR.Core.Application.Interface.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.Mime;

namespace signalRApiStudent.Controllers
{
    [Authorize (Roles ="Admin")]
    public class StudentController : BaseController
    {
        private readonly IHubContext<SiganalServer> _hubContext;
        private readonly IEmailService _emailService;

        public StudentController(IHubContext<SiganalServer> hubContext,IEmailService emailService)
        {
            _hubContext = hubContext;
            _emailService = emailService;
        }
        
        #region Command
        [HttpPost("Create")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof( StudentDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
      
        public async Task<IActionResult> Create([FromBody] CreateStudentCommand command)
        {
            try
            {
                var result = await Mediator.Send(command);
               await _hubContext.Clients.All.SendAsync("newCreateStudent", result);
             await _emailService.SendAsync(
                    new EmailRequest
                    {
                        To = "alexanderrpolanco11@gmail.com",
                        Body = @"
            <html>
                <body>
                    <h1>¡Hola!</h1>
                    <p>Este es un ejemplo de correo con una imagen:</p>
                    <img src='https://picsum.photos/200/300' alt='Imagen'>
 <button style='background-color: blue; color: white; border-radius: 5px; padding: 10px;'>Botón Azul</button>
                    <button style='background-color: red; color: white; border-radius: 5px; padding: 10px;'>Botón Rojo</button>

                </body>
            </html>
        ",
                        Subject = "Registrado"
                    });
                return Ok(result);

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
        #endregion

        #region Queries
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(IList<StudentDTO>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
       
        public async Task<IActionResult> GetAllMedicalCenter()
        {

            try
            {
                var toReturn = await Mediator.Send(new GetAllStudent());

             
                return Ok(toReturn);

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
        #endregion

    }
}
