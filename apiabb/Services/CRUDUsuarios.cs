using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ABBAPI.Services
{
    public class CRUDUsuarios
    {
        public List<Usuario> lusuarios2 = new List<Usuario>() {
            new Usuario {
                Age = 26,
                Email = "admin@M365x17087683.onmicrosoft.com",
                Nombre = "Armando",
                PApellido = "Rodriguez",
                Rol = "Admin",
                SApellido = "Martinez",
                Telefono="1542348591"
            },
            new Usuario
            {
                Age = 32,
                Email = "aMorua@mail.com",
                Nombre = "Alfonso",
                PApellido = "Morua",
                Rol = "User",
                SApellido = "Moreno",
                Telefono="1542348591"
            },
            new Usuario
            {
                Age = 26,
                Email = "MAguilar@mail.com",
                Nombre = "Maria",
                PApellido = "Aguilar",
                Rol = "User",
                SApellido = "Pino",
                Telefono="1542348591"
            },
            new Usuario
            {
                Age = 26,
                Email = "MGarcia@mail.com",
                Nombre = "Miriam",
                PApellido = "Garcia",
                Rol = "User",
                SApellido = "Pino",
                Telefono="1542348591"
            },
            new Usuario
            {
                Age = 26,
                Email = "LMendoza@mail.com",
                Nombre = "Luis",
                PApellido = "Aguilar",
                Rol = "User",
                SApellido = "mendoza",
                Telefono="1542348591"
            }
        };

        public void addusuario(Usuario usuario)
        {
            lusuarios2.Add(usuario);
        }

        public Boolean delusuario(string mail)
        {
            if(lusuarios2.Any(u => u.Email == mail))
            {
                lusuarios2.Remove(lusuarios2.Where(u => u.Email == mail).First());
                return true;
            }
            else { return false; }
            
        }

        public Boolean updateUsuario(Usuario usuario)
        {
            if(lusuarios2.Any(u => u.Email == usuario.Email))
            {
                var useru = lusuarios2.Where(u => u.Email == usuario.Email).First();
                useru.Email = usuario.Email;
                useru.Nombre = usuario.Nombre;
                useru.Age   = usuario.Age;
                useru.PApellido = usuario.PApellido;
                useru.SApellido = usuario.SApellido;
                useru.Rol=usuario.Rol;

                return true;
            }else { return false; }
            

            
        }

        public List<Usuario> GetUsuarios()
        {
            return lusuarios2;
        }

        public JwtSecurityToken createToken(IConfiguration _config, string name, string papellido, string sapellido, string mail, string rol)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                            new Claim(ClaimTypes.Name, name),
                            new Claim(ClaimTypes.GivenName, papellido),
                            new Claim(ClaimTypes.Surname, sapellido),
                            new Claim(ClaimTypes.Email, mail),
                            new Claim(ClaimTypes.Role, rol),
                        };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
            _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
            );

            return token;
        }
    }
}
