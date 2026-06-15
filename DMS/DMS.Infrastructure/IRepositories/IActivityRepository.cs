using DMS.Entities.Models;
using Kendo.Mvc.UI;
using Master.Common.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DMS.DTO.DTOs;

namespace DMS.Infrastructure.IRepositories

{

    public interface IActivityRepository

    {

        Task<IEnumerable<ActivityDto>> GetAll();


        Task Add(ActivityDto dto);


    }

}
 