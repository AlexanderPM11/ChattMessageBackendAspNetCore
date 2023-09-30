using AutoMapper;
using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Interface.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Features.Message.Commands.Delete
{
   
    public class DeleteMessageCommands : IRequest<GenericApiResponse<bool>>
    {
        public int id { get; set; }
        public string idTo { get; set; }
        public string From { get; set; }
    }

    public class GetAllStudentdHandler : IRequestHandler<DeleteMessageCommands, GenericApiResponse<bool>>
    {
        private IMessageRepository _messageRepository;
        private IMapper _mapper;
        public GetAllStudentdHandler(IMessageRepository messageRepository, IMapper mapper)
        {

            _mapper = mapper;
            _messageRepository = messageRepository;
        }

        public async Task<GenericApiResponse<bool>> Handle(DeleteMessageCommands request, CancellationToken cancellationToken)
        {
            GenericApiResponse<bool> response = new();
            try
            {
               
                var message =await _messageRepository.GetByIdAsync(request.id);
                message.IsDeleted = true;
                var result=  await _messageRepository.DesactiveMessage(message);
                if (result)
                {
                    response.success= true;
                    response.statuscode=200;
                    response.messages= new List<string>() { "Mensaje eliminado con exito" };
                }
                else
                {
                    response.success = false;
                    response.statuscode = 500;
                    response.messages = new List<string>() { "Ocurrió un error al intentar eliminar el menaje" };
                }
                return response;
            }
            catch (Exception)
            {
                response.messages = new List<string>() { GeneralMessageResponse.Error };
                response.success = true;
                return response;
                throw;
            }


        }


    }
}
