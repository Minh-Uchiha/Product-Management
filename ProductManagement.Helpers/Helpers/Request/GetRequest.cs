namespace ProductManagementWebApi.Helpers.Request
{
    public class GetRequest
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public SortRequest? SortReq { get; set; }
        public SearchRequest? SearchReq { get; set; }
    }
}
