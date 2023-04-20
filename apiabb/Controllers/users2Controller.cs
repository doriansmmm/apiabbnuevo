using Microsoft.AspNetCore.Hosting;

using Microsoft.AspNetCore.Mvc;
using System.Linq;
//using ApiExpedienteMedico.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IdentityModel.Tokens.Jwt;
using System.Data;
using System;
using ABBAPI.Services;

namespace ABBAPI.Controllers
{
    [ApiController]
    [Route("User2")]
    public class User2Controller : Controller
    {
        private readonly IConfiguration _config;    
        public CRUDUsuarios usercrud = new CRUDUsuarios();
        public List<Usuario> lusuarios = new List<Usuario>();
        
        public User2Controller( IConfiguration config)
        {
            
            _config = config;
           
        }

        [HttpGet]
        [Route("Getusers")]
        //Get para obtener los usuarios activos en la BD
        public ActionResult Getusers(string token)
        {
            try
            {

                GetTokenValues otoken = new GetTokenValues();

                if(otoken.validToken(token))
                {
                    var tokenV = otoken.getTokenValues(token);

                    if(tokenV.Role.ToLower() == "admin") {

                        return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.OK.ToString(),
                                code = HttpStatusCode.OK,
                                response = usercrud.GetUsuarios().ToList()
                            });
                    }
                    else
                    {
                        return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "Este usuario no tiene los permisos necesarios"
                            });
                    }
                }
                else
                {
                    return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "Token vencido"
                            });
                }
                
                

                


            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = msg
                });


            }

        }

        [HttpPost]
        [Route("Deluser")]
        //Get para obtener los usuarios activos en la BD
        public ActionResult Deluser(string token, string mail)
        {
            try
            {

                GetTokenValues otoken = new GetTokenValues();

                if (otoken.validToken(token))
                {
                    var tokenV = otoken.getTokenValues(token);

                    if (tokenV.Role.ToLower() == "admin")
                    {
                        if(usercrud.delusuario(mail))
                        {
                            return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.OK.ToString(),
                                code = HttpStatusCode.OK,
                                response = "Usuario eliminado correctamente"
                            });
                        }
                        else
                        {
                            return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "El usuario a eliminar no existe"
                            });
                        }
                        
                    }
                    else
                    {
                        return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "Este usuario no tiene los permisos necesarios"
                            });
                    }
                }
                else
                {
                    return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "Token vencido"
                            });
                }






            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = msg
                });


            }

        }

        [HttpPost]
        [Route("Updateuser")]
        //Get para obtener los usuarios activos en la BD
        public ActionResult Updateuser(string token, Usuario user)
        {
            try
            {

                GetTokenValues otoken = new GetTokenValues();

                if (otoken.validToken(token))
                {
                    var tokenV = otoken.getTokenValues(token);

                    if (tokenV.Role.ToLower() == "admin")
                    {
                        if (usercrud.updateUsuario(user))
                        {
                            return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.OK.ToString(),
                                code = HttpStatusCode.OK,
                                response = "Usuario modificado correctamente"
                            });
                        }
                        else
                        {
                            return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "El usuario a modificar no existe"
                            });
                        }

                    }
                    else
                    {
                        return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "Este usuario no tiene los permisos necesarios"
                            });
                    }
                }
                else
                {
                    return new JsonResult(
                            new
                            {
                                message = HttpStatusCode.BadRequest.ToString(),
                                code = HttpStatusCode.BadRequest,
                                response = "Token vencido"
                            });
                }






            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = msg
                });


            }

        }

        [HttpPost]
        [Route("AddUser")]
        //Get para obtener los usuarios activos en la BD
        public ActionResult AddUser(Usuario user)
        {
            try
            {
                usercrud.addusuario(user);


                return new JsonResult(
                new
                {
                    message = HttpStatusCode.OK.ToString(),
                    code = HttpStatusCode.OK,
                    response = usercrud.GetUsuarios().ToList()
                });


            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = msg
                });


            }

        }

        [HttpPost]
        [Route("login")]
        //Get para obtener los usuarios activos en la BD
        public ActionResult login(string mail)
        {
            try
            {
                var usuariosl = usercrud.GetUsuarios();
                if (usuariosl.Any(s => s.Email == mail))
                {
                    var user = usuariosl.Where(s => s.Email == mail).First();
                    CRUDUsuarios crud = new CRUDUsuarios();

                    var token = crud.createToken(_config,user.Nombre, user.PApellido, user.SApellido, user.Email, user.Rol);

                    return new JsonResult(
                           new
                           {
                               message = HttpStatusCode.OK.ToString(),
                               code = HttpStatusCode.OK,
                               response = new JwtSecurityTokenHandler().WriteToken(token)
                           });
                }
                else
                {
                    return new JsonResult(
                    new
                    {
                        message = HttpStatusCode.BadRequest.ToString(),
                        code = HttpStatusCode.BadRequest,
                        response = "El usuario "+mail+" no existe"
                    });
                }


               


            }
            catch (Exception ex)
            {

                string msg = ex.Message;
                return new JsonResult(
                new
                {
                    message = HttpStatusCode.BadRequest.ToString(),
                    code = HttpStatusCode.BadRequest,
                    response = msg
                });


            }

        }
    }
}
