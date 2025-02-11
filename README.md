# User Permission Api

# Installation

```bash
# CREATE PERMISSION TYPE TABLE
create table TipoPermisos(
id INT IDENTITY(1,1) PRIMARY KEY,
descripcion TEXT NOT NULL
)

# CREATE PERMISSION TABLE
create table Permisos(
id INT IDENTITY(1,1) PRIMARY KEY,
nombreEmpleado TEXT NOT NULL,
apellidoEmpleado TEXT NOT NULL,
tipoPermiso INT NOT NULL,
fechaPermiso DATE NOT NULL,
FOREIGN KEY (tipoPermiso) REFERENCES TipoPermisos(id)
)

# INSERT INITIAL DATA INTO PERMISSION TYPE TABLE
INSERT INTO TipoPermisos values ('test1')
INSERT INTO TipoPermisos values ('test2')

# INSERT INITIAL DATA INTO PERMISSION TABLE
INSERT INTO Permisos values ('Juan','Martinez', 1,'11/11/2025')
INSERT INTO Permisos values ('Ramon','Perez', 1,'11/11/2025')
INSERT INTO Permisos values ('Roberto','Gomez', 2,'11/11/2025')
