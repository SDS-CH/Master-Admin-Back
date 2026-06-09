using System.Threading.Tasks;

namespace Master.Common.Interfaces.Services
{
    public interface IBaseService<TDto> where TDto : class
    {
        Task<TDto> GetById(int id);
    }
}
