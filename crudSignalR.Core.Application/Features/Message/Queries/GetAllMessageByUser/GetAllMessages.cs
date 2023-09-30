using AutoMapper;
using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Dtos.Message;
using crudSignalR.Core.Application.Interface.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Features.Message.Queries.GetAll
{
    
    public class GetAllMessagesByUser : IRequest<GenericApiResponse<IList<MessageDTO>>>
    {
        public string UserFrom { get; set; }
        public string UserTo { get; set; }
    }

    public class GetAllStudentdHandler : IRequestHandler<GetAllMessagesByUser, GenericApiResponse<IList<MessageDTO>>>
    {
        private IMessageRepository _messageRepository;
        private IMapper _mapper;
        public GetAllStudentdHandler(IMessageRepository messageRepository, IMapper mapper)
        {

            _mapper = mapper;
            _messageRepository = messageRepository;
        }

        public async Task<GenericApiResponse<IList<MessageDTO>>> Handle(GetAllMessagesByUser request, CancellationToken cancellationToken)
        {
            GenericApiResponse<IList<MessageDTO>> response = new();
            try
            {

                var message = await _messageRepository.GetAllMessageByUser(request.UserFrom,request.UserTo);
                var Resultmessages=  _mapper.Map<List<MessageDTO>>(message);
                if(Resultmessages.Count!= 0)
                {
                    response.payload= Resultmessages;
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
