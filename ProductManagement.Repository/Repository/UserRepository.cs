using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProductManagement.DataAccess.Data;
using ProductManagement.Models;
using ProductManagement.Repository.IRepository;
using ProductManagementWebApi.Helpers.Common;
using ProductManagementWebApi.Helpers.Request;
using ProductManagementWebApi.Helpers.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repository.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            _configuration = configuration;
        }

        // Create a new user 
        public async Task Add(UserPostRequest entity)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserPostRequest, User>();
            });
            var mapper = new Mapper(config);
            User AddedObj = mapper.Map<User>(entity);

            // Upload the user's avatar to azure storage account and store the avatar url in the database
            AddedObj.AvatarUrl = await FileHelper.UploadUserImage(entity.Avatar, _configuration.GetSection("AzureStorageAcc")["ConnectionString"], _configuration.GetSection("AzureStorageAcc")["UserContainerName"]);

            await _db.Users.AddAsync(AddedObj);
        }
        
        // Get all users. Support paging, searching, sorting
        public IEnumerable<UserGetResponse> GetAll(GetRequest req)
        {
            var CurrPageNumber = req.PageNumber ?? 1;
            var CurrPageSize = req.PageSize ?? 5;
            var users = (from user in _db.Users
                         select new UserGetResponse(user.Id, user.UserName, user.AvatarUrl, (bool)user.IsInActive)).ToList();
            if (req.SearchReq != null)
            {
                // Filter the list by some search params
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(UserGetResponse)).Find(req.SearchReq.FieldName, true);
                if (prop != null)
                {
                    users = (from u in users
                             where Convert.ToString(prop.GetValue(u)).Contains(req.SearchReq.Query)
                             select u).ToList();
                }
                //if (req.SearchReq.Type == "boolean")
                //{
                //   if (req.SearchReq.FieldName == "IsInActive") users = (from u in users
                //                                                         where u.IsInActive == Convert.ToBoolean(req.SearchReq.Query)
                //                                                         select u).ToList();
                //} else if (req.SearchReq.Type == "string")
                //{
                //    if (req.SearchReq.FieldName == "UserName") users = (from u in users
                //                                                        where u.UserName.Contains(req.SearchReq.Query)
                //                                                        select u).ToList();
                //}
            }
            if (req.SortReq != null)
            {
                // Filter the list by some sort params
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(UserGetResponse)).Find(req.SortReq.FieldName, true);
                if (prop != null)
                {
                    if (req.SortReq.IsAsc) users = (from u in users
                                                    select u).OrderBy(u => prop.GetValue(u)).ToList();
                    else users = (from u in users
                                  select u).OrderByDescending(u => prop.GetValue(u)).ToList();
                }
            }
            return users.Skip(CurrPageSize * (CurrPageNumber - 1)).Take(CurrPageSize).ToList();
     
        }

        // Update a user
        public void Update(User user, UserPostRequest userPostRequest)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserPostRequest, User>();
            });
            var mapper = new Mapper(config);
            user = mapper.Map<User>(userPostRequest);
            _db.Users.Update(user);
        }
    }
}
