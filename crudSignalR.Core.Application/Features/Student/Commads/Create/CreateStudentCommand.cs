using AutoMapper;
using crudSignalR.Core.Application.Dtos.Student;
using crudSignalR.Core.Application.Interface.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using crudSignalR.Core.Domain.Entities;

namespace crudSignalR.Core.Application.Features.Student.Commads.Create
{
    public class CreateStudentCommand:IRequest<StudentDTO>
    {
      
        public string name { get; set; }
        public byte age { get; set; }
    }

    public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentDTO>
    {
        private IStudentRepository _studentRepository;
        private IMapper _mapper;
     public   CreateStudentCommandHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<StudentDTO> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestMapper = _mapper.Map<crudSignalR.Core.Domain.Entities.Student>(request);
                var result = await _studentRepository.AddAsync(requestMapper);
                return _mapper.Map<StudentDTO>(result);
            }
            catch (Exception)
            {

                throw;
            }

           
        }
    }
}
