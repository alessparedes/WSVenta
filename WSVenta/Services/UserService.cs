using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WSVenta.Models;
using WSVenta.Models.Common;
using WSVenta.Models.Request;
using WSVenta.Models.Response;
using WSVenta.Tools;

namespace WSVenta.Services;

public class UserService : IUserService
{
    private readonly VentaRealContext _db;
    private readonly AppSettings _appSettings;
    
    public UserService(VentaRealContext db, IOptions<AppSettings> appSettings)
    {
        _db = db;
        _appSettings = appSettings.Value;
    } 
    
    public UserReponse? Auth(AuthRequest model)
    {
        UserReponse? oResponse = new UserReponse();
        if (model.Password != null)
        {
            string spassword = Encrypt.GetSHA256(model.Password);

            var usuario = _db.Usuarios
                .FirstOrDefault(u => u.Email == model.Email 
                                     && u.Password == spassword);
        
            if (usuario == null) return null;
            oResponse.Email = usuario.Email;
            oResponse.Token = GetToken(usuario);
        }
        else return null;
        
        return oResponse; 
    }

    private string GetToken(Usuario oUsuario)
    {

        var tokenHandler = new JwtSecurityTokenHandler();
        var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
                new Claim[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, oUsuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, oUsuario.Email),
                    }),
            Expires = DateTime.UtcNow.AddDays(60),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave),SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}