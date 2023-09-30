using AutoMapper;
using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Dtos.Message;
using crudSignalR.Core.Application.Features.Message.Commands.Create;
using crudSignalR.Core.Application.Interface.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Features.Message.Queries.GetMessagePagination
{
    public class GetMessagePagination : IRequest<GenericApiResponse<List<MessageDTO>>>
    {
        public int pageNumber { get; set; }
        public int pageCountRegister { get; set; }
        public string userFrom { get; set; }
        public string userTo { get; set; }

  
    }
public class GetMessagePaginationandler : IRequestHandler<GetMessagePagination, GenericApiResponse<List<MessageDTO>>>
{
    private IMessageRepository _messageRepository;
    private IMapper _mapper;
    public GetMessagePaginationandler(IMessageRepository messageRepository, IMapper mapper)
    {

        _mapper = mapper;
        _messageRepository = messageRepository;
    }

    public async Task<GenericApiResponse<List<MessageDTO>>> Handle(GetMessagePagination request, CancellationToken cancellationToken)
    {
        try
        {
            GenericApiResponse<List<MessageDTO>> response = new();

            var result = await _messageRepository.GetMessagePagination(request.pageNumber, request.pageCountRegister,request.userFrom, request.userTo);
                var targetResult = _mapper.Map<List<MessageDTO>>(result);
            if (result != null)
            {
                response.payload = targetResult;
                response.messages = new List<string>() { GeneralMessageResponse.Success };
                response.success = true;
            }
            else
            {
                response.messages = new List<string>() { GeneralMessageResponse.Error };
                response.success = false;
            }
            return response;
        }
        catch (Exception)
        {

            throw;
        }


    }


}


}
