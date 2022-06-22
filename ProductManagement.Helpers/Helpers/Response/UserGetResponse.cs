using ProductManagement.Models;
using ProductManagementWebApi.Helpers.Interfaces;

namespace ProductManagementWebApi.Helpers.Response
{
    public class UserGetResponse : IGetResponse<User>
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public bool IsInActive { get; set; }
        public UserGetResponse()
        {

        }
        public UserGetResponse(int Id, string UserName, string Avatar, bool IsInActive)
        {
            this.Id = Id;
            this.UserName = UserName;
            this.Avatar = Avatar;
            this.IsInActive = IsInActive;
        }
    }
}
