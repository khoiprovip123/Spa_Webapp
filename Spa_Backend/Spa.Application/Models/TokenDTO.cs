namespace Spa.Application.Models
{
    public record TokenDTO()
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken {  get;set; }
    }
}
