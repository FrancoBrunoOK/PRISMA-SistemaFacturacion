# PRISMA

## Sistema de Gestión y Facturación

PRISMA es una aplicación web desarrollada como proyecto académico para la gestión de clientes, productos y facturación.

El sistema permite administrar la información necesaria para realizar un ciclo completo de facturación, desde el registro de clientes y productos hasta la generación, consulta y anulación de facturas.

La aplicación fue desarrollada utilizando ASP.NET Core MVC, Entity Framework Core y SQL Server, aplicando una arquitectura basada en el patrón Modelo-Vista-Controlador (MVC).

---

## Integrantes

- Bruno, Franco Nicolás
- Heredia, Nahuel Valentín
- Lobera, Pastorino Mateo
- Oviedo, Danilo
- Torres Oliva, Héctor Gabriel

---

## Objetivo del proyecto

El objetivo de PRISMA es desarrollar una aplicación web que permita gestionar de forma centralizada las principales operaciones relacionadas con un proceso de facturación.

El sistema permite:

- Administrar clientes.
- Administrar un catálogo de productos.
- Registrar precios e IVA de los productos.
- Generar facturas.
- Incorporar múltiples productos a una factura.
- Definir cantidades para cada producto.
- Calcular automáticamente subtotales e IVA.
- Calcular el total final de una factura.
- Registrar un método de pago.
- Incorporar notas u observaciones.
- Consultar el historial de facturas emitidas.
- Buscar y filtrar información.
- Anular facturas sin eliminarlas físicamente.
- Imprimir una factura o guardarla como PDF.
- Consultar indicadores generales desde un panel de control.

---

# Tecnologías utilizadas

## Backend

- C#
- .NET 10
- ASP.NET Core MVC
- Entity Framework Core 10

## Base de datos

- Microsoft SQL Server
- SQL Server Management Studio (SSMS)

## Frontend

- HTML
- CSS
- Razor Views
- Bootstrap
- JavaScript

## Herramientas de desarrollo

- Visual Studio 2026
- Git
- GitHub

---

# Arquitectura

PRISMA utiliza el patrón arquitectónico MVC:

**Model - View - Controller**

La aplicación puede representarse de forma simplificada de la siguiente manera:

```text
Usuario
   │
   ▼
Navegador
   │
   ▼
Views (Razor)
   │
   ▼
Controllers
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server
```

## Models

Los modelos representan las entidades y estructuras de datos utilizadas por el sistema.

Entre los principales modelos se encuentran:

- Cliente
- Producto
- Factura
- DetalleFactura
- NuevaFacturaViewModel
- DetalleFacturaViewModel
- DashboardViewModel

## Views

Las vistas representan la interfaz gráfica con la que interactúa el usuario.

Se encuentran organizadas principalmente en:

```text
Views/
├── Clientes/
├── Productos/
├── Facturas/
├── Home/
└── Shared/
```

Las vistas fueron desarrolladas utilizando Razor, HTML, Bootstrap y CSS.

## Controllers

Los controladores reciben las acciones realizadas por el usuario, procesan las solicitudes y se comunican con la capa de datos.

Los principales controladores son:

- HomeController
- ClientesController
- ProductosController
- FacturasController

## Entity Framework Core

Entity Framework Core se utiliza como ORM (Object-Relational Mapper).

Permite trabajar con la base de datos utilizando clases y objetos de C#, reduciendo la necesidad de escribir consultas SQL manualmente para las operaciones habituales.

También se utilizan migraciones para mantener y crear la estructura de la base de datos.

---

# Base de datos

La aplicación utiliza SQL Server.

La base de datos utilizada por el proyecto se denomina:

```text
FacturacionDB
```

Las tablas principales son:

```text
Clientes
Productos
Facturas
DetallesFactura
__EFMigrationsHistory
```

## Relaciones principales

El modelo general puede representarse de la siguiente manera:

```text
CLIENTES
    │
    │ 1
    │
    └────────── N
              FACTURAS
                  │
                  │ 1
                  │
                  └────────── N
                         DETALLES_FACTURA
                              │
                              │ N
                              │
                              └────────── 1
                                      PRODUCTOS
```

Un cliente puede tener múltiples facturas.

Una factura pertenece a un cliente.

Una factura puede contener múltiples detalles.

Cada detalle representa un producto incluido en una factura.

---

# Entidades principales

## Cliente

Contiene información relacionada con los clientes del sistema.

Entre sus datos se encuentran:

- Nombre
- CUIT
- DNI
- Dirección
- Localidad
- Provincia
- Código postal
- Teléfono
- Email
- Fecha de alta
- Estado

Los clientes pueden encontrarse activos o inactivos.

La baja se realiza de forma lógica, por lo que el registro permanece almacenado en la base de datos.

---

## Producto

Representa los productos disponibles para facturación.

Contiene:

- Código
- Descripción
- Precio
- IVA
- Estado
- Fecha de alta

Al igual que los clientes, los productos pueden darse de baja de forma lógica.

---

## Factura

Representa una factura emitida por el sistema.

Contiene información como:

- Número
- Cliente
- Fecha de emisión
- Subtotal
- IVA total
- Total
- Método de pago
- Nota
- Estado

Una factura puede encontrarse activa o anulada.

---

## DetalleFactura

Representa cada producto incluido dentro de una factura.

Almacena:

- Producto
- Descripción
- Cantidad
- Precio unitario
- IVA
- Subtotal
- Importe de IVA

Esto permite conservar la información correspondiente a cada línea de la factura.

---

# Módulos del sistema

## Dashboard

La pantalla principal funciona como panel de control.

Permite visualizar:

- Cantidad de clientes activos.
- Cantidad de productos activos.
- Cantidad de facturas activas.
- Cantidad de facturas anuladas.
- Total facturado correspondiente a facturas activas.

También proporciona accesos a los módulos principales.

---

## Clientes

El módulo permite:

- Crear clientes.
- Editar clientes.
- Consultar detalles.
- Buscar clientes.
- Filtrar por estado.
- Dar de baja clientes.
- Volver a dar de alta clientes.

La baja es lógica y no elimina físicamente el registro de la base de datos.

---

## Productos

El módulo permite:

- Crear productos.
- Editar productos.
- Consultar detalles.
- Buscar productos.
- Filtrar por estado.
- Definir precio.
- Definir porcentaje de IVA.
- Dar de baja productos.
- Volver a dar de alta productos.

---

## Facturación

El proceso de creación de una factura está dividido en cuatro etapas:

```text
1. Cliente
      ↓
2. Productos
      ↓
3. Resumen
      ↓
4. Factura
```

### Paso 1 - Cliente

Se selecciona el cliente al cual se emitirá la factura.

### Paso 2 - Productos

Se seleccionan uno o más productos.

Para cada producto se puede establecer una cantidad.

El sistema calcula los subtotales correspondientes.

### Paso 3 - Resumen

Antes de confirmar la operación se muestran:

- Datos del cliente.
- Productos seleccionados.
- Cantidades.
- Precios unitarios.
- IVA.
- Subtotal.
- Total de IVA.
- Total final.
- Método de pago.
- Nota opcional.

Los métodos de pago disponibles son:

- Efectivo
- Tarjeta de débito
- Tarjeta de crédito
- Pago con QR

### Paso 4 - Factura

Una vez confirmada la operación se genera el comprobante final.

La factura contiene:

- Número de factura.
- Fecha de emisión.
- Cliente.
- Productos.
- Cantidades.
- Precios.
- IVA.
- Método de pago.
- Nota.
- Subtotal.
- Total de IVA.
- Total final.

El comprobante puede imprimirse utilizando las funciones del navegador o guardarse como PDF.

---

# Historial de facturas

Las facturas generadas quedan almacenadas en la base de datos.

El historial permite:

- Consultar facturas.
- Buscar por número de factura.
- Buscar por cliente.
- Filtrar facturas activas.
- Filtrar facturas anuladas.
- Visualizar el comprobante.

---

# Anulación de facturas

Las facturas no se eliminan físicamente.

El sistema utiliza una anulación lógica mediante el campo de estado de la factura.

Esto permite conservar el historial de las operaciones realizadas.

Una factura anulada continúa disponible para consulta, pero queda identificada visualmente como:

```text
FACTURA ANULADA
```

---

# Conceptos aplicados

Durante el desarrollo de PRISMA se aplicaron diferentes conceptos de programación y desarrollo de software.

## Programación orientada a objetos

Se utilizan clases para representar las entidades principales del sistema.

Ejemplos:

```text
Cliente
Producto
Factura
DetalleFactura
```

Se utilizan propiedades, objetos, colecciones y relaciones entre entidades.

---

## Patrón MVC

La aplicación separa responsabilidades utilizando:

```text
Model
View
Controller
```

Esto permite mantener separada la representación de los datos, la interfaz y el procesamiento de las solicitudes.

---

## ORM

Se utiliza Entity Framework Core como Object-Relational Mapper.

Esto permite relacionar objetos de C# con tablas de SQL Server.

---

## Migraciones

Las migraciones de Entity Framework Core permiten crear y modificar progresivamente la estructura de la base de datos.

El proyecto incluye la carpeta:

```text
Migrations/
```

Por este motivo no es necesario crear manualmente todas las tablas al instalar el proyecto en otra computadora.

---

## Relaciones entre entidades

Se aplican relaciones uno a muchos.

Ejemplos:

```text
Cliente 1 ───── N Facturas

Factura 1 ───── N DetallesFactura

Producto 1 ──── N DetallesFactura
```

---

## ViewModels

Se utilizan ViewModels para representar información específica de determinadas vistas y procesos.

Por ejemplo:

```text
NuevaFacturaViewModel
DetalleFacturaViewModel
DashboardViewModel
```

Esto permite evitar que toda la lógica de una pantalla dependa directamente de una única entidad de la base de datos.

---

## Inyección de dependencias

ASP.NET Core utiliza inyección de dependencias para proporcionar el contexto de Entity Framework a los controladores.

El contexto principal es:

```text
ApplicationDbContext
```

---

## Operaciones CRUD

Los módulos de clientes y productos implementan operaciones relacionadas con:

```text
Create
Read
Update
Delete
```

En este proyecto, determinadas operaciones de eliminación fueron adaptadas para implementar bajas lógicas.

---

## Baja lógica

Los clientes y productos no necesitan eliminarse físicamente de la base de datos.

Se utiliza un estado:

```text
Activo = true
Activo = false
```

Esto permite conservar la información histórica.

Las facturas utilizan un mecanismo similar para controlar si se encuentran activas o anuladas.

---

## Sesiones

Durante el proceso de generación de una factura se utiliza Session para conservar temporalmente la información mientras el usuario avanza entre las diferentes etapas.

El proceso conserva información como:

- Cliente seleccionado.
- Productos agregados.
- Cantidades.
- Nota.
- Totales.

Al finalizar la factura, la información definitiva se almacena en SQL Server.

---

## Validación

Se utilizan validaciones tanto del lado de ASP.NET Core como controles HTML para evitar determinadas entradas inválidas.

Por ejemplo:

- Selección obligatoria de cliente.
- Selección obligatoria del método de pago.
- Cantidades de productos.
- Validaciones de formularios.

---

## Transacciones

Durante la confirmación de una factura se utiliza una transacción de base de datos.

Esto permite tratar la creación de la factura y sus detalles como una operación consistente.

Si ocurre un error durante el proceso, la transacción puede revertirse.

---

## Diseño responsive

La interfaz utiliza Bootstrap y CSS personalizado.

Esto permite adaptar las diferentes pantallas a distintos tamaños de pantalla.

---

# Estructura del proyecto

Una representación simplificada de la solución es:

```text
SistemaFacturacion/
│
├── Controllers/
│   ├── ClientesController.cs
│   ├── FacturasController.cs
│   ├── HomeController.cs
│   └── ProductosController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Migrations/
│
├── Models/
│   ├── Cliente.cs
│   ├── Producto.cs
│   ├── Factura.cs
│   ├── DetalleFactura.cs
│   ├── NuevaFacturaViewModel.cs
│   ├── DetalleFacturaViewModel.cs
│   └── DashboardViewModel.cs
│
├── Views/
│   ├── Clientes/
│   ├── Facturas/
│   ├── Home/
│   ├── Productos/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── lib/
│
├── Program.cs
├── appsettings.json
└── SistemaFacturacion.csproj
```

---

# Requisitos para ejecutar el proyecto

Antes de ejecutar PRISMA se recomienda tener instalado:

- Visual Studio 2026 o una versión compatible.
- .NET 10 SDK.
- SQL Server.
- SQL Server Management Studio.
- Git, si se desea clonar el repositorio.

En Visual Studio se debe contar con las herramientas necesarias para desarrollo ASP.NET y web.

---

# Instalación

## 1. Clonar el repositorio

Desde una terminal:

```bash
git clone https://github.com/FrancoBrunoOK/PRISMA-SistemaFacturacion.git
```

Ingresar a la carpeta:

```bash
cd PRISMA-SistemaFacturacion
```

También puede clonarse directamente utilizando Visual Studio.

---

## 2. Abrir la solución

Abrir el archivo de solución del proyecto con Visual Studio.

Por ejemplo:

```text
SistemaFacturacion.sln
```

---

## 3. Configurar SQL Server

Cada desarrollador puede utilizar su propia instancia local de SQL Server.

Crear un archivo:

```text
appsettings.Development.json
```

y configurar la conexión correspondiente.

Ejemplo utilizando SQL Server Express:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=FacturacionDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

El nombre de la instancia puede variar según la computadora.

Por ejemplo:

```text
.\SQLEXPRESS
NOMBRE-PC\SQLEXPRESS
NOMBRE-PC\SQLEXPRESS01
```

---

## 4. Restaurar dependencias

Visual Studio normalmente restaura automáticamente los paquetes NuGet.

También puede realizarse desde una terminal:

```bash
dotnet restore
```

---

## 5. Crear la base de datos

El proyecto utiliza migraciones de Entity Framework Core.

Desde Visual Studio abrir:

```text
Tools
→ NuGet Package Manager
→ Package Manager Console
```

Ejecutar:

```powershell
Update-Database
```

Entity Framework utilizará las migraciones existentes para crear la base de datos y sus tablas.

---

## 6. Ejecutar la aplicación

Desde Visual Studio iniciar el proyecto utilizando:

```text
F5
```

o:

```text
Ctrl + F5
```

También puede ejecutarse desde terminal:

```bash
dotnet run
```

La aplicación se abrirá en el navegador utilizando la dirección local configurada por ASP.NET Core.

---

# Trabajo colaborativo con Git

Para evitar modificar directamente la rama principal, cada integrante puede crear una rama para sus cambios.

Ejemplo:

```bash
git checkout -b feature/nombre-de-la-funcionalidad
```

Después de realizar cambios:

```bash
git add .
git commit -m "Descripcion del cambio"
git push -u origin feature/nombre-de-la-funcionalidad
```

Luego puede crearse un Pull Request en GitHub para integrar los cambios a la rama principal.

---

# Flujo general de uso

Una demostración completa del sistema puede realizarse siguiendo este orden:

```text
Dashboard
   ↓
Crear cliente
   ↓
Crear producto
   ↓
Nueva factura
   ↓
Seleccionar cliente
   ↓
Agregar productos
   ↓
Revisar resumen
   ↓
Seleccionar método de pago
   ↓
Confirmar factura
   ↓
Visualizar comprobante
   ↓
Imprimir / Guardar PDF
   ↓
Consultar historial
```

---

# Estado del proyecto

PRISMA cuenta actualmente con los módulos principales necesarios para realizar un ciclo completo de facturación académica:

- Gestión de clientes.
- Gestión de productos.
- Generación de facturas.
- Cálculo de IVA y totales.
- Métodos de pago.
- Historial.
- Búsquedas y filtros.
- Anulación de facturas.
- Dashboard.
- Impresión de comprobantes.

---

# Alcance

PRISMA fue desarrollado con fines académicos.

El sistema demuestra la aplicación de conceptos de desarrollo web, programación orientada a objetos, arquitectura MVC, persistencia de datos, bases de datos relacionales y control de versiones.

No está planteado como un sistema de facturación fiscal homologado ni como reemplazo de plataformas oficiales de emisión de comprobantes.

---

# Autores

**PRISMA - Sistema de Gestión y Facturación**

Proyecto desarrollado por:

- Bruno, Franco Nicolás
- Heredia, Nahuel Valentín
- Lobera, Pastorino Mateo
- Oviedo, Danilo
- Torres Oliva, Héctor Gabriel
