using AutoMapper;
using crudSegnalR.Infrastructure.Identity.ViewModels;
using crudSignalR.Core.Application.Dtos.Account;
using crudSignalR.Core.Application.Dtos.Message;
using crudSignalR.Core.Application.Dtos.Student;
using crudSignalR.Core.Application.Features.Message.Commands.Create;
using crudSignalR.Core.Application.Features.Student.Commads.Create;
using crudSignalR.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace crudSignalR.Core.Application.Mappings
{
    public class GeneralProfile:Profile
    {
        public GeneralProfile()
        {
            #region Medical Center
           CreateMap<Student,StudentDTO>()               
                .ReverseMap();
            CreateMap<Student, CreateStudentCommand>()
               
                .ReverseMap() .ForMember(x=>x.id, x=>x.Ignore()); 
            
            CreateMap<ResetPasswordViewModel, ResetPasswordRequest>()               
                .ReverseMap() ;
            #endregion

            #region Message
            CreateMap<Message, CreateMessageCommands>()               
                .ReverseMap() ; 
            CreateMap<MessageDTO, Message>()               
                .ReverseMap() ;
            #endregion
        }

    }
}
