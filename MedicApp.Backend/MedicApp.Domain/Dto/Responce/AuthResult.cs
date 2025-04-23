namespace MedicApp.Domain.Dto.Responce;

public class AuthResult
{
    public bool Succeeded { get; set; }
    public string Error { get; set; }
    public AuthResponseDto Response { get; set; }
}