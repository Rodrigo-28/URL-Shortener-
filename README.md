# 🔗 URL Shortener

Este es un proyecto backend desarrollado en **C# con ASP.NET Core 8**, que permite **acortar URLs largas** generando enlaces únicos y mucho más manejables. Está construido con una estructura **Clean Architecture**, incluye **caché en memoria**, y respeta principios sólidos de diseño y separación de responsabilidades.

---

## 🚀 ¿Por qué hice este proyecto?

En el mundo real, especialmente en aplicaciones de marketing, análisis de datos y redes sociales, **acortar URLs es esencial**:

- Permite **compartir enlaces limpios** y amigables.
- Hace posible el **seguimiento estadístico** mediante códigos únicos.
- Mejora la **estética y usabilidad** en interfaces web o móviles.

Este proyecto representa un caso real y útil, ideal para demostrar:

- Arquitectura profesional en capas.
- Lógica propia para generar códigos únicos.
- Uso práctico de herramientas del ecosistema .NET.

---

## 🏗️ Arquitectura: Clean Architecture

Elegí **Clean Architecture** para separar las responsabilidades y facilitar el mantenimiento, escalabilidad y testeo del proyecto. Las capas están distribuidas de la siguiente manera:

- **Domain**: Entidades y contratos (interfaces, configuraciones).
- **Application**: Reglas de negocio, DTOs, mapeos, servicios, caché.
- **Infrastructure**: Persistencia con EF Core (PostgreSQL o InMemory).
- **API**: Controladores, inyección de dependencias y configuración web.

Esta organización te permite:

- Reemplazar la base de datos sin afectar la lógica de negocio.
- Testear servicios sin depender del controlador o infraestructura.
- Aplicar principios SOLID y buenas prácticas de ingeniería.

---

## ⚙️ ¿Cómo funciona internamente?

### 🔁 Flujo POST para acortar una URL

1. El usuario envía una URL larga.
2. El sistema genera un **código único aleatorio** usando lógica propia.
3. Construye la `shortUrl` como `https://localhost:xxxx/{code}`.
4. Guarda el mapeo `longUrl <-> code` en la base de datos.
5. Invalida el caché interno si ya existía para ese código.
6. Devuelve al cliente la `shortUrl` junto con la original.

### ↺ Flujo GET para redirigir

1. El navegador accede a `/abc123`.
2. Se busca primero en **memoria caché** (`IMemoryCache`).
3. Si está en caché: redirige automáticamente.
4. Si no está: consulta la base de datos, guarda en caché y redirige.

---

## ⚡ Uso de caché interno en .NET

Este proyecto implementa `IMemoryCache` para almacenar temporalmente las URLs más utilizadas.\
Cada vez que una URL acortada es accedida, el sistema primero busca en memoria antes de ir a la base de datos.

### ✅ Beneficios de usar caché en memoria:

- 🔹 **Velocidad**: acceso a memoria es más rápido que a disco.
- 🔹 **Reducción de carga**: se minimizan llamadas a la base de datos.
- 🔹 **Eficiencia**: mejora el rendimiento bajo alto tráfico.
- 🔹 **Configuración simple**: se integra directamente en .NET con `AddMemoryCache()`.

### ↺ ¿Cómo funciona?

```csharp
if (!_cache.TryGetValue(cacheKey, out ShortenedUrlDto dto))
{
    // Cache MISS: consulta base de datos
    ...
    _cache.Set(cacheKey, dto, TimeSpan.FromSeconds(60));
}
```

---

## 🛠️ Lógica propia vs soluciones industriales

Para mostrar mis habilidades, desarrollé desde cero la lógica para:

- Generar códigos aleatorios únicos.
- Construir las URLs cortas.
- Manejar el caché manualmente.

Sin embargo, en la industria se suelen usar **librerías especializadas** y soluciones más robustas. Algunas alternativas reales son:

### 🔗 Librerías para acortar URLs

| Librería            | Descripción                                   | Link                                                       |
| ------------------- | --------------------------------------------- | ---------------------------------------------------------- |
| ShortLink Generator | Genera códigos únicos al estilo de Bitly.     | [NuGet](https://www.nuget.org/packages/ShortLinkGenerator) |
| Hashids.net         | Convierte números en códigos únicos (hashes). | [GitHub](https://github.com/ullmark/hashids.net)           |
| TinyURL API         | Acorta URLs usando API externa.               | [TinyURL](https://tinyurl.com/app/dev)                     |

---

## 🧠 ¿Y qué hay de soluciones de caché reales?

En entornos productivos, lo ideal es usar sistemas distribuidos de caché como:

### 🧱 Redis

- **Alto rendimiento**
- **Escalable y persistente**
- **Integración nativa con .NET** usando [StackExchange.Redis](https://github.com/StackExchange/StackExchange.Redis)

📚 Más info: [https://redis.io/docs](https://redis.io/docs)

---

## ✅ Tecnologías utilizadas

- [C# 8 / .NET 8](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [AutoMapper](https://automapper.org/)
- [IMemoryCache](https://learn.microsoft.com/en-us/dotnet/core/extensions/caching)
- [Clean Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)

---

## 🧪 ¿Qué aprendí al desarrollar esto?

- A separar claramente las capas de una aplicación moderna.
- A aplicar conceptos de diseño como SRP, inyección de dependencias y reutilización de servicios.
- A manejar el caché interno de .NET para optimizar rendimiento.
- A comunicar mis decisiones técnicas de forma clara y profesional.

---

## 🧠 Ideas para escalar este proyecto

- Usar **Redis** en lugar de `IMemoryCache`.
- Agregar **estadísticas de clics por URL**.
- Implementar **autenticación y gestión de usuarios**.
- Generar enlaces con fecha de expiración.
- Testear con xUnit + Moq.

---

## 🤝 Contacto

Este proyecto fue desarrollado con fines educativos y de portfolio.\
Estoy buscando oportunidades como Backend Developer.\
Si te interesa cómo trabajo o querés colaborar, no dudes en contactarme.

---

