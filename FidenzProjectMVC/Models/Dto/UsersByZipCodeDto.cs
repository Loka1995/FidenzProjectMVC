namespace FidenzProjectMVC.Models.Dto
{
    public class UsersByZipCodeDto
    {
        public int ZipCode { get; set; }
        public List<User> Users { get; set; }
        //public List<string> UserNames { get; set; }
    }
}
