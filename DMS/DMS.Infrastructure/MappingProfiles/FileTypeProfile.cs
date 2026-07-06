using AutoMapper;
using DMS.DTO.DTOs;
using DMS.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Infrastructure.MappingProfiles
{
    public class FileTypeProfile :Profile
    {
        public FileTypeProfile()
        {
            CreateMap<TnTypesDossier, FileTypeDto>().ReverseMap();
        }
    }
}
