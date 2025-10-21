ALTER TABLE [dbo].[Empleados]
ADD Areas_EmpleadoId INT,
    FOREIGN KEY (Areas_EmpleadoId) REFERENCES Areas_Empleado(Id);
