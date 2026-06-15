using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using DMS.DTO.DTOs;


namespace DMS.Infrastructure.IServices
{

    public interface IActivityService

    {

        Task<IEnumerable<ActivityDto>> GetAll();

        Task Add(ActivityDto dto);

    }

}
