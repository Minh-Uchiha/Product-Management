using Microsoft.AspNetCore.Mvc;
using ProductManagement.Models;
using ProductManagementWebApi.Helpers.Interfaces;
using ProductManagementWebApi.Helpers.Request;
using ProductManagementWebApi.Helpers.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repository.IRepository
{
    public interface IUserRepository : IRepository<User>
    {
        public IEnumerable<UserGetResponse> GetAll(GetRequest req);
        void Update(User user, UserPostRequest userPostRequest);
        Task Add(UserPostRequest entity);
    }
}
