# WebApi-Imaginemos

## Prueba Técnica para Desarrollador Backend .NET

## Desarrollador: Deyson Vente

# Descripción del Proyecto

El objetivo de esta prueba técnica es evaluar tus habilidades para desarrollar una API REST utilizando .NET con una estructura limpia (Clean Architecture), aplicando principios de SOLID y buenas prácticas de desarrollo. Además, se espera que implementes un CRUD completo con paginación y filtros, utilizando Entity Framework con una base de datos PostgreSQL sobre el tema de ventas en una pizzería.

---

## Requerimientos Técnicos

- Este proyecto es una API RESTful desarrollada en C#.
- Se usa una base de datos relacional en PostgreSQL
- Se usa la arquitectura limpia (Clean Architecture)
- Inyecciones de dependencias
- Trabajo con interfaces como servicios
- Se usan DTOs para el mapeo de objetos sin exponer las entidades o modelos principales de la aplicación en el API
- Separación de los proyecto en capas.

# Paso a paso e instrucciones

1. Clona el repositorio en tu máquina local, utilizando la siguiente dirección [Del Proyecto](https://github.com/Deyson19/WebApi-Imaginemos.git)
2. Establecer el string de conexión en el archivo **appsettings.json**, en la sección **`ConnectionString`**.
3. Ejecutar migraciones: Una vez que se haya establecido la conexión hacia su servidor, ejecutar los siguientes comandos en Visual Studio o usando el `dotnet cli`.
   - Usando Visual Studio: Ubicar el apartado _*Consola de Administrador de Paquetes*_, seleccionar en "proyecto predeterminado" el proyecto _WebApi-Imaginemos_ y escribir el siguiente comando: `update-database`
   - Usando Dotnet Cli: Ubicarse en la carpeta raíz del proyecto **WebApi-Imaginemos.DataAccess** y correr el comando: `dotnet ef database update`
   - En caso de errores: si al correr las migraciones se presenta algun error, es necesario verificar la versión del Entity Framework con la que se esté trabajando y que esté actualizada tanto la versión del runtime de `dotnet` como la del tools usada en el proyecto.
4. Ingresar los respectivos datos al servidor ya sea de forma masiva o de forma manual usando ya sea el api o accediendo a la base de datos.

## Nota importante

La base de datos utilizada es Postgres SQL y se asume que ya cuenta con una instancia configurada y corriendo correctamente

- > El servidor usado es local, por ende, se deben ingresar datos en la respectiva máquina del usuario final.
- > El orden recomendado para ingresar los registros es primeramente de la entidad productos y posteriormente las demás entidades mediante el uso del api y los respectivos endpoints.

## Entorno de Pruebas

La aplicación fue probada utilizando un entorno de pruebas como MSTest:

- Se hicieron las pruebas a los servicios mencionados en los requerimientos, en este caso la capa ``WebApi-Imaginemos.Services``
- Antes de tratar de ejecutar las pruebas, es necesario que la base de datos cuente con los registros en todas las entidades definidas en el dbcontext.

![Evidencia de pruebas a los servicios](/testing_image.png)
