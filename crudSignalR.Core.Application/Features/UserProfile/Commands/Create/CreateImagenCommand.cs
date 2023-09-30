using AutoMapper;
using crudSignalR.Core.Application.Common;
using crudSignalR.Core.Application.Dtos.Student;
using crudSignalR.Core.Application.Features.Student.Commads.Create;
using crudSignalR.Core.Application.Interface.Repository;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Features.Message.Commands.Create
{
    public class CreateImagenCommand : IRequest<GenericApiResponse<byte[]>>
    {     
        public IFormFile file { get; set; }
    }

    public class CreateImagenHandler : IRequestHandler<CreateImagenCommand, GenericApiResponse<byte[]>>
    {
        private IMessageRepository _messageRepository;
        private IMapper _mapper;
        public CreateImagenHandler(IMessageRepository messageRepository, IMapper mapper)
        {

            _mapper = mapper;
            _messageRepository = messageRepository;
        }

        public async Task<GenericApiResponse<byte[]>> Handle(CreateImagenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                GenericApiResponse<byte[]> response = new();
                var message = _mapper.Map<crudSignalR.Core.Domain.Entities.Message>(request);

                var result = await _messageRepository.AddImageAsync(message, request.file);
                if (result != null)
                {
                    response.payload = result.FileBytes;
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

