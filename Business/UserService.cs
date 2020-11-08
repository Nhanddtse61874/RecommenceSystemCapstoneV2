using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public interface IUserService
    {
        void Create(User newUser);

        void Delete(int userId);

        bool GetById(int id);

        bool CheckUserExist(int code);
    }
    public class UserService : ServiceBase, IUserService
    {
        private readonly IRepository<User> _repo;

        public UserService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _repo = _unitOfWork.Repository<User>();
        }

        public bool CheckUserExist(int code)
        {
            var result = _repo.Get(x => x.Code == code);
            if (result == null)
            {
                return true;
            }
            else return false;
        }

        public void Create(User newUser)
        {
            if (CheckUserExist(newUser.Code))
            {
                _repo.Create(newUser);
                 _unitOfWork.Save();
            }
        }

        public void Delete(int userId)
        {
            _repo.Delete(userId);
            _unitOfWork.Save();
        }

        public bool GetById(int id)
        {
            var result = _repo.Get(x => x.Id == id);
            if(result == null)
            {
                return false;
            }
            return true;
        }

    }
}
