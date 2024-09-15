using APIUsuarios.Controllers;
using APIUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIUsuarios.Test2
{
    public class UsuariosControllerTest
    {
        [Fact]
        public async Task PostUsuario_AgregaUsuario_CuandoUsuarioEsValido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext(); 
            var controller = new UsuariosController(context); 

            var nuevoUsuario = new UsuarioT
            {
                Nombre = "Manuel Araniva",
                Email = "manuel.araniva@gmail.com",
                Contraseña = "Password123",
                RolId = 1 
            };

            // Act
            var result = await controller.PostUsuarioT(nuevoUsuario); 

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result); 
            var usuario = Assert.IsType<UsuarioT>(createdResult.Value); 
            Assert.Equal("Manuel Araniva", usuario.Nombre); 
            Assert.Equal("manuel.araniva@gmail.com", usuario.Email); 
            Assert.Equal("Password123", usuario.Contraseña); 
            Assert.Equal(1, usuario.RolId); 
        }

        [Fact]
        public async Task PostUsuario_NoAgregaUsuario_CuandoNombreEsInvalido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext(); 
            var controller = new UsuariosController(context); 

            var usuarioInvalido = new UsuarioT
            {
                Nombre = "Ma", 
                Email = "manuel.araniva@gmail.com",
                Contraseña = "Password123",
                RolId = 1 
            };

            // Act
            var result = await controller.PostUsuarioT(usuarioInvalido); 

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); 
            Assert.Equal(400, badRequestResult.StatusCode); 
            Assert.Equal("El nombre es obligatorio y debe tener entre 3 y 50 caracteres.", badRequestResult.Value); 
        }

        [Fact]
        public async Task PostUsuario_NoAgregarUsuario_CuandoEmailEsInvalido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext(); 
            var controller = new UsuariosController(context); 

            var usuarioInvalido = new UsuarioT
            {
                Nombre = "Manuel Araniva",
                Email = "manuel", 
                Contraseña = "Password123",
                RolId = 1 
            };

            // Act
            var result = await controller.PostUsuarioT(usuarioInvalido); 

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); 
            Assert.Equal(400, badRequestResult.StatusCode); 
            Assert.Equal("El email es obligatorio y debe tener un formato válido.", badRequestResult.Value); 
        }

        [Fact]
        public async Task PostUsuario_NoAgregarUsuario_CuandoContraseñaEsInvalida()
        {
            // Arrange
            var context = Setup.GetDatabaseContext(); 
            var controller = new UsuariosController(context); 

            var usuarioInvalido = new UsuarioT
            {
                Nombre = "Manuel Araniva",
                Email = "manuel.araniva@example.com",
                Contraseña = "12345", 
                RolId = 1 
            };

            // Act
            var result = await controller.PostUsuarioT(usuarioInvalido); 

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result); 
            Assert.Equal(400, badRequestResult.StatusCode); 
            Assert.Equal("La contraseña es obligatoria y debe tener al menos 8 caracteres.", badRequestResult.Value); 
        }

        [Fact]
        public async Task GetUsuario_RetornarUsuario_CuandoIdEsValido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            var usuario = new UsuarioT
            {
                UsuarioId = 1,
                Nombre = "Manuel Araniva",
                Email = "manuel.araniva@example.com",
                Contraseña = "Password123",
                RolId = 1
            };

            context.UsuariosT.Add(usuario);
            await context.SaveChangesAsync();

            var controller = new UsuariosController(context, null); 

            // Act
            var result = await controller.GetUsuarioT(usuario.UsuarioId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<UsuarioT>>(result);
            var returnValue = Assert.IsType<UsuarioT>(actionResult.Value);
            Assert.Equal("Manuel Araniva", returnValue.Nombre);
            Assert.Equal("manuel.araniva@example.com", returnValue.Email);
            Assert.Equal("Password123", returnValue.Contraseña);
            Assert.Equal(1, returnValue.RolId);
        }

        [Fact]
        public async Task GetUsuario_NoRetornarUsuario_CuandoIdEsInvalido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();            

            var controller = new UsuariosController(context, null); 

            // Act
            var result = await controller.GetUsuarioT(999); 

            // Assert
            var actionResult = Assert.IsType<ActionResult<UsuarioT>>(result);
            Assert.IsType<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task PutUsuario_ActualizaUsuario_CuandoDatosSonValidos()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            
            var originalUsuario = new UsuarioT
            {
                UsuarioId = 1,
                Nombre = "Manuel Original",
                Email = "manuel.original@gmail.com",
                Contraseña = "OriginalPassword123",
                RolId = 1
            };
            context.UsuariosT.Add(originalUsuario);
            await context.SaveChangesAsync();
            
            var existingUsuario = await context.UsuariosT.FindAsync(1);
            existingUsuario.Nombre = "Manuel Actualizado";
            existingUsuario.Email = "manuel.actualizado@gmail.com";
            existingUsuario.Contraseña = "NewPassword123";

            var controller = new UsuariosController(context, null);

            // Act
            var result = await controller.PutUsuarioT(1, existingUsuario);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            
            var usuarioEnDb = await context.UsuariosT.FindAsync(1);
            Assert.NotNull(usuarioEnDb);
            Assert.Equal("Manuel Actualizado", usuarioEnDb.Nombre);
            Assert.Equal("manuel.actualizado@gmail.com", usuarioEnDb.Email);
            Assert.Equal("NewPassword123", usuarioEnDb.Contraseña);
        }

        [Fact]
        public async Task EliminarUsuario_RetornaNoContent_CuandoIdEsValido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            var usuario = new UsuarioT
            {
                UsuarioId = 1, 
                Nombre = "Manuel Araniva",
                Email = "manuelaraniva@gmail.com",
                Contraseña = "Password123",
                RolId = 1
            };

            context.UsuariosT.Add(usuario);
            await context.SaveChangesAsync();

            var controller = new UsuariosController(context, null);

            // Act
            var result = await controller.DeleteUsuarioT(usuario.UsuarioId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result); 

            var usuarioEliminado = await context.UsuariosT.FindAsync(usuario.UsuarioId);
            Assert.Null(usuarioEliminado); 
        }

        [Fact]
        public async Task EliminarUsuario_RetornaNotFound_CuandoIdNoExiste()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            
            var usuariosIniciales = context.UsuariosT.ToList();
            Assert.Empty(usuariosIniciales);

            var controller = new UsuariosController(context, null);

            // Act
            var result = await controller.DeleteUsuarioT(999); 

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result); 

            var usuariosDespues = context.UsuariosT.ToList();
            Assert.Empty(usuariosDespues); 
        }
    }
}
