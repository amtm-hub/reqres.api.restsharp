using System;

namespace ReqResIn.Dto
{
    public sealed class RespCreate
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
