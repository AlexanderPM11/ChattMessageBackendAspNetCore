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
    public class GetAllStudent : IRequest<IList<StudentDTO>>
    {
    }

    public class GetAllStudentdHandler : IRequestHandler<GetAllStudent, IList<StudentDTO>>
    {
        private IStudentRepository _studentRepository;
        private IMapper _mapper;
        public GetAllStudentdHandler(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IList<StudentDTO>> Handle(GetAllStudent request, CancellationToken cancellationToken)
        {
            try
            {
      
                var result = await _studentRepository.GetAllAsync();
                return _mapper.Map<IList<StudentDTO>>(result);
            }
            catch (Exception)
            {

                throw;
            }


        }

      
    }
}
