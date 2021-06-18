namespace ReqResIn.Dto
{
    public sealed class RespSingleUser
    {
        public Data Data { get; set; }
        public Support Support { get; set; }
    }

    public sealed class Data
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Avatar { get; set; }
    }
}
