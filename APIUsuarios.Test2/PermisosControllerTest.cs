using APIUsuarios.Controllers;
using APIUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIUsuarios.Test2
{
    public class PermisosControllerTest
    {
        [Fact]
        public async Task PostPermiso_AgregarPermiso_CuandoPermisoEsValido()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new PermisosController(context);
            var nuevoPermiso = new PermisoT { Nombre = "Permisos de creación", Descripcion = "" };

            //Act
            var result = await controller.PostPermisoT(nuevoPermiso);

            //Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var permiso = Assert.IsType<PermisoT>(createdResult.Value);
            Assert.Equal("Permisos de creación", permiso.Nombre);
    }

        [Fact]
        public async Task PostPermiso_NoAgregarPermiso_CuandoNombreEsNulo()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new PermisosController(context);
            var nuevoPermiso = new PermisoT { Nombre = "", Descripcion = "" };

            //Act
            var result = await controller.PostPermisoT(nuevoPermiso);

            //Assert            
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task PostPermiso_NoAgregarPermiso_CuandoLongitudEsMenor()
        {
            //Arrange
            var context = Setup.GetDatabaseContext();
            var controller = new PermisosController(context);
            var nuevoPermiso = new PermisoT { Nombre = "ab" };

            //Act
            var result = await controller.PostPermisoT(nuevoPermiso);

            //Assert            
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetPermiso_RetornarPermiso_CuandoIdEsValido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            var permiso = new PermisoT { Nombre = "Permiso de prueba", Descripcion = "Descripcion" };
            context.PermisosT.Add(permiso);
            await context.SaveChangesAsync();
            
            var controller = new PermisosController(context, null);

            // Act
            var result = await controller.GetPermisoT(permiso.PermisoId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PermisoT>>(result);
            var returnValue = Assert.IsType<PermisoT>(actionResult.Value);
            Assert.Equal("Permiso de prueba", returnValue.Nombre);
        }

        [Fact]
        public async Task GetPermiso_RetornarPermiso_CuandoIdNoExiste()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();            
            var controller = new PermisosController(context, null);

            // Act
            var result = await controller.GetPermisoT(999);

            // Assert            
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task EliminarPermiso_RetornaNoContent_CuandoIdEsValido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext();
            var permiso = new PermisoT { Nombre = "Permiso de prueba", Descripcion = "Descripcion" };
            
            context.PermisosT.Add(permiso);
            await context.SaveChangesAsync();
            
            var controller = new PermisosController(context, null);

            // Act
            var result = await controller.DeletePermisoT(permiso.PermisoId);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result); // Verifica que el resultado sea NoContent (204)
            
            var permisoEliminado = await context.PermisosT.FindAsync(permiso.PermisoId);
            Assert.Null(permisoEliminado);
        }

        [Fact]
        public async Task EliminarPermiso_RetornaNotFound_CuandoIdNoExiste()
        {
            // Arrange
            var context = Setup.GetDatabaseContext(); 
            
            var permisosIniciales = context.PermisosT.ToList();
            Assert.Empty(permisosIniciales); 
            
            var controller = new PermisosController(context, null);

            // Act
            var result = await controller.DeletePermisoT(999); 

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result); 
            
            var permisosDespues = context.PermisosT.ToList();
            Assert.Empty(permisosDespues);
        }

        [Fact]
        public async Task PutPermiso_ActualizarPermiso_CuandoIdEsValido()
        {
            // Arrange
            var context = Setup.GetDatabaseContext(); 
            
            var originalPermiso = new PermisoT { PermisoId = 1, Nombre = "Permiso Original", Descripcion = "Descripcion Original" };
            context.PermisosT.Add(originalPermiso);
            await context.SaveChangesAsync();
            
            var existingPermiso = await context.PermisosT.FindAsync(1);
            
            existingPermiso.Nombre = "Permiso Actualizado";
            existingPermiso.Descripcion = "Descripcion Actualizada";

            var controller = new PermisosController(context, null);

            // Act
            var result = await controller.PutPermisoT(1, existingPermiso);

            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result); 
            
            var permisoEnDb = await context.PermisosT.FindAsync(1);
            Assert.NotNull(permisoEnDb); 
            Assert.Equal("Permiso Actualizado", permisoEnDb.Nombre); 
            Assert.Equal("Descripcion Actualizada", permisoEnDb.Descripcion); 
        }

        [Fact]
        public async Task PutPermisoT_RetornaNotFound_CuandoPermisoNoExiste()
        {
            // Arrange
            var context = Setup.GetDatabaseContext(); 

            var updatedPermiso = new PermisoT { PermisoId = 999, Nombre = "Permiso Actualizado", Descripcion = "Descripcion Actualizada" };
            var controller = new PermisosController(context, null);

            // Act
            var result = await controller.PutPermisoT(999, updatedPermiso);

            // Assert
            var actionResult = Assert.IsType<NotFoundResult>(result); 
        }
    }
}
