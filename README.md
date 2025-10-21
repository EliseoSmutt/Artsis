# 🎬 Artsis — Sistema administrativo del Cineclub Municipal de Córdoba

**Artsis** es una aplicación de escritorio desarrollada en **C# .NET MAUI** para la gestión integral del Cineclub Municipal de Córdoba.  
El sistema centraliza la administración de abonados, boletos vendidos, talleres y biblioteca, optimizando la organización interna y mejorando la trazabilidad de la información.

---

## 🧭 Propósito del sistema

El proyecto fue concebido como solución a las necesidades administrativas del Cineclub Municipal, donde la gestión manual de datos generaba duplicación de tareas y pérdida de tiempo.  
**Artsis** busca digitalizar esos procesos y ofrecer una interfaz moderna, intuitiva y eficiente.

---

## ⚙️ Funcionalidades principales

- 📋 **Gestión de abonados y boletos vendidos**  
- 🎨 **Administración de talleres y profesores (talleristas)**  
- 📚 **Control de biblioteca y alquiler de libros**  
- 📈 **Visualización de estadísticas mediante Google Charts**  
- 🧾 **Generación de reportes administrativos**

---

## 🏗️ Arquitectura y tecnologías

- **Lenguaje:** C#  
- **Framework:** .NET MAUI  
- **Patrón de diseño:** MVVM  
- **ORM:** Entity Framework Core  
- **Base de datos:** SQL Server Management Studio  
- **Arquitectura:** En capas (Domain, Repository, Data, Views, ViewModels)

### 📂 Sobre la capa de servicios
Durante la planificación inicial se contempló una capa de **Services** para centralizar la lógica de negocio.  
Sin embargo, por razones de **optimización del flujo de desarrollo y mantenimiento del cronograma académico**, parte de esa lógica fue integrada temporalmente en otras capas (principalmente Controller y Repository).  

> 🧠 Esta decisión fue **consciente y controlada**, buscando mantener el equilibrio entre una arquitectura limpia y los tiempos reales del proyecto.  
> De implementarse una nueva iteración profesional del sistema, la capa **Services** sería incorporada para una separación de responsabilidades más estricta.

---

## 🧰 Tecnologías y herramientas utilizadas

| Categoría | Herramienta |
|------------|-------------|
| Lenguaje principal | C# |
| Framework | .NET MAUI |
| ORM | Entity Framework Core |
| Base de datos | SQL Server Management Studio |
| Arquitectura | MVVM / En capas |
| Librerías adicionales | Google Charts |
| IDE | Visual Studio 2022 |

---

## 👥 Autor del proyecto

Proyecto desarrollado como trabajo final de la **Tecnicatura Superior en Desarrollo de Software** (Instituto Superior Santo Domingo).

- **Eliseo Smutt**  

> El código presentado corresponde a una versión previa a la entrega final, sin datos sensibles ni configuraciones privadas.

