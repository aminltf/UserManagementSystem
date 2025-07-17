namespace UserManagement.Application.Features.Auth.Dtos;

public record TokenResponse(string AccessToken, string RefreshToken);
