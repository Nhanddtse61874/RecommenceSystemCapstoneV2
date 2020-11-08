using Business.Services;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public interface ICategoryService
    {
        void Create(Category newCate);

        bool CheckCategory(int code);
    }

    public class CategoryService : ServiceBase, ICategoryService
    {
        private readonly IRepository<Category> _categoryRepo;

        public CategoryService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _categoryRepo = _unitOfWork.Repository<Category>();
        }
        public void Create(Category newCate)
        {
            _categoryRepo.Create(newCate);
            _unitOfWork.Save();

        }
        public bool CheckCategory(int code)
        {
            var result = _categoryRepo.Get(x => x.Code == code);
            if (result == null)
            { 
                return true; 
            }
            else return false;
        }
    }
}
