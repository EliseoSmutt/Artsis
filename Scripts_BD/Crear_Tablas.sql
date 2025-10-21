CREATE TABLE Genero_Libros (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50)
);

CREATE TABLE Genero_Peliculas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50)
);

CREATE TABLE Personas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50),
    Apellido NVARCHAR(50),
    Email NVARCHAR(50),
    Telefono NVARCHAR(50),
    Localidad NVARCHAR(50),
    Direccion NVARCHAR(300)
);

CREATE TABLE Empleados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Persona_Id INT,
    Contraseña NVARCHAR(20),
    FOREIGN KEY (Persona_Id) REFERENCES Personas(Id)
);

CREATE TABLE Estudiantes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Persona_Id INT,
    FOREIGN KEY (Persona_Id) REFERENCES Personas(Id)
);

CREATE TABLE Talleristas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Persona_Id INT,
    FOREIGN KEY (Persona_Id) REFERENCES Personas(Id)
);

CREATE TABLE Abonados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Persona_Id INT,
    NroAbonado NVARCHAR(50),
    UltimoMesPagado NVARCHAR(20),
    FOREIGN KEY (Persona_Id) REFERENCES Personas(Id)
);

CREATE TABLE Espacios_Taller (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50),
    Capacidad INT
);

CREATE TABLE Talleres_Seminarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreTaller NVARCHAR(200),
    FechaInicio DATE,
    FechaFinal DATE,
    TalleristaId INT,
    Espacios_TallerId INT,
    FOREIGN KEY (TalleristaId) REFERENCES Talleristas(Id),
    FOREIGN KEY (Espacios_TallerId) REFERENCES Espacios_Taller(Id)
);

CREATE TABLE Inscripciones_Alumno (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AlumnoId INT,
    Talleres_SeminariosId INT,
    FOREIGN KEY (AlumnoId) REFERENCES Estudiantes(Id),
    FOREIGN KEY (Talleres_SeminariosId) REFERENCES Talleres_Seminarios(Id)
);

CREATE TABLE Notas_Estudiante_Taller (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EstudianteId INT,
    Talleres_SeminariosId INT,
    Nota NVARCHAR(20),
    FOREIGN KEY (EstudianteId) REFERENCES Estudiantes(Id),
    FOREIGN KEY (Talleres_SeminariosId) REFERENCES Talleres_Seminarios(Id)
);

CREATE TABLE Areas_Empleado (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(50)
);

CREATE TABLE Facturas (
    Id INT IDENTITY(1,1) PRIMARY KEY
);

CREATE TABLE MediosDePago (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(100)
);

CREATE TABLE ServiciosPrestados (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Descripcion NVARCHAR(100)
);

CREATE TABLE Libros (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50),
    Autor NVARCHAR(50),
    Genero_LibrosId INT,
    FOREIGN KEY (Genero_LibrosId) REFERENCES Genero_Libros(Id)
);

CREATE TABLE Peliculas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(100),
    Director NVARCHAR(100),
    Duracion INT,
    Genero_PeliculasId INT,
    FOREIGN KEY (Genero_PeliculasId) REFERENCES Genero_Peliculas(Id)
);

CREATE TABLE Salas_Funcion (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreSala NVARCHAR(50),
    Capacidad INT
);

CREATE TABLE Funciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PeliculaId INT,
    Salas_FuncionId INT,
    Dia NVARCHAR(20),
    Horario NVARCHAR(20),
    EntradasVendidas INT,
    FOREIGN KEY (PeliculaId) REFERENCES Peliculas(Id),
    FOREIGN KEY (Salas_FuncionId) REFERENCES Salas_Funcion(Id)
);

CREATE TABLE Reservas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AbonadoId INT,
    LibroId INT,
    FechaInicio DATE,
    FechaDevolucion DATE,
    Devuelto BIT,
    FOREIGN KEY (AbonadoId) REFERENCES Abonados(Id),
    FOREIGN KEY (LibroId) REFERENCES Libros(Id)
);
