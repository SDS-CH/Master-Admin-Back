using DMS.DTO.DTOs;
using DMS.Infrastructure.IRepositories;
using DMS.Infrastructure.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services.Services

{

    public class ActivityService : IActivityService

    {

        private readonly IActivityRepository _repository;

        public ActivityService(IActivityRepository repository)

        {

            _repository = repository;

        }

        public async Task<IEnumerable<ActivityDto>> GetAll()

            => await _repository.GetAll();

        public async Task Add(ActivityDto dto)

            => await _repository.Add(dto);

    }

}
