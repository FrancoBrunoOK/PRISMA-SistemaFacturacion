# PRISMA

## Sistema de Gestión y Facturación

PRISMA es una aplicación web desarrollada como proyecto académico para la gestión de clientes, productos y facturación.

El sistema permite administrar la información necesaria para realizar un ciclo completo de facturación, desde el registro de clientes y productos hasta la generación, consulta y anulación de facturas.

Desarrollada con **ASP.NET Core MVC**, **Entity Framework Core** y **SQL Server**, siguiendo el patrón **Modelo-Vista-Controlador (MVC)**.

---

## Integrantes

- Bruno, Franco Nicolás
- Heredia, Nahuel Valentín
- Lobera, Pastorino Mateo
- Oviedo, Danilo
- Torres Oliva, Héctor Gabriel

---

## Índice

- [Características](#características)
- [Tecnologías](#tecnologías)
- [Arquitectura](#arquitectura)
- [Base de datos](#base-de-datos)
- [Entidades principales](#entidades-principales)
- [Módulos del sistema](#módulos-del-sistema)
- [Conceptos aplicados](#conceptos-aplicados)
- [Estructura del proyecto](#estructura-del-proyecto)
- [Requisitos para ejecutar el proyecto](#requisitos-para-ejecutar-el-proyecto)
- [Instalación](#instalación)
- [Datos de prueba](#datos-de-prueba)
- [Trabajo colaborativo con Git](#trabajo-colaborativo-con-git)
- [Flujo general de uso](#flujo-general-de-uso)
- [Estado del proyecto](#estado-del-proyecto)

---

## Características

- Administración de clientes (alta, edición, baja lógica y reactivación)
- Catálogo de productos con precio e IVA
- Facturación en 4 pasos:
  1. Selección de cliente
  2. Agregado de productos y cantidades
  3. Resumen y método de pago
  4. Generación del comprobante
- Cálculo automático de subtotales, IVA y total
- Métodos de pago (Efectivo, Débito, Crédito, QR)
- Notas / observaciones en la factura
- Historial de facturas con búsqueda y filtros
- Anulación lógica de facturas (sin eliminación física)
- Dashboard con indicadores generales
- Impresión / guardado como PDF desde el navegador
- **Datos de prueba cargados automáticamente** al iniciar la aplicación (si la base está vacía)

---

## Tecnologías

| Capa            | Tecnología                              |
|-----------------|-----------------------------------------|
| Backend         | C# · .NET 10 · ASP.NET Core MVC         |
| ORM             | Entity Framework Core 10                |
| Base de datos   | Microsoft SQL Server                    |
| Frontend        | Razor Views · HTML · CSS · Bootstrap · JavaScript |
| Herramientas    | Visual Studio · Git · GitHub            |

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

Las vistas fueron desarrolladas utilizando Razor, HTML, Bootstrap y CSS. La plantilla común (menú, pie de página y scripts) se define en `Views/Shared/_Layout.cshtml`.

## Controllers

Los controladores reciben las acciones realizadas por el usuario, procesan las solicitudes y se comunican con la capa de datos.

Los principales controladores son:

- HomeController
- ClientesController
- ProductosController
- FacturasController

## Data

La carpeta `Data/` contiene:

- `ApplicationDbContext`: contexto de Entity Framework Core que representa la conexión con la base de datos y sus tablas.
- `DbInitializer`: carga datos de prueba al iniciar la aplicación cuando la base está vacía.

## Extensions

La carpeta `Extensions/` contiene `SessionExtensions`, métodos de extensión que permiten guardar y recuperar objetos completos en la sesión (serializándolos como JSON). Se utilizan durante el armado de una factura.


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
- IVA (21 % por defecto)
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

El número de factura se genera automáticamente a partir del identificador del registro, con 8 dígitos (por ejemplo, `00000042`).

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

Los datos del producto (descripción, precio e IVA) se copian al momento de facturar. Esto permite conservar la información correspondiente a cada línea de la factura aunque el producto se modifique más adelante.

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
- Buscar productos por código o descripción.
- Filtrar por estado (activos, inactivos o todos).
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

Mientras la factura se arma (pasos 1 a 3), los datos se conservan temporalmente en la sesión. Recién al confirmar (paso 4) se guardan en la base de datos.


### Paso 1 - Cliente

Se selecciona el cliente al cual se emitirá la factura. Solo se pueden elegir clientes activos.

### Paso 2 - Productos

Se seleccionan uno o más productos.

Para cada producto se puede establecer una cantidad. Si un producto se agrega más de una vez, las cantidades se acumulan en una única línea. 
También es posible quitar productos agregados.

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

La factura y sus detalles se guardan en una única transacción de base de datos.

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

Las facturas anuladas no se incluyen en el total facturado del dashboard.

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

Por este motivo no es necesario crear manualmente todas las tablas al instalar el proyecto en otra computadora: alcanza con ejecutar `Update-Database`.

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

Los objetos se guardan serializados como JSON mediante los métodos de `SessionExtensions`.

Al finalizar la factura, la información definitiva se almacena en SQL Server.

---

## Validación

Se utilizan validaciones tanto del lado del servidor (ASP.NET Core) como del navegador (HTML y jQuery Validation).

Por ejemplo:

- Campos obligatorios en los formularios de clientes y productos (validados en el navegador y nuevamente en el servidor con `ModelState`).
- Selección obligatoria de cliente (validada en el servidor).
- Selección obligatoria del método de pago (atributo `required` del formulario).
- Cantidades de productos.
- Tokens antiforgery en los formularios que envían datos.

---

## Transacciones

Durante la confirmación de una factura se utiliza una transacción de base de datos.

Esto permite tratar la creación de la factura, sus detalles y su número como una operación consistente.

Si ocurre un error durante el proceso, la transacción se revierte.

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
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs          # Seed de datos de prueba
│
├── Extensions/
│   └── SessionExtensions.cs      # Guardar/leer objetos en Session (JSON)
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
│   ├── DashboardViewModel.cs
│   └── ErrorViewModel.cs
│
├── Properties/
│   └── launchSettings.json       # Puertos y perfiles de ejecución
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
- SQL Server (por ejemplo, SQL Server Express).
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

Abrir el archivo de solución del proyecto con Visual Studio:

```text
SistemaFacturacion.slnx
```

El formato `.slnx` es el formato de solución más nuevo de Visual Studio. Si la versión instalada no puede abrirlo, 
el proyecto también puede abrirse directamente desde `SistemaFacturacion.csproj` o ejecutarse desde la terminal.

---

## 3. Configurar SQL Server

El proyecto ya incluye en `appsettings.json` una cadena de conexión que apunta a una instancia local de SQL Server Express:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=FacturacionDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Si la instancia de SQL Server de tu computadora tiene otro nombre, no hace falta modificar `appsettings.json`. Lo recomendable es crear un archivo propio:

```text
appsettings.Development.json
```

en la misma carpeta que `appsettings.json`, con la conexión correspondiente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=NOMBRE-PC\\SQLEXPRESS;Database=FacturacionDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Este archivo está incluido en `.gitignore`, por lo que cada integrante puede tener su propia configuración sin subirla al repositorio.

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

Este paso debe hacerse **antes** de ejecutar la aplicación por primera vez, ya que la carga de datos de prueba necesita que las tablas ya existan.

Alternativa desde terminal (requiere la herramienta `dotnet-ef`):

```bash
dotnet ef database update
```

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

También puede ejecutarse desde terminal, parado en la carpeta del proyecto:

```bash
dotnet run
```

La aplicación se abrirá en el navegador utilizando la dirección local configurada por ASP.NET Core.

---

# Datos de prueba

Al iniciar la aplicación, `Program.cs` ejecuta `DbInitializer`, que carga datos de ejemplo para poder probar el sistema sin tener que cargarlos a mano.

**La carga se realiza solo si las tablas `Clientes` y `Productos` están vacías.** Si ya existen datos, no se modifica nada, por lo que es seguro reiniciar la aplicación.

| Entidad   | Cantidad | Detalle |
|-----------|----------|---------|
| Clientes  | 10       | 9 activos y 1 inactivo (baja lógica de ejemplo). Incluye empresas (con CUIT) y personas (con DNI) de distintas provincias |
| Productos | 12       | 11 activos y 1 inactivo. Artículos de tecnología con IVA del 21 % |
| Facturas  | 5        | 4 activas y 1 anulada, con distintos métodos de pago y notas |

Con la base recién cargada, el dashboard debería mostrar:

| Indicador            | Valor esperado |
|----------------------|----------------|
| Clientes activos     | 9              |
| Productos activos    | 11             |
| Facturas activas     | 4              |
| Facturas anuladas    | 1              |
| Total facturado      | $ 2.550.377,50 |

Si ocurre un error durante la carga (por ejemplo, porque todavía no se ejecutó `Update-Database`), la aplicación no se detiene: el error queda registrado en el log con el mensaje "Error al cargar datos de prueba".

## Volver a cargar los datos de prueba

Para empezar de cero, eliminar la base y volver a crearla desde la Package Manager Console:

```powershell
Drop-Database
Update-Database
```

Al volver a ejecutar la aplicación, los datos de prueba se cargan nuevamente.

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
- Datos de prueba iniciales.

---

## Mejoras futuras

Estado de evolución del sistema:

- [x] **Usabilidad** → Paginado en listados
- [x] **Usabilidad** → Mejoras y ampliación de filtros
- [ ] **Seguridad** → Login con roles (Superusuario / Usuario)
- [ ] **Arquitectura** → Separar frontend con React + Expo
- [ ] **Pagos** → Integración de pasarela de pago real
- [ ] **Automatización** → Envío de factura por WhatsApp
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

---

# Licencia
Proyecto académico de uso libre con fines educativos.
