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
    public class RolesControllerTest
    {
        [Fact]
        public async Task PostRol_AgregarRol_CuandoRolEsValido()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new RolesController(context);
            var nuevoRol = new RolT { Nombre = "Roles de creación", Descripcion = "" };

            //Act
            var result = await controller.PostRolT(nuevoRol);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var rol = Assert.IsType<RolT>(createdResult.Value);
            Assert.Equal("Roles de creación", rol.Nombre);
        }

        [Fact]
        public async Task PostRol_NoAgregarRol_CuandoNombreEsNulo()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new RolesController(context);
            var nuevoRol = new RolT { Nombre = "", Descripcion = "" };

            //Act
            var result = await controller.PostRolT(nuevoRol);

            //Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("El nombre es obligatorio y debe tener entre 3 y 30 caracteres.", badRequestResult.Value);
            //Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task GetRol_RetornarRol_CuandoLongitudEsMenor()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new RolesController(context);
            var nuevoRol = new RolT { Nombre = "admin" };

            //Act
            var result = await controller.PostRolT(nuevoRol);

            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public async Task GetRol_RetornarRol_CuandoIdEsValido()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var rol = new RolT { Nombre = "Rol de prueba", Descripcion = "Descripción" };
            context.RolesT.Add(rol);
            await context.SaveChangesAsync();

            var controller = new RolesController(context, null);

            //Act
            var result = await controller.GetRolT(rol.RolId);

            //Assert
            var actionResult = Assert.IsType<ActionResult<RolT>>(result);
            var returnValue = Assert.IsType<RolT>(actionResult.Value);
            Assert.Equal("Rol de prueba", returnValue.Nombre);
        }

        [Fact]
        public async Task GetRol_RetornarRol_CuandoIdNoExiste()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new RolesController(context, null);

            //Act
            var result = await controller.GetRolT(999);

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task EliminarRol_RetornarNoContent_CuandoIdEsValido()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var rol = new RolT { Nombre = "Rol de prueba", Descripcion = "Descripcion" };

            context.RolesT.Add(rol);
            await context.SaveChangesAsync();

            var controller = new RolesController(context);

            //Act
            var result = await controller.DeleteRolT(rol.RolId);

            //Assert
            var actionResult = Assert.IsType<NoContentResult>(result); // Verifica que el resultado sea NoContent (204)

            var rolEliminado = await context.RolesT.FindAsync(rol.RolId);
            Assert.Null(rolEliminado);
        }

        [Fact]
        public async Task EliminarRol_RetornarNotFound_CuandoIdNoExiste()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();

            var rolesIniciales = context.RolesT.ToList();
            Assert.Empty(rolesIniciales);

            var controller = new RolesController(context, null);

            //Act
            var result = await controller.DeleteRolT(999);

            //Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);

            var rolesDespues = context.RolesT.ToList();
            Assert.Empty(rolesDespues);
        }

        [Fact]
        public async Task PutRol_ActualizarRol_CuandoIdEsValido()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();

            var originalRol = new RolT { RolId = 1, Nombre = "Rol original", Descripcion = "Descripcion Original" };
            context.RolesT.Add(originalRol);
            await context.SaveChangesAsync();

            var existingRol = await context.RolesT.FindAsync(1);

            existingRol.Nombre = "Rol Actualizado";
            existingRol.Descripcion = "Descripcion Actualizada";

            var controller = new RolesController(context, null);

            //Act
            var result = await controller.PutRolT(1, existingRol);

            //Assert
            var actionResult = Assert.IsType<NoContentResult>(result);

            var rolEnDb = await context.RolesT.FindAsync(1);
            Assert.NotNull(rolEnDb);
            Assert.Equal("Rol Actualizado", rolEnDb.Nombre);
            Assert.Equal("Descripcion Actualizada", rolEnDb.Descripcion);
        }

        [Fact]
        public async Task PutRolT_RetornaNotFound_CuandoPermisoNoExiste()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();

            var updatedRol = new RolT { RolId = 999, Nombre = "Rol Actualizado", Descripcion = "Descripcion Actualizada" };
            var controller = new RolesController(context, null);

            //Act
            var result = await controller.PutRolT(999, updatedRol);

            //Assert
            var actionResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
