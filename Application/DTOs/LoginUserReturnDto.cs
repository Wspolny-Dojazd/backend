namespace Application.DTOs
{
    public class LoginUserReturnDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Nickname { get; set; }

        public string JWTToken {  get; set; }

    }
}
