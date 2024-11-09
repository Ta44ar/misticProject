using Microsoft.AspNetCore.Identity;
using misticProject.Data.DTOs;
using misticProject.Data.Models;
using misticProject.Interface;

public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, SignInManager<AppUser> signInManager, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _signInManager = signInManager;
        _logger = logger;
    }

    public async Task<IdentityResult> RegisterUser(RegisterUserDto registerUserDto)
    {
        _logger.LogInformation("Rozpoczêcie rejestracji u¿ytkownika {Email}", registerUserDto.Email);

        var user = new AppUser
        {
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            Email = registerUserDto.Email.ToLower(),
            UserName = registerUserDto.Email.ToLower(),
            DateOfBirth = registerUserDto.DateOfBirth,
            Role = UserRole.RegisteredUser
        };

        _logger.LogInformation("Sprawdzanie, czy email {Email} jest ju¿ zajêty", registerUserDto.Email);
        var existingUser = await _userRepository.GetUserByEmailAsync(registerUserDto.Email.ToLower());
        if (existingUser != null)
        {
            _logger.LogWarning("Email {Email} jest ju¿ zajêty", registerUserDto.Email);
            return IdentityResult.Failed(new IdentityError { Description = "Podany adres email jest zajêty." });
        }

        _logger.LogInformation("Tworzenie u¿ytkownika z email: {Email}", registerUserDto.Email);
        var result = await _userRepository.CreateUserAsync(user, registerUserDto.Password);
        if (!result)
        {
            _logger.LogError("Nie uda³o siê stworzyæ u¿ytkownika z email: {Email}", registerUserDto.Email);
            return IdentityResult.Failed(new IdentityError { Description = "Nie uda³o siê stworzyæ u¿ytkownika." });
        }

        _logger.LogInformation("Rejestracja u¿ytkownika zakoñczona sukcesem {Email}", registerUserDto.Email);
        return IdentityResult.Success;
    }

    public async Task<SignInResult> LoginUser(string email, string password, bool rememberMe)
    {
        _logger.LogInformation("Rozpoczêcie logowania u¿ytkownika {Email}", email);

        var user = await _userRepository.GetUserByEmailAsync(email.ToLower());
        if (user == null)
        {
            _logger.LogWarning("Nie znaleziono u¿ytkownika {Email}", email);
            return SignInResult.Failed;
        }

        var result = await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            _logger.LogInformation("U¿ytkownik {Email} zalogowany pomyœlnie", email);
        }
        else if (result.IsLockedOut)
        {
            _logger.LogWarning("Konto u¿ytkownika {Email} jest zablokowane", email);
        }
        else
        {
            _logger.LogWarning("Nieudana próba logowania u¿ytkownika {Email}", email);
        }

        return result;
    }
}
