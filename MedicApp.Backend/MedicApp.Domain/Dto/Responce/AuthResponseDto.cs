namespace MedicApp.Domain.Dto.Responce;

    
public class AuthResponseDto
{
    public string Token { get; set; }
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}   