using Persistence.DataAccess;
using Persistence.Models;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Services
{
    public abstract class ServiceBase
    {
        protected readonly IUnitOfWork _unitOfWork;


        protected ServiceBase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
    }

    
}
