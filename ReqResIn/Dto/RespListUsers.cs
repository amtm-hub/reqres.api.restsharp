using System.Collections.Generic;

namespace ReqResIn.Dto
{
    public sealed class RespListUsers
    {
        public int Page { get; set; }
        public int PerPage { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public List<Datum> Data { get; set; }
        public Support Support { get; set; }
    }
    
    public class Datum
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Avatar { get; set; }
    }

    public sealed class Support
    {
        public string Url { get; set; }
        public string Text { get; set; }
    }
}
