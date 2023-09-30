
using crudSignalR.Core.Application;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.Mime;
using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Features.Message.Commands.Create;
using crudSignalR.Core.Application.Dtos.Student;
using crudSignalR.Core.Application.Features.Student.Commads.Create;
using crudSignalR.Core.Application.Features.Message.Queries.GetAll;
using crudSignalR.Core.Application.Dtos.Message;
using crudSignalR.Core.Application.Features.Message.Commands.Delete;
using crudSignalR.Core.Application.Features.Message.Queries.GetMessagePagination;

namespace signalRApiStudent.Controllers
{
    public class MessageController : BaseController
    {
        private readonly IHubContext<SiganalServer> _hubContext;     

        public MessageController(IHubContext<SiganalServer> hubContext)
        {
            _hubContext = hubContext;
        }

        #region Command
        [HttpPost("Create")]
        //[Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Create([FromForm] CreateMessageCommands command)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (result.success)
                {
                    Dictionary<string, dynamic> payload = new Dictionary<string, dynamic>();

                    // Agregar elementos al mapa
                    payload.Add("idTo", command.To);
                    payload.Add("from", command.From);
                    payload.Add("message", command.Messae);
                    payload.Add("id", result.payload.Id);
                    payload.Add("fileBytes", result.payload.FileBytes);
                    payload.Add("isDeleted", result.payload.IsDeleted);
                    await _hubContext.Clients.Group(command.To).SendAsync("NewMessage", payload);
                }
                
                return Ok(result);

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenericApiResponse<bool>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> Delete([FromForm] DeleteMessageCommands command)
        {
            try
            {
                var result = await Mediator.Send(command);
                if (result.success)
                {
                    Dictionary<string, dynamic> payload = new Dictionary<string, dynamic>();
                    // Agregar elementos al mapa
                    payload.Add("id", command.id);
                    payload.Add("idTo", command.idTo);
                    payload.Add("from", command.From);
                    payload.Add("response", "Se elimino un mensaje");
                    await _hubContext.Clients.Group(command.idTo).SendAsync("DeleteMessage", payload);
                }
                
                return Ok(result);

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }

        }
        #endregion

        #region Queries
        [HttpGet ("GetAllMessagesByUser")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GenericApiResponse<List<MessageDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllMessagesByUser(string UserFrom, string UserTo)
        {   

            try
            {
                var toReturn = await Mediator.Send(new GetAllMessagesByUser { UserTo=UserTo,UserFrom= UserFrom });

                return Ok(toReturn);

            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message) { StatusCode = StatusCodes.Status500InternalServerError };
            }
        }
        [HttpGet ("GetMessagePagination")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(statusCode: StatusCodes.Status200OK, Type = typeof(GenericApiResponse<List<MessageDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetMessagePagination(int pageNumber, int pageCountRegister, string UserFrom, string UserTo)
        {   

            try
            {
                var toReturn = await Mediator.Send
                    (new GetMessagePagination { pageNumber=pageNumber, pageCountRegister= pageCountRegister, userTo =UserTo,userFrom= UserFrom });

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
